import BrowserServiceBase from "./browser-service-base";
import EventsService from "../events/events.service";
import Language from "../../datas/interfaces/output/language.interface";
import { registerLoginBrowserPath } from "../../datas/constants";

export default class RegisterLoginBrowserService extends BrowserServiceBase {

    constructor(eventsService: EventsService) {
        super(registerLoginBrowserPath);
        eventsService.onLanguageChanged.on(this.syncLanguage.bind(this));
    }

    sendDataToBrowser(name: string, isRegistered: boolean, lang: Language) {
        this.execute("b", name, isRegistered, lang.LOGIN_REGISTER_TEXTS);
    }

    private syncLanguage(data: { language: Language, beforeLogin: boolean }) {
        this.execute("a", data.language.LOGIN_REGISTER_TEXTS);
    } 
}
