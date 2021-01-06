import { FormControl, FormGroup } from '@angular/forms';
import { Constants } from 'projects/main/src/app/constants';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';

const MIN_TEAM_SPAWNS = 3;

export function teamSpawnsValidator(typeGetter: () => MapType) {
    return (c: FormControl) => {
        const minTeamAmount = Constants.MIN_TEAMS_PER_TYPE[typeGetter()];
        const data = c.value;
        if (data.length < minTeamAmount) {
            return { notEnoughTeams: { expected: minTeamAmount, actual: data.length } };
        }

        // Check enough spawns per team
        for (let i = 0; i < data.length; ++i) {
            const spawnArr = data[i];
            if (spawnArr.length < MIN_TEAM_SPAWNS) {
                return { notEnoughSpawns: { team: i + 1, minAmount: MIN_TEAM_SPAWNS, currentAmount: spawnArr.length } };
            }
        }
        return null;
    };
}
