import { BodyHeritageData } from './body-heritage-data';
import { BodyFeaturesData } from './body-features-data';
import { BodyAppearanceData } from './body-appearance-data';
import { BodyHairAndColorsData } from './body-hair-and-colors-data';
import { BodyGeneralData } from './body-general-data';

export interface BodyData {
    /** GeneralData */
    [0]: BodyGeneralData[];

    /** HeritageData */
    [1]: BodyHeritageData[];

    /** FeaturesData */
    [2]: BodyFeaturesData[];

    /** AppearanceData */
    [3]: BodyAppearanceData[];

    /** HairAndColorsData */
    [4]: BodyHairAndColorsData[];

    /** Slot */
    [99]: number;
}
