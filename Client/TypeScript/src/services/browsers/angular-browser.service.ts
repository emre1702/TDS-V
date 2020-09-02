import EventsService from "../events/events.service";
import BrowserServiceBase from "./browser-service-base";
import { angularMainBrowserPath } from "../../datas/constants";
import { inject } from "inversify";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";
import FromBrowserEvent from "../../datas/enums/events/from-browser-event.enum";
import { hashPasswordClient } from "../../datas/utils";
import PlayerDataKey from "../../datas/enums/data/player-data-key.enum";
import { Player, onServer } from "alt-client";
import PlayerSettings from "../../datas/interfaces/players/player-settings.interface";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import MapCreatorObjectData from "../../datas/interfaces/lobbies/map-creator/map-creator-object-data.interface";
import { logError } from "../../datas/helper/logging.helper";
import HudDataType from "../../datas/enums/draw/hud-data-type.enum";


export default class AngularBrowserService extends BrowserServiceBase {

    private nameSynced = false;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService
    ) {
        super(angularMainBrowserPath);

        eventsService.onInFightStatusChanged.on((inFight) => this.execute(ToBrowserEvent.ToggleRoundStats, inFight));
        eventsService.onCountdownStarted.on(this.onCountdownStart.bind(this));
        eventsService.onLobbyLeft.on(this.onLobbyLeft.bind(this));
        eventsService.onRoundEnded.on((_) => this.execute(ToBrowserEvent.ResetMapVoting));
        eventsService.onAngularCooldown.on(() => this.execute(ToBrowserEvent.ShowCooldown));
        eventsService.onChatInputToggled.on((activated) => this.execute(ToBrowserEvent.ToggleChatOpened, activated));
        eventsService.onLanguageChanged.on(data => this.execute(ToBrowserEvent.LoadLanguage, data.language));
        eventsService.onDataChanged.on(this.onDataChanged.bind(this));

        onServer(ToClientEvent.ToBrowserEvent, this.execute.bind(this));
        onServer(ToClientEvent.FromBrowserEventReturn, this.execute.bind(this));
        onServer(ToClientEvent.JoinSameLobby, (player: Player) => this.execute(ToBrowserEvent.AddNameForChat, player.getName()));
        onServer(ToClientEvent.SyncSettings, this.loadChatSettings.bind(this));

        this.browser.on(FromBrowserEvent.GetHashedPassword, this.onGetHashedPassword.bind(this));
    }

    closeMapMenu() {
        this.execute(ToBrowserEvent.CloseMapMenu);
    }

    private onCountdownStart() {
        this.hideRankings();
    }

    private onLobbyLeft() {
        this.execute(ToBrowserEvent.ResetMapVoting);
        this.hideRankings();
    }

    private onGetHashedPassword(password: string) {
        const hashedPassword = hashPasswordClient(password);
        this.execute(FromBrowserEvent.GetHashedPassword as unknown as ToBrowserEvent, hashedPassword);
    }

    private hideRankings() {
        this.execute(ToBrowserEvent.HideRankings);
    }

    private loadChatSettings(settings: PlayerSettings) {
        this.execute(ToBrowserEvent.LoadChatSettings, settings.Chat)
    }

    //Todo make this private again?
    addMapCreatorObject(objectData: MapCreatorObjectData) {
        this.execute(ToBrowserEvent.AddMapCreatorObject, objectData);
    }

    private onDataChanged(data: { key: PlayerDataKey, value: any }) {
        try {
            switch (data.key) {
                case PlayerDataKey.Money:
                    this.execute(ToBrowserEvent.SyncMoney, data.value);
                    this.execute(ToBrowserEvent.SyncHudDataChange, HudDataType.Money, data.value);
                    break;

                case PlayerDataKey.AdminLevel:
                    this.execute(ToBrowserEvent.RefreshAdminLevel, data.value);
                    break;

                case PlayerDataKey.Name:
                    // Need this check else name will get synced after login twice
                    if (!this.nameSynced) {
                        this.nameSynced = true;
                        return;
                    }
                    this.execute(ToBrowserEvent.SyncUsernameChange, data.value);
                    break;

                case PlayerDataKey.GangId:
                    this.execute(ToBrowserEvent.SyncGangId, data.value);
                    break;
            }
        } catch (ex) {
            logError(ex, `Key: ${data.key} | Value: ${data.value}`);
        }
        
    }
}
