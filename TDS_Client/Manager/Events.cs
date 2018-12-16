using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;

namespace TDS_Client.Manager
{
    class Events : Script
    {
        public Events()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddToClientEvents();
            AddFromBrowserEvents();
        }

        #region Load on start
        private void LoadOnStart()
        {
            Settings.Load();
        }
        #endregion Load on start

        #region RAGE events
        private void AddRAGEEvents()
        {
            Tick += OnTickMethod;
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
        }

        private void OnTickMethod(List<TickNametagData> nametags)
        {
            Damagesys.ShowBloodscreenIfNecessary();
            ScaleformMessage.Render();
        }

        private void OnPlayerWeaponShotMethod(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            Damagesys.OnWeaponShot(targetPos, target, cancel);
        }

        private void OnPlayerSpawnMethod(CancelEventArgs cancel)
        {
            Death.PlayerSpawn();
        }

        private void OnPlayerDeathMethod(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            Death.PlayerDeath(player);
        }
        #endregion RAGE events

        #region From Server events 
        private void AddToClientEvents()
        {
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string) args[0];
            MainBrowser.OnLoadOwnMapRatings(datajson);
        }

        private void OnHitOpponentMethod(object[] args)
        {
            DeathmatchInfo.HittedOpponent();
        }
        #endregion From Server events

        #region From Browser events 
        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMap);
        }

        private void OnAddRatingToMap(object[] args)
        {
            string currentmap = (string)args[0];
            int rating = (int)args[1];
            MainBrowser.OnSendMapRating(currentmap, rating);
        }
        #endregion Browser events 
    }
}
