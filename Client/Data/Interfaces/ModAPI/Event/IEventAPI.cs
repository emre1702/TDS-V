using System;
using System.Collections.Generic;
using System.ComponentModel;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Event
{
    public delegate void ObjectArgsDelegate(object[] args);
    public delegate void DeathDelegate(IPlayer player, uint reason, IPlayer killer, CancelEventArgs cancel);
    public delegate void EntityStreamInDelegate(IEntity entity);
    public delegate void TickDelegate(ulong currentMs);
    public delegate void TickNametagDelegate(List<TickNametagData> nametagDatas);
    public delegate void WeaponShotDelegate(Position3D targetPos, IPlayer target, CancelEventArgs cancel);

    public interface IEventAPI
    {
        ICollection<EventMethodData<DeathDelegate>> Death { get; }
        ICollection<EventMethodData<EntityStreamInDelegate>> EntityStreamIn { get; }
        ICollection<EventMethodData<TickDelegate>> Tick { get; }
        ICollection<EventMethodData<TickNametagDelegate>> TickNametag { get; }
        ICollection<EventMethodData<WeaponShotDelegate>> WeaponShot { get; }

        void Add(string eventName, ObjectArgsDelegate method);
    }
}
