import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { Observable } from 'rxjs';
import { ClothesDataKey } from '../../enums/clothes-config-key.enum';
import { ClothesService } from '../../services/clothes.service';

@Component({
    selector: 'app-clothes-content',
    templateUrl: './clothes-content.component.html',
    styleUrls: ['./clothes-content.component.scss'],
})
export class ClothesContentComponent implements OnInit {
    @Input() key: ClothesDataKey;
    @Input() drawableId: number;
    @Output() drawableIdChange = new EventEmitter<number>();
    @Input() textureId: number;
    @Output() textureIdChange = new EventEmitter<number>();

    drawableAmount$: Observable<number>;
    textureAmount$: Observable<number>;

    private _initialDrawableId: number;
    private _initialTextureId: number;

    constructor(private service: ClothesService, public settings: SettingsService, private rageConnector: RageConnectorService) {}

    ngOnInit(): void {
        this.drawableAmount$ = this.service.getDrawableAmount(this.key);
        this.textureAmount$ = this.service.getTextureAmount(this.key, this.drawableId);
    }

    revert() {
        this.drawableId = this._initialDrawableId;
        this.textureId = this._initialTextureId;
        this.textureAmount$ = this.service.getTextureAmount(this.key, this.drawableId);
    }

    drawableIdChanged(value: number) {
        this.drawableId = value;
        this.drawableIdChange.emit(value);
        this.textureAmount$ = this.service.getTextureAmount(this.key, value);
        this.rageConnector.call(ToClientEvent.ClothesDataChanged, this.key, this.drawableId, this.textureId);
    }

    textureIdChanged(value: number) {
        this.textureId = value;
        this.textureIdChange.emit(value);
        this.rageConnector.call(ToClientEvent.ClothesDataChanged, this.key, this.drawableId, this.textureId);
    }
}
