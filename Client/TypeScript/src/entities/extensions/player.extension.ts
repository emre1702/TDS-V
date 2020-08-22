import { Player } from "alt-client"
import PlayerDataKey from "../../datas/enums/data/player-data-key.enum";

declare module "alt-client" {
    interface Player {
        getName(): string;
        getTDSId(): number;
    }
}

Player.prototype.getName = function () {
    return (this as Player).getStreamSyncedMeta(PlayerDataKey[PlayerDataKey.Name]);
}

Player.prototype.getTDSId = function () {
    return (this as Player).getSyncedMeta(PlayerDataKey[PlayerDataKey.Id]);
}
