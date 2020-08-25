import DIIdentifier from "../../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../../events/events.service";
import { injectable, inject } from "inversify";
import alt from "alt-client";
import ToClientEvent from "../../../datas/enums/events/to-client-event.enum";
import LobbySettings from "../../../datas/interfaces/lobbies/lobby-settings.interface";
import LobbyType from "../../../datas/enums/lobbies/lobby-type.enum";
import { Blip } from "alt-client";
import GangHouseData from "../../../datas/interfaces/lobbies/gang/gang-house-data.model";
import { gangHouseFreeBlipModel, gangHouseFreeBlipAlpha } from "../../../datas/constants";
import SettingsService from "../../settings/settings.service";

@injectable()
export default class GangHouseService {
    private blip: Blip[] = [];

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService
    ) {
        eventsService.onLobbyLeft.on(this.onLobbyLeft.bind(this));

        //Todo: Add house blips on request at server
        alt.onServer(ToClientEvent.CreateFreeGangHousesForLevel, this.createFreeGangHousesForLevel.bind(this));
    }

    private createFreeGangHousesForLevel(gangHouseDatas: GangHouseData[]) {
        for (const blipData of gangHouseDatas) {
            const blip = new alt.PointBlip(blipData.position.x, blipData.position.y, blipData.position.z);
            blip.alpha = gangHouseFreeBlipAlpha;
            blip.shortRange = true;
            blip.sprite = gangHouseFreeBlipModel;
            blip.name = this.settingsService.language.GANG_LOBBY_FREE_HOUSE_DESCRIPTION.format(blipData.level);

            this.blip.push(blip);
        }
    }

    private onLobbyLeft(settings: LobbySettings) {
        if (settings.Type != LobbyType.GangLobby)
            return;

        for (const blip of this.blip) {
            blip.destroy();
        }
        this.blip.length = 0;
    }
}
