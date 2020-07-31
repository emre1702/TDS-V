import { GangPermissionSettings } from './gang-permission-settings';
import { GangRank } from '../../models/gang-rank';

export interface GangPermissionData {
    /** Ranks */
    0: GangRank[];

    /** PermissionSettings */
    1: GangPermissionSettings;
}
