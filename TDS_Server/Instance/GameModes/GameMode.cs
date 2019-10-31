﻿using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.GameModes
{
    abstract class GameMode
    {
        protected Arena Lobby;
        protected MapDto Map;

        public Team? WinnerTeam { get; set; }

        public GameMode(Arena lobby, MapDto map)
        {
            Lobby = lobby;
            Map = map;
        }

        public virtual void StartRoundCountdown() { }

        public virtual void StartRound() { }

        public virtual void StartMapChoose() { }

        public virtual void StartMapClear() { }

        public virtual void StopRound() { }


        public virtual void SendPlayerRoundInfoOnJoin(TDSPlayer player) { }


        public virtual void RemovePlayer(TDSPlayer player) { }
        public virtual void RemovePlayerFromAlive(TDSPlayer player) { }

        public virtual bool IsWeaponAllowed(EWeaponHash weaponHash) => true;

        public virtual void OnPlayerEnterColShape(ColShape shape, TDSPlayer player) { }
        public virtual void OnPlayerDeath(TDSPlayer player, TDSPlayer killer) { }
        public virtual void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon) { }

    }
}
