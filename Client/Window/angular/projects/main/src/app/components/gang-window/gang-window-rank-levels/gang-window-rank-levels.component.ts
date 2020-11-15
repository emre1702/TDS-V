import { Component, OnInit, ChangeDetectorRef, OnDestroy, Output, EventEmitter } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { GangRank } from '../models/gang-rank';
import { DomSanitizer } from '@angular/platform-browser';
import { GangWindowService } from '../services/gang-window-service';
import { GangWindowNav } from '../enums/gang-window-nav.enum';
import { GangCommand } from '../enums/gang-command.enum';
import { NotificationService } from '../../../modules/shared/services/notification.service';

@Component({
    selector: 'app-gang-window-rank-levels',
    templateUrl: './gang-window-rank-levels.component.html',
    styleUrls: ['./gang-window-rank-levels.component.scss']
})
export class GangWindowRankLevelsComponent implements OnInit, OnDestroy {

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

    @Output() back = new EventEmitter();

    constructor(
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private sanitizer: DomSanitizer,
        public gangWindowService: GangWindowService,
        private notificationService: NotificationService
    ) { }

    ngOnInit(): void {
        this.gangWindowService.loadedData.on(GangWindowNav[GangWindowNav.RanksLevels], this.loadedData.bind(this));
        this.notificationService.showInfo(this.settings.Lang.RankLevelsModifyInfo);
    }

    ngOnDestroy(): void {
        this.gangWindowService.loadedData.off(GangWindowNav[GangWindowNav.RanksLevels], this.loadedData.bind(this));
    }

    addRankAfter() {
        const formGroup = new FormGroup({
            Id: new FormControl(-1),
            Name: new FormControl("Rank " + (this.currentFormGroup + 1), this.nameFormControl.validator),
            Color: new FormControl("rgb(255,255,255)", this.colorFormControl.validator)
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
        const ranks: GangRank[] = [];

        for (const formGroup of this.rankLevelFormGroups) {
            ranks.push({ 0: formGroup.get("Name").value, 1: formGroup.get("Color").value, 2: formGroup.get("Id").value });
        }

        this.gangWindowService.executeCommand(GangCommand.ModifyRanks, [JSON.stringify(ranks)], () => {
            this.back.emit();
        });
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

    private loadedData() {
        this.currentFormGroup = undefined;
        this.rankLevelFormGroups = [];

        for (const rank of this.gangWindowService.ranks) {
            const formGroup = new FormGroup({
                Name: new FormControl(rank[0], this.nameFormControl.validator),
                Color: new FormControl(rank[1], this.colorFormControl.validator),
                Id: new FormControl(rank[2])
            });
            this.rankLevelFormGroups.push(formGroup);
        }

        this.changeDetector.detectChanges();
    }
}
