﻿using AltV.Net.Async;
using AltV.Net.Data;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Commands
{
    partial class BaseCommands
    {
        #region Public Methods

        [TDSCommand(AdminCommand.AdminChat)]
        public void AdminChat(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminChat(player, text);
        }

        [TDSCommand(AdminCommand.AdminSay)]
        public void AdminSay(ITDSPlayer player, [TDSRemainingText] string text)
        {
            _chatHandler.SendAdminMessage(player, text);
        }

        [TDSCommand(AdminCommand.Ban, 1)]
        public async Task BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            PlayerBans? ban = null;
            if (length == TimeSpan.MinValue)
                await AltAsync.Do(() => _lobbiesHandler.MainMenu.UnbanPlayer(player, target, reason));
            else if (length == TimeSpan.MaxValue)
                ban = await _lobbiesHandler.MainMenu.BanPlayer(player, target, null, reason);
            else
                ban = await _lobbiesHandler.MainMenu.BanPlayer(player, target, length, reason);

            if (ban is { })
                await AltAsync.Do(() => Utils.HandleBan(target, ban));

            if (!cmdinfos.AsLobbyOwner)
                await AltAsync.Do(() => _loggingHandler.LogAdmin(LogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.Ban, 0)]
        public async Task BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (length == TimeSpan.MinValue)
                _lobbiesHandler.MainMenu.UnbanPlayer(player, dbTarget, reason);
            else if (length == TimeSpan.MaxValue)
                await _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, null, reason);
            else
                await _lobbiesHandler.MainMenu.BanPlayer(player, dbTarget, length, reason);

            if (!cmdinfos.AsLobbyOwner)
                await AltAsync.Do(() => _loggingHandler.LogAdmin(LogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            var targetPos = target.Position;

            #region Admin is in vehicle
            if (player.IsInVehicle && player.Vehicle is { })
            {
                player.Vehicle.Position = targetPos.Around(2f, false);
                return;
            }
            #endregion Admin is in vehicle

            #region Target is in vehicle and we want to sit in it
            if (target.IsInVehicle && target.Vehicle is { })
            {
                if (target.Vehicle.HasFreeSeat)
                {
                    player.SetIntoVehicle(target.Vehicle);
                    return;
                }
            }
            #endregion Target is in vehicle and we want to sit in it

            #region Normal
            player.Position = targetPos.Around(2f, false);
            #endregion Normal

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoVector(ITDSPlayer player, TDSCommandInfos cmdinfos, Position pos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            player.Position = pos;

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Kick)]
        public void KickPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_INFO, target.DisplayName, player.DisplayName, reason));
            target.SendNotification(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
            target.SendMessage(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));

            var _ = new TDSTimer(() =>
            {
                if (target.Exists)
                    target.Kick("Kick");
            }, 2000);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
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

        [TDSCommand(AdminCommand.LobbyKick)]
        public async Task LobbyKick(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player == target)
                return;
            if (target.Lobby is null)
                return;
            if (!cmdinfos.AsLobbyOwner)
            {
                await AltAsync.Do(() =>
                {
                    _loggingHandler.LogAdmin(LogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                    _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
                });
            }
            else
            {
                await AltAsync.Do(() =>
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
        public async Task MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                await AltAsync.Do(() => _loggingHandler.LogAdmin(LogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
        }

        [TDSCommand(AdminCommand.NextMap)]
        public void NextMap(ITDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);
            if (arena.CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
            {
                arena.CurrentRoundEndBecauseOfPlayer = player;
                arena.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Command);
            }
        }

        [TDSCommand(AdminCommand.Test)]
        public void Test(ITDSPlayer player, string type, string? arg1 = null, string? arg2 = null, string? arg3 = null)
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

                    player.SetClothes(slot, drawable, texture);
                    break;

                case "admin":
                    if (!short.TryParse(arg1, out short adminLvl))
                    {
                        player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true);
                        return;
                    }
                    player.Entity!.AdminLvl = adminLvl;
                    player.SetStreamSyncedMetaData(PlayerDataKey.AdminLevel.ToString(), adminLvl);
                    break;

                default:
                    player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true);
                    break;
            }
        }

        [TDSCommand(AdminCommand.VoiceMute, 0)]
        public async Task VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                await AltAsync.Do(() => _loggingHandler.LogAdmin(LogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP));
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

        [TDSCommand(AdminCommand.CreateHouse)]
        public void CreateHouse(ITDSPlayer player, byte neededGangLevel)
        {
            if (player.Entity is null)
                return;

            if (!(player.Lobby is IGangLobby))
            {
                player.SendNotification(player.Language.ONLY_ALLOWED_IN_GANG_LOBBY);
                return;
            }

            if (neededGangLevel > _gangLevelsHandler.HighestLevel)
            {
                player.SendNotification(string.Format(player.Language.GANG_LEVEL_MAX_ALLOWED, _gangLevelsHandler.HighestLevel));
                return;
            }

            _gangHousesHandler.AddHouse(player.Position, player.Rotation.Yaw, neededGangLevel, player.Entity.Id);
            player.SendNotification(player.Language.ADDED_THE_GANG_HOUSE_SUCCESSFULLY);
        }

        #endregion Public Methods

        #region Private Methods

        private bool IsMuteTimeValid(int muteTime, ITDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                outputTo.SendMessage(outputTo.Language.MUTETIME_INVALID);
                return false;
            }
            return true;
        }

        #endregion Private Methods

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
