import { Player } from "alt-client"
import PlayerDataKey from "../../datas/enums/data/player-data-key.enum";
import { serverTeamSuffixMinAdminLevel, serverTeamSuffix } from "../../datas/constants";

declare module "alt-client" {
    interface Player {
        getName(): string;
        getTDSId(): number;
        displayName(): string;
    }
}

Player.prototype.getName = function () {
    return (this as Player).getStreamSyncedMeta(PlayerDataKey[PlayerDataKey.Name]);
}

Player.prototype.getTDSId = function () {
    return (this as Player).getSyncedMeta(PlayerDataKey[PlayerDataKey.Id]);
}

Player.prototype.displayName = function () {
    const me = this as Player;
    let name = me.getName();
    const adminLevel = me.getStreamSyncedMeta(PlayerDataKey[PlayerDataKey.AdminLevel]);
    if (adminLevel >= serverTeamSuffixMinAdminLevel) {
        name = serverTeamSuffix;
    }
    return name;
}
