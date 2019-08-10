import { UserpanelRulesTarget } from '../enums/userpanel-rules-target.enum';
import { UserpanelRulesCategory } from '../enums/userpanel-rules-category.enum';

export interface UserpanelRuleDataDto {
    Id: number;
    Texts: {[language: number]: string};
    Target: number;
    Category: number;
}
