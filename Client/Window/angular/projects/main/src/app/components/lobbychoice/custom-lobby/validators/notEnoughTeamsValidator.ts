import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { MapDataDto } from '../../../mapvoting/models/mapDataDto';
import { CustomLobbyTeamData } from '../../models/custom-lobby-team-data';
import { Constants } from 'projects/main/src/app/constants';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';

export function notEnoughTeamsValidator(selectedMapsGetter: () => MapDataDto[], teamsGetter: () => CustomLobbyTeamData[]): ValidatorFn {
    return (control: AbstractControl): ValidationErrors => {
        const teams = teamsGetter(); // teams with spectators
        let teamsLength = 0;
        if (teams) teamsLength = teams.length;
        // teams is null if the window was never opened
        else teamsLength = 3; // Default

        let selectedMaps = selectedMapsGetter();
        if (!selectedMaps) selectedMaps = [{ 0: -1, 2: MapType.Normal, 1: '', 3: { 7: '', 9: '' }, 4: '', 5: 0 }];

        for (const selectedMap of selectedMaps) {
            if (teamsLength - 1 < Constants.MIN_TEAMS_PER_TYPE[selectedMap[2]]) {
                return { ['notenoughteams']: selectedMap };
            }
        }

        return null;
    };
}
