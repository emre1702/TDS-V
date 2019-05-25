import { LanguageEnum } from 'src/app/enums/language.enum';

export class MapDataDto {
    public Id: number;
    public Name: string;
    public Type: number;
    public Description: {
        [key in LanguageEnum]: string
    };
    public CreatorName: string;
    public Rating: number;
}
