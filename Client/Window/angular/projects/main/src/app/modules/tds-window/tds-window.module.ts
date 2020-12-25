import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TDSWindowComponent } from './tds-window.component';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    declarations: [TDSWindowComponent],
    imports: [CommonModule, MaterialModule, SharedModule],
    exports: [TDSWindowComponent],
})
export class TDSWindowModule {}
