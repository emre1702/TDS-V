import { Injectable, NgZone } from '@angular/core';
import { MainMenuService } from './main-menu.service';
import { Announcement } from '../models/announcement';
import { Observable, of } from 'rxjs';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent as ToServerEvent } from 'projects/main/src/app/enums/to-server-event.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { ChangelogsGroup } from 'projects/main/src/app/interfaces/changelogs/changelogs-group';

@Injectable()
export class MainMenuProdService extends MainMenuService {
    loadAnnouncements(): Observable<Announcement[]> {
        if (this.settings.announcements) {
            return of(this.settings.announcements);
        }

        const observable = new Observable<Announcement[]>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.LoadAnnouncements, [], (json: string) => {
                const announcements = JSON.parse(json.escapeJson());
                this.settings.announcements = announcements;
                observer.next(announcements);
                observer.complete();
            });
        });
        return observable;
    }

    loadChangelogs(): Observable<ChangelogsGroup[]> {
        if (this.settings.changelogs) {
            return of(this.settings.changelogs);
        }

        const observable = new Observable<ChangelogsGroup[]>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.LoadChangelogs, [], (json: string) => {
                const changelogs = JSON.parse(json.escapeJson());
                this.settings.changelogs = changelogs;
                observer.next(changelogs);
                observer.complete();
            });
        });
        return observable;
    }

    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {
        super();
    }
}
