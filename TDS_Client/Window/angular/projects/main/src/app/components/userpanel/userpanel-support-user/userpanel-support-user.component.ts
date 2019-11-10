import { Component, OnInit, ChangeDetectorRef, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { UserpanelService } from '../services/userpanel.service';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';

@Component({
    selector: 'app-userpanel-support-user',
    templateUrl: './userpanel-support-user.component.html',
    styleUrls: ['./userpanel-support-user.component.scss']
})
export class UserpanelSupportUserComponent implements OnInit, OnDestroy {
    supportTypeIcons: { [type: number]: string} = {
        [UserpanelSupportType.Question]: "help",
        [UserpanelSupportType.Help]: "info",
        [UserpanelSupportType.Compliment]: "thumb_up",
        [UserpanelSupportType.Complaint]: "thumb_down"
    };
    userpanelSupportType = UserpanelSupportType;
    creatingRequest = false;
    inRequest: number = undefined;

    currentRequest: {
        Title: string,
        Messages: { Author: string, Message: string, CreateTime: string }[],
        Type: UserpanelSupportType,
        AtleastAdminLevel: number };

    requestGroup: FormGroup;

    readonly titleMinLength = 10;
    readonly titleMaxLength = 80;
    readonly messageMinLength = 10;
    readonly messageMaxLength = 255;

    constructor(
        public userpanelService: UserpanelService,
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));


        this.requestGroup = new FormGroup({
            title: new FormControl('', [Validators.required, Validators.minLength(this.titleMinLength), Validators.maxLength(this.titleMaxLength)]),
            message: new FormControl('', [Validators.required, Validators.minLength(this.messageMinLength), Validators.maxLength(this.messageMaxLength)]),
            type: new FormControl(UserpanelSupportType.Question, [Validators.required]),
        });
    }

    ngOnDestroy() {
        this.userpanelService.supportRequests = undefined;
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    openCreateRequest() {
        for (const control of Object.values(this.requestGroup.controls)) {
            control.reset();
        }
        this.currentRequest = { Title: "", Messages: [], Type: UserpanelSupportType.Question, AtleastAdminLevel: 1 };
        this.requestGroup.get("type").enable();

        this.creatingRequest = true;
        this.changeDetector.detectChanges();
    }

    openRequest(id: number) {
        this.inRequest = id;
        this.requestGroup.get("type").disable();

        // DEBUG //
        this.currentRequest = { Title: "Bonus, wieso bist du so toll?", Messages: [
            { Author: "Pluz.", Message: "Hallo Bonus. Ich wollte wissen: Wie kann man so toll sein wie du?", CreateTime: "12.21.3213 13:12:23" },
            { Author: "Bonus", Message: "Naja, leider ist das etwas, womit man geboren wird. Tut mir Leid Pluz., du bist nicht damit geboren du Opfer.", CreateTime: "12.21.3213 13:12:23" },
            { Author: "Pluz.", Message: "Ach Mist, das ist so Schade!", CreateTime: "12.21.3213 13:12:23" },
            { Author: "Pluz.", Message: "Naja, aber es ist toll, dass ich Administrator unter so einem tollen Projektleiter sein kann.", CreateTime: "12.21.3213 13:12:23" },
            { Author: "Bonus", Message: "Jetzt übertreibst du aber! Es gibt viiiel besser Projektleiter wie z.B. diese eine Schlange! So toll der Typ!", CreateTime: "12.21.3213 13:12:23" },
            { Author: "Pluz.", Message: "JA MAN STIMMT! Peanut hat so Glück, dass er da Ticketsupporter sein durfte!!! ICH WILL AUCH!", CreateTime: "12.21.3213 13:12:23" },
        ], Type: UserpanelSupportType.Compliment, AtleastAdminLevel: 3 };
        this.changeDetector.detectChanges();
    }

    detectChanges() {
        this.changeDetector.detectChanges();
    }

    submitRequest() {
        this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];

        this.currentRequest.Title = this.requestGroup.get("title").value;
        this.currentRequest.Messages = [this.requestGroup.get("message").value];
        this.currentRequest.Type = this.requestGroup.get("type").value;
    }

    setAtleastAdminLevel(adminLevel: number) {
        this.currentRequest.AtleastAdminLevel = adminLevel;
        this.changeDetector.detectChanges();
    }

    closeSupportView() {
        this.inRequest = undefined;
        this.changeDetector.detectChanges();
    }

    getAdminLevels() {
        return this.settings.AdminLevels.slice(1);
    }

    goBack() {
        this.creatingRequest = false;
        this.changeDetector.detectChanges();
    }
}
