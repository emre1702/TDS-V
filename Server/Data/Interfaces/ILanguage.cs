namespace TDS_Server.Data.Interfaces
{
    public interface ILanguage
    {
        string A_MAP_WAS_ALREADY_BOUGHT { get; }
        string ACCOUNT_DOESNT_EXIST { get; }
        string ADDED_THE_GANG_HOUSE_SUCCESSFULLY { get; }
        string ALREADY_IN_PRIVATE_CHAT_WITH { get; }

        string BALANCE_TEAM_INFO { get; }
        string BOMB_PLANT_INFO { get; }
        string BOMB_PLANTED { get; }

        string CHAR_IN_NAME_IS_NOT_ALLOWED { get; }
        string COMMAND_DOESNT_EXIST { get; }
        string COMMAND_TOO_LESS_ARGUMENTS { get; }
        string COMMAND_TOO_MANY_ARGUMENTS { get; }
        string COMMAND_USED_WRONG { get; }
        string COMMITED_SUICIDE { get; }
        string CONNECTING { get; }
        string CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR { get; }
        string CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR { get; }
        string CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR { get; }

        string DEATH_DIED_INFO { get; }
        string DEATH_KILLED_INFO { get; }
        string[] DEFUSE_INFO { get; }
        string DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO { get; }
        string DISCORD_IDENTITY_SAVE_FAILED { get; }
        string DISCORD_IDENTITY_SAVED_SUCCESSFULLY { get; }

        string ERROR_INFO { get; }

        string FRIEND_JOINED_LOBBY_INFO { get; }
        string FRIEND_LEFT_LOBBY_INFO { get; }
        string FRIEND_LOGGEDIN_INFO { get; }
        string FRIEND_LOGGEDOFF_INFO { get; }
        string GANG_DOESNT_EXIST_ANYMORE { get; }
        string GANG_LEVEL_MAX_ALLOWED { get; }
        string GANG_INVITATION_INFO { get; }
        string GANG_INVITATION_WAS_REMOVED { get; }
        string GANG_REMOVED { get; }
        string GANGACTION_CANT_JOIN_AGAIN { get; }
        string GANGWAR_ATTACK_INVITATION { get; }
        string GANGWAR_ATTACK_PREPARATION_INVITATION { get; }
        string GANGWAR_ATTACKER_PREPARATION_INFO { get; }
        string GANGWAR_ATTACKER_STARTED_INFO { get; }
        string GANGWAR_DEFEND_INVITATION { get; }
        string GANGWAR_OWNER_PREPARATION_INFO { get; }
        string GANGWAR_OWNER_STARTED_INFO { get; }
        string GANGWAR_PREPARATION_INFO { get; }
        string GANGWAR_STARTED_INFO { get; }
        string TEAM_ALREADY_FULL_INFO { get; }
        string GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO { get; }
        string GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO { get; }
        string GIVE_MONEY_NEED_FEE { get; }
        string GIVE_MONEY_TOO_LESS { get; }
        string GOT_ASSIST { get; }
        string GOT_LAST_HITTED_KILL { get; }
        string GOT_LOBBY_BAN { get; }
        string GOT_UNREAD_OFFLINE_MESSAGES { get; }

        string HITSOUND_ACTIVATED { get; }
        string HITSOUND_DEACTIVATED { get; }

        string INVITATION_MAPCREATELOBBY { get; }
        string INVITATION_WAS_WITHDRAWN_OR_REMOVED { get; }

        string JOINED_LOBBY_MESSAGE { get; }

        string KICK_INFO { get; }
        string KICK_LOBBY_INFO { get; }
        string KICK_YOU_INFO { get; }
        string KILLING_SPREE_HEALTHARMOR { get; }

        string LOBBY_DOESNT_EXIST { get; }
        string LOBBY_ERROR_REMOVE { get; }

        string MAP_BUY_INFO { get; }
        string MAP_WON_VOTING { get; }
        string MUTE_EXPIRED { get; }
        string MUTETIME_INVALID { get; }

        string NEW_OFFLINE_MESSAGE { get; }
        string NOT_ALLOWED { get; }
        string NOT_ENOUGH_MONEY { get; }
        string NOT_IN_PRIVATE_CHAT { get; }
        string NOT_MORE_MAPS_FOR_VOTING_ALLOWED { get; }
        string NOT_POSSIBLE_IN_THIS_LOBBY { get; }

        string ONLY_ALLOWED_IN_GANG_LOBBY { get; }
        string ORDER_ATTACK { get; }
        string ORDER_GO_TO_BOMB { get; }
        string ORDER_SPREAD_OUT { get; }
        string ORDER_STAY_BACK { get; }

        string PERMABAN_INFO { get; }
        string PERMABAN_LOBBY_INFO { get; }
        string PERMABAN_LOBBY_YOU_INFO { get; }
        string PERMABAN_YOU_INFO { get; }
        string PERMAMUTE_INFO { get; }
        string PERMAVOICEMUTE_INFO { get; }
        string PLAYER_ACCEPTED_YOUR_INVITATION { get; }
        string PLAYER_ALREADY_MUTED { get; }
        string PLAYER_ALREADY_ON_THIS_PAGE { get; }
        string PLAYER_DOESNT_EXIST { get; }

        //string PLAYER_GOT_KILLINGSPREE_NOTIFICATION { get; }
        string PLAYER_ISNT_BANED { get; }

        string PLAYER_JOINED_YOUR_GANG { get; }
        string PLAYER_LOGGED_IN { get; }
        string PLAYER_LOGGED_OUT { get; }
        string PLAYER_NOT_IN_YOUR_GANG { get; }
        string PLAYER_NOT_MUTED { get; }
        string PLAYER_REGISTERED { get; }
        string PLAYER_REJECTED_YOUR_INVITATION { get; }
        string PLAYER_WITH_NAME_ALREADY_EXISTS { get; }
        string PLAYER_WON_INFO { get; }
        string PRIVATE_CHAT_CLOSED_PARTNER { get; }
        string PRIVATE_CHAT_CLOSED_YOU { get; }
        string PRIVATE_CHAT_DISCONNECTED { get; }
        string PRIVATE_CHAT_OPENED_WITH { get; }
        string PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER { get; }
        string PRIVATE_CHAT_REQUEST_CLOSED_YOU { get; }
        string PRIVATE_CHAT_REQUEST_RECEIVED_FROM { get; }
        string PRIVATE_CHAT_REQUEST_SENT_TO { get; }
        string PRIVATE_MESSAGE_SENT { get; }

        string REASON_MISSING { get; }
        string REPORT_ANSWERED_INFO { get; }
        string REPORT_CREATED_INFO { get; }
        string REPORT_GOT_ANSWER_INFO { get; }
        string RESOURCE_RESTART_INFO_MINUTES { get; }
        string RESOURCE_RESTART_INFO_SECONDS { get; }
        string ROUND_END_BOMB_DEFUSED_INFO { get; }
        string ROUND_END_BOMB_EXPLODED_INFO { get; }
        string ROUND_END_COMMAND_INFO { get; }
        string ROUND_END_DEATH_ALL_INFO { get; }
        string ROUND_END_DEATH_INFO { get; }
        string ROUND_END_NEW_PLAYER_INFO { get; }
        string ROUND_END_TARGET_EMPTY_INFO { get; }
        string ROUND_END_TIME_INFO { get; }
        string ROUND_END_TIME_TIE_INFO { get; }
        string ROUND_MISSION_BOMB_BAD { get; }
        string ROUND_MISSION_BOMB_GOOD { get; }
        string ROUND_MISSION_BOMG_SPECTATOR { get; }
        string ROUND_MISSION_NORMAL { get; }
        string ROUND_REWARD_INFO { get; }

        string SENT_APPLICATION { get; }
        string SENT_APPLICATION_TO { get; }
        string STILL_BANED_IN_LOBBY { get; }
        string STILL_MUTED { get; }
        string STILL_PERMABANED_IN_LOBBY { get; }
        string STILL_PERMAMUTED { get; }
        string SUPPORT_REQUEST_CREATED { get; }

        string TARGET_ACCEPTED_INVITATION { get; }
        string TARGET_ADDED_BLOCK { get; }
        string TARGET_ALREADY_BLOCKED { get; }
        string TARGET_ALREADY_IN_A_GANG { get; }
        string TARGET_ALREADY_IN_PRIVATE_CHAT { get; }
        string TARGET_NOT_BLOCKED { get; }
        string TARGET_NOT_IN_SAME_LOBBY { get; }
        string TARGET_NOT_LOGGED_IN { get; }
        string TARGET_PLAYER_DEFEND_INFO { get; }
        string TARGET_RANK_IS_HIGHER_OR_EQUAL { get; }
        string TARGET_REJECTED_INVITATION { get; }
        string TARGET_REMOVED_FRIEND_ADDED_BLOCK { get; }
        string TESTING_MAP_NOTIFICATION { get; }
        string TEXT_TOO_LONG { get; }
        string TEXT_TOO_SHORT { get; }
        string THE_RANK_IS_INVALID_REFRESH_WINDOW { get; }
        string TIMEBAN_INFO { get; }
        string TIMEBAN_LOBBY_INFO { get; }
        string TIMEBAN_LOBBY_YOU_INFO { get; }
        string TIMEBAN_YOU_INFO { get; }
        string TIMEMUTE_INFO { get; }
        string TIMEVOICEMUTE_INFO { get; }
        string TOO_LONG_OUTSIDE_MAP { get; }
        string TRY_AGAIN_LATER { get; }

        string UNBAN_INFO { get; }
        string UNBAN_LOBBY_INFO { get; }
        string UNBAN_YOU_LOBBY_INFO { get; }
        string UNMUTE_INFO { get; }
        string UNVOICEMUTE_INFO { get; }

        string VOICE_MUTE_EXPIRED { get; }

        string WRONG_PASSWORD { get; }

        string YOU_ACCEPTED_INVITATION { get; }
        string YOU_ACCEPTED_TEAM_INVITATION { get; }
        string YOU_ALREADY_INVITED_TARGET { get; }
        string YOU_ARE_ALREADY_IN_A_GANG { get; }
        string YOU_ARE_NOT_IN_A_GANG { get; }
        string YOU_CANT_BUY_A_MAP_IN_CUSTOM_LOBBY { get; }
        string YOU_GAVE_MONEY_TO_WITH_FEE { get; }
        string YOU_GOT_BLOCKED_BY { get; }
        string YOU_GOT_INVITATION_BY { get; }
        string YOU_GOT_KICKED_OUT_OF_THE_GANG_BY { get; }
        string YOU_GOT_MONEY_BY_WITH_FEE { get; }
        string YOU_GOT_RANK_DOWN_BY { get; }
        string YOU_GOT_RANK_UP { get; }
        string YOU_GOT_UNBLOCKED_BY { get; }
        string YOU_JOINED_THE_GANG { get; }
        string YOU_KICKED_OUT_OF_GANG { get; }
        string YOU_REJECTED_GANG_INVITATION { get; }
        string YOU_REJECTED_INVITATION { get; }
        string YOU_REJECTED_TEAM_INVITATION { get; }
        string YOU_UNBLOCKED { get; }
        string YOUVE_BECOME_GANG_LEADER { get; }
        string YOUVE_LEFT_THE_GANG { get; }
        string NO_EMAIL_ADDRESS_HAS_BEEN_SET { get; }
        string EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID { get; }
        string PASSWORD_HAS_BEEN_RESET_EMAIL_SENT { get; }
    }
}
