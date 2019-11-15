import { Component, OnInit, ChangeDetectorRef, Output, Input, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { EventEmitter } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';

@Component({
    selector: 'app-userpanel-support-view',
    templateUrl: './userpanel-support-view.component.html',
    styleUrls: ['./userpanel-support-view.component.scss']
})
export class UserpanelSupportViewComponent implements OnInit, OnDestroy, AfterViewInit {

    requestGroup: FormGroup;
    userpanelSupportType = UserpanelSupportType;

    @Input() currentRequest: {
        ID: number,
        Title: string,
        Messages: { Author: string, Message: string, CreateTime: string }[],
        Type: UserpanelSupportType,
        AtleastAdminLevel: number,
        Closed: boolean };

    readonly titleMinLength = 10;
    readonly titleMaxLength = 80;
    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    @ViewChild("messagesPanel", { static: false }) private messagesPanel: ElementRef;

    @Output() back: EventEmitter<null> = new EventEmitter<null>();

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.requestGroup = new FormGroup({
            title: new FormControl('', [Validators.required, Validators.minLength(this.titleMinLength), Validators.maxLength(this.titleMaxLength)]),
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });

        if (this.currentRequest.Closed) {
            this.requestGroup.get("message").disable();
            this.changeDetector.detectChanges();
        }

        this.rageConnector.listen(DFromClientEvent.SyncNewSupportRequestMessage, this.syncNewSupportRequestMessage.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.call(DToServerEvent.LeftSupportRequest, this.currentRequest.ID);

        this.rageConnector.remove(DFromClientEvent.SyncNewSupportRequestMessage, this.syncNewSupportRequestMessage.bind(this));
    }

    ngAfterViewInit() {
        this.messagesPanel.nativeElement.scrollTop = this.messagesPanel.nativeElement.scrollHeight;
        this.changeDetector.detectChanges();
    }

    sendMessage() {
        const message = this.requestGroup.get("message").value;
        this.rageConnector.call(DToServerEvent.SendSupportRequestMessage, this.currentRequest.ID, message);
        this.requestGroup.get("message").setValue("");

        this.changeDetector.detectChanges();
    }

    toggleRequestClosed() {
        this.currentRequest.Closed = !this.currentRequest.Closed;

        if (this.currentRequest.Closed) {
            this.requestGroup.get("message").disable();
            this.requestGroup.get("message").setValue("");
        } else {
            this.requestGroup.get("message").enable();
        }

        this.changeDetector.detectChanges();

        this.rageConnector.call(DToServerEvent.SetSupportRequestClosed, this.currentRequest.ID, this.currentRequest.Closed);
    }

    goBack() {
        this.back.emit(null);
    }

    getAdminLevels() {
        return this.settings.AdminLevels.slice(1);
    }

    private syncNewSupportRequestMessage(requestId: number, messageJson: string) {
        if (this.currentRequest.ID != requestId) {
            return;
        }
        const isScrolledToBottom = this.messagesPanel.nativeElement.scrollTop == this.messagesPanel.nativeElement.scrollHeight;
        this.currentRequest.Messages.push(JSON.parse(messageJson));

        this.changeDetector.detectChanges();

        if (isScrolledToBottom) {
            this.messagesPanel.nativeElement.scrollTop = this.messagesPanel.nativeElement.scrollHeight;
            this.changeDetector.detectChanges();
        }
    }
}