import { RegisterLoginService } from './register-login.service';
import { RegisterLoginInitData } from '../models/register-login-init-data';
import { Injectable } from '@angular/core';
import { InitialDatas } from '../../../initial-datas';
import { Observable, of } from 'rxjs';
import { first } from 'rxjs/operators';

@Injectable()
export class RegisterLoginDebugService extends RegisterLoginService {
    loadRegisterLoginInitData(): Observable<RegisterLoginInitData> {
        return of(InitialDatas.registerLoginInitData).pipe(first());
    }
}
