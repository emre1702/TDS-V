import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import alt from "alt-client";
import SettingsService from "../settings/settings.service";
import BindsService from "./binds.service";
import Key from "../../datas/enums/input/key.enum";

@injectable()
export default class CursorService {
    private cursorOpenedCounter = 0;

    private _visible = false;
    get visible(): boolean {
        return this._visible;
    }
    set visible(show: boolean) {
        if (show) {
            if (++this.cursorOpenedCounter == 1) {
                alt.showCursor(true);
                this.eventsService.onCursorToggled.emit(true);
            }
        } else if (--this.cursorOpenedCounter <= 0) {
            alt.showCursor(false);
            this.cursorOpenedCounter = 0;
            this.eventsService.onCursorToggled.emit(false);
        }
    }

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService,
        @inject(DIIdentifier.SettingsService) private settingsService: SettingsService,
        @inject(DIIdentifier.BindsService) bindsService: BindsService
    ) {
        eventsService.onChatInputToggled.on(this.chatInputToggled.bind(this));
        eventsService.onCursorToggleRequested.on(b => this.visible = b);

        bindsService.addKey(Key.End, this.manuallyToggleCursor.bind(this));
    }


    private chatInputToggled(bool: boolean) {
        if (this.settingsService.playerSettings.ShowCursorOnChatOpen) {
            this.visible = bool;
        }
    }

    private manuallyToggleCursor() {
        this.cursorOpenedCounter = this.visible ? 0 : 1;
        this.visible = !this.visible;
    }
}
