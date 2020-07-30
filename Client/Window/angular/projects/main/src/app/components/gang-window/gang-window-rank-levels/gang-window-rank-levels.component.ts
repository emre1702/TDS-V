import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { GangRank } from './models/gang-rank';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'app-gang-window-rank-levels',
    templateUrl: './gang-window-rank-levels.component.html',
    styleUrls: ['./gang-window-rank-levels.component.scss']
})
export class GangWindowRankLevelsComponent implements OnInit {

    currentFormGroup: number;
    showColorPickerForColor: boolean;

    rankLevelFormGroups: FormGroup[] = [];

    nameFormControl = new FormControl("", [
        Validators.minLength(1),
        Validators.maxLength(25),
        Validators.required
    ]);

    colorFormControl = new FormControl("rgb(255,255,255)", [
        Validators.required,
        Validators.pattern("rgb\\((\\d{1,3}), ?(\\d{1,3}), ?(\\d{1,3})\\)")
    ]);

    columns = ["Rank", "Name", "Color"];

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private sanitizer: DomSanitizer
    ) { }

    ngOnInit(): void {
        // Debug:
        const ranks: GangRank[] = [
            { 0: "Rank 0", 1: "rgb(255,255,255)" },
            { 0: "Rank 1", 1: "rgb(220,220,220)" },
            { 0: "Rank 2", 1: "rgb(0,150,0)" },
            { 0: "Rank 3", 1: "rgb(150,0,0)" },
        ];
        this.loadRanks(JSON.stringify(ranks));
    }

    loadRanks(json: string) {
        const ranks: GangRank[] = JSON.parse(json);
        this.rankLevelFormGroups = [];

        for (const rank of ranks) {
            const formGroup = new FormGroup({
                Name: new FormControl(rank[0], this.nameFormControl.validator),
                Color: new FormControl(rank[1], this.colorFormControl.validator),
            });
            this.rankLevelFormGroups.push(formGroup);
        }

        this.changeDetector.detectChanges();
    }

    addRankAfter() {
        const formGroup = new FormGroup({
            Name: new FormControl("Rank " + (this.currentFormGroup + 1), this.nameFormControl.validator),
            Color: new FormControl("rgb(255,255,255)", this.colorFormControl.validator),
        });
        this.rankLevelFormGroups.splice(this.currentFormGroup + 1, 0, formGroup);
        this.rankLevelFormGroups = [...this.rankLevelFormGroups];
        this.changeDetector.detectChanges();
    }

    deleteRank() {
        this.rankLevelFormGroups.splice(this.currentFormGroup, 1);
        if (this.currentFormGroup >= this.rankLevelFormGroups.length) {
            this.currentFormGroup = this.rankLevelFormGroups.length - 1;
        }
        this.rankLevelFormGroups = [...this.rankLevelFormGroups];
        this.changeDetector.detectChanges();
    }

    saveRanks() {

    }

    selectFormGroup(formGroup: FormGroup) {
        const rank = this.rankLevelFormGroups.indexOf(formGroup);
        if (rank == this.currentFormGroup) {
            this.currentFormGroup = undefined;
        } else {
            this.currentFormGroup = rank;

            this.nameFormControl = formGroup.get("Name") as FormControl;
            this.colorFormControl = formGroup.get("Color") as FormControl;
        }

        this.changeDetector.detectChanges();
    }

    isValid(): boolean {
        for (const formGroup of this.rankLevelFormGroups) {
            if (formGroup.invalid)
                return false;
        }
        return true;
    }

    getColor(rgb: string) {
        return this.sanitizer.bypassSecurityTrustStyle(rgb);
    }

    getErrorMessage(control: AbstractControl) {
        for (const error in control.errors) {
            return error;
        }
    }
}
