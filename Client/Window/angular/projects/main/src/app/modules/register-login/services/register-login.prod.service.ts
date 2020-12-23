import { Injectable } from '@angular/core';
import { RegisterLoginService } from './register-login.service';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { RegisterLoginInitData } from '../models/register-login-init-data';
import { Observable } from 'rxjs';

@Injectable()
export class RegisterLoginProdService extends RegisterLoginService {
    constructor(private rageConnector: RageConnectorService) {
        super();
    }

    loadRegisterLoginInitData(): Observable<RegisterLoginInitData> {
        const observable = new Observable<RegisterLoginInitData>((observer) => {
            this.rageConnector.callCallbackServer(DToServerEvent.LoadRegisterLoginInitData, [], (json: string) => {
                const data = JSON.parse(json);
                observer.next(data);
                observer.complete();
            });
        });

        return observable;
    }
}
