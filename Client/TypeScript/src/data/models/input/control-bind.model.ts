import Control from "../../enums/input/control.enum";
import KeyPressState from "../../enums/input/key-press-state.enum";

export default class ControlBind {
    method: (control: Control) => void;
    onDisabled: boolean;
    onEnabled: boolean;
    onPressState: KeyPressState;

    get onDown(): boolean {
        return this.onPressState == KeyPressState.Both || this.onPressState == KeyPressState.Down;
    }

    get onUp(): boolean {
        return this.onPressState == KeyPressState.Both || this.onPressState == KeyPressState.Up;
    }

    constructor(method: (control: Control) => void, onPressState: KeyPressState, onEnabled: boolean, onDisabled: boolean) {
        this.method = method;
        this.onPressState = onPressState;
        this.onEnabled = onEnabled;
        this.onDisabled = onDisabled;
    }
}
