import { UserpanelAdminQuestion } from './userpanelAdminQuestion';

export interface UserpanelAdminQuestionsGroup {
    /** AdminName */
    [0]: string;

    /** Questions */
    [1]: UserpanelAdminQuestion[];
}
