import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import LobbySettings from "../../datas/interfaces/lobbies/lobby-settings.interface";
import LobbyType from "../../datas/enums/lobbies/lobby-type.enum";
import alt from "alt-client";
import SettingsService from "../settings/settings.service";
import BindsService from "../input/binds.service";
import Control from "../../datas/enums/input/control.enum";
import { neededDistanceToBeNotAFK } from "../../datas/constants";
import game from "natives";
import { Interval } from "../../entities/interval/interval.entity";
import ChatService from "../output/chat.service";
import RemoteEventsSender from "../events/remote-events-sender.service";
import ToServerEvent from "../../datas/enums/events/to-server-event.enum";

@injectable()
export class AFKCheckService {
    private inAFKCheckLobby: boolean;
    private lastPos: alt.Vector3;
    private afkStartPos: alt.Vector3;
    //private draw: DxTextRectangle;

    private kickInterval: Interval;
    private checkInterval: Interval;
    private onTickId: number;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.BindsService) private bindsService: BindsService,
        @inject(DIIdentifier.FightService) private fightService: FightService,
        @inject(DIIdentifier.ChatService) private chatService: ChatService,
        @inject(DIIdentifier.RemoteEventsSender) private remoteEventsSender: RemoteEventsSender
    ) {
        this.kickInterval = new Interval(this.isAFKEnd, settingsService.playerSettings.AFKKickAfterSeconds * 1000);
        this.checkInterval = new Interval(this.check, 5 * 1000);

        eventsService.onLobbyJoined.on(this.onLobbyJoined.bind(this));
        eventsService.onLobbyLeft.on(this.onLobbyLeft.bind(this));
        eventsService.onLocalPlayerDied.on(this.onLocalPlayerDied.bind(this));
        eventsService.onRoundStarted.on(this.onRoundStarted.bind(this));
        eventsService.onRoundEnded.on(this.onRoundEnded.bind(this));
    }

    private onLobbyJoined(settings: LobbySettings) {
        this.inAFKCheckLobby = settings.Type === LobbyType.Arena && settings.IsOfficial;
        this.lastPos = undefined;
    }

    private onLobbyLeft(settings: LobbySettings) {
        if (this.inAFKCheckLobby) {
            this.stopCheck();
        }
    }

    private onLocalPlayerDied() {
        //Todo Is that check needed?
        //if (!(_kickTimer is null) && (int)(_kickTimer.RemainingMsToExecute) <= _settingsHandler.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
        if (this.kickInterval.isRunning) {
            this.isAFKEnd();
        }
    }

    private onRoundStarted(data: { isSpectator: boolean }) {
        if (!this.inAFKCheckLobby)
            return;
        if (data.isSpectator)
            return;
        if (!this.settingsService.playerSettings.CheckAFK)
            return;
        this.lastPos = alt.Player.local.pos;
        this.checkInterval.start();

        this.bindsService.addControl(Control.Attack, this.stopAFK.bind(this));
        this.bindsService.addControl(Control.MeleeAttack1, this.stopAFK.bind(this));
    }

    private onRoundEnded() {
        if (this.checkInterval.isRunning) {
            this.stopCheck();
        }
    }

    private check() {
        if (!this.canBeAFK()) {
            this.stopAFK();
            return;
        }

        const currentPos = alt.Player.local.pos;
        if (!this.kickInterval.isRunning) {
            this.checkStart(currentPos);
        } else {
            this.checkStop(currentPos);
        }
    }

    private onTick() {
        if (!this.isStillAFK()) {
            this.stopAFK();
            return;
        }

        if (this.kickInterval.remainingSecToExec > this.settingsService.playerSettings.AFKKickShowWarningLastSeconds)
            return;

        //Todo: Activate after DxTextRectangle implementation
        /*if (!this.draw) {
            this.draw = new DxTextRectangle(_dxHandler, ModAPI, _timerHandler, GetWarning().ToString(), 0, 0, 1, 1, Color.FromArgb(255, 255, 255), Color.FromArgb(40, 200, 0, 0), 1.2f,
                frontPriority: 1, relativePos: true);
        } else {
            this.draw.setText(this.getWarning());
        }*/
    }

    private checkStart(currentPos: alt.Vector3) {
        const previousPos = this.lastPos;
        this.lastPos = currentPos;
        if (currentPos.distanceTo(previousPos) <= neededDistanceToBeNotAFK) {
            this.isAFKStart();
        }
    }

    private checkStop(currentPos: alt.Vector3) {
        if (currentPos.distanceTo(this.afkStartPos) > neededDistanceToBeNotAFK) {
            this.stopAFK();
        }
    }

    private stopAFK() {
        if (!this.kickInterval.isRunning) {
            return;
        }
        this.kickInterval.stop();
        alt.clearEveryTick(this.onTickId);
        //Todo: Activate after DxTextRectangle implementation
        /*if (this.draw) {
            this.draw.remove();
            this.draw = undefined;
        }*/      
    }

    private stopCheck() {
        this.checkInterval.stop();
        this.stopAFK();
        this.bindsService.removeControl(Control.Attack, this.stopAFK.bind(this));
        this.bindsService.removeControl(Control.MeleeAttack1, this.stopAFK.bind(this));
    }

    private isAFKStart() {
        this.afkStartPos = alt.Player.local.pos;
        this.onTickId = alt.everyTick(this.onTick.bind(this));
        this.kickInterval.startOnce();
    }

    private isAFKEnd() {
        this.afkStartPos = undefined;
        this.stopCheck();
        this.chatService.output(this.settingsService.language.AFK_KICK_INFO);
        this.remoteEventsSender.send(ToServerEvent.LeaveLobby);
    }

    private isStillAFK() {
        if (!this.canBeAFK()) {
            return false;
        }

        const currentPos = alt.Player.local.pos;
        if (currentPos.distanceTo(this.afkStartPos) > neededDistanceToBeNotAFK)
            return false;

        return true;
    }

    private canBeAFK(): boolean {
        return this.fightService.inFight
            && this.settingsService.playerSettings.CheckAFK
            && game.isPlayerPlaying(alt.Player.local.scriptID)
            && !game.isPlayerClimbing(alt.Player.local.scriptID)
            && !game.isPlayerFreeAiming(alt.Player.local.scriptID);
    }

    private getWarning(): string {
        const secsLeft = this.kickInterval.remainingSecToExec;
        return this.settingsService.language.AFK_KICK_WARNING.format(secsLeft);
    }

}
