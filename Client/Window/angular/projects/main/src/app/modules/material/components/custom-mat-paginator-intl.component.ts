import { SettingsService } from '../../../services/settings.service';
import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { LanguagePipe } from '../../shared/pipes/language.pipe';

@Injectable()
export class CustomMatPaginatorIntl extends MatPaginatorIntl {
    private languagePipe = new LanguagePipe();

    constructor(private settings: SettingsService) {
        super();
        this.initLangs();

        settings.LanguageChanged.on(null, () => {
            this.initLangs();
        });
    }

    private initLangs() {
        this.firstPageLabel = this.languagePipe.transform('FirstPageLabel', this.settings.Lang);
        this.itemsPerPageLabel = this.languagePipe.transform('ItemsPerPage', this.settings.Lang);
        this.lastPageLabel = this.languagePipe.transform('LastPageLabel', this.settings.Lang);
        this.nextPageLabel = this.languagePipe.transform('NextPageLabel', this.settings.Lang);
        this.previousPageLabel = this.languagePipe.transform('PreviousPageLabel', this.settings.Lang);
    }

    getRangeLabel = (page: number, pageSize: number, length: number) => {
        if (length === 0 || pageSize === 0) {
            return '0 ' + this.languagePipe.transform('of', this.settings.Lang) + ' ' + length;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        // If the start index exceeds the list length, do not try and fix the end index to the end.
        const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
        return startIndex + 1 + ' - ' + endIndex + ' ' + this.languagePipe.transform('of', this.settings.Lang) + ' ' + length;
    };
}
