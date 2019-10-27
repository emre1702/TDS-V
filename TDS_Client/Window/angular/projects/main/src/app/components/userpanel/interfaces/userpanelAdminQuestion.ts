import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';

export interface UserpanelAdminQuestion {
    ID: number;
    Question: string;
    AnswerType: UserpanelAdminQuestionAnswerType;
}
