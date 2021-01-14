import { LanguageEnum } from '../../enums/language.enum';
import { MapType } from '../../enums/maptype.enum';
import { MapCreateDataKey } from './enums/map-create-data-key';
import { LocationData } from './models/location-data';
import { MapCreateData } from './models/map-create-data';
import { MapSharedLocation } from './models/map-shared-location';

export function createNewMap(): MapCreateData {
    const data: MapCreateData = {
        [MapCreateDataKey.Id]: 0,
        [MapCreateDataKey.Name]: '',
        [MapCreateDataKey.Type]: MapType.Normal,
        [MapCreateDataKey.Settings]: { 0: 0, 1: 999 },
        [MapCreateDataKey.Description]: { [LanguageEnum.German]: '', [LanguageEnum.English]: '' },
        [MapCreateDataKey.Objects]: [],
        [MapCreateDataKey.TeamSpawns]: [[]],
        [MapCreateDataKey.MapEdges]: [],
        [MapCreateDataKey.BombPlaces]: [],
        [MapCreateDataKey.MapCenter]: undefined,
        [MapCreateDataKey.Target]: undefined,
        [MapCreateDataKey.Vehicles]: [],
    };
    return data;
}

export function convertLocationDataToShared(locationData: LocationData): MapSharedLocation {
    return {
        name: locationData.name,
        hash: locationData.hash,
        ipls: locationData.ipls,
        iplsToUnload: locationData.iplsToUnload,
    };
}
