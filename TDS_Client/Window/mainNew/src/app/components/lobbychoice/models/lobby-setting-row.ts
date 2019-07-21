import { LobbySettingType } from '../enums/lobby-setting-type';
import { FormControl } from '@angular/forms';

export class LobbySettingRow {
  public type: LobbySettingType;
  public formControl: FormControl;

  public enum?: any;
  public defaultValue: any;

  public onlyInt?: boolean;

  public dataSettingIndex: string;
}
