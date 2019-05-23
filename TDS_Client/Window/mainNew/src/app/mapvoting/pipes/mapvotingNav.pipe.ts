import { Pipe, PipeTransform } from "@angular/core";
import { MapDataDto } from '../models/mapDataDto';
import { SettingsService } from '../../services/settings.service';
import { MapVotingService } from '../services/mapvoting.service';
import { MapType } from '../enums/maptype.enum';

@Pipe({name: 'mapVotingNav'})
export class MapVotingNavPipe implements PipeTransform {
    constructor(private settings: SettingsService, private mapVoting: MapVotingService) {}

    transform(map: MapDataDto[], showNav: string) {
        if (!map || !showNav || showNav == "All")
            return map;

        if (MapType[showNav] != undefined) {
            const type = MapType[showNav];
            console.log(type);
            return map.filter(m => m.Type == type);
        }

        if (showNav == "Favourites") {
            return map.filter(m => this.settings.FavoriteMapIDs.has(m.Id));
        }

        if (showNav == "Voting") {
            return map.filter(m => this.mapVoting.mapsInVoting.some(mv => mv.Id == m.Id));
        }
    }
}
