class DIIdentifier {
    static BindsService = Symbol.for("BindsService");
    static BrowsersService = Symbol.for("BrowsersService");
    static CharCreatorService = Symbol.for("CharCreatorService");
    static ChatService = Symbol.for("ChatService");
    static DiscordService = Symbol.for("DiscordService");
    static DxService = Symbol.for("DxService");
    static EventsService = Symbol.for("EventsService");
    static MapCreatorObject = Symbol.for("MapCreatorObject");
    static MapCreatorObjectsService = Symbol.for("MapCreatorObjectsService");
    static RemoteEventsSender = Symbol.for("RemoteEventsSender");
    static SettingsService = Symbol.for("SettingsService");
    static Start = Symbol.for("Start");

    static Factory_MapCreatorObject = Symbol.for("Factory<MapCreatorObject>");
}

export default DIIdentifier;
