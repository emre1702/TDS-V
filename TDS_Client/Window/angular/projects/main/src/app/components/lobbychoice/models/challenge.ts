import { ChallengeType } from '../enums/challenge-type.enum';
import { SafeHtml } from '@angular/platform-browser';

export class Challenge {

    /** Type */
    0: ChallengeType;

    /** Amount */
    1: number;

    /** CurrentAmount */
    2: number;

    /** Challenge info (not synced) */
    99: SafeHtml;
}
