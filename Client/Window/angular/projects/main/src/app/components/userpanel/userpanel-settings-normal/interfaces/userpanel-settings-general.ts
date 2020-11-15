import { SettingsGeneralIndex } from '../enums/settings-general-index.enum';
import { LanguageEnum } from '../../../../enums/language.enum';

export interface UserpanelSettingsGeneral {
    [SettingsGeneralIndex.Language]: LanguageEnum;
    [SettingsGeneralIndex.AllowDataTransfer]: boolean;
    [SettingsGeneralIndex.ShowConfettiAtRanking]: boolean;
    [SettingsGeneralIndex.Timezone]: string;
    [SettingsGeneralIndex.DateTimeFormat]: string;
    [SettingsGeneralIndex.DiscordUserId]?: number;
    [SettingsGeneralIndex.CheckAFK]: boolean;
    [SettingsGeneralIndex.WindowsNotifications]: boolean;
}