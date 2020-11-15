import { NgModule } from '@angular/core';
import { SettingsComponent } from './settings.component';
import { StringSettingRowComponent } from './components/string/string-setting-row.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NumberSettingRowComponent } from './components/number/number-setting-row.component';
import { NumberSliderSettingRowComponent } from './components/numberslider/numberslider-setting-row.component';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { EnumSettingRowComponent } from './components/enum/enum-setting-row.component';
import { BooleanSettingRowComponent } from './components/boolean/boolean-setting-row.component';
import { BooleanSliderSettingRowComponent } from './components/booleanslider/booleanslider-setting-row.component';
import { ButtonRowComponent } from './components/button/button-row.component';
import { ColorSettingRowComponent } from './components/color/color-setting-row.component';
import { ColorPickerModule } from 'ngx-color-picker';
import { PasswordSettingRowComponent } from './components/password/password-setting-row.component';
import { DateTimeEnumSettingRowComponent } from './components/datetimeenum/datetimeenum-setting-row.component';

@NgModule({
    declarations: [
        SettingsComponent,
        BooleanSettingRowComponent,
        BooleanSliderSettingRowComponent,
        ButtonRowComponent,
        ColorSettingRowComponent,
        DateTimeEnumSettingRowComponent,
        EnumSettingRowComponent,
        NumberSettingRowComponent,
        NumberSliderSettingRowComponent,
        StringSettingRowComponent,
        PasswordSettingRowComponent,
    ],
    exports: [
        SettingsComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        SharedModule,
        ColorPickerModule
    ]
})
export class SettingsModule {

}
