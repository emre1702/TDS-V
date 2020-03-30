import { UserpanelCommandSyntaxDto } from './userpanelCommandSyntaxDto';

export interface UserpanelCommandDataDto {
    /** Command */
    [0]: string;
    /** MinAdminLevel */
    [1]?: number;
    /** MinDonation */
    [2]?: number;
    /** VIPCanUse */
    [3]: boolean;
    /** LobbyOwnerCanUse */
    [4]: boolean;

    /** Syntaxes */
    [5]: UserpanelCommandSyntaxDto[];
    /** Aliases */
    [6]: string[];
    /** Description */
    [7]: { [language: number]: string };
}
