using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Deathmatch
{
    public class DamageHandler
    {
        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly IModAPI _modAPI;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        #endregion Private Fields

        #region Public Constructors

        public DamageHandler(IModAPI modAPI, BrowserHandler browserHandler, RemoteEventsSender remoteEventsSender, PlayerFightHandler playerFightHandler,
            LobbyHandler lobbyHandler)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _remoteEventsSender = remoteEventsSender;
            _playerFightHandler = playerFightHandler;
            _lobbyHandler = lobbyHandler;

            modAPI.Event.IncomingDamage.Add(new EventMethodData<IncomingDamageDelegate>(OnIncomingDamageMethod, () => playerFightHandler.InFight));
            modAPI.Event.OutgoingDamage.Add(new EventMethodData<OutgoingDamageDelegate>(OnOutgoingDamageMethod, () => playerFightHandler.InFight));
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnIncomingDamageMethod(IPlayer sourcePlayer, IEntity sourceEntity, IEntity targetEntity, WeaponHash weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            _modAPI.Console.Log(ConsoleVerbosity.Info, $"Incoming damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type}, targetEntity {targetEntity.Type} - {targetEntity is IPlayer}", true);

            if (sourcePlayer != null)
            {
                cancel.Cancel = true;
                if (_lobbyHandler.Teams.IsInSameTeam(sourcePlayer))
                    return;

                _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.GotHit, (int)sourcePlayer.RemoteId, weaponHash.ToString(), boneIdx.ToString());
                _browserHandler.PlainMain.ShowBloodscreen();
            }
            else
                _browserHandler.PlainMain.ShowBloodscreen();
        }

        private void OnOutgoingDamageMethod(IEntity sourceEntity, IEntity targetEntity, IPlayer sourcePlayer, WeaponHash weaponHash, ulong boneIdx, int damage, CancelEventArgs cancel)
        {
            _modAPI.Console.Log(ConsoleVerbosity.Info, $"Outgoing damage: Source {sourcePlayer.Name}, source entity {sourceEntity.Type}, targetEntity {targetEntity.Type} - {targetEntity is IPlayer}", true);

            if (sourcePlayer is null)
                return;

            if (_lobbyHandler.Teams.IsInSameTeam(sourcePlayer))
            {
                cancel.Cancel = true;
                return;
            }

            _playerFightHandler.HittedOpponent();
        }

        #endregion Private Methods
    }
}
