import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';

export interface UserpanelSupportRequestListData {
    /** ID */
    0: number;

    /** PlayerName */
    1: string;

    /** CreateTime */
    2: string;

    /** Type */
    3: UserpanelSupportType;

    /** Title */
    4: string;

    /** Closed */
    5: boolean;
}
