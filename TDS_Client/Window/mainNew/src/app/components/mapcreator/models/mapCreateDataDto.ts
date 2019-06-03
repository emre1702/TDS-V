import { LanguageEnum } from 'src/app/enums/language.enum';
import { Position } from './position';

export class MapCreateDataDto {
    public Name: string;
    public Type = 0;
    public Description: {
        [key in LanguageEnum]: string
    } = { [7]: "", [9]: "" };
    public TeamSpawns: Position[][] = [];
    public MapEdges: Position[] = [];
    public BombPlaces: Position[] = [];
}
