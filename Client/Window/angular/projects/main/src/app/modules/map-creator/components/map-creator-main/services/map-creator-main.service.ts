import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { MapCreateError } from '../../../enums/map-create-error';
import { LoadMapDialogGroup } from '../../../models/load-map-dialog-group';
import { LocationData } from '../../../models/location-data';

export abstract class MapCreatorMainService {
    abstract sendData(formGroup: FormGroup): Observable<MapCreateError>;
    abstract saveData(formGroup: FormGroup): Observable<MapCreateError>;
    abstract removeMap(formGroup: FormGroup): void;
    abstract loadPossibleMapNames(): Observable<LoadMapDialogGroup[]>;
    abstract changeLocation(locationData?: LocationData): void;
}
