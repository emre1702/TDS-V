import { Component, OnInit, Input, ChangeDetectorRef, OnDestroy, HostListener } from '@angular/core';
import { WeaponHash } from '../lobbychoice/enums/weapon-hash.enum';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../../enums/from-client-event.enum';
import { SettingsService } from '../../services/settings.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-damage-test-menu',
    templateUrl: './damage-test-menu.component.html',
    styleUrls: ['./damage-test-menu.component.scss'],
})
export class DamageTestMenuComponent implements OnInit, OnDestroy {
    @Input() set initWeapon(value: WeaponHash) {
        this.setCurrentWeapon(value);
    }

    weaponFormGroup = new FormGroup({
        0: new FormControl('', [Validators.required]),
        1: new FormControl('', [Validators.required, Validators.min(0), Validators.max(100000)]),
        2: new FormControl('', [Validators.required, Validators.min(0), Validators.max(100000)]),
    });

    weaponHash = WeaponHash;
    currentWeapon: WeaponHash;

    constructor(private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService, public settings: SettingsService) {}

    ngOnInit() {
        this.rageConnector.listen(FromClientEvent.SetDamageTestMenuCurrentWeapon, this.setCurrentWeapon.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromClientEvent.SetDamageTestMenuCurrentWeapon, this.setCurrentWeapon.bind(this));
    }

    applyDamageSettings() {
        const weaponHash = this.weaponFormGroup.controls['0'].value;
        const weaponData = this.settings.DamageTestWeaponDatas.find((w) => w[0] == weaponHash);
        if (weaponData) {
            this.rageConnector.callServer(DToServerEvent.SetDamageTestWeaponDamage, JSON.stringify(this.weaponFormGroup.value));
        }
    }

    private setCurrentWeapon(weaponHash: WeaponHash) {
        this.currentWeapon = weaponHash;
        const weaponData = this.settings.DamageTestWeaponDatas.find((w) => w[0] == weaponHash);
        if (weaponData) {
            this.weaponFormGroup.controls['0'].setValue(weaponData[0]);
            this.weaponFormGroup.controls['1'].setValue(weaponData[1]);
            this.weaponFormGroup.controls['2'].setValue(weaponData[2]);
        } else {
            this.weaponFormGroup.controls['0'].setValue(weaponHash);
            this.weaponFormGroup.controls['1'].setValue(0);
            this.weaponFormGroup.controls['2'].setValue(0);
        }
        this.changeDetector.detectChanges();
    }

    @HostListener('document:keydown.tab', ['$event'])
    preventTab(e: KeyboardEvent) {
        e.preventDefault();
    }
}
