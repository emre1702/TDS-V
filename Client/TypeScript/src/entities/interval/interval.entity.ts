import alt from "alt-client";

export class Interval {
    get remainingMsToExec(): number {
        return Math.max(0, this.willExecuteAtMs - Date.now());
    }
    get remainingSecToExec(): number {
        return Math.ceil(this.remainingMsToExec / 1000);
    }
    get isRunning(): boolean {
        return this._isRunning;
    }

    private altId: number;
    private willExecuteAtMs: number;
    private _isRunning: boolean;
    private wasFasterStarted: boolean;
    private execAmount: number;

    constructor(private handler: () => void, private ms: number) {}

    /**
     * Start the interval.
     * @param ms If set: will start ONLY the first execute faster.
     */
    start(ms: number = this.ms) {
        this.stop();
        this.altId = alt.setInterval(this.exec.bind(this), ms);
        this.willExecuteAtMs = Date.now() + ms;
        this._isRunning = true;
        this.wasFasterStarted = this.ms !== ms;
        this.execAmount = 0
    }

    /**
     * Start the interval once.
     * @param ms If set: will start ONLY the first execute faster.
     */
    startOnce(ms: number = this.ms) {
        this.start(ms);
        this.execAmount = 1;
    }

    stop() {
        if (this.isRunning) {
            this._isRunning = false;
            alt.clearInterval(this.altId);
            this.willExecuteAtMs = Number.MAX_VALUE;
            this.altId = undefined;
        } 
    }

    setMs(ms: number) {
        this.ms = ms;
    }

    private exec() {
        this.handler();

        if (--this.execAmount <= 0) {
            this.stop();
            return;
        } 

        if (this.wasFasterStarted) {
            this.stop();
            this.start();
        } else {
            this.willExecuteAtMs = Date.now() + this.ms;
        }
       
    }
}
