using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.GangEntities;

namespace TDS_Server.Instance.Utility
{
    class GangwarArea
    {
        public GangwarAreas? Entity { get; private set; }
        public MapDto Map { get; private set; }
        public Gang? Owner { get; private set; }
        public Gang? Attacker { get; private set; }
        public Arena? InLobby { get; set; }

        private Team? OwnerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.OwnerTeam : null;
        private Team? AttackerTeamInGangwar => InLobby?.CurrentGameMode is Gangwar gangwar ? gangwar.AttackerTeam : null;

        // public delegate void Gangwar

        public bool HasCooldown
        {
            get => Entity is null ? false : (DateTime.UtcNow - Entity.LastAttacked).TotalMinutes < SettingsManager.ServerSettings.GangwarAreaAttackCooldownMinutes;
            set
            {
                if (Entity is null)
                    return;
                if (value)
                    Entity.LastAttacked = DateTime.UtcNow;
                else 
                    Entity.LastAttacked = DateTime.UtcNow.AddMinutes(-SettingsManager.ServerSettings.GangwarAreaAttackCooldownMinutes);
            }
        }

        private TDSTimer? _checkAtTarget;
        private int _playerNotAtTargetCounter;

        public GangwarArea(GangwarArea copyFrom)
        {
            Entity = null;
            Map = copyFrom.Map;
        }

        public GangwarArea(GangwarAreas entity, MapDto map)
        {
            Entity = entity;
            Map = map;

            if (entity.OwnerGangId != Gang.None.Entity.Id)
            {
                Owner = Gang.GetById(entity.OwnerGangId);
            }
        }

        public void SetInPreparation(Gang attackerGang)
        {
            Attacker = attackerGang;
            Attacker.InAction = true;
            Owner!.InAction = true;
        }

        public void SetInAttack()
        {
            _playerNotAtTargetCounter = 0;
            _checkAtTarget = new TDSTimer(CheckIsAttackerAtTarget, 1000, 0);
        }

        public async Task SetAttackEnded(bool conquered)
        {
            if (Entity is { })
            {
                using var dbContext = new TDSDbContext();

                dbContext.Attach(Entity);

                ++Entity.AttackCount;
                HasCooldown = true;

                if (conquered)
                    SetConquered(dbContext, true);
                else
                    SetDefended(dbContext);

                await dbContext.SaveChangesAsync();
            }
            ClearAttack();
        }

        public Task? SetConqueredWithoutAttack(Gang newOwner)
        {
            if (Entity is { })
            {
                Attacker = newOwner;

                using var dbContext = new TDSDbContext();

                dbContext.Attach(Entity);

                HasCooldown = true;

                SetConquered(dbContext, false);

                return dbContext.SaveChangesAsync();
            }
            return null;
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

        private void CheckIsAttackerAtTarget()
        {
            if (InLobby is null)
                return;

            if (!(InLobby.CurrentGameMode is Gangwar gangwar))
                return;

            if (gangwar.TargetObject is null)
                return;

            var posToCheck = gangwar.TargetObject.Position;
            bool isAnyPlayerAtTarget = NAPI.Player.GetPlayersInRadiusOfPosition(SettingsManager.ServerSettings.GangwarTargetRadius, posToCheck)
                .Where(p => p.Dimension == InLobby.Dimension)
                .Select(p => p.GetChar())
                .Any(p => IsAtTarget(p));

            if (isAnyPlayerAtTarget && _playerNotAtTargetCounter > 0)
            {
                _playerNotAtTargetCounter = 0;
                //Todo: Output to owner that a player is at target
                return;
            }

            if (++_playerNotAtTargetCounter < SettingsManager.ServerSettings.GangwarTargetWithoutAttackerMaxSeconds)
            {
                //Todo: Output to owner that they have xx seconds left to go back to target
            }
            else
            {
                //Todo: Output to owner that no one was at target for too long
                /*OwnerTeamInGangwar?.FuncIterate((player, team) =>
                {
                    player.SendMessage(player.Language.GANGWAR_LOST_);
                });*/
                var lobby = InLobby;
                lobby.SetRoundStatus(Enums.ERoundStatus.RoundEnd, Enums.ERoundEndReason.TargetEmpty);
            }




        }

        private void ClearAttack()
        {
            Attacker!.InAction = false;
            Owner!.InAction = false;
            Attacker = null;
            _checkAtTarget?.Kill();
            _checkAtTarget = null;
        }

        private bool IsAtTarget(TDSPlayer player)
        {
            if (player.Player is null || player.Player.Dead)
                return false;
            if (!player.LoggedIn)
                return false;
            if (player.CurrentLobby is null)
                return false;
            if (!(player.CurrentLobby is Arena arena))
                return false;
            if (arena.GangwarArea != this)
                return false;
            
            return true;
        }
    }
}

