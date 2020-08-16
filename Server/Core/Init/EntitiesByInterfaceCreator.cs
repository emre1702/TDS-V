using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Entity.Blip;
using TDS_Server.Entity.ColShape;
using TDS_Server.Entity.Gamemodes.Gangwar;
using TDS_Server.Entity.GangSystem;
using TDS_Server.Entity.LobbySystem.ArenaSystem;
using TDS_Server.Entity.LobbySystem.BaseSystem;
using TDS_Server.Entity.LobbySystem.CharCreateLobbySystem;
using TDS_Server.Entity.LobbySystem.FightLobbySystem;
using TDS_Server.Entity.LobbySystem.GangLobbySystem;
using TDS_Server.Entity.LobbySystem.MapCreateLobbySystem;
using TDS_Server.Entity.Player;
using TDS_Server.Entity.TeamSystem;
using TDS_Server.Entity.TextLabel;
using TDS_Server.Entity.Vehicle;
using TDS_Server.Entity.VoiceChannel;

namespace TDS_Server.Core.Init
{
    public class EntitiesByInterfaceCreator : IEntitiesByInterfaceCreator
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<Type, Type> _classByInterface = new Dictionary<Type, Type>
        {
            [typeof(IArena)] = typeof(Arena),
            [typeof(ICharCreateLobby)] = typeof(CharCreateLobby),
            [typeof(IMapCreateLobby)] = typeof(MapCreateLobby),
            [typeof(IGangLobby)] = typeof(GangLobby),
            [typeof(IFightLobby)] = typeof(FightLobby),
            [typeof(ILobby)] = typeof(Lobby),
            [typeof(ITDSPlayer)] = typeof(TDSPlayer),
            [typeof(ITDSVehicle)] = typeof(TDSVehicle),
            [typeof(ITDSBlip)] = typeof(TDSBlip),
            [typeof(ITDSColShape)] = typeof(TDSColShape),
            [typeof(ITDSVoiceChannel)] = typeof(TDSVoiceChannel),
            [typeof(ITDSTextLabel)] = typeof(TDSTextLabel),
            [typeof(IGangHouse)] = typeof(GangHouse),
            [typeof(IGang)] = typeof(Gang),
            [typeof(IGangwarArea)] = typeof(GangwarArea),
            [typeof(IGangwar)] = typeof(Gangwar),
            [typeof(ITeam)] = typeof(Team),
            [typeof(ITDSVoiceChannel)] = typeof(TDSVoiceChannel),

        };

        #endregion Fields

        #region Constructors

        public EntitiesByInterfaceCreator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public TInterface Create<TInterface>(params object[] parameters)
            where TInterface : class
        {
            var type = typeof(TInterface);
            if (!_classByInterface.TryGetValue(type, out Type? classType))
            {
                Console.WriteLine($"Missing class type for {type.FullName}.", Environment.StackTrace, "Custom");
                return default!;
            }

            return (TInterface)ActivatorUtilities.CreateInstance(_serviceProvider, classType, parameters);
        }

        #endregion Methods
    }
}
