import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CharCreatorComponent } from './char-creator.component';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { BodyComponent } from './body/body.component';
import { BodyAppearanceComponent } from './body/menus/appearance/body-appearance.component';
import { BodyFeaturesComponent } from './body/menus/features/body-features.component';
import { BodyHairandcolorsComponent } from './body/menus/hairandcolors/body-hairandcolors.component';
import { BodyHeritageComponent } from './body/menus/heritage/body-heritage.component';
import { BodyMainmenuComponent } from './body/menus/mainmenu/body-mainmenu.component';
import { FormsModule } from '@angular/forms';
import { TDSWindowModule } from '../tds-window/tds-window.module';
@NgModule({
    declarations: [
        CharCreatorComponent,
        BodyComponent,
        BodyAppearanceComponent,
        BodyFeaturesComponent,
        BodyHairandcolorsComponent,
        BodyHeritageComponent,
        BodyMainmenuComponent,
    ],
    imports: [CommonModule, FormsModule, MaterialModule, SharedModule, TDSWindowModule],
    exports: [CharCreatorComponent],
})
export class CharCreatorModule {}
