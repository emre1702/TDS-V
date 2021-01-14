import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { ToServerEvent } from 'projects/main/src/app/enums/to-server-event.enum';
import { RageConnectorService } from 'rage-connector';
import { Observable } from 'rxjs';
import { MapCreateDataKey } from '../../../enums/map-create-data-key';
import { MapCreateError } from '../../../enums/map-create-error';
import { LoadMapDialogGroup } from '../../../models/load-map-dialog-group';
import { LocationData } from '../../../models/location-data';
import { MapCreatorMainService } from './map-creator-main.service';

@Injectable()
export class MapCreatorMainProdService extends MapCreatorMainService {
    constructor(private rageConnector: RageConnectorService) {
        super();
    }

    sendData(formGroup: FormGroup): Observable<MapCreateError> {
        const observable = new Observable<MapCreateError>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.SendMapCreatorData, [JSON.stringify(formGroup.value)], (err: MapCreateError) => {
                observer.next(err);
                observer.complete();
            });
        });
        return observable;
    }

    saveData(formGroup: FormGroup): Observable<MapCreateError> {
        const observable = new Observable<MapCreateError>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.SaveMapCreatorData, [JSON.stringify(formGroup.value)], (err: MapCreateError) => {
                observer.next(err);
                observer.complete();
            });
        });
        return observable;
    }

    removeMap(formGroup: FormGroup): void {
        const mapId = formGroup.controls[MapCreateDataKey.Id].value;
        if (mapId !== 0) this.rageConnector.call(ToServerEvent.RemoveMap, mapId);
    }

    loadPossibleMapNames(): Observable<LoadMapDialogGroup[]> {
        const observable = new Observable<LoadMapDialogGroup[]>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.LoadMapNamesToLoadForMapCreator, null, (possibleMapsJson: string) => {
                const possibleMaps = JSON.parse(possibleMapsJson) as LoadMapDialogGroup[];
                observer.next(possibleMaps);
                observer.complete();
            });
        });
        return observable;
    }

    changeLocation(locationData?: LocationData) {
        this.rageConnector.call(ToClientEvent.MapCreatorChangeLocation, locationData ? JSON.stringify(locationData) : undefined);
    }
}
