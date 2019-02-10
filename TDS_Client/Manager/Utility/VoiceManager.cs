using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Client.Manager.Lobby;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    class VoiceManager
    {
        private const bool voice3d = true;

        private static readonly HashSet<Player> sendingToPlayers = new HashSet<Player>();

        public static void Init()
        {
            //if (!Voice.Allowed)
            //    return;

            BindManager.Add(Control.PushToTalk, Start, EKeyPressState.Down);
            BindManager.Add(Control.PushToTalk, Stop, EKeyPressState.Up);
        }

        public static void AddPlayer(Player player)
        {
            sendingToPlayers.Add(player);
            player.AutoVolume = true;
            player.Voice3d = voice3d;
            EventsSender.Send(DToServerEvent.VoiceToAdd, player);
        }

        public static void RemovePlayer(Player player)
        {
            sendingToPlayers.Remove(player);
            EventsSender.Send(DToServerEvent.VoiceToRemove, player);
        }

        private static void Start(Control _)
        {
            Voice.Muted = false;
        }

        private static void Stop(Control _)
        {
            Voice.Muted = true;
        }
    }
}
