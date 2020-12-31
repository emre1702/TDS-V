import { Injectable } from '@angular/core';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { RageConnectorService } from 'rage-connector';
import { Observable, of } from 'rxjs';
import { ClothesDataKey } from '../enums/clothes-config-key.enum';
import { ClothesConfigs } from '../models/clothes-configs';
import { ClothesService } from './clothes.service';

@Injectable()
export class ClothesProdService extends ClothesService {
    private _cachedData: ClothesConfigs;

    getData(): Observable<ClothesConfigs> {
        if (this._cachedData) {
            return of(this._cachedData);
        }

        const observable = new Observable<ClothesConfigs>((observer) => {
            this.rageConnector.callCallback(ToClientEvent.LoadClothesData, [], (json: string) => {
                this._cachedData = JSON.parse(json);
                observer.next(this._cachedData);
                observer.complete();
            });
        });
        return observable;
    }

    getDrawableAmount(key: ClothesDataKey): Observable<number> {
        const observable = new Observable<number>((observer) => {
            this.rageConnector.callCallback(ToClientEvent.GetClothesDrawableAmount, [key], (value: number) => {
                observer.next(value);
                observer.complete();
            });
        });
        return observable;
    }

    getTextureAmount(key: ClothesDataKey, drawableId: number): Observable<number> {
        if (drawableId === -1) {
            return of(0);
        }
        const observable = new Observable<number>((observer) => {
            this.rageConnector.callCallback(ToClientEvent.GetClothesTextureAmount, [key, drawableId], (value: number) => {
                observer.next(value);
                observer.complete();
            });
        });
        return observable;
    }

    constructor(private rageConnector: RageConnectorService) {
        super();
    }
}
