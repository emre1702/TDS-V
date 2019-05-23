import { Pipe, PipeTransform } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { Language } from '../interfaces/language.interface';

@Pipe({
  name: 'language'
})
export class LanguagePipe implements PipeTransform {

  transform(index: string, lang: Language): any {
    return lang[index] || index;
  }

}
