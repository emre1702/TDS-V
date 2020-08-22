import KeyPressState from "../../enums/input/key-press-state.enum";
import Key from "../../enums/input/key.enum";

export default class KeyBind {
    method: (key: Key) => void;
    onPressState: KeyPressState;

    get onDown(): boolean {
        return this.onPressState == KeyPressState.Both || this.onPressState == KeyPressState.Down;
    }

    get onUp(): boolean {
        return this.onPressState == KeyPressState.Both || this.onPressState == KeyPressState.Up;
    }

    constructor(method: (key: Key) => void, onPressState: KeyPressState) {
        this.method = method;
        this.onPressState = onPressState;
    }
}
