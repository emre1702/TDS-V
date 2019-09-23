import { LanguageEnum } from '../../../enums/language.enum';
import { MapCreatorPosition } from './mapCreatorPosition';
import { MapCreateSettings } from './mapCreateSettings';

export class MapCreateDataDto {
    public Id = 0;
    public Name = "";
    public Type = 0;
    public Settings: MapCreateSettings = new MapCreateSettings();
    public Description: {
        [key in LanguageEnum]: string
    } = { [LanguageEnum.German]: "", [LanguageEnum.English]: "" };
    public Objects: MapCreatorPosition[] = [];
    public TeamSpawns: MapCreatorPosition[][] = [[]];
    public MapEdges: MapCreatorPosition[] = [];
    public BombPlaces: MapCreatorPosition[] = [];
    public MapCenter: MapCreatorPosition;
}
