import { LanguageEnum } from '../../../enums/language.enum';

export class MapDataDto {
    /** Id */
    [0]: number;
    /** Name */
    [1]: string;
    /** Type */
    [2] = 0;
    /** Description */
    [3]: {
        [key in LanguageEnum]: string
    } = { [7]: "", [9]: "" };
    /** CreatorName */
    [4]: string;
    /** Rating */
    [5]: number;
}
