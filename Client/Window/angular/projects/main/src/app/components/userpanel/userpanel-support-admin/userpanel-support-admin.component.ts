import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';

@Component({
    selector: 'app-userpanel-support-admin',
    templateUrl: './userpanel-support-admin.component.html',
    styleUrls: ['./userpanel-support-admin.component.scss']
})
export class UserpanelSupportAdminComponent implements OnInit, OnDestroy {

    inRequest: number = undefined;
    requestGroup: FormGroup;

    currentRequest: [
        /** ID */
        number,
        /** Title */
        string,
        /** Messages */
        [
            /** Author */
            string,
            /** Message */
            string,
            /** CreateTime */
            string
        ][],
        /** Type */
        UserpanelSupportType,
        /** AtleastAdminLevel */
        number,
        /** Closed */
        boolean
    ] = undefined;

    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    constructor(
        public userpanelService: UserpanelService,
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.rageConnector.listen(DFromServerEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));

        this.requestGroup = new FormGroup({
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));
    }

    openRequest(id: number) {
        this.inRequest = id;
        this.requestGroup.get("type").disable();

        this.rageConnector.callCallbackServer(DToServerEvent.GetSupportRequestData, [id], (json: string) => {
            this.currentRequest = JSON.parse(json);
            this.changeDetector.detectChanges();
        });
    }

    closeSupportView() {
        this.inRequest = undefined;
        this.currentRequest = undefined;
        this.changeDetector.detectChanges();
    }

    private setRequestClosed(requestId: number, closed: boolean) {
        if (this.currentRequest[0] == requestId) {
            this.currentRequest[5] = closed;
        }
        const request = this.userpanelService.supportRequests.find(r => r[0] == requestId);
        if (request) {
            request[5] = closed;
        }
    }
}
