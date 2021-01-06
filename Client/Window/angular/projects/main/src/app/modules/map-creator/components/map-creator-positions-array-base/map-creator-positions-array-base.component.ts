import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MapCreatorPositionType } from '../../enums/map-creator-position-type';
import { MapCreatorPosition } from '../../models/map-creator-position';

@Component({
    selector: 'app-map-creator-positions-array-base',
    templateUrl: './map-creator-positions-array-base.component.html',
    styleUrls: ['./map-creator-positions-array-base.component.scss'],
})
export class MapCreatorPositionsArrayBaseComponent implements OnInit, OnDestroy {
    @Input() fC: FormControl;
    @Input() displayedColumns: string[];
    @Input() canAdd: boolean;
    @Input() type: MapCreatorPositionType;

    @Output() positionSelected = new EventEmitter<MapCreatorPosition>();
    @Output() positionPlacingStarted = new EventEmitter<string | number | undefined>();
    @Output() positionRemoved = new EventEmitter<MapCreatorPosition>();
    @Output() positionHold = new EventEmitter<MapCreatorPosition>();
    @Output() positionTeleported = new EventEmitter<MapCreatorPosition>();

    mapCreatorPositionType = MapCreatorPositionType;
    selectedPosition: MapCreatorPosition;
    dataSource: MapCreatorPosition[];

    private _destroySubject = new Subject();

    constructor(public readonly settings: SettingsService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.fC.valueChanges.pipe(takeUntil(this._destroySubject)).subscribe((arr: MapCreatorPosition[]) => {
            if (this.selectedPosition && !arr.indexOf(this.selectedPosition)) {
                this.selectPos(undefined);
            }
            this.dataSource = [...this.fC.value];
            this.changeDetector.detectChanges();
        });
        this.dataSource = this.fC.value;
    }

    ngOnDestroy() {
        this._destroySubject.next();
    }

    selectPos(pos?: MapCreatorPosition) {
        this.selectedPosition = pos;
        this.positionSelected.emit(pos);
    }

    addPos(info?: number | string) {
        this.positionPlacingStarted.emit(info);
    }

    removeSelectedPos() {
        const pos = this.selectedPosition;
        this.selectPos(undefined);

        const positions = this.fC.value as MapCreatorPosition[];
        const index = positions.indexOf(pos);
        if (index >= 0) {
            positions.splice(index, 1);
        }
        // need to create a new dataSource object, else table will not refresh
        this.dataSource = [...positions];
        this.positionRemoved.emit(pos);
    }

    holdSelectedPos() {
        this.positionHold.emit(this.selectedPosition);
    }

    tpToSelectedPos() {
        this.positionTeleported.emit(this.selectedPosition);
    }

    canModifyPos(pos: MapCreatorPosition) {
        return this.selectedPosition && (this.settings.IsLobbyOwner || this.isOwner(pos));
    }

    private isOwner(pos: MapCreatorPosition) {
        return pos[9] == this.settings.Constants[1];
    }
}
