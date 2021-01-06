import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';
import { Observable, of } from 'rxjs';
import { MapCreateError } from '../../../enums/map-create-error';
import { LoadMapDialogGroup } from '../../../models/load-map-dialog-group';
import { MapCreatorMainService } from './map-creator-main.service';

@Injectable()
export class MapCreatorMainDebugService extends MapCreatorMainService {
    private possibleMapNames: LoadMapDialogGroup[] = [
        {
            0: 'Created',
            1: [
                { 0: 1, 1: 'Test Map 1' },
                { 0: 2, 1: 'Test Map 2' },
                { 0: 3, 1: 'Test Map 3' },
                { 0: 4, 1: 'Test Map 4' },
                { 0: 5, 1: 'Test Map 5' },
            ],
        },
        {
            0: 'Saved',
            1: [
                { 0: 1, 1: 'Test Saved Map 1' },
                { 0: 2, 1: 'Test Saved Map 2' },
                { 0: 3, 1: 'Test Saved Map 3' },
                { 0: 4, 1: 'Test Saved Map 4' },
                { 0: 5, 1: 'Test Saved Map 5' },
            ],
        },
        {
            0: 'Test',
            1: [
                { 0: 1, 1: 'Test Test Map 1' },
                { 0: 2, 1: 'Test Test Map 2' },
                { 0: 3, 1: 'Test Test Map 3' },
                { 0: 4, 1: 'Test Test Map 4' },
                { 0: 5, 1: 'Test Test Map 5' },
            ],
        },
    ];

    constructor() {
        super();
    }

    sendData(formGroup: FormGroup): Observable<MapCreateError> {
        return of(MapCreateError.MapCreatedSuccessfully);
    }

    saveData(formGroup: FormGroup): Observable<MapCreateError> {
        return of(MapCreateError.MapCreatedSuccessfully);
    }

    removeMap(formGroup: FormGroup): void {}

    loadPossibleMapNames(): Observable<LoadMapDialogGroup[]> {
        return of(this.possibleMapNames);
    }
}
