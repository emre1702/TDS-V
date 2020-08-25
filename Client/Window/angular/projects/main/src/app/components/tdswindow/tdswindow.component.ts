import { Component, OnInit, ChangeDetectionStrategy, Input, ChangeDetectorRef, Output, EventEmitter, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { MatMenuPanel } from '@angular/material';
import { SettingsService } from '../../services/settings.service';
import { UserpanelSettingKey } from '../userpanel/enums/userpanel-setting-key.enum';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'tdswindow',
    templateUrl: './tdswindow.component.html',
    styleUrls: [
        './styles/window/window.design1.scss',

        './styles/toolbar/toolbar.design1.scss',
        './styles/toolbar/toolbar.design2.scss'
    ],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TDSWindowComponent implements OnInit, OnDestroy {

    private _isLoading: boolean;
    get isLoading(): boolean {
        return this._isLoading;
    }

    @Input("isLoading")
    set isLoading(value: boolean) {
        this._isLoading = value;
        this.changeDetector.detectChanges();
    }


    private _title: string;
    get title(): string {
        return this._title;
    }

    @Input("title")
    set title(value: string) {
        this._title = value;
        this.changeDetector.detectChanges();
    }


    private _navMenu: MatMenuPanel;
    get navMenu(): MatMenuPanel {
        return this._navMenu;
    }

    @Input("navMenu")
    set navMenu(value: MatMenuPanel) {
        this._navMenu = value;
        this.changeDetector.detectChanges();
    }

    private _navGlow: () => boolean;
    get navGlow(): () => boolean {
        return this._navGlow;
    }

    @Input("navGlow")
    set navGlow(value: () => boolean) {
        this._navGlow = value;
        this.changeDetector.detectChanges();
    }

    private _currentNav: string;
    get currentNav(): string {
        return this._currentNav;
    }

    @Input("currentNav")
    set currentNav(value: string) {
        this._currentNav = value;
        this.changeDetector.detectChanges();
    }

    private _showBack = true;
    get showBack(): boolean {
        return this._showBack;
    }

    @Input("showBack")
    set showBack(value: boolean) {
        this._showBack = value;
        this.changeDetector.detectChanges();
    }

    private _hideToolbar: boolean;
    get hideToolbar(): boolean {
        return this._hideToolbar;
    }

    @Input("hideToolbar")
    set hideToolbar(value: boolean) {
        this._hideToolbar = value;
        this.changeDetector.detectChanges();
    }

    private _showSave = true;
    get showSave(): boolean {
        return this._showSave;
    }

    @Input("showSave")
    set showSave(value: boolean) {
        this._showSave = value;
        this.changeDetector.detectChanges();
    }

    private _contentHeight = "auto";
    get contentHeight(): string {
        return this._contentHeight;
    }

    @Input("contentHeight")
    set contentHeight(value: string) {
        this._contentHeight = value;
        this.changeDetector.detectChanges();
    }

    windowDesign = 1;
    toolbarDesign = 1;

    // tslint:disable-next-line: no-output-native
    @Output() close = new EventEmitter();
    @Output() back = new EventEmitter();
    @Output() save = new EventEmitter();

    @ViewChild('toolbar1') toolbar1: TemplateRef<any>;
    @ViewChild('toolbar2') toolbar2: TemplateRef<any>;

    constructor(private changeDetector: ChangeDetectorRef, private settings: SettingsService) { }

    ngOnInit(): void {
        this.settings.ThemeSettingChanged.on(null, this.themeChanged.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.ThemeSettingChanged.off(null, this.themeChanged.bind(this));
        this.settings.ThemeSettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    private themeChanged(key: UserpanelSettingKey, value: any) {
        if (key == UserpanelSettingKey.ToolbarDesign) {
            this.toolbarDesign = value;
        }
        this.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}