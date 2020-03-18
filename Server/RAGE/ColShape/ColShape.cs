using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.RAGE.Startup;
using static TDS_Server.Data.Interfaces.ModAPI.ColShape.IColShape;

namespace TDS_Server.RAGE.ColShape
{
    class ColShape : IColShape
    {
        private readonly GTANetworkAPI.ColShape _instance;

        public ColShape(GTANetworkAPI.ColShape colShape)
        {
            _instance = colShape;

            _instance.OnEntityEnterColShape += Instance_OnEntityEnterColShape;
            _instance.OnEntityExitColShape += Instance_OnEntityExitColShape;
        }

        public event ColShapeEnterExitDelegate? PlayerEntered;
        public event ColShapeEnterExitDelegate? PlayerExited;

        public ushort Id => _instance.Id;

        public void Delete()
        {
            _instance.Delete();
        }

        public bool Equals(IColShape? other)
        {
            return _instance.Id == other?.Id;
        }

        private void Instance_OnEntityEnterColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (_instance != colShape)
                return;
            var tdsPlayer = Program.GetTDSPlayer(client);
            if (tdsPlayer is null)
                return;

            PlayerEntered?.Invoke(tdsPlayer);
        }

        private void Instance_OnEntityExitColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (_instance != colShape)
                return;
            var tdsPlayer = Program.GetTDSPlayer(client);
            if (tdsPlayer is null)
                return;

            PlayerExited?.Invoke(tdsPlayer);
        }
    }
}
