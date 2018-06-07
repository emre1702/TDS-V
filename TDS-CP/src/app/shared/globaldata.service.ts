import { Injectable } from "@angular/core";

@Injectable()
export class GlobalDataService {
    public readonly apiUrl = "http://194.95.0.48:4201";
    public showingPlayername = false;
    public adminLvlNames = ["User", "Supporter", "Administrator", "Projectleader"];
    public adminLvlColors = ["rgb(220,220,220)", "rgb(113,202,113)", "rgb(253,132,85)", "rgb(222,50,50)"];
}
