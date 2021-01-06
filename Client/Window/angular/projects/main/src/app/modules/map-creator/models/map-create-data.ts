import { LanguageEnum } from '../../../enums/language.enum';
import { MapCreatorPosition } from './map-creator-position';
import { MapCreateSettings } from './map-create-settings';
import { MapType } from '../../../enums/maptype.enum';
import { MapCreateDataKey } from '../enums/map-create-data-key';

export interface MapCreateData {
    [MapCreateDataKey.Id]: number;
    [MapCreateDataKey.Name]: string;
    [MapCreateDataKey.Type]: MapType;
    [MapCreateDataKey.Settings]: MapCreateSettings;
    [MapCreateDataKey.Description]: {
        [key in LanguageEnum]: string;
    };
    [MapCreateDataKey.Objects]: MapCreatorPosition[];
    [MapCreateDataKey.TeamSpawns]: MapCreatorPosition[][];
    [MapCreateDataKey.MapEdges]: MapCreatorPosition[];
    [MapCreateDataKey.BombPlaces]: MapCreatorPosition[];
    [MapCreateDataKey.MapCenter]: MapCreatorPosition;
    [MapCreateDataKey.Target]: MapCreatorPosition;
    [MapCreateDataKey.Vehicles]: MapCreatorPosition[];
}
