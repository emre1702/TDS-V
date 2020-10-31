using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Commands
{
    partial class BaseCommands
    {
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
                await _lobbiesHandler.MainMenu.Bans.Unban(player, target, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                ban = await _lobbiesHandler.MainMenu.Bans.Ban(player, target, null, reason).ConfigureAwait(false);
            else
                ban = await _lobbiesHandler.MainMenu.Bans.Ban(player, target, length, reason).ConfigureAwait(false);

            if (ban is { })
                NAPI.Task.RunSafe(() => Utils.HandleBan(target, ban));

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Ban, 0)]
        public async Task BanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (length == TimeSpan.MinValue)
                await _lobbiesHandler.MainMenu.Bans.Unban(player, dbTarget, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await _lobbiesHandler.MainMenu.Bans.Ban(player, dbTarget, null, reason).ConfigureAwait(false);
            else
                await _lobbiesHandler.MainMenu.Bans.Ban(player, dbTarget, length, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Ban, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
            var targetPos = target.Position;

            if (player.IsInVehicle && player.Vehicle is { })
            {
                var pos = targetPos.Around(2f, false);
                player.Vehicle.Position = pos;
                return;
            }

            if (target.IsInVehicle && target.Vehicle is { })
            {
                uint? freeSeat = Utils.GetVehicleFreeSeat(target.Vehicle);
                if (freeSeat.HasValue)
                {
                    player.SetIntoVehicle(target.Vehicle, (int) freeSeat.Value);
                    return;
                }
}

                player.Position = targetPos.Around(2f, false);
            });
            
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Goto)]
        public void GotoVector(ITDSPlayer player, TDSCommandInfos cmdinfos, Vector3 pos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player is null)
                return;

            NAPI.Task.RunSafe(() => player.Position = pos);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Goto, player, null, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Kick)]
        public void KickPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, [TDSRemainingText(MinLength = 4)] string reason)
        {
            _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_INFO, target.DisplayName, player.DisplayName, reason));
            NAPI.Task.RunSafe(() =>
            {
                target.SendNotification(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
                target.SendChatMessage(string.Format(target.Language.KICK_YOU_INFO, player.DisplayName, reason));
            });
            
            var _ = new TDSTimer(() =>
            {
                NAPI.Task.RunSafe(() =>
                {
                    if (target is { } && !target.IsNull)
                        target?.Kick("Kick");
                });
                
            }, 2000);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 1)]
        public async Task LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby is IMainMenu)
                return;
            var lobby = player.Lobby;

            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            else if (lobby.Type == LobbyType.DamageTestLobby)
                lobby = _lobbiesHandler.DamageTestLobbyDummy;

            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == TimeSpan.MinValue)
                await lobby.Bans.Unban(player, target, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await lobby.Bans.Ban(player, target, null, reason).ConfigureAwait(false);
            else
                await lobby.Bans.Ban(player, target, length, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Lobby_Ban, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.LobbyBan, 0)]
        public async Task LobbyBanPlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, TimeSpan length, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (player.Lobby is null || player.Lobby.Type == LobbyType.MainMenu)
                return;
            var lobby = player.Lobby;

            if (lobby.Type == LobbyType.MapCreateLobby)
                lobby = _lobbiesHandler.MapCreateLobbyDummy;
            else if (lobby.Type == LobbyType.DamageTestLobby)
                lobby = _lobbiesHandler.DamageTestLobbyDummy;

            if (!lobby.IsOfficial && !cmdinfos.AsLobbyOwner)
                return;

            if (length == TimeSpan.MinValue)
                await lobby.Bans.Unban(player, dbTarget, reason).ConfigureAwait(false);
            else if (length == TimeSpan.MaxValue)
                await lobby.Bans.Ban(player, dbTarget, null, reason).ConfigureAwait(false);
            else
                await lobby.Bans.Ban(player, dbTarget, length, reason).ConfigureAwait(false);

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
                _loggingHandler.LogAdmin(LogType.Lobby_Kick, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
                _langHelper.SendAllChatMessage(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            else
            {
                if (player.Lobby != target.Lobby)
                {
                    NAPI.Task.RunSafe(() =>
                    {
                        player.SendChatMessage(player.Language.TARGET_NOT_IN_SAME_LOBBY);
                    });
                     return;
                }
                target.Lobby.Chat.Send(lang => string.Format(lang.KICK_LOBBY_INFO, target.DisplayName, player.DisplayName, reason));
            }
            await target.Lobby.Players.RemovePlayer(target).ConfigureAwait(false);
            await _lobbiesHandler.MainMenu.Players.AddPlayer(target, 0).ConfigureAwait(false);
        }

        [TDSCommand(AdminCommand.Mute, 1)]
        public void MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            target.MuteHandler.ChangeMuteTime(target, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Mute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.Mute, 0)]
        public async Task MutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerMuteTime(player, dbTarget, minutes, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Mute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.NextMap)]
        public void NextMap(ITDSPlayer player, TDSCommandInfos cmdinfos, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.Next, player, reason, asdonator: cmdinfos.AsDonator, asvip: cmdinfos.AsVIP);

            arena.Rounds.RoundStates.EndRound(new CommandRoundEndReason(player));
        }

        [TDSCommand(AdminCommand.Test)]
        public void Test(ITDSPlayer player, string type, string? arg1 = null, string? arg2 = null, string? arg3 = null)
        {
            switch (type)
            {
                case "cloth":
                    if (player is null)
                        return;

                    if (!int.TryParse(arg1, out int slot))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }
                    if (!int.TryParse(arg2, out int drawable))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }
                    if (!int.TryParse(arg3, out int texture))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_USED_WRONG, true));
                        return;
                    }

                    NAPI.Task.RunSafe(() => player.SetClothes(slot, drawable, texture));
                    break;

                case "admin":
                    if (!short.TryParse(arg1, out short adminLvl))
                    {
                        NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true));
                        return;
                    }
                    player.Entity!.AdminLvl = adminLvl;
                    NAPI.Task.RunSafe(() => _dataSyncHandler.SetData(player, PlayerDataKey.AdminLevel, DataSyncMode.All, adminLvl));
                    break;

                default:
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.COMMAND_DOESNT_EXIST, true));
                    break;
            }
        }

        [TDSCommand(AdminCommand.VoiceMute, 0)]
        public async Task VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, Players dbTarget, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            await _databasePlayerHelper.ChangePlayerVoiceMuteTime(player, dbTarget, minutes, reason).ConfigureAwait(false);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.VoiceMute, player, reason, dbTarget.Id, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.VoiceMute, 1)]
        public void VoiceMutePlayer(ITDSPlayer player, TDSCommandInfos cmdinfos, ITDSPlayer target, int minutes, [TDSRemainingText(MinLength = 4)] string reason)
        {
            if (!IsMuteTimeValid(minutes, player))
                return;

            target.MuteHandler.ChangeVoiceMuteTime(player, minutes, reason);

            if (!cmdinfos.AsLobbyOwner)
                _loggingHandler.LogAdmin(LogType.VoiceMute, player, target, reason, cmdinfos.AsDonator, cmdinfos.AsVIP);
        }

        [TDSCommand(AdminCommand.CreateHouse)]
        public async Task CreateHouse(ITDSPlayer player, byte neededGangLevel)
        {
            if (player is null || player.Entity is null)
                return;

            if (!(player.Lobby is IGangLobby))
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ONLY_ALLOWED_IN_GANG_LOBBY));
                return;
            }

            if (neededGangLevel > _gangLevelsHandler.HighestLevel)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.GANG_LEVEL_MAX_ALLOWED, _gangLevelsHandler.HighestLevel)));
                return;
            }

            await _gangHousesHandler.AddHouse(player.Position, player.Rotation.Z, neededGangLevel, player.Entity.Id).ConfigureAwait(false);
            NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ADDED_THE_GANG_HOUSE_SUCCESSFULLY));
        }

        private bool IsMuteTimeValid(int muteTime, ITDSPlayer outputTo)
        {
            if (muteTime < -1)
            {
                NAPI.Task.RunSafe(() => outputTo.SendChatMessage(outputTo.Language.MUTETIME_INVALID));
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
