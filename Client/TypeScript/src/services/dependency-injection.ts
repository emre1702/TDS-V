import { Container } from "inversify";
import { log } from "alt-client";

import Start from "./start"
import CharCreatorService from "./appearance/char-creator.service";
import LoggingService from "./output/logging.service";
import DxService from "./draw/dx/dx.service";
import EventsService from "./events/events.service";
import RemoteEventsSender from "./events/remote-events-sender.service";
import BindsService from "./input/binds.service";
import DiscordHandler from "./output/discord.service";
import BrowsersService from "./browsers/browsers.service";



log("Initializing services ...");

var container = new Container({ defaultScope: "Singleton" });

// Singleton //
container.bind(Start).toSelf();

container.bind(CharCreatorService).toSelf();
container.bind(LoggingService).toSelf();
container.bind(DxService).toSelf();
container.bind(EventsService).toSelf();
container.bind(RemoteEventsSender).toSelf();
container.bind(BindsService).toSelf();
container.bind(DiscordHandler).toSelf();
container.bind(BrowsersService).toSelf();
///////////////

const logger = container.get(LoggingService);
try {
    // Resolve all singletons //
    container.get(Start);
    container.get(CharCreatorService);
    container.get(DxService);
    container.get(EventsService);
    container.get(RemoteEventsSender);
    container.get(BindsService);
    container.get(DiscordHandler);
    container.get(BrowsersService);
    ////////////////////////////
} catch (ex) {
    logger.logError(ex, "Services initializing failed");
    logger.logErrorToServer(ex, "Services initializing failed");
}

log("Services successfully initialized");

export default container;
