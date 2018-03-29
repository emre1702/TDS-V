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

    public enum GangActivity {
        INVITE, UNINVITE, RANKUP, RANKDOWN, CHANGE_RANKNAMES, CHANGE_RANKCOLORS  
    }

	public enum LogType {
		LOGIN, REGISTER, CHAT, ERROR, LOBBYOWNER, LOBBYJOIN, VIP
	}

	public enum AdminLogType {
		PERMABAN, TIMEBAN, UNBAN, PERMAMUTE, TIMEMUTE, UNMUTE, NEXT, KICK, LOBBYKICK 
	}
}
