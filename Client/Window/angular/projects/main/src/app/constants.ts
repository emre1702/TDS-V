import { MapType } from './enums/maptype.enum';

export class Constants {
    public static MIN_TEAM_SPAWNS = 3;

    public static MIN_MAP_CREATE_NAME_LENGTH = 3;
    public static MAX_MAP_CREATE_NAME_LENGTH = 100;

    public static MAX_POSSIBLE_ARMOR = 16959;

    public static MIN_TEAMS_PER_TYPE = {
        [MapType.Normal]: 2,
        [MapType.Bomb]: 2,
        [MapType.Sniper]: 2,
        [MapType.Gangwar]: 2,
        [MapType.ArmsRace]: 1
    };
}
