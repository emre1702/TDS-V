import { Component, OnInit, Input } from '@angular/core';
import { validateName } from '../../validators/name.validator';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ErrorService, FormControlCheck } from '../../../shared/services/error.service';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';

@Component({
    selector: 'app-password-forgotten',
    templateUrl: './password-forgotten.component.html',
    styleUrls: ['./password-forgotten.component.scss'],
    providers: [ErrorService],
})
export class PasswordForgottenComponent implements OnInit {
    @Input() name: string;

    formGroup: FormGroup;

    constructor(public settings: SettingsService, public errorService: ErrorService, private rageConnector: RageConnectorService) {}

    ngOnInit(): void {
        this.formGroup = new FormGroup({
            name: new FormControl(this.name, [validateName, Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
            email: new FormControl('', [Validators.required, Validators.email]),
        });
        this.addChecks();
    }

    resetPassword() {
        if (!this.formGroup.valid) {
            return;
        }
        this.rageConnector.call(ToClientEvent.ResetPassword, this.formGroup.controls.name.value, this.formGroup.controls.email.value);
    }

    private addChecks() {
        this.errorService.add(new FormControlCheck('NameCheck', this.formGroup.controls.name));
        this.errorService.add(new FormControlCheck('EmailCheck', this.formGroup.controls.email));
    }
}
