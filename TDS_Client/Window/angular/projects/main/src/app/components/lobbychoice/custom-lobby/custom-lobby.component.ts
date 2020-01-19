import { Component, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { SettingType } from '../../../enums/setting-type';
import { LobbySettingPanel } from '../models/lobby-setting-panel';
import { FormControl, Validators } from '@angular/forms';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { CustomLobbyData } from '../models/custom-lobby-data';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { MatSnackBar, MatDialog } from '@angular/material';
import { CustomLobbyPasswordDialog } from '../dialog/custom-lobby-password-dialog';
import { LobbyMapLimitType } from '../enums/lobby-map-limit-type';
import { CustomLobbyTeamData } from '../models/custom-lobby-team-data';
import { LobbySetting } from '../enums/lobby-setting.enum';

@Component({
    selector: 'app-custom-lobby',
    templateUrl: './custom-lobby.component.html',
    styleUrls: ['./custom-lobby.component.scss'],
    animations: [
        trigger('lobbyShowHideAnimation', [
            transition('* => *', [
                query(':enter', [
                    style({ transform: 'translateY(100vh)', opacity: 0 }),
                    stagger(80, [
                        animate('800ms ease-in-out', style({ transform: 'translateY(0)', opacity: 1 })),
                    ])
                ], { optional: true }),
                query(':leave', [
                    style({ transform: 'translateY(0)', opacity: 1 }),
                    stagger(80, [
                        animate('800ms ease-out', style({ transform: 'translateY(100vh)', opacity: 0 }))
                    ])
                ], { optional: true })
            ])
        ]),

        trigger('settingsShowHideAnimation', [
            transition(
                ':enter', [
                style({ transform: 'translateX(100%)', opacity: 0 }),
                animate('500ms', style({ transform: 'translateX(0)', opacity: 1 }))
            ]
            ),
            transition(
                ':leave', [
                animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 })),
            ]
            )]
        )
    ],
})

export class CustomLobbyMenuComponent implements OnInit, OnDestroy {
    private spectatorTeam: CustomLobbyTeamData = ["Spectator", "rgb(255, 255, 255)", 4, 0, true];
    private team1: CustomLobbyTeamData = ["SWAT", "rgb(0, 150, 0)", 52, -1920001264, false];
    private team2: CustomLobbyTeamData = ["Terroristen", "rgb(150, 0, 0)", 1, 275618457, false];

    settingPanel: LobbySettingPanel[] = [
        {
            title: "Default", rows: [
                {
                    type: SettingType.string, dataSettingIndex: 1 /*"Name"*/, defaultValue: "",
                    formControl: new FormControl("", [Validators.required, Validators.maxLength(50), Validators.minLength(3)])
                },
                /*{ type: LobbySettingType.option, dataSettingIndex: "Type", required: true, value: "",
                  options: Object.keys(LobbyMapType).slice(Object.keys(LobbyMapType).length / 2) }*/
                {
                    type: SettingType.password, dataSettingIndex: 3 /*"Password"*/, defaultValue: "",
                    formControl: new FormControl("", [Validators.maxLength(100)])
                },
                {
                    type: SettingType.boolean, dataSettingIndex: 8 /*"ShowRanking"*/, defaultValue: true,
                    formControl: new FormControl(true)
                }
            ]
        },

        {
            title: "Player", rows: [
                {
                    type: SettingType.number, dataSettingIndex: 4 /*"StartHealth"*/, defaultValue: 100,
                    formControl: new FormControl(100, [Validators.required, Validators.max(100), Validators.min(1)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 5 /*"StartArmor"*/, defaultValue: 100,
                    formControl: new FormControl(100, [Validators.required, Validators.max(200), Validators.min(0)])
                },
                {
                    type: SettingType.number, dataSettingIndex: 6 /*"AmountLifes"*/, defaultValue: 1,
                    formControl: new FormControl(1, [Validators.required, Validators.max(999), Validators.min(1)]), onlyInt: true
                },
            ]
        },

        {
            title: "Teams", rows: [
                {
                    type: SettingType.boolean, dataSettingIndex: 7 /*"MixTeamsAfterRound"*/, defaultValue: true,
                    formControl: new FormControl(true, [])
                },
                {
                    type: SettingType.button, dataSettingIndex: 17 /*"Teams"*/, defaultValue: [this.spectatorTeam, this.team1, this.team2],
                    formControl: new FormControl([this.spectatorTeam, this.team1, this.team2]),
                    action: () => { this.inTeamsMenu = true; this.changeDetector.detectChanges(); }
                }
            ]
        },

        /*{
          title: "Weapons", rows: [
            {
              type: SettingType.button, dataSettingIndex: "Weapons", defaultValue: [],
              action: () => { this.inWeaponsMenu = true; this.changeDetector.detectChanges(); }
            }
          ]
        },*/

        {
            title: "Map", rows: [
                {
                    type: SettingType.enum, dataSettingIndex: 15 /*"MapLimitType"*/, defaultValue: "KillAfterTime",
                    enum: LobbyMapLimitType,
                    formControl: new FormControl(LobbyMapLimitType.KillAfterTime, [])
                },
                {
                    type: SettingType.button, dataSettingIndex: 18, defaultValue: [-1],
                    formControl: new FormControl([-1]),
                    action: () => { this.inMapsMenu = true; this.changeDetector.detectChanges(); }
                }
            ]
        },

        {
            title: "Times", rows: [
                {
                    type: SettingType.number, dataSettingIndex: 9 /*"BombDetonateTimeMs"*/, defaultValue: 45000,
                    formControl: new FormControl(45000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 10 /*"BombDefuseTimeMs"*/, defaultValue: 8000,
                    formControl: new FormControl(8000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 11 /*"BombPlantTimeMs"*/, defaultValue: 3000,
                    formControl: new FormControl(3000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 12 /*"RoundTime"*/, defaultValue: 240,
                    formControl: new FormControl(240, [Validators.required, Validators.max(999999), Validators.min(60)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 13 /*"CountdownTime"*/, defaultValue: 5,
                    formControl: new FormControl(5, [Validators.required, Validators.max(60), Validators.min(0)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 14 /*"SpawnAgainAfterDeathMs"*/, defaultValue: 400,
                    formControl: new FormControl(400, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
                },
                {
                    type: SettingType.number, dataSettingIndex: 15 /*"MapLimitTime"*/, defaultValue: 10,
                    formControl: new FormControl(10, [Validators.required, Validators.max(9999), Validators.min(0)]), onlyInt: true
                },
            ]
        }
    ];
    lobbySettingType = SettingType;
    lobbySetting = LobbySetting;
    MathFloor = Math.floor;

    creating = true;
    inTeamsMenu = false;
    inWeaponsMenu = false;
    inMapsMenu = false;

    lobbyDatas: CustomLobbyData[] = [
        [1, "Bonus Lobby", "Bonus", "asd", 200, 100, 3,
            true, true, 10000, 10000, 10000, 30, 30, 30, 30,
            LobbyMapLimitType.TeleportBackAfterTime, [], []]
    ];

    constructor(public settings: SettingsService, private rageConnector: RageConnectorService,
        public changeDetector: ChangeDetectorRef, private snackBar: MatSnackBar, private dialog: MatDialog) {
    }

    ngOnInit() {
        this.rageConnector.listen(DFromClientEvent.AddCustomLobby, this.addCustomLobby.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemoveCustomLobby, this.removeCustomLobby.bind(this));
        this.rageConnector.listen(DFromClientEvent.SyncAllCustomLobbies, this.syncAllCustomLobbies.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanged.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromClientEvent.AddCustomLobby, this.addCustomLobby.bind(this));
        this.rageConnector.remove(DFromClientEvent.RemoveCustomLobby, this.removeCustomLobby.bind(this));
        this.rageConnector.remove(DFromClientEvent.SyncAllCustomLobbies, this.syncAllCustomLobbies.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanged.bind(this));
    }

    private addCustomLobby(customLobbyDataJson: string) {
        this.lobbyDatas.push(JSON.parse(customLobbyDataJson));
        this.changeDetector.detectChanges();
    }

    private removeCustomLobby(lobbyId: number) {
        const lobbyIndex = this.lobbyDatas.findIndex(l => l[0] == lobbyId);
        if (lobbyIndex >= 0) {
            this.lobbyDatas.splice(lobbyIndex, 1);
            this.changeDetector.detectChanges();
        }
    }

    private syncAllCustomLobbies(allCustomLobbyDatas: string) {
        this.lobbyDatas = JSON.parse(allCustomLobbyDatas);
        this.changeDetector.detectChanges();
    }

    private detectChanged() {
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
        if (!this.creating)
            return;
        if (!this.areSettingsValid())
            return;

        const data = new CustomLobbyData();
        for (const panel of this.settingPanel) {
            for (const setting of panel.rows) {
                data[setting.dataSettingIndex] = setting.formControl.value;
            }
        }

        this.rageConnector.callCallback(DToClientEvent.CreateCustomLobby, [JSON.stringify(data)], (error: string) => {
            if (!error || error == "")
                return;
            this.snackBar.open(error, "OK", {
                duration: 6000,
                panelClass: "mat-app-background"
            });
        });
    }

    joinLobby() {
        if (this.creating)
            return;

        const clickedLobbyName = this.settingPanel[0].rows[0].formControl.value;
        const clickedLobbyData = this.lobbyDatas.find(l => l[1] == clickedLobbyName);
        if (!clickedLobbyData)
            return;

        if (clickedLobbyData[3] != "") {
            const dialogRef = this.dialog.open(CustomLobbyPasswordDialog, { data: clickedLobbyData[3], panelClass: "mat-app-background" });

            dialogRef.beforeClosed().subscribe((inputedPassword) => {
                if (inputedPassword == undefined)
                    return;
                if (inputedPassword == false) {
                    this.snackBar.open(this.settings.Lang.PasswordIncorrect, "OK", { duration: 7000, panelClass: "mat-app-background" });
                    return;
                }
                this.rageConnector.call(DToClientEvent.JoinCustomLobbyWithPassword, clickedLobbyData[0], inputedPassword);
            });
        } else {
            this.rageConnector.call(DToClientEvent.JoinCustomLobby, clickedLobbyData[0]);
        }
    }

    goBack() {
        this.settings.InUserLobbiesMenu = false;
        this.rageConnector.call(DToClientEvent.LeftCustomLobbiesMenu);
        this.changeDetector.detectChanges();
    }

    goBackToMainSettingsFromLobbyTeams(event: CustomLobbyTeamData[]) {
        this.inTeamsMenu = false;
        this.setSelectedLobbyTeams(event);
        this.changeDetector.detectChanges();
    }

    goBackToMainSettingsFromLobbyMaps(event: number[]) {
        this.inMapsMenu = false;
        this.setSelectedLobbyMaps(event);
        this.changeDetector.detectChanges();
    }

    areSettingsValid(): boolean {
        for (const panel of this.settingPanel) {
            for (const setting of panel.rows) {
                if (!setting.formControl.valid) {
                    return false;
                }
            }
        }
        const lobbyName: string = (this.settingPanel
            .find(p => p.title === "Default").rows
            .find(p => p.dataSettingIndex === 1 /*"Name"*/).formControl.value as string).toLowerCase();
        if (lobbyName.startsWith("mapcreator"))
            return false;
        if (this.lobbyDatas.findIndex(l => l[1] == lobbyName))
            return false;

        return true;
    }

    getEnumKeys(e: {}) {
        const keys = Object.keys(e);
        return keys.slice(keys.length / 2);
    }

    getSelectedLobbyTeams() {
        return this.settingPanel
            .find(p => p.title === "Teams").rows
            .find(p => p.dataSettingIndex === 17 /*"Teams"*/).formControl.value;
    }

    setSelectedLobbyTeams(teams: CustomLobbyTeamData[]) {
        this.settingPanel
            .find(p => p.title === "Teams").rows
            .find(p => p.dataSettingIndex === 17 /*"Teams"*/).formControl.setValue(teams);
        this.changeDetector.detectChanges();
    }

    getSelectedLobbyMaps() {
        return this.settingPanel
            .find(p => p.title === "Map").rows
            .find(p => p.dataSettingIndex === 18 /*"Map"*/).formControl.value;
    }

    setSelectedLobbyMaps(maps: number[]) {
        this.settingPanel
            .find(p => p.title === "Map").rows
            .find(p => p.dataSettingIndex === 18 /*"Map"*/).formControl.setValue(maps);
        this.changeDetector.detectChanges();
    }
}
