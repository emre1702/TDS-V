import { Observable } from 'rxjs';
import { ClothesDataKey } from '../enums/clothes-config-key.enum';
import { ClothesConfigs } from '../models/clothes-configs';

export abstract class ClothesService {
    abstract getData(): Observable<ClothesConfigs>;
    abstract getDrawableAmount(key: ClothesDataKey): Observable<number>;
    abstract getTextureAmount(key: ClothesDataKey, drawableId: number): Observable<number>;
}
