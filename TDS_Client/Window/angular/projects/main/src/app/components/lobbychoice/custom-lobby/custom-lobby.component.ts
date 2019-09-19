import { Component, ChangeDetectorRef } from '@angular/core';
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
            animate('800ms ease-in-out', style({ transform: 'translateY(0)', opacity: 0.95 })),
          ])
        ], { optional: true }),
        query(':leave', [
          style({ transform: 'translateY(0)', opacity: 0.95 }),
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
          animate('500ms', style({ transform: 'translateX(0)', opacity: 0.9 }))
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

export class CustomLobbyMenuComponent {
  settingPanel: LobbySettingPanel[] = [
    {
      title: "Default", rows: [
        {
          type: SettingType.string, dataSettingIndex: "Name", defaultValue: "",
          formControl: new FormControl("", [Validators.required, Validators.maxLength(50), Validators.minLength(3)])
        },
        /*{ type: LobbySettingType.option, dataSettingIndex: "Type", required: true, value: "",
          options: Object.keys(LobbyMapType).slice(Object.keys(LobbyMapType).length / 2) }*/
        {
          type: SettingType.password, dataSettingIndex: "Password", defaultValue: "",
          formControl: new FormControl("", [Validators.maxLength(100)])
        }
      ]
    },

    {
      title: "Player", rows: [
        {
          type: SettingType.number, dataSettingIndex: "StartHealth", defaultValue: 100,
          formControl: new FormControl(100, [Validators.required, Validators.max(100), Validators.min(1)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "StartArmor", defaultValue: 100,
          formControl: new FormControl(100, [Validators.required, Validators.max(200), Validators.min(0)])
        },
        {
          type: SettingType.number, dataSettingIndex: "AmountLifes", defaultValue: 1,
          formControl: new FormControl(1, [Validators.required, Validators.max(999), Validators.min(1)]), onlyInt: true
        },

      ]
    },

    {
      title: "Teams", rows: [
        {
          type: SettingType.boolean, dataSettingIndex: "MixTeamsAfterRound", defaultValue: true,
          formControl: new FormControl(true, [])
        }
      ]
    },

    {
      title: "Map", rows: [
        {
          type: SettingType.enum, dataSettingIndex: "MapLimitType", defaultValue: "KillAfterTime",
          enum: LobbyMapLimitType,
          formControl: new FormControl(LobbyMapLimitType.KillAfterTime, [])
        }
      ]
    },

    {
      title: "Times", rows: [
        {
          type: SettingType.number, dataSettingIndex: "BombDetonateTimeMs", defaultValue: 45000,
          formControl: new FormControl(45000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "BombDefuseTimeMs", defaultValue: 8000,
          formControl: new FormControl(8000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "BombPlantTimeMs", defaultValue: 3000,
          formControl: new FormControl(3000, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "RoundTime", defaultValue: 240,
          formControl: new FormControl(240, [Validators.required, Validators.max(999999), Validators.min(60)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "CountdownTime", defaultValue: 5,
          formControl: new FormControl(5, [Validators.required, Validators.max(60), Validators.min(0)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "SpawnAgainAfterDeathMs", defaultValue: 400,
          formControl: new FormControl(400, [Validators.required, Validators.max(999999), Validators.min(0)]), onlyInt: true
        },
        {
          type: SettingType.number, dataSettingIndex: "MapLimitTime", defaultValue: 10,
          formControl: new FormControl(10, [Validators.required, Validators.max(9999), Validators.min(0)]), onlyInt: true
        },
      ]
    }
  ];
  lobbySettingType = SettingType;
  MathFloor = Math.floor;

  creating = true;

  lobbyDatas: CustomLobbyData[] = [];

  constructor(public settings: SettingsService, private rageConnector: RageConnectorService,
    public changeDetector: ChangeDetectorRef, private snackBar: MatSnackBar, private dialog: MatDialog) {

    this.rageConnector.listen(DFromClientEvent.AddCustomLobby, (customLobbyDataJson: string) => {
      this.lobbyDatas.push(JSON.parse(customLobbyDataJson));
      changeDetector.detectChanges();
    });

    this.rageConnector.listen(DFromClientEvent.RemoveCustomLobby, (lobbyId: number) => {
      const lobbyIndex = this.lobbyDatas.findIndex(l => l.LobbyId == lobbyId);
      if (lobbyIndex >= 0) {
        this.lobbyDatas.splice(lobbyIndex, 1);
        changeDetector.detectChanges();
      }
    });

    this.rageConnector.listen(DFromClientEvent.SyncAllCustomLobbies, (allCustomLobbyDatas: string) => {
      this.lobbyDatas = JSON.parse(allCustomLobbyDatas);
      changeDetector.detectChanges();
    });
  }

  getLobbyDatas(): Array<CustomLobbyData> {
    return Object.values(this.lobbyDatas);
  }

  showLobbyCreating() {
    for (const panel of this.settingPanel) {
      for (const setting of panel.rows) {
        setting.formControl.setValue(setting.defaultValue);
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
      }
    }
    this.changeDetector.detectChanges();
  }

  createLobby() {
    if (!this.creating)
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
    const clickedLobbyData = this.lobbyDatas.find(l => l.Name == clickedLobbyName);
    if (!clickedLobbyData)
      return;

    if (clickedLobbyData.Password != "") {
      const dialogRef = this.dialog.open(CustomLobbyPasswordDialog, { data: clickedLobbyData.Password, panelClass: "mat-app-background" });

      dialogRef.beforeClosed().subscribe((inputedPassword) => {
        if (!inputedPassword) {
          this.snackBar.open(this.settings.Lang.PasswordIncorrect, "OK", { duration: 7000, panelClass: "mat-app-background" });
          return;
        }
        this.rageConnector.call(DToClientEvent.JoinCustomLobbyWithPassword, clickedLobbyData.LobbyId, inputedPassword);
      });
    } else {
      this.rageConnector.call(DToClientEvent.JoinCustomLobby, clickedLobbyData.LobbyId);
    }
  }

  goBack() {
    this.settings.InUserLobbiesMenu = false;
    this.rageConnector.call(DToClientEvent.LeftCustomLobbiesMenu);
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
    return true;
  }

  getEnumKeys(e: {}) {
    const keys = Object.keys(e);
    return keys.slice(keys.length / 2);
  }
}
