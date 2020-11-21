import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { EnumSettingRow } from 'projects/main/src/app/modules/settings/models/enum-setting-row';
import { ScoreboardPlayerSorting } from '../../../enums/scoreboard-player-sorting';
import { SettingsScoreboardIndex } from '../../enums/settings-scoreboard-index.enum';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { TimeSpanUnitsOfTime } from '../../../enums/timespan-units-of-time.enum';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsScoreboard } from '../../interfaces/userpanel-settings-scoreboard';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';

@Component({
    selector: 'app-userpanel-settings-scoreboard',
    templateUrl: './userpanel-settings-scoreboard.component.html'
})
export class UserpanelSettingsScoreboardComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new EnumSettingRow(ScoreboardPlayerSorting, { 
            defaultValue: ScoreboardPlayerSorting.Name, dataSettingIndex: SettingsScoreboardIndex.ScoreboardPlayerSorting,
            label: SettingsScoreboardIndex[SettingsScoreboardIndex.ScoreboardPlayerSorting], 
            tooltipLangKey: "ScoreboardPlayerSortingInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: false, dataSettingIndex: SettingsScoreboardIndex.ScoreboardPlayerSortingDesc, 
            label: SettingsScoreboardIndex[SettingsScoreboardIndex.ScoreboardPlayerSortingDesc], 
            tooltipLangKey: "ScoreboardPlayerSortingDescInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        }),
        new EnumSettingRow(TimeSpanUnitsOfTime, { 
            defaultValue: TimeSpanUnitsOfTime.HourMinute, dataSettingIndex: SettingsScoreboardIndex.ScoreboardPlaytimeUnit,
            label: SettingsScoreboardIndex[SettingsScoreboardIndex.ScoreboardPlaytimeUnit], 
            tooltipLangKey: "ScoreboardPlaytimeUnitInfo",
            containerGetter: this.getSettingsContainer.bind(this)
        })
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsScoreboard) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Scoreboard, obj).subscribe();
    }

    private getSettingsContainer(): UserpanelSettingsScoreboard {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Scoreboard] as UserpanelSettingsScoreboard;
    }
}   