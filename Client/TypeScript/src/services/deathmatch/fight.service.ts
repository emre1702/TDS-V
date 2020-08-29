import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import { resetPedVisibleDamage, clearPedBloodDamage } from "natives";
import { Player } from "alt-client";
import SettingsService from "../settings/settings.service";
import BrowsersService from "../browsers/browsers.service";
import { onServer } from "alt-client";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";

@injectable()
export class FightService {
    private _inFight: boolean;
    get inFight(): boolean {
        return this._inFight;
    }
    
    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.BrowsersService) private browsersService: BrowsersService,
        @inject(DIIdentifier.FloatingDamageInfoService) private floatingDamageInfoService: FloatingDamageInfoService
    ) {
        eventsService.onRoundStarted.on(this.onRoundStarted.bind(this));

        eventsService.onRespawned.on(() => this.setInFight(true));

        eventsService.onLobbyLeft.on(() => this.setInFight(false));
        eventsService.onLocalPlayerDied.on(() => this.setInFight(false));
        eventsService.onMapChanged.on(() => this.setInFight(false));
        eventsService.onMapCleared.on(() => this.setInFight(false));
        eventsService.onRoundEnded.on(() => this.setInFight(false));

        onServer(ToClientEvent.HitOpponent, this.hittedOpponent.bind(this));
    }

    private setInFight(inFight: boolean) {
        if (this._inFight == inFight) {
            return;
        }
        this._inFight = inFight;

        resetPedVisibleDamage(Player.local.scriptID);
        clearPedBloodDamage(Player.local.scriptID);

        this.eventsService.onInFightStatusChanged.emit(inFight);
    }

    private hittedOpponent(hitted: Player, damage: number) {
        if (this.settingsService.playerSettings.Hitsound) {
            this.browsersService.plainMain.playHitsound();
        }
        if (this.settingsService.playerSettings.FloatingDamageInfo && hitted) {
            this.floatDamageInfoHandler.add(hitted, damage);
        }
    }

    private onRoundStarted(data: { isSpectator: boolean }) {
        this.setInFight(!data.isSpectator);
    }
}
