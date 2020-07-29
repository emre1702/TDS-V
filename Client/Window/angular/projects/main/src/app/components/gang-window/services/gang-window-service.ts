import { Injectable } from '@angular/core';
import { GangMember } from '../models/gang-member';
import { GangPermissions } from '../models/gang-permissions';
import { GangData } from '../models/gang-data';

@Injectable()
export class GangWindowService {

    gangData: GangData = {
        0: "Bonus Gang",
        1: "BG",
        2: "rgb(255, 0, 0)",
        3: 0,
        98: 1,
        99: 1
    };

    permissions: GangPermissions = {
        0: true, 1: true, 2: true, 3: true, 4: 4, 5: false
    };

    members: GangMember[] = [
        { 0: 1, 1: "Bonus", 2: "19.04.2020 12:23:33", 3: "10.07.2020 12:32:32", 4: true, 5: 5, 6: 1, 7: 1 },
        { 0: 2, 1: "Bonus 2", 2: "20.05.2020 12:23:33", 3: "14.07.2020 12:32:32", 4: true, 5: 4, 6: 2, 7: 2 },
        { 0: 3, 1: "Bonus 3", 2: "19.06.2020 12:23:33", 3: "15.07.2020 12:32:32", 4: false, 5: 2, 6: 3, 7: 3 },
        { 0: 4, 1: "Bonus 4", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: false, 5: 3, 6: 4, 7: 4 },
        { 0: 5, 1: "Bonus5fdsgdsf", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: true, 5: 1, 6: 4, 7: 4 },
        { 0: 5, 1: "Bonus 5", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: true, 5: 1, 6: 4, 7: 4 },
        { 0: 5, 1: "Bonus 5", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: true, 5: 1, 6: 4, 7: 4 },
        { 0: 5, 1: "Bonus 5", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: true, 5: 1, 6: 4, 7: 4 },
        { 0: 5, 1: "Bonus 5", 2: "19.07.2020 12:23:33", 3: "19.07.2020 12:32:32", 4: true, 5: 1, 6: 4, 7: 4 },
    ];
}
