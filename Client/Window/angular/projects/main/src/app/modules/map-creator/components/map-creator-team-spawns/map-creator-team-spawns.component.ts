import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MapCreatorPositionType } from '../../enums/map-creator-position-type';

import { MapCreatorPosition } from '../../models/map-creator-position';
@Component({
    selector: 'app-map-creator-team-spawns',
    templateUrl: './map-creator-team-spawns.component.html',
    styleUrls: ['./map-creator-team-spawns.component.scss'],
})
export class MapCreatorTeamSpawnsComponent implements OnInit, OnDestroy {
    @Input() fC: FormControl;
    @Output() positionSelected = new EventEmitter<MapCreatorPosition>();
    @Output() positionPlacingStarted = new EventEmitter<string | number | undefined>();
    @Output() positionRemoved = new EventEmitter<MapCreatorPosition>();
    @Output() positionHold = new EventEmitter<MapCreatorPosition>();
    @Output() positionTeleported = new EventEmitter<MapCreatorPosition>();

    displayedColumns: string[] = ['id', 'x', 'y', 'z', 'rot'];
    selectedPosition: MapCreatorPosition;
    dataSource: MapCreatorPosition[];

    selectedTeamNumber: number = 0;

    private _destroySubject = new Subject();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit() {
        this.fC.valueChanges.pipe(takeUntil(this._destroySubject)).subscribe((value: MapCreatorPosition[][]) => {
            this.selectTeamNumber(Math.max(0, Math.min(value.length - 1, this.selectedTeamNumber)));
            if (value[this.selectedTeamNumber].indexOf(this.selectedPosition) === -1) {
                this.selectPosition(undefined);
            }
            this.dataSource = [...this.fC.value[this.selectedTeamNumber]];
            this.changeDetector.detectChanges();
        });
        this.dataSource = this.fC.value[0];
    }

    ngOnDestroy() {
        this._destroySubject.next();
    }

    selectPosition(pos?: MapCreatorPosition) {
        this.selectedPosition = pos;
        this.positionSelected.emit(pos);
        this.changeDetector.detectChanges();
    }

    addTeam() {
        const teams = this.fC.value as [][];
        teams.push([]);
        this.fC.patchValue([...teams]);
        this.selectTeamNumber(teams.length - 1);
    }

    removeSelectedTeam() {
        const teams = this.fC.value as [][];
        if (teams.length == 1) return;

        const teamNumber = this.selectedTeamNumber;
        teams.splice(teamNumber, 1);
        this.selectTeamNumber(Math.max(0, Math.min(teams.length - 1, this.selectedTeamNumber)));
        this.fC.patchValue([...teams]);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.RemoveMapCreatorTeamNumber, teamNumber);
    }

    selectTeamNumber(value: number) {
        this.selectedTeamNumber = value;
        this.dataSource = this.fC.value[value];
        this.changeDetector.detectChanges();
    }

    addPos() {
        this.positionPlacingStarted.emit(Number(this.selectedTeamNumber));
    }

    removeSelectedPos() {
        const pos = this.selectedPosition;
        this.selectPosition(undefined);

        const positionsPerTeam = this.fC.value as MapCreatorPosition[][];
        const positions = positionsPerTeam[this.selectedTeamNumber];
        const index = positions.indexOf(pos);
        if (index >= 0) {
            positions.splice(index, 1);
        }
        this.dataSource = [...positions];
        this.positionRemoved.emit(pos);
        this.changeDetector.detectChanges();
    }

    holdSelectedPos() {
        this.positionHold.emit(this.selectedPosition);
    }

    tpToSelectedPos() {
        this.positionTeleported.emit(this.selectedPosition);
    }

    canModifyPos(pos: MapCreatorPosition) {
        return pos && (this.settings.IsLobbyOwner || this.isOwner(pos));
    }

    private isOwner(pos: MapCreatorPosition) {
        return pos && pos[9] == this.settings.Constants[1];
    }
}
