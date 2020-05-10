import { Component, OnInit, ChangeDetectorRef, Output, Input, ChangeDetectionStrategy, EventEmitter, ViewChild } from '@angular/core';
import { CustomLobbyWeaponData } from '../../models/custom-lobby-weapon-data';
import { WeaponHashGroupConstants } from '../../enums/weapon-hash-group.constants';
import { WeaponHash } from '../../enums/weapon-hash.enum';
import { WeaponType } from '../../enums/weapon-type.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { MatTable, MatTableDataSource, MatSort } from '@angular/material';

@Component({
    selector: 'app-custom-lobby-weapons-menu',
    templateUrl: './custom-lobby-weapons-menu.component.html',
    styleUrls: ['./custom-lobby-weapons-menu.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomLobbyWeaponsMenuComponent implements OnInit {

    @Input() creating: boolean;
    @Input() selectedWeapons: CustomLobbyWeaponData[];
    @Input() allWeapons: CustomLobbyWeaponData[];
    @Output() backClicked = new EventEmitter<CustomLobbyWeaponData[]>();

    @ViewChild("selectedWeaponsSort", { static: true }) selectedWeaponsSort: MatSort;

    selectedWeaponsDataSource: MatTableDataSource<CustomLobbyWeaponData>;
    weaponHash = WeaponHash;
    weaponHashGroups: [string, [string, WeaponHash, boolean][]][] = [];
    selectedWeaponsTableColumns = ['Name', 'Ammo', 'Damage', 'HeadMultiplicator', 'Delete'];

    constructor(
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService
    ) { }

    ngOnInit() {
        for (const weaponType in WeaponHashGroupConstants.data) {
            let arr: [string, WeaponHash, boolean][] = [];
            for (const weaponName of WeaponHashGroupConstants.data[weaponType]) {
                const weaponHash = WeaponHash[weaponName as string] as WeaponHash;
                if (!weaponHash) {
                    continue;
                }
                const weapon = this.allWeapons.find(w => w[0] == weaponHash);
                if (!weapon) {
                    continue;
                }

                let selected = false;
                const selectedWeapon = this.selectedWeapons.find(w => w[0] == weaponHash);
                if (selectedWeapon) {
                    selected = true;
                }
                arr.push([weaponName, weaponHash, selected]);
            }
            arr = arr.sort((a, b) => a[0].localeCompare(b[0]));
            this.weaponHashGroups.push([WeaponType[weaponType as unknown as number], arr]);
        }

        this.selectedWeaponsDataSource = new MatTableDataSource(this.selectedWeapons);
        this.selectedWeaponsDataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.selectedWeaponsDataSource.sort = this.selectedWeaponsSort;

        this.changeDetector.detectChanges();
    }

    backButtonClicked() {
        if (this.selectedWeapons) {
            for (const weapon of this.selectedWeapons) {
                weapon[1] = Math.floor(weapon[1]);  // Only allow integer for ammo
            }
        }

        this.backClicked.emit(this.selectedWeapons);
    }

    clearButtonClicked() {
        for (const group of this.weaponHashGroups) {
            for (const weapon of group[1]) {
                weapon[2] = false;
            }
        }
        this.selectedWeapons = [];
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        this.changeDetector.detectChanges();
    }

    selectWeapon(weaponData: [string, WeaponHash, boolean]) {
        if (weaponData[2]) {
            return;
        }

        const weapon = this.allWeapons.find(w => w[0] == weaponData[1]);
        if (!weapon) {
            return;
        }

        weaponData[2] = true;
        this.selectedWeapons.push(Object.assign({}, weapon));
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        this.changeDetector.detectChanges();
    }

    unselectWeapon(weapon: CustomLobbyWeaponData) {
        const index = this.selectedWeapons.indexOf(weapon);
        this.selectedWeapons.splice(index, 1);
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        try {
            const weaponGroup = this.weaponHashGroups.find(g => g[1].find(w => w[1] == weapon[0]));
            const weaponData = weaponGroup[1].find(w => w[1] == weapon[0]);

            weaponData[2] = false;
        } catch {}

        this.changeDetector.detectChanges();
    }

    sortingDataAccessor(obj: CustomLobbyWeaponData, property: any) {
        const index = this.selectedWeaponsTableColumns.indexOf(property);
        if (index == 0) {
            return WeaponHash[obj[index]];
        }
        return obj[index];
    }
}
