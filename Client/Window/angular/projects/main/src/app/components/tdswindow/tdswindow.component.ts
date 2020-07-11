import { Component, OnInit, ChangeDetectionStrategy, Input, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { MatMenuPanel } from '@angular/material';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'tdswindow',
    templateUrl: './tdswindow.component.html',
    styleUrls: ['./tdswindow.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TDSWindowComponent implements OnInit {

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

    design = 1;

    // tslint:disable-next-line: no-output-native
    @Output() close = new EventEmitter();
    @Output() back = new EventEmitter();

    constructor(private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void {
    }

}
