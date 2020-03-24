using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.GameModes;
using TDS_Server.Handler.Entities.GangTeam;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Instance;

namespace TDS_Server.Handler.Entities.Utility
{
    public class GangwarArea : DatabaseEntityWrapper
    {
        public GangwarAreas? Entity { get; private set; }
        public MapDto Map { get; private set; }
        public IGang? Owner { get; private set; }
        public IGang? Attacker { get; private set; }
        public Arena? InLobby { get; set; }

        private ITeam? OwnerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.OwnerTeam : null;
        private ITeam? AttackerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.AttackerTeam : null;

        // public delegate void Gangwar

        public bool HasCooldown
        {
            get => Entity is null ? false : (DateTime.UtcNow - Entity.LastAttacked).TotalMinutes < _settingsHandler.ServerSettings.GangwarAreaAttackCooldownMinutes;
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

        private TDSTimer? _checkAtTarget;
        private int _playerNotAtTargetCounter;
        private readonly SettingsHandler _settingsHandler;
        private readonly GangsHandler _gangsHandler;

        public GangwarArea(GangwarArea copyFrom, SettingsHandler settingsHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : this(copyFrom.Map, settingsHandler, gangsHandler, dbContext, loggingHandler)
        {
            Entity = null;
        }

        public GangwarArea(MapDto map, SettingsHandler settingsHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            Map = map;
            _settingsHandler = settingsHandler;
            _gangsHandler = gangsHandler;
        }

        public GangwarArea(GangwarAreas entity, MapDto map, SettingsHandler settingsHandler, GangsHandler gangsHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : this(map, settingsHandler, gangsHandler, dbContext, loggingHandler)
        {
            Entity = entity;

            if (entity.OwnerGangId != gangsHandler.None.Entity.Id)
            {
                Owner = _gangsHandler.GetById(entity.OwnerGangId);
            }
        }

        public void SetInPreparation(IGang attackerGang)
        {
            Attacker = attackerGang;
            Attacker.InAction = true;
            Owner!.InAction = true;
        }

        public void SetInAttack()
        {
            _playerNotAtTargetCounter = 0;
            //Todo: Use this on gangwar start if no one is at target OR on colshape leave
            _checkAtTarget = new TDSTimer(OutputAtTargetInfo, 1000, 0);
        }

        public async Task SetAttackEnded(bool conquered)
        {
            if (Entity is { })
            {
                await ExecuteForDBAsync(async dbContext =>
                {
                    dbContext.Attach(Entity);

                    ++Entity.AttackCount;
                    HasCooldown = true;

                    if (conquered)
                        SetConquered(dbContext, true);
                    else
                        SetDefended(dbContext);

                    await dbContext.SaveChangesAsync();
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
                    dbContext.Attach(Entity);

                    HasCooldown = true;

                    SetConquered(dbContext, false);

                    await dbContext.SaveChangesAsync();
                });

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

        private void SetConquered(TDSDbContext dbContext, bool outputInfos)
        {
            if (Entity is { })
            {
                var oldOwner = Owner;

                Owner = Attacker;

                Entity.OwnerGangId = Owner!.Entity.Id;
                Entity.DefendCount = 0;

                if (outputInfos)
                {
                    if (Owner is { })
                    {
                        //Todo: Inform the owner
                    }
                    //Todo: Inform everyone + attacker
                }
            }

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
                 //   player.SendMessage(player.Language.GANGWAR_LOST_);
                //});
            var lobby = InLobby;
            lobby.SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.TargetEmpty);*/
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

        private bool IsAtTarget(TDSPlayer player)
        {
            if (player.ModPlayer is null || player.ModPlayer.Dead)
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
    }
}

