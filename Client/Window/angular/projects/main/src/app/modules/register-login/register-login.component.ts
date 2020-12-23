import { Component, ChangeDetectorRef, AfterContentChecked, OnInit, OnDestroy, Injector } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { RegisterLoginInitData } from './models/register-login-init-data';
import { RegisterLoginService } from './services/register-login.service';
import { tap } from 'rxjs/operators';
import { InitialDatas } from '../../initial-datas';
import { RegisterLoginDebugService } from './services/register-login.debug.service';
import { RegisterLoginProdService } from './services/register-login.prod.service';
import { RageConnectorService } from 'rage-connector';

@Component({
    selector: 'app-register-login',
    templateUrl: './register-login.component.html',
    styleUrls: ['./register-login.component.scss'],
    providers: [{ provide: RegisterLoginService, useFactory: createService, deps: [Injector] }],
})
export class RegisterLoginComponent implements AfterContentChecked, OnInit, OnDestroy {
    registerLoginInitData$ = this.service.loadRegisterLoginInitData().pipe(tap((registerLoginData) => this.onRegisterLoginDataLoaded(registerLoginData)));

    inTabIndex: number;
    isRegistered = true;

    constructor(public settings: SettingsService, private service: RegisterLoginService, private changeDetector: ChangeDetectorRef) {}

    ngAfterContentChecked() {
        this.detectChanges();
    }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    openPasswordForgotten() {
        this.inTabIndex = 2;
        this.detectChanges();
    }

    private onRegisterLoginDataLoaded(registerLoginData: RegisterLoginInitData) {
        this.isRegistered = registerLoginData[0];
        this.inTabIndex = this.isRegistered ? 0 : 1;
        this.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}

export function createService(injector: Injector) {
    if (InitialDatas.inDebug) {
        return new RegisterLoginDebugService();
    } else {
        return new RegisterLoginProdService(injector.get(RageConnectorService));
    }
}
