import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnDestroy, OnInit } from '@angular/core';
import { CharCreateHairAndColorsData } from '../../interfaces/charCreateHairAndColorsData';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from 'projects/main/src/app/enums/dtoclientevent.enum';
import { CharCreatorDataKey } from '../../enums/charCreatorDataKey.enum';
import { MatSelectChange } from '@angular/material/select';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'char-creator-hairandcolors',
    templateUrl: './char-creator-hairandcolors.component.html',
    styleUrls: ['./char-creator-hairandcolors.component.scss']
})
export class CharCreatorHairandcolorsComponent implements OnInit, OnDestroy {

    @Input() data: CharCreateHairAndColorsData;
    @Output() dataChange = new EventEmitter<CharCreateHairAndColorsData>();

    @Output() recreate = new EventEmitter();

    @Input() isMale: boolean;

    hairs: { [0]: { Id: number, Name: string }[], [1]: { Id: number, Name: string }[] } = {
        // male
        [1]: [
            { Id: 0, Name: "Close Shave" },
            { Id: 1, Name: "Buzzcut" },
            { Id: 2, Name: "Faux Hawk" },
            { Id: 3, Name: "Hipster" },
            { Id: 4, Name: "Side Parting" },
            { Id: 5, Name: "Shorter Cut" },
            { Id: 6, Name: "Biker" },
            { Id: 7, Name: "Ponytail" },
            { Id: 8, Name: "Cornrows" },
            { Id: 9, Name: "Slicked" },
            { Id: 10, Name: "Short Brushed" },
            { Id: 11, Name: "Spikey" },
            { Id: 12, Name: "Caesar" },
            { Id: 13, Name: "Chopped" },
            { Id: 14, Name: "Dreads" },
            { Id: 15, Name: "Long Hair" },
            { Id: 16, Name: "Shaggy Curls" },
            { Id: 17, Name: "Surfer Dude" },
            { Id: 18, Name: "Short Side Part" },
            { Id: 19, Name: "High Slicked Sides" },
            { Id: 20, Name: "Long Slicked" },
            { Id: 21, Name: "Hipster Youth" },
            { Id: 22, Name: "Mullet" },
            { Id: 24, Name: "Classic Cornrows" },
            { Id: 25, Name: "Palm Cornrows" },
            { Id: 26, Name: "Lightning Cornrows" },
            { Id: 27, Name: "Whipped Cornrows" },
            { Id: 28, Name: "Zig Zag Cornrows" },
            { Id: 29, Name: "Snail Cornrows" },
            { Id: 30, Name: "Hightop" },
            { Id: 31, Name: "Loose Swept Back" },
            { Id: 32, Name: "Undercut Swept Back" },
            { Id: 33, Name: "Undercut Swept Side" },
            { Id: 34, Name: "Spiked Mohawk" },
            { Id: 35, Name: "Mod" },
            { Id: 36, Name: "Layered Mod" },
            { Id: 72, Name: "Flattop" },
            { Id: 73, Name: "Military Buzzcut" }
        ],

        // female
        [0]: [
            { Id: 0, Name: "Close Shave" },
            { Id: 1, Name: "Short" },
            { Id: 2, Name: "Layered Bob" },
            { Id: 3, Name: "Pigtails" },
            { Id: 4, Name: "Ponytail" },
            { Id: 5, Name: "Braided Mohawk" },
            { Id: 6, Name: "Braids" },
            { Id: 7, Name: "Bob" },
            { Id: 8, Name: "Faux Hawk" },
            { Id: 9, Name: "French Twist" },
            { Id: 10, Name: "Long Bob" },
            { Id: 11, Name: "Loose Tied" },
            { Id: 12, Name: "Pixie" },
            { Id: 13, Name: "Shaved Bangs" },
            { Id: 14, Name: "Top Knot" },
            { Id: 15, Name: "Wavy Bob" },
            { Id: 16, Name: "Messy Bun" },
            { Id: 17, Name: "Pin Up Girl" },
            { Id: 18, Name: "Tight Bun" },
            { Id: 19, Name: "Twisted Bob" },
            { Id: 20, Name: "Flapper Bob" },
            { Id: 21, Name: "Big Bangs" },
            { Id: 22, Name: "Braided Top Knot" },
            { Id: 23, Name: "Mullet" },
            { Id: 25, Name: "Pinched Cornrows" },
            { Id: 26, Name: "Leaf Cornrows" },
            { Id: 27, Name: "Zig Zag Cornrows" },
            { Id: 28, Name: "Pigtail Bangs" },
            { Id: 29, Name: "Wave Braids" },
            { Id: 30, Name: "Coil Braids" },
            { Id: 31, Name: "Rolled Quiff" },
            { Id: 32, Name: "Loose Swept Back" },
            { Id: 33, Name: "Undercut Swept Back" },
            { Id: 34, Name: "Undercut Swept Side" },
            { Id: 35, Name: "Spiked Mohawk" },
            { Id: 36, Name: "Bandana and Braid" },
            { Id: 37, Name: "Layered Mod" },
            { Id: 38, Name: "Skinbyrd" },
            { Id: 76, Name: "Neat Bun" },
            { Id: 77, Name: "Short Bob" }
        ]
    };

    eyeColors = [
        "Green", "Emerald", "Light Blue", "Ocean Blue", "Light Brown", "Dark Brown", "Hazel", "Dark Gray", "Light Gray", "Pink",
        "Yellow", "Purple", "Blackout", "Shades of Gray", "Tequila Sunrise", "Atomic", "Warp", "ECola", "Space Ranger", "Ying Yang",
        "Bullseye", "Lizard", "Dragon", "Extra Terrestrial", "Goat", "Smiley", "Possessed", "Demon", "Infected", "Alien", "Undead", "Zombie"
    ];
    hairColorsAmount = 64;
    blushColorsAmount = 27;
    lipstickColorsAmount = 32;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    onHairChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Hair, this.data[0]);
    }

    onHairColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.HairColor, this.data[1], this.data[2]);
    }

    onHairHighlightColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.HairColor, this.data[1], this.data[2]);
    }

    onEyebrowColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.EyebrowColor, this.data[3]);
    }

    onFacialHairColorColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.FacialHairColor, this.data[4]);
    }

    onEyeColorChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.EyeColor, this.data[5]);
    }

    onBlushColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.BlushColor, this.data[6]);
    }

    onLipstickColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.LipstickColor, this.data[7]);
    }

    onChestHairColorChanged(event: { srcElement: { value: string }}) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.ChestHairColor, this.data[8]);
    }

    randomize() {
        this.data[0] = this.getRandomHair();
        this.data[1] = this.getRandomHairColor();
        this.data[2] = this.getRandomHairColor();
        this.data[3] = this.getRandomHairColor();
        this.data[4] = this.getRandomHairColor();
        this.data[5] = this.getRandomEyeColor();
        this.data[6] = this.getRandomBlushColor();
        this.data[7] = this.getRandomLipstickColor();
        this.data[8] = this.getRandomHairColor();

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

    private getRandomHair() {
        return this.hairs[Number(this.isMale)][Math.floor(Math.random() * this.hairs[Number(this.isMale)].length)].Id;
    }

    private getRandomHairColor() {
        return Math.floor(Math.random() * this.hairColorsAmount);
    }

    private getRandomEyeColor() {
        return Math.floor(Math.random() * this.eyeColors.length);
    }

    private getRandomBlushColor() {
        return Math.floor(Math.random() * this.blushColorsAmount);
    }

    private getRandomLipstickColor() {
        return Math.floor(Math.random() * this.lipstickColorsAmount);
    }
}
