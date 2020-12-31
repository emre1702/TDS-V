import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { FromClientEvent } from '../../enums/from-client-event.enum';
import { ToClientEvent } from '../../enums/to-client-event.enum';
import { ToServerEvent } from '../../enums/to-server-event.enum';
import { InitialDatas } from '../../initial-datas';
import { SettingsService } from '../../services/settings.service';
import { BodyDebugService } from './body/services/body.debug.service';
import { BodyProdService } from './body/services/body.prod.service';
import { BodyService } from './body/services/body.service';
import { ClothesDebugService } from './clothes/services/clothes.debug.service';
import { ClothesProdService } from './clothes/services/clothes.prod.service';
import { ClothesService } from './clothes/services/clothes.service';
import { CharCreatorNav } from './enums/char-creator-nav.enum';

@Component({
    selector: 'app-char-creator',
    templateUrl: './char-creator.component.html',
    styleUrls: ['./char-creator.component.scss'],
    providers: [
        { provide: BodyService, useFactory: createBodyService, deps: [Injector] },
        { provide: ClothesService, useFactory: createClothesService, deps: [Injector] },
    ],
})
export class CharCreatorComponent {
    showCharCreator: boolean = InitialDatas.opened.charCreator;
    nav = CharCreatorNav.Main;
    charCreatorNav = CharCreatorNav;

    constructor(private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef, public settings: SettingsService) {
        rageConnector.listen(FromClientEvent.ToggleCharCreator, (bool: boolean) => {
            this.showCharCreator = bool;
            changeDetector.detectChanges();
        });
    }

    cancel() {
        this.rageConnector.callServer(ToServerEvent.CancelCharCreateData);
    }

    goToNav(nav: CharCreatorNav) {
        this.nav = nav;
        this.changeDetector.detectChanges();
    }

    goToMain() {
        this.nav = CharCreatorNav.Main;
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.CharCreatorResetCameraTarget);
    }
}

export function createBodyService(injector: Injector) {
    if (InitialDatas.inDebug) {
        return new BodyDebugService();
    } else {
        return new BodyProdService(injector.get(RageConnectorService));
    }
}

export function createClothesService(injector: Injector) {
    if (InitialDatas.inDebug) {
        return new ClothesDebugService();
    } else {
        return new ClothesProdService(injector.get(RageConnectorService));
    }
}
