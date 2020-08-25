import { injectable, inject } from "inversify";
import alt from "alt-client";
import ScaleformName from "../../datas/enums/gta/scaleform-name.enum";
import BasicScaleform from "../../entities/draw/basic-scaleform.entity";
import ScaleformFunction from "../../datas/enums/gta/scaleform-function.enum";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import SettingsService from "../settings/settings.service";
import EventsService from "../events/events.service";

@injectable()
export default class ScaleformMessagesService {

    private _scaleform: BasicScaleform;
    private get scaleform(): BasicScaleform {
        if (!this._scaleform) {
            this._scaleform = new BasicScaleform(ScaleformName.mp_big_message_freemode);
        }
        return this._scaleform;
    }
    private initTimeMs = 0;
    private animatedOut: boolean;
    private msgDurationMs: number;

    constructor(
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.EventsService) eventsService: EventsService
    ) {
        eventsService.onPlayerDied.on(this.onPlayerDied.bind(this));

        alt.everyTick(this.render.bind(this));
    }

    showPlaneMessage(title: string, planeName: string, planeHash: string, time: number = 5000) {
        this.scaleform.call(ScaleformFunction.SHOW_PLANE_MESSAGE, title, planeName, planeHash);
        this.initCommonSettings(time);
    }

    showShardMessage(title: string, message: string, titleColor: string, bgColor: number, time: number = 5000) {
        this.scaleform.call(ScaleformFunction.SHOW_SHARD_CENTERED_MP_MESSAGE, title, message, titleColor, bgColor);
        this.initCommonSettings(time);
    }

    showWastedMessage(time: number = 5000) {
        this.scaleform.call(ScaleformFunction.SHOW_SHARD_WASTED_MP_MESSAGE, "~r~Wasted", this.settingsService.language.YOU_DIED, 5, true, true);
        this.initCommonSettings(time);
    }

    showWeaponPurchasedMessage(title: string, weaponName: string, weaponHash: number, time: number = 5000) {
        this.scaleform.call(ScaleformFunction.SHOW_WEAPON_PURCHASED, title, weaponName, weaponHash);
        this.initCommonSettings(time);
    }

    private initCommonSettings(time: number) {
        this.initTimeMs = Date.now();
        this.msgDurationMs = time;
        this.animatedOut = false;
    }

    private render() {
        if (!this.scaleform)
            return;
        if (this.initTimeMs == 0)
            return;        

        this.scaleform.renderFullscreen();
        const currentMs = Date.now();
        if (currentMs - this.initTimeMs > this.msgDurationMs) {
            if (!this.animatedOut) {
                this.scaleform.call(ScaleformFunction.TRANSITION_OUT);
                this.animatedOut = true;
                this.msgDurationMs += 750;
            }
            else {
                this.initTimeMs = 0;
                this.scaleform.remove();
                this._scaleform = undefined;
            }
        }
    }

    private onPlayerDied(data: { player: alt.Player, teamIndex: number, willRespawn: boolean }) {
        if (data.player != alt.Player.local) {
            return;
        }

        this.showWastedMessage();
    }


    /*



        

        #endregion Public Methods

        #region Private Methods

        

        #endregion Private Methods*/
}
