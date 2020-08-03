import { Injectable } from '@angular/core';
import { GangMember } from '../models/gang-member';
import { MyGangData } from '../models/my-gang-permissions';
import { GangPermissionSettings } from '../gang-window-rank-permissions/models/gang-permission-settings';
import { RageConnectorService } from 'rage-connector';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';
import { GangWindowNav } from '../enums/gang-window-nav.enum';
import { EventEmitter } from 'events';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { GangData } from '../models/gang-data';
import { SettingsService } from '../../../services/settings.service';
import { GangRank } from '../models/gang-rank';
import { MatSnackBar, MatDialog } from '@angular/material';
import { GangCommand } from '../enums/gang-command.enum';
import { AreYouSureDialog } from '../../../dialog/are-you-sure-dialog';

@Injectable()
export class GangWindowService {

    loadingData = false;
    loadingDataChanged = new EventEmitter();
    loadedData = new EventEmitter();

    gangData: GangData;
    myGangData: MyGangData;
    permissions: GangPermissionSettings;
    ranks: GangRank[];      // Todo: Get for ranks
    highestRank: number;    // Todo: Get for members

    members: GangMember[];

    loadData(type: GangWindowNav) {
        switch (type) {
            case GangWindowNav.Create:
                return;
        }

        this.loadingData = true;
        this.loadingDataChanged.emit(null);
        this.rageConnector.callServer(DToServerEvent.LoadGangWindowData, type);
    }

    clear() {
        if (this.loadingData) {
            this.loadingData = false;
            this.loadingDataChanged.emit(null);
        }
        this.gangData = undefined;
        this.myGangData = undefined;
        this.permissions = undefined;
        this.members = undefined;
    }

    showSuccess(msg: string) {
        this.snackBar.open(msg, "OK", { duration: 5000, panelClass: "mat-app-background" });
    }

    showError(msg: string) {
        this.snackBar.open(msg, "OK", { duration: undefined, panelClass: "mat-app-background" });
    }

    showInfo(msg: string) {
        this.snackBar.open(msg, "OK", { duration: undefined, panelClass: "mat-app-background" });
    }

    executeCommand(command: GangCommand, args: any[], onSuccess: () => void, withConfirm: boolean = true, showSuccess: boolean = true) {
        if (withConfirm) {
            this.doOnConfirm(() => {
                this.executeCommand(command, args, onSuccess, false);
            });
        } else {
            this.rageConnector.callCallbackServer(DToServerEvent.GangCommand, [command, ...args], (msg: string) => {
                if (msg && msg.length) {
                    this.showError(msg);
                    return;
                }
                if (showSuccess) {
                    this.showSuccess(this.settings.Lang.CommandExecutedSuccessfully);
                }
                onSuccess();
            });
        }
    }

    private loadedGangWindowData(type: GangWindowNav, json: string) {
        json = this.escapeSpecialChars(json);

        switch (type) {
            case GangWindowNav.Members:
                this.members = JSON.parse(json);
                break;
            case GangWindowNav.RanksLevels:
                this.ranks = JSON.parse(json);
                break;
            case GangWindowNav.RanksPermissions:
                this.permissions = JSON.parse(json);
                break;
            case GangWindowNav.MainMenu:
                const data: { 0: GangData, 1: MyGangData, 2: number } = JSON.parse(json);
                this.gangData = data[0];
                this.myGangData = data[1];
                this.highestRank = data[2];
                break;
        }

        this.loadedData.emit(GangWindowNav[type]);
        this.loadingData = false;
        this.loadingDataChanged.emit(null);
    }

    private escapeSpecialChars(json: string) {
        return json.replace(/\n/g, "\\n")
            .replace(/\r/g, "\\r")
            .replace(/\t/g, "\\t")
            .replace(/\f/g, "\\f");
    }

    private doOnConfirm(func: () => void) {
        this.dialog.open(AreYouSureDialog, { panelClass: "mat-app-background" })
            .afterClosed()
            .subscribe((bool: boolean) => {
                if (!bool) {
                    return;
                }
                func();
            });
    }


    constructor(
        private rageConnector: RageConnectorService,
        private settings: SettingsService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog
    ) {
        rageConnector.listen(DFromServerEvent.LoadedGangWindowData, this.loadedGangWindowData.bind(this));
    }
}
