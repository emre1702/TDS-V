using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby : Lobby
    {
        public MapCreateLobby(Lobbies entity) : base(entity) {}

        public static async void Create(TDSPlayer player)
        {
            if (player.Entity is null)
                return;

            Lobbies entity = new Lobbies
            {
                Name = "MapCreator-" + player.Client.Name,
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Client.Name, ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = ELobbyType.MapCreateLobby,
                OwnerId = player.Entity.Id,
                IsTemporary = true,
                DefaultSpawnX = -365.425f,
                DefaultSpawnY = -131.809f,
                DefaultSpawnZ = 37.873f,
                DefaultSpawnRotation = 0f
            };
            MapCreateLobby lobby = new MapCreateLobby(entity);
            await lobby.ExecuteForDBAsync(async (dbContext) => 
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();
            });
           
            await lobby.AddPlayer(player, 0);

            LobbyManager.AddLobby(lobby);
        }
    }
}