import { Injectable } from '@angular/core';
import { LanguageEnum } from '../enums/language.enum';
import { German } from '../language/german.language';
import { English } from '../language/english.language';
import { Language } from '../interfaces/language.interface';
import { RageConnectorService } from './rage-connector.service';
import { DFromClientEvent } from '../enums/dfromclientevent.enum';
import { DToClientEvent } from '../enums/dtoclientevent.enum';

// tslint:disable: member-ordering

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

    ///////////////////// Language /////////////////////
    public LangValue: LanguageEnum = LanguageEnum.English;
    public Lang: Language = SettingsService.langByLangValue[this.LangValue];

    private static langByLangValue = {
      [LanguageEnum.German]: new German(),
      [LanguageEnum.English]: new English()
    };

    private loadLanguage(lang: LanguageEnum) {
      this.LangValue = lang;
      this.Lang = SettingsService.langByLangValue[lang];
    }
    ////////////////////////////////////////////////////

    ////////////////// Map Favourites //////////////////
    public FavoriteMapIDs: Set<number> = new Set<number>();

    public toggleMapIdToFavorite(id: number) {
      if (this.FavoriteMapIDs.has(id)) {
        this.FavoriteMapIDs.delete(id);
        this.rageConnector.call(DToClientEvent.ToggleMapFavorite, id, false);
      } else {
        this.FavoriteMapIDs.add(id);
        this.rageConnector.call(DToClientEvent.ToggleMapFavorite, id, true);
      }
    }

    public loadFavoriteMapIds(idsJson: string) {
      this.FavoriteMapIDs = JSON.parse(idsJson);
    }
    ////////////////////////////////////////////////////

    constructor(private rageConnector: RageConnectorService) {
      console.log("Settings listener started.");
      rageConnector.listen(DFromClientEvent.LoadLanguage, this.loadLanguage.bind(this));
      rageConnector.listen(DFromClientEvent.LoadFavoriteMaps, this.loadFavoriteMapIds.bind(this));
    }
}
