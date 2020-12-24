import { Component, ChangeDetectorRef, OnInit, OnDestroy, Input } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../../services/settings.service';
import { trigger, transition, animate, style } from '@angular/animations';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { FromServerEvent } from '../../../enums/from-server-event.enum';
import { OfficialLobbyId } from '../enums/official-lobby-id.enum';

@Component({
    selector: 'app-lobby-choice',
    templateUrl: './lobby-choice.component.html',
    animations: [
        trigger('hideShowAnimation', [
            transition(':enter', [
                style({ transform: 'translateX(-100%)', opacity: 0 }),
                animate('800ms', style({ transform: 'translateX(0)', opacity: 0.95 })),
            ]),
            transition(':leave', [animate('800ms', style({ transform: 'translateX(-100%)', opacity: 0 }))]),
        ]),
    ],
})
export class LobbyChoiceComponent implements OnInit, OnDestroy {
    @Input() showRegisterLogin: boolean;

    constructor(private rageConnector: RageConnectorService, public settings: SettingsService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.rageConnector.listen(FromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromServerEvent.LeaveCustomLobbyMenu, this.leaveCustomLobbyMenu.bind(this));

        // Clear it so it doesn't use fill our RAM without a reason
        this.settings.AllMapsForCustomLobby = [];
    }

    private leaveCustomLobbyMenu() {
        this.settings.InUserLobbiesMenu = false;
        this.changeDetector.detectChanges();
    }

    joinLobby(id: number) {
        if (id === OfficialLobbyId.CustomLobby) this.showUserLobbies();
        else this.rageConnector.callServer(ToServerEvent.JoinLobby, id);
    }

    showUserLobbies() {
        this.settings.InUserLobbiesMenu = true;
        this.changeDetector.detectChanges();
    }
}
