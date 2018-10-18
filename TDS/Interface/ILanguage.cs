using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Interface
{
    interface ILanguage
    {
        string ACCOUNT_DOESNT_EXIST { get; }
        string ACTIVATED_HITSOUND { get; }
        string ADMINLVL_NOT_HIGH_ENOUGH { get; }
        string ANSWERED_REPORT_INFO { get; }

        string BOMB_PLANTED { get; }

        string COMMITED_SUICIDE { get; }
        string CONNECTING { get; }
        string CREATED_REPORT_INFO { get; }

        string DEACTIVATED_HITSOUND { get; }
        string DEATH_KILLED_INFO { get; }
        string DEATH_DIED_INFO { get; }
        string[] DEFUSE_INFO { get; }

        string GANG_DOESNT_EXIST_ANYMORE { get; }
        string GANG_REMOVED { get; }
        string GOT_ASSIST { get; }
        string GOT_LAST_HITTED_KILL { get; }
        string GOT_OFFLINE_MESSAGE_WITH_NEW { get; }
        string GOT_OFFLINE_MESSAGE_WITHOUT_NEW { get; }
        string GOT_REPORT_ANSWER_INFO { get; }

        string GANG_INVITATION_WAS_REMOVED { get; }
        
        string KICK_INFO { get; }
        string KICK_YOU_INFO { get; }
        string KICK_LOBBY_INFO { get; }
        string KICK_LOBBY_YOU_INFO { get; }
        string KILLING_SPREE_HEALTHARMOR { get; }

        string MAP_WON_VOTING { get; }

        string NOT_ALLOWED { get; }
        string NOT_MORE_MAPS_FOR_VOTING_ALLOWED { get; }
        string NOT_POSSIBLE_IN_THIS_LOBBY { get; }

        string ORDER_ATTACK { get; }
        string ORDER_GO_TO_BOMB { get; }
        string ORDER_SPREAD_OUT { get; }
        string ORDER_STAY_BACK { get; }

        string PERMABAN_INFO { get; }
        string PERMABAN_YOU_INFO { get; }
        string PERMABAN_LOBBY_INFO { get; }
        string PERMAMUTE_INFO { get; }
        string PLANT_INFO { get; }
        string PLAYER_ALREADY_MUTED { get; }
        string PLAYER_DOESNT_EXIST { get; }
        string PLAYER_NOT_MUTED { get; }

        string REASON_MISSING { get; }
        string ROUND_END_BOMB_DEFUSED_INFO { get; }
        string ROUND_END_BOMB_EXPLODED_INFO { get; }
        string ROUND_END_COMMAND_INFO { get; }
        string ROUND_END_DEATH_INFO { get; }
        string ROUND_END_DEATH_ALL_INFO { get; }
        string ROUND_END_NEW_PLAYER_INFO { get; }
        string ROUND_END_TIME_INFO { get; }
        string ROUND_MISSION_BOMB_BAD { get; }
        string ROUND_MISSION_BOMB_GOOD { get; }
        string ROUND_MISSION_BOMG_SPECTATOR { get; }
        string ROUND_MISSION_NORMAL { get; }
        string ROUND_REWARD_INFO { get; }

        string STILL_BANED_IN_LOBBY { get; }
        string STILL_MUTED { get; }
        string STILL_PERMABANED_IN_LOBBY { get; }
        string STILL_PERMAMUTED { get; }

        string TARGET_NOT_IN_SAME_LOBBY { get; }
        string TARGET_NOT_LOGGED_IN { get; }
        string TIMEBAN_INFO { get; }
        string TIMEBAN_YOU_INFO { get; }
        string TIMEBAN_LOBBY_INFO { get; }
        string TIMEMUTE_INFO { get; }
        string TOO_LONG_OUTSIDE_MAP { get; }

        string UNBAN_INFO { get; }
        string UNBAN_LOBBY_INFO { get; }
        string UNBAN_YOU_LOBBY_INFO { get; }
        string UNMUTE_INFO { get; }

        string[] WELCOME_MESSAGE { get; }

        string WRONG_PASSWORD { get; }
        
        
        
    }
}
