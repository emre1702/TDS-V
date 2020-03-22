using System;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Commands
{
    public class AdminCommandsHandler
    {
        private readonly ChatHandler _chatHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly LangHelper _langHelper;

        public AdminCommandsHandler(ChatHandler chatHandler, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, LangHelper langHelper)
            => (_chatHandler, _loggingHandler, _lobbiesHandler, _langHelper) = (chatHandler, loggingHandler, lobbiesHandler, langHelper);

        [TDSCommand(AdminCommand.AdminSay)]
        public void AdminSay(TDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }

        [TDSCommand(AdminCommand.AdminChat)]
        public void AdminChat(TDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommand(AdminCommand.NextMap)]
        public void NextMap(TDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);
            if (arena.CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
            {
                arena.CurrentRoundEndBecauseOfPlayer = player;
                arena.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Command);
            }
        }

        [TDSCommand(AdminCommand.LobbyKick)]
        public async void LobbyKick(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player == target)
                return;
            if (target.Lobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                _loggingHandler.LogAdmin(LogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            else
            {
                if (player.Lobby != target.Lobby)
                {
                    player.SendMessage(player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    return;
                }
                target.Lobby.SendAllPlayerLangMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            target.Lobby.RemovePlayer(target);
            await _lobbiesHandler.MainMenu.AddPlayer(target, 0).ConfigureAwait(false);
        }

        [TDSCommand(AdminCommand.LobbyBan, 1)]
        public void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;
            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;
            if (length == DateTime.MinValue)
                lobby.UnbanPlayer(player, target, reason);
            else if (length == DateTime.MaxValue)
                lobby.BanPlayer(player, target, null, reason);
            else
                lobby.BanPlayer(player, target, length, reason);
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 0)]
        public void LobbyBanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;
            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == DateTime.MinValue)
                lobby.UnbanPlayer(player, dbTarget, reason);
            else if (length == DateTime.MaxValue)
                lobby.BanPlayer(player, dbTarget, null, reason);
            else
                lobby.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Lobby_Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Ban, 1)]
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (length == DateTime.MinValue)
                _lobbiesHandler.MainMenu.UnbanPlayer(player, target, reason);
            else if (length == DateTime.MaxValue)
                _lobbiesHandler.MainMenu.BanPlayer(player, target, null, reason);
            else
                _lobbiesHandler.MainMenu.BanPlayer(player, target, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Ban, 0)]
        public void BanPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, DateTime length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (length == DateTime.MinValue)
                _lobbiesHandler.MainMenu.UnbanPlayer(player, dbTarget, reason);
            else if (length == DateTime.MaxValue)
                _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, null, reason);
            else
                _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Kick)]
        public void KickPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_INFO, target.DisplayName, player.DisplayName, reason));
            target.ModPlayer?.Kick(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 1)]
        public void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 0)]
        public void MutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.VoiceMute, 0)]
        public void VoiceMutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.VoiceMute, 1)]
        public void VoiceMutePlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            Account.ChangePlayerVoiceMuteTime(player, target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.VoiceMute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoPlayer(TDSPlayer player, TDSCommandInfos cmdinfos, TDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
                return;

            Vector3 targetpos = NAPI.Entity.GetEntityPosition(target.Player);

            #region Admin is in vehicle
            if (player.ModPlayer.IsInVehicle)
            {
                NAPI.Entity.SetEntityPosition(player.ModPlayer.Vehicle, targetpos.Around(2f));
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
                _loggingHandler.LogAdmin(LogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoVector(TDSPlayer player, TDSCommandInfos cmdinfos, float x, float y, float z, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Player is null)
                return;

            Vector3 pos = new Vector3(x, y, z);
            NAPI.Entity.SetEntityPosition(player.Player, pos);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Test)]
        public void Test(TDSPlayer player, string type, string arg1, string arg2, string arg3)
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

        private bool IsMuteTimeValid(int muteTime, TDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                outputTo.SendMessage(outputTo.Language.MUTETIME_INVALID);
                return false;
            }
            return true;
        }

        /*private readonly Dictionary<string, uint> neededLevels = new Dictionary<string, uint> {
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
		public void TestSkin ( Player player, PedHash hash ) {
			if ( player.IsAdminLevel( neededLevels["testskin"] ) ) {
				player.SetSkin( hash );
			}
		}

		[Command( "testweapon" )]
		public void TestWeapon ( Player player, string name ) {
			if ( player.IsAdminLevel( neededLevels["testweapon"] ) ) {
				NAPI.Player.GivePlayerWeapon( player, NAPI.Util.WeaponNameToModel( name ), 1000 );
			}
		}
		#endregion

		#endregion

		#region RCON
		[Command( "rcon" )]
		public void AddRCONRights ( Player player ) {
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
