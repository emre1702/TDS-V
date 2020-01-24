using GTANetworkAPI;
using System;
using TDS_Common.Enum;
using TDS_Server.CustomAttribute;
using TDS_Server.Default;
using TDS_Server.Enums;
using TDS_Server.Instance.Dto;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Commands
{
    class AdminCommand
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
        public static void NextMap(TDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!(player.CurrentLobby is Arena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);
            if (arena.CurrentGameMode?.CanEndRound(ERoundEndReason.NewPlayer) != false)
            {
                arena.CurrentRoundEndBecauseOfPlayer = player;
                arena.SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Command);
            }
        }

        [TDSCommand(DAdminCommand.LobbyKick)]
        public static async void LobbyKick(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player == target)
                return;
            if (target.CurrentLobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                AdminLogsManager.Log(ELogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                LangUtils.SendAllChatMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            else
            {
                if (player.CurrentLobby != target.CurrentLobby)
                {
                    player.SendMessage(player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    return;
                }
                target.CurrentLobby.SendAllPlayerLangMessage(lang => Utils.GetReplaced(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            target.CurrentLobby.RemovePlayer(target);
            await LobbyManager.MainMenu.AddPlayer(target, 0).ConfigureAwait(false);
        }

        [TDSCommand(DAdminCommand.LobbyBan, 1)]
        public static void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.CurrentLobby is null || player.CurrentLobby.Type == ELobbyType.MainMenu)
                return;
            var lobby = player.CurrentLobby;
            if (lobby.Type == ELobbyType.MapCreateLobby) 
                lobby = LobbyManager.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;
            if (length == DateTime.MinValue)
                lobby.UnbanPlayer(player, target, reason);
            else if (length == DateTime.MaxValue)
                lobby.BanPlayer(player, target, null, reason);
            else
                lobby.BanPlayer(player, target, length, reason);
            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.LobbyBan, 0)]
        public static void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.CurrentLobby is null || player.CurrentLobby.Type == ELobbyType.MainMenu)
                return;
             var lobby = player.CurrentLobby;
            if (lobby.Type == ELobbyType.MapCreateLobby)
                lobby = LobbyManager.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == DateTime.MinValue)
                lobby.UnbanPlayer(player, dbTarget, reason);
            else if (length == DateTime.MaxValue)
                lobby.BanPlayer(player, dbTarget, null, reason);
            else
                lobby.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Lobby_Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Ban, 1)]
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
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
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
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
        public static void KickPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            LangUtils.SendAllChatMessage(lang => lang.KICK_INFO.Formatted(target.DisplayName, player.DisplayName, reason));
            target.Player!.Kick(target.Language.KICK_YOU_INFO.Formatted(player.DisplayName, reason));

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Mute, 1)]
        public static void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Mute, 0)]
        public static void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.VoiceMute, 0)]
        public static void VoiceMutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.VoiceMute, 1)]
        public static void VoiceMutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerVoiceMuteTime(player, target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.VoiceMute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Goto)]
        public static void GotoPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Player is null || target.Player is null)
                return;

            Vector3 targetpos = NAPI.Entity.GetEntityPosition(target.Player);

            #region Admin is in vehicle
            if (player.Player.IsInVehicle)
            {
                NAPI.Entity.SetEntityPosition(player.Player.Vehicle, targetpos.Around(2f));
                return;
            }
            #endregion Admin is in vehicle

            #region Target is in vehicle and we want to sit in it
            if (target.Player.IsInVehicle)
            {
                uint? freeSeat = Utils.GetVehicleFreeSeat(target.Player.Vehicle);
                if (freeSeat.HasValue)
                {
                    NAPI.Player.SetPlayerIntoVehicle(player.Player, target.Player.Vehicle, (int)freeSeat.Value);
                    return;
                }
            }
            #endregion Target is in vehicle and we want to sit in it

            #region Normal
            NAPI.Entity.SetEntityPosition(player.Player, targetpos.Around(2f));
            #endregion Normal

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Goto)]
        public static void GotoVector(TDSPlayer player, TDSCommandInfos cmdinfos, float x, float y, float z, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Player is null)
                return;

            Vector3 pos = new Vector3(x, y, z);
            NAPI.Entity.SetEntityPosition(player.Player, pos);

            if (!cmdinfos.AsLobbyOwner)
                AdminLogsManager.Log(ELogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(DAdminCommand.Test)]
        public static void Test(TDSPlayer player, string type, string arg1, string arg2, string arg3)
        {
            switch (type)
            {
                case "cloth":
                    if (!int.TryParse(arg1, out int slot))
                    {
                        player.SendNotification(player.Language.COMMAND_USED_WRONG, true);
                        return;
                    }
                    if (!int.TryParse(arg2, out int drawable))
                    {
                        player.SendNotification(player.Language.COMMAND_USED_WRONG, true);
                        return;
                    }
                    if (!int.TryParse(arg3, out int texture))
                    {
                        player.SendNotification(player.Language.COMMAND_USED_WRONG, true);
                        return;
                    }

                    NAPI.Player.SetPlayerClothes(player.Player, slot, drawable, texture);
                    break;

                default:
                    player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true);
                    break;
            }
        }

        private static bool IsMuteTimeValid(int muteTime, TDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                outputTo.SendMessage(outputTo.Language.MUTETIME_INVALID);
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
		public void SpawnCarCommand ( Player player, string name ) {
			if ( player.IsAdminLevel( neededLevels["cveh"], true ) ) {
				VehicleHash model = NAPI.Util.VehicleNameToModel( name );

				Vector3 rot = player.Rotation;
				Vehicle veh = NAPI.Vehicle.CreateVehicle( model, player.Position, rot.Z, 0, 0, numberPlate: player.Name, dimension: player.Dimension );

				NAPI.Player.SetPlayerIntoVehicle( player, veh, -1 );
			}
		}

		[Command( "testskin" )]
		public static void TestSkin ( Player player, PedHash hash ) {
			if ( player.IsAdminLevel( neededLevels["testskin"] ) ) {
				player.SetSkin( hash );
			}
		}

		[Command( "testweapon" )]
		public static void TestWeapon ( Player player, string name ) {
			if ( player.IsAdminLevel( neededLevels["testweapon"] ) ) {
				NAPI.Player.GivePlayerWeapon( player, NAPI.Util.WeaponNameToModel( name ), 1000 );
			}
		}
		#endregion

		#endregion

		#region RCON
		[Command( "rcon" )]
		public static void AddRCONRights ( Player player ) {
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
