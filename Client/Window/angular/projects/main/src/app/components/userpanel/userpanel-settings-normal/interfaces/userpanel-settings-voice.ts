import { SettingsVoiceIndex } from '../enums/settings-voice-index.enum';

export interface UserpanelSettingsVoice {
    [SettingsVoiceIndex.Voice3D]: boolean;
    [SettingsVoiceIndex.VoiceAutoVolume]: boolean;
    [SettingsVoiceIndex.VoiceVolume]: number;
}