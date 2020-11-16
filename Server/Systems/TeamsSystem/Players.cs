using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Utility;

namespace TDS.Server.TeamsSystem
{
    public class Players : ITeamPlayers
    {
        public int Amount
        {
            get { lock (_all) { return _all.Count; } }
        }

        public int AmountAlive
        {
            get
            {
                if (_alive is null)
                    return 0;
                lock (_alive) { return _alive.Count; }
            }
        }

        public int AmountSpectatable
        {
            get
            {
                if (_spectatable is null)
                    return 0;
                lock (_spectatable) { return _spectatable.Count; }
            }
        }

        public bool HasAnySpectatable
        {
            get
            {
                if (_spectatable is null)
                    return false;
                lock (_spectatable) { return _spectatable.Count > 0; }
            }
        }

        public bool HasAnyAlive
        {
            get
            {
                if (_alive is null)
                    return false;
                lock (_alive) { return _alive.Count > 0; }
            }
        }

        private List<ITDSPlayer>? _alive;
        private List<ITDSPlayer>? _spectatable;
        private readonly List<ITDSPlayer> _all = new List<ITDSPlayer>();

#nullable disable
        private ITeam _team;
#nullable enable

        public void Init(ITeam team)
        {
            _team = team;
            if (!team.IsSpectator)
            {
                _alive = new List<ITDSPlayer>();
                _spectatable = new List<ITDSPlayer>();
            }
        }

        public void Add(ITDSPlayer player)
        {
            lock (_all) { _all.Add(player); }
            NAPI.Task.RunSafe(() => 
                player.SetSkin(_team.Entity.SkinHash != 0 ? (PedHash)_team.Entity.SkinHash : player.FreemodeSkin));
            _team.Sync.SyncAddedPlayer(player);
        }

        public void AddAlive(ITDSPlayer player)
        {
            if (_alive is null)
                return;
            lock (_alive) { _alive.Add(player); }
            ++_team.SyncedData.AmountPlayers.Amount;
            ++_team.SyncedData.AmountPlayers.AmountAlive;
        }

        public bool Remove(ITDSPlayer player)
        {
            var wasIn = false;
            lock (_all) { wasIn = _all.Remove(player); }
            if (_spectatable is { })
                lock (_spectatable) { _spectatable.Remove(player); }
            RemoveAlive(player);
            _team.Sync.SyncRemovedPlayer(player);

            return wasIn;
        }

        public bool RemoveAlive(ITDSPlayer player)
        {
            var wasAlive = false;
            if (_alive is { })
            {
                lock (_alive)
                {
                    wasAlive = _alive.Remove(player);
                    if (wasAlive)
                        _team.SyncedData.AmountPlayers.AmountAlive = (uint)_alive.Count;
                }
            }

            return wasAlive;
        }

        public bool RemoveSpectatable(ITDSPlayer player)
        {
            var wasSpectatable = false;
            if (_spectatable is { })
            {
                lock (_spectatable)
                {
                    wasSpectatable = _spectatable.Remove(player);
                }
            }

            return wasSpectatable;
        }

        public void DoForAll(Action<ITDSPlayer> action)
        {
            lock (_all)
            {
                foreach (var player in _all)
                    action(player);
            }
        }

        public void DoInMain(Action<ITDSPlayer> action)
        {
            lock (_all)
            {
                NAPI.Task.RunSafe(() =>
                {
                    foreach (var player in _all)
                        action(player);
                });
            }
        }

        public void DoList(Action<List<ITDSPlayer>> action)
        {
            lock (_all)
            {
                action(_all);
            }
        }

        public ITDSPlayer? GetNearestPlayer(Vector3 position)
        {
            ITDSPlayer? player;
            if (_alive is { })
                lock (_alive) { player = _alive.MinBy(p => p.Position.DistanceTo(position)).FirstOrDefault(); }
            else
                lock (_all) { player = _all.MinBy(p => p.Position.DistanceTo(position)).FirstOrDefault(); }
            return player;
        }

        public IEnumerable<ushort> GetRemoteIds()
        {
            lock (_all)
            {
                return _all.Select(p => p.RemoteId);
            }
        }

        public ITDSPlayer[] GetAllArray()
        {
            lock (_all)
            {
                return _all.ToArray();
            }
        }

        public ITDSPlayer[] GetAllArrayExcept(ITDSPlayer player)
        {
            lock (_all)
            {
                return _all.Where(p => p != player).ToArray();
            }
        }

        public void AddToSpectatable(ITDSPlayer player)
        {
            if (_spectatable is null)
                return;

            lock (_spectatable)
            {
                if (!_spectatable.Contains(player))
                    _spectatable.Add(player);
            }
        }

        public void ClearAlive()
        {
            if (_alive is { })
                lock (_alive) { _alive.Clear(); }
        }

        public void ClearRound()
        {
            ClearAlive();

            _team.SyncedData.AmountPlayers.AmountAlive = 0;
            _team.SyncedData.AmountPlayers.Amount = 0;
        }

        public void ClearLists()
        {
            lock (_all) { _all.Clear(); }
            ClearAlive();
            if (_spectatable is { })
                lock (_spectatable) { _spectatable.Clear(); }
        }

        public ITDSPlayer GetRandom()
        {
            lock (_all)
            {
                return SharedUtils.GetRandom(_all);
            }
        }

        public int GetSpectatableIndex(ITDSPlayer player)
        {
            if (_spectatable is null)
                return -1;
            lock (_spectatable)
            {
                return _spectatable.IndexOf(player);
            }
        }

        public ITDSPlayer? GetSpectatableAtIndex(int index)
        {
            if (_spectatable is null)
                return null;
            lock (_spectatable)
            {
                return index < _spectatable.Count ? _spectatable[index] : null;
            }
        }

        public ITDSPlayer? Last()
        {
            lock (_all)
            {
                return _all.LastOrDefault();
            }
        }

        public int GetAlivesHealth(int armorPerLife, int hpPerLife)
        {
            if (_alive is null)
                return -1;
            int teamHp = 0;

            int healthPerLife = armorPerLife + hpPerLife;
            lock (_alive)
            {
                foreach (var player in _alive)
                    teamHp += player.Health + player.Armor + ((player.Lifes - 1) * healthPerLife);
                return teamHp;
            }
        }
    }
}
