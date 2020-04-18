using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Shared.Data.Enums;

namespace TDS_Server.RAGEAPI.Chat
{
    class ChatAPI : IChatAPI
    {
        public ChatAPI()
        {

        }

        public void SendMessage(string message, ITDSPlayer? source = null)
        {
            foreach (var player in Init.TDSCore.LoggedInPlayers)
            {
                if (player.ModPlayer is Player.Player modPlayer && (source is null || !player.HasRelationTo(source, PlayerRelation.Block)))
                    modPlayer._instance.SendChatMessage(message);
            }
        }

        public void SendMessage(ILobby lobby, string message, ITDSPlayer? source = null)
        {
            foreach (var player in lobby.Players.Values)
            {
                if (player.ModPlayer is Player.Player modPlayer && (source is null || !player.HasRelationTo(source, PlayerRelation.Block)))
                    modPlayer._instance.SendChatMessage(message);
            }
        }

        public void SendMessage(ITeam lobby, string message, ITDSPlayer? source = null)
        {
            foreach (var player in lobby.Players)
            {
                if (player.ModPlayer is Player.Player modPlayer && (source is null || !player.HasRelationTo(source, PlayerRelation.Block)))
                    modPlayer._instance.SendChatMessage(message);
            }
        }
    }
}
