import { LanguageEnum } from '../../../enums/language.enum';
import { MapCreatorPosition } from './mapCreatorPosition';

export class MapCreateDataDto {
    public Id = 0;
    public Name = "";
    public Type = 0;
    public MinPlayers = 0;
    public MaxPlayers = 999;
    public Description: {
        [key in LanguageEnum]: string
    } = { [7]: "", [9]: "" };
    public Objects: MapCreatorPosition[] = [];
    public TeamSpawns: MapCreatorPosition[][] = [[]];
    public MapEdges: MapCreatorPosition[] = [];
    public BombPlaces: MapCreatorPosition[] = [];
    public MapCenter: MapCreatorPosition;
}
