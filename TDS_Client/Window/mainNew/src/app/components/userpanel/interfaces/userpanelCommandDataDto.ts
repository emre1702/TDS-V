import { UserpanelCommandSyntaxDto } from './userpanelCommandSyntaxDto';

export interface UserpanelCommandDataDto {
    Command: string;
    MinAdminLevel?: number;
    MinDonation?: number;
    VIPCanUse: boolean;
    LobbyOwnerCanUse: boolean;

    Syntaxes: UserpanelCommandSyntaxDto[];
    Aliases: string[];
    Description: { [language: number]: string };
}
