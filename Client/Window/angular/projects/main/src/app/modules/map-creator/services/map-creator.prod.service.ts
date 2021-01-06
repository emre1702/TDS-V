import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { RageConnectorService } from 'rage-connector';
import { Observable } from 'rxjs';
import { LanguageEnum } from '../../../enums/language.enum';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { MapCreateDataKey } from '../enums/map-create-data-key';
import { MapCreatorInfoType } from '../enums/map-creator-info-type';
import { MapCreatorNav } from '../enums/map-creator-nav';
import { createNewMap } from '../map-creator.helper';
import { MapCreateData } from '../models/map-create-data';
import { MapCreatorPosition } from '../models/map-creator-position';
import { MapCreatorService } from './map-creator.service';

@Injectable()
export class MapCreatorProdService extends MapCreatorService {
    constructor(private rageConnector: RageConnectorService) {
        super();
    }

    initMap(formGroup: FormGroup) {
        const map = createNewMap();
        formGroup.patchValue(map);
    }

    startNewMap(): void {
        this.rageConnector.call(ToClientEvent.MapCreatorStartNew);
    }

    loadMap(mapId: number): Observable<MapCreateData> {
        const observable = new Observable<MapCreateData>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.LoadMapForMapCreator, [mapId], (json: string) => {
                const dataObj: MapCreateData = JSON.parse(json.escapeJson());
                observer.next(dataObj);
                observer.complete();
            });
        });
        return observable;
    }

    startNewPosPlacing(type: MapCreatorNav, info?: string | number) {
        this.rageConnector.call(ToClientEvent.StartMapCreatorPosPlacing, type, info);
    }

    positionRemoved(pos: MapCreatorPosition) {
        this.rageConnector.call(ToClientEvent.RemoveMapCreatorPosition, pos[0]);
    }

    positionHold(pos: MapCreatorPosition) {
        this.rageConnector.call(ToClientEvent.HoldMapCreatorObject, pos[0]);
    }

    positionSelected(pos?: MapCreatorPosition) {
        this.rageConnector.call(ToClientEvent.MapCreatorHighlightPos, pos ? pos[0] : -1);
    }

    tpToPosition(pos: MapCreatorPosition) {
        this.rageConnector.call(ToClientEvent.TeleportToPositionRotation, pos[3], pos[4], pos[5], pos[8]);
    }

    addSyncListeners(formGroup: FormGroup) {
        formGroup.controls[MapCreateDataKey.Name].valueChanges.subscribe((value) => this.syncData(MapCreatorInfoType.Name, value));
        formGroup.controls[MapCreateDataKey.Type].valueChanges.subscribe((value) => this.syncData(MapCreatorInfoType.Type, value));
        formGroup.controls[MapCreateDataKey.Settings].valueChanges.subscribe((value) => this.syncData(MapCreatorInfoType.Settings, value));
        for (const lang of this.getLanguages()) {
            const langEnum = LanguageEnum[lang];
            (formGroup.controls[MapCreateDataKey.Description] as FormGroup).controls[langEnum].valueChanges.subscribe((value) =>
                this.syncDescriptionChange(langEnum, value)
            );
        }
    }

    syncDescriptionChange(langEnum: LanguageEnum, value: string) {
        const infoType = this.getMapCreatorInfoType(langEnum);
        this.syncData(infoType, value);
    }

    private syncData(infoType: MapCreatorInfoType, value: any) {
        this.rageConnector.call(ToServerEvent.MapCreatorSyncData, infoType, typeof value === 'object' ? JSON.stringify(value) : value);
    }

    private getMapCreatorInfoType(language: LanguageEnum): MapCreatorInfoType {
        switch (language) {
            case LanguageEnum.English:
                return MapCreatorInfoType.DescriptionEnglish;
            case LanguageEnum.German:
                return MapCreatorInfoType.DescriptionGerman;
        }
    }

    private getLanguages(): string[] {
        const keys = Object.keys(LanguageEnum);
        return keys.slice(keys.length / 2);
    }
}
