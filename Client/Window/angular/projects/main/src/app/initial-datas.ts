import { MapVoteDto } from './components/mapvoting/models/mapVoteDto';
import { MapDataDto } from './components/mapvoting/models/mapDataDto';
import { ConstantsData } from './interfaces/constants-data';
import { DamageTestWeapon } from './components/damage-test-menu/interfaces/damage-test-weapon';
import { WeaponHash } from './components/lobbychoice/enums/weapon-hash.enum';
import { UserpanelSettingsNormalType } from './components/userpanel/userpanel-settings-normal/enums/userpanel-settings-normal-type.enum';
import { UserpanelSettingsChat } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-chat';
import { UserpanelSettingsCooldownsAndDurations } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-cooldowns-and-durations';
import { UserpanelSettingsVoice } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-voice';
import { UserpanelSettingsFightEffect } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-fight-effect';
import { UserpanelSettingsGeneral } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-general';
import { UserpanelSettingsInfo } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-info';
import { UserpanelSettingsIngameColors } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-ingame-colors';
import { UserpanelSettingsKillInfo } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-kill-info';
import { UserpanelSettingsScoreboard } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-scoreboard';
import { UserpanelSettingsTheme } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-theme';
import { LanguageEnum } from './enums/language.enum';
import { ScoreboardPlayerSorting } from './components/userpanel/enums/scoreboard-player-sorting';
import { TimeSpanUnitsOfTime } from './components/userpanel/enums/timespan-units-of-time.enum';
import { ChallengeGroup } from './components/lobbychoice/models/challenge-group';
import { ChallengeFrequency } from './components/lobbychoice/enums/challenge-frequency.enum';
import { ChallengeType } from './components/lobbychoice/enums/challenge-type.enum';
import { UserpanelSettingCommandDataDto } from './components/userpanel/interfaces/settings-commands/userpanelSettingCommandDataDto';
import { UserpanelSettingsHud } from './components/userpanel/userpanel-settings-normal/interfaces/userpanel-settings-hud';
import { HudDesign } from './components/hud/enums/hud-design.enum';
import { SyncedSettings } from './interfaces/synced-settings';
import { VoiceInfo } from './components/hud/voice-info/models/voice-info';
import { RegisterLoginInitData } from './modules/register-login/models/register-login-init-data';
import { Announcement } from './components/lobbychoice/main-menu/models/announcement';
import { ChangelogsGroup } from './interfaces/changelogs/changelogs-group';
import { BodyData } from './modules/char-creator/body/models/body-data';
import { ClothesConfigs } from './modules/char-creator/clothes/models/clothes-configs';
import { ClothesDataKey } from './modules/char-creator/clothes/enums/clothes-config-key.enum';
import { MapCreateDataDto } from './components/mapcreator/models/mapCreateDataDto';
import { MapCreatorPosition } from './components/mapcreator/models/mapCreatorPosition';
import { MapCreatorPositionType } from './components/mapcreator/enums/mapcreatorpositiontype.enum';
import { MapCreateSettings } from './components/mapcreator/models/mapCreateSettings';

declare const mp: {};

export class InitialDatas {
    private static readonly longText = `asdjaois isodfaj oisdaji ofsadjio fjsadoi jfioasdjf iojsadhfui sadhoufi sadholiuf
        sadhoiu fhjsaodiuhfoiausdhofiusadh ioufsadhoiu shadoi fhasioudh foiasdh foiuasdhf iuosadhiu fhsadiuof dsaf`;

    static readonly inDebug = typeof mp === 'undefined';

    static readonly started: boolean = InitialDatas.inDebug && false;

    static readonly opened = {
        mapCreator: InitialDatas.inDebug && true,
        freeroam: InitialDatas.inDebug && false,
        lobbyChoice: !InitialDatas.inDebug || false,
        teamChoice: InitialDatas.inDebug && false,
        rankings: InitialDatas.inDebug && false,
        hud: InitialDatas.inDebug && false,
        charCreator: InitialDatas.inDebug && false,
        gangWindow: InitialDatas.inDebug && false,
        damageTestMenu: InitialDatas.inDebug && false,
        userpanel: InitialDatas.inDebug && false,
        registerLogin: !InitialDatas.inDebug || false,
    };

    static readonly adminLevel = 0;
    static readonly isMapVotingActive = false;
    static readonly language: LanguageEnum = LanguageEnum.English;
    static readonly isLobbyOwner: boolean = InitialDatas.inDebug && true;

    static readonly settingsByType: { [key: number]: {} } = {
        [UserpanelSettingsNormalType.Chat]: { 0: 30, 1: 35, 2: 1.4, 3: false, 4: true, 5: false, 6: 1, 7: 15000 } as UserpanelSettingsChat,
        [UserpanelSettingsNormalType.CooldownsAndDurations]: { 0: 150, 1: 100, 2: 100, 3: 25, 4: 10, 5: 1000 } as UserpanelSettingsCooldownsAndDurations,
        [UserpanelSettingsNormalType.FightEffect]: { 0: true, 1: true, 2: true } as UserpanelSettingsFightEffect,
        [UserpanelSettingsNormalType.General]: {
            0: LanguageEnum.English,
            1: false,
            2: true,
            3: 'UTC',
            4: "yyyy'-'MM'-'dd HH':'mm':'ss",
            6: true,
            '7': true,
        } as UserpanelSettingsGeneral,
        [UserpanelSettingsNormalType.Info]: { 0: true, 1: true } as UserpanelSettingsInfo,
        [UserpanelSettingsNormalType.IngameColors]: {
            0: 'rgba(150,0,0,0.35)',
            1: 'rgba(0, 0, 0, 1)',
            2: 'rgba(50, 0, 0, 1)',
            3: 'rgba(0, 255, 0, 1)',
            4: undefined,
            5: 'rgba(255, 255, 255, 1)',
        } as UserpanelSettingsIngameColors,
        [UserpanelSettingsNormalType.Hud]: { 0: HudDesign.BonusV1 } as UserpanelSettingsHud,
        [UserpanelSettingsNormalType.KillInfo]: { 0: true, 1: 1.4, 2: 60, 3: 15, 4: 10, 5: 30 } as UserpanelSettingsKillInfo,
        [UserpanelSettingsNormalType.Scoreboard]: {
            0: ScoreboardPlayerSorting.Name,
            1: false,
            2: TimeSpanUnitsOfTime.HourMinute,
        } as UserpanelSettingsScoreboard,
        [UserpanelSettingsNormalType.Theme]: {
            0: true,
            1: 'rgba(0,0,77,1)',
            2: 'rgba(255,152,0,1)',
            3: 'rgba(244,67,54,1)',
            4: 'linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)',
            5: 'rgba(250, 250, 250, 0.87)',
            6: 1,
        } as UserpanelSettingsTheme,
        [UserpanelSettingsNormalType.Voice]: { 0: false, 1: false, 2: 6 } as UserpanelSettingsVoice,
    };

    static readonly registerLoginInitData: RegisterLoginInitData = {
        0: false,
        1: 'Emre',
    };

    private static readonly testMapsInVoting: MapVoteDto[] = [
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
        { 0: 1, 1: '1231', 2: 2 },
    ];

    private static readonly testMapsForVoting: MapDataDto[] = [
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
        { 0: 1, 1: 'Bonus Test Map', 2: 0, 3: { 7: 'Hallo Deutsch', 9: 'Hello English' }, 4: 'Bonus', 5: 5 },
    ];

    private static readonly damageTestWeapons: DamageTestWeapon[] = [
        { 0: WeaponHash.Assaultrifle, 1: 100, 2: 2 },
        { 0: WeaponHash.Appistol, 1: 20, 2: 1 },
        { 0: WeaponHash.Assaultrifle, 1: 20, 2: 1 },
        { 0: WeaponHash.Assaultrifle_mk2, 1: 20, 2: 1 },
        { 0: WeaponHash.Assaultshotgun, 1: 20, 2: 1 },
        { 0: WeaponHash.Assaultsmg, 1: 20, 2: 1 },
        { 0: WeaponHash.Ball, 1: 2012312312, 2: 1132123 },
        { 0: WeaponHash.Bat, 1: 20, 2: 1 },
        { 0: WeaponHash.Battleaxe, 1: 20, 2: 1 },
        { 0: WeaponHash.Bottle, 1: 20, 2: 1 },
    ];

    private static readonly testSettingsConstants: ConstantsData = {
        0: 1,
        1: 1,
        2: 10000,
        3: 60,
        4: 12,
        5: 23,
        7: 'Bonus',
        8: 'Bonus',
    };

    private static readonly challengeGroups: ChallengeGroup[] = [
        [
            ChallengeFrequency.Weekly,
            [
                [ChallengeType.Assists, 5, 3],
                [ChallengeType.Kills, 5, 2],
                [ChallengeType.RoundPlayed, 1, 0],
            ],
        ],
        [
            ChallengeFrequency.Forever,
            [
                [ChallengeType.Assists, 5, 2],
                [ChallengeType.Kills, 5, 1],
                [ChallengeType.BeHelpfulEnough, 1, 0],
                [ChallengeType.ReadTheFAQ, 1, 0],
                [ChallengeType.ReadTheRules, 1, 0],
                [ChallengeType.ReviewMaps, 30, 10],
                [ChallengeType.ChangeSettings, 1, 0],
                [ChallengeType.CreatorOfAcceptedMap, 1, 0],
                [ChallengeType.ReviewMaps, 30, 10],
            ],
        ],
    ];

    private static readonly settingsCommandData: UserpanelSettingCommandDataDto = {
        0: [
            { 0: 1, 1: 'GoTo' },
            { 0: 2, 1: 'DoThis' },
            { 0: 2, 1: 'DoThat' },
        ],
        1: [{ 0: 1, 1: 1, 2: 'GehHin' }],
    };

    private static readonly mapCreateData: MapCreateDataDto = {
        0: 1,
        1: 'MapCenter',
        2: 0,
        3: { 0: 0, 1: 999 },
        4: { 7: 'English', 9: 'Deutsch' },
        5: [],
        6: [],
        7: [],
        8: [],
        9: new MapCreatorPosition(99, MapCreatorPositionType.MapCenter, 1213, 134, -31, 31, 555.51, 3, 0),
        10: new MapCreatorPosition(99, MapCreatorPositionType.Target, 1, 2, -3, 4, 5.51, 6, 0),
        11: [],
    };

    static readonly syncedSettings: SyncedSettings = {
        0: 30,
        1: 35,
        2: 1.4,
        3: false,
        4: false,
        5: 1,
        6: 15000,

        7: true,
        8: 1.4,
        9: 60,
        10: 15,
        11: 10,
        12: 30,

        13: true,
        14: 'rgba(0,0,77,1)',
        15: 'rgba(255,152,0,1)',
        16: 'rgba(244,67,54,1)',
        17: 'linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)',
        18: 'rgba(250, 250, 250, 0.87)',
        19: 1,
        20: HudDesign.BonusV1,
        21: undefined,
        22: undefined,
        23: undefined,
        24: undefined,
    };

    static readonly voiceInfos: VoiceInfo[] = [
        /*{ RemoteId: 1, Name: 'Emre1' },
        { RemoteId: 2, Name: 'Emre2' },
        { RemoteId: 3, Name: 'Emre3' },
        { RemoteId: 4, Name: 'Emre asdf sadfasdfsa' },
        { RemoteId: 5, Name: 'Emresaiodjfoisadfoiasdaf' },
        { RemoteId: 6, Name: 'Bonus' },
        { RemoteId: 7, Name: 'Emre1' },
        { RemoteId: 8, Name: 'Emre2' },
        { RemoteId: 9, Name: 'Emre3' },
        { RemoteId: 10, Name: 'Emre asdf sadfasdfsa' },
        { RemoteId: 11, Name: 'Emresaiodjfoisadfoiasdaf' },
        { RemoteId: 12, Name: 'Bonus' },
        { RemoteId: 13, Name: 'Emre1' },
        { RemoteId: 14, Name: 'Emre2' },
        { RemoteId: 15, Name: 'Emre3' },
        { RemoteId: 16, Name: 'Emre asdf sadfasdfsa' },
        { RemoteId: 17, Name: 'Emresaiodjfoisadfoiasdaf' },
        { RemoteId: 18, Name: 'Bonus' },*/
    ];

    static readonly announcements: Announcement[] = [[1, 'asd']];

    static readonly changelogs: ChangelogsGroup[] = [
        { 0: new Date(), 1: ['[NEW] Test 123 123', '[BUG] ' + InitialDatas.longText] },
        { 0: new Date(Date.parse('2000-1-4')), 1: ['[NEW] Test 123 123', '[BUG] ' + InitialDatas.longText] },
        { 0: new Date(Date.parse('2000-1-3')), 1: ['[NEW] Test 123 123', '[BUG] ' + InitialDatas.longText] },
        { 0: new Date(Date.parse('2000-1-2')), 1: ['[NEW] Test 123 123', '[BUG] ' + InitialDatas.longText] },
        { 0: new Date(Date.parse('2000-1-1')), 1: ['[NEW] Test 123 123', '[BUG] ' + InitialDatas.longText] },
        { 0: new Date(Date.parse('1995-2-17')), 1: ['[NEW] ASD ASD ASD', '[BUG] ' + InitialDatas.longText, '[CHANGE] ASDAsdi joidsajofsa'] },
    ];

    static readonly bodyData: BodyData = {
        [0]: [
            { [0]: true, [99]: 0 },
            { [0]: true, [99]: 1 },
            { [0]: true, [99]: 2 },
            { [0]: true, [99]: 3 },
        ],
        [1]: [
            {
                [0]: 0,
                [1]: 21,
                [2]: 0.5,
                [3]: 0.5,
                [99]: 0,
            },
            {
                [0]: 0,
                [1]: 21,
                [2]: 0.5,
                [3]: 0.5,
                [99]: 1,
            },
        ],
        [2]: [
            {
                [0]: 0,
                [1]: 0,
                [2]: 0,
                [3]: 0,
                [4]: 0,
                [5]: 0,
                [6]: 0,
                [7]: 0,
                [8]: 0,
                [9]: 0,
                [10]: 0,
                [11]: 0,
                [12]: 0,
                [13]: 0,
                [14]: 0,
                [15]: 0,
                [16]: 0,
                [17]: 0,
                [18]: 0,
                [19]: 0,
                [99]: 0,
            },
            {
                [0]: 0,
                [1]: 0,
                [2]: 0,
                [3]: 0,
                [4]: 0,
                [5]: 0,
                [6]: 0,
                [7]: 0,
                [8]: 0,
                [9]: 0,
                [10]: 0,
                [11]: 0,
                [12]: 0,
                [13]: 0,
                [14]: 0,
                [15]: 0,
                [16]: 0,
                [17]: 0,
                [18]: 0,
                [19]: 0,
                [99]: 1,
            },
        ],
        [3]: [
            {
                [0]: 0,
                [1]: 100,
                [2]: 0,
                [3]: 100,
                [4]: 0,
                [5]: 100,
                [6]: 0,
                [7]: 100,
                [8]: 0,
                [9]: 100,
                [10]: 0,
                [11]: 100,
                [12]: 0,
                [13]: 100,
                [14]: 0,
                [15]: 100,
                [16]: 0,
                [17]: 100,
                [18]: 0,
                [19]: 100,
                [20]: 0,
                [21]: 100,
                [22]: 0,
                [23]: 100,
                [24]: 0,
                [25]: 100,
                [99]: 0,
            },
            {
                [0]: 0,
                [1]: 100,
                [2]: 0,
                [3]: 100,
                [4]: 0,
                [5]: 100,
                [6]: 0,
                [7]: 100,
                [8]: 0,
                [9]: 100,
                [10]: 0,
                [11]: 100,
                [12]: 0,
                [13]: 100,
                [14]: 0,
                [15]: 100,
                [16]: 0,
                [17]: 100,
                [18]: 0,
                [19]: 100,
                [20]: 0,
                [21]: 100,
                [22]: 0,
                [23]: 100,
                [24]: 0,
                [25]: 100,
                [99]: 1,
            },
        ],
        [4]: [
            {
                [0]: 0,
                [1]: 0,
                [2]: 0,
                [3]: 0,
                [4]: 0,
                [5]: 0,
                [6]: 0,
                [7]: 0,
                [8]: 0,
                [99]: 0,
            },
            {
                [0]: 0,
                [1]: 0,
                [2]: 0,
                [3]: 0,
                [4]: 0,
                [5]: 0,
                [6]: 0,
                [7]: 0,
                [8]: 0,
                [99]: 1,
            },
        ],
        [99]: 0,
    };

    static readonly clothesData: ClothesConfigs = {
        0: [
            {
                [ClothesDataKey.Slot as number]: 0,
                [ClothesDataKey.Hats as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Glasses as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Masks as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Jackets as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shirts as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Hands as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Accessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bags as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Legs as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shoes as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.BodyArmors as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Decals as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.EarAccessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Watches as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bracelets as number]: { 0: -1, 1: -1 },
            },
            {
                [ClothesDataKey.Slot as number]: 1,
                [ClothesDataKey.Hats as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Glasses as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Masks as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Jackets as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shirts as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Hands as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Accessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bags as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Legs as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shoes as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.BodyArmors as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Decals as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.EarAccessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Watches as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bracelets as number]: { 0: -1, 1: -1 },
            },
            {
                [ClothesDataKey.Slot as number]: 2,
                [ClothesDataKey.Hats as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Glasses as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Masks as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Jackets as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shirts as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Hands as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Accessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bags as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Legs as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Shoes as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.BodyArmors as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Decals as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.EarAccessories as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Watches as number]: { 0: -1, 1: -1 },
                [ClothesDataKey.Bracelets as number]: { 0: -1, 1: -1 },
            },
        ],
        1: 0,
    };

    static getAdminLevel(): number {
        return this.inDebug ? this.adminLevel : 0;
    }

    static getMapsInVoting(): MapVoteDto[] {
        return this.inDebug ? this.testMapsInVoting : [];
    }

    static getMapsForVoting(): MapDataDto[] {
        return this.inDebug ? this.testMapsForVoting : [];
    }

    static getSettingsConstants(): ConstantsData | undefined {
        return this.inDebug ? this.testSettingsConstants : undefined;
    }

    static getDamageTestWeaponDatas(): DamageTestWeapon[] {
        return this.inDebug ? this.damageTestWeapons : [];
    }

    static getDamageTestInitialWeapon(): WeaponHash | undefined {
        return this.inDebug ? this.damageTestWeapons[0][0] : undefined;
    }

    static getChallengeGroups(): ChallengeGroup[] {
        return this.inDebug ? this.challengeGroups : [];
    }

    static getSettingsCommandData(): UserpanelSettingCommandDataDto {
        return this.inDebug ? this.settingsCommandData : undefined;
    }

    static getVoiceInfos(): VoiceInfo[] {
        return this.inDebug ? this.voiceInfos : [];
    }

    static getMapCreatorData(): MapCreateDataDto {
        return this.inDebug ? this.mapCreateData : new MapCreateDataDto();
    }
}
