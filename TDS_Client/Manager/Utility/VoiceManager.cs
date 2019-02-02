using RAGE;
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

            BindManager.Add(ConsoleKey.C, Start, EKeyPressState.Down);
            BindManager.Add(ConsoleKey.C, Stop, EKeyPressState.Up);
        }

        public static void AddPlayer(Player player)
        {
            sendingToPlayers.Add(player);
            player.AutoVolume = true;
            player.Voice3d = voice3d;
            Events.CallRemote(DToServerEvent.VoiceToAdd, player);
        }

        public static void RemovePlayer(Player player)
        {
            sendingToPlayers.Remove(player);
            Events.CallRemote(DToServerEvent.VoiceToRemove, player);
        }

        private static void Start(ConsoleKey _)
        {
            Voice.Muted = false;
        }

        private static void Stop(ConsoleKey _)
        {
            Voice.Muted = true;
        }
    }
}
