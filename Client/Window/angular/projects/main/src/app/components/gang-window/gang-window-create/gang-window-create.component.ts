import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { SettingsService } from '../../../services/settings.service';
import { SafeStyle, DomSanitizer } from '@angular/platform-browser';
import { Constants } from '../../../constants';
import { validBlipColorValidator } from './validators/valid-blip-color-validator';
import { ClipboardService } from 'ngx-clipboard';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { MatSnackBar } from '@angular/material';
import { GangWindowService } from '../services/gang-window-service';
import { GangCommand } from '../enums/gang-command.enum';
import { CustomMatSnackBarComponent } from '../../../extensions/customMatSnackbar';

@Component({
    selector: 'app-gang-window-create',
    templateUrl: './gang-window-create.component.html',
    styleUrls: ['./gang-window-create.component.scss']
})
export class GangWindowCreateComponent implements OnInit {

    nameFormControl = new FormControl("", [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(50),
        Validators.pattern("[a-zA-Z0-9_\\- ]*")
    ]);
    shortFormControl = new FormControl("", [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(10),
        Validators.pattern("[a-zA-Z0-9_\\-]*")
    ]);
    colorFormControl = new FormControl("rgb(255,255,255)", [
        Validators.required,
        Validators.pattern("rgb\\((\\d{1,3}), ?(\\d{1,3}), ?(\\d{1,3})\\)")
    ]);
    blipColorFormControl = new FormControl(1, [
        Validators.required,
        validBlipColorValidator()
    ]);

    createFormGroup = new FormGroup({
        0: this.nameFormControl,
        1: this.shortFormControl,
        2: this.colorFormControl,
        3: this.blipColorFormControl
    });

    showColorPickerForColor = false;
    showBlipColorWindow = false;

    constants = Constants;

    @Output() back = new EventEmitter();

    constructor(
        public settings: SettingsService,
        public sanitizer: DomSanitizer,
        private clipboardService: ClipboardService,
        private snackBar: MatSnackBar,
        private gangWindowService: GangWindowService) { }

    ngOnInit(): void {
    }

    createGang() {
        if (this.createFormGroup.invalid)
            return;

        const data = this.createFormGroup.getRawValue();
        this.gangWindowService.executeCommand(GangCommand.Create, [JSON.stringify(data)], () => {
            this.snackBar.openFromComponent(CustomMatSnackBarComponent,
                { data: this.settings.Lang.GangSuccessfullyCreatedInfo, duration: undefined });
            this.back.emit();
        }, true, false);
    }

    copyBlipColor() {
        const currentBlipId = this.blipColorFormControl.value;
        const blipColorData = Constants.BLIP_COLORS.find(b => b.ID == currentBlipId);
        if (!blipColorData)
            return;
        this.clipboardService.copy(blipColorData.Color);
    }

    getErrorMessage(control: FormControl) {
        for (const error in control.errors) {
            return error;
        }
    }

    getColor(color: string): SafeStyle {
        return this.sanitizer.bypassSecurityTrustStyle(color);
    }

    getBlipColor(id: number): SafeStyle {
        const blip = Constants.BLIP_COLORS.find(c => c.ID == id);
        if (!blip) {
            return this.sanitizer.bypassSecurityTrustStyle("rgb(255,255,255");
        }

        return this.sanitizer.bypassSecurityTrustStyle(blip.Color);
    }
}
