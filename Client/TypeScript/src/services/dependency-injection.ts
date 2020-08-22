import { Container, interfaces } from "inversify";
import { log } from "alt-client";

import Start from "./start"
import CharCreatorService from "./appearance/char-creator.service";
import DxService from "./draw/dx/dx.service";
import EventsService from "./events/events.service";
import RemoteEventsSender from "./events/remote-events-sender.service";
import BindsService from "./input/binds.service";
import BrowsersService from "./browsers/browsers.service";
import { logError, logErrorToServer } from "../datas/helper/logging.helper";
import MapCreatorObject from "../entities/lobbies/map-creator/map-creator-object.entity";
import DIIdentifier from "../datas/enums/dependency-injection/di-identifier.enum";
import DiscordService from "./output/discord.service";
import ChatService from "./output/chat.service";
import MapCreatorObjectsService from "./lobbies/map-creator/map-creator-objects.service";
import SettingsService from "./settings/settings.service";

log("Initializing services ...");

var container = new Container({ defaultScope: "Singleton" });

// Singleton //
container.bind(DIIdentifier.Start).to(Start);
container.bind(DIIdentifier.CharCreatorService).to(CharCreatorService);
container.bind(DIIdentifier.DxService).to(DxService);
container.bind(DIIdentifier.EventsService).to(EventsService);
container.bind(DIIdentifier.RemoteEventsSender).to(RemoteEventsSender);
container.bind(DIIdentifier.BindsService).to(BindsService);
container.bind(DIIdentifier.DiscordService).to(DiscordService);
container.bind(DIIdentifier.BrowsersService).to(BrowsersService);
container.bind(DIIdentifier.ChatService).to(ChatService);
container.bind(DIIdentifier.MapCreatorObjectsService).to(MapCreatorObjectsService);
container.bind(DIIdentifier.SettingsService).to(SettingsService);
///////////////

// Factory //
container.bind<interfaces.Factory<MapCreatorObject>>(DIIdentifier.Factory_MapCreatorObject).toAutoFactory<MapCreatorObject>(DIIdentifier.MapCreatorObject);
/////////////

try {
    // Resolve all singletons //
    container.get(DIIdentifier.Start);
    container.get(DIIdentifier.CharCreatorService);
    container.get(DIIdentifier.DxService);
    container.get(DIIdentifier.EventsService);
    container.get(DIIdentifier.RemoteEventsSender);
    container.get(DIIdentifier.BindsService);
    container.get(DIIdentifier.DiscordService);
    container.get(DIIdentifier.BrowsersService);
    container.get(DIIdentifier.ChatService);
    container.get(DIIdentifier.MapCreatorObjectsService);
    container.get(DIIdentifier.SettingsService);
    ////////////////////////////
} catch (ex) {
    logError(ex, "Services initializing failed");
    logErrorToServer(ex, "Services initializing failed");
}

log("Services successfully initialized");

export default container;
