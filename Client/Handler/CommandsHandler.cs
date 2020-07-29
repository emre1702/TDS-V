using System;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class CommandsHandler : ServiceBase
    {
        #region Private Fields

        private readonly CamerasHandler _camerasHandler;
        private readonly ChatHandler _chatHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        private PedBone _nextPedBone = PedBone.SKEL_ROOT;

        #endregion Private Fields

        #region Public Constructors

        public CommandsHandler(IModAPI modAPI, LoggingHandler loggingHandler, ChatHandler chatHandler, SettingsHandler settingsHandler,
                    LobbyHandler lobbyHandler, PlayerFightHandler playerFightHandler, CamerasHandler camerasHandler, RemoteEventsSender remoteEventsSender,
            UtilsHandler utilsHandler)
            : base(modAPI, loggingHandler)
        {
            _chatHandler = chatHandler;
            _lobbyHandler = lobbyHandler;
            _playerFightHandler = playerFightHandler;
            _camerasHandler = camerasHandler;
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;

            modAPI.Event.Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);
        }

        #endregion Public Constructors

        #region Private Methods

        private void DrawBone(int currentMs)
        {
            var pedId = ModAPI.LocalPlayer.PlayerPedId();
            var coords = ModAPI.Ped.GetPedBoneCoords(pedId, (int)_nextPedBone);
            if (coords is null)
                return;

            var screenCoords = _utilsHandler.GetScreenCoordFromWorldCoord(coords);
            ModAPI.Graphics.DrawText(_nextPedBone.ToString(), (int)(screenCoords.X * 1920f), (int)(screenCoords.Y * 1080f), Data.Enums.Font.Monospace, 1f, Color.Red, AlignmentX.Center, true, true, 999);
        }

        private void OnCommandUsedMethod(object[] args)
        {
            _chatHandler.CloseChatInput();

            string msg = (string)args[0];
            var cmd = msg.Split(' ')[0];

            if (cmd == "checkshoot")
            {
                if (_lobbyHandler.Bomb.BombOnHand || !_playerFightHandler.InFight)
                    ModAPI.Chat.Output("Shooting is blocked. Reason: " + (_playerFightHandler.InFight ? "bomb" : (!_lobbyHandler.Bomb.BombOnHand ? "round" : "both")));
                else
                    ModAPI.Chat.Output("Shooting is not blocked.");
                return;
            }
            else if (cmd == "activecam" || cmd == "activecamera")
            {
                Logging.LogWarning((_camerasHandler.ActiveCamera?.Name ?? "No camera") + " | " + (_camerasHandler.ActiveCamera?.SpectatingEntity is null ? "no spectating" : "spectating"), "ChatHandler.Command");
                Logging.LogWarning((_camerasHandler.Spectating.IsSpectator ? "Is spectator" : "Is not spectator") + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + ((IPlayer)_camerasHandler.Spectating.SpectatingEntity).Name : "not spectating entity"), "ChatHandler.Command");
                Logging.LogWarning(_camerasHandler.SpectateCam.Position.ToString() + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + _camerasHandler.Spectating.SpectatingEntity.Position.ToString() : "not spectating entity"), "ChatHandler.Command");
                return;
            }
            else if (cmd == "campos" || cmd == "camerapos")
            {
                ModAPI.Chat.Output("Position: " + _camerasHandler.ActiveCamera.Position.ToString());
                ModAPI.Chat.Output("Rotation: " + _camerasHandler.ActiveCamera.Rotation.ToString());
                return;
            }
            else if (cmd == "boneindex")
            {
                foreach (var boneId in Enum.GetValues(typeof(PedBone)).Cast<PedBone>())
                {
                    var boneIndex = ModAPI.Ped.GetPedBoneIndex(ModAPI.LocalPlayer.Handle, (int)boneId);
                    Logging.LogWarning("Bone id: " + boneId + " | Bone index: " + boneIndex);
                }
            }
            else if (cmd == "bone")
            {
                string input = msg.Split(' ')[1];
                if (input == "start")
                {
                    ModAPI.Event.Tick.Add(new Data.Models.EventMethodData<Data.Interfaces.ModAPI.Event.TickDelegate>(DrawBone));
                }
                else
                {
                    var bodyPart = SharedUtils.GetPedBodyPart(_nextPedBone);
                    if (!bodyPart.ToString().Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        string m = "Wrong position for " + _nextPedBone.ToString() + ". Input: " + input + " - Expected: " + bodyPart.ToString();
                        Logging.LogWarning(m);
                        ModAPI.Chat.Output(m);
                    }
                    _nextPedBone = Enum.GetValues(typeof(PedBone)).Cast<PedBone>().Concat(new[] { default(PedBone) }).SkipWhile(e => !_nextPedBone.Equals(e)).Skip(1).First();
                }

                return;
            }
            /*else if (cmd == "cutscene")
            {
                
                ModAPI.Cutscene.RequestCutscene(Enum.Parse<CutsceneType>(msg.Split(' ')[1]));
                ModAPI.Native.Invoke(NativeHash.SET_NETWORK_CUTSCENE_ENTITIES, true);
                ModAPI.Native.Invoke(NativeHash.NETWORK_SET_IN_MP_CUTSCENE, true, true);
                ModAPI.Native.Invoke(NativeHash.SET_LOCAL_PLAYER_VISIBLE_IN_CUTSCENE, true, false);
                ModAPI.Native.Invoke(NativeHash.SET_ENTITY_VISIBLE_IN_CUTSCENE, ModAPI.LocalPlayer.Handle, true, true);
                ModAPI.Cutscene.RegisterEntityForCutscene(ModAPI.LocalPlayer, "MP_1", 0, 0, 64);
                ModAPI.Native.Invoke(NativeHash.SET_CUTSCENE_ENTITY_STREAMING_FLAGS, "MP_1", 0, 1);
                ModAPI.Native.Invoke((NativeHash)17876293797489278094, 2);
                ModAPI.Native.Invoke((NativeHash)11413602432665415843, true);
                ModAPI.Native.Invoke((NativeHash)18165510258038530118, true);
                ModAPI.Native.Invoke((NativeHash)2361794840612055679, true);
                ModAPI.Cutscene.StartCutscene(0);
                return;
            }*/

            var origCmdData = _settingsHandler.CommandsData.AddedCommands.FirstOrDefault(c => c.CustomCommand.Equals(cmd, StringComparison.OrdinalIgnoreCase));

            if (!(origCmdData is null))
            {
                var origCmd = _settingsHandler.CommandsData.InitialCommands.FirstOrDefault(c => c.Id == origCmdData.CommandId);
                if (!(origCmd is null))
                {
                    msg = origCmd.Command + (msg.Length > cmd.Length ? msg.Substring(cmd.Length) : string.Empty);
                    cmd = origCmd.Command;
                }
            }
            _remoteEventsSender.Send(ToServerEvent.CommandUsed, msg);
        }

        #endregion Private Methods
    }
}
