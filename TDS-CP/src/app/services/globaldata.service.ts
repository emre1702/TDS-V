import { Injectable } from "@angular/core";

@Injectable()
export class GlobalDataService {
    private static singleton: GlobalDataService;

    public readonly apiUrl = "http://194.95.0.48:4201";
    public showingPlayername = false;
    public readonly adminLvlNames = ["User", "Supporter", "Administrator", "Projectleader"];
    public readonly adminLvlColors = ["rgb(220,220,220)", "rgb(113,202,113)", "rgb(253,132,85)", "rgb(222,50,50)"];

    public readonly logTypes = {
        ["admin"]: ["permaban", "timeban", "unban", "permamute", "timemute", "unmute", "next", "kick", "lobbykick", "permabanlobby", "timebanlobby", "unbanlobby", "all"],
        ["report"]: [],
        ["rest"]: ["login", "register", "chat", "error", "lobbyowner", "lobbyjoin", "vip"]
    };
    public readonly neededAdminLvlForLogTypes = {
        ["login"]: 1,
        ["register"]: 1,
        ["chat"]: 2,
        ["error"]: 3,
        ["lobbyowner"]: 1,
        ["lobbyjoin"]: 1,
        ["vip"]: 1,

        ["permaban"]: 0,
        ["timeban"]: 0,
        ["unban"]: 0,
        ["permamute"]: 0,
        ["timemute"]: 0,
        ["unmute"]: 0,
        ["next"]: 0,
        ["kick"]: 0,
        ["lobbykick"]: 0,
        ["permabanlobby"]: 0,
        ["timebanlobby"]: 0,
        ["unbanlobby"]: 0,
        ["all"]: 0,
    };

    get adminLvl(): number {
        return parseInt(localStorage.getItem("adminlvl"), 10) || 0;
    }

    public readonly showLogEntriesPerPage = 25;


    constructor() {
        if (!GlobalDataService.singleton) {
            GlobalDataService.singleton = this;
        }
        return GlobalDataService.singleton;
    }
}