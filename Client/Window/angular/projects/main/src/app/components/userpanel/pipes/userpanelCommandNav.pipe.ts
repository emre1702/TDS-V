import { Pipe, PipeTransform } from "@angular/core";
import { UserpanelCommandDataDto } from '../interfaces/userpanelCommandDataDto';
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';

@Pipe({name: 'userpanelCommandNav'})
export class UserpanelCommandNavPipe implements PipeTransform {

    transform(list: UserpanelCommandDataDto[], currentNav: string) {
        if (!list || !currentNav || !list.length)
            return list;

        const currentNavEnum = UserpanelNavPage[currentNav] as UserpanelNavPage;
        switch (currentNavEnum) {
            case UserpanelNavPage.CommandsUser:
                return list.filter(c => !c[1] && !c[2] && !c[3] && !c[4]);
            case UserpanelNavPage.CommandsTDSTeam:
                return list.filter(c => c[1]);
            case UserpanelNavPage.CommandsVIP:
                return list.filter(c => c[3]);
            case UserpanelNavPage.CommandsDonator:
                return list.filter(c => c[2]);
            case UserpanelNavPage.CommandsLobbyOwner:
                return list.filter(c => c[4]);
        }
        return list;
    }
}
