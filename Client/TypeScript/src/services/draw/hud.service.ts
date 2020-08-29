import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import WeaponHash from "../../datas/enums/gta/weapon-hash.enum";
import alt from "alt-client";
import game from "natives";
import SettingsService from "../settings/settings.service";
import BrowsersService from "../browsers/browsers.service";
import HudDataType from "../../datas/enums/draw/hud-data-type.enum";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";


@injectable()
export class HudService {
    private currentWeapon: WeaponHash;
    private currentArmor: number = -1;
    private currentHp: number = -1;

    //private lastBloodscreenUpdateMs: number = -1;
    private lastHudHealthUpdateMs: number = -1;
    private lastHudAmmoUpdateMs: number = -1;

    private lastArmorSentToHud: number = -1;
    private lastHpSentToHud: number = -1;
    private lastAmmoInClipSentToHud: number = -1;
    private lastAmmoTotalSentToHud: number = -1;


    private onTickId: number;
    

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.BrowsersService) private browsersService: BrowsersService
    ) {
        eventsService.onWeaponChanged.on(this.onWeaponChanged.bind(this));
        eventsService.onInFightStatusChanged.on(this.onInFightStatusChanged.bind(this));
    }

    private onWeaponChanged(data: { previous: WeaponHash, next: WeaponHash }) {
        this.currentWeapon = data.next;
    }

    private onInFightStatusChanged(inFight: boolean) {
        if (this.onTickId !== undefined) {
            alt.clearEveryTick(this.onTickId);
            this.onTickId = undefined;
        }
        if (inFight) {
            this.reset();
            this.onTickId = alt.everyTick(this.onTick.bind(this));
        }
    }

    private onTick() {
        const currentMs = Date.now();
        const healthLost = this.refreshAndGetHealthLost();

        if (healthLost) {
            if (healthLost > 0) {
                this.handleBloodscreen(currentMs)
            }
            this.handleHpArmorChange(currentMs);
        }

        this.handleAmmoChange(currentMs);
    }

    private refreshAndGetHealthLost() {
        const prevArmor = this.currentArmor;
        const prevHp = this.currentHp;
        this.currentArmor = alt.Player.local.getArmor();
        this.currentHp = alt.Player.local.getHp();

        return (prevArmor + prevHp) - (this.currentArmor + this.currentHp); 
    }

    //Todo Do it here or on damage?
    private handleBloodscreen(currentMs: number) {
        /*if (currentMs - this.lastBloodscreenUpdateMs >= this.settingsService.playerSettings.BloodscreenCooldownMs) {
            this.browsersService.plainMain.showBloodscreen();
            this.lastBloodscreenUpdateMs = currentMs;
        }*/
    }

    private handleHpArmorChange(currentMs: number) {
        if (currentMs - this.lastHudHealthUpdateMs < this.settingsService.playerSettings.HudHealthUpdateCooldownMs)
            return;

        // Update hp
        if (this.currentHp != this.lastHpSentToHud) {
            this.browsersService.angular.execute(ToBrowserEvent.SyncHudDataChange, HudDataType.HP, this.currentHp);
            this.lastHpSentToHud = this.currentHp;
        }
        // Update armor 
        if (this.currentArmor != this.lastArmorSentToHud) {
            this.browsersService.angular.execute(ToBrowserEvent.SyncHudDataChange, HudDataType.Armor, this.currentArmor);
            this.lastArmorSentToHud = this.currentArmor;
        }

        this.lastHudHealthUpdateMs = currentMs;
        
    }

    private handleAmmoChange(currentMs: number) {
        if (currentMs - this.lastHudAmmoUpdateMs < this.settingsService.playerSettings.HudAmmoUpdateCooldownMs)
            return;

        const [, ammoInClip] = game.getAmmoInClip(alt.Player.local.scriptID, this.currentWeapon, 0);
        if (ammoInClip != this.lastAmmoInClipSentToHud) {
            this.browsersService.angular.execute(ToBrowserEvent.SyncHudDataChange, HudDataType.AmmoInClip, ammoInClip);
            this.lastAmmoInClipSentToHud = ammoInClip;
        }

        const ammoTotal = game.getAmmoInPedWeapon(alt.Player.local.scriptID, this.currentWeapon);
        if (ammoTotal != this.lastAmmoTotalSentToHud) {
            this.browsersService.angular.execute(ToBrowserEvent.SyncHudDataChange, HudDataType.AmmoTotal, ammoTotal);
            this.lastAmmoTotalSentToHud = ammoTotal;
        }

        this.lastHudAmmoUpdateMs = currentMs;
    }


    private reset() {
        //this.lastBloodscreenUpdateMs = -1;
        this.lastHudHealthUpdateMs = -1;
        this.lastHudAmmoUpdateMs = -1;

        this.lastArmorSentToHud = -1;
        this.lastHpSentToHud = -1;
        this.lastAmmoInClipSentToHud = -1;
        this.lastAmmoTotalSentToHud = -1;

        this.currentHp = alt.Player.local.getHp();
        this.currentArmor = alt.Player.local.getArmor();
        this.onTick();
    }
}
