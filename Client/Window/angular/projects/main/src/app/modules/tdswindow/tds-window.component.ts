import { Component, OnInit, Input, ChangeDetectorRef, Output, EventEmitter, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { MatMenuPanel } from '@angular/material/menu';
import { SettingsThemeIndex } from '../../components/userpanel/userpanel-settings-normal/enums/settings-theme-index.enum';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'tds-window',
    templateUrl: './tds-window.component.html',
    styleUrls: ['./styles/window/window.design1.scss', './styles/toolbar/toolbar.design1.scss', './styles/toolbar/toolbar.design2.scss'],
})
export class TDSWindowComponent implements OnInit, OnDestroy {
    @Input() isLoading: boolean;
    @Input() title: string;
    @Input() navMenu: MatMenuPanel;
    @Input() navGlow: () => boolean;
    @Input() currentNav: string;
    @Input() hideToolbar: boolean;
    @Input() showBack = true;
    @Input() showSave = true;
    @Input() containerHeight = '100%';
    @Input() containerMaxHeight = 'initial';
    @Input() contentWidth = 'auto';
    @Input() contentHeight = 'auto';
    @Input() contentPadding = '10px';
    @Input() canMinimize = true;
    @Input() isDraggable = true;

    @Output() close = new EventEmitter();
    @Output() back = new EventEmitter();
    @Output() save = new EventEmitter();
    @Output() sideNavClick = new EventEmitter();

    @ViewChild('toolbar1') toolbar1: TemplateRef<any>;
    @ViewChild('toolbar2') toolbar2: TemplateRef<any>;

    windowDesign = 1;
    toolbarDesign = 1;

    constructor(private changeDetector: ChangeDetectorRef, private settings: SettingsService) {}

    ngOnInit(): void {
        this.settings.ThemeSettingChanged.on(null, this.themeChanged.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.ThemeSettingChanged.off(null, this.themeChanged.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    private themeChanged(key: SettingsThemeIndex, value: any) {
        if (key == SettingsThemeIndex.ToolbarDesign) {
            this.toolbarDesign = value;
        }
        this.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
