using System;
using System.Collections.Generic;
using TDS.Server.Data.Interfaces;

namespace TDS.Server.Data.Languages
{
    public class English : ILanguage
    {
        public virtual string A_MAP_WAS_ALREADY_BOUGHT => "A map has already been purchased/set.";
        public virtual string ACCOUNT_DOESNT_EXIST => "Account doesn't exist.";
        public virtual string ADDED_THE_GANG_HOUSE_SUCCESSFULLY => "The gang house was successfully added.";
        public virtual string ALREADY_IN_PRIVATE_CHAT_WITH => "You are already in a private chat with {0}.";

        public virtual string BALANCE_TEAM_INFO => "{0} was put into another team to balance the teams.";
        public virtual string BOMB_PLANT_INFO => "To plant the bomb you have to take your fists and hold the left mouse button on one of the bomb-spots.";
        public virtual string BOMB_PLANTED => "Bomb was planted!";

        public virtual string CHAR_IN_NAME_IS_NOT_ALLOWED => "The character '{0}' in your name is not allowed.";
        public virtual string COMMAND_DOESNT_EXIST => "The command doesn't exist.";
        public virtual string COMMAND_TOO_LESS_ARGUMENTS => "You used too few arguments for this command. Expected are {0}, but given are {1} arguments.";
        public virtual string COMMAND_TOO_MANY_ARGUMENTS => "You used too many arguments for this command. Expected are {0}, but given are {1} arguments.";
        public virtual string COMMAND_USED_WRONG => "The command usage was wrong.";
        public virtual string COMMITED_SUICIDE => "You commited suicide.";
        public virtual string CONNECTING => "connecting ...";
        public virtual string CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR => "This name is already taken.";
        public virtual string CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR => "This name is not allowed for lobbies.";
        public virtual string CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR => "Unknown error when creating the lobby.";

        public virtual string DEATH_DIED_INFO => "{0} died";
        public virtual string DEATH_KILLED_INFO => "{0} killed {1} with {2}";

        public virtual List<string> DEFUSE_INFO => new List<string> {
            "Round-time got changed. Now you have to kill all opponents or defuse the bomb.",
            "To defuse the bomb go to the red blip on your minimap (bomb), take your fists and hold the left mouse button."
        };

        public virtual string DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO => "The player '{0}' has set your discord user id in TDS-V."
            + Environment.NewLine + "Were you that player?"
            + Environment.NewLine + "Please confirm the setting with the command '!confirmtds'.";

        public virtual string DISCORD_IDENTITY_SAVE_FAILED => "Saving the Discord user id failed: {0}";
        public virtual string DISCORD_IDENTITY_SAVED_SUCCESSFULLY => "The discord user id has been successfully saved.";

        public virtual string ERROR_INFO => "An error occured. The Developer(s) has/have been notified.";

        public virtual string FRIEND_JOINED_LOBBY_INFO => "~g~Your friend {0} has joined the lobby '{1}'.";
        public virtual string FRIEND_LEFT_LOBBY_INFO => "~r~Your friend {0} has left the lobby '{1}'.";
        public virtual string FRIEND_LOGGEDIN_INFO => "Your friend {0} has logged in.";
        public virtual string FRIEND_LOGGEDOFF_INFO => "Your friend {0} has logged off.";
        public virtual string GANG_DOESNT_EXIST_ANYMORE => "The gang doesn't exist anymore!";
        public virtual string GANG_INVITATION_INFO => "You've got a gang invitation.";
        public virtual string GANG_INVITATION_WAS_REMOVED => "The invitation already got removed!";
        public virtual string GANG_LEVEL_MAX_ALLOWED => "A maximum of {0} is allowed as gang level.";
        public virtual string GANG_REMOVED => "Your gang got disbanded.";
        public virtual string GANGACTION_CANT_JOIN_AGAIN => "You are not allowed to join the gang action again.";
        public virtual string GANGWAR_STARTED_INFO => "The gang '{0}' is preparing a gangwar.";
        public virtual string GIVE_MONEY_NEED_FEE => "You need ${0} with ${1} fee included.";
        public virtual string GIVE_MONEY_TOO_LESS => "The amount you tried to give is too less.";
        public virtual string GOT_ASSIST => "You got the assist of {0}.";
        public virtual string GOT_LAST_HITTED_KILL => "You hitted {0} last and got the kill.";
        public virtual string GOT_LOBBY_BAN => "You got a ban from the lobby-owner here. Duration: {0} | Reason: {1}";
        public virtual string GOT_UNREAD_OFFLINE_MESSAGES => "You got {0} unread offline message(s) in userpanel";
        public virtual string HITSOUND_ACTIVATED => "Hitsound activated!";
        public virtual string HITSOUND_DEACTIVATED => "Hitsound deactivated!";

        public virtual string INVITATION_LOBBY => "You've been invited to the '{1}' lobby by {0}.";
        public virtual string INVITATION_WAS_WITHDRAWN_OR_REMOVED => "The invitation has already been withdrawn or removed.";

        public virtual string JOINED_LOBBY_MESSAGE => "You joined lobby \"{0}\".\nUse '/{1}' to leave.";

        public virtual string KICK_INFO => "{0} was kicked by {1}. Reason: {2}";
        public virtual string KICK_LOBBY_INFO => "{0} was kicked by {1} out of the lobby. Reason: {2}";
        public virtual string KICK_YOU_INFO => "You were kicked by {0}. Reason: {1}";
        public virtual string KILLING_SPREE_HEALTHARMOR => "{0} has got a {1}-killingspree and gets a {2} life/armor bonus.";

        public virtual string LOBBY_DOESNT_EXIST => "This lobby does not exist.";
        public virtual string LOBBY_ERROR_REMOVE => "The lobby has got an error and got removed. The developer are informed.";

        public virtual string MAP_BUY_INFO => "{0} has bought the map {1}.";
        public virtual string MAP_WON_VOTING => "The map {0} won the voting!";
        public virtual string MUTE_EXPIRED => "The chat mute has expired. You can write again.";
        public virtual string MUTETIME_INVALID => "The mute time is invalid. Allowed are -1, 0 and higher.";

        public virtual string NEW_OFFLINE_MESSAGE => "You have received a new offline message.";
        public virtual string NOT_ALLOWED => "You are not allowed to do that.";
        public virtual string NOT_ENOUGH_MONEY => "You don't have enough money.";
        public virtual string NOT_IN_PRIVATE_CHAT => "You are not in a private chat.";
        public virtual string NOT_MORE_MAPS_FOR_VOTING_ALLOWED => "There can be only 6 maps in voting!";
        public virtual string NOT_POSSIBLE_IN_THIS_LOBBY => "Not possible in this lobby!";

        public virtual string ONLY_ALLOWED_IN_GANG_LOBBY => "That is only allowed in the gang lobby.";
        public virtual string ORDER_ATTACK => "Attack! Go go go!";
        public virtual string ORDER_GO_TO_BOMB => "Go to the bomb!";
        public virtual string ORDER_SPREAD_OUT => "Spread out!";
        public virtual string ORDER_STAY_BACK => "Stay back!";

        public virtual string PERMABAN_INFO => "{0} was baned permanently by {1}. Reason: {2}";
        public virtual string PERMABAN_LOBBY_INFO => "{0} was baned permanently in lobby '{1}' by {2}. Reason: {3}";
        public virtual string PERMABAN_LOBBY_YOU_INFO => "You was baned for {0} hours from lobby '{1}' by {2}. Reason {3}";
        public virtual string PERMABAN_YOU_INFO => "You were baned permanently by {0}. Reason: {1}";
        public virtual string PERMAMUTE_INFO => "{0} was muted permanently by {1}. Reason: {2}";
        public virtual string PERMAVOICEMUTE_INFO => "{0} was muted permanently in voice-chat by {1}. Reason: {2}";
        public virtual string PLAYER_ACCEPTED_YOUR_INVITATION => "{0} accepted your team-invitation. He is your member now.";
        public virtual string PLAYER_ALREADY_MUTED => "The player is already muted!";
        public virtual string PLAYER_ALREADY_ON_THIS_PAGE => "{0} is already on this page. Only one person may edit this page at a time.";
        public virtual string PLAYER_DOESNT_EXIST => "The player doesn't exist!";
        public virtual string PLAYER_ISNT_BANED => "The player isn't banned.";
        public virtual string PLAYER_JOINED_YOUR_GANG => "{0} joined your gang.";
        public virtual string PLAYER_LOGGED_IN => "~b~~h~{0}~h~ ~w~logged in.";
        public virtual string PLAYER_LOGGED_OUT => "~b~~h~{0}~h~ ~w~logged out.";
        public virtual string PLAYER_NOT_IN_YOUR_GANG => "The target is not in your gang (anymore).";
        public virtual string PLAYER_NOT_MUTED => "The player is not muted!";
        public virtual string PLAYER_REGISTERED => "A new player named \"{0}\" has registered!";
        public virtual string PLAYER_REJECTED_YOUR_INVITATION => "{0} rejected your team-invitation.";
        public virtual string PLAYER_WITH_NAME_ALREADY_EXISTS => "A player with that name already exists.";
        public virtual string PLAYER_WON_INFO => "{0} has won the round.";
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
        public virtual string ROUND_END_DEATH_ALL_INFO => "All players are dead!<br>No team wins!";
        public virtual string ROUND_END_DEATH_INFO => "All opponents are dead!<br>Team {0} wins!";
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
        public virtual string TARGET_ALREADY_IN_A_GANG => "The target is already in a gang.";
        public virtual string TARGET_ALREADY_IN_PRIVATE_CHAT => "The target is already in a private chat.";
        public virtual string TARGET_NOT_BLOCKED => "{0} is not blocked.";
        public virtual string TARGET_NOT_IN_SAME_LOBBY => "The target is not in the same lobby.";
        public virtual string TARGET_NOT_LOGGED_IN => "The target is not logged in.";
        public virtual string TARGET_PLAYER_DEFEND_INFO => "Your teammate {0} is at the target. Defend him!";
        public virtual string TARGET_RANK_IS_HIGHER_OR_EQUAL => "The target's rank is higher or equal to yours.";
        public virtual string TARGET_REJECTED_INVITATION => "{0} turned down your invitation.";
        public virtual string TARGET_REMOVED_FRIEND_ADDED_BLOCK => "The player {0} is not your friend anymore and got blocked now.";
        public virtual string TEAM_ALREADY_FULL_INFO => "Your team is already full. Wait for a opponent to join and try again.";
        public virtual string TESTING_MAP_NOTIFICATION => "This is a newly created map, the round-stats will not be saved.";
        public virtual string TEXT_TOO_LONG => "The text is too long.";
        public virtual string TEXT_TOO_SHORT => "The text is too short.";
        public virtual string THE_RANK_IS_INVALID_REFRESH_WINDOW => "The rank is invalid. Please refresh the window.";
        public virtual string TIMEBAN_INFO => "{0} was baned for {1} hours by {2}. Reason: {3}";
        public virtual string TIMEBAN_LOBBY_INFO => "{0} was baned for {1} hours in lobby '{2}' by {3}. Reason: {4}";
        public virtual string TIMEBAN_LOBBY_YOU_INFO => "You were baned for {0} hours from lobby '{1}' by {2}. Reason: {3}";
        public virtual string TIMEBAN_YOU_INFO => "You were baned for {0} hours by {1}. Reason: {2}";
        public virtual string TIMEMUTE_INFO => "{0} was muted for {1} minutes by {2}. Reason: {3}";
        public virtual string TIMEVOICEMUTE_INFO => "{0} was muted in voice-chat for {1} minutes by {2}. Reason: {3}";
        public virtual string TOO_LONG_OUTSIDE_MAP => "You've been too long outside the map.";
        public virtual string TRY_AGAIN_LATER => "Try again later.";

        public virtual string UNBAN_INFO => "{0} was unbaned by {1}. Reason: {2}";
        public virtual string UNBAN_LOBBY_INFO => "{0} was unbaned in lobby '{1}' by {2}. Reason: {3}";
        public virtual string UNBAN_YOU_LOBBY_INFO => "You got unbaned in lobby '{0}' by {1}. Reason: {2}";
        public virtual string UNMUTE_INFO => "{0} was unmuted by {1}. Reason: {2}";
        public virtual string UNVOICEMUTE_INFO => "{0} was unmuted in voice-chat by {1}. Reason: {2}";

        public virtual string VOICE_MUTE_EXPIRED => "Your voice channel mute has expired. You can talk again.";

        public virtual string WRONG_PASSWORD => "Wrong password!";

        public virtual string YOU_ACCEPTED_INVITATION => "You accepted the invitation of {0}.";
        public virtual string YOU_ACCEPTED_TEAM_INVITATION => "You accepted the team-invitation. {0} is your team-leader now.";
        public virtual string YOU_ALREADY_INVITED_TARGET => "You've already invited the target.";
        public virtual string YOU_ARE_ALREADY_IN_A_GANG => "You are already in a gang.";
        public virtual string YOU_ARE_NOT_IN_A_GANG => "You are not in a gang.";
        public virtual string YOU_CANT_BUY_A_MAP_IN_CUSTOM_LOBBY => "You can't buy a map in a custom lobby.";
        public virtual string YOU_GAVE_MONEY_TO_WITH_FEE => "You gave ${0} (${1} fee) to {2}.";
        public virtual string YOU_GOT_BLOCKED_BY => "You got blocked by {0}.";
        public virtual string YOU_GOT_INVITATION_BY => "You got an invitation to join the team from {0}.";
        public virtual string YOU_GOT_KICKED_OUT_OF_THE_GANG_BY => "You got kicked out of the gang '{1}' by {0}.";
        public virtual string YOU_GOT_MONEY_BY_WITH_FEE => "You got ${0} (${1} fee) from {2}.";
        public virtual string YOU_GOT_RANK_DOWN_BY => "You got a rank down from {1} to {2} by {0}.";
        public virtual string YOU_GOT_RANK_UP => "You got a rank up from {1} to {2} by {0}.";
        public virtual string YOU_GOT_UNBLOCKED_BY => "You got unblocked by {0}.";
        public virtual string YOU_JOINED_THE_GANG => "You've joined the gang '{0}'.";
        public virtual string YOU_KICKED_OUT_OF_GANG => "You kicked {0} out of the gang.";
        public virtual string YOU_REJECTED_GANG_INVITATION => "You've rejected the gang invitation from the gang '{0}'.";
        public virtual string YOU_REJECTED_INVITATION => "You rejected the invitation of {0}.";
        public virtual string YOU_REJECTED_TEAM_INVITATION => "You rejected the team-invitation of {0}.";
        public virtual string YOU_UNBLOCKED => "You unblocked {0}.";
        public virtual string YOUVE_BECOME_GANG_LEADER => "You've become the gang owner.";
        public virtual string YOUVE_LEFT_THE_GANG => "You've left the gang.";

        public virtual string NO_EMAIL_ADDRESS_HAS_BEEN_SET => "This account has not set any email address.";

        public virtual string EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID => "The email address for this account is invalid.";
        public virtual string PASSWORD_HAS_BEEN_RESET_EMAIL_SENT => "Your password has been successfully reset. Check your emails (look into junk mails folder!).";
        public virtual string REGISTER_FAILED_DEVS_INFORMED => "An error has occured while trying to register. The developers are informed and will fix this problem as soon as possible. Please try again later.";
        public virtual string GANGWAR_TARGET_EMPTY_SECS_LEFT => "The target is empty! You have {0} seconds to occupy it again.";
        public virtual string GANG_ACTION_IN_PREPARATION => "An attack is prepared in the area {0}.";
        public virtual string GANG_ACTION_IN_PREPARATION_ATTACKER => "Your gang is preparing an attack against {0} in the area {1}.";
        public virtual string GANG_ACTION_IN_PREPARATION_OWNER => "A gang is preparing an attack in your area {0}.";
        public virtual string GANG_ACTION_STARTED => "An attack has been started by {0} against {1}. Area: {2}";
        public virtual string GANG_AREA_CONQUERED_WITH_OWNER => "The area {0} has been successfully conquered by {1} from owners {2}.";
        public virtual string GANG_AREA_CONQUERED_WITHOUT_OWNER => "The area {0} has been successfully taken by {1}.";
        public virtual string GANG_AREA_DEFENDED => "The area {0} has been successfully defended by {1} against attackers {2}.";
        public virtual string GANG_ACTION_AREA_IN_COOLDOWN => "The area has a cooldown. Try again after {0} minutes.";
        public virtual string GANG_ACTION_AREA_OWNER_IN_ACTION => "The owner of that area is already in an gang action.";
        public virtual string GANG_ACTION_ATTACK_PREPARATION_INVITATION => "Your gang is preparing an attack for area {0}!\n(Accept or use /attack)";
        public virtual string GANG_ACTION_ATTACK_INVITATION => "Your gang is attacking area {0}!\n(Accept or use /attack)";
        public virtual string GANG_ACTION_DEFEND_INVITATION => "Defend your gang area {0}!\n(Accept or use /defend)";
        public virtual string GANG_ACTION_TEAM_YOURS_PLAYER_JOINED_INFO => "{0} joined the gang action for your gang.";
        public virtual string GANG_ACTION_TEAM_OPPONENT_PLAYER_JOINED_INFO => "{0} joined the gang action on the opponents side.";
        public virtual string YOUR_GANG_ALREADY_REACHED_MAX_ATTACK_COUNT => "Your gang has already reached the max. attack count of {0} for today.";
        public virtual string YOUR_GANG_ALREADY_IN_ACTION => "Your gang is already in an gang action.";
        public virtual string NOT_ENOUGH_PLAYERS_ONLINE_IN_YOUR_GANG => "There are not enough players online in your gang (expected {0}).";
        public virtual string GANG_ACTION_AREA_DOES_NOT_EXIST => "The gang area does not exist?!";
        public virtual string NOT_ENOUGH_PLAYERS_ONLINE_IN_TARGET_GANG => "There are not enough players online in the opponents gang (expected {0}).";
        public virtual string ADMIN_LEVEL_MIN_NUMBER => "The admin level has to be atleast {0}.";
        public virtual string ADMIN_LEVEL_MAX_NUMBER => "The admin level can be at most {0}.";
        public virtual string MIN_NUMBER => "The number has to be atleast {0}.";
        public virtual string MAX_NUMBER => "The number can be at most {0}.";
    }
}
