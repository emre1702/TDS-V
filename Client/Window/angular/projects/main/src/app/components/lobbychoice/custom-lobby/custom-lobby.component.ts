import { Component, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { SettingType } from '../../../enums/setting-type';
import { LobbySettingPanel } from '../models/lobby-setting-panel';
import { FormControl, Validators } from '@angular/forms';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { CustomLobbyData } from '../models/custom-lobby-data';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { CustomLobbyPasswordDialog } from '../dialog/custom-lobby-password-dialog';
import { LobbyMapLimitType } from '../enums/lobby-map-limit-type';
import { CustomLobbyTeamData } from '../models/custom-lobby-team-data';
import { LobbySetting } from '../enums/lobby-setting.enum';
import { Constants } from '../../../constants';
import { CustomLobbyMenuType } from '../enums/custom-lobby-menu-type.enum';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';
import { DataForCustomLobbyCreation } from '../models/data-for-custom-lobby-creation';
import { CustomLobbyWeaponData } from '../models/custom-lobby-weapon-data';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';
import { notEnoughTeamsValidator } from './validators/notEnoughTeamsValidator';
import { ErrorService, CustomErrorCheck, FormControlCheck } from '../../../modules/shared/services/error.service';
import { CustomLobbyArmsRaceWeaponData } from '../models/custom-lobby-armsraceweapon-data';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from '../../../modules/shared/services/notification.service';

@Component({
    selector: 'app-custom-lobby',
    templateUrl: './custom-lobby.component.html',
    styleUrls: ['./custom-lobby.component.scss'],
    providers: [ErrorService],
    animations: [
        trigger('lobbyShowHideAnimation', [
            transition('* => *', [
                query(
                    ':enter',
                    [
                        style({ transform: 'translateY(100vh)', opacity: 0 }),
                        stagger(80, [animate('800ms ease-in-out', style({ transform: 'translateY(0)', opacity: 1 }))]),
                    ],
                    { optional: true }
                ),
                query(
                    ':leave',
                    [
                        style({ transform: 'translateY(0)', opacity: 1 }),
                        stagger(80, [animate('800ms ease-out', style({ transform: 'translateY(100vh)', opacity: 0 }))]),
                    ],
                    { optional: true }
                ),
            ]),
        ]),

        trigger('settingsShowHideAnimation', [
            transition(':enter', [style({ transform: 'translateX(100%)', opacity: 0 }), animate('500ms', style({ transform: 'translateX(0)', opacity: 1 }))]),
            transition(':leave', [animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))]),
        ]),
    ],
})
export class CustomLobbyMenuComponent implements OnInit, OnDestroy {
    private spectatorTeam: CustomLobbyTeamData = { [0]: 'Spectator', [1]: 'rgb(255, 255, 255)', [2]: 4, [3]: 0, [4]: true };
    private team1: CustomLobbyTeamData = { [0]: 'SWAT', [1]: 'rgb(0, 150, 0)', [2]: 52, [3]: -1920001264, [4]: false };
    private team2: CustomLobbyTeamData = { [0]: 'Terrorists', [1]: 'rgb(150, 0, 0)', [2]: 1, [3]: 275618457, [4]: false };

    settingPanel: LobbySettingPanel[] = [
        {
            title: 'Default',
            rows: [
                {
                    type: SettingType.string,
                    dataSettingIndex: LobbySetting.Name,
                    defaultValue: '',
                    formControl: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]),
                },
                /*{ type: LobbySettingType.option, dataSettingIndex: "Type", required: true, value: "",
                  options: Object.keys(LobbyMapType).slice(Object.keys(LobbyMapType).length / 2) }*/
                {
                    type: SettingType.password,
                    dataSettingIndex: LobbySetting.Password,
                    defaultValue: '',
                    formControl: new FormControl('', [Validators.maxLength(100)]),
                },
                {
                    type: SettingType.boolean,
                    dataSettingIndex: LobbySetting.ShowRanking,
                    defaultValue: true,
                    formControl: new FormControl(true),
                },
            ],
        },

        {
            title: 'Player',
            rows: [
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.StartHealth,
                    defaultValue: 100,
                    formControl: new FormControl(100, [Validators.required, Validators.max(Constants.MAX_POSSIBLE_HEALTH), Validators.min(1)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.StartArmor,
                    defaultValue: 100,
                    formControl: new FormControl(100, [Validators.required, Validators.max(Constants.MAX_POSSIBLE_ARMOR), Validators.min(0)]),
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.AmountLifes /*"AmountLifes"*/,
                    defaultValue: 1,
                    formControl: new FormControl(1, [Validators.required, Validators.max(999), Validators.min(1)]),
                    onlyInt: true,
                },
            ],
        },

        {
            title: 'Teams',
            rows: [
                {
                    type: SettingType.boolean,
                    dataSettingIndex: LobbySetting.MixTeamsAfterRound,
                    defaultValue: true,
                    formControl: new FormControl(true, []),
                },
                {
                    type: SettingType.button,
                    dataSettingIndex: LobbySetting.Teams,
                    defaultValue: [this.spectatorTeam, this.team1, this.team2],
                    formControl: new FormControl(
                        [this.spectatorTeam, this.team1, this.team2],
                        [notEnoughTeamsValidator(this.getSelectedLobbyMaps.bind(this), this.getSelectedLobbyTeams.bind(this))]
                    ),
                    action: () => {
                        this.changeToOtherMenu(CustomLobbyMenuType.Teams);
                    },
                },
            ],
        },

        /* "Weapons" */
        {
            title: 'Weapons',
            rows: [
                {
                    type: SettingType.button,
                    dataSettingIndex: LobbySetting.Weapons,
                    defaultValue: null,
                    formControl: new FormControl(null),
                    action: () => {
                        this.changeToOtherMenu(CustomLobbyMenuType.Weapons);
                    },
                },
                {
                    type: SettingType.button,
                    dataSettingIndex: LobbySetting.ArmsRaceWeapons,
                    defaultValue: null,
                    formControl: new FormControl(null),
                    action: () => {
                        this.changeToOtherMenu(CustomLobbyMenuType.ArmsRaceWeapons);
                    },
                },
            ],
        },

        {
            title: 'Map',
            rows: [
                {
                    type: SettingType.enum,
                    dataSettingIndex: LobbySetting.MapLimitType,
                    defaultValue: 'KillAfterTime',
                    enum: LobbyMapLimitType,
                    formControl: new FormControl(LobbyMapLimitType.KillAfterTime, []),
                },
                {
                    type: SettingType.button,
                    dataSettingIndex: LobbySetting.Maps,
                    defaultValue: [-1],
                    formControl: new FormControl([-1]),
                    action: () => {
                        this.changeToOtherMenu(CustomLobbyMenuType.Maps);
                    },
                },
            ],
        },

        {
            title: 'Times',
            rows: [
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.BombDetonateTimeMs /*"BombDetonateTimeMs"*/,
                    defaultValue: 45000,
                    formControl: new FormControl(45000, [Validators.required, Validators.max(999999), Validators.min(0)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.BombDefuseTimeMs,
                    defaultValue: 8000,
                    formControl: new FormControl(8000, [Validators.required, Validators.max(999999), Validators.min(0)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.BombPlantTimeMs,
                    defaultValue: 3000,
                    formControl: new FormControl(3000, [Validators.required, Validators.max(999999), Validators.min(0)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.RoundTime,
                    defaultValue: 240,
                    formControl: new FormControl(240, [Validators.required, Validators.max(999999), Validators.min(60)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.CountdownTime,
                    defaultValue: 5,
                    formControl: new FormControl(5, [Validators.required, Validators.max(60), Validators.min(0)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.SpawnAgainAfterDeathMs,
                    defaultValue: 400,
                    formControl: new FormControl(400, [Validators.required, Validators.max(999999), Validators.min(0)]),
                    onlyInt: true,
                },
                {
                    type: SettingType.number,
                    dataSettingIndex: LobbySetting.MapLimitTime,
                    defaultValue: 10,
                    formControl: new FormControl(10, [Validators.required, Validators.max(9999), Validators.min(0)]),
                    onlyInt: true,
                },
            ],
        },
    ];
    lobbySettingType = SettingType;
    lobbySetting = LobbySetting;
    MathFloor = Math.floor;

    creating = true;
    inMenu: CustomLobbyMenuType = CustomLobbyMenuType.Main;
    customLobbyMenuType = CustomLobbyMenuType;
    loadingData = false;

    lobbyDatas: CustomLobbyData[] = [];
    createLobbyDatas: DataForCustomLobbyCreation;
    validationError: string;

    constructor(
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        public changeDetector: ChangeDetectorRef,
        private notificationService: NotificationService,
        private dialog: MatDialog,
        public errorService: ErrorService
    ) {
        this.addValidatorsToErrorService();
    }

    ngOnInit() {
        this.rageConnector.listen(DFromServerEvent.AddCustomLobby, this.addCustomLobby.bind(this));
        this.rageConnector.listen(DFromServerEvent.RemoveCustomLobby, this.removeCustomLobby.bind(this));
        this.rageConnector.listen(DFromServerEvent.SyncAllCustomLobbies, this.syncAllCustomLobbies.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
        this.rageConnector.callServer(DToServerEvent.JoinedCustomLobbiesMenu);

        // DEBUG //
        /*this.createLobbyDatas = {
            0: [
                { 0: WeaponHash.Advancedrifle, 1: 99, 2: 99, 3: 99 },
                { 0: WeaponHash.Appistol, 1: 99, 2: 99, 3: 99 },
                { 0: WeaponHash.Assaultrifle, 1: 99, 2: 99, 3: 99 },
                { 0: WeaponHash.Assaultsmg, 1: 99, 2: 99, 3: 99 }
            ],
            1: [],
            2: []
        };
        this.setSelectedLobbyWeapons(this.createLobbyDatas[1]);
        this.setSelectedLobbyArmsRaceWeapons(this.createLobbyDatas[2]);*/
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromServerEvent.AddCustomLobby, this.addCustomLobby.bind(this));
        this.rageConnector.remove(DFromServerEvent.RemoveCustomLobby, this.removeCustomLobby.bind(this));
        this.rageConnector.remove(DFromServerEvent.SyncAllCustomLobbies, this.syncAllCustomLobbies.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
        this.rageConnector.callServer(DToServerEvent.LeftCustomLobbiesMenu);
    }

    private addCustomLobby(customLobbyDataJson: string) {
        this.lobbyDatas.push(JSON.parse(customLobbyDataJson));
        this.changeDetector.detectChanges();
    }

    private removeCustomLobby(lobbyId: number) {
        const lobbyIndex = this.lobbyDatas.findIndex((l) => l[0] == lobbyId);
        if (lobbyIndex >= 0) {
            this.lobbyDatas.splice(lobbyIndex, 1);
            if (this.isSelectedLobby(lobbyId)) {
                this.showLobbyCreating();
            } else {
                this.changeDetector.detectChanges();
            }
        }
    }

    private syncAllCustomLobbies(allCustomLobbyDatas: string) {
        this.lobbyDatas = JSON.parse(allCustomLobbyDatas);
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }

    showLobbyCreating() {
        for (const panel of this.settingPanel) {
            for (const setting of panel.rows) {
                setting.formControl.setValue(setting.defaultValue);
                setting.formControl.enable();
            }
        }
        this.creating = true;
        this.changeDetector.detectChanges();
    }

    showLobbyData(lobbyData: CustomLobbyData) {
        this.creating = false;
        for (const panel of this.settingPanel) {
            for (const setting of panel.rows) {
                setting.formControl.setValue(lobbyData[setting.dataSettingIndex]);
                setting.formControl.disable();
            }
        }
        this.changeDetector.detectChanges();
    }

    createLobby() {
        if (!this.creating) return;
        if (this.errorService.hasError()) return;

        const data = new CustomLobbyData();
        for (const panel of this.settingPanel) {
            for (const setting of panel.rows) {
                data[setting.dataSettingIndex] = setting.formControl.value;
            }
        }

        this.rageConnector.callCallbackServer(DToServerEvent.CreateCustomLobby, [JSON.stringify(data)], (error: string) => {
            if (!error || error == '') return;
            this.notificationService.showError(error);
        });
    }

    joinLobby() {
        if (this.creating) return;

        const clickedLobbyName = this.settingPanel[0].rows[0].formControl.value;
        const clickedLobbyData = this.lobbyDatas.find((l) => l[1] == clickedLobbyName);
        if (!clickedLobbyData) return;

        if (clickedLobbyData[3] != '') {
            const dialogRef = this.dialog.open(CustomLobbyPasswordDialog, { data: clickedLobbyData[3], panelClass: 'mat-app-background' });

            dialogRef.beforeClosed().subscribe((inputedPassword) => {
                if (inputedPassword == undefined) return;
                if (inputedPassword == false) {
                    this.notificationService.showError(this.settings.Lang.PasswordIncorrect);
                    return;
                }
                this.rageConnector.callServer(DToServerEvent.JoinLobbyWithPassword, clickedLobbyData[0], inputedPassword);
            });
        } else {
            this.rageConnector.callServer(DToServerEvent.JoinLobby, clickedLobbyData[0]);
        }
    }

    private changeToOtherMenu(menuType: CustomLobbyMenuType) {
        if (this.loadingData) {
            return;
        }
        if (this.creating) {
            if (!this.createLobbyDatas) {
                this.loadDatasForCustomLobby(menuType);
            } else {
                this.inMenu = menuType;
                this.loadingData = false;
            }
        }
        this.changeDetector.detectChanges();
    }

    private loadDatasForCustomLobby(menuType: CustomLobbyMenuType) {
        this.loadingData = true;
        this.changeDetector.detectChanges();

        this.rageConnector.callCallbackServer(DToServerEvent.LoadDatasForCustomLobby, [], (json: string) => {
            this.createLobbyDatas = JSON.parse(json);

            this.setSelectedLobbyWeapons(this.createLobbyDatas[1]);
            this.setSelectedLobbyArmsRaceWeapons(this.createLobbyDatas[2]);

            this.inMenu = menuType;
            this.loadingData = false;
            this.changeDetector.detectChanges();
        });
    }

    goBack() {
        this.settings.InUserLobbiesMenu = false;
        this.changeDetector.detectChanges();
    }

    goBackToMainSettings(event: CustomLobbyTeamData[] | number[] | CustomLobbyWeaponData[]) {
        if (this.creating) {
            switch (this.inMenu) {
                case CustomLobbyMenuType.Weapons:
                    this.setSelectedLobbyWeapons(event as CustomLobbyWeaponData[]);
                    break;
                case CustomLobbyMenuType.Maps:
                    this.setSelectedLobbyMaps(event as number[]);
                    break;
                case CustomLobbyMenuType.Teams:
                    this.setSelectedLobbyTeams(event as CustomLobbyTeamData[]);
                    break;
                case CustomLobbyMenuType.ArmsRaceWeapons:
                    this.setSelectedLobbyArmsRaceWeapons(event as CustomLobbyArmsRaceWeaponData[]);
                    break;
            }
        }

        this.inMenu = CustomLobbyMenuType.Main;
        this.changeDetector.detectChanges();
    }

    private addValidatorsToErrorService() {
        // Add checks without form controls
        const checkStartsWithMapcreator = new CustomErrorCheck(
            'StartWithMapcreator',
            () => {
                const lobbyName = this.getLobbyName().toLowerCase();
                return !lobbyName.startsWith('mapcreator');
            },
            'StartWithMapcreatorError'
        );
        this.errorService.add(checkStartsWithMapcreator);

        const checkLobbyWithSameName = new CustomErrorCheck(
            'CheckLobbyWithSameName',
            () => {
                const lobbyName = this.getLobbyName().toLowerCase();
                return !this.lobbyDatas.some((l) => l[1].toLowerCase() == lobbyName);
            },
            'LobbyWithNameAlreadyExistsError'
        );
        this.errorService.add(checkLobbyWithSameName);

        // Add the form controls
        for (const setting of this.settingPanel) {
            for (const row of setting.rows) {
                if (row.formControl) {
                    this.errorService.add(new FormControlCheck(LobbySetting[row.dataSettingIndex], row.formControl));
                }
            }
        }
    }

    getEnumKeys(e: {}) {
        const keys = Object.keys(e);
        return keys.slice(keys.length / 2);
    }

    getLobbyName(): string {
        return this.settingPanel.find((p) => p.title === 'Default').rows.find((p) => p.dataSettingIndex === LobbySetting.Name).formControl.value as string;
    }

    getSelectedLobbyTeams() {
        if (!this.settingPanel) return undefined;
        return this.settingPanel.find((p) => p.title === 'Teams').rows.find((p) => p.dataSettingIndex === LobbySetting.Teams).formControl.value;
    }

    setSelectedLobbyTeams(teams: CustomLobbyTeamData[]) {
        this.settingPanel
            .find((p) => p.title === 'Teams')
            .rows.find((p) => p.dataSettingIndex === LobbySetting.Teams)
            .formControl.setValue(teams);
        this.changeDetector.detectChanges();
    }

    getSelectedLobbyMaps() {
        if (!this.settingPanel) return undefined;
        return this.settingPanel.find((p) => p.title === 'Map').rows.find((p) => p.dataSettingIndex === LobbySetting.Maps).formControl.value;
    }

    getSelectedLobbyWeapons() {
        if (!this.settingPanel) return undefined;
        return this.settingPanel.find((p) => p.title === 'Weapons').rows.find((p) => p.dataSettingIndex === LobbySetting.Weapons).formControl.value;
    }

    getSelectedLobbyArmsRaceWeapons() {
        if (!this.settingPanel) return undefined;
        return this.settingPanel.find((p) => p.title === 'Weapons').rows.find((p) => p.dataSettingIndex === LobbySetting.ArmsRaceWeapons).formControl.value;
    }

    setSelectedLobbyMaps(maps: number[]) {
        this.settingPanel
            .find((p) => p.title === 'Map')
            .rows.find((p) => p.dataSettingIndex === LobbySetting.Maps)
            .formControl.setValue(maps);
        this.changeDetector.detectChanges();
    }

    setSelectedLobbyWeapons(weapons: CustomLobbyWeaponData[]) {
        this.settingPanel
            .find((p) => p.title === 'Weapons')
            .rows.find((p) => p.dataSettingIndex === LobbySetting.Weapons)
            .formControl.setValue(weapons);
        this.changeDetector.detectChanges();
    }

    setSelectedLobbyArmsRaceWeapons(weapons: CustomLobbyArmsRaceWeaponData[]) {
        this.settingPanel
            .find((p) => p.title === 'Weapons')
            .rows.find((p) => p.dataSettingIndex === LobbySetting.ArmsRaceWeapons)
            .formControl.setValue(weapons);
        this.changeDetector.detectChanges();
    }

    private isSelectedLobby(lobbyId: number): boolean {
        const selectedLobbyName = this.settingPanel[0].rows[0].formControl.value;
        const lobbyData = this.lobbyDatas.find((l) => l[0] == lobbyId);
        return lobbyData && lobbyData[1] == selectedLobbyName;
    }
}
