import ToServerEvent from "../enums/events/to-server-event.enum";
import * as alt from "alt-client"


const _outputLogInfo = false;

export function logMessageToServer(msg: string, source: string = "") {
    alt.emitServer(ToServerEvent.LogMessageToServer, msg, source);
}

export function logErrorToServer(e: unknown, title?: string) {
    const eInfo = _getErrorInfo(e);

    alt.emitServer(ToServerEvent.LogExceptionToServer, title ? `[${title}] ` + eInfo.message : eInfo.message, eInfo.stackTrace, eInfo.type);
}

export function logError(e: unknown, title?: string) {
    const eInfo = _getErrorInfo(e);

    if (title) {
        alt.logError(title);
    } else {
        alt.logError("Exception occured");
    }
    alt.logError(eInfo.message, eInfo.stackTrace);
}

export function logInfo(msg: string, source: string = "", isEnd: boolean = false) {
    if (!_outputLogInfo) {
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

export function logWarning(msg: string, source: string = "") {
    if (!_outputLogInfo) {
        return;
    }

    if (source.length > 0) {
        source = "[" + source + "]";
    } else {
        source += " ";
    }

    alt.log(source + msg);
}



function _getErrorInfo(e: unknown): { message: string, stackTrace: string, type: string } {
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

