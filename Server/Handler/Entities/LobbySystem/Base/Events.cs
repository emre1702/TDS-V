﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        #region Protected Properties

        protected Dictionary<ITDSPlayer, TDSTimer> DeathSpawnTimer { get; } = new Dictionary<ITDSPlayer, TDSTimer>();

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        ///
        /// </summary> <param name="player"></param> <param name="killer"></param> <param
        /// name="weapon"></param> <returns>Time in ms to disapper & spawn again</returns>
        public virtual void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon, bool spawnPlayer = true)
        {
            if (spawnPlayer)
            {
                if (DeathSpawnTimer.ContainsKey(player))
                {
                    DeathSpawnTimer[player].Kill();
                    DeathSpawnTimer.Remove(player);
                }

                player.Spawn(SpawnPoint.Around(Entity.AroundSpawnPoint), Entity.DefaultSpawnRotation);
            }
        }

        public virtual void OnPlayerEnterColshape(ITDSColShape colshape, ITDSPlayer player)
        {
        }

        public ValueTask OnPlayerLoggedOut(ITDSPlayer player)
            => new ValueTask(RemovePlayer(player));

        public virtual void OnPlayerSpawn(ITDSPlayer player)
        {
            player.Health = Entity.FightSettings?.StartHealth ?? 100;
            player.Armor = Entity.FightSettings?.StartArmor ?? 100;
            player.SetClothes(11, 0, 0);
        }

        #endregion Public Methods
    }
}