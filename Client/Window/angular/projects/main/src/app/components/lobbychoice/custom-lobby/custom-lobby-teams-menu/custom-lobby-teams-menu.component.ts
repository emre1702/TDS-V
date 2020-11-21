import { Component, Output, EventEmitter, Input, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { CustomLobbyTeamData } from '../../models/custom-lobby-team-data';
import { BlipColor } from '../../models/blip-color';
import { DomSanitizer } from '@angular/platform-browser';
import { PedSkins } from '../../enums/ped-skins';
import { Constants } from 'projects/main/src/app/constants';

@Component({
    selector: 'app-custom-lobby-teams-menu',
    templateUrl: './custom-lobby-teams-menu.component.html',
    styleUrls: ['./custom-lobby-teams-menu.component.scss']
})
export class CustomLobbyTeamsMenuComponent {

    displayedColumns = ["Index", "Name", "Color", "BlipColor", "SkinHash"];

    private defaultTeamData = [
        { name: undefined, color: "rgb(255, 255, 255)", blipColor: 4, skinHash: 0 },
        { name: "SWAT", color: "rgb(0, 150, 0)", blipColor: 52, skinHash: -1920001264 },
        { name: "Terrorists", color: "rgb(150, 0, 0)", blipColor: 1, skinHash: 275618457 },
        { name: "Gang", color: "rgb(0, 0, 150)", blipColor: 38, skinHash: -1057787465 }
    ];

    pedSkins = PedSkins;
    objectKeys = Object.keys;
    constants = Constants;

    showBlipColorWindow = false;
    private editingTeam: CustomLobbyTeamData;

    @Input() teams: CustomLobbyTeamData[];
    @Input() creating: boolean;
    @Output() backClicked = new EventEmitter();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    backButtonClicked() {

        this.backClicked.emit(this.teams);
    }

    toggleBlipColorWindow(team: CustomLobbyTeamData) {
        this.editingTeam = team;
        this.showBlipColorWindow = !this.showBlipColorWindow;
        this.changeDetector.detectChanges();
    }

    getBlipColorByID(id: number) {
        return Constants.BLIP_COLORS.find(b => b.ID == id).Color;
    }

    clickedOnBlipColor(blip: BlipColor) {
        this.editingTeam[2] = blip.ID;
        this.showBlipColorWindow = false;
        this.changeDetector.detectChanges();
    }

    addTeam() {
        const defaultData = this.defaultTeamData.length > this.teams.length ? this.defaultTeamData[this.teams.length] : this.defaultTeamData[0];

        this.teams = [...this.teams, {
            [0]: defaultData.name || "Team" + this.teams.length,
            [1]: defaultData.color,
            [2]: defaultData.blipColor,
            [3]: defaultData.skinHash,
            [4]: false
        }];
        this.changeDetector.detectChanges();
    }

    removeLastTeam() {
        if (this.teams.length <= 1)
            return;
        this.teams.splice(this.teams.length - 1, 1);
        this.teams = [...this.teams];
        this.changeDetector.detectChanges();
    }

    getPedSkinKeys() {
        const keys = Object.keys(PedSkins);
        return keys.sort((a, b) => a.localeCompare(b)).slice(keys.length / 2);
    }

    isValid(): boolean {
        if (this.teams.length <= 1)
            return false;
        for (const team of this.teams) {
            if (team[0].length < 1 || team[0].length > 25)
                return false;
        }
        return true;
    }
}
