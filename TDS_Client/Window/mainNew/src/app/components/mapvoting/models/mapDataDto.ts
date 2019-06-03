import { LanguageEnum } from 'src/app/enums/language.enum';

export class MapDataDto {
    public Id: number;
    public Name: string;
    public Type = 0;
    public Description: {
        [key in LanguageEnum]: string
    } = { [7]: "", [9]: "" };
    public CreatorName: string;
    public Rating: number;
}
