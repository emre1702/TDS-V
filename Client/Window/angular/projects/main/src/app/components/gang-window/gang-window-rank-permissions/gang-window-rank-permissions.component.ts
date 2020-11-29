import { Component, OnInit, ChangeDetectorRef, OnDestroy, Output, EventEmitter } from '@angular/core';
import { GangPermissionKey } from './enums/gang-permission-key.enum';
import { SettingsService } from '../../../services/settings.service';
import { GangWindowService } from '../services/gang-window-service';
import { SafeStyle, DomSanitizer } from '@angular/platform-browser';
import { GangWindowNav } from '../enums/gang-window-nav.enum';
import { GangPermissionSettings } from './models/gang-permission-settings';
import { GangCommand } from '../enums/gang-command.enum';

@Component({
    selector: 'app-gang-window-rank-permissions',
    templateUrl: './gang-window-rank-permissions.component.html',
    styleUrls: ['./gang-window-rank-permissions.component.scss']
})
export class GangWindowRankPermissionsComponent implements OnInit, OnDestroy {

    rankPermissions = [
        {
            title: "Administration", rows: [
                {
                    index: GangPermissionKey.ManageRanks,
                    text: GangPermissionKey[GangPermissionKey.ManageRanks],
                    hint: GangPermissionKey[GangPermissionKey.ManageRanks] + "Hint",
                    value: 0
                },
                {
                    index: GangPermissionKey.ManagePermissions,
                    text: GangPermissionKey[GangPermissionKey.ManagePermissions],
                    hint: GangPermissionKey[GangPermissionKey.ManagePermissions] + "Hint",
                    value: 0
                }
            ]
        },

        {
            title: "Members", rows: [
                {
                    index: GangPermissionKey.InviteMembers,
                    text: GangPermissionKey[GangPermissionKey.InviteMembers],
                    hint: GangPermissionKey[GangPermissionKey.InviteMembers] + "Hint",
                    value: 0
                },
                {
                    index: GangPermissionKey.KickMembers,
                    text: GangPermissionKey[GangPermissionKey.KickMembers],
                    hint: GangPermissionKey[GangPermissionKey.KickMembers] + "Hint",
                    value: 0
                },
                {
                    index: GangPermissionKey.SetRanks,
                    text: GangPermissionKey[GangPermissionKey.SetRanks],
                    hint: GangPermissionKey[GangPermissionKey.SetRanks] + "Hint",
                    value: 0
                },
            ]
        },

        {
            title: "Action", rows: [
                {
                    index: GangPermissionKey.StartGangAction,
                    text: GangPermissionKey[GangPermissionKey.StartGangAction],
                    hint: GangPermissionKey[GangPermissionKey.StartGangAction] + "Hint",
                    value: 0
                }
            ]
        }
    ];

    canEdit: boolean;

    @Output() back = new EventEmitter();

    constructor(
        public settings: SettingsService,
        public changeDetector: ChangeDetectorRef,
        public gangWindowService: GangWindowService,
        private sanitizer: DomSanitizer
    ) { }

    ngOnInit(): void {
        this.gangWindowService.loadedData.on(GangWindowNav[GangWindowNav.RanksPermissions], this.dataLoaded.bind(this));
    }

    ngOnDestroy(): void {
        this.gangWindowService.loadedData.off(GangWindowNav[GangWindowNav.RanksPermissions], this.dataLoaded.bind(this));
    }

    save() {
        const data = {};

        for (const topic of this.rankPermissions) {
            for (const row of topic.rows) {
                data[row.index] = row.value;
            }
        }

        this.gangWindowService.executeCommand(GangCommand.ModifyPermissions, [JSON.stringify(data)], () => {
            this.back.emit();
        });
    }

    getColor(color: string): SafeStyle {
        return this.sanitizer.bypassSecurityTrustStyle(color);
    }

    private dataLoaded() {
        this.canEdit = this.gangWindowService.myGangData[1] || this.gangWindowService.myGangData[0] >= this.gangWindowService.permissions[2];

        for (const settingPanel of this.rankPermissions) {
            for (const setting of settingPanel.rows) {
                setting.value = this.gangWindowService.permissions[setting.index];
            }
        }

        this.changeDetector.detectChanges();
    }
}
