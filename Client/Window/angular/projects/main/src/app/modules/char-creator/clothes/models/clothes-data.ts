import { ClothesDataKey } from '../enums/clothes-config-key.enum';
import { ClothesComponentOrPropData } from './clothes-component-or-prop-data';

export interface ClothesData {
    [key: number]: ClothesComponentOrPropData | number;
}
