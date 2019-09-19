import { SettingType } from '../../../enums/setting-type';
import { FormControl } from '@angular/forms';

export class LobbySettingRow {
  public type: SettingType;
  public formControl: FormControl;

  public enum?: any;
  public defaultValue: any;

  public onlyInt?: boolean;

  public dataSettingIndex: string;
}
