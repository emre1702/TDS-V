namespace TDS_Client.Handler.Lobby
{
    public class RoundHandler
    {


        

        public void StopFight()
        {
            // bei LobbyLeave schon gemacht
            InFight = false;
            IsSpectator = false;
            FightInfo.Reset();
            Countdown.Stop();
        }

        public void Reset(bool removemapinfo)
        {
            StopFight();

            // bei LobbyLeave schon gemacht
            Spectate.Stop();
            if (removemapinfo)
                LobbyMapDatasHandler.RemoveMapInfo();
        }
    }
}
