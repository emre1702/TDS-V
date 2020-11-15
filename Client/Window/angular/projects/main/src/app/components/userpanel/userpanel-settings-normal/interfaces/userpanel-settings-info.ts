import { SettingsInfoIndex } from '../enums/settings-info-index.enum';

export interface UserpanelSettingsInfo {
    [SettingsInfoIndex.ShowCursorInfo]: boolean;
    [SettingsInfoIndex.ShowLobbyLeaveInfo]: boolean;
}