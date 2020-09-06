﻿using RAGE.Elements;
using RAGE.Ui;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler.Deathmatch
{
    public class DamageHandler
    {
        private readonly BrowserHandler _browserHandler;
        private readonly LobbyHandler _lobbyHandler;

        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public DamageHandler(BrowserHandler browserHandler, RemoteEventsSender remoteEventsSender, PlayerFightHandler playerFightHandler,
            LobbyHandler lobbyHandler, EventsHandler eventsHandler)
        {
            _browserHandler = browserHandler;
            _remoteEventsSender = remoteEventsSender;
            _playerFightHandler = playerFightHandler;
            _lobbyHandler = lobbyHandler;

            eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        private void OnIncomingDamageMethod(Player sourcePlayer, Entity sourceEntity, Entity targetEntity, ulong weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            RAGE.Ui.Console.Log(ConsoleVerbosity.Info, $"Incoming damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type}, targetEntity {targetEntity.Type} - {targetEntity is ITDSPlayer}", true);

            if (sourcePlayer is null)
            {
                _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.GotHit, damage);
                _browserHandler.PlainMain.ShowBloodscreen();
            }
            else
            {
                cancel.Cancel = true;

                if (!_lobbyHandler.Teams.IsInSameTeam(sourcePlayer as ITDSPlayer))
                    _browserHandler.PlainMain.ShowBloodscreen();
            }
        }

        private void OnOutgoingDamageMethod(Entity sourceEntity, Entity targetEntity, Player sourcePlayer, ulong weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            RAGE.Ui.Console.Log(ConsoleVerbosity.Info, $"Outgoing damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type}, targetEntity {targetEntity.Type} - {(targetEntity is ITDSPlayer targetPlayer ? targetPlayer.Name : "Not player")}", true);

            if (sourcePlayer is null)
                return;
            if (sourcePlayer == Player.LocalPlayer)
                return;

            if (_lobbyHandler.Teams.IsInSameTeam(sourcePlayer as ITDSPlayer))
            {
                cancel.Cancel = true;
                return;
            }

            var bodyPart = SharedUtils.GetPedBodyPart((int)boneIdx);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.HitOtherPlayer, (int)sourcePlayer.RemoteId, (long)weaponHash, (int)bodyPart);
            _playerFightHandler.HittedOpponent();
        }

        private void EventsHandler_InFightStatusChanged(bool boolean)
        {
            if (boolean)
            {
                OnIncomingDamage += OnIncomingDamageMethod;
                OnOutgoingDamage += OnOutgoingDamageMethod;
            }
            else
            {
                OnIncomingDamage -= OnIncomingDamageMethod;
                OnOutgoingDamage -= OnOutgoingDamageMethod;
            }
        }
    }
}
