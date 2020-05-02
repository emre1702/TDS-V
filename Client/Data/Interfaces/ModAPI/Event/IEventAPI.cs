using System;
using System.Collections.Generic;
using TDS_Shared.Data.Models;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Client.Data.Interfaces.ModAPI.Event
{
    public delegate void DeathDelegate(IPlayer player, uint reason, IPlayer killer, CancelEventArgs cancel);
    public delegate void EntityStreamInDelegate(IEntity entity);
    public delegate void EntityStreamOutDelegate(IEntity entity);
    public delegate void IncomingDamageDelegate(IPlayer sourcePlayer, IEntity sourceEntity, IEntity targetEntity, WeaponHash weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel);
    public delegate void OutgoingDamageDelegate(IEntity sourceEntity, IEntity targetEntity, IPlayer sourcePlayer, WeaponHash weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel);
    public delegate void PlayerDelegate(IPlayer player);
    public delegate void PlayerStartEnterVehicleDelegate(IVehicle vehicle, VehicleSeat seat, CancelEventArgs cancel);
    public delegate void SpawnDelegate(CancelEventArgs cancel);
    public delegate void TickDelegate(int currentMs);
    public delegate void TickNametagDelegate(List<TickNametagData> nametagDatas);
    public delegate void WeaponShotDelegate(Position3D targetPos, IPlayer target, CancelEventArgs cancel);

    public delegate void ObjectArgsDelegate(object[] args);

    public interface IEventAPI
    {
        ICollection<EventMethodData<DeathDelegate>> Death { get; }
        ICollection<EventMethodData<EntityStreamInDelegate>> EntityStreamIn { get; }
        ICollection<EventMethodData<EntityStreamOutDelegate>> EntityStreamOut { get; }
        ICollection<EventMethodData<IncomingDamageDelegate>> IncomingDamage { get; }
        ICollection<EventMethodData<OutgoingDamageDelegate>> OutgoingDamage { get; }
        ICollection<EventMethodData<SpawnDelegate>> Spawn { get; }
        ICollection<EventMethodData<PlayerStartEnterVehicleDelegate>> PlayerStartEnterVehicle { get; }
        ICollection<EventMethodData<PlayerDelegate>> PlayerStartTalking { get; }
        ICollection<EventMethodData<PlayerDelegate>> PlayerStopTalking { get; }
        ICollection<EventMethodData<TickDelegate>> Tick { get; }
        ICollection<EventMethodData<TickNametagDelegate>> TickNametag { get; }
        ICollection<EventMethodData<WeaponShotDelegate>> WeaponShot { get; }

        void Add(string eventName, ObjectArgsDelegate method);
        void CallLocal(string eventName, params object[] args);
    }
}
