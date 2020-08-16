import alt from "alt-client";
import natives from "natives";
import Control from "../../data/enums/input/control.enum";
import ControlBind from "../../data/models/input/control-bind.model";
import KeyBind from "../../data/models/input/key-bind.model";
import Key from "../../data/enums/input/key.enum";
import KeyPressState from "../../data/enums/input/key-press-state.enum";
import LoggingService from "../output/logging.service";
import { inject } from "inversify";
import InputGroup from "../../data/enums/input/input-group.enum";


export default class BindsService {

    private readonly bindedControls: { control: Control, binds: ControlBind[] }[] = [];
    private readonly bindedKeys: { key: Key, binds: KeyBind[] }[] = [];

    private readonly lastControlPressedState: { [control: number]: boolean } = {};


    constructor(
        @inject(LoggingService) private loggingService: LoggingService
    ) {
        alt.on("keydown", (key) => this.onKey(key, true));
        alt.on("keyup", (key) => this.onKey(key, false));
        alt.everyTick(this.onTick.bind(this));
    }


    addKey(key: Key, method: (key: Key) => void, pressState: KeyPressState = KeyPressState.Down) {
        var entry = this.bindedKeys.find(k => k.key == key);
        if (!entry) {
            entry = { key: key, binds: [] };
            this.bindedKeys.push(entry);
        }
        entry.binds.push(new KeyBind(method, pressState));
    }

    addControl(control: Control, method: (control: Control) => void, pressState: KeyPressState = KeyPressState.Down,
        onEnabled: boolean = true, onDisabled: boolean = false) {

        var entry = this.bindedControls.find(k => k.control == control);
        if (!entry) {
            entry = { control: control, binds: [] };
            this.bindedControls.push(entry);
        }
        entry.binds.push(new ControlBind(method, pressState, onEnabled, onDisabled));
    }

    removeKey(key: Key, method: (key: Key) => void = undefined, pressState: KeyPressState = KeyPressState.None) {
        var keyEntry = this.bindedKeys.find(k => k.key == key);
        if (!keyEntry) {
            return;
        }
        var entryIndex = keyEntry.binds.findIndex(b =>
            (!method || b.method == method) && (pressState == KeyPressState.None || b.onPressState == pressState));

        if (entryIndex >= 0) {
            keyEntry.binds.splice(entryIndex, 1);
            if (!keyEntry.binds.length) {
                const index = this.bindedKeys.indexOf(keyEntry);
                this.bindedKeys.splice(index, 1);
            }
        }
    }

    removeControl(control: Control, method: (control: Control) => void = undefined, pressState: KeyPressState = KeyPressState.None) {
        var controlEntry = this.bindedControls.find(k => k.control == control);
        if (!controlEntry) {
            return;
        }
        var entryIndex = controlEntry.binds.findIndex(b =>
            (!method || b.method == method) && (pressState == KeyPressState.None || b.onPressState == pressState));

        if (entryIndex >= 0) {
            controlEntry.binds.splice(entryIndex, 1);
            if (!controlEntry.binds.length) {
                const index = this.bindedControls.indexOf(controlEntry);
                this.bindedControls.splice(index, 1);
            }
        }
    }

    private onKey(key: number, isDown: boolean) {
        try {
            const keyData = this.bindedKeys.find(k => k.key == key);
            if (!keyData) {
                return;
            }

            const binds = keyData.binds.filter(b => isDown && b.onDown || !isDown && b.onUp);
            if (!binds.length) {
                return;
            }

            for (const bind of binds) {
                bind.method(key);
            }
        } catch (ex) {
            this.loggingService.logError(ex, "Bind on key failed");
        }
    }

    private onTick() {
        try {
            for (let i = this.bindedControls.length - 1; i >= 0; --i) {
                const entry = this.bindedControls[i];
                const isDownEnabled = natives.isControlPressed(InputGroup.Move, entry.control);
                const isDownDisabled = natives.isDisabledControlPressed(InputGroup.Move, entry.control);

                if (this.lastControlPressedState[entry.control] != null) {
                    if (this.lastControlPressedState[entry.control] === (isDownEnabled || isDownDisabled)) {
                        continue;
                    }
                }
                this.lastControlPressedState[entry.control] = isDownEnabled || isDownDisabled;

                for (let j = entry.binds.length - 1; j >= 0; --j) {
                    const bind = entry.binds[j];
                    if (bind.onDown && (isDownEnabled && bind.onEnabled || isDownDisabled && bind.onDisabled)
                            || bind.onUp && (!isDownEnabled && bind.onEnabled || !isDownDisabled && bind.onDisabled)) {
                        bind.method(entry.control);
                    }    
                }
            }
        } catch (ex) {
            this.loggingService.logError(ex, "Bind on control failed");
        }
       
    }
}
