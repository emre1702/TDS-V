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
  public AdminLevel = 0;
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
  ////////////////////////////////////////////////////

  constructor(private rageConnector: RageConnectorService) {
    console.log("Settings listener started.");
    rageConnector.listen(DFromClientEvent.LoadLanguage, this.loadLanguage.bind(this));
    rageConnector.listen(DFromClientEvent.LoadFavoriteMaps, this.loadFavoriteMapIds.bind(this));
    rageConnector.listen(DFromClientEvent.ToggleInFightLobby, this.toggleInFightLobby.bind(this));
    rageConnector.listen(DFromClientEvent.ToggleTeamOrderModus, this.toggleInTeamOrderModus.bind(this));
    rageConnector.listen(DFromClientEvent.ToggleChatOpened, this.setChatOpened.bind(this));

    this.LanguageChanged.setMaxListeners(9999);
  }
}
