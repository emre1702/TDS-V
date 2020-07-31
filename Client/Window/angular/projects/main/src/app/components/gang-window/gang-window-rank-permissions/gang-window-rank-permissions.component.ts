import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { GangPermissionKey } from './enums/gang-permission-key.enum';
import { SettingsService } from '../../../services/settings.service';
import { GangWindowService } from '../services/gang-window-service';
import { GangPermissionSettings } from './models/gang-permission-settings';
import { GangRank } from '../models/gang-rank';
import { GangPermissionData } from './models/gang-permission-data';
import { SafeStyle, DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'app-gang-window-rank-permissions',
    templateUrl: './gang-window-rank-permissions.component.html',
    styleUrls: ['./gang-window-rank-permissions.component.scss']
})
export class GangWindowRankPermissionsComponent implements OnInit {

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
                    index: GangPermissionKey.ManagePermission,
                    text: GangPermissionKey[GangPermissionKey.ManagePermission],
                    hint: GangPermissionKey[GangPermissionKey.ManagePermission] + "Hint",
                    value: 0
                }
            ]
        },

        {
            title: "Member", rows: [
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
                }
            ]
        },

        {
            title: "Action", rows: [
                {
                    index: GangPermissionKey.StartGangwar,
                    text: GangPermissionKey[GangPermissionKey.StartGangwar],
                    hint: GangPermissionKey[GangPermissionKey.StartGangwar] + "Hint",
                    value: 0
                }
            ]
        }
    ];

    gangRanks: GangRank[] = [];

    constructor(
        public settings: SettingsService,
        public changeDetector: ChangeDetectorRef,
        private gangWindowService: GangWindowService,
        private sanitizer: DomSanitizer
    ) { }

    ngOnInit(): void {
        const data: GangPermissionData = {
            0: [
                { 0: "NoobRank0", 1: "rgb(255, 255, 255)" },
                { 0: "NoobRank1", 1: "rgb(255, 255, 255)" },
                { 0: "NoobRank2", 1: "rgb(255, 255, 255)" },
                { 0: "NoobRank3", 1: "rgb(255, 255, 255)" },
                { 0: "Bonus' Rank", 1: "rgb(150, 0, 0)" },
            ],
            1: { 0: 3, 1: 2, 2: 4, 3: 2, 4: 5 }
        };
        this.loadPermissions(JSON.stringify(data));
    }

    loadPermissions(json: string) {
        const data: GangPermissionData = JSON.parse(json);
        for (const settingPanel of this.rankPermissions) {
            for (const setting of settingPanel.rows) {
                setting.value = data[1][setting.index];
            }
        }

        this.gangRanks = data[0];

        this.changeDetector.detectChanges();
    }

    save() {

    }

    getColor(color: string): SafeStyle {
        return this.sanitizer.bypassSecurityTrustStyle(color);
    }
}
