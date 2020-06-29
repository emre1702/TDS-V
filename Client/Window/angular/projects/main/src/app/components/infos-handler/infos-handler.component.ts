import { Component, OnInit, ChangeDetectionStrategy, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../enums/dfromclientevent.enum';
import { InfoType } from './enums/info-type.enum';

@Component({
    selector: 'app-infos-handler',
    templateUrl: './infos-handler.component.html',
    styleUrls: ['./infos-handler.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfosHandlerComponent implements OnInit, OnDestroy {

    showCursorInfo: boolean;
    showLobbyLeaveInfo: boolean;

    constructor(
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void {
        this.rageConnector.listen(DFromClientEvent.ToggleInfo, this.toggleInfo.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromClientEvent.ToggleInfo, this.toggleInfo.bind(this));
    }

    private toggleInfo(type: InfoType, toggle: boolean) {
        switch (type) {
            case InfoType.Cursor:
                this.showCursorInfo = toggle;
                break;
            case InfoType.LobbyLeave:
                this.showLobbyLeaveInfo = toggle;
                break;
        }
        this.changeDetector.detectChanges();
    }

}
