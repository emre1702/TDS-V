﻿import SettingsService from "../../services/settings/settings.service";
import game from "natives";
import alt from "alt-client";
import DxService from "../../services/draw/dx.service";
import { DxText } from "./dx/dx-text.entity";
import Alignment from "../../datas/enums/draw/alignment.enum";

export class FloatingDamageInfo {
    get removeAtHandler(): boolean {
        return this._removeAtHandler;
    }

    private text: DxText;
    private targetPos: alt.Vector3;

    private _removeAtHandler: boolean;
    private startMs: number;

    constructor(target: alt.Player, private damage: number, private settingsService: SettingsService, private dxService: DxService) {
        this.startMs = Date.now();
        this.targetPos = target.pos;
    }

    updatePosition(currentMs: number) {
        const elapsedMs = currentMs - this.startMs; 
        if (elapsedMs > this.settingsService.playerSettings.ShowFloatingDamageInfoDurationMs) {
            this._removeAtHandler = true;
            this.remove();
            return;
        }

        let [, screenX, screenY] = game.getScreenCoordFromWorldCoord(this.targetPos.x, this.targetPos.y, this.targetPos.z, 0, 0);
        const percentage = elapsedMs / this.settingsService.playerSettings.ShowFloatingDamageInfoDurationMs;
        screenY -= 0.2 * percentage;

        const scale = 0.4 - (0.3 * percentage);
        const color = new alt.RGBA(220, 220, 200, 255 - (255 * percentage));

        if (!this.text) {
            this.text = new DxText(this.dxService, String(this.damage), screenX, screenY, scale, color, undefined, Alignment.Center, Alignment.End, undefined, false, true);
        } else {
            this.text.setY(screenY, true);
        }
    }

    remove() {
        if (this.text) {
            this.text.remove();
            this.text = undefined;
        }
    }
}
