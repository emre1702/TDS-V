import Language from "../../datas/interfaces/output/language.interface";
import { injectable, inject } from "inversify";
import alt from "alt-client";
import LanguageValue from "../../datas/enums/output/language-value.enum";
import { languagesDict } from "../../datas/constants";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import RemoteEventsSender from "../events/remote-events-sender.service";
import ToServerEvent from "../../datas/enums/events/to-server-event.enum";
import PlayerSettings from "../../datas/interfaces/players/player-settings.interface";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";
import FromBrowserEvent from "../../datas/enums/events/from-browser-event.enum";
import LobbySettings from "../../datas/interfaces/lobbies/lobby-settings.interface";
import { getRGBAFromString } from "../../datas/utils";
import BrowsersService from "../browsers/browsers.service";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";


@injectable()
export default class SettingsService {
    language: Language;
    playerSettings: PlayerSettings;
    lobbySettings: LobbySettings;

    private languageManuallyChanged = false;
    private _languageValue: LanguageValue;

    get languageValue(): LanguageValue {
        return this._languageValue;
    }
    set languageValue(value: LanguageValue) {
        this._languageValue = value;
        this.languageManuallyChanged = true;
        this.language = languagesDict[value];

        const beforeLogin = this.playerSettings === undefined;
        this.eventsService.onLanguageChanged.emit({ language: this.language, beforeLogin });

        if (!beforeLogin) {
            this.playerSettings.Language = value;
            this.remoteEventsSender.send(ToServerEvent.LanguageChange, value);
        }
    }

    // This is the old MapBorderColor if we changed the color in Angular and not saved it (for display)
    notTempMapBorderColor?: alt.RGBA;
    mapBorderColor: alt.RGBA;
    nametagColorsDict: { armorEmpty?: alt.RGBA, armorFull: alt.RGBA, dead?: alt.RGBA, healthEmpty: alt.RGBA, healthFull: alt.RGBA } = {
        armorFull: new alt.RGBA(255, 255, 255, 255),
        dead: new alt.RGBA(0, 0, 0, 255),
        healthEmpty: new alt.RGBA(50, 0, 0, 255),
        healthFull: new alt.RGBA(0, 255, 0, 255)
    };
    

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService,
        @inject(DIIdentifier.RemoteEventsSender) private remoteEventsSender: RemoteEventsSender,
        @inject(DIIdentifier.BrowsersService) private browserService: BrowsersService
    ) {
        this.loadGameLanguage();

        eventsService.onLobbyJoined.on(this.loadSyncedLobbySettings.bind(this));
        alt.on(FromBrowserEvent.LanguageChange, this.onBrowserRequestedLanguageChange.bind(this));
        alt.on(FromBrowserEvent.OnColorSettingChange, this.onColorSettingChange.bind(this));
        alt.onServer(ToClientEvent.SyncSettings, this.onSyncPlayerSettings.bind(this));
        /*alt.on(FromBrowserEvent.SyncRegisterLoginLanguageTexts, this.syncRegisterLoginLanguageTexts.bind(this));
        alt.on(FromBrowserEvent.ReloadPlayerSettings, this.reloadTempChangedPlayerSettings.bind(this));
        alt.onServer(ToClientEvent.SyncPlayerCommandsSettings, this.loadCommandsData.bind(this));*/
    }

    private loadSyncedLobbySettings(lobbySettings: LobbySettings) {
        this.lobbySettings = lobbySettings;
    }

    private onBrowserRequestedLanguageChange(languageValue: LanguageValue) {
        this.languageValue = languageValue;
    }

    private onColorSettingChange(color: string, dataSetting: UserpanelSettingKey) {

        switch (dataSetting) {
            case UserpanelSettingKey.MapBorderColor:
                this.mapBorderColor = getRGBAFromString(color) || this.mapBorderColor;
                this.eventsService.onMapBorderColorChanged.emit(this.mapBorderColor);
                break;

            case UserpanelSettingKey.NametagDeadColor:
                this.nametagColorsDict.dead = getRGBAFromString(color);
                break;

            case UserpanelSettingKey.NametagHealthEmptyColor:
                this.nametagColorsDict.healthEmpty = getRGBAFromString(color) || this.nametagColorsDict.healthEmpty;
                break;

            case UserpanelSettingKey.NametagHealthFullColor:
                this.nametagColorsDict.healthFull = getRGBAFromString(color) || this.nametagColorsDict.healthFull;
                break;

            case UserpanelSettingKey.NametagArmorEmptyColor:
                this.nametagColorsDict.armorEmpty = getRGBAFromString(color);
                break;

            case UserpanelSettingKey.NametagArmorFullColor:
                this.nametagColorsDict.armorFull = getRGBAFromString(color) || this.nametagColorsDict.armorFull;
                break;
        }
    }

    private onSyncPlayerSettings(playerSettings: PlayerSettings) {
        this.loadPlayerSettings(playerSettings);
        this.browserService.angular.execute(ToBrowserEvent.LoadUserpanelData, UserpanelLoadDataType.SettingsNormal, playerSettings);
    }

    private loadPlayerSettings(playerSettings: PlayerSettings) {
        if (!this.languageManuallyChanged || this.languageValue == playerSettings.Language || this.playerSettings != null) {
            this.languageValue = playerSettings.Language;
        } else {
            playerSettings.Language = this.languageValue;
            this.remoteEventsSender.send(ToServerEvent.LanguageChange, playerSettings.Language);
        }

        this.languageManuallyChanged = false;
        this.playerSettings = playerSettings;

        this.mapBorderColor = getRGBAFromString(playerSettings.MapBorderColor) || this.mapBorderColor;
        this.nametagColorsDict = {
            dead: getRGBAFromString(playerSettings.NametagDeadColor),
            healthEmpty: getRGBAFromString(playerSettings.NametagHealthEmptyColor) || this.nametagColorsDict.healthEmpty,
            healthFull: getRGBAFromString(playerSettings.NametagHealthFullColor) || this.nametagColorsDict.healthFull,
            armorEmpty: getRGBAFromString(playerSettings.NametagArmorEmptyColor),
            armorFull: getRGBAFromString(playerSettings.NametagArmorFullColor) || this.nametagColorsDict.armorFull
        }

        this.notTempMapBorderColor = null;

        this.eventsService.onMapBorderColorChanged.emit(this.mapBorderColor);
        this.eventsService.onSettingsLoaded.emit(playerSettings);
    }

    private loadGameLanguage() {
        switch (alt.getLocale()) {
            case "de":
                this.languageValue = LanguageValue.German;
                break;
            default:
                this.languageValue = LanguageValue.English;
                break;
        }
        this.languageManuallyChanged = false;
    }
}
