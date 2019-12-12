import { Injectable } from '@angular/core';
import { LanguageEnum } from '../enums/language.enum';
import { German } from '../language/german.language';
import { English } from '../language/english.language';
import { Language } from '../interfaces/language.interface';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../enums/dfromclientevent.enum';
import { DToClientEvent } from '../enums/dtoclientevent.enum';
import { EventEmitter } from 'events';

// tslint:disable: member-ordering

@Injectable({
    providedIn: 'root'
})
export class SettingsService {

    //////////////////// AdminLevel ////////////////////
    public AdminLevel = 3;
    public AdminLevelForApplicationInvites = 2;

    public AdminLevelChanged = new EventEmitter();

    public loadAdminLevel(adminLevel: number) {
        this.AdminLevel = adminLevel;
        this.AdminLevelChanged.emit(null);
    }

    ///////////////////// Language /////////////////////
    public LangValue: LanguageEnum = LanguageEnum.German;
    public Lang: Language = SettingsService.langByLangValue[this.LangValue];
    public LanguageChanged = new EventEmitter();

    private static langByLangValue = {
        [LanguageEnum.German]: new German(),
        [LanguageEnum.English]: new English()
    };

    public loadLanguage(lang: number) {
        this.LangValue = lang;
        this.Lang = SettingsService.langByLangValue[lang];
        this.LanguageChanged.emit(null);
    }
    ////////////////////////////////////////////////////

    ////////////////// Map Favourites //////////////////
    public FavoriteMapIDs: number[] = [];
    public FavoriteMapsChanged = new EventEmitter();

    public toggleMapIdToFavorite(id: number) {
        const index = this.FavoriteMapIDs.indexOf(id);
        if (index >= 0) {
            this.FavoriteMapIDs[index] = undefined;
            this.rageConnector.call(DToClientEvent.ToggleMapFavorite, id, false);
        } else {
            this.FavoriteMapIDs.push(id);
            this.rageConnector.call(DToClientEvent.ToggleMapFavorite, id, true);
        }
    }

    public loadFavoriteMapIds(idsJson: string) {
        this.FavoriteMapIDs = JSON.parse(idsJson);
        this.FavoriteMapsChanged.emit(null);
    }

    public isInFavorites(mapId: number) {
        return this.FavoriteMapIDs.indexOf(mapId) >= 0;
    }
    ////////////////////////////////////////////////////

    /////////////////////// Rest ///////////////////////
    public InTeamOrderModus = false;
    public InTeamOrderModusChanged = new EventEmitter();

    public ChatOpened = false;
    public ChatOpenedChange = new EventEmitter();

    public InFightLobby = false;
    public InFightLobbyChanged = new EventEmitter();

    public InUserLobbiesMenu = false;

    public MapBuyBasePrice = 0;
    public MapBuyCounterMultiplicator = 0;
    public MapsBoughtCounter = 0;
    public MapBuyStatsChanged = new EventEmitter();

    public Money = 0;
    public MoneyChanged = new EventEmitter();

    public IsLobbyOwner = false;
    public IsLobbyOwnerChanged = new EventEmitter();

    public TDSId: number;
    public RemoteId = 0;

    public AdminLevels = [
        { Level: 0, Name: "User", Color: "rgb(220,220,220)" },
        { Level: 1, Name: "Supporter", Color: "rgb(113,202,113)" },
        { Level: 2, Name: "Administrator", Color: "rgb(253,132,85)" },
        { Level: 3, Name: "Projectleader", Color: "rgb(222,50,50)" }
    ];

    public toggleInTeamOrderModus(bool: boolean) {
        this.InTeamOrderModus = bool;
        this.InTeamOrderModusChanged.emit(null);
    }

    public setChatOpened(bool: boolean) {
        this.ChatOpened = bool;
        this.ChatOpenedChange.emit(null);
    }

    public toggleInFightLobby(bool: boolean) {
        this.InFightLobby = bool;
        this.InFightLobbyChanged.emit(null);
    }

    public loadTDSId(tdsId: number) {
        this.TDSId = tdsId;
    }

    public loadRemoteId(remoteId: number) {
        this.RemoteId = remoteId;
    }

    private syncMapPriceData(mapBuyBasePrice: number, mapBuyCounterMultiplicator: number, mapsBoughtCounter: number) {
        this.MapBuyBasePrice = mapBuyBasePrice;
        this.MapBuyCounterMultiplicator = mapBuyCounterMultiplicator;
        this.MapsBoughtCounter = mapsBoughtCounter;
        this.MapBuyStatsChanged.emit(null);
    }

    private onMoneySync(money: number) {
        this.Money = money;
        this.MoneyChanged.emit(null);
    }

    private onSyncIsLobbyOwner(isLobbyOwner: boolean) {
        this.IsLobbyOwner = isLobbyOwner;
        this.IsLobbyOwnerChanged.emit(null);
    }
    ////////////////////////////////////////////////////

    constructor(private rageConnector: RageConnectorService) {
        console.log("Settings listener started.");
        rageConnector.listen(DFromClientEvent.LoadLanguage, this.loadLanguage.bind(this));
        rageConnector.listen(DFromClientEvent.LoadFavoriteMaps, this.loadFavoriteMapIds.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleInFightLobby, this.toggleInFightLobby.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleTeamOrderModus, this.toggleInTeamOrderModus.bind(this));
        rageConnector.listen(DFromClientEvent.ToggleChatOpened, this.setChatOpened.bind(this));
        rageConnector.listen(DFromClientEvent.SyncMapPriceData, this.syncMapPriceData.bind(this));
        rageConnector.listen(DFromClientEvent.SyncMoney, this.onMoneySync.bind(this));
        rageConnector.listen(DFromClientEvent.SyncIsLobbyOwner, this.onSyncIsLobbyOwner.bind(this));

        this.LanguageChanged.setMaxListeners(9999);
    }
}
