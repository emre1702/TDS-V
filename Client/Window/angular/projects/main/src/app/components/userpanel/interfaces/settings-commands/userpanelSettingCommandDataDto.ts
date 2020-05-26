import { UserpanelSettingCommandInitialDataDto } from './userpanelSettingCommandInitialDataDto';
import { UserpanelSettingCommandConfiguredDataDto } from './userpanelSettingCommandConfiguredDataDto';

export interface UserpanelSettingCommandDataDto {
    /** InitialCommands */
    0: UserpanelSettingCommandInitialDataDto[];

    /** AddedCommands */
    1: UserpanelSettingCommandConfiguredDataDto[];
}
