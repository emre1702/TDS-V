import { Injectable } from '@angular/core';
import { InitialDatas } from 'projects/main/src/app/initial-datas';
import { Observable, of } from 'rxjs';
import { BodyData } from '../models/body-data';
import { BodyService } from './body.service';

@Injectable()
export class BodyDebugService extends BodyService {
    getData(): Observable<BodyData> {
        return of(InitialDatas.bodyData);
    }
}
