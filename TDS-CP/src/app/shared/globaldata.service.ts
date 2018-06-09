import { Injectable } from "@angular/core";

@Injectable()
export class GlobalDataService {
    private static singleton: GlobalDataService;

    public readonly apiUrl = "http://194.95.0.48:4201";
    public showingPlayername = false;
    public readonly adminLvlNames = ["User", "Supporter", "Administrator", "Projectleader"];
    public readonly adminLvlColors = ["rgb(220,220,220)", "rgb(113,202,113)", "rgb(253,132,85)", "rgb(222,50,50)"];

    public readonly logTypes = ["login", "register", "chat", "error", "lobbyowner", "lobbyjoin", "vip", "admin", "report"];
    public readonly neededAdminLvlForLogTypes = {
        ["admin"]: 0,
        ["report"]: 0,
        ["login"]: 1,
        ["register"]: 1,
        ["chat"]: 2,
        ["error"]: 3,
        ["lobbyowner"]: 1,
        ["lobbyjoin"]: 1,
        ["vip"]: 1
    };
    public readonly adminLogTypes = ["permaban", "timeban", "unban", "permamute", "timemute", "unmute", "next", "kick", "lobbykick", "permabanlobby", "timebanlobby", "unbanlobby", "all"];

    get adminLvl() {
        return localStorage.getItem("adminlvl") || 0;
    }

    constructor() {
        if (!GlobalDataService.singleton) {
            GlobalDataService.singleton = this;
        }
        return GlobalDataService.singleton;
    }
}
