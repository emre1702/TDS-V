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
                return list.filter(c => !c.MinAdminLevel && !c.MinDonation && !c.VIPCanUse && !c.LobbyOwnerCanUse);
            case UserpanelNavPage.CommandsTDSTeam:
                return list.filter(c => c.MinAdminLevel);
            case UserpanelNavPage.CommandsVIP:
                return list.filter(c => c.VIPCanUse);
            case UserpanelNavPage.CommandsDonator:
                return list.filter(c => c.MinDonation);
            case UserpanelNavPage.CommandsLobbyOwner:
                return list.filter(c => c.LobbyOwnerCanUse);
        }
        return list;
    }
}
