import { LanguageEnum } from '../../../enums/language.enum';
import { MapCreatorPosition } from './mapCreatorPosition';
import { MapCreateSettings } from './mapCreateSettings';

export class MapCreateDataDto {
    /** Id */
    [0] = 0;
    /** Name */
    [1] = "";
    /** Type */
    [2] = 0;
    /** Settings */
    [3]: MapCreateSettings = new MapCreateSettings();
    /** Description */
    [4]: {
        [key in LanguageEnum]: string
    } = { [LanguageEnum.German]: "", [LanguageEnum.English]: "" };
    /** Objects */
    [5]: MapCreatorPosition[] = [];
    /** TeamSpawns */
    [6]: MapCreatorPosition[][] = [[]];
    /** MapEdges */
    [7]: MapCreatorPosition[] = [];
    /** BombPlaces */
    [8]: MapCreatorPosition[] = [];
    /** MapCenter */
    [9]: MapCreatorPosition;
}
