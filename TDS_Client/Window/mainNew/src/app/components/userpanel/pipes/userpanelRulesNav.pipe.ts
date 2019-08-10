import { Pipe, PipeTransform } from "@angular/core";
import { UserpanelNavPage } from '../enums/userpanel-nav-page.enum';
import { UserpanelRuleDataDto } from '../interfaces/userpanelRuleDataDto';
import { UserpanelRulesTarget } from '../enums/userpanel-rules-target.enum';

@Pipe({name: 'userpanelRulesNav'})
export class UserpanelRulesNavPipe implements PipeTransform {

    transform(list: UserpanelRuleDataDto[], currentNav: string) {
        if (!list || !currentNav || !list.length)
            return list;

        const currentNavEnum = UserpanelNavPage[currentNav] as UserpanelNavPage;
        switch (currentNavEnum) {
            case UserpanelNavPage.RulesUser:
                return list.filter(c => c.Target === UserpanelRulesTarget.User);
            case UserpanelNavPage.RulesTDSTeam:
                return list.filter(c => c.Target === UserpanelRulesTarget.Admin);
            case UserpanelNavPage.RulesVIP:
                return list.filter(c => c.Target === UserpanelRulesTarget.VIP);
        }
        return list;
    }
}
