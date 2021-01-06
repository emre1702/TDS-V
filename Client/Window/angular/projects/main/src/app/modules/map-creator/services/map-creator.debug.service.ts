import { FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { InitialDatas } from '../../../initial-datas';
import { MapCreatorNav } from '../enums/map-creator-nav';
import { createNewMap } from '../map-creator.helper';
import { MapCreateData } from '../models/map-create-data';
import { MapCreatorPosition } from '../models/map-creator-position';
import { MapCreatorService } from './map-creator.service';

export class MapCreatorDebugService extends MapCreatorService {
    initMap(formGroup: FormGroup) {
        formGroup.patchValue(InitialDatas.mapCreateData);
    }

    startNewMap(): void {}

    loadMap(): Observable<MapCreateData> {
        return of(createNewMap());
    }

    startNewPosPlacing(type: MapCreatorNav, info?: string | number) {}

    positionRemoved(pos: MapCreatorPosition) {}
    positionHold(pos: MapCreatorPosition) {}
    tpToPosition(pos: MapCreatorPosition) {}
    positionSelected(pos?: MapCreatorPosition) {}
    addSyncListeners(formGroup: FormGroup) {}
}
