import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MapCreatorPositionType } from '../../enums/map-creator-position-type';
import { MapCreatorPosition } from '../../models/map-creator-position';

@Component({
    selector: 'app-map-creator-position-base',
    templateUrl: './map-creator-position-base.component.html',
    styleUrls: ['./map-creator-position-base.component.scss'],
})
export class MapCreatorPositionBaseComponent implements OnInit {
    @Input() info: string;
    @Input() fC: FormControl;
    @Output() positionSelected = new EventEmitter<MapCreatorPosition>();
    @Output() positionPlacingStarted = new EventEmitter<string | number | undefined>();
    @Output() positionRemoved = new EventEmitter<MapCreatorPosition>();
    @Output() positionHold = new EventEmitter<MapCreatorPosition>();
    @Output() positionTeleported = new EventEmitter<MapCreatorPosition>();

    mapCreatorPositionType = MapCreatorPositionType;

    private _destroySubject = new Subject();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit(): void {
        this.fC.valueChanges.pipe(takeUntil(this._destroySubject)).subscribe((value) => {
            this.changeDetector.detectChanges();
        });
    }

    ngOnDestroy() {
        this._destroySubject.next();
    }

    addOrChangePos() {
        const pos = this.getPos();
        if (pos) {
            this.positionHold.emit(pos);
        } else {
            this.positionPlacingStarted.emit();
        }
    }

    removePos() {
        const pos = this.getPos();
        if (!pos) return;

        this.fC.setValue(undefined);
        this.positionSelected.emit(undefined);
        this.positionRemoved.emit(pos);
    }

    tpToPos() {
        const pos = this.getPos();
        if (!pos) return;
        this.positionTeleported.emit(pos);
    }

    hasPos() {
        return !!this.getPos();
    }

    getPos() {
        return this.fC.value;
    }
}
