import ToServerEvent from "../../datas/enums/events/to-server-event.enum";
import * as alt from "alt-client"

export default class LoggingService {

    private readonly outputLogInfo = false;

    constructor() { }

    logMessageToServer(msg: string, source: string = "") {
        alt.emitServer(ToServerEvent.LogMessageToServer, msg, source);
    }

    logErrorToServer(e: unknown, title?: string) {
        const eInfo = this.getErrorInfo(e);

        alt.emitServer(ToServerEvent.LogExceptionToServer, title ? `[${title}] ` + eInfo.message : eInfo.message, eInfo.stackTrace, eInfo.type);
    }

    logError(e: unknown, title?: string) {
        const eInfo = this.getErrorInfo(e);

        if (title) {
            alt.logError(title);
        } else {
            alt.logError("Exception occured");
        }
        alt.logError(eInfo.message, eInfo.stackTrace);
    }

    logInfo(msg: string, source: string = "", isEnd: boolean = false) {
        if (!this.outputLogInfo) {
            return;
        }

        if (source.length > 0) {
            source = "[" + source + "]";
        }
        if (isEnd) {
            source += "[END] ";
        } else {
            source += " ";
        }

        alt.log(source + msg);
    }

    logWarning(msg: string, source: string = "") {
        if (!this.outputLogInfo) {
            return;
        }

        if (source.length > 0) {
            source = "[" + source + "]";
        } else {
            source += " ";
        }

        alt.log(source + msg);
    }



    private getErrorInfo(e: unknown): { message: string, stackTrace: string, type: string } {
        const data = { message: "?", stackTrace: "?", type: "?" };
        if (typeof e === "string") {
            data.message = e;
            data.type = "string";
        } else if (e instanceof Error) {
            data.message = e.message;
            data.stackTrace = "?";
            data.type = "Error";
        } else {
            data.message += JSON.stringify(e);
        }

        return data;
    }

}
