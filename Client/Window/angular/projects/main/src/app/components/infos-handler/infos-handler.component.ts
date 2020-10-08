import { Component, OnInit, ChangeDetectionStrategy, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { InfosHandlerService } from './services/infos-handler.service';
import { SettingsService } from '../../services/settings.service';
import { rightToLeftItemsEnterAnimation } from '../../animations/rightToLeftItemsEnter.animation';

@Component({
    selector: 'app-infos-handler',
    templateUrl: './infos-handler.component.html',
    styleUrls: ['./infos-handler.component.scss'],
    animations: [rightToLeftItemsEnterAnimation],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfosHandlerComponent implements OnInit, OnDestroy {
    constructor(
        private changeDetector: ChangeDetectorRef,
        public infosHandler: InfosHandlerService,
        public settings: SettingsService) { }

    ngOnInit(): void {
        this.infosHandler.infosChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.infosHandler.infosChanged.off(null, this.detectChanges.bind(this));
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
