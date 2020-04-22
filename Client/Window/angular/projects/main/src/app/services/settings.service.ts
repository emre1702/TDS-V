import { Injectable } from '@angular/core';
import { LanguageEnum } from '../enums/language.enum';
import { German } from '../language/german.language';
import { English } from '../language/english.language';
import { Language } from '../interfaces/language.interface';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../enums/dfromclientevent.enum';
import { DToClientEvent } from '../enums/dtoclientevent.enum';
import { EventEmitter } from 'events';
import { ConstantsData } from '../interfaces/constants-data';
import { ChallengeGroup } from '../components/lobbychoice/models/challenge-group';
import { ChallengeFrequency } from '../components/lobbychoice/enums/challenge-frequency.enum';
import { ChallengeType } from '../components/lobbychoice/enums/challenge-type.enum';
import { DFromServerEvent } from '../enums/dfromserverevent.enum';
import { MapDataDto } from '../components/mapvoting/models/mapDataDto';
import { MapType } from '../enums/maptype.enum';
import { Challenge } from '../components/lobbychoice/models/challenge';
import { DomSanitizer } from '@angular/platform-browser';
import { LanguagePipe } from '../pipes/language.pipe';
import { DToServerEvent } from '../enums/dtoserverevent.enum';

// tslint:disable: member-ordering

@Injectable({
    providedIn: 'root'
})
export class SettingsService {

    //////////////////// AdminLevel ////////////////////
    public AdminLevel = 3;
    public AdminLevelForApplicationInvites = 2;

    public AdminLevelChanged = new EventEmitter();

    public loadAdminLevel(adminLevel: number) {
        this.AdminLevel = adminLevel;
        this.AdminLevelChanged.emit(null);
    }

    ///////////////////// Language /////////////////////
    public LangValue: LanguageEnum = LanguageEnum.English;
    public Lang: Language = SettingsService.langByLangValue[this.LangValue];
    public LanguageChanged = new EventEmitter();
    private langPipe = new LanguagePipe();

    private static langByLangValue = {
        [LanguageEnum.German]: new German(),
        [LanguageEnum.English]: new English()
    };

    public loadLanguage(lang: number) {
        if (this.LangValue == lang) {
            return;
        }

        this.LangValue = lang;
        this.Lang = SettingsService.langByLangValue[lang];
        this.LanguageChanged.emit(null);

        this.loadChallenges();
    }
    ////////////////////////////////////////////////////

    ////////////////// Map Favourites //////////////////
    public FavoriteMapIDs: number[] = [];
    public FavoriteMapsChanged = new EventEmitter();

    public toggleMapIdToFavorite(id: number) {
        const index = this.FavoriteMapIDs.indexOf(id);
        if (index >= 0) {
            this.FavoriteMapIDs[index] = undefined;
            this.rageConnector.callServer(DToServerEvent.ToggleMapFavouriteState, id, false);
        } else {
            this.FavoriteMapIDs.push(id);
            this.rageConnector.callServer(DToServerEvent.ToggleMapFavouriteState, id, true);
        }
    }

    public loadFavoriteMapIds(idsJson: string) {
        this.FavoriteMapIDs = JSON.parse(idsJson);
        this.FavoriteMapsChanged.emit(null);
    }

    public isInFavorites(mapId: number) {
        return this.FavoriteMapIDs.indexOf(mapId) >= 0;
    }
    ////////////////////////////////////////////////////

    /////////////////////// Rest ///////////////////////
    public InTeamOrderModus = false;
    public InTeamOrderModusChanged = new EventEmitter();

    public ChatOpened = false;
    public ChatOpenedChange = new EventEmitter();

    public ChatInputOpen = false;
    public UserpanelOpened = false;

    public InFightLobby = false;
    public InFightLobbyChanged = new EventEmitter();

    public InUserLobbiesMenu = false;

    public MapsBoughtCounter = 0;
    public MapBuyStatsChanged = new EventEmitter();

    public Money = 0;
    public MoneyChanged = new EventEmitter();

    public IsLobbyOwner = false;
    public IsLobbyOwnerChanged = new EventEmitter();

    public ChatWidth = "30vw";
    public ChatMaxHeight = "35vh";
    public ChatFontSize = "1.4em";
    public ChatHideDirtyChat = false;
    public ChatSettingsChanged = new EventEmitter();

    public Constants: ConstantsData;
    public ChallengeGroups: ChallengeGroup[] /* = [
        [ChallengeFrequency.Weekly, [
            [ ChallengeType.Assists, 5, 3 ],
            [ ChallengeType.Kills, 5, 2 ],
            [ ChallengeType.RoundPlayed, 1, 0 ],
        ]],

        [ChallengeFrequency.Forever, [
            [ ChallengeType.Assists, 5, 2 ],
            [ ChallengeType.Kills, 5, 1 ],
            [ ChallengeType.BeHelpfulEnough, 1, 0 ],
            [ ChallengeType.ReadTheFAQ, 1, 0 ],
            [ ChallengeType.ReadTheRules, 1, 0 ],
            [ ChallengeType.ReviewMaps, 30, 10 ],
            [ ChallengeType.ChangeSettings, 1, 0 ],
            [ ChallengeType.CreatorOfAcceptedMap, 1, 0 ],
            [ ChallengeType.ReviewMaps, 30, 10 ],
        ]],
    ]*/;
    public ChallengesLoaded = new EventEmitter();

    public ShownRoundStatsType = 1;
    public ShownHudType = 1;
    public AllMapsForCustomLobby: MapDataDto[] = [];

    public AdminLevels = [
        { Level: 0, Name: "User", Color: "rgb(220,220,220)" },
        { Level: 1, Name: "Supporter", Color: "rgb(113,202,113)" },
        { Level: 2, Name: "Administrator", Color: "rgb(253,132,85)" },
        { Level: 3, Name: "Projectleader", Color: "rgb(222,50,50)" }
    ];

    public toggleInTeamOrderModus(bool: boolean) {
        this.InTeamOrderModus = bool;
        this.InTeamOrderModusChanged.emit(null);
    }

    public setChatOpened(bool: boolean) {
        this.ChatOpened = bool;
        this.ChatOpenedChange.emit(null);
    }

    public setChatInputOpen(bool: boolean) {
        this.ChatInputOpen = bool;
    }

    public setUserpanelOpened(bool: boolean) {
        this.UserpanelOpened = bool;
    }

    public toggleInFightLobby(bool: boolean) {
        this.InFightLobby = bool;
        this.InFightLobbyChanged.emit(null);
    }

    private onChallengeCurrentAmountChange(frequency: ChallengeFrequency, type: ChallengeType, currentAmount: number) {
        this.ChallengeGroups.find(g => g[0] == frequency)[1].find(c => c[0] == type)[2] = currentAmount;
        this.ChallengesLoaded.emit(null);
    }

    private syncMapPriceData(mapsBoughtCounter: number) {
        this.MapsBoughtCounter = mapsBoughtCounter;
        this.MapBuyStatsChanged.emit(null);
    }

    private onMoneySync(money: number) {
        this.Money = money;
        this.MoneyChanged.emit(null);
    }

    private onSyncIsLobbyOwner(isLobbyOwner: boolean) {
        this.IsLobbyOwner = isLobbyOwner;
        this.IsLobbyOwnerChanged.emit(null);
    }

    public loadChallenges(challengesJson?: string) {
        if (challengesJson) {
            this.ChallengeGroups = JSON.parse(challengesJson);
        }

        if (this.ChallengeGroups) {
            for (const group of this.ChallengeGroups) {
                for (const challenge of group[1]) {
                    challenge[99] = this.getChallengeInfo(challenge);
                }
            }
        }

        this.ChallengesLoaded.emit(null);
    }

    public triggerChatSettingsChanged() {
        this.ChatSettingsChanged.emit(null);
    }

    private loadChatSettings(width: number, maxHeight: number, fontSize: number, hideDirtyChat: boolean) {
        this.ChatWidth = width + "vw";
        this.ChatMaxHeight = maxHeight + "vh";
        this.ChatFontSize = fontSize + "em";
        this.ChatHideDirtyChat = hideDirtyChat;
        this.triggerChatSettingsChanged();
    }

    private getChallengeInfo(challenge: Challenge) {
        return this.sanitizer.bypassSecurityTrustHtml(
            this.langPipe.transform('Challenge_' + ChallengeType[challenge[0]], this.Lang, this.getColorText(challenge[1], "orange"))
            + " (" + this.langPipe.transform("Current:", this.Lang)
            + " " + this.getColorText(challenge[2], "yellow") + ")");
    }

    private getColorText(text: string|number, color: string) {
        return "<span style='color: " + color + "'>" + text + "</span>";
    }
    ////////////////////////////////////////////////////

    constructor(
        private rageConnector: RageConnectorService,
        private sanitizer: DomSanitizer) {
        console.log("Settings listener started.");
        rageConnector.listen(DFromClientEvent.LoadLanguage, this.loadLanguage.bind(this));
        rageConnector.listen(DFromServerEvent.LoadMapFavourites, this.loadFavoriteMapIds.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleInFightLobby, this.toggleInFightLobby.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleTeamOrderModus, this.toggleInTeamOrderModus.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleChatOpened, this.setChatOpened.bind(this));
        rageConnector.listen(DFromServerEvent.SyncChallengeCurrentAmountChange, this.onChallengeCurrentAmountChange.bind(this));
        rageConnector.listen(DFromServerEvent.SyncChallenges, this.loadChallenges.bind(this));
        rageConnector.listen(DFromClientEvent.SyncMapPriceData, this.syncMapPriceData.bind(this));
        rageConnector.listen(DFromClientEvent.SyncMoney, this.onMoneySync.bind(this));
        rageConnector.listen(DFromClientEvent.SyncIsLobbyOwner, this.onSyncIsLobbyOwner.bind(this));
        rageConnector.listen(DFromClientEvent.LoadChatSettings, this.loadChatSettings.bind(this));

        this.LanguageChanged.setMaxListeners(9999);
    }
}
