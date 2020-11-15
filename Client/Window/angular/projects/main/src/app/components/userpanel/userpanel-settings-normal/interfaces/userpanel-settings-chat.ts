import { SettingsChatIndex } from '../enums/settings-chat-index.enum';

export interface UserpanelSettingsChat {
    /** ChatWidth */
    [SettingsChatIndex.ChatWidth]: number;

    /** ChatMaxHeight */
    [SettingsChatIndex.ChatMaxHeight]: number;

    /** ChatFontSize */
    [SettingsChatIndex.ChatFontSize]: number;

    /** HideDirtyChat */
    [SettingsChatIndex.HideDirtyChat]: boolean;

    /** ShowCursorOnChatOpen */
    [SettingsChatIndex.ShowCursorOnChatOpen]: boolean;

    /** HideChatInfo */
    [SettingsChatIndex.HideChatInfo]: boolean;

    /** ChatInfoFontSize */
    [SettingsChatIndex.ChatInfoFontSize]: number;

    /** ChatInfoMoveTimeMs */
    [SettingsChatIndex.ChatInfoMoveTimeMs]: number;
}