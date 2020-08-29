import { Container, interfaces } from "inversify";
import { log } from "alt-client";

import Start from "./start"
import CharCreatorService from "./appearance/char-creator.service";
import DxService from "./draw/dx.service";
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
import CursorService from "./input/cursor.service";
import FreeroamService from "./lobbies/freeroam.service";
import DataSyncService from "./datas/data-sync.service";
import SpectateService from "./cameras/spectate.service";
import CamerasService from "./cameras/cameras.service";
import DeathService from "./deathmatch/death.service";
import ScaleformMessagesService from "./draw/scaleform-messages.service";
import Camera from "../entities/cameras/camera.entity";
import GangHouseService from "./lobbies/gang/gang-house.service";
import GangVehiclesService from "./lobbies/gang/gang-vehicles.service";
import { AFKCheckService } from "./others/afk-check.service";
import { FightService } from "./deathmatch/fight.service";
import { HudService } from "./draw/hud.service";
import { FloatingDamageInfoService } from "./draw/floating-damage-info.service";

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
container.bind(DIIdentifier.CursorService).to(CursorService);
container.bind(DIIdentifier.FreeroamService).to(FreeroamService);
container.bind(DIIdentifier.DataSyncService).to(DataSyncService);
container.bind(DIIdentifier.CamerasService).to(CamerasService);
container.bind(DIIdentifier.SpectateService).to(SpectateService);
container.bind(DIIdentifier.DeathService).to(DeathService);
container.bind(DIIdentifier.ScaleformMessagesService).to(ScaleformMessagesService);
container.bind(DIIdentifier.GangHouseService).to(GangHouseService);
container.bind(DIIdentifier.GangVehiclesService).to(GangVehiclesService);
container.bind(DIIdentifier.AFKCheckService).to(AFKCheckService);
container.bind(DIIdentifier.FightService).to(FightService);
container.bind(DIIdentifier.HudService).to(HudService);
container.bind(DIIdentifier.FloatingDamageInfoService).to(FloatingDamageInfoService);
///////////////

// Factory //
container.bind<interfaces.Factory<MapCreatorObject>>(DIIdentifier.Factory_MapCreatorObject).toAutoFactory<MapCreatorObject>(DIIdentifier.MapCreatorObject);
container.bind<interfaces.Factory<Camera>>(DIIdentifier.Factory_Camera).toAutoFactory<Camera>(DIIdentifier.Factory_Camera);
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
    container.get(DIIdentifier.CursorService);
    container.get(DIIdentifier.FreeroamService);
    container.get(DIIdentifier.DataSyncService);
    container.get(DIIdentifier.CamerasService);
    container.get(DIIdentifier.SpectateService);
    container.get(DIIdentifier.DeathService);
    container.get(DIIdentifier.ScaleformMessagesService);
    container.get(DIIdentifier.GangHouseService);
    container.get(DIIdentifier.GangVehiclesService);
    container.get(DIIdentifier.AFKCheckService);
    container.get(DIIdentifier.FightService)
    container.get(DIIdentifier.HudService);
    container.get(DIIdentifier.FloatingDamageInfoService);
    ////////////////////////////
} catch (ex) {
    logError(ex, "Services initializing failed");
    logErrorToServer(ex, "Services initializing failed");
}

log("Services successfully initialized");

export default container;
