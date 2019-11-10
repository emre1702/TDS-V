import { Component, OnInit, ChangeDetectorRef, Output, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { EventEmitter } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-userpanel-support-view',
    templateUrl: './userpanel-support-view.component.html',
    styleUrls: ['./userpanel-support-view.component.scss']
})
export class UserpanelSupportViewComponent implements OnInit, AfterViewInit {

    requestGroup: FormGroup;
    userpanelSupportType = UserpanelSupportType;

    @Input() currentRequest: {
        Id: number,
        Title: string,
        Messages: { Author: string, Message: string, CreateTime: string }[],
        Type: UserpanelSupportType,
        AtleastAdminLevel: number };

    readonly titleMinLength = 10;
    readonly titleMaxLength = 80;
    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    @ViewChild("messagesPanel", { static: false }) private messagesPanel: ElementRef;

    @Output() back: EventEmitter<null> = new EventEmitter<null>();

    constructor(
        public settings: SettingsService) { }

    ngOnInit() {
        this.requestGroup = new FormGroup({
            title: new FormControl('', [Validators.required, Validators.minLength(this.titleMinLength), Validators.maxLength(this.titleMaxLength)]),
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });
    }

    ngAfterViewInit() {
        this.messagesPanel.nativeElement.scrollTop = this.messagesPanel.nativeElement.scrollHeight;
    }

    sendMessage() {

    }

    goBack() {
        this.back.emit(null);
    }

    getAdminLevels() {
        return this.settings.AdminLevels.slice(1);
    }
}
