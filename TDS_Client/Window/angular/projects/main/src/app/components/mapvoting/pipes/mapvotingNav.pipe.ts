import { Pipe, PipeTransform } from "@angular/core";
import { MapDataDto } from '../models/mapDataDto';
import { SettingsService } from '../../../services/settings.service';
import { MapVotingService } from '../services/mapvoting.service';
import { MapType } from '../enums/maptype.enum';

@Pipe({name: 'mapVotingNav'})
export class MapVotingNavPipe implements PipeTransform {
    constructor(private settings: SettingsService, private mapVoting: MapVotingService) {}

    transform(map: MapDataDto[], showNav: string, filter: string) {
        if (!map)
          return map;

        if (filter)
          filter = filter.trim().toLowerCase();

        if (!showNav || showNav == "All")
            return map.filter(m => filter.length == 0 || m.Name.toLowerCase().indexOf(filter) >= 0);

        if (MapType[showNav] != undefined) {
            const type = MapType[showNav];
            return map.filter(m => m.Type == type
                && (filter.length == 0 || m.Name.toLowerCase().indexOf(filter) >= 0));
        }

        if (showNav == "Favourites") {
            return map.filter(m => this.settings.isInFavorites(m.Id)
                && (filter.length == 0 || m.Name.toLowerCase().indexOf(filter) >= 0));
        }

        if (showNav == "Voting") {
            return map.filter(m => this.mapVoting.mapsInVoting.some(mv => mv.Id == m.Id)
                && (filter.length == 0 || m.Name.toLowerCase().indexOf(filter) >= 0));
        }
    }
}
