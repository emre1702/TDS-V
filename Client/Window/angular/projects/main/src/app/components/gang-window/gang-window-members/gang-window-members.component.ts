import { Component, OnInit, ChangeDetectorRef, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { GangMember } from '../models/gang-member';
import { SettingsService } from '../../../services/settings.service';
import { MatTableDataSource, MatSort } from '@angular/material';
import { GangWindowService } from '../services/gang-window-service';

@Component({
    selector: 'app-gang-window-members',
    templateUrl: './gang-window-members.component.html',
    styleUrls: ['./gang-window-members.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class GangWindowMembersComponent implements OnInit {

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

    @ViewChild(MatSort, {static: true}) sort: MatSort;

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        public gangWindowService: GangWindowService) { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.gangWindowService.members);
        this.sort.sort({ id: "IsOnline", start: "desc", disableClear: false });
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.changeDetector.detectChanges();
    }

    selectMember(member: GangMember) {
        if (this.selectedMember == member)
            this.selectedMember = undefined;
        else
            this.selectedMember = member;

        this.canChangeUser = this.selectedMember    // Have selected someone
                            && this.selectedMember[0] != this.gangWindowService.gangData[98]     // he is not the leader
                            && (this.gangWindowService.permissions[5] || this.gangWindowService.permissions[4] > member[5]);    // My rank is higher

        this.changeDetector.detectChanges();
    }

    private sortingDataAccessor(member: GangMember, sortHeaderId: string): string | number {
        return member[this.sortIndexByColumnName[sortHeaderId]];
    }

}
