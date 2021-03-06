import { Injectable } from '@angular/core';
import { LanguageEnum } from '../enums/language.enum';
import { Language } from '../interfaces/language.interface';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../enums/from-client-event.enum';
import { EventEmitter } from 'events';
import { ConstantsData } from '../interfaces/constants-data';
import { ChallengeGroup } from '../components/lobbychoice/models/challenge-group';
import { ChallengeFrequency } from '../components/lobbychoice/enums/challenge-frequency.enum';
import { ChallengeType } from '../components/lobbychoice/enums/challenge-type.enum';
import { FromServerEvent } from '../enums/from-server-event.enum';
import { MapDataDto } from '../components/mapvoting/models/mapDataDto';
import { Challenge } from '../components/lobbychoice/models/challenge';
import { DomSanitizer } from '@angular/platform-browser';
import { ToServerEvent } from '../enums/to-server-event.enum';
import { UserpanelCommandDataDto } from '../components/userpanel/interfaces/userpanelCommandDataDto';
import { DamageTestWeapon } from '../components/damage-test-menu/interfaces/damage-test-weapon';
import { LanguagePipe } from '../modules/shared/pipes/language.pipe';
import { InitialDatas } from '../initial-datas';
import { SyncedSettings } from '../interfaces/synced-settings';
import { SettingsThemeIndex } from '../components/userpanel/userpanel-settings-normal/enums/settings-theme-index.enum';
import { Constants } from '../constants';
import { Announcement } from '../components/lobbychoice/main-menu/models/announcement';
import { ChangelogsGroup } from '../interfaces/changelogs/changelogs-group';

// tslint:disable: member-ordering

@Injectable({
    providedIn: 'root',
})
export class SettingsService {
    //////////////////// AdminLevel ////////////////////
    public AdminLevel = InitialDatas.getAdminLevel();
    public AdminLevelForApplicationInvites = 2;

    public AdminLevelChanged = new EventEmitter();

    public loadAdminLevel(adminLevel: number) {
        this.AdminLevel = adminLevel;
        this.AdminLevelChanged.emit(null);
    }

    ///////////////////// Language /////////////////////
    public LangValue: LanguageEnum = InitialDatas.language;
    public Lang: Language = Constants.LANGUAGE_BY_ENUM[this.LangValue];
    public LanguageChanged = new EventEmitter();
    private langPipe = new LanguagePipe();

    public loadLanguage(lang: number) {
        if (this.LangValue == lang) {
            return;
        }

        this.LangValue = lang;
        this.Lang = Constants.LANGUAGE_BY_ENUM[lang];
        this.loadChallenges();
        this.LanguageChanged.emit(null);
    }
    ////////////////////////////////////////////////////

    ////////////////// Map Favourites //////////////////
    public FavoriteMapIDs: number[] = [];
    public FavoriteMapsChanged = new EventEmitter();

    public toggleMapIdToFavorite(id: number) {
        const index = this.FavoriteMapIDs.indexOf(id);
        if (index >= 0) {
            this.FavoriteMapIDs[index] = undefined;
            this.rageConnector.callServer(ToServerEvent.ToggleMapFavouriteState, id, false);
        } else {
            this.FavoriteMapIDs.push(id);
            this.rageConnector.callServer(ToServerEvent.ToggleMapFavouriteState, id, true);
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
    public UserpanelOpen: boolean = InitialDatas.opened.userpanel;
    public UserpanelOpenChanged = new EventEmitter();

    public InFightLobby = false;
    public InFightLobbyChanged = new EventEmitter();

    public InUserLobbiesMenu = false;

    public MapsBoughtCounter = 0;
    public MapBuyStatsChanged = new EventEmitter();

    public Money = 0;
    public MoneyChanged = new EventEmitter();

    public IsLobbyOwner = InitialDatas.isLobbyOwner;
    public IsLobbyOwnerChanged = new EventEmitter();

    public Settings: SyncedSettings = InitialDatas.syncedSettings;
    public SettingsLoaded = new EventEmitter();
    public ChatSettingsChanged = new EventEmitter();
    public HudSettingsChanged = new EventEmitter();

    public Constants: ConstantsData = InitialDatas.getSettingsConstants();
    public ChallengeGroups: ChallengeGroup[] = InitialDatas.getChallengeGroups();
    public ChallengesLoaded = new EventEmitter();

    public CommandsData: UserpanelCommandDataDto[] = [];
    public CommandsDataLoaded = new EventEmitter();

    public ShownRoundStatsType = 1;
    public ShownHudType = 1;
    public AllMapsForCustomLobby: MapDataDto[] = [];

    public IsInGang = false;
    public IsInGangChanged = new EventEmitter();

    KillInfoSettingsChanged = new EventEmitter();

    public InputFocused = false;
    announcements: Announcement[];
    changelogs: ChangelogsGroup[];

    public DamageTestWeaponDatas: DamageTestWeapon[] = InitialDatas.getDamageTestWeaponDatas();

    public AdminLevels = [
        { Level: 0, Name: 'User', Color: 'rgb(220,220,220)' },
        { Level: 1, Name: 'Supporter', Color: 'rgb(113,202,113)' },
        { Level: 2, Name: 'Administrator', Color: 'rgb(253,132,85)' },
        { Level: 3, Name: 'Projectleader', Color: 'rgb(222,50,50)' },
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

    public setUserpanelOpen(bool: boolean) {
        this.UserpanelOpen = bool;
        this.UserpanelOpenChanged.emit(null);
    }

    public toggleInFightLobby(bool: boolean) {
        this.InFightLobby = bool;
        this.InFightLobbyChanged.emit(null);
    }

    private onChallengeCurrentAmountChange(frequency: ChallengeFrequency, type: ChallengeType, currentAmount: number) {
        this.ChallengeGroups.find((g) => g[0] == frequency)[1].find((c) => c[0] == type)[2] = currentAmount;
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

        this.setChallengeInfosToAll();
        this.ChallengesLoaded.emit(null);
    }

    private setChallengeInfosToAll() {
        if (this.ChallengeGroups) {
            for (const group of this.ChallengeGroups) {
                for (const challenge of group[1]) {
                    challenge[99] = this.getChallengeInfo(challenge);
                }
            }
        }
    }

    public syncGangId(gangId: number) {
        this.IsInGang = gangId > 0;
        this.IsInGangChanged.emit(null, this.IsInGang);
    }

    private loadSettings(settingsJson: string) {
        this.Settings = JSON.parse(settingsJson.escapeJson());
        this.SettingsLoaded.emit(null);
    }

    private getChallengeInfo(challenge: Challenge) {
        return this.sanitizer.bypassSecurityTrustHtml(
            this.langPipe.transform('Challenge_' + ChallengeType[challenge[0]], this.Lang, this.getColorText(challenge[1], 'orange')) +
                ' (' +
                this.langPipe.transform('Current:', this.Lang) +
                ' ' +
                this.getColorText(challenge[2] != undefined ? challenge[2] : 0, 'yellow') +
                ')'
        );
    }

    private syncCommandsData(dataJson: string) {
        this.CommandsData = JSON.parse(dataJson);
        this.CommandsDataLoaded.emit(null);
    }

    private getColorText(text: string | number, color: string) {
        return "<span style='color: " + color + "'>" + text + '</span>';
    }
    ////////////////////////////////////////////////////

    //////////////////// Theme ////////////////////////

    ThemeSettingChangedBefore = new EventEmitter();
    ThemeSettingChanged = new EventEmitter();
    ThemeSettingChangedAfter = new EventEmitter();

    setThemeChange(key: SettingsThemeIndex, value: any) {
        this.ThemeSettingChangedBefore.emit(null, key, value);
        this.ThemeSettingChanged.emit(null, key, value);
        this.ThemeSettingChangedAfter.emit(null, key, value);
    }

    ////////////////////////////////////////////////////

    constructor(private rageConnector: RageConnectorService, private sanitizer: DomSanitizer) {
        console.log('Settings listener started.');
        rageConnector.listen(FromClientEvent.LoadLanguage, this.loadLanguage.bind(this));
        rageConnector.listen(FromServerEvent.LoadMapFavourites, this.loadFavoriteMapIds.bind(this));
        rageConnector.listen(FromClientEvent.ToggleInFightLobby, this.toggleInFightLobby.bind(this));
        rageConnector.listen(FromClientEvent.ToggleTeamOrderModus, this.toggleInTeamOrderModus.bind(this));
        rageConnector.listen(FromClientEvent.ToggleChatOpened, this.setChatOpened.bind(this));
        rageConnector.listen(FromServerEvent.SyncChallengeCurrentAmountChange, this.onChallengeCurrentAmountChange.bind(this));
        rageConnector.listen(FromServerEvent.SyncChallenges, this.loadChallenges.bind(this));
        rageConnector.listen(FromClientEvent.SyncMapPriceData, this.syncMapPriceData.bind(this));
        rageConnector.listen(FromClientEvent.SyncMoney, this.onMoneySync.bind(this));
        rageConnector.listen(FromClientEvent.SyncIsLobbyOwner, this.onSyncIsLobbyOwner.bind(this));
        rageConnector.listen(FromClientEvent.LoadSettings, this.loadSettings.bind(this));
        rageConnector.listen(FromServerEvent.SyncCommandsData, this.syncCommandsData.bind(this));
        rageConnector.listen(FromClientEvent.SyncGangId, this.syncGangId.bind(this));

        this.setChallengeInfosToAll();

        this.LanguageChanged.setMaxListeners(100);
        this.SettingsLoaded.setMaxListeners(100);
        this.ThemeSettingChangedBefore.setMaxListeners(100);
        this.ThemeSettingChanged.setMaxListeners(100);
        this.ThemeSettingChangedAfter.setMaxListeners(100);
    }
}
