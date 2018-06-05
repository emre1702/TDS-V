import { Injectable } from "@angular/core";

@Injectable()
export class GlobalDataService {
    public readonly apiUrl = "http://localhost:5000";
    public showingUsername = false;
}
