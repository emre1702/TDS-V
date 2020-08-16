import { injectable, inject } from "inversify";
import alt from "alt-client";
import natives from "natives";
import ToServerEvent from "../../data/enums/events/to-server-event.enum";
import { TypedEvent } from "../../entity/events/typed-event";
import ToClientEvent from "../../data/enums/events/to-client-event.enum";
import PlayerDataKey from "../../data/enums/data/player-data-key.enum";
import Language from "../../data/interfaces/Language.interface";
import LobbySettings from "../../data/interfaces/lobbies/lobby-settings.interface";
import MapCreatorObject from "../../data/interfaces/lobbies/map-creator/map-creator-object.interface";
import PlayerSettings from "../../data/interfaces/players/player-settings.interface";
import WeaponHash from "../../data/enums/weapons/weapon-hash.enum";
import RemoteEventsSender from "./remote-events-sender.service";

@injectable()
export default class EventsService {

    readonly onAngularCooldown = new TypedEvent<void>();      ////////
    readonly onChatInputToggled = new TypedEvent<boolean>();        ////////
    readonly onCountdownStarted = new TypedEvent<{ isSpectator: boolean }>();       ////////
    readonly onCursorToggled = new TypedEvent<boolean>();       ////////
    readonly onCursorToggleRequested = new TypedEvent<boolean>();       ////////
    readonly onDataChanged = new TypedEvent<{ key: PlayerDataKey, value: any }>();      ////////
    readonly onFreecamToggled = new TypedEvent<boolean>();      ////////
    readonly onHideScoreboard = new TypedEvent<void>();       ////////
    readonly onInFightStatusChanged = new TypedEvent<boolean>();        ////////
    readonly onLanguageChanged = new TypedEvent<{ language: Language, beforeLogin: boolean }>();        ////////
    readonly onLobbyJoined = new TypedEvent<LobbySettings>();       ////////
    readonly onLobbyLeft = new TypedEvent<LobbySettings>();     ////////
    readonly onLocalPlayerDied = new TypedEvent<void>();      ////////
    readonly onLoggedIn = new TypedEvent<void>();     ////////
    readonly onMapBorderColorChanged = new TypedEvent<alt.RGBA>();      ////////
    readonly onMapChanged = new TypedEvent<void>();       ////////
    readonly onMapCleared = new TypedEvent<void>();       ////////
    readonly onMapCreatorObjectDeleted = new TypedEvent<void>();      ////////
    readonly onMapCreatorSyncLatestObjectIDRequest = new TypedEvent<void>();      ////////
    readonly onMapCreatorSyncObjectDeleted = new TypedEvent<MapCreatorObject>();        ////////
    readonly onMapCreatorSyncTeamObjectsDeleted = new TypedEvent<{ teamNumber: number }>();     ////////
    readonly onPlayerDied = new TypedEvent<{ player: alt.Player, teamIndex: number, willRespawn: boolean }>();      ////////
    readonly onPlayerJoinedSameLobby = new TypedEvent<{ player: alt.Player }>();        ////////
    readonly onPlayerLeftSameLobby = new TypedEvent<{ player: alt.Player, name: string }>();        ////////
    readonly onRespawned = new TypedEvent<{ inFightAgain: boolean }>();     
    readonly onRoundEnded = new TypedEvent<{ isSpectator: boolean }>();     ////////
    readonly onRoundStarted = new TypedEvent<{ isSpectator: boolean }>();   ////////
    readonly onSettingsLoaded = new TypedEvent<PlayerSettings>();   ////////
    readonly onShowScoreboardRequest = new TypedEvent<void>();    ////////     
    readonly onSpawned = new TypedEvent<void>();
    readonly onTeamChanged = new TypedEvent<{ newTeamName: string }>();
    readonly onWeaponChanged = new TypedEvent<{ previous: WeaponHash, next: WeaponHash }>();

    private currentWeapon: WeaponHash = WeaponHash.Unarmed;

    constructor(
        @inject(RemoteEventsSender) private remoteEventsSender: RemoteEventsSender
    ) {
        //Todo: Make this settable later
        alt.setInterval(this.check.bind(this), 100);
        alt.on(ToServerEvent.FromBrowserEvent, this.onFromBrowserEvent.bind(this));

        //Todo: Add this at server
        alt.onServer(ToClientEvent.PlayerSpawn, this.onPlayerSpawn.bind(this))
        alt.onServer(ToClientEvent.PlayerTeamChange, (teamName) => this.onTeamChanged.emit({ newTeamName: teamName }));
    }



    private check() {
        this.checkWeaponChange();
    }

    private checkWeaponChange() {
        const weapon = natives.getSelectedPedWeapon(alt.Player.local.scriptID) as WeaponHash;
        if (this.currentWeapon != weapon) {
            this.onWeaponChanged.emit({ previous: this.currentWeapon, next: weapon });
            this.currentWeapon = weapon;
        }
    }

    private onPlayerSpawn(isRespawn: boolean, inFightAgain: boolean) {
        this.onSpawned.emit();
        if (isRespawn) {
            this.onRespawned.emit({ inFightAgain: inFightAgain });
        }
    }

    private onFromBrowserEvent(...args: any[]) {
        if (!this.remoteEventsSender.sendFromBrowser(args)) {
            this.onAngularCooldown.emit();
        }
    }
}

