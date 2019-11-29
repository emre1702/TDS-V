import { Component, OnInit, ChangeDetectorRef, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { UserpanelService } from '../services/userpanel.service';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';

@Component({
    selector: 'app-userpanel-support-user',
    templateUrl: './userpanel-support-user.component.html',
    styleUrls: ['./userpanel-support-user.component.scss']
})
export class UserpanelSupportUserComponent implements OnInit, OnDestroy {
    userpanelSupportType = UserpanelSupportType;
    creatingRequest = false;
    inRequest: number = undefined;

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
    ];

    requestGroup: FormGroup;

    readonly titleMinLength = 10;
    readonly titleMaxLength = 80;
    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    constructor(
        public userpanelService: UserpanelService,
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.rageConnector.listen(DFromClientEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));

        this.requestGroup = new FormGroup({
            title: new FormControl('', [Validators.required, Validators.minLength(this.titleMinLength), Validators.maxLength(this.titleMaxLength)]),
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });
    }

    ngOnDestroy() {
        this.userpanelService.supportRequests = undefined;
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.rageConnector.remove(DFromClientEvent.SetSupportRequestClosed, this.setRequestClosed.bind(this));
        this.rageConnector.call(DToServerEvent.LeftSupportRequestsList);
    }

    openCreateRequest() {
        for (const control of Object.values(this.requestGroup.controls)) {
            control.reset();
        }
        this.currentRequest = [0, "", [], UserpanelSupportType.Question, 1, false];
        this.requestGroup.get("type").enable();

        this.creatingRequest = true;
        this.changeDetector.detectChanges();
    }

    openRequest(id: number) {
        this.inRequest = id;
        this.requestGroup.get("type").disable();

        this.rageConnector.callCallback(DToServerEvent.GetSupportRequestData, [id], (json: string) => {
            this.currentRequest = JSON.parse(json);
            this.changeDetector.detectChanges();
        });
    }

    detectChanges() {
        this.changeDetector.detectChanges();
    }

    submitRequest() {
        this.currentRequest[1] = this.requestGroup.get("title").value;
        this.currentRequest[2] = [[ "", this.requestGroup.get("message").value, ""]];
        this.currentRequest[3] = this.requestGroup.get("type").value;

        this.rageConnector.call(DToServerEvent.SendSupportRequest, JSON.stringify(this.currentRequest));

        this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];
        this.changeDetector.detectChanges();
    }

    setAtleastAdminLevel(adminLevel: number) {
        this.currentRequest[4] = adminLevel;
        this.changeDetector.detectChanges();
    }

    closeSupportView() {
        this.inRequest = undefined;
        this.currentRequest = undefined;
        this.changeDetector.detectChanges();
    }

    getAdminLevels() {
        return this.settings.AdminLevels.slice(1);
    }

    goBack() {
        this.creatingRequest = false;
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
