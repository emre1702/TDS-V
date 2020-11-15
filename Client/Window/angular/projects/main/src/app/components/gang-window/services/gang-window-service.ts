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
import { GangCommand } from '../enums/gang-command.enum';
import { AreYouSureDialog } from '../../../dialog/are-you-sure-dialog';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from '../../../modules/shared/services/notification.service';

@Injectable()
export class GangWindowService {

    loadingData = false;
    loadingDataChanged = new EventEmitter();
    loadedData = new EventEmitter();

    gangData: GangData;
    myGangData: MyGangData;
    permissions: GangPermissionSettings;
    ranks: GangRank[];
    highestRank: number;

    members: GangMember[];

    loadData(type: GangWindowNav) {
        switch (type) {
            case GangWindowNav.Create:
                return;
            case GangWindowNav.MainMenu:
                if (!this.settings.IsInGang) {
                    return;
                }
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

    executeCommand(command: GangCommand, args: any[], onSuccess: () => void, withConfirm: boolean = true, showSuccess: boolean = true) {
        if (withConfirm) {
            this.doOnConfirm(() => {
                this.executeCommand(command, args, onSuccess, false);
            });
        } else {
            this.rageConnector.callCallbackServer(DToServerEvent.GangCommand, [command, ...args], (msg: string) => {
                if (msg && msg.length) {
                    this.notificationService.showError(msg);
                    return;
                }
                if (showSuccess) {
                    this.notificationService.showSuccess(this.settings.Lang.CommandExecutedSuccessfully);
                }
                onSuccess();
            });
        }
    }

    private loadedGangWindowData(type: GangWindowNav, json: string) {
        json = json.escapeJson();

        if (json.length) {
            switch (type) {
                case GangWindowNav.Members:
                    this.members = JSON.parse(json);
                    break;
                case GangWindowNav.RanksLevels:
                    this.ranks = JSON.parse(json);
                    break;
                case GangWindowNav.RanksPermissions:
                    const permissionsData: { 0: GangPermissionSettings, 1: GangRank[] } = JSON.parse(json);
                    this.permissions = permissionsData[0];
                    this.ranks = permissionsData[1];
                    break;
                case GangWindowNav.MainMenu:
                    const data: { 0: GangData, 1: MyGangData, 2: number } = JSON.parse(json);
                    this.gangData = data[0];
                    this.myGangData = data[1];
                    this.highestRank = data[2];
                    break;
            }
        } else {
            this.notificationService.showError(this.settings.Lang.LoadingDataFailed);
        }

        this.loadedData.emit(GangWindowNav[type]);
        this.loadingData = false;
        this.loadingDataChanged.emit(null);
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
        private notificationService: NotificationService,
        private dialog: MatDialog
    ) {
        rageConnector.listen(DFromServerEvent.LoadedGangWindowData, this.loadedGangWindowData.bind(this));
    }
}
