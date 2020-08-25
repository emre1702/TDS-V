import { inject, injectable } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import alt from "alt-client";
import PlayerDataKey from "../../datas/enums/data/player-data-key.enum";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";


declare module "alt-client" {
    interface Player {
        /**
         * Works only for the local player.
         * @param key PlayerDataKey or the key of it.
         * @returns Either the client set data, the stream synced data, the synced data or undefined.
         */
        getClientSyncedMeta(key: keyof typeof PlayerDataKey | PlayerDataKey): any;
    }

    interface Entity {
        getAnySyncedMeta(key: keyof typeof PlayerDataKey | PlayerDataKey): any;
    }
}

@injectable()
export default class DataSyncService {
    private myCustomData: { [key: number]: any } = {};

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService
    ) {
        alt.on("streamSyncedMetaChange", this.dataChanged.bind(this));
        alt.on("syncedMetaChange", this.dataChanged.bind(this));

        alt.onServer(ToClientEvent.SetPlayerData, this.dataChangedCustom.bind(this));

        alt.Player.prototype.getClientSyncedMeta = this.getPlayerData.bind(this);
        alt.Player.prototype.getAnySyncedMeta = this.getEntityData;
    }

    private getPlayerData(key: keyof typeof PlayerDataKey | PlayerDataKey): any {
        if (typeof key === "string") {
            key = PlayerDataKey[key];
        }
        if (key in this.myCustomData) {
            return this.myCustomData[key];
        }

        const keyString = PlayerDataKey[key];
        if (alt.Player.local.hasStreamSyncedMeta(keyString)) {
            return alt.Player.local.getStreamSyncedMeta(keyString);
        }
        if (alt.Player.local.hasSyncedMeta(keyString)) {
            return alt.Player.local.getSyncedMeta(keyString);
        }

        return undefined;
    }

    private getEntityData(key: keyof typeof PlayerDataKey | PlayerDataKey): any {
        if (typeof key === "string") {
            key = PlayerDataKey[key];
        }

        const entity = this as unknown as alt.Entity;
        const keyString = PlayerDataKey[key];
        
        if (entity.hasStreamSyncedMeta(keyString)) {
            return entity.getStreamSyncedMeta(keyString);
        }
        if (entity.hasSyncedMeta(keyString)) {
            return entity.getSyncedMeta(keyString);
        }

        return undefined;
    }

    private dataChanged(entity: alt.Entity, key: string, value: any, oldValue: any) {
        if (entity == alt.Player.local) {
            const playerDataKey = PlayerDataKey[key as keyof typeof PlayerDataKey];
            this.eventsService.onDataChanged.emit({ key: playerDataKey, value: value })
        }  
    }

    private dataChangedCustom(key: string, value: any) {
        const playerDataKey = PlayerDataKey[key as keyof typeof PlayerDataKey];
        this.myCustomData[playerDataKey] = value;
        this.eventsService.onDataChanged.emit({ key: playerDataKey, value: value })
    }
}
