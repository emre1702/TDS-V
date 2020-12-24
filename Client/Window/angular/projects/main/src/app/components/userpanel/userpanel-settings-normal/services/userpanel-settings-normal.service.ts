import { Injectable, EventEmitter } from '@angular/core';
import { UserpanelSettingsNormalType } from '../enums/userpanel-settings-normal-type.enum';
import { UserpanelService } from '../../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from 'projects/main/src/app/enums/to-server-event.enum';
import { Observable } from 'rxjs';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { NotificationService } from 'projects/main/src/app/modules/shared/services/notification.service';
import { InitialDatas } from 'projects/main/src/app/initial-datas';
import { LanguagePipe } from 'projects/main/src/app/modules/shared/pipes/language.pipe';

@Injectable()
export class UserpanelSettingsNormalService {
    currentType?: UserpanelSettingsNormalType;

    loadedSettingsByType: { [key: number]: {} } = {};
    settingsLoaded = new EventEmitter<UserpanelSettingsNormalType>();

    constructor(
        private userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        private settings: SettingsService,
        private notificationService: NotificationService
    ) {}

    save(type: UserpanelSettingsNormalType, setting: {} | string): Observable<string> {
        if (typeof setting !== 'string') {
            setting = JSON.stringify(setting);
        }
        const observable = new Observable<string>((observer) => {
            this.rageConnector.callCallbackServer(ToServerEvent.SaveUserpanelNormalSettings, [type, setting], (error: string) => {
                if (error.length) {
                    this.notificationService.showError(error);
                    observer.error(error);
                } else {
                    this.notificationService.showSuccess(error);
                    observer.next(new LanguagePipe().transform('SettingSavedSuccessfully', this.settings.Lang));
                }
                observer.complete();
            });
        });

        return observable;
    }

    navigateTo(type: UserpanelSettingsNormalType) {
        if (this.loadedSettingsByType[type]) {
            this.currentType = type;
            this.userpanelService.setLoadingData(false);
            return;
        }

        this.userpanelService.setLoadingData(true);

        if (!InitialDatas.inDebug) {
            this.loadDataFromServer(type);
        } else {
            this.useInitialDatas(type);
        }
    }

    private loadDataFromServer(type: UserpanelSettingsNormalType) {
        this.rageConnector.callCallbackServer(ToServerEvent.LoadUserpanelNormalSettingsData, [type], (json: string) => {
            this.loadedSettingsByType[type] = JSON.parse(json.escapeJson());
            this.currentType = type;
            this.userpanelService.setLoadingData(false);
            this.settingsLoaded.emit(type);
        });
    }

    private useInitialDatas(type: UserpanelSettingsNormalType) {
        this.loadedSettingsByType[type] = InitialDatas.settingsByType[type];
        this.currentType = type;
        this.userpanelService.setLoadingData(false);
        this.settingsLoaded.emit(type);
    }
}
