﻿using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        public override void StartMapChoose()
        {
            base.StartMapChoose();

            CreateTargetBlip();
            CreateTargetObject();
        }

        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();

            if (Lobby.IsGangActionLobby)
            {
                LangUtils.SendAllNotification(lang => lang.GANGWAR_PREPARATION_INFO);
                _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Map.SyncedData.Name, OwnerTeam.Entity.Name));

                _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Map.SyncedData.Name, AttackerTeam.Entity.Name));

                _gangwarArea?.Attacker.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, onAccept: AcceptAttackPreparationInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });
            }
           
            // Do we need to force someone to stay at target?
            // If yes, force him! Kill him if he don't want to stay there!
            else if (TargetObject is { })
            {
                var playerAtTarget = GetNextTargetMan();
                SetTargetMan(playerAtTarget);
            }
        }

        public override void StartRound()
        {
            base.StartRound();

            if (Lobby.IsGangActionLobby)
            {
                Lobby.LobbyEntity.Name = $"[GW] {Lobby.GangwarArea!.Attacker!.Entity.Short} - {Lobby.GangwarArea.Owner!.Entity.Short}";

                LangUtils.SendAllNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea?.Map.SyncedData.Name ?? "?", OwnerTeam.Entity.Name));
                _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Map.SyncedData.Name, OwnerTeam.Entity.Name));

                _gangwarArea?.Attacker!.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, onAccept: AcceptAttackInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });

                _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Map.SyncedData.Name, AttackerTeam.Entity.Name));
                _gangwarArea?.Owner!.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, onAccept: AcceptDefendInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });
            }            
        }

        public override void StopRound()
        {
            base.StopRound();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _gangwarArea?.SetAttackEnded(IsConquered);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (Lobby.IsGangActionLobby && Lobby.RoundEndReasonText is { })
            {
                Lobby.SendAllPlayerLangMessage(Lobby.RoundEndReasonText);
            }

            SetTargetMan(null);
        }

        public override void StartMapClear()
        {
            base.StartMapClear();

            ClearMapFromTarget();
        }

        public override bool CanEndRound(ERoundEndReason endReason)
        {
            return endReason switch
            {
                ERoundEndReason.BombDefused => false,
                ERoundEndReason.BombExploded => false,
                ERoundEndReason.Death => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                ERoundEndReason.Empty => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                ERoundEndReason.NewPlayer => !Lobby.IsGangActionLobby,

                ERoundEndReason.TargetEmpty => true,
                ERoundEndReason.Time => true,
                ERoundEndReason.Command => true,  

                _ => true
            };
        }


        private bool IsConquered => 
            Lobby.CurrentRoundEndReason switch
            {
                ERoundEndReason.Time => true,

                ERoundEndReason.Command => false,
                ERoundEndReason.TargetEmpty => false,
                ERoundEndReason.Empty => false,

                ERoundEndReason.Death when (Lobby.CurrentGameMode is Gangwar gangwar) => gangwar.AttackerTeam.AlivePlayers!.Count > 0,

                _ => false 

            };
    }
}