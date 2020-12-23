import { Injectable } from '@angular/core';
import { Announcement } from '../models/announcement';
import { Observable } from 'rxjs';
import { ChangelogsGroup } from 'projects/main/src/app/interfaces/changelogs/changelogs-group';

@Injectable()
export abstract class MainMenuService {
    abstract loadAnnouncements(): Observable<Announcement[]>;
    abstract loadChangelogs(): Observable<ChangelogsGroup[]>;
}
