import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MapVotingComponent } from './components/mapvoting/mapvoting.component';

import { DragDropModule } from '@angular/cdk/drag-drop';

import {
  MatButtonModule,
  MatToolbarModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatFormFieldModule,
  MatInputModule,
  MatSelectModule,
  MatOptionModule,
  MatTableModule,
  MatAutocompleteModule,
  MatSortModule,
  MatDialogModule,
  MatSnackBarModule,
  MatRippleModule,
  MatSliderModule,
  MatExpansionModule,
  MatGridListModule,
  MatCheckboxModule,
  MatMenuModule,
  MatBadgeModule,
  MatTooltipModule,
  MatSlideToggleModule,
  MatButtonToggleModule,
  MatPaginatorModule,
  MatPaginatorIntl,
  MatStepperModule,
  MatProgressSpinnerModule,
  MatCardModule
} from "@angular/material";
import { MapVotingNavPipe } from './components/mapvoting/pipes/mapvotingNav.pipe';
import { LanguagePipe } from './pipes/language.pipe';
import { TeamOrdersComponent } from './components/teamorders/teamorders.component';
import { OrderByPipe } from './pipes/orderby.pipe';
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
import { UserpanelStatsComponent } from './components/userpanel/userpanel-stats/userpanel-stats.component';
import { CustomMatPaginatorIntl } from './extensions/customMatPaginatorIntl';
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
import { CarouselComponent } from './components/carousel/carousel.component';
import { FilterPipe } from './pipes/filter.pipe';
import { ReversePipe } from './pipes/reverse.pipe';
import { UserpanelOfflineMessagesComponent } from './components/userpanel/userpanel-offline-messages/userpanel-offline-messages.component';
import { InvitationComponent } from './components/utils/invitation/invitation.component';
import { UserpanelSettingsSpecialComponent } from './components/userpanel/userpanel-settings-special/userpanel-settings-special.component';
import { RoundStatsComponent } from './components/hud/round-stats/round-stats.component';
import { ClipboardModule } from 'ngx-clipboard';
import { HudComponent } from './components/hud/hud.component';
import { CustomLobbyMapsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-maps-menu/custom-lobby-maps-menu.component';
import { ChatComponent } from './components/hud/chat/chat.component';
import { CustomLobbyWeaponsMenuComponent } from './components/lobbychoice/custom-lobby/custom-lobby-weapons-menu/custom-lobby-weapons-menu.component';
import { ToolbarDirective } from './extensions/toolbarDirective';
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
import { MaterialCssVarsModule } from 'angular-material-css-vars';
import { MatAppBackgroundDirective } from './extensions/matAppBackgroundDirective';

@NgModule({
  declarations: [
    AppComponent,
    MapVotingComponent,
    TeamOrdersComponent,
    MapCreatorComponent,
    FreeroamComponent,
    CustomLobbyMenuComponent,
    OrderByPipe,
    MapVotingNavPipe,
    UserpanelCommandNavPipe,
    UserpanelRulesNavPipe,
    LanguagePipe,
    FilterPipe,
    ReversePipe,
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
    UserpanelStatsComponent,
    UserpanelApplicationComponent,

    InputTypeDirective,
    TextareaTypeDirective,
    ToolbarDirective,
    MentionDirective,
    MatAppBackgroundDirective,
    MentionListComponent,
    RankingComponent,
    CustomLobbyTeamsMenuComponent,
    UserpanelApplicationsComponent,
    UserpanelAdminQuestionsComponent,
    UserpanelSupportUserComponent,
    UserpanelSupportAdminComponent,
    UserpanelSupportViewComponent,
    CarouselComponent,
    UserpanelOfflineMessagesComponent,
    InvitationComponent,
    UserpanelSettingsSpecialComponent,
    RoundStatsComponent,
    HudComponent,
    CustomLobbyMapsMenuComponent,
    ChatComponent,
    CustomLobbyWeaponsMenuComponent,
    CharCreatorComponent,
    CharCreatorMainmenuComponent,
    CharCreatorHeritageComponent,
    CharCreatorFeaturesComponent,
    CharCreatorAppearanceComponent,
    CharCreatorHairandcolorsComponent,
    UserpanelSettingsCommandsComponent
  ],
  imports: [
    ReactiveFormsModule,
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    DragDropModule,
    ColorPickerModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatOptionModule,
    MatTableModule,
    MatAutocompleteModule,
    MatSortModule,
    MatDialogModule,
    MatRippleModule,
    MatSliderModule,
    MatExpansionModule,
    MatGridListModule,
    MatCheckboxModule,
    MatMenuModule,
    MatBadgeModule,
    MatTooltipModule,
    MatSlideToggleModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    MatStepperModule,
    MatProgressSpinnerModule,
    MatCardModule,
    ClipboardModule,
    OverlayModule,
    CommonModule,
    HttpClientModule,

    MaterialCssVarsModule.forRoot({
        isAutoContrast: true,
        isDarkTheme: true,
        primary: "rgba(0,0,77,1)",
        accent: "rgba(255,152,0,1)",
        warn: "rgba(244,67,54,1)"
    })
  ],
  entryComponents: [LoadMapDialog, AreYouSureDialog, CustomLobbyPasswordDialog, ApplicationInviteDialog, MentionListComponent],
  providers: [
    OrderByPipe,
    { provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
