import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MapVotingComponent } from './components/mapvoting/mapvoting.component';
import { MapVotingNavPipe } from './components/mapvoting/pipes/mapvotingNav.pipe';
import { TeamOrdersComponent } from './components/teamorders/teamorders.component';
import { MapCreatorComponent } from './components/mapcreator/map-creator.component';
import { FreeroamComponent } from './components/freeroam/freeroam.component';
import { LoadMapDialog } from './components/mapcreator/dialog/load-map-dialog';
import { AreYouSureDialog } from './dialog/are-you-sure-dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomLobbyMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby.component';
import { CustomLobbyPasswordDialog } from './components/lobbychoice/dialog/custom-lobby-password-dialog';
import { LobbyChoiceComponent } from './components/lobbychoice/lobby-choice/lobby-choice.component';
import { TeamChoiceComponent } from './components/team-choice/team-choice.component';
import { UserpanelComponent } from './components/userpanel/userpanel.component';
import { UserpanelCommandNavPipe } from './components/userpanel/pipes/userpanelCommandNav.pipe';
import { UserpanelCommandsComponent } from './components/userpanel/userpanel-commands/userpanel-commands.component';
import { UserpanelRulesComponent } from './components/userpanel/userpanel-rules/userpanel-rules.component';
import { UserpanelRulesNavPipe } from './components/userpanel/pipes/userpanelRulesNav.pipe';
import { UserpanelFAQsComponent } from './components/userpanel/userpanel-faqs/userpanel-faqs.component';
import { UserpanelSettingsNormalComponent } from './components/userpanel/userpanel-settings-normal/userpanel-settings-normal.component';
import { UserpanelStatsGeneralComponent } from './components/userpanel/userpanel-stats-general/userpanel-stats-general.component';
import { InputTypeDirective } from './extensions/inputTypeDirective';
import { TextareaTypeDirective } from './extensions/textareaTypeDirective';
import { RankingComponent } from './components/ranking/ranking.component';
import { ColorPickerModule } from 'ngx-color-picker';
import { CustomLobbyTeamsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-teams-menu/custom-lobby-teams-menu.component';
import { UserpanelApplicationComponent } from './components/userpanel/userpanel-application (user)/userpanel-application.component';
import { UserpanelApplicationsComponent } from './components/userpanel/userpanel-applications (admin)/userpanel-applications.component';
import { UserpanelAdminQuestionsComponent } from './components/userpanel/userpanel-admin-questions/userpanel-admin-questions.component';
import { ApplicationInviteDialog } from './dialog/application-invite-dialog';
import { UserpanelSupportUserComponent } from './components/userpanel/userpanel-support-user/userpanel-support-user.component';
import { UserpanelSupportAdminComponent } from './components/userpanel/userpanel-support-admin/userpanel-support-admin.component';
import { UserpanelSupportViewComponent } from './components/userpanel/userpanel-support-view/userpanel-support-view.component';
import { UserpanelOfflineMessagesComponent } from './components/userpanel/userpanel-offline-messages/userpanel-offline-messages.component';
import { InvitationComponent } from './components/utils/invitation/invitation.component';
import { UserpanelSettingsSpecialComponent } from './components/userpanel/userpanel-settings-special/userpanel-settings-special.component';
import { RoundStatsComponent } from './components/hud/round-stats/round-stats.component';
import { ClipboardModule } from 'ngx-clipboard';
import { HudComponent } from './components/hud/hud.component';
import { CustomLobbyMapsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-maps-menu/custom-lobby-maps-menu.component';
import { ChatComponent } from './components/hud/chat/chat.component';
import { CustomLobbyWeaponsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-weapons-menu/custom-lobby-weapons-menu.component';
import { MentionDirective } from './extensions/mention/mentionDirective';
import { MentionListComponent } from './extensions/mention/mentionListComponent';
import { OverlayModule } from '@angular/cdk/overlay';
import { CommonModule } from '@angular/common';
import { CharCreatorComponent } from './components/char-creator/char-creator.component';
import { CharCreatorMainmenuComponent } from './components/char-creator/menus/char-creator-mainmenu/char-creator-mainmenu.component';
import { CharCreatorHeritageComponent } from './components/char-creator/menus/char-creator-heritage/char-creator-heritage.component';
import { CharCreatorFeaturesComponent } from './components/char-creator/menus/char-creator-features/char-creator-features.component';
import { CharCreatorAppearanceComponent } from './components/char-creator/menus/char-creator-appearance/char-creator-appearance.component';
import { CharCreatorHairandcolorsComponent } from './components/char-creator/menus/char-creator-hairandcolors/char-creator-hairandcolors.component';
import { HttpClientModule } from '@angular/common/http';
import { UserpanelSettingsCommandsComponent } from './components/userpanel/userpanel-settings-commands/userpanel-settings-commands.component';
import { MaterialCssVarsModule, MaterialCssVarsService } from 'angular-material-css-vars';
import { InfosHandlerComponent } from './components/infos-handler/infos-handler.component';
import { InfoComponent } from './components/infos-handler/info/info.component';
import { UserpanelStatsWeaponComponent } from './components/userpanel/userpanel-stats-weapon/userpanel-stats-weapon.component';
import { GangWindowComponent } from './components/gang-window/gang-window.component';
import { GangWindowMembersComponent } from './components/gang-window/gang-window-members/gang-window-members.component';
import { GangWindowRankLevelsComponent } from './components/gang-window/gang-window-rank-levels/gang-window-rank-levels.component';
import { GangWindowCreateComponent } from './components/gang-window/gang-window-create/gang-window-create.component';
import { GangWindowGangInfoComponent } from './components/gang-window/gang-window-gang-info/gang-window-gang-info.component';
import { GangWindowVehiclesComponent } from './components/gang-window/gang-window-vehicles/gang-window-vehicles.component';
import { GangWindowAllGangsComponent } from './components/gang-window/gang-window-all-gangs/gang-window-all-gangs.component';
import { GangWindowRankPermissionsComponent } from './components/gang-window/gang-window-rank-permissions/gang-window-rank-permissions.component';
import { GangWindowMainmenuComponent } from './components/gang-window/gang-window-mainmenu/gang-window-mainmenu.component';
import { CustomLobbyArmsRaceWeaponsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-armsraceweapons-menu/custom-lobby-armsraceweapons-menu.component';
import { MainMenuComponent } from './components/lobbychoice/main-menu/main-menu.component';
import { DamageTestMenuComponent } from './components/damage-test-menu/damage-test-menu.component';
import { KillMessagesComponent } from './components/hud/kill-messages/kill-messages.component';
import { MaterialModule } from './modules/material/material.module';
import { SettingsModule } from './modules/settings/settings.module';
import { SharedModule } from './modules/shared/shared.module';
import { UserpanelSettingsChatComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-chat/userpanel-settings-chat.component';
import { UserpanelSettingsCooldownsAndDurationsComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-cooldowns-and-durations/userpanel-settings-cooldowns-and-durations.component';
import { UserpanelSettingsFightEffectComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-fight-effect/userpanel-settings-fight-effect.component';
import { UserpanelSettingsGeneralComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-general/userpanel-settings-general.component';
import { UserpanelSettingsInfoComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-info/userpanel-settings-info.component';
import { UserpanelSettingsIngameColorsComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-ingame-colors/userpanel-settings-ingame-colors.component';
import { UserpanelSettingsKillInfoComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-kill-info/userpanel-settings-kill-info.component';
import { UserpanelSettingsScoreboardComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-scoreboard/userpanel-settings-scoreboard.component';
import { UserpanelSettingsVoiceComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-voice/userpanel-settings-voice.component';
import { UserpanelSettingsThemeComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-theme/userpanel-settings-theme.component';
import { UserpanelSettingsHudComponent } from './components/userpanel/userpanel-settings-normal/components/userpanel-settings-hud/userpanel-settings-hud.component';
import { TDSWindowModule } from './modules/tds-window/tds-window.module';
import { VoiceInfoComponent } from './components/hud/voice-info/voice-info.component';
import { RegisterLoginModule } from './modules/register-login/register-login.module';

@NgModule({
    declarations: [
        AppComponent,
        MapVotingComponent,
        TeamOrdersComponent,
        MapCreatorComponent,
        FreeroamComponent,
        CustomLobbyMenuComponent,
        MapVotingNavPipe,
        UserpanelCommandNavPipe,
        UserpanelRulesNavPipe,
        LoadMapDialog,
        AreYouSureDialog,
        CustomLobbyPasswordDialog,
        ApplicationInviteDialog,
        LobbyChoiceComponent,
        TeamChoiceComponent,
        UserpanelComponent,
        UserpanelCommandsComponent,
        UserpanelRulesComponent,
        UserpanelFAQsComponent,
        UserpanelSettingsNormalComponent,
        UserpanelStatsGeneralComponent,
        UserpanelApplicationComponent,
        UserpanelSettingsChatComponent,
        UserpanelSettingsCooldownsAndDurationsComponent,
        UserpanelSettingsFightEffectComponent,
        UserpanelSettingsGeneralComponent,
        UserpanelSettingsInfoComponent,
        UserpanelSettingsIngameColorsComponent,
        UserpanelSettingsKillInfoComponent,
        UserpanelSettingsScoreboardComponent,
        UserpanelSettingsThemeComponent,
        UserpanelSettingsVoiceComponent,

        InputTypeDirective,
        TextareaTypeDirective,
        MentionDirective,
        MentionListComponent,
        RankingComponent,
        CustomLobbyTeamsMenuComponent,
        UserpanelApplicationsComponent,
        UserpanelAdminQuestionsComponent,
        UserpanelSupportUserComponent,
        UserpanelSupportAdminComponent,
        UserpanelSupportViewComponent,
        UserpanelOfflineMessagesComponent,
        InvitationComponent,
        UserpanelSettingsSpecialComponent,
        RoundStatsComponent,
        HudComponent,
        CustomLobbyMapsMenuComponent,
        ChatComponent,
        CustomLobbyWeaponsMenuComponent,
        CustomLobbyArmsRaceWeaponsMenuComponent,
        CharCreatorComponent,
        CharCreatorMainmenuComponent,
        CharCreatorHeritageComponent,
        CharCreatorFeaturesComponent,
        CharCreatorAppearanceComponent,
        CharCreatorHairandcolorsComponent,
        UserpanelSettingsCommandsComponent,
        InfosHandlerComponent,
        InfoComponent,
        UserpanelStatsWeaponComponent,
        GangWindowComponent,
        GangWindowMembersComponent,
        GangWindowRankLevelsComponent,
        GangWindowCreateComponent,
        GangWindowGangInfoComponent,
        GangWindowVehiclesComponent,
        GangWindowAllGangsComponent,
        GangWindowRankPermissionsComponent,
        GangWindowMainmenuComponent,
        MainMenuComponent,
        DamageTestMenuComponent,
        KillMessagesComponent,
        UserpanelSettingsHudComponent,
        VoiceInfoComponent,
    ],
    imports: [
        ReactiveFormsModule,
        FormsModule,
        BrowserModule,
        BrowserAnimationsModule,
        ColorPickerModule,
        ClipboardModule,
        OverlayModule,
        CommonModule,
        HttpClientModule,
        MaterialModule,
        SettingsModule,
        SharedModule,
        TDSWindowModule,
        RegisterLoginModule,

        MaterialCssVarsModule.forRoot({
            isAutoContrast: true,
            isDarkTheme: true,
            primary: 'rgba(0,0,77,1)',
            accent: 'rgb(255,152,0)',
            warn: 'rgba(244,67,54,1)',
        }),
    ],
    providers: [MaterialCssVarsService],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
