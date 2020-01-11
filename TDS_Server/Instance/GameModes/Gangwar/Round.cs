﻿using TDS_Server.Instance.Player;
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

            LangUtils.SendAllNotification(lang => lang.GANGWAR_PREPARATION_INFO);
            _gangwarArea.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));

            _gangwarArea.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));

            _gangwarArea.Attacker.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, onAccept: AcceptAttackPreparationInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });
        }

        public override void StartRound()
        {
            base.StartRound();

            Lobby.LobbyEntity.Name = $"[GW] {Lobby.GangwarArea!.Attacker!.Entity.Short} - {Lobby.GangwarArea.Owner!.Entity.Short}";

            LangUtils.SendAllNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInAttack();

            _gangwarArea.Attacker!.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, onAccept: AcceptAttackInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });

            _gangwarArea.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));
            _gangwarArea.Owner!.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, onAccept: AcceptDefendInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });
        }

        public override void StartMapClear()
        {
            base.StartMapClear();

            ClearMapFromTarget();
        }
    }
}
