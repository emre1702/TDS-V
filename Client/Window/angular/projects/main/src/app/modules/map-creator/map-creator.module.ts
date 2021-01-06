import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { TDSWindowModule } from '../tds-window/tds-window.module';
import { MapCreatorComponent } from './map-creator.component';
import { MapCreatorMainComponent } from './components/map-creator-main/map-creator-main.component';
import { MapCreatorSettingsComponent } from './components/map-creator-settings/map-creator-settings.component';
import { MapCreatorDescriptionComponent } from './components/map-creator-description/map-creator-description.component';
import { MapCreatorTeamSpawnsComponent } from './components/map-creator-team-spawns/map-creator-team-spawns.component';
import { MapCreatorPositionsArrayBaseComponent } from './components/map-creator-positions-array-base/map-creator-positions-array-base.component';
import { MapCreatorPositionBaseComponent } from './components/map-creator-position-base/map-creator-position-base.component';
import { MapCreatorObjectChoiceComponent } from './components/map-creator-object-choice/map-creator-object-choice.component';
import { MapCreatorVehicleChoiceComponent } from './components/map-creator-vehicle-choice/map-creator-vehicle-choice.component';

@NgModule({
    declarations: [
        MapCreatorComponent,
        MapCreatorMainComponent,
        MapCreatorSettingsComponent,
        MapCreatorDescriptionComponent,
        MapCreatorTeamSpawnsComponent,
        MapCreatorPositionBaseComponent,
        MapCreatorPositionsArrayBaseComponent,
        MapCreatorObjectChoiceComponent,
        MapCreatorVehicleChoiceComponent,
    ],
    exports: [MapCreatorComponent],
    imports: [CommonModule, FormsModule, MaterialModule, SharedModule, TDSWindowModule],
})
export class MapCreatorModule {}
