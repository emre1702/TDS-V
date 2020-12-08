using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Default;

namespace TDS.Server.GamemodesSystem.MapHandlers
{
    public class GangwarMapHandler : BaseGamemodeMapHandler, IGangwarGamemodeMapHandler
    {
        private ITDSBlip? _targetBlip;
        private ITDSTextLabel? _targetTextLabel;

        public ITDSObject? TargetObject { get; set; }

        private readonly IRoundFightLobby _lobby;
        private readonly ISettingsHandler _settingsHandler;

        public GangwarMapHandler(IRoundFightLobby lobby, ISettingsHandler settingsHandler)
        {
            _lobby = lobby;
            _settingsHandler = settingsHandler;
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);

            events.InitNewMap += InitMap;
            events.RoundClear += ClearTarget;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);

            events.InitNewMap -= InitMap;
            if (events.RoundClear is { })
                events.RoundClear -= ClearTarget;
        }

        private ValueTask ClearTarget()
        {
            NAPI.Task.RunSafe(() =>
            {
                _targetBlip?.Delete();
                _targetBlip = null;

                TargetObject?.Delete();
                TargetObject = null;

                _targetTextLabel?.Delete();
                _targetTextLabel = null;
            });

            return default;
        }

        private void InitMap(MapDto map)
        {
            NAPI.Task.RunSafe(() =>
            {
                CreateTargetObject(map);
                CreateTargetBlip(map);
                CreateTargetTextLabel();
            });
        }

        private void CreateTargetBlip(MapDto map)
        {
            if (map.Target is null)
                return;

            _targetBlip = NAPI.Blip.CreateBlip(SharedConstants.TargetBlipSprite, map.Target.ToVector3(), 1f, 0, name: "Target", dimension: _lobby.MapHandler.Dimension) as ITDSBlip;
        }

        private void CreateTargetObject(MapDto map)
        {
            if (map.Target is null)
                return;

            TargetObject = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(SharedConstants.TargetHashName), map.Target.ToVector3(), new Vector3(), 120, _lobby.MapHandler.Dimension) as ITDSObject;
            TargetObject?.Freeze(true, _lobby);
            TargetObject?.SetCollisionsless(true, _lobby);
        }

        private void CreateTargetTextLabel()
        {
            if (TargetObject is null)
                return;

            _targetTextLabel = NAPI.TextLabel.CreateTextLabel("Target", TargetObject.Position,
                (float)_settingsHandler.ServerSettings.GangActionTargetRadius, 7f, 0, new Color(220, 220, 220), true, _lobby.MapHandler.Dimension) as ITDSTextLabel;
        }
    }
}
