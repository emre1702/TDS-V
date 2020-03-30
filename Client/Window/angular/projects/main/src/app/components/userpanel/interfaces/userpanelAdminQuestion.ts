import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';

export interface UserpanelAdminQuestion {
    /** ID */
    [0]: number;
    /** Question */
    [1]: string;
    /** AnswerType */
    [2]: UserpanelAdminQuestionAnswerType;
}
