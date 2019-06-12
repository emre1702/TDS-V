using System.Collections.Generic;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby : Lobby
    {
        public MapCreateLobby(Lobbies entity) : base(entity) {}

        public static async void Create(TDSPlayer player)
        {
            if (player.Entity == null)
                return;

            using var dbContext = new TDSNewContext();
            Lobbies entity = new Lobbies
            {
                Name = "MapCreator-" + player.Client.Name,  // Todo after custom lobbies: They can't be named "MapCreator-*"
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Client.Name, ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = (short) ELobbyType.MapCreateLobby,
                Owner = player.Entity.Id,
                IsTemporary = true
            };
            await dbContext.Lobbies.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            MapCreateLobby lobby = new MapCreateLobby(entity);
            await lobby.AddPlayer(player, 0);
        }
    }
}