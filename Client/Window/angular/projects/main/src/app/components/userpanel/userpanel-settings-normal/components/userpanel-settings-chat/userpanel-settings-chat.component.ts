import { Component, OnInit } from '@angular/core';
import { SettingRow } from 'projects/main/src/app/modules/settings/models/setting-row';
import { ButtonRow } from 'projects/main/src/app/modules/settings/models/button-row';
import { BooleanSliderSettingRow } from 'projects/main/src/app/modules/settings/models/booleanslider-setting-row';
import { NumberSettingRow } from 'projects/main/src/app/modules/settings/models/number-setting-row';
import { NumberSliderSettingRow } from 'projects/main/src/app/modules/settings/models/numberslider-setting-row';
import { SettingChangedEvent } from 'projects/main/src/app/modules/settings/interfaces/setting-changed-event';
import { SettingsChatIndex } from '../../enums/settings-chat-index.enum';
import { UserpanelSettingsNormalService } from '../../services/userpanel-settings-normal.service';
import { UserpanelSettingsChat } from '../../interfaces/userpanel-settings-chat';
import { UserpanelSettingsNormalType } from '../../enums/userpanel-settings-normal-type.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-userpanel-settings-chat',
    templateUrl: './userpanel-settings-chat.component.html'
})
export class UserpanelSettingsChatComponent implements OnInit {
    userpanelSettings: (SettingRow | ButtonRow)[] = [
        new NumberSliderSettingRow({
            defaultValue: 30, dataSettingIndex: SettingsChatIndex.ChatWidth, 
            label: SettingsChatIndex[SettingsChatIndex.ChatWidth], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ChatWidthSettingInfo"
        }, 0, 100),
        new NumberSliderSettingRow({
            defaultValue: 35, dataSettingIndex: SettingsChatIndex.ChatMaxHeight, 
            label: SettingsChatIndex[SettingsChatIndex.ChatWidth], 
            containerGetter: this.getSettingsContainer.bind(this),

            tooltipLangKey: "ChatHeightSettingInfo"
        }, 0, 100),
        new NumberSliderSettingRow({
            defaultValue: 1.4, dataSettingIndex: SettingsChatIndex.ChatFontSize, 
            label: SettingsChatIndex[SettingsChatIndex.ChatFontSize], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ChatFontSizeSettingInfo"
        }, 0, 3, 0.01),
        new BooleanSliderSettingRow({ 
            defaultValue: false, dataSettingIndex: SettingsChatIndex.HideDirtyChat, 
            label: SettingsChatIndex[SettingsChatIndex.HideDirtyChat], 
            tooltipLangKey: "HideDirtyChatInfo",
            containerGetter: this.getSettingsContainer.bind(this),
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: true, dataSettingIndex: SettingsChatIndex.ShowCursorOnChatOpen, 
            label: SettingsChatIndex[SettingsChatIndex.ShowCursorOnChatOpen], 
            tooltipLangKey: "ShowCursorOnChatOpenInfo",
            containerGetter: this.getSettingsContainer.bind(this),
        }),
        new BooleanSliderSettingRow({ 
            defaultValue: false, dataSettingIndex: SettingsChatIndex.HideChatInfo, 
            label: SettingsChatIndex[SettingsChatIndex.HideChatInfo], 
            tooltipLangKey: "HideChatInfoInfo",
            containerGetter: this.getSettingsContainer.bind(this),
        }),
        new NumberSliderSettingRow({
            defaultValue: 1, dataSettingIndex: SettingsChatIndex.ChatInfoFontSize, 
            label: SettingsChatIndex[SettingsChatIndex.ChatInfoFontSize], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ChatInfoFontSizeInfo"
        }, 0, 5),
        new NumberSettingRow({ 
            defaultValue: 15000, dataSettingIndex: SettingsChatIndex.ChatInfoMoveTimeMs, 
            label: SettingsChatIndex[SettingsChatIndex.ChatInfoMoveTimeMs], 
            containerGetter: this.getSettingsContainer.bind(this),
            tooltipLangKey: "ChatInfoMoveTimeMsInfo"
        }, 50, 1000000, true, true),
    ];

    constructor(
        private userpanelSettingsService: UserpanelSettingsNormalService,
        private settings: SettingsService) {
    }

    ngOnInit() {
        const container = this.getSettingsContainer();
        for (const setting of this.userpanelSettings.filter(s => s instanceof SettingRow) as SettingRow[]) {
            setting.formControl.setValue(container[setting.dataSettingIndex]);
        }
    }

    save(obj: UserpanelSettingsChat) {
        this.userpanelSettingsService.save(UserpanelSettingsNormalType.Chat, obj).subscribe();
    }

    changed(data: SettingChangedEvent) {
        switch (data.index) {
            case SettingsChatIndex.ChatWidth:
                this.settings.Settings[0] = data.value;
                break;
            case SettingsChatIndex.ChatMaxHeight:
                this.settings.Settings[1] = data.value;
                break; 
            case SettingsChatIndex.ChatFontSize:
                this.settings.Settings[2] = data.value;
                break;
            case SettingsChatIndex.HideDirtyChat:
                this.settings.Settings[3] = data.value;
                break;
            case SettingsChatIndex.HideChatInfo:
                this.settings.Settings[4] = data.value;
                break;
            case SettingsChatIndex.ChatInfoFontSize:
                this.settings.Settings[5] = data.value;
                break;
            case SettingsChatIndex.ChatInfoMoveTimeMs:
                this.settings.Settings[6] = data.value;
                break;
        }
        this.settings.ChatSettingsChanged.emit(null);
    }

    private getSettingsContainer(): UserpanelSettingsChat {
        return this.userpanelSettingsService.loadedSettingsByType[UserpanelSettingsNormalType.Chat] as UserpanelSettingsChat;
    }
}   