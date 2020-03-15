using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces.ModAPI.Sync
{
    public interface ISyncAPI
    {
        /// <summary>
        /// Sends an event to every player
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void SendEvent(string eventName, params object[] args);

        /// <summary>
        /// Sends an event to a single player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void SendEvent(ITDSPlayer player, string eventName, params object[] args);

        /// <summary>
        /// Sends an event to every player in a lobby.
        /// </summary>
        /// <param name="lobby"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void SendEvent(ILobby lobby, string eventName, params object[] args);

        /// <summary>
        /// Sends an event to a collection of players
        /// </summary>
        /// <param name="players"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void SendEvent(ICollection<ITDSPlayer> players, string eventName, params object[] args);

        /// <summary>
        /// Sends an event to every player in a team.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void SendEvent(ITeam team, string eventName, params object[] args);
    }
}
