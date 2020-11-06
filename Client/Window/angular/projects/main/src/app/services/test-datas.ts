import { MapVoteDto } from '../components/mapvoting/models/mapVoteDto';
import { MapDataDto } from '../components/mapvoting/models/mapDataDto';
import { ConstantsData } from '../interfaces/constants-data';
import { DamageTestWeapon } from '../components/damage-test-menu/interfaces/damage-test-weapon';
import { WeaponHash } from '../components/lobbychoice/enums/weapon-hash.enum';

export class InitialDatas {

    private static readonly longText = `asdjaois isodfaj oisdaji ofsadjio fjsadoi jfioasdjf iojsadhfui sadhoufi sadholiuf
        sadhoiu fhjsaodiuhfoiausdhofiusadh ioufsadhoiu shadoi fhasioudh foiasdh foiuasdhf iuosadhiu fhsadiuof dsaf`;

    private static readonly inDebug = true;

    static readonly started = InitialDatas.inDebug;
    static readonly isMapVotingActive = false;

    static readonly opened = {
        mapCreator: false,
        freeroam: false,
        lobbyChoice: false,
        teamChoice: false,
        rankings: false,
        hud: true,
        charCreator: false,
        gangWindow: false,
        damageTestMenu: false,
    };

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
