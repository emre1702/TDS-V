import Language from "../../datas/interfaces/language.interface";
import { injectable, inject } from "inversify";
import alt from "alt-client";
import LanguageValue from "../../datas/enums/output/language-value.enum";
import { languagesDict } from "../../datas/constants";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import RemoteEventsSender from "../events/remote-events-sender.service";
import ToServerEvent from "../../datas/enums/events/to-server-event.enum";
import PlayerSettings from "../../datas/interfaces/players/player-settings.interface";


@injectable()
export default class SettingsService {
    language: Language;
    playerSettings: PlayerSettings;

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
    

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService,
        @inject(DIIdentifier.RemoteEventsSender) private remoteEventsSender: RemoteEventsSender
    ) {
        this.loadGameLanguage();

        /*eventsHandler.LobbyJoined += LoadSyncedLobbySettings;
        modAPI.Event.Add(FromBrowserEvent.LanguageChange, OnLanguageChangeMethod);
        modAPI.Event.Add(FromBrowserEvent.OnColorSettingChange, OnColorSettingChangeMethod);
        modAPI.Event.Add(ToClientEvent.SyncSettings, OnSyncSettingsMethod);
        modAPI.Event.Add(FromBrowserEvent.SyncRegisterLoginLanguageTexts, SyncRegisterLoginLanguageTexts);
        modAPI.Event.Add(FromBrowserEvent.ReloadPlayerSettings, ReloadTempChangedPlayerSettings);
        modAPI.Event.Add(ToClientEvent.SyncPlayerCommandsSettings, LoadCommandsData);*/
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
