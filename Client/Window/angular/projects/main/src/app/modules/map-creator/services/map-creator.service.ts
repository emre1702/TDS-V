import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { MapCreatorNav } from '../enums/map-creator-nav';
import { MapCreateData } from '../models/map-create-data';
import { MapCreatorPosition } from '../models/map-creator-position';

export abstract class MapCreatorService {
    abstract initMap(formGroup: FormGroup): void;
    abstract startNewMap(): void;
    abstract loadMap(mapId: number): Observable<MapCreateData>;
    abstract startNewPosPlacing(type: MapCreatorNav, info?: string | number): void;
    abstract positionRemoved(pos: MapCreatorPosition): void;
    abstract positionHold(pos: MapCreatorPosition): void;
    abstract tpToPosition(pos: MapCreatorPosition): void;
    abstract positionSelected(pos?: MapCreatorPosition): void;
    abstract addSyncListeners(formGroup: FormGroup): void;
}
