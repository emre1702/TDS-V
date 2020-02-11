import { Component, OnInit, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { UserpanelService } from '../services/userpanel.service';
import { MatSlideToggleChange, MatSnackBar } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { UserpanelSettingsSpecialType } from '../enums/userpanel-settings-special-type.enum';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';

@Component({
    selector: 'app-userpanel-settings-special',
    templateUrl: './userpanel-settings-special.component.html',
    styleUrls: ['./userpanel-settings-special.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelSettingsSpecialComponent implements OnInit, OnDestroy {

    formGroup = new FormGroup({
        usernameControl: new FormControl("", [Validators.minLength(3), Validators.maxLength(20)]),
        usernameBuyControl: new FormControl(false),
        passwordControl: new FormControl("", [Validators.minLength(3), Validators.maxLength(50)]),
        emailControl: new FormControl("", [Validators.email]),
        confirmPasswordControl: new FormControl("", [Validators.required, Validators.minLength(3), Validators.minLength(50)])
    });
    hasToBuyUsername = false;

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        private snackBar: MatSnackBar) { }

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
            const username = this.formGroup.get("usernameControl").value;
            if ((!this.hasToBuyUsername || this.formGroup.get("usernameBuyControl").value) && this.userpanelService.allSettingsSpecial[0] != username) {
                this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                    [UserpanelSettingsSpecialType.Username, username, hashedPassword],
                    (err: string) => {
                        if (err.length) {
                            this.showSaveError(err);
                        } else {
                            this.showSaveSuccess(UserpanelSettingsSpecialType.Username);
                        }
                    });
            }

            const password = this.formGroup.get("passwordControl").value as string;
            if (password && password.length) {
                this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                    [UserpanelSettingsSpecialType.Password, password, hashedPassword],
                    (err: string) => {
                        if (err.length) {
                            this.showSaveError(err);
                        } else {
                            this.showSaveSuccess(UserpanelSettingsSpecialType.Password);
                        }
                    });
            }

            const email = this.formGroup.get("emailControl").value;
            if (this.userpanelService.allSettingsSpecial[1] != email) {
                this.rageConnector.callCallbackServer(DToServerEvent.SaveSpecialSettingsChange,
                    [UserpanelSettingsSpecialType.Email, email, hashedPassword],
                    (err: string) => {
                        if (err.length) {
                            this.showSaveError(err);
                        } else {
                            this.showSaveSuccess(UserpanelSettingsSpecialType.Email);
                        }
                    });
            }
        });
    }

    private showSaveError(err: string) {
        this.snackBar.open(err, "OK", {
            duration: undefined,
            panelClass: "mat-app-background"
        });
    }

    private showSaveSuccess(type: UserpanelSettingsSpecialType) {
        this.snackBar.open(this.settings.Lang[UserpanelSettingsSpecialType[type] + "SettingSaved"], "OK", {
            duration: 3000,
            panelClass: "mat-app-background"
        });
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
