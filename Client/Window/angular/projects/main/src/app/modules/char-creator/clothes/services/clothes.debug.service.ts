import { Injectable } from '@angular/core';
import { InitialDatas } from 'projects/main/src/app/initial-datas';
import { Observable, of } from 'rxjs';
import { ClothesDataKey } from '../enums/clothes-config-key.enum';
import { ClothesConfigs } from '../models/clothes-configs';
import { ClothesService } from './clothes.service';

@Injectable()
export class ClothesDebugService extends ClothesService {
    getData(): Observable<ClothesConfigs> {
        return of(InitialDatas.clothesData);
    }

    getDrawableAmount(key: ClothesDataKey): Observable<number> {
        return of(Math.floor(Math.random() * 20));
    }

    getTextureAmount(key: ClothesDataKey, drawableId: number): Observable<number> {
        return of(drawableId);
    }
}
