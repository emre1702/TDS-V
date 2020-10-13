using GTANetworkAPI;
using NSubstitute;
using NUnit.Framework;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.TeamsSystem;

namespace TDS_Server.Tests.Systems.TeamsSystem
{
    public class SyncTests
    {
        [Test]
        public void Calls_SyncAddedPlayer_OnPlayerAdd()
        {
            /*var chat = Substitute.For<ITeamChat>();
            var players = Substitute.For<ITeamPlayers>();
            var sync = Substitute.For<ITeamSync>();

            var testTeam = Substitute.For<Team>(chat, players, sync);
            var testPlayer = Substitute.For<ITDSPlayer>(new NetHandle());

            testTeam.Players.Add(testPlayer);

            sync.ReceivedWithAnyArgs().SyncAddedPlayer(Arg.Any<ITDSPlayer>());*/
        }
    }
}
