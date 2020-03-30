import { ChallengeFrequency } from '../enums/challenge-frequency.enum';
import { Challenge } from './challenge';

export class ChallengeGroup {
    /** Frequency */
    0: ChallengeFrequency;

    /** Challenges */
    1: Challenge[];
}
