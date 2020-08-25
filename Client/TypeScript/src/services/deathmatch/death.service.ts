import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import alt from "alt-client";
import game from "natives";
import SettingsService from "../settings/settings.service";
import { screenFadeInTimeAfterSpawn, screenFadeOutTimeAfterSpawn } from "../../datas/constants";
import EffectName from "../../datas/enums/gta/effect-name.enum";
import CamEffect from "../../datas/enums/gta/cam-effect.enum";
import AudioName from "../../datas/enums/gta/audio-name.enum";
import AudioRef from "../../datas/enums/gta/audio-ref.enum";

@injectable()
export default class DeathService {

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService
    ) {
        

        eventsService.onSpawned.on(this.onSpawned.bind(this));
        eventsService.onLobbyJoined.on(this.onLobbyJoined.bind(this));
    }


    private onDeath() {
        game.doScreenFadeOut(screenFadeOutTimeAfterSpawn);
        game.ignoreNextRestart(true);
        game.setFadeOutAfterDeath(false);
        game.playSoundFrontend(-1, AudioName.Bed, AudioRef.WastedSounds, true);
        game.animpostfxPlay(EffectName.DeathFailMPIn, 0, true);
    }

    private onSpawned() {
        game.doScreenFadeIn(screenFadeInTimeAfterSpawn);
        game.animpostfxStop(EffectName.DeathFailMPIn);
        game.setCamEffect(CamEffect.Cancel);
    }
}
