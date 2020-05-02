using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    class EventAPI : IEventAPI
    {
        public ICollection<EventMethodData<DeathDelegate>> Death { get; }
        public ICollection<EventMethodData<EntityStreamInDelegate>> EntityStreamIn { get; }
        public ICollection<EventMethodData<EntityStreamOutDelegate>> EntityStreamOut { get; }
        public ICollection<EventMethodData<TickDelegate>> Tick { get; }
        public ICollection<EventMethodData<TickNametagDelegate>> TickNametag { get; }
        public ICollection<EventMethodData<WeaponShotDelegate>> WeaponShot { get; }

        public ICollection<EventMethodData<IncomingDamageDelegate>> IncomingDamage { get; }

        public ICollection<EventMethodData<OutgoingDamageDelegate>> OutgoingDamage { get; }

        public ICollection<EventMethodData<SpawnDelegate>> Spawn { get; }
        public ICollection<EventMethodData<PlayerStartEnterVehicleDelegate>> PlayerStartEnterVehicle { get; }

        public ICollection<EventMethodData<PlayerDelegate>> PlayerStartTalking { get; }

        public ICollection<EventMethodData<PlayerDelegate>> PlayerStopTalking { get; }

        public EventAPI(PlayerConvertingHandler playerConvertingHandler, EntityConvertingHandler entityConvertingHandler, LoggingHandler loggingHandler)
        {
            Death = new DeathEventHandler(loggingHandler, playerConvertingHandler);
            EntityStreamIn = new EntityStreamInEventHandler(loggingHandler, entityConvertingHandler);
            EntityStreamOut = new EntityStreamOutEventHandler(loggingHandler, entityConvertingHandler);
            IncomingDamage = new IncomingDamageEventHandler(loggingHandler, entityConvertingHandler);
            OutgoingDamage = new OutgoingDamageEventHandler(loggingHandler, entityConvertingHandler);
            Spawn = new PlayerSpawnEventHandler(loggingHandler);
            PlayerStartEnterVehicle = new PlayerStartEnterVehicleEventHandler(loggingHandler, entityConvertingHandler);
            PlayerStartTalking = new PlayerStartTalkingEventHandler(loggingHandler, playerConvertingHandler);
            PlayerStopTalking = new PlayerStopTalkingEventHandler(loggingHandler, playerConvertingHandler);
            Tick = new TickEventHandler(loggingHandler);
            TickNametag = new TickNametagEventHandler(loggingHandler, playerConvertingHandler);
            WeaponShot = new WeaponShotHandler(loggingHandler, playerConvertingHandler);
        }

        public void Add(string eventName, ObjectArgsDelegate method)
        {
            RAGE.Events.Add(eventName, new RAGE.Events.CallDelegate(method));
        }

        public void CallLocal(string eventName, params object[] args)
        {
            RAGE.Events.CallLocal(eventName, args);
        }
    }
}
