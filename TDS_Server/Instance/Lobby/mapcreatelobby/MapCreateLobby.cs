using System.Collections.Generic;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby : Lobby
    {
        public MapCreateLobby(Lobbies entity) : base(entity)
        {

        }

        public static async void Create(TDSPlayer player)
        {
            if (player.Entity == null)
                return;

            Lobbies entity = new Lobbies
            {
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Client.Name } },
                Type = (short) ELobbyType.MapCreateLobby,
                Owner = player.Entity.Id,
                OwnerNavigation = player.Entity
            };
            MapCreateLobby lobby = new MapCreateLobby(entity);
            await lobby.AddPlayer(player, 0);
        }
    }
}