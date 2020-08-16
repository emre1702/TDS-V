import { injectable } from "inversify";
import * as alt from "alt-client";
import * as native from "natives";

@injectable()
export default class Start {

    constructor() {
        native.requestNamedPtfxAsset("scr_xs_celebration");
        native.setWeatherTypeNowPersist("CLEAR");
        native.setWind(0);
        native.requestAnimDict("MP_SUICIDE");
        native.setAudioFlag("LoadMPData", true);
        native.setPlayerHealthRechargeMultiplier(alt.Player.local.scriptID, 0);
        native.setPedCanRagdoll(alt.Player.local.scriptID, false);

        native.clearGpsCustomRoute();
    }
}
