import { Pipe, PipeTransform } from "@angular/core";
import { MapDataDto } from '../models/mapDataDto';
import { SettingsService } from '../../../services/settings.service';
import { MapVotingService } from '../services/mapvoting.service';
import { MapType } from '../../../enums/maptype.enum';

@Pipe({name: 'mapVotingNav'})
export class MapVotingNavPipe implements PipeTransform {
    constructor(private settings: SettingsService, private mapVoting: MapVotingService) {}

    transform(map: MapDataDto[], showNav: string, filter: string) {
        if (!map)
          return map;

        if (filter)
          filter = filter.trim().toLowerCase();

        if (!showNav || showNav == "All")
            return map.filter(m => filter.length == 0 || m[1].toLowerCase().indexOf(filter) >= 0);

        if (MapType[showNav] != undefined) {
            const type = MapType[showNav];
            return map.filter(m => m[2] == type
                && (filter.length == 0 || m[1].toLowerCase().indexOf(filter) >= 0));
        }

        if (showNav == "Favourites") {
            return map.filter(m => this.settings.isInFavorites(m[0])
                && (filter.length == 0 || m[1].toLowerCase().indexOf(filter) >= 0));
        }

        if (showNav == "Voting") {
            return map.filter(m => this.mapVoting.mapsInVoting.some(mv => mv[0] == m[0])
                && (filter.length == 0 || m[1].toLowerCase().indexOf(filter) >= 0));
        }
    }
}
