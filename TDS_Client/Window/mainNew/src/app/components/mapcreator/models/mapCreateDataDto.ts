import { LanguageEnum } from 'src/app/enums/language.enum';
import { Position4D } from './position4d';
import { Position3D } from './position3d';

export class MapCreateDataDto {
    public Name: string;
    public Type = 0;
    public MinPlayers = 0;
    public MaxPlayers = 999;
    public Description: {
        [key in LanguageEnum]: string
    } = { [7]: "", [9]: "" };
    public TeamSpawns: Position4D[][] = [];
    public MapEdges: Position3D[] = [];
    public BombPlaces: Position3D[] = [];
    public MapCenter: Position3D = new Position3D();
}
