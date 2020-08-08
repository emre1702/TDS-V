import { CharCreateHeritageData } from './charCreateHeritageData';
import { CharCreateFeaturesData } from './charCreateFeaturesData';
import { CharCreateAppearanceData } from './charCreateAppearanceData';
import { CharCreateHairAndColorsData } from './charCreateHairAndColorsData';
import { CharCreateGeneralData } from './charCreateGeneralData';

export interface CharCreateData {
    /** GeneralData */
    [0]: CharCreateGeneralData[];

    /** HeritageData */
    [1]: CharCreateHeritageData[];

    /** FeaturesData */
    [2]: CharCreateFeaturesData[];

    /** AppearanceData */
    [3]: CharCreateAppearanceData[];

    /** HairAndColorsData */
    [4]: CharCreateHairAndColorsData[];

    /** Slot */
    [99]: number;
}
