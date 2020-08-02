import { Component, OnInit, ChangeDetectorRef, ViewChild, ChangeDetectionStrategy, OnDestroy, Output, EventEmitter } from '@angular/core';
import { GangMember } from '../models/gang-member';
import { SettingsService } from '../../../services/settings.service';
import { MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { GangWindowService } from '../services/gang-window-service';
import { GangWindowNav } from '../enums/gang-window-nav.enum';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { GangCommand } from '../enums/gang-command.enum';

@Component({
    selector: 'app-gang-window-members',
    templateUrl: './gang-window-members.component.html',
    styleUrls: ['./gang-window-members.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class GangWindowMembersComponent implements OnInit, OnDestroy {

    columns = ["Name", "Rank", "JoinDate", "LastLoginDate", "IsOnline"];
    private sortIndexByColumnName = {
        PlayerId: 0,
        Name: 1,
        JoinDate: 6,
        LastLogin: 7,
        IsOnline: 4,
        Rank: 5
    };

    selectedMember: GangMember;
    canChangeUser: boolean;

    dataSource: MatTableDataSource<GangMember>;

    @Output() back = new EventEmitter();
    @ViewChild(MatSort, {static: true}) sort: MatSort;

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        public gangWindowService: GangWindowService,
        private dialog: MatDialog,
        private rageConnector: RageConnectorService) { }

    ngOnInit(): void {
        this.gangWindowService.loadedData.on(GangWindowNav[GangWindowNav.Members], this.dataLoaded.bind(this));

        this.dataSource = new MatTableDataSource(this.gangWindowService.members);
        this.sort.sort({ id: "IsOnline", start: "desc", disableClear: false });
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.changeDetector.detectChanges();
    }

    ngOnDestroy(): void {
        this.gangWindowService.loadedData.off(GangWindowNav[GangWindowNav.Members], this.dataLoaded.bind(this));
    }

    selectMember(member: GangMember) {
        if (this.selectedMember == member)
            this.selectedMember = undefined;
        else
            this.selectedMember = member;

        this.canChangeUser = this.selectedMember    // Have selected someone
                            && this.selectedMember[0] != this.gangWindowService.gangData[98]     // he is not the leader
                            && (this.gangWindowService.myGangData[5] || this.gangWindowService.myGangData[4] > member[5]);    // My rank is higher

        this.changeDetector.detectChanges();
    }

    giveRankUp() {
        if (!this.canGiveRankUp()) {
            return;
        }
        const member = this.selectedMember;
        this.gangWindowService.executeCommand(GangCommand.RankUp, [this.selectedMember[0]], () => {
            member[5]++;
            this.changeDetector.detectChanges();
        });
    }

    giveRankDown() {
        if (!this.canGiveRankDown()) {
            return;
        }
        const member = this.selectedMember;
        this.gangWindowService.executeCommand(GangCommand.RankDown, [this.selectedMember[0]], () => {
            member[5]--;
            this.changeDetector.detectChanges();
        });
    }

    kick() {
        if (!this.canKick()) {
            return;
        }
        const member = this.selectedMember;
        this.gangWindowService.executeCommand(GangCommand.Kick, [this.selectedMember[0]], () => {
            if (this.selectedMember == member) {
                this.selectedMember = undefined;
            }

            const index = this.gangWindowService.members.indexOf(member);
            if (index >= 0) {
                this.gangWindowService.members.splice(index, 1);
            }
            this.changeDetector.detectChanges();
        });
    }

    invite(name: string) {
        if (!this.canInvite()) {
            return;
        }
        this.gangWindowService.executeCommand(GangCommand.Invite, [name], () => {});
    }

    leaveGang() {
        this.gangWindowService.executeCommand(GangCommand.Leave, [], () => {
            this.settings.syncIsInGang(false);
            this.gangWindowService.clear();
            this.back.emit();
        });
    }

    canGiveRankUp() {
        return this.selectedMember[5] < this.gangWindowService.highestRank
            && (this.gangWindowService.myGangData[1]
            || (this.canChangeUser && this.gangWindowService.myGangData[0] >= this.gangWindowService.permissions[3]));
    }

    canGiveRankDown() {
        return this.selectedMember[5] > 0
            && (this.gangWindowService.myGangData[1]
            || (this.canChangeUser && this.gangWindowService.myGangData[0] >= this.gangWindowService.permissions[3]));
    }

    canKick() {
        return this.canChangeUser
            && (this.gangWindowService.myGangData[1] || this.gangWindowService.myGangData[0] >= this.gangWindowService.permissions[1]);
    }

    canInvite() {
        return this.gangWindowService.myGangData[1] || this.gangWindowService.myGangData[0] >= this.gangWindowService.permissions[0];
    }

    private sortingDataAccessor(member: GangMember, sortHeaderId: string): string | number {
        return member[this.sortIndexByColumnName[sortHeaderId]];
    }

    private dataLoaded() {
        this.dataSource.data = this.gangWindowService.members;
        this.changeDetector.detectChanges();
    }

}
