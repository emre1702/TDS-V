import { Component, OnInit, Input } from '@angular/core';
import { validateName } from '../../validators/name.validator';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ErrorService, FormControlCheck } from '../../../shared/services/error.service';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { NotificationService } from '../../../shared/services/notification.service';
import { RegisterLoginBase } from '../register-login-base';

@Component({
    selector: 'app-password-forgotten',
    templateUrl: './password-forgotten.component.html',
    styleUrls: ['./password-forgotten.component.scss'],
    providers: [ErrorService],
})
export class PasswordForgottenComponent extends RegisterLoginBase implements OnInit {
    @Input() name: string;

    constructor(
        public settings: SettingsService,
        public errorService: ErrorService,
        rageConnector: RageConnectorService,
        notificationService: NotificationService
    ) {
        super(rageConnector, notificationService);
    }

    ngOnInit(): void {
        this.formGroup = new FormGroup({
            name: new FormControl(this.name, [validateName, Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
            email: new FormControl('', [Validators.required, Validators.email]),
        });
        this.addChecks();
    }

    resetPassword() {
        this.send(ToClientEvent.ResetPassword, [this.formGroup.controls.name.value, this.formGroup.controls.email.value]);
    }

    private addChecks() {
        this.errorService.add(new FormControlCheck('NameCheck', this.formGroup.controls.name));
        this.errorService.add(new FormControlCheck('EmailCheck', this.formGroup.controls.email));
    }
}
