import { MatPaginatorIntl } from '@angular/material';
import { SettingsService } from '../services/settings.service';
import { Injectable, ChangeDetectorRef } from '@angular/core';

@Injectable()
export class CustomMatPaginatorIntl extends MatPaginatorIntl {
  constructor(
    private settings: SettingsService,
    private changeDetector: ChangeDetectorRef) {

    super();
    this.initLangs();

    settings.LanguageChanged.on(null, () => {
      this.initLangs();
      this.changeDetector.detectChanges();
    });
  }

  private initLangs() {
    this.firstPageLabel = this.settings.Lang.FirstPageLabel;
    this.itemsPerPageLabel = this.settings.Lang.ItemsPerPage;
    this.lastPageLabel = this.settings.Lang.LastPageLabel;
    this.nextPageLabel = this.settings.Lang.NextPageLabel;
    this.previousPageLabel = this.settings.Lang.PreviousPageLabel;
  }

  getRangeLabel = (page: number, pageSize: number, length: number) => {
    if (length === 0 || pageSize === 0) {
      return "0 " + this.settings.Lang.of + " " + length;
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    // If the start index exceeds the list length, do not try and fix the end index to the end.
    const endIndex = startIndex < length ?
      Math.min(startIndex + pageSize, length) :
      startIndex + pageSize;
    return startIndex + 1 + ' - ' + endIndex + ' ' + this.settings.Lang.of + ' ' + length;
  }
}
