import { Component, ChangeDetectorRef, OnDestroy, OnInit } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { UserpanelNavPage } from './enums/userpanel-nav-page.enum';
import { UserpanelCommandDataDto } from './interfaces/userpanelCommandDataDto';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { UserpanelService } from './services/userpanel.service';
import { LanguagePipe } from '../../modules/shared/pipes/language.pipe';
import { InitialDatas } from '../../initial-datas';
import { UserpanelSettingsNormalType } from './userpanel-settings-normal/enums/userpanel-settings-normal-type.enum';
import { UserpanelSettingsNormalService } from './userpanel-settings-normal/services/userpanel-settings-normal.service';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-userpanel',
    templateUrl: './userpanel.component.html',
    styleUrls: ['./userpanel.component.scss'],
    providers: [UserpanelSettingsNormalService]
})
export class UserpanelComponent implements OnInit, OnDestroy {

    langPipe = new LanguagePipe();
    userpanelNavPage = UserpanelNavPage;
    userpanelSettingsNormalType = UserpanelSettingsNormalType;
    currentCommand: UserpanelCommandDataDto;
    myStatsGeneralColumns = {
        0:  "Id",
        1:  "Name",
        2:  "SCName",
        3:  "Gang",
        4:  "AdminLvl",
        5:  "Donation",
        6:  "IsVip",
        7:  "Money",
        8:  "TotalMoney",
        9:  "PlayTime",

        10:  "MuteTime",
        11:  "VoiceMuteTime",

        12:  "BansInLobbies",

        13:  "AmountMapsCreated",
        14:  "MapsRatedAverage",
        15:  "CreatedMapsAverageRating",
        16:  "AmountMapsRated",
        17:  "LastLogin",
        18: "RegisterTimestamp",
        19: "LobbyStats",
        20: "Logs"
    };

    private navigatedToSettingsNormal: boolean;

    constructor(public settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService,
        public userpanelService: UserpanelService,
        private userpanelSettingsNormalService: UserpanelSettingsNormalService) {

    }

    ngOnInit() {
        this.userpanelService.loadingData = false;
        this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.Main];
        this.settings.AdminLevelChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.myStatsGeneralLoaded.on(null, this.detectChanges.bind(this));
        this.userpanelService.loadingDataChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.currentNavChanged.on(null, this.detectChanges.bind(this));
        this.userpanelService.myStatsWeaponsUsedLoaded.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.AdminLevelChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.myStatsGeneralLoaded.off(null, this.detectChanges.bind(this));
        this.userpanelService.loadingDataChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.currentNavChanged.off(null, this.detectChanges.bind(this));
        this.userpanelService.myStatsWeaponsUsedLoaded.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));

        if (this.navigatedToSettingsNormal) {
            this.rageConnector.callServer(DToServerEvent.ReloadPlayerSettings);
        }
    }

    closeUserpanel() {
        this.userpanelService.loadingData = false;
        this.rageConnector.call(DToClientEvent.CloseUserpanel);
    }

    gotoNav(nav: UserpanelNavPage) {
        this.currentCommand = undefined;
        if (!InitialDatas.inDebug) {
            this.userpanelService.loadingData = true;
        }
        this.userpanelService.currentNav = UserpanelNavPage[nav];

        /*if (this.userpanelService.currentNav.startsWith("Commands") && !this.userpanelService.allCommands.length) {
            this.userpanelService.loadCommands();
        } else*/ if (this.userpanelService.currentNav.startsWith("Rules") && !this.userpanelService.allRules.length) {
            this.userpanelService.loadRules();
        } else if (nav == UserpanelNavPage.FAQ && !this.userpanelService.allFAQs.length) {
            this.userpanelService.loadFAQs();
        } else if (nav == UserpanelNavPage.SettingsSpecial) {
            this.userpanelService.loadSettingsSpecial();
        } else if (nav == UserpanelNavPage.SettingsCommands) {
            this.userpanelService.loadSettingsCommands();
        } else if (nav == UserpanelNavPage.MyStatsGeneral) {
            this.userpanelService.loadMyStatsGeneral();
        } else if (nav == UserpanelNavPage.MyStatsWeapon) {
            this.userpanelService.loadMyStatsWeapon();
        } else if (nav == UserpanelNavPage.Application) {
            this.userpanelService.loadApplicationPage();
        } else if (nav == UserpanelNavPage.Applications) {
            this.userpanelService.loadApplicationsPage();
        } else if (nav == UserpanelNavPage.SupportUser) {
            this.userpanelService.loadUserSupportRequests();
        } else if (nav == UserpanelNavPage.SupportAdmin) {
            this.userpanelService.loadSupportRequestsForAdmin();
        } else if (nav == UserpanelNavPage.OfflineMessages) {
            this.userpanelService.loadOfflineMessages();
        } else {
            this.userpanelService.loadingData = false;
            this.changeDetector.detectChanges();
        }
    }

    gotoSettingsNormalNav(nav: UserpanelSettingsNormalType) {
        this.navigatedToSettingsNormal = true;
        this.userpanelService.currentNav = UserpanelNavPage[UserpanelNavPage.SettingsNormal];
        this.userpanelService.currentNavChanged.emit(null);
        this.userpanelSettingsNormalService.navigateTo(nav);
    }

    getNavs(): Array<string> {
        const keys = Object.keys(UserpanelNavPage);
        return keys.slice(keys.length / 2);
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
