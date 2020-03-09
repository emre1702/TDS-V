﻿using System;
using TDS_Server.Interfaces;

namespace TDS_Server.Data.Language
{
    internal class English : ILanguage
    {
        public virtual string ACCOUNT_DOESNT_EXIST => "Account doesn't exist.";
        public virtual string ALREADY_IN_PRIVATE_CHAT_WITH => "You are already in a private chat with {0}.";

        public virtual string BALANCE_TEAM_INFO => "{0} was put into another team to balance the teams.";
        public virtual string BOMB_PLANT_INFO => "To plant the bomb you have to take your fists and hold the left mouse button on one of the bomb-spots.";
        public virtual string BOMB_PLANTED => "Bomb was planted!";

        public virtual string CHAR_IN_NAME_IS_NOT_ALLOWED => "The character '{0}' in your name is not allowed.";
        public virtual string COMMAND_DOESNT_EXIST => "The command doesn't exist.";
        public virtual string COMMAND_TOO_LESS_ARGUMENTS => "You used too less arguments for this command.";
        public virtual string COMMAND_USED_WRONG => "The command usage was wrong.";
        public virtual string COMMITED_SUICIDE => "You commited suicide.";
        public virtual string CONNECTING => "connecting ...";
        public virtual string CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR => "This name is already taken.";
        public virtual string CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR => "This name is not allowed for lobbies.";
        public virtual string CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR => "Unknown error when creating the lobby.";

        public virtual string DEATH_KILLED_INFO => "{0} killed {1} with {2}";
        public virtual string DEATH_DIED_INFO => "{0} died";

        public virtual string[] DEFUSE_INFO => new string[] {
            "Round-time got changed. Now you have to kill all opponents or defuse the bomb.",
            "To defuse the bomb go to the red blip on your minimap (bomb), take your fists and hold the left mouse button."
        };
        public virtual string DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO => "The player '{0}' has set your discord user id in TDS-V." 
            + Environment.NewLine + "Were you that player?"
            + Environment.NewLine + "Please confirm the setting with the command '!confirmtds'.";
        public virtual string DISCORD_IDENTITY_SAVE_FAILED => "Saving the Discord user id failed: {0}";
        public virtual string DISCORD_IDENTITY_SAVED_SUCCESSFULLY => "The discord user id has been successfully saved.";

        public virtual string ERROR_INFO => "An error occured. The Developer(s) have been notified.";

        public virtual string GANG_DOESNT_EXIST_ANYMORE => "The gang doesn't exist anymore!";
        public virtual string GANG_REMOVED => "Your gang got disbanded.";
        public virtual string GANGWAR_ATTACKER_PREPARATION_INFO => "You are preparing an attack against '{1}' in the area '{0}'.";
        public virtual string GANGWAR_ATTACKER_STARTED_INFO => "You attack the area '{0}' of '{1}'.";
        public virtual string GANGWAR_ATTACK_PREPARATION_INVITATION => "Your gang is preparing a gangwar!\n(Accept or use /attack)";
        public virtual string GANGWAR_ATTACK_INVITATION => "Your gang is attacking an area!\n(Accept or use /attack)";
        public virtual string GANGWAR_DEFEND_INVITATION => "Defend your gang area!\n(Accept or use /defend)";
        public virtual string GANGWAR_OWNER_PREPARATION_INFO => "A gangwar is being prepared against one of your areas.";
        public virtual string GANGWAR_OWNER_STARTED_INFO => "Your area '{0}' is being attacked by the gang '{1}'.";
        public virtual string GANGWAR_PREPARATION_INFO => "Die Gang '{0}' bereitet einen Gangwar vor.";
        public virtual string GANGWAR_STARTED_INFO => "The gang '{0}' is preparing a gangwar.";
        public virtual string GANGWAR_TEAM_ALREADY_FULL_INFO => "Your team is already full. Wait for a opponent to join and try again.";
        public virtual string GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO => "{0} ist bei den Gegnern in den Gangwar beigetreten.";
        public virtual string GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO => "{0} ist bei euch in den Gangwar beigetreten.";
        public virtual string GIVE_MONEY_NEED_FEE => "You need ${0} with ${1} fee included.";
        public virtual string GIVE_MONEY_TOO_LESS => "The amount you tried to give is too less.";
        public virtual string GOT_ASSIST => "You got the assist of {0}.";
        public virtual string GOT_LAST_HITTED_KILL => "You hitted {0} last and got the kill.";
        public virtual string GOT_LOBBY_BAN => "You got a ban from the lobby-owner here. Duration: {0} | Reason: {1}";
        public virtual string GOT_UNREAD_OFFLINE_MESSAGES => "You got {0} unread offline message(s) in userpanel";
        public virtual string GANG_INVITATION_WAS_REMOVED => "The invitation already got removed!";

        public virtual string HITSOUND_ACTIVATED => "Hitsound activated!";
        public virtual string HITSOUND_DEACTIVATED => "Hitsound deactivated!";

        public virtual string INVITATION_MAPCREATELOBBY => "You've been invited to the MapCreator lobby by {0}.";
        public virtual string INVITATION_WAS_WITHDRAWN_OR_REMOVED => "The invitation has already been withdrawn or removed.";

        public virtual string JOINED_LOBBY_MESSAGE => "You joined lobby \"{0}\".\nUse '/{1}' to leave.";

        public virtual string KICK_INFO => "{0} was kicked by {1}. Reason: {2}";
        public virtual string KICK_YOU_INFO => "You were kicked by {0}. Reason: {1}";
        public virtual string KICK_LOBBY_INFO => "{0} was kicked by {1} out of the lobby. Reason: {2}";
        public virtual string KILLING_SPREE_HEALTHARMOR => "{0} has got a {1}-killingspree and gets a {2} life/armor bonus.";

        public virtual string LOBBY_DOESNT_EXIST => "This lobby does not exist.";
        public virtual string LOBBY_ERROR_REMOVE => "The lobby has got an error and got removed. The developer are informed.";

        public virtual string MAP_BUY_INFO => "{0} has bought the map {1}.";
        public virtual string MAP_WON_VOTING => "The map {0} won the voting!";
        public virtual string MUTE_EXPIRED => "The chat mute has expired. You can write again.";
        public virtual string MUTETIME_INVALID => "The mute time is invalid. Allowed are -1, 0 and higher.";

        public virtual string NOT_ALLOWED => "You are not allowed to do that.";
        public virtual string NOT_ENOUGH_MONEY => "You don't have enough money.";
        public virtual string NOT_IN_PRIVATE_CHAT => "You are not in a private chat.";
        public virtual string NOT_MORE_MAPS_FOR_VOTING_ALLOWED => "There can be only 6 maps in voting!";
        public virtual string NOT_POSSIBLE_IN_THIS_LOBBY => "Not possible in this lobby!";

        public virtual string ORDER_ATTACK => "Attack! Go go go!";
        public virtual string ORDER_GO_TO_BOMB => "Go to the bomb!";
        public virtual string ORDER_SPREAD_OUT => "Spread out!";
        public virtual string ORDER_STAY_BACK => "Stay back!";

        public virtual string PERMABAN_INFO => "{0} was baned permanently by {1}. Reason: {2}";
        public virtual string PERMABAN_YOU_INFO => "You were baned permanently by {0}. Reason: {1}";
        public virtual string PERMABAN_LOBBY_INFO => "{0} was baned permanently in lobby '{1}' by {2}. Reason: {3}";
        public virtual string PERMABAN_LOBBY_YOU_INFO => "You was baned for {0} hours from lobby '{1}' by {2}. Reason {3}";
        public virtual string PERMAMUTE_INFO => "{0} was muted permanently by {1}. Reason: {2}";
        public virtual string PERMAVOICEMUTE_INFO => "{0} was muted permanently in voice-chat by {1}. Reason: {2}";
        public virtual string PLAYER_ACCEPTED_YOUR_INVITATION => "{0} accepted your team-invitation. He is your member now.";
        public virtual string PLAYER_ALREADY_MUTED => "The player is already muted!";
        public virtual string PLAYER_DOESNT_EXIST => "The player doesn't exist!";
        public virtual string PLAYER_ISNT_BANED => "The player isn't banned.";
        public virtual string PLAYER_LOGGED_IN => "~b~~h~{0}~h~ ~w~logged in.";
        public virtual string PLAYER_LOGGED_OUT => "~b~~h~{0}~h~ ~w~logged out.";
        public virtual string PLAYER_NOT_MUTED => "The player is not muted!";
        public virtual string PLAYER_REJECTED_YOUR_INVITATION => "{0} rejected your team-invitation.";
        public virtual string PLAYER_WITH_NAME_ALREADY_EXISTS => "A player with that name already exists.";
        public virtual string PRIVATE_CHAT_CLOSED_PARTNER => "Your private chat partner closed the chat.";
        public virtual string PRIVATE_CHAT_CLOSED_YOU => "You closed the private chat.";
        public virtual string PRIVATE_CHAT_DISCONNECTED => "Your private chat partner disconnected.";
        public virtual string PRIVATE_CHAT_OPENED_WITH => "Private chat with {0} opened.";
        public virtual string PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER => "Chat request by {0} has been withdrawn.";
        public virtual string PRIVATE_CHAT_REQUEST_CLOSED_YOU => "You've withdrawn the chat request.";
        public virtual string PRIVATE_CHAT_REQUEST_RECEIVED_FROM => "You've received a private chat request from {0}.";
        public virtual string PRIVATE_CHAT_REQUEST_SENT_TO => "You've sent a private chat request to {0}.";
        public virtual string PRIVATE_MESSAGE_SENT => "The private message has been sent.";

        public virtual string REASON_MISSING => "The reason is missing!";
        public virtual string REPORT_ANSWERED_INFO => "The creator answered to his report with ID {0}.";
        public virtual string REPORT_CREATED_INFO => "A report with ID {0} was created.";
        public virtual string REPORT_GOT_ANSWER_INFO => "A team-member answered your report with ID {0}.";
        public virtual string RESOURCE_RESTART_INFO_MINUTES => "TDS-V will restart in {0} minute(s).";
        public virtual string RESOURCE_RESTART_INFO_SECONDS => "TDS-V will restart in {0} second(s).";
        public virtual string ROUND_END_BOMB_DEFUSED_INFO => "Bomb defused!<br>Team {0} wins!";
        public virtual string ROUND_END_BOMB_EXPLODED_INFO => "Bomb exploded!<br>Team {0} wins!";
        public virtual string ROUND_END_COMMAND_INFO => "Map was skipped by {0}!";
        public virtual string ROUND_END_DEATH_INFO => "All opponents are dead!<br>Team {0} wins!";
        public virtual string ROUND_END_DEATH_ALL_INFO => "All players are dead!<br>No team wins!";
        public virtual string ROUND_END_NEW_PLAYER_INFO => "Enough players in ...<br>Round starting!";
        public virtual string ROUND_END_TARGET_EMPTY_INFO => "The attackers had not been at the target for too long.<br>Owners win!";
        public virtual string ROUND_END_TIME_INFO => "Time's up!<br>Team {0} wins because of most HP left!";
        public virtual string ROUND_END_TIME_TIE_INFO => "Time's up!<br>Tie!";
        public virtual string ROUND_MISSION_BOMB_BAD => "Mission: Let the bomb explode on one of the spots or kill all opponents!";
        public virtual string ROUND_MISSION_BOMB_GOOD => "Mission: Prevent the bomb-explosion or kill all opponents!";
        public virtual string ROUND_MISSION_BOMG_SPECTATOR => "Mission: Place & defend the bomb or defuse it - or kill all opponents.";
        public virtual string ROUND_MISSION_NORMAL => "Mission: All opponents have to die.";
        public virtual string ROUND_REWARD_INFO => "#w#Round-reward:#n#Kills: #g#${0}#n##w#Assists: #g#${1}#n##w#Damage: #g#${2}#n##o#Total: #g#${3}";

        public virtual string SENT_APPLICATION => "You sent an invitation.";
        public virtual string SENT_APPLICATION_TO => "You sent {0} an invitation.";
        public virtual string STILL_BANED_IN_LOBBY => "You are still baned until {0} by {1}. Reason: {2}";
        public virtual string STILL_MUTED => "You are still muted for {0} minutes.";
        public virtual string STILL_PERMABANED_IN_LOBBY => "You are still permanently baned by {0} in this lobby. Reason: {1}";
        public virtual string STILL_PERMAMUTED => "You are still permamuted.";
        public virtual string SUPPORT_REQUEST_CREATED => "The support request has been created.";

        public virtual string TARGET_ACCEPTED_INVITATION => "{0} accepted your invitation.";
        public virtual string TARGET_ADDED_BLOCK => "You blocked {0}.";
        public virtual string TARGET_ALREADY_BLOCKED => "You already blocked {0}.";
        public virtual string TARGET_ALREADY_IN_PRIVATE_CHAT => "The target is already in a private chat.";
        public virtual string TARGET_NOT_BLOCKED => "{0} is not blocked.";
        public virtual string TARGET_NOT_IN_SAME_LOBBY => "The target is not in the same lobby.";
        public virtual string TARGET_NOT_LOGGED_IN => "The target is not logged in.";
        public virtual string TARGET_PLAYER_DEFEND_INFO => "Your teammate {0} is at the target. Defend him!";
        public virtual string TARGET_REJECTED_INVITATION => "{0} turned down your invitation.";
        public virtual string TARGET_REMOVED_FRIEND_ADDED_BLOCK => "The player {0} is not your friend anymore and got blocked now.";
        public virtual string TESTING_MAP_NOTIFICATION => "This is a newly created map, the round-stats will not be saved.";
        public virtual string TEXT_TOO_LONG => "The text is too long.";
        public virtual string TEXT_TOO_SHORT => "The text is too short.";
        public virtual string TIMEBAN_INFO => "{0} was baned for {1} hours by {2}. Reason: {3}";
        public virtual string TIMEBAN_YOU_INFO => "You were baned for {0} hours by {1}. Reason: {2}";
        public virtual string TIMEBAN_LOBBY_INFO => "{0} was baned for {1} hours in lobby '{2}' by {3}. Reason: {4}";
        public virtual string TIMEBAN_LOBBY_YOU_INFO => "You were baned for {0} hours from lobby '{1}' by {2}. Reason: {3}";
        public virtual string TIMEMUTE_INFO => "{0} was muted for {1} minutes by {2}. Reason: {3}";
        public virtual string TIMEVOICEMUTE_INFO => "{0} was muted in voice-chat for {1} minutes by {2}. Reason: {3}";
        public virtual string TOO_LONG_OUTSIDE_MAP => "You've been too long outside the map.";

        public virtual string UNBAN_INFO => "{0} was unbaned by {1}. Reason: {2}";
        public virtual string UNBAN_LOBBY_INFO => "{0} was unbaned in lobby '{1}' by {2}. Reason: {3}";
        public virtual string UNBAN_YOU_LOBBY_INFO => "You got unbaned in lobby '{0}' by {1}. Reason: {2}";
        public virtual string UNMUTE_INFO => "{0} was unmuted by {1}. Reason: {2}";
        public virtual string UNVOICEMUTE_INFO => "{0} was unmuted in voice-chat by {1}. Reason: {2}";

        public virtual string VOICE_MUTE_EXPIRED => "Your voice channel mute has expired. You can talk again.";

        public virtual string WRONG_PASSWORD => "Wrong password!";

        public virtual string YOU_ACCEPTED_INVITATION => "You accepted the invitation of {0}.";
        public virtual string YOU_ACCEPTED_TEAM_INVITATION => "You accepted the team-invitation. {0} is your team-leader now.";
        public virtual string YOU_GAVE_MONEY_TO_WITH_FEE => "You gave ${0} (${1} fee) to {2}.";
        public virtual string YOU_GOT_BLOCKED_BY => "You got blocked by {0}.";
        public virtual string YOU_GOT_INVITATION_BY => "You got an invitation to join the team from {0}.";
        public virtual string YOU_GOT_MONEY_BY_WITH_FEE => "You got ${0} (${1} fee) from {2}.";
        public virtual string YOU_GOT_UNBLOCKED_BY => "You got unblocked by {0}.";
        public virtual string YOU_REJECTED_INVITATION => "You rejected the invitation of {0}.";
        public virtual string YOU_REJECTED_TEAM_INVITATION => "You rejected the team-invitation of {0}.";
        public virtual string YOU_UNBLOCKED => "You unblocked {0}.";
    }
}