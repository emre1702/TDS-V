import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { UserpanelSettingsSpecialType } from '../enums/userpanel-settings-special-type.enum';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { NotificationService } from '../../../modules/shared/services/notification.service';

@Component({
    selector: 'app-userpanel-settings-special',
    templateUrl: './userpanel-settings-special.component.html',
    styleUrls: ['./userpanel-settings-special.component.scss']
})
export class UserpanelSettingsSpecialComponent implements OnInit, OnDestroy {

    formGroup = new FormGroup({
        usernameControl: new FormControl("", [Validators.minLength(3), Validators.maxLength(20)]),
        usernameBuyControl: new FormControl(false),
        passwordControl: new FormControl("", [Validators.minLength(3), Validators.maxLength(50)]),
        emailControl: new FormControl("", [Validators.email]),
        confirmPasswordControl: new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(50)])
    });
    hasToBuyUsername = false;

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        private notificationService: NotificationService) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.settingsSpecialLoaded.on(null, this.loadSettings.bind(this));
        this.settings.MoneyChanged.on(null, this.moneyChanged.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.settingsSpecialLoaded.off(null, this.loadSettings.bind(this));
        this.settings.MoneyChanged.off(null, this.moneyChanged.bind(this));
    }

    wantsToBuyUsernameChanged(event: MatSlideToggleChange) {
        if (event.checked) {
            this.formGroup.get("usernameControl").enable();
        } else {
            this.formGroup.get("usernameControl").disable();
        }
    }

    saveChanges() {
        const confirmedPassword = this.formGroup.get("confirmPasswordControl").value;
        this.rageConnector.callCallback(DToClientEvent.GetHashedPassword, [confirmedPassword], (hashedPassword) => {
            const username = (this.formGroup.get("usernameControl").value as string).trim();
            if ((!this.hasToBuyUsername || this.formGroup.get("usernameBuyControl").enabled && username.length) && this.userpanelService.allSettingsSpecial[0] != username) {
                this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                    [UserpanelSettingsSpecialType.Username, username, hashedPassword],
                    (err: string) => {
                        if (err.length) {
                            this.notificationService.showError(err);
                        } else {
                            this.userpanelService.allSettingsSpecial[0] = username;
                            this.showSaveSuccess(UserpanelSettingsSpecialType.Username);
                        }
                    });
            }

            const password = this.formGroup.get("passwordControl").value as string;
            if (password && password.length) {
                this.rageConnector.callCallback(DToClientEvent.GetHashedPassword, [password], (hashedNewPassword) => {
                    this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                        [UserpanelSettingsSpecialType.Password, hashedNewPassword, hashedPassword],
                        (err: string) => {
                            if (err.length) {
                                this.notificationService.showError(err);
                            } else {
                                this.showSaveSuccess(UserpanelSettingsSpecialType.Password);
                            }
                        });
                });
            }

            const email = this.formGroup.get("emailControl").value;
            if (this.userpanelService.allSettingsSpecial[1] != email) {
                this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                    [UserpanelSettingsSpecialType.Email, email, hashedPassword],
                    (err: string) => {
                        if (err.length) {
                            this.notificationService.showError(err);
                        } else {
                            this.showSaveSuccess(UserpanelSettingsSpecialType.Email);
                        }
                    });
            }
        });
    }

    private showSaveSuccess(type: UserpanelSettingsSpecialType) {
        this.notificationService.showSuccess(this.settings.Lang[UserpanelSettingsSpecialType[type] + "SettingSaved"]);
    }

    private loadSettings() {
        this.formGroup.get("usernameControl").setValue(this.userpanelService.allSettingsSpecial[0]);
        this.formGroup.get("passwordControl").setValue("");
        this.formGroup.get("emailControl").setValue(this.userpanelService.allSettingsSpecial[1]);
        this.formGroup.get("confirmPasswordControl").setValue("");
        this.hasToBuyUsername = this.userpanelService.allSettingsSpecial[2];
        this.wantsToBuyUsernameChanged(new MatSlideToggleChange(undefined, !this.hasToBuyUsername));

        if (this.hasToBuyUsername) {
            this.formGroup.get("usernameBuyControl").setValue(false);
            if (this.settings.Money > this.settings.Constants[2]) {
                this.formGroup.get("usernameBuyControl").enable();
            } else {
                this.formGroup.get("usernameBuyControl").disable();
            }
        }
    }

    private moneyChanged() {
        if (this.hasToBuyUsername) {
            this.formGroup.get("usernameBuyControl").setValue(false);
            if (this.settings.Money > this.settings.Constants[2]) {
                this.formGroup.get("usernameBuyControl").enable();
            } else {
                this.formGroup.get("usernameBuyControl").disable();
            }
        }
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
