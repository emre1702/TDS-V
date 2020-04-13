using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    class EventAPI : IEventAPI
    {
        public ICollection<EventMethodData<DeathDelegate>> Death { get; }
        public ICollection<EventMethodData<EntityStreamInDelegate>> EntityStreamIn { get; }
        public ICollection<EventMethodData<TickDelegate>> Tick { get; }
        public ICollection<EventMethodData<TickNametagDelegate>> TickNametag { get; }
        public ICollection<EventMethodData<WeaponShotDelegate>> WeaponShot { get; }

        public ICollection<EventMethodData<IncomingDamageDelegate>> IncomingDamage { get; }

        public ICollection<EventMethodData<OutgoingDamageDelegate>> OutgoingDamage { get; }

        public ICollection<EventMethodData<SpawnDelegate>> Spawn { get; }

        public ICollection<EventMethodData<PlayerDelegate>> PlayerStartTalking { get; }

        public ICollection<EventMethodData<PlayerDelegate>> PlayerStopTalking { get; }

        public EventAPI(PlayerConvertingHandler playerConvertingHandler, EntityConvertingHandler entityConvertingHandler)
        {
            Death = new DeathEventHandler(playerConvertingHandler);
            EntityStreamIn = new EntityStreamInEventHandler(entityConvertingHandler);
            IncomingDamage = new IncomingDamageEventHandler(entityConvertingHandler);
            OutgoingDamage = new OutgoingDamageEventHandler(entityConvertingHandler);
            Spawn = new PlayerSpawnEventHandler();
            PlayerStartTalking = new PlayerStartTalkingEventHandler(playerConvertingHandler);
            PlayerStopTalking = new PlayerStopTalkingEventHandler(playerConvertingHandler);
            Tick = new TickEventHandler();
            TickNametag = new TickNametagEventHandler(playerConvertingHandler);
            WeaponShot = new WeaponShotHandler(playerConvertingHandler);
        }

        public void Add(string eventName, ObjectArgsDelegate method)
        {
            RAGE.Events.Add(eventName, new RAGE.Events.CallDelegate(method));
        }
    }
}
