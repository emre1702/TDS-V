using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.RAGEAPI.Entity
{
    class Entity : IEntity
    {
        private readonly RAGE.Elements.Entity _instance;

        public Entity(RAGE.Elements.Entity instance)
        {
            _instance = instance;
        }
    }
}
