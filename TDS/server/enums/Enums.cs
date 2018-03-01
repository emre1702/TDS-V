namespace TDS.server.enums {
    public enum LobbyStatus {
        NONE, MAPCHOOSE, COUNTDOWN, ROUND, ROUNDEND
    }

    public enum Language {
        ENGLISH, GERMAN
    }

    public enum MapType {
        NORMAL, BOMB
    }

    public enum RoundEndReason {
        DEATH, TIME, BOMB, COMMAND, NEWPLAYER
    }

    public enum PlayerSetting {
        LANGUAGE, HITSOUND
    }
}
