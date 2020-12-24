import { Component, OnInit, EventEmitter, Input, Output, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CharCreateAppearanceData } from '../../interfaces/charCreateAppearanceData';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { CharCreatorDataKey } from '../../enums/charCreatorDataKey.enum';
import { MatSelectChange } from '@angular/material/select';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'char-creator-appearance',
    templateUrl: './char-creator-appearance.component.html',
    styleUrls: ['./char-creator-appearance.component.scss'],
})
export class CharCreatorAppearanceComponent implements OnInit, OnDestroy {
    @Input() data: CharCreateAppearanceData;
    @Output() dataChange = new EventEmitter<CharCreateAppearanceData>();

    @Output() recreate = new EventEmitter();

    appearances = [
        {
            Name: 'Blemishes',
            Items: [
                'None',
                'Measles',
                'Pimples',
                'Spots',
                'Break Out',
                'Blackheads',
                'Build Up',
                'Pustules',
                'Zits',
                'Full Acne',
                'Acne',
                'Cheek Rash',
                'Face Rash',
                'Picker',
                'Puberty',
                'Eyesore',
                'Chin Rash',
                'Two Face',
                'T Zone',
                'Greasy',
                'Marked',
                'Acne Scarring',
                'Full Acne Scarring',
                'Cold Sores',
                'Impetigo',
            ],
        },
        {
            Name: 'Facial Hair',
            Items: [
                'None',
                'Light Stubble',
                'Balbo',
                'Circle Beard',
                'Goatee',
                'Chin',
                'Chin Fuzz',
                'Pencil Chin Strap',
                'Scruffy',
                'Musketeer',
                'Mustache',
                'Trimmed Beard',
                'Stubble',
                'Thin Circle Beard',
                'Horseshoe',
                "Pencil and 'Chops",
                'Chin Strap Beard',
                'Balbo and Sideburns',
                'Mutton Chops',
                'Scruffy Beard',
                'Curly',
                'Curly & Deep Stranger',
                'Handlebar',
                'Faustic',
                'Otto & Patch',
                'Otto & Full Stranger',
                'Light Franz',
                'The Hampstead',
                'The Ambrose',
                'Lincoln Curtain',
            ],
        },
        {
            Name: 'Eyebrows',
            Items: [
                'None',
                'Balanced',
                'Fashion',
                'Cleopatra',
                'Quizzical',
                'Femme',
                'Seductive',
                'Pinched',
                'Chola',
                'Triomphe',
                'Carefree',
                'Curvaceous',
                'Rodent',
                'Double Tram',
                'Thin',
                'Penciled',
                'Mother Plucker',
                'Straight and Narrow',
                'Natural',
                'Fuzzy',
                'Unkempt',
                'Caterpillar',
                'Regular',
                'Mediterranean',
                'Groomed',
                'Bushels',
                'Feathered',
                'Prickly',
                'Monobrow',
                'Winged',
                'Triple Tram',
                'Arched Tram',
                'Cutouts',
                'Fade Away',
                'Solo Tram',
            ],
        },
        {
            Name: 'Ageing',
            Items: [
                'None',
                "Crow's Feet",
                'First Signs',
                'Middle Aged',
                'Worry Lines',
                'Depression',
                'Distinguished',
                'Aged',
                'Weathered',
                'Wrinkled',
                'Sagging',
                'Tough Life',
                'Vintage',
                'Retired',
                'Junkie',
                'Geriatric',
            ],
        },
        {
            Name: 'Makeup',
            Items: [
                'None',
                'Smoky Black',
                'Bronze',
                'Soft Gray',
                'Retro Glam',
                'Natural Look',
                'Cat Eyes',
                'Chola',
                'Vamp',
                'Vinewood Glamour',
                'Bubblegum',
                'Aqua Dream',
                'Pin Up',
                'Purple Passion',
                'Smoky Cat Eye',
                'Smoldering Ruby',
                'Pop Princess',
                'Kiss My Axe',
                'Panda Pussy',
                'The Bat',
                'Skull in Scarlet',
                'Serpentine',
                'The Veldt',
                'Tribal Lines',
                'Tribal Swirls',
                'Tribal Orange',
                'Tribal Red',
                'Trapped in a Box',
                'Clowning',
                'Guyliner',
                'Stars n Stripes',
                'Blood Tears',
                'Heavy Metal',
                'Sorrow',
                'Prince of Darkness',
                'Rocker',
                'Goth',
                'Devasted',
                'Shadow Demon',
                'Fleshy Demon',
                'Flayed Demon',
                'Sorrow Demon',
                'Smiler Demon',
                'Cracked Demon',
                'Danger Skull',
                'Wicked Skull',
                'Menace Skull',
                'Bone Jaw Skull',
                'Flesh Jaw Skull',
                'Spirit Skull',
                'Ghoul Skull',
                'Phantom Skull',
                'Gnasher Skull',
                'Exposed Skull',
                'Ghostly Skull',
                'Fury Skull',
                'Demi Skull',
                'Inbred Skull',
                'Spooky Skull',
                'Slashed Skull',
                'Web Sugar Skull',
                'Señor Sugar Skull',
                'Swirl Sugar Skull',
                'Floral Sugar Skull',
                'Mono Sugar Skull',
                'Femme Sugar Skull',
                'Demi Sugar Skull',
                'Scarred Sugar Skull',
            ],
        },
        { Name: 'Blush', Items: ['None', 'Full', 'Angled', 'Round', 'Horizontal', 'High', 'Sweetheart', 'Eighties'] },
        {
            Name: 'Complexion',
            Items: [
                'None',
                'Rosy Cheeks',
                'Stubble Rash',
                'Hot Flush',
                'Sunburn',
                'Bruised',
                'Alchoholic',
                'Patchy',
                'Totem',
                'Blood Vessels',
                'Damaged',
                'Pale',
                'Ghostly',
            ],
        },
        {
            Name: 'Sun Damage',
            Items: ['None', 'Uneven', 'Sandpaper', 'Patchy', 'Rough', 'Leathery', 'Textured', 'Coarse', 'Rugged', 'Creased', 'Cracked', 'Gritty'],
        },
        {
            Name: 'Lipstick',
            Items: [
                'None',
                'Color Matte',
                'Color Gloss',
                'Lined Matte',
                'Lined Gloss',
                'Heavy Lined Matte',
                'Heavy Lined Gloss',
                'Lined Nude Matte',
                'Liner Nude Gloss',
                'Smudged',
                'Geisha',
            ],
        },
        {
            Name: 'Moles & Freckles',
            Items: [
                'None',
                'Cherub',
                'All Over',
                'Irregular',
                'Dot Dash',
                'Over the Bridge',
                'Baby Doll',
                'Pixie',
                'Sun Kissed',
                'Beauty Marks',
                'Line Up',
                'Modelesque',
                'Occasional',
                'Speckled',
                'Rain Drops',
                'Double Dip',
                'One Sided',
                'Pairs',
                'Growth',
            ],
        },
        {
            Name: 'Chest Hair',
            Items: [
                'None',
                'Natural',
                'The Strip',
                'The Tree',
                'Hairy',
                'Grisly',
                'Ape',
                'Groomed Ape',
                'Bikini',
                'Lightning Bolt',
                'Reverse Lightning',
                'Love Heart',
                'Chestache',
                'Happy Face',
                'Skull',
                'Snail Trail',
                'Slug and Nips',
                'Hairy Arms',
            ],
        },
        {
            Name: 'Body Blemishes',
            Items: [
                'None',
                'Unknown1',
                'Unknown2',
                'Unknown3',
                'Unknown4',
                'Unknown5',
                'Unknown6',
                'Unknown7',
                'Unknown8',
                'Unknown9',
                'Unknown10',
                'Unknown11',
            ],
        },
        { Name: 'Add Body Blemishes', Items: ['No', 'Yes'] },
    ];

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    onAppearanceDataChanged(overlayId: number, dataIndex: number, event: MatSelectChange) {
        // dataIndex % 2 == 0  =>  Item changed
        if (dataIndex % 2 == 0) {
            this.rageConnector.call(ToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Appearance, overlayId, event.value, this.data[dataIndex + 1]);

            // dataIndex % 2 == 1  =>  Item opacity changed
        } else {
            this.rageConnector.call(ToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Appearance, overlayId, this.data[dataIndex - 1], event.value);
        }
    }

    randomize() {
        for (let i = 0; i < this.appearances.length; ++i) {
            this.data[i * 2] = this.getRandomAppearanceItemIndex(i);
            this.data[i * 2 + 1] = this.getRandomPercentage();
        }

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

    getPercentage(value: number) {
        return Math.round(value * 100) + '%';
    }

    private getRandomAppearanceItemIndex(appearanceIndex: number) {
        return Math.floor(Math.random() * this.appearances[appearanceIndex].Items.length);
    }

    private getRandomPercentage() {
        return Math.floor(Math.random() * (100 + 1)) / 100;
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
