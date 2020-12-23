import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MainMenuService } from './main-menu.service';
import { Announcement } from '../models/announcement';
import { InitialDatas } from 'projects/main/src/app/initial-datas';
import { ChangelogsGroup } from 'projects/main/src/app/interfaces/changelogs/changelogs-group';

@Injectable()
export class MainMenuDebugService extends MainMenuService {
    loadAnnouncements(): Observable<Announcement[]> {
        return of(InitialDatas.announcements);
    }

    loadChangelogs(): Observable<ChangelogsGroup[]> {
        return of(InitialDatas.changelogs);
    }
}
