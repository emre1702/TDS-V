using GTANetworkAPI;
using System;
using TDS_Common.Enum;
using TDS_Server.CustomAttribute;
using TDS_Server.Default;
using TDS_Server.Enum;
using TDS_Server.Instance.Dto;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Commands
{
    internal class AdminCommand
    {
        [TDSCommand(DAdminCommand.AdminSay)]
        public static void AdminSay(TDSPlayer player, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminMessage(player, text);
        }

        [TDSCommand(DAdminCommand.AdminChat)]
        public static void AdminChat(TDSPlayer player, [TDSRemainingText] string text)
        {
            ChatManager.SendAdminChat(player, text);
        }

        [TDSCommand(DAdminCommand.NextMap)]
        public static void NextMap(TDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText] string reason)
        {
            if (!(player.CurrentLobby is Arena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);
            arena.CurrentRoundEndBecauseOfPlayer = player;
            arena.SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Command);
        }

        [TDSCommand(DAdminCommand.LobbyKick)]
        public static async void LobbyKick(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;
            if (player == target)
                return;
            if (target.CurrentLobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                AdminLogsManager.Log(ELogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                LangUtils.SendAllChatMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.Client.Name, player.Client.Name, reason));
            }
            else
            {
                if (player.CurrentLobby != target.CurrentLobby)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    return;
                }
                target.CurrentLobby.SendAllPlayerLangMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.Client.Name, player.Client.Name, reason));
            }
            target.CurrentLobby.RemovePlayer(target);
            await LobbyManager.GetLobby(0).AddPlayer(target, 0);
        }

        [TDSCommand(DAdminCommand.LobbyBan, 1)]
        public static void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;
            if (player.CurrentLobby == null || player.CurrentLobby.Id == 0)
                return;
            if (!player.CurrentLobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;
            if (length == DateTime.MinValue)
                player.CurrentLobby.UnbanPlayer(player, target, reason);
            else if (length == DateTime.MaxValue)
                player.CurrentLobby.BanPlayer(player, target, null, reason);
            else
                player.CurrentLobby.BanPlayer(player, target, length, reason);
            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.LobbyBan, 0)]
        public static void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;

            if (player.CurrentLobby == null || player.CurrentLobby.Id == 0)
                return;
            if (!player.CurrentLobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == DateTime.MinValue)
                player.CurrentLobby.UnbanPlayer(player, dbTarget, reason);
            else if (length == DateTime.MaxValue)
                player.CurrentLobby.BanPlayer(player, dbTarget, null, reason);
            else
                player.CurrentLobby.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Lobby_Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Ban, 1)]
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;

            if (length == DateTime.MinValue)
                LobbyManager.MainMenu.UnbanPlayer(player, target, reason);
            else if (length == DateTime.MaxValue)
                LobbyManager.MainMenu.BanPlayer(player, target, null, reason);
            else
                LobbyManager.MainMenu.BanPlayer(player, target, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Ban, 0)]
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;

            if (length == DateTime.MinValue)
                LobbyManager.MainMenu.UnbanPlayer(player, dbTarget, reason);
            else if (length == DateTime.MaxValue)
                LobbyManager.MainMenu.BanPlayer(player, dbTarget, null, reason);
            else
                LobbyManager.MainMenu.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Kick)]
        public static void KickPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;

            LangUtils.SendAllChatMessage(lang => lang.KICK_INFO.Formatted(target.Client.Name, player.Client.Name, reason));
            target.Client.Kick(target.Language.KICK_YOU_INFO.Formatted(player.Client.Name, reason));

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Mute, 1)]
        public static void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, int minutes, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Mute, 0)]
        public static void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText] string reason)
        {
            if (!IsReasonValid(reason, player))
                return;
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Goto)]
        public static void GotoPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText] string reason)
        {
            Vector3 targetpos = NAPI.Entity.GetEntityPosition(target.Client);

            #region Admin is in vehicle
            if (player.Client.IsInVehicle)
            {
                NAPI.Entity.SetEntityPosition(player.Client.Vehicle, targetpos.Around(2f));
                return;
            }
            #endregion Admin is in vehicle

            #region Target is in vehicle and we want to sit in it
            if (target.Client.IsInVehicle)
            {
                uint? freeSeat = Utils.GetVehicleFreeSeat(target.Client.Vehicle);
                if (freeSeat.HasValue)
                {
                    NAPI.Player.SetPlayerIntoVehicle(player.Client, target.Client.Vehicle, (int)freeSeat.Value);
                    return;
                }
            }
            #endregion Target is in vehicle and we want to sit in it

            #region Normal
            NAPI.Entity.SetEntityPosition(player.Client, targetpos.Around(2f));
            #endregion Normal

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Goto)]
        public static void GotoVector(TDSPlayer player, TDSCommandInfos cmdinfos, float x, float y, float z, [TDSRemainingText] string reason)
        {
            Vector3 pos = new Vector3(x, y, z);
            NAPI.Entity.SetEntityPosition(player.Client, pos);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        private static bool IsReasonValid(string reason, TDSPlayer outputTo)
        {
            if (reason.Length < 3)
            {
                if (outputTo != null)
                    NAPI.Chat.SendChatMessageToPlayer(outputTo.Client, outputTo.Language.REASON_MISSING);
                return false;
            }
            return true;
        }

        private static bool IsMuteTimeValid(int muteTime, TDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                if (outputTo != null)
                    NAPI.Chat.SendChatMessageToPlayer(outputTo.Client, outputTo.Language.MUTETIME_INVALID);
                return false;
            }
            return true;
        }

        /*private static readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
			{ "goto", 2 },
			{ "xyz", 2 },
			{ "cveh", 2 },
			{ "testskin", 2 },
			{ "testweapon", 2 },
			{ "object", 2 }
		};

		[CommandDescription( "Creates a vehicle." )]
		[CommandGroup( "administrator/lobby-owner" )]
		[CommandAlias( "createvehicle" )]
		[Command( "cveh" )]
		public void SpawnCarCommand ( Client player, string name ) {
			if ( player.IsAdminLevel( neededLevels["cveh"], true ) ) {
				VehicleHash model = NAPI.Util.VehicleNameToModel( name );

				Vector3 rot = player.Rotation;
				Vehicle veh = NAPI.Vehicle.CreateVehicle( model, player.Position, rot.Z, 0, 0, numberPlate: player.Name, dimension: player.Dimension );

				NAPI.Player.SetPlayerIntoVehicle( player, veh, -1 );
			}
		}

		[Command( "testskin" )]
		public static void TestSkin ( Client player, PedHash hash ) {
			if ( player.IsAdminLevel( neededLevels["testskin"] ) ) {
				player.SetSkin( hash );
			}
		}

		[Command( "testweapon" )]
		public static void TestWeapon ( Client player, string name ) {
			if ( player.IsAdminLevel( neededLevels["testweapon"] ) ) {
				NAPI.Player.GivePlayerWeapon( player, NAPI.Util.WeaponNameToModel( name ), 1000 );
			}
		}
		#endregion

		#endregion

		#region RCON
		[Command( "rcon" )]
		public static void AddRCONRights ( Client player ) {
			if ( player.IsRcon ) {
				Character character = player.GetChar();
				character.UID = 0;
				character.AdminLvl = 4;
				character.Login();
				character.ArenaStats = new LobbyDeathmatchStats();
				character.CurrentStats = character.ArenaStats;
				character.Lobby = lobby.Arena.TheLobby;
			}
		}
		#endregion*/
    }
}