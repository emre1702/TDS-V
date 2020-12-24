import { Component, OnInit, ChangeDetectorRef, Output, Input, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { EventEmitter } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { FromServerEvent } from '../../../enums/from-server-event.enum';
import { UserpanelSupportRequestData } from '../interfaces/userpanelSupportRequestData';

@Component({
    selector: 'app-userpanel-support-view',
    templateUrl: './userpanel-support-view.component.html',
    styleUrls: ['./userpanel-support-view.component.scss'],
})
export class UserpanelSupportViewComponent implements OnInit, OnDestroy, AfterViewInit {
    requestGroup: FormGroup;
    userpanelSupportType = UserpanelSupportType;

    @Input() currentRequest: UserpanelSupportRequestData;

    readonly titleMinLength = 10;
    readonly titleMaxLength = 80;
    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    @ViewChild('messagesPanel') private messagesPanel: ElementRef;

    @Output() back: EventEmitter<null> = new EventEmitter<null>();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit() {
        this.requestGroup = new FormGroup({
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
        });

        if (this.currentRequest[5]) {
            this.requestGroup.get('message').disable();
            this.changeDetector.detectChanges();
        }

        this.rageConnector.listen(FromServerEvent.SyncNewSupportRequestMessage, this.syncNewSupportRequestMessage.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.callServer(ToServerEvent.LeftSupportRequest, this.currentRequest[0]);

        this.rageConnector.remove(FromServerEvent.SyncNewSupportRequestMessage, this.syncNewSupportRequestMessage.bind(this));
    }

    ngAfterViewInit() {
        this.messagesPanel.nativeElement.scrollTop = this.messagesPanel.nativeElement.scrollHeight;
        this.changeDetector.detectChanges();
    }

    sendMessage() {
        const message = this.requestGroup.get('message').value;
        this.rageConnector.callServer(ToServerEvent.SendSupportRequestMessage, this.currentRequest[0], message);
        this.requestGroup.get('message').setValue('');

        this.changeDetector.detectChanges();
    }

    toggleRequestClosed() {
        this.currentRequest[5] = !this.currentRequest[5];

        if (this.currentRequest[5]) {
            this.requestGroup.get('message').disable();
            this.requestGroup.get('message').setValue('');
        } else {
            this.requestGroup.get('message').enable();
        }

        this.changeDetector.detectChanges();

        this.rageConnector.callServer(ToServerEvent.SetSupportRequestClosed, this.currentRequest[0], this.currentRequest[5]);
    }

    goBack() {
        this.back.emit(null);
    }

    getAdminLevels() {
        return this.settings.AdminLevels.slice(1);
    }

    private syncNewSupportRequestMessage(requestId: number, messageJson: string) {
        if (this.currentRequest[0] != requestId) {
            return;
        }
        const isScrolledToBottom = this.messagesPanel.nativeElement.scrollTop == this.messagesPanel.nativeElement.scrollHeight;
        this.currentRequest[2].push(JSON.parse(messageJson));

        this.changeDetector.detectChanges();

        if (isScrolledToBottom) {
            this.messagesPanel.nativeElement.scrollTop = this.messagesPanel.nativeElement.scrollHeight;
            this.changeDetector.detectChanges();
        }
    }
}
