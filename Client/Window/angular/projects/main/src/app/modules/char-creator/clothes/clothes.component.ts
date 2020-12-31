import { ChangeDetectorRef, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { SettingsService } from '../../../services/settings.service';
import { NotificationService } from '../../shared/services/notification.service';
import { ClothesDataKey } from './enums/clothes-config-key.enum';
import { ClothesConfigs } from './models/clothes-configs';
import { ClothesData } from './models/clothes-data';
import { ClothesService } from './services/clothes.service';

@Component({
    selector: 'app-clothes',
    templateUrl: './clothes.component.html',
    styleUrls: ['./clothes.component.scss'],
})
export class ClothesComponent {
    @Output() back = new EventEmitter();

    data$ = this.service.getData();

    currentNav: ClothesDataKey = ClothesDataKey.Main;
    clothesDataKey = ClothesDataKey;
    readonly contents = [
        ClothesDataKey.Hats,
        ClothesDataKey.Glasses,
        ClothesDataKey.Masks,
        ClothesDataKey.EarAccessories,
        ClothesDataKey.Jackets,
        ClothesDataKey.Shirts,
        ClothesDataKey.BodyArmors,
        ClothesDataKey.Decals,
        ClothesDataKey.Hands,
        ClothesDataKey.Watches,
        ClothesDataKey.Bracelets,
        ClothesDataKey.Accessories,
        ClothesDataKey.Bags,
        ClothesDataKey.Legs,
        ClothesDataKey.Shoes,
    ];

    constructor(
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService,
        public settings: SettingsService,
        private service: ClothesService,
        private notificationService: NotificationService
    ) {}

    goBack() {
        if (this.currentNav === ClothesDataKey.Main) {
            this.back.emit();
            return;
        }
        this.currentNav = ClothesDataKey.Main;
        this.changeDetector.detectChanges();
    }

    goToNav(key: ClothesDataKey) {
        this.currentNav = key;
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.ClothesNavChanged, key);
    }

    save(data: ClothesConfigs) {
        this.rageConnector.callCallbackServer(ToServerEvent.SaveClothesData, [JSON.stringify(data)], (err: string) => {
            if (err?.length) {
                this.notificationService.showError(err);
            } else {
                this.notificationService.showSuccess('SettingSavedSuccessfully');
            }
        });
    }

    getData(configs: ClothesConfigs): ClothesData {
        return configs[0].find((c) => c[ClothesDataKey.Slot] === configs[1]);
    }

    slotChanged(configs: ClothesConfigs, slotIndex: number) {
        configs[1] = slotIndex;
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.ClothesDataChanged, ClothesDataKey.Slot, slotIndex);
    }
}
