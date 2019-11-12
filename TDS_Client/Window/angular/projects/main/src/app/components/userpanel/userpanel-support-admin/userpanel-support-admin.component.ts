import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-userpanel-support-admin',
    templateUrl: './userpanel-support-admin.component.html',
    styleUrls: ['./userpanel-support-admin.component.scss']
})
export class UserpanelSupportAdminComponent implements OnInit, OnDestroy {

    inRequest: number = undefined;
    requestGroup: FormGroup;

    currentRequest: {
        ID: number,
        Title: string,
        Messages: { Author: string, Message: string, CreateTime: string }[],
        Type: UserpanelSupportType,
        AtleastAdminLevel: number,
        Closed: boolean } = undefined;

    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    constructor(
        public userpanelService: UserpanelService,
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.rageConnector.listen(DFromClientEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));

        this.requestGroup = new FormGroup({
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromClientEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));
    }

    openRequest(id: number) {
        this.inRequest = id;
        this.requestGroup.get("type").disable();

        this.rageConnector.callCallback(DToServerEvent.GetSupportRequestData, [id], (json: string) => {
            this.currentRequest = JSON.parse(json);
            this.changeDetector.detectChanges();
        });


        setTimeout(() => {
            this.currentRequest = { AtleastAdminLevel: 2, Closed: false, ID: 1, Messages: [{Author: "Bonus", CreateTime: "12.23.3232", Message: "asd"}], Title: "HALLO", Type: UserpanelSupportType.Question};
            this.changeDetector.detectChanges();
        }, 3000);
    }

    closeSupportView() {
        this.inRequest = undefined;
        this.currentRequest = undefined;
        this.changeDetector.detectChanges();
    }

    private setRequestClosed(requestId: number, closed: boolean) {
        if (this.currentRequest.ID == requestId) {
            this.currentRequest.Closed = closed;
        }
        const request = this.userpanelService.supportRequests.find(r => r.ID == requestId);
        if (request) {
            request.Closed = closed;
        }
    }
}
