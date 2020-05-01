using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Ped;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.RAGEAPI
{
    internal class EntityConvertingHandler
    {

        private readonly Dictionary<GTANetworkAPI.Player, IPlayer> _playerCache = new Dictionary<GTANetworkAPI.Player, IPlayer>();
        private readonly Dictionary<GTANetworkAPI.Entity, IEntity> _entityCache = new Dictionary<GTANetworkAPI.Entity, IEntity>();


        internal IPlayer? GetEntity(GTANetworkAPI.Player modPlayer)
        {
            if (_playerCache.TryGetValue(modPlayer, out IPlayer? player))
                return player;
            return null;
        }

        internal IPed GetEntity(GTANetworkAPI.Ped modPed)
        {
            if (!_entityCache.TryGetValue(modPed, out IEntity? ped))
            {
                ped = new Ped.Ped(modPed);
                _entityCache[modPed] = ped;
            }
            return (IPed)ped;
        }

        internal IVehicle? GetEntity(GTANetworkAPI.Vehicle modVehicle)
        {
            if (modVehicle is null)
                return null;
            if (!_entityCache.TryGetValue(modVehicle, out IEntity? vehicle))
            {
                vehicle = new Vehicle.Vehicle(modVehicle, this);
                _entityCache[modVehicle] = vehicle;
            }
            return (IVehicle)vehicle;
        }

        internal IEntity GetEntity(GTANetworkAPI.Entity modEntity)
        {
            if (!_entityCache.TryGetValue(modEntity, out IEntity? entity))
            {
                entity = new Entity.Entity(modEntity);
                _entityCache[modEntity] = entity;
            }
            return entity;
        }

        internal void PlayerConnected(GTANetworkAPI.Player modPlayer)
        {
            var player = new Player.Player(modPlayer, this);
            _playerCache[modPlayer] = player;
        }

        internal void PlayerDisconnected(GTANetworkAPI.Player modPlayer)
        {
            _playerCache.Remove(modPlayer);
        }

        internal IPlayer? GetPlayerByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (var entry in _playerCache.Keys)
            {
                if (entry.Name.Equals(name, System.StringComparison.CurrentCultureIgnoreCase))
                    return _playerCache[entry];
            }

            return null;
        }
    }
}
