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
import { German } from './language/german.language';
import { SettingsService } from './services/settings.service';
import { ScoreboardPlayerSorting } from './components/userpanel/enums/scoreboard-player-sorting';
import { TimeSpanUnitsOfTime } from './components/userpanel/enums/timespan-units-of-time.enum';
import { Constants } from './constants';

export class InitialDatas {

    private static readonly longText = `asdjaois isodfaj oisdaji ofsadjio fjsadoi jfioasdjf iojsadhfui sadhoufi sadholiuf
        sadhoiu fhjsaodiuhfoiausdhofiusadh ioufsadhoiu shadoi fhasioudh foiasdh foiuasdhf iuosadhiu fhsadiuof dsaf`;

    static readonly inDebug = false;

    static readonly started = InitialDatas.inDebug;
    static readonly isMapVotingActive = false;

    static readonly opened = {
        mapCreator: false,
        freeroam: false,
        lobbyChoice: false,
        teamChoice: false,
        rankings: false,
        hud: false,
        charCreator: false,
        gangWindow: false,
        damageTestMenu: false,
        userpanel: false
    };

    static settingsByType: { [key: number]: {} } = {
        [UserpanelSettingsNormalType.Chat]: { 0: 30, 1: 35, 2: 1.4, 3: false, 4: true, 5: false, 6: 1, 7: 15000 } as UserpanelSettingsChat,
        [UserpanelSettingsNormalType.CooldownsAndDurations]: { 0: 150, 1: 100, 2: 100, 3: 25, 4: 10, 5: 1000 } as UserpanelSettingsCooldownsAndDurations,
        [UserpanelSettingsNormalType.FightEffect]: { 0: true, 1: true, 2: true } as UserpanelSettingsFightEffect,
        [UserpanelSettingsNormalType.General]: { 
            0: LanguageEnum.English, 1: false, 2: true, 3: "UTC",
            4: "yyyy'-'MM'-'dd HH':'mm':'ss", 6: true, "7": true 
        } as UserpanelSettingsGeneral,
        [UserpanelSettingsNormalType.Info]: { 0: true, 1: true } as UserpanelSettingsInfo,
        [UserpanelSettingsNormalType.IngameColors]: { 
            0: "rgba(150,0,0,0.35)", 1: "rgba(0, 0, 0, 1)", 2: "rgba(50, 0, 0, 1)", 3: "rgba(0, 255, 0, 1)",
            4: undefined, 5: "rgba(255, 255, 255, 1)" 
        } as UserpanelSettingsIngameColors,
        [UserpanelSettingsNormalType.KillInfo]: { 0: true, 1: 1.4, 2: 60, 3: 15, 4: 10, 5: 30 } as UserpanelSettingsKillInfo,
        [UserpanelSettingsNormalType.Scoreboard]: { 
            0: ScoreboardPlayerSorting.Name, 1: false, 2: TimeSpanUnitsOfTime.HourMinute 
        } as UserpanelSettingsScoreboard,
        [UserpanelSettingsNormalType.Theme]: { 
            0: true, 1: "rgba(0,0,77,1)", 2: "rgba(255,152,0,1)", 3: "rgba(244,67,54,1)", 
            4: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)", 5: "rgba(250, 250, 250, 0.87)", 6: 1
        } as UserpanelSettingsTheme,
        [UserpanelSettingsNormalType.Voice]: { 0: false, 1: false, 2: 6 } as UserpanelSettingsVoice,
    }

    private static readonly testMapsInVoting: MapVoteDto[] = [
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
        { 0: 1, 1: "1231", 2: 2 },
    ];

    private static readonly testMapsForVoting: MapDataDto[] = [
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
        { 0: 1, 1: "Bonus Test Map", 2: 0, 3: { 7: "Hallo Deutsch", 9: "Hello English" }, 4: "Bonus", 5: 5 },
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
        0: 1, 1: 1, 2: 10000, 3: 60, 4: 12, 5: 23, 6: [[1, "asd"]], 7: "Bonus", 8: "Bonus",
        9: [
            { 0: new Date(), 1: ["[NEW] Test 123 123", "[BUG] " + InitialDatas.longText] },
            { 0: new Date(Date.parse("2000-1-4")), 1: ["[NEW] Test 123 123", "[BUG] " + InitialDatas.longText] },
            { 0: new Date(Date.parse("2000-1-3")), 1: ["[NEW] Test 123 123", "[BUG] " + InitialDatas.longText] },
            { 0: new Date(Date.parse("2000-1-2")), 1: ["[NEW] Test 123 123", "[BUG] " + InitialDatas.longText] },
            { 0: new Date(Date.parse("2000-1-1")), 1: ["[NEW] Test 123 123", "[BUG] " + InitialDatas.longText] },
            { 0: new Date(Date.parse("1995-2-17")), 1: ["[NEW] ASD ASD ASD", "[BUG] " + InitialDatas.longText, "[CHANGE] ASDAsdi joidsajofsa"] }
        ]
    };

    

    static getMapsInVoting(): MapVoteDto[] {
        return this.inDebug ? this.testMapsInVoting : [];
    }

    static getMapsForVoting(): MapDataDto[]  {
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
}
