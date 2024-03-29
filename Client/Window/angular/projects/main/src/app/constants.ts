import { MapType } from './enums/maptype.enum';
import { BlipColor } from './components/lobbychoice/models/blip-color';
import { LanguageEnum } from './enums/language.enum';
import { German } from './language/german.language';
import { English } from './language/english.language';
import { Language } from './interfaces/language.interface';

export class Constants {
    public static MIN_MAP_CREATE_NAME_LENGTH = 3;
    public static MAX_MAP_CREATE_NAME_LENGTH = 100;

    public static MAX_POSSIBLE_ARMOR = 16959;
    public static MAX_POSSIBLE_HEALTH = 16959;

    public static MIN_TEAMS_PER_TYPE = {
        [MapType.Normal]: 2,
        [MapType.Bomb]: 2,
        [MapType.Sniper]: 2,
        [MapType.Gangwar]: 2,
        [MapType.ArmsRace]: 1,
    };

    public static BLIP_COLORS: BlipColor[] = [
        { ID: 1, Color: 'rgb(225, 59, 59)', Info: 'HeightArrowDoesntBlink' },
        { ID: 2, Color: 'rgb(121, 206, 121)', Info: 'HeightArrowDoesntBlink' },
        { ID: 3, Color: 'rgb(101, 185, 231)', Info: 'HeightArrowDoesntBlink' },
        { ID: 4, Color: 'rgb(241, 241, 241)' },
        { ID: 5, Color: 'rgb(240, 203, 88)', Info: 'HeightArrowDoesntBlink' },
        { ID: 6, Color: 'rgb(198, 88, 89)' },
        { ID: 7, Color: 'rgb(161, 117, 180)' },
        { ID: 8, Color: 'rgb(255, 129, 200)' },
        { ID: 9, Color: 'rgb(247, 165, 128)' },
        { ID: 10, Color: 'rgb(182, 150, 139)' },
        { ID: 11, Color: 'rgb(146, 208, 171)' },
        { ID: 12, Color: 'rgb(120, 173, 179)' },
        { ID: 13, Color: 'rgb(213, 211, 232)' },
        { ID: 14, Color: 'rgb(149, 133, 159)' },
        { ID: 15, Color: 'rgb(113, 200, 194)' },
        { ID: 16, Color: 'rgb(216, 198, 158)' },
        { ID: 17, Color: 'rgb(236, 147, 89)' },
        { ID: 18, Color: 'rgb(158, 205, 235)' },
        { ID: 19, Color: 'rgb(182, 105, 141)' },
        { ID: 20, Color: 'rgb(149, 146, 127)' },
        { ID: 21, Color: 'rgb(170, 123, 103)' },
        { ID: 22, Color: 'rgb(180, 171, 172)' },
        { ID: 23, Color: 'rgb(233, 147, 160)' },
        { ID: 24, Color: 'rgb(191, 216, 99)' },
        { ID: 25, Color: 'rgb(23, 129, 93)' },
        { ID: 26, Color: 'rgb(128, 199, 255)' },
        { ID: 27, Color: 'rgb(175, 69, 231)' },
        { ID: 28, Color: 'rgb(208, 172, 24)' },
        { ID: 29, Color: 'rgb(79, 106, 177)' },
        { ID: 30, Color: 'rgb(53, 170, 188)' },
        { ID: 31, Color: 'rgb(189, 162, 132)' },
        { ID: 32, Color: 'rgb(205, 226, 255)' },
        { ID: 33, Color: 'rgb(241, 241, 155)' },
        { ID: 34, Color: 'rgb(238, 145, 164)' },
        { ID: 35, Color: 'rgb(249, 143, 143)' },
        { ID: 36, Color: 'rgb(253, 240, 170)' },
        { ID: 37, Color: 'rgb(241, 241, 241)' },
        { ID: 38, Color: 'rgb(55, 118, 189)' },
        { ID: 39, Color: 'rgb(159, 159, 159)' },
        { ID: 40, Color: 'rgb(85, 85, 85)' },
        { ID: 41, Color: 'rgb(242, 158, 158)' },
        { ID: 42, Color: 'rgb(109, 184, 215)' },
        { ID: 43, Color: 'rgb(176, 238, 175)' },
        { ID: 44, Color: 'rgb(255, 167, 95)' },
        { ID: 45, Color: 'rgb(241, 241, 241)' },
        { ID: 46, Color: 'rgb(236, 240, 41)' },
        { ID: 47, Color: 'rgb(255, 154, 24)' },
        { ID: 48, Color: 'rgb(247, 69, 165)' },
        { ID: 49, Color: 'rgb(225, 59, 59)' },
        { ID: 50, Color: 'rgb(138, 109, 227)' },
        { ID: 51, Color: 'rgb(255, 139, 92)' },
        { ID: 52, Color: 'rgb(66, 109, 66)' },
        { ID: 53, Color: 'rgb(179, 221, 243)' },
        { ID: 54, Color: 'rgb(58, 100, 122)' },
        { ID: 55, Color: 'rgb(160, 160, 160)' },
        { ID: 56, Color: 'rgb(132, 114, 50)' },
        { ID: 57, Color: 'rgb(101, 185, 231)' },
        { ID: 58, Color: 'rgb(76, 66, 118)' },
        { ID: 59, Color: 'rgb(225, 59, 59)' },
        { ID: 60, Color: 'rgb(240, 203, 88)' },
        { ID: 61, Color: 'rgb(206, 63, 153)' },
        { ID: 62, Color: 'rgb(207, 207, 207)' },
        { ID: 63, Color: 'rgb(40, 107, 159)' },
        { ID: 64, Color: 'rgb(216, 123, 27)' },
        { ID: 65, Color: 'rgb(142, 131, 147)' },
        { ID: 66, Color: 'rgb(240, 203, 88)' },
        { ID: 67, Color: 'rgb(101, 185, 231)' },
        { ID: 68, Color: 'rgb(101, 185, 231)' },
        { ID: 69, Color: 'rgb(121, 206, 121)' },
        { ID: 70, Color: 'rgb(240, 203, 88)' },
        { ID: 71, Color: 'rgb(240, 203, 88)' },
        { ID: 72, Color: 'rgb(0, 0, 0)', Info: 'Transparent' },
        { ID: 73, Color: 'rgb(240, 203, 88)' },
        { ID: 74, Color: 'rgb(101, 185, 231)' },
        { ID: 75, Color: 'rgb(225, 59, 59)' },
        { ID: 76, Color: 'rgb(120, 36, 36)' },
        { ID: 77, Color: 'rgb(101, 185, 231)' },
        { ID: 78, Color: 'rgb(58, 100, 122)' },
        { ID: 79, Color: 'rgb(225, 59, 59)', Info: 'Transparent' },
        { ID: 80, Color: 'rgb(101, 185, 231)', Info: 'Transparent' },
        { ID: 81, Color: 'rgb(242, 164, 12)' },
        { ID: 82, Color: 'rgb(164, 204, 170)' },
        { ID: 83, Color: 'rgb(168, 84, 242)' },
        { ID: 84, Color: 'rgb(101, 185, 231)' },
        { ID: 85, Color: 'rgb(0, 0, 0)' },
    ];

    static LANGUAGE_BY_ENUM: { [key: number]: Language } = {
        [LanguageEnum.German]: new German(),
        [LanguageEnum.English]: new English(),
    };
}
