import { Pipe, PipeTransform } from '@angular/core';
import { Language } from '../interfaces/language.interface';

@Pipe({
  name: 'language'
})
export class LanguagePipe implements PipeTransform {

  transform(index: string, lang: Language): any {
    return lang[index] || index;
  }

}
