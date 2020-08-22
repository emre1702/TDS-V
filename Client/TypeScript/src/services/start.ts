import { injectable } from "inversify";
import alt from "alt-client";
import game from "natives";
import PedStat from "../datas/enums/gta/ped-stat.enum";

@injectable()
export default class Start {

    constructor() {
        game.requestNamedPtfxAsset("scr_xs_celebration");
        game.setWeatherTypeNowPersist("CLEAR");
        game.setWind(0);
        game.requestAnimDict("MP_SUICIDE");
        game.setAudioFlag("LoadMPData", true);
        game.setPlayerHealthRechargeMultiplier(alt.Player.local.scriptID, 0);
        game.setPedCanRagdoll(alt.Player.local.scriptID, false);

        game.clearGpsCustomRoute();

        game.statSetInt(alt.hash(PedStat.Flying), 100, false);
        game.statSetInt(alt.hash(PedStat.Lung), 100, false);
        game.statSetInt(alt.hash(PedStat.Shooting), 100, false);
        game.statSetInt(alt.hash(PedStat.Stamina), 100, false);
        game.statSetInt(alt.hash(PedStat.Stealth), 100, false);
        game.statSetInt(alt.hash(PedStat.Strength), 100, false);
        game.statSetInt(alt.hash(PedStat.Wheelie), 100, false);
    }
}
