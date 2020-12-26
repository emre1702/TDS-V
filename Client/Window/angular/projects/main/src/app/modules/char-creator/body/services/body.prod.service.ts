import { Injectable } from '@angular/core';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { RageConnectorService } from 'rage-connector';
import { Observable, of } from 'rxjs';
import { BodyData } from '../models/body-data';
import { BodyService } from './body.service';

@Injectable()
export class BodyProdService extends BodyService {
    private _cachedBodyData: BodyData;

    getData(): Observable<BodyData> {
        if (this._cachedBodyData) {
            return of(this._cachedBodyData);
        }

        return new Observable<BodyData>((observer) => {
            this.rageConnector.callCallback(ToClientEvent.LoadBodyData, [], (json) => {
                this._cachedBodyData = JSON.parse(json);
                observer.next(this._cachedBodyData);
                observer.complete();
            });
        });
    }

    constructor(private rageConnector: RageConnectorService) {
        super();
    }
}
