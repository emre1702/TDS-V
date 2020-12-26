import { Observable } from 'rxjs';
import { BodyData } from '../models/body-data';

export abstract class BodyService {
    abstract getData(): Observable<BodyData>;
}
