import { UserpanelSupportType } from '../enums/userpanel-support-type.enum';
import { UserpanelSupportRequestMessageData } from './userpanelSupportRequestMessageData';

export interface UserpanelSupportRequestData {
    /** ID */
    0: number;

    /** Title */
    1: string;

    /** Messages */
    2: UserpanelSupportRequestMessageData[];

    /** Type */
    3: UserpanelSupportType;

    /** AtleastAdminLevel */
    4: number;

    /** Closed */
    5: boolean;
}
