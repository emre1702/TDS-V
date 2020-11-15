import { Pipe, PipeTransform } from '@angular/core';
import { Language } from '../../../interfaces/language.interface';

@Pipe({
    name: 'language'
})
export class LanguagePipe implements PipeTransform {

    transform(index: string, lang: Language, ...formatReplace: any[]): any {
        const str = lang[index] || index;
        if (formatReplace) {
            return this.formatString(str, ...formatReplace);
        }
        return str;
    }

    formatString(str: string, ...val: any[]) {
        for (let index = 0; index < val.length; index++) {
            str = str.replace(`{${index}}`, String(val[index]));
        }
        return str;
    }
}
