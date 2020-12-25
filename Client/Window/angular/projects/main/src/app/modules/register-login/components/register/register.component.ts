import { Component, OnInit, Input } from '@angular/core';
import { SettingsService } from '../../../../services/settings.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { validateName } from '../../validators/name.validator';
import { ErrorService, FormControlCheck, CustomErrorCheck } from '../../../shared/services/error.service';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { RegisterLoginBase } from '../register-login-base';
import { NotificationService } from '../../../shared/services/notification.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    providers: [ErrorService],
})
export class RegisterComponent extends RegisterLoginBase implements OnInit {
    @Input() name: string;

    hidePasswordOne = true;
    hidePasswordTwo = true;

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
            password1: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
            password2: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
            email: new FormControl('', [Validators.email]),
        });

        this.addChecks();
    }

    register() {
        this.send(ToClientEvent.TryRegister, [
            this.formGroup.controls.name.value,
            this.formGroup.controls.password1.value,
            this.formGroup.controls.email.value,
        ]);
    }

    private addChecks() {
        this.errorService.add(new FormControlCheck('NameCheck', this.formGroup.controls.name));
        this.errorService.add(new FormControlCheck('PasswordCheck', this.formGroup.controls.password1));
        this.errorService.add(new FormControlCheck('PasswordCheck', this.formGroup.controls.password2));
        this.errorService.add(
            new CustomErrorCheck(
                'PasswordSameCheck',
                () => this.formGroup.controls.password1.value == this.formGroup.controls.password2.value,
                'PasswordsAreDifferentError'
            )
        );
        this.errorService.add(new FormControlCheck('EmailCheck', this.formGroup.controls.email));
    }
}
