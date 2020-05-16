using System;
using TDS_Server.Data;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Commands
{
    partial class BaseCommands
    {

        [TDSCommand(AdminCommand.AdminSay)]
        public void AdminSay(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }

        [TDSCommand(AdminCommand.AdminChat)]
        public void AdminChat(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommand(AdminCommand.NextMap)]
        public void NextMap(ITDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText(MinLength = 4)] string reason)
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
        public async void LobbyKick(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player == target)
                return;
            if (target.Lobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                _modAPI.Thread.RunInMainThread(() => 
                {
                    _loggingHandler.LogAdmin(LogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
                });
            }
            else
            {
                _modAPI.Thread.RunInMainThread(() =>
                { 
                    if (player.Lobby != target.Lobby)
                    {
                        player.SendMessage(player.Language.TARGET_NOT_IN_SAME_LOBBY);
                        return;
                    }
                    target.Lobby.SendAllPlayerLangMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
                });
            }
            await target.Lobby.RemovePlayer(target);
            await _lobbiesHandler.MainMenu.AddPlayer(target, 0).ConfigureAwait(false);
        }

        [TDSCommand(AdminCommand.LobbyBan, 1)]
        public void LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;
            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;
            if (length == TimeSpan.MinValue)
                lobby.UnbanPlayer(player, target, reason);
            else if (length == TimeSpan.MaxValue)
                lobby.BanPlayer(player, target, null, reason);
            else
                lobby.BanPlayer(player, target, length, reason);
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 0)]
        public void LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;
            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == TimeSpan.MinValue)
                lobby.UnbanPlayer(player, dbTarget, reason);
            else if (length == TimeSpan.MaxValue)
                lobby.BanPlayer(player, dbTarget, null, reason);
            else
                lobby.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Lobby_Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Ban, 1)]
        public async void BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            PlayerBans? ban = null;
            if (length == TimeSpan.MinValue)
                _modAPI.Thread.RunInMainThread(() => _lobbiesHandler.MainMenu.UnbanPlayer(player, target, reason));
            else if (length == TimeSpan.MaxValue)
                ban = await _lobbiesHandler.MainMenu.BanPlayer(player, target, null, reason);
            else
                ban = await _lobbiesHandler.MainMenu.BanPlayer(player, target, length, reason);

            if (ban is { } && target.ModPlayer is { })
                _modAPI.Thread.RunInMainThread(() => Utils.HandleBan(target.ModPlayer, ban));

            if (!cmdinfos.AsLobbyOwner)
                _modAPI.Thread.RunInMainThread(() => _loggingHandler.LogAdmin(LogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.Ban, 0)]
        public async void BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (length == TimeSpan.MinValue)
                _lobbiesHandler.MainMenu.UnbanPlayer(player, dbTarget, reason);
            else if (length == TimeSpan.MaxValue)
                await _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, null, reason);
            else
                await _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                _modAPI.Thread.RunInMainThread(() => _loggingHandler.LogAdmin(LogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.Kick)]
        public void KickPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_INFO, target.DisplayName, player.DisplayName, reason));
            target.SendNotification(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
            target.SendMessage(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));

            var _ = new TDSTimer(() =>
            {
                if (target.ModPlayer is { } && !target.ModPlayer.IsNull)
                    target.ModPlayer?.Kick("Kick");
            }, 2000);
            

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 1)]
        public void MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            target.ChangeMuteTime(target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 0)]
        public async void MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _modAPI.Thread.RunInMainThread(() => _loggingHandler.LogAdmin(LogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.VoiceMute, 0)]
        public async void VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _modAPI.Thread.RunInMainThread(() => _loggingHandler.LogAdmin(LogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.VoiceMute, 1)]
        public void VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            target.ChangeVoiceMuteTime(player, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.VoiceMute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.ModPlayer is null || target.ModPlayer is null)
                return;

            var targetPos = target.ModPlayer.Position;

            #region Admin is in vehicle
            if (player.ModPlayer.IsInVehicle && player.ModPlayer.Vehicle is { } && player.ModPlayer.Vehicle.Vehicle is { })
            {
                player.ModPlayer.Vehicle.Vehicle.Position = targetPos.Around(2f, false);
                return;
            }
            #endregion Admin is in vehicle

            #region Target is in vehicle and we want to sit in it
            if (target.ModPlayer.IsInVehicle && target.ModPlayer.Vehicle is { } && target.ModPlayer.Vehicle.Vehicle is { })
            {
                uint? freeSeat = Utils.GetVehicleFreeSeat(target.ModPlayer.Vehicle);
                if (freeSeat.HasValue)
                {
                    player.ModPlayer.SetIntoVehicle(target.ModPlayer.Vehicle, (int)freeSeat.Value);
                    return;
                }
            }
            #endregion Target is in vehicle and we want to sit in it

            #region Normal
            player.ModPlayer.Position = targetPos.Around(2f, false);
            #endregion Normal

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        //Todo implement Position3D as parameter
        [TDSCommand(AdminCommand.Goto)]
        public void GotoVector(ITDSPlayer player, TDSCommandInfos cmdinfos, float x, float y, float z, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.ModPlayer is null)
                return;


            player.ModPlayer.Position = new Position3D(x, y, z);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Test)]
        public void Test(ITDSPlayer player, string type, string? arg1 = null, string? arg2 = null, string? arg3 = null)
        {
            switch (type)
            {
                case "cloth":
                    if (player.ModPlayer is null)
                        return;

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

                    player.ModPlayer.SetClothes(slot, drawable, texture);
                    break;

                case "admin":
                    if (!short.TryParse(arg1, out short adminLvl))
                    {
                        player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true);
                        return;
                    }
                    player.Entity!.AdminLvl = adminLvl;
                    _dataSyncHandler.SetData(player, PlayerDataKey.AdminLevel, DataSyncMode.All, adminLvl);
                    break;

                default:
                    player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true);
                    break;
            }
        }

        private bool IsMuteTimeValid(int muteTime, ITDSPlayer outputTo)
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
