using RAGE;
using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    internal class VoiceManager
    {
        private static readonly HashSet<Player> _sendingToPlayers = new HashSet<Player>();

        public static void Init()
        {
            //if (!Voice.Allowed)
            //    return;
            BindManager.Add(Control.PushToTalk, Start, EKeyPressState.Down);
            BindManager.Add(Control.PushToTalk, Stop, EKeyPressState.Up);
        }

        public static void AddPlayer(Player player)
        {
            _sendingToPlayers.Add(player);
            player.AutoVolume = true;
            player.Voice3d = Constants.Voice3D;
            EventsSender.Send(DToServerEvent.VoiceToAdd, player);
        }

        public static void RemovePlayer(Player player)
        {
            _sendingToPlayers.Remove(player);
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