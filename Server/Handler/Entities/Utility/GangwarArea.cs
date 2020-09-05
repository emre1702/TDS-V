using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.Gamemodes;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.GangSystem;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.Utility
{
    public class GangwarArea : DatabaseEntityWrapper
    {
        private readonly GangsHandler _gangsHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly LobbiesHandler? _lobbiesHandler;
        private TDSTimer? _checkAtTarget;
        private int _playerNotAtTargetCounter;
        private ITDSBlip? _blip;

        public GangwarArea(GangwarArea copyFrom, ISettingsHandler settingsHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : this(copyFrom.Map, settingsHandler, gangsHandler, dbContext, loggingHandler)
        {
            Entity = null;
        }

        public GangwarArea(MapDto map, ISettingsHandler settingsHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            Map = map;

            _settingsHandler = settingsHandler;
            _gangsHandler = gangsHandler;
        }

        [ActivatorUtilitiesConstructor]
        public GangwarArea(GangwarAreas entity, MapDto map, ISettingsHandler settingsHandler, GangsHandler gangsHandler,
            TDSDbContext dbContext, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler)
            : this(map, settingsHandler, gangsHandler, dbContext, loggingHandler)
        {
            Entity = entity;
            _lobbiesHandler = lobbiesHandler;

            if (entity.OwnerGangId != gangsHandler.None.Entity.Id)
            {
                Owner = _gangsHandler.GetById(entity.OwnerGangId);
            }

            dbContext.Attach(entity);

            CreateGangLobbyMapInfo();
        }

        public IGang? Attacker { get; private set; }
        public GangwarAreas? Entity { get; private set; }

        public bool HasCooldown
        {
            get => !(Entity is null) && (DateTime.UtcNow - Entity.LastAttacked).TotalMinutes < _settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes;
            set
            {
                if (Entity is null)
                    return;
                if (value)
                    Entity.LastAttacked = DateTime.UtcNow;
                else
                    Entity.LastAttacked = DateTime.UtcNow.AddMinutes(-_settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes);
            }
        }

        public Arena? InLobby { get; set; }
        public MapDto Map { get; private set; }
        public IGang? Owner { get; private set; }

        private ITeam? AttackerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.AttackerTeam : null;
        private ITeam? OwnerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.OwnerTeam : null;

        public async Task SetAttackEnded(bool conquered)
        {
            if (Entity is { })
            {
                await ExecuteForDBAsync(async dbContext =>
                {
                    ++Entity.AttackCount;
                    HasCooldown = true;

                    if (conquered)
                        SetConquered(dbContext);
                    else
                        SetDefended(dbContext);

                    await dbContext.SaveChangesAsync();
                });

                NAPI.Task.Run(() =>
                {
                    CreateGangLobbyMapInfo();

                    //Todo: Inform the owner
                    //Todo: Inform everyone + attacker
                });
            }
            ClearAttack();
        }

        public async Task? SetConqueredWithoutAttack(IGang newOwner)
        {
            if (Entity is { })
            {
                Attacker = newOwner;

                await ExecuteForDBAsync(async dbContext =>
                {
                    HasCooldown = true;

                    SetConquered(dbContext);

                    await dbContext.SaveChangesAsync();
                });

                NAPI.Task.Run(() =>
                {
                    //Todo: Inform everyone + attacker
                });
            }
        }

        public void SetInAttack()
        {
            _playerNotAtTargetCounter = 0;
            //Todo: Use this on gangwar start if no one is at target OR on colshape leave
            _checkAtTarget = new TDSTimer(OutputAtTargetInfo, 1000, 0);
        }

        // public delegate void Gangwar
        public void SetInPreparation(IGang attackerGang)
        {
            Attacker = attackerGang;
            Attacker.InAction = true;
            Owner!.InAction = true;
        }

        private void ClearAttack()
        {
            if (Attacker is { })
            {
                Attacker!.InAction = false;
                Attacker = null;
            }
            if (Owner is { })
            {
                Owner!.InAction = false;
            }

            _checkAtTarget?.Kill();
            _checkAtTarget = null;
        }

        private bool IsAtTarget(ITDSPlayer player)
        {
            if (player.Dead)
                return false;
            if (!player.LoggedIn)
                return false;
            if (player.Lobby is null)
                return false;
            if (!(player.Lobby is Arena arena))
                return false;
            if (arena.GangwarArea != this)
                return false;

            return true;
        }

        private void OutputAtTargetInfo()
        {
            if (InLobby is null)
                return;

            if (!(InLobby.CurrentGameMode is Gangwar gangwar))
                return;

            if (gangwar.TargetObject is null)
                return;

            if (++_playerNotAtTargetCounter < _settingsHandler.ServerSettings.GangwarTargetWithoutAttackerMaxSeconds)
            {
                //Todo: Output to owner that they have xx seconds left to go back to target
            }

            /*else
             //Todo: Output to owner that no one was at target for too long
                //OwnerTeamInGangwar?.FuncIterate((player, team) =>
                //{
                 //   player.SendChatMessage(player.Language.GANGWAR_LOST_);
                //});
            var lobby = InLobby;
            lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.TargetEmpty);*/
        }

        private void SetConquered(TDSDbContext dbContext)
        {
            if (Entity is { })
            {
                var oldOwner = Owner;

                Owner = Attacker;

                Entity.OwnerGangId = Owner!.Entity.Id;
                Entity.DefendCount = 0;
            }
        }

        private void SetDefended(TDSDbContext dbContext)
        {
            if (Entity is { })
            {
                ++Entity.DefendCount;

                //Todo: Inform everyone + owner + attacker
            }
        }

        public void CreateGangLobbyMapInfo()
        {
            if (_blip is { })
                _blip.Delete();
            if (_lobbiesHandler is null)
                return;

            _blip = NAPI.Blip.CreateBlip(
                sprite: 84,
                position: Map.Target!.ToVector3(),
                scale: 3f,
                color: Owner?.Entity.BlipColor ?? 4,
                name: Map.Info.Name,
                alpha: (byte)(HasCooldown ? 120 : 255),
                drawDistance: 100f,
                shortRange: true,
                dimension: _lobbiesHandler.GangLobby.Dimension) as ITDSBlip;
        }
    }
}
