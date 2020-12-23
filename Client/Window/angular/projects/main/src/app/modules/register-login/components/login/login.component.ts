import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SettingsService } from '../../../../services/settings.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { validateName } from '../../validators/name.validator';
import { ErrorService, FormControlCheck } from '../../../shared/services/error.service';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from 'projects/main/src/app/enums/dtoclientevent.enum';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [ErrorService],
})
export class LoginComponent implements OnInit {
    @Input() isRegistered: boolean;
    @Input() name: string;

    @Output() openPasswordForgotten = new EventEmitter();

    hidePassword = true;

    formGroup: FormGroup;

    constructor(public settings: SettingsService, public errorService: ErrorService, private rageConnector: RageConnectorService) {}

    ngOnInit(): void {
        this.formGroup = new FormGroup({
            name: new FormControl(this.name, [validateName, Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
            password: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
        });

        this.addChecks();
    }

    login() {
        if (!this.formGroup.valid) {
            return;
        }
        this.rageConnector.call(DToClientEvent.TryLogin, this.formGroup.controls.name.value, this.formGroup.controls.password.value);
    }

    private addChecks() {
        this.errorService.add(new FormControlCheck('NameCheck', this.formGroup.controls.name));
        this.errorService.add(new FormControlCheck('PasswordCheck', this.formGroup.controls.password));
    }
}
