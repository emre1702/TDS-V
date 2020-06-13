using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    internal class EventAPI : IEventAPI
    {
        #region Public Constructors

        public EventAPI(LoggingHandler loggingHandler)
        {
            Death = new DeathEventHandler(loggingHandler);
            EntityStreamIn = new EntityStreamInEventHandler(loggingHandler);
            EntityStreamOut = new EntityStreamOutEventHandler(loggingHandler);
            IncomingDamage = new IncomingDamageEventHandler(loggingHandler);
            OutgoingDamage = new OutgoingDamageEventHandler(loggingHandler);
            Spawn = new PlayerSpawnEventHandler(loggingHandler);
            PlayerEnterCheckpoint = new PlayerEnterCheckpointEventHandler(loggingHandler);
            PlayerExitCheckpoint = new PlayerExitCheckpointEventHandler(loggingHandler);
            PlayerStartEnterVehicle = new PlayerStartEnterVehicleEventHandler(loggingHandler);
            PlayerStartTalking = new PlayerStartTalkingEventHandler(loggingHandler);
            PlayerStopTalking = new PlayerStopTalkingEventHandler(loggingHandler);
            Tick = new TickEventHandler(loggingHandler);
            TickNametag = new TickNametagEventHandler(loggingHandler);
            WeaponShot = new WeaponShotHandler(loggingHandler);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICollection<EventMethodData<DeathDelegate>> Death { get; }
        public ICollection<EventMethodData<EntityStreamInDelegate>> EntityStreamIn { get; }
        public ICollection<EventMethodData<EntityStreamOutDelegate>> EntityStreamOut { get; }
        public ICollection<EventMethodData<IncomingDamageDelegate>> IncomingDamage { get; }
        public ICollection<EventMethodData<OutgoingDamageDelegate>> OutgoingDamage { get; }
        public ICollection<EventMethodData<PlayerEnterCheckpointDelegate>> PlayerEnterCheckpoint { get; }
        public ICollection<EventMethodData<PlayerExitCheckpointDelegate>> PlayerExitCheckpoint { get; }
        public ICollection<EventMethodData<PlayerStartEnterVehicleDelegate>> PlayerStartEnterVehicle { get; }
        public ICollection<EventMethodData<PlayerDelegate>> PlayerStartTalking { get; }
        public ICollection<EventMethodData<PlayerDelegate>> PlayerStopTalking { get; }
        public ICollection<EventMethodData<SpawnDelegate>> Spawn { get; }
        public ICollection<EventMethodData<TickDelegate>> Tick { get; }
        public ICollection<EventMethodData<TickNametagDelegate>> TickNametag { get; }
        public ICollection<EventMethodData<WeaponShotDelegate>> WeaponShot { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(string eventName, ObjectArgsDelegate method)
        {
            RAGE.Events.Add(eventName, new RAGE.Events.CallDelegate(method));
        }

        public void CallLocal(string eventName, params object[] args)
        {
            RAGE.Events.CallLocal(eventName, args);
        }

        #endregion Public Methods
    }
}
