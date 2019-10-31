import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { UserpanelAdminQuestionsGroup } from '../interfaces/userpanelAdminQuestionsGroup';
import { UserpanelAdminQuestionAnswerType } from '../enums/userpanel-admin-question-answer-type';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-userpanel-admin-questions',
    templateUrl: './userpanel-admin-questions.component.html',
    styleUrls: ['./userpanel-admin-questions.component.scss']
})
export class UserpanelAdminQuestionsComponent {

    @Input() disabled = false;
    @Input() questions: UserpanelAdminQuestionsGroup[];
    @Input() answers: { [index: number]: any };

    @Output() answersChange = new EventEmitter();

    answerType = UserpanelAdminQuestionAnswerType;

    constructor(private changeDetector: ChangeDetectorRef, public settings: SettingsService) { }

    save() {
        this.changeDetector.detectChanges();
    }

}
