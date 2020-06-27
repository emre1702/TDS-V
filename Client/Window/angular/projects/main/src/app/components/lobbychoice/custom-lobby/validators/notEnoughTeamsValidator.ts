import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { MapDataDto } from '../../../mapvoting/models/mapDataDto';
import { CustomLobbyTeamData } from '../../models/custom-lobby-team-data';
import { Constants } from 'projects/main/src/app/constants';

export function notEnoughTeamsValidator(selectedMapsGetter: () => MapDataDto[], teamsGetter: () => CustomLobbyTeamData[])
    : ValidatorFn {
    return (control: AbstractControl): ValidationErrors => {
        const teams = teamsGetter();    // teams with spectators
        const selectedMaps = selectedMapsGetter();

        for (const selectedMap of selectedMaps) {
            if (teams.length - 1 < Constants.MIN_TEAMS_PER_TYPE[selectedMap[2]]) {
                return { ["map"]: selectedMap };
            }
        }

        return null;
    };
}

