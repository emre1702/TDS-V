import { injectable, inject } from "inversify";
import { Player } from "alt-client";
import { FloatingDamageInfo } from "../../entities/draw/floating-damage-info.entity";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import SettingsService from "../settings/settings.service";
import DxService from "./dx.service";
import EventsService from "../events/events.service";
import { clearEveryTick } from "alt-client";
import { everyTick } from "alt-client";

@injectable()
export class FloatingDamageInfoService {

    private damageInfos: FloatingDamageInfo[] = [];

    private updateAllPositionTickId: number;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.DxService) private dxService: DxService
    ) {
        eventsService.onInFightStatusChanged.on(this.onInFightStatusChanged.bind(this));
    }

    add(target: Player, damage: number) {
        const info = new FloatingDamageInfo(target, damage, this.settingsService, this.dxService);
        this.damageInfos.push(info);
    }

    private clear() {
        for (const info of this.damageInfos) {
            info.remove();
        }
        this.damageInfos.length = 0;
    }

    private updateAllPosition() {
        const currentMs = Date.now();

        for (let i = this.damageInfos.length - 1; i >= 0; --i) {
            const damageInfo = this.damageInfos[i];
            if (damageInfo.removeAtHandler)
                this.damageInfos.splice(i, 1);
            else
                damageInfo.updatePosition(currentMs);
        }
    }

    private onInFightStatusChanged(inFight: boolean) {
        if (this.updateAllPositionTickId !== undefined) {
            clearEveryTick(this.updateAllPositionTickId);
            this.updateAllPositionTickId = undefined;
        }

        if (inFight) {
            this.updateAllPositionTickId = everyTick(this.updateAllPosition);
        } else {
            this.clear();
        }
    }

}
