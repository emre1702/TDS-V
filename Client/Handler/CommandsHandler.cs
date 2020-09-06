using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Defaults;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class CommandsHandler : ServiceBase
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly ChatHandler _chatHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        private PedBone _nextPedBone = PedBone.SKEL_ROOT;

        public CommandsHandler(LoggingHandler loggingHandler, ChatHandler chatHandler, SettingsHandler settingsHandler,
                    LobbyHandler lobbyHandler, PlayerFightHandler playerFightHandler, CamerasHandler camerasHandler, RemoteEventsSender remoteEventsSender,
            UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _chatHandler = chatHandler;
            _lobbyHandler = lobbyHandler;
            _playerFightHandler = playerFightHandler;
            _camerasHandler = camerasHandler;
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;

            Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);
        }

        private void DrawBone(List<TickNametagData> _)
        {
            var coords = Ped.GetPedBoneCoords(RAGE.Elements.Player.LocalPlayer.Handle, (int)_nextPedBone, 0, 0, 0);
            if (coords is null)
                return;

            var screenCoords = _utilsHandler.GetScreenCoordFromWorldCoord(coords);
            RAGE.NUI.UIResText.Draw(_nextPedBone.ToString(), (int)(screenCoords.X * 1920f), (int)(screenCoords.Y * 1080f), Font.Monospace, 1f, Color.Red, RAGE.NUI.UIResText.Alignment.Centered, true, true, 999);
        }

        private bool _superJumpOn;
        private bool _moveOverrideOn;
        private bool _explosiveAmmoOn;
        private bool _explosiveMeleeOn;
        private bool _fireAmmoOn;

        private float _moveOverrideValue;

        private void OnCommandUsedMethod(object[] args)
        {
            _chatHandler.CloseChatInput();

            string msg = (string)args[0];
            var cmd = msg.Split(' ')[0];

            if (cmd == "checkshoot")
            {
                if (_lobbyHandler.Bomb.BombOnHand || !_playerFightHandler.InFight)
                    RAGE.Chat.Output("Shooting is blocked. Reason: " + (_playerFightHandler.InFight ? "bomb" : (!_lobbyHandler.Bomb.BombOnHand ? "round" : "both")));
                else
                    RAGE.Chat.Output("Shooting is not blocked.");
                return;
            }
            else if (cmd == "activecam" || cmd == "activecamera")
            {
                Logging.LogWarning((_camerasHandler.ActiveCamera?.Name ?? "No camera") + " | " + (_camerasHandler.ActiveCamera?.SpectatingEntity is null ? "no spectating" : "spectating"), "ChatHandler.Command");
                Logging.LogWarning((_camerasHandler.Spectating.IsSpectator ? "Is spectator" : "Is not spectator") + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + ((ITDSPlayer)_camerasHandler.Spectating.SpectatingEntity).Name : "not spectating entity"), "ChatHandler.Command");
                Logging.LogWarning(_camerasHandler.SpectateCam.Position.ToString() + " | " + (_camerasHandler.Spectating.SpectatingEntity != null ? "spectating " + _camerasHandler.Spectating.SpectatingEntity.Position.ToString() : "not spectating entity"), "ChatHandler.Command");
                return;
            }
            else if (cmd == "campos" || cmd == "camerapos")
            {
                RAGE.Chat.Output("Position: " + _camerasHandler.ActiveCamera.Position.ToString());
                RAGE.Chat.Output("Rotation: " + _camerasHandler.ActiveCamera.Rotation.ToString());
                return;
            }
            else if (cmd == "boneindex")
            {
                foreach (var boneId in Enum.GetValues(typeof(PedBone)).Cast<PedBone>())
                {
                    var boneIndex = Ped.GetPedBoneIndex(RAGE.Elements.Player.LocalPlayer.Handle, (int)boneId);
                    Logging.LogWarning("Bone id: " + boneId + " | Bone index: " + boneIndex);
                }
            }
            else if (cmd == "bone")
            {
                string input = msg.Split(' ')[1];
                if (input == "start")
                {
                    Tick += DrawBone;
                }
                else
                {
                    var bodyPart = SharedUtils.GetPedBodyPart(_nextPedBone);
                    if (!bodyPart.ToString().Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        string m = "Wrong position for " + _nextPedBone.ToString() + ". Input: " + input + " - Expected: " + bodyPart.ToString();
                        Logging.LogWarning(m);
                        RAGE.Chat.Output(m);
                    }
                    _nextPedBone = Enum.GetValues(typeof(PedBone)).Cast<PedBone>().Concat(new[] { default(PedBone) }).SkipWhile(e => !_nextPedBone.Equals(e)).Skip(1).First();
                }

                return;
            }
            else if (cmd == "stat")
            {
                string type = msg.Split(' ')[1];
                switch (type)
                {
                    case "stun":
                        RAGE.Elements.Player.LocalPlayer.SetMinGroundTimeForStungun(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "move":
                        if (_moveOverrideOn)
                            Tick -= MoveOverride;
                        else
                            Tick += MoveOverride;
                        _moveOverrideOn = !_moveOverrideOn;
                        _moveOverrideValue = float.Parse(msg.Split(' ')[2]);
                        break;

                    case "run":
                        Player.SetRunSprintMultiplierForPlayer(float.Parse(msg.Split(' ')[2]));
                        break;

                    case "swim":
                        Player.SetSwimMultiplierForPlayer(float.Parse(msg.Split(' ')[2]));
                        break;

                    case "airdrag":
                        Player.SetAirDragMultiplierForPlayersVehicle(float.Parse(msg.Split(' ')[2]));
                        break;

                    case "accuracy":
                        RAGE.Elements.Player.LocalPlayer.SetAccuracy(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "shootrate":
                        RAGE.Elements.Player.LocalPlayer.SetShootRate(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "gravity":
                        RAGE.Elements.Player.LocalPlayer.SetGravity(bool.Parse(msg.Split(' ')[2]));
                        break;

                    case "hasgravity":
                        RAGE.Elements.Player.LocalPlayer.SetHasGravity(bool.Parse(msg.Split(' ')[2]));
                        break;

                    case "health":
                        RAGE.Elements.Player.LocalPlayer.SetHealth(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "maxhealth":
                        RAGE.Elements.Player.LocalPlayer.SetMaxHealth(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "rechargehealth":
                        Player.SetPlayerHealthRechargeMultiplier(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "knocked":
                        RAGE.Elements.Player.LocalPlayer.SetCanBeKnockedOffVehicle(bool.Parse(msg.Split(' ')[2]) ? 1 : 0);
                        break;

                    case "ragdoll":
                        RAGE.Elements.Player.LocalPlayer.SetCanRagdoll(bool.Parse(msg.Split(' ')[2]));
                        break;

                    case "gravitylevel":
                        Misc.SetGravityLevel(int.Parse(msg.Split(' ')[2]));
                        break;

                    case "explosiveammo":
                        if (_explosiveAmmoOn)
                            Tick -= ExplosiveAmmo;
                        else
                            Tick += ExplosiveAmmo;
                        _explosiveAmmoOn = !_explosiveAmmoOn;
                        break;

                    case "explosivemelee":
                        if (_explosiveMeleeOn)
                            Tick -= ExplosiveMelee;
                        else
                            Tick += ExplosiveMelee;
                        _explosiveMeleeOn = !_explosiveMeleeOn;
                        break;

                    case "fireammo":
                        if (_fireAmmoOn)
                            Tick -= FireAmmo;
                        else
                            Tick += FireAmmo;
                        _fireAmmoOn = !_fireAmmoOn;
                        break;

                    case "superjump":
                        if (_superJumpOn)
                            Tick -= SuperJump;
                        else
                            Tick += SuperJump;
                        _superJumpOn = !_superJumpOn;
                        break;

                    case "infiniteammo":
                        RAGE.Elements.Player.LocalPlayer.SetInfiniteAmmo(bool.Parse(msg.Split(' ')[2]), RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon());
                        break;

                    case "infiniteammoclip":
                        RAGE.Elements.Player.LocalPlayer.SetInfiniteAmmoClip(bool.Parse(msg.Split(' ')[2]));
                        break;
                }

                return;
            }
            /*else if (cmd == "cutscene")
            {
                RAGE.Game.Cutscene.RequestCutscene(Enum.Parse<CutsceneType>(msg.Split(' ')[1]));
                RAGE.Game.Invoker.Invoke(NativeHash.SET_NETWORK_CUTSCENE_ENTITIES, true);
                RAGE.Game.Invoker.Invoke(NativeHash.NETWORK_SET_IN_MP_CUTSCENE, true, true);
                RAGE.Game.Invoker.Invoke(NativeHash.SET_LOCAL_PLAYER_VISIBLE_IN_CUTSCENE, true, false);
                RAGE.Game.Invoker.Invoke(NativeHash.SET_ENTITY_VISIBLE_IN_CUTSCENE, RAGE.Elements.Player.LocalPlayer.Handle, true, true);
                RAGE.Game.Cutscene.RegisterEntityForCutscene(RAGE.Elements.Player.LocalPlayer, "MP_1", 0, 0, 64);
                RAGE.Game.Invoker.Invoke(NativeHash.SET_CUTSCENE_ENTITY_STREAMING_FLAGS, "MP_1", 0, 1);
                RAGE.Game.Invoker.Invoke((NativeHash)17876293797489278094, 2);
                RAGE.Game.Invoker.Invoke((NativeHash)11413602432665415843, true);
                RAGE.Game.Invoker.Invoke((NativeHash)18165510258038530118, true);
                RAGE.Game.Invoker.Invoke((NativeHash)2361794840612055679, true);
                RAGE.Game.Cutscene.StartCutscene(0);
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

        private void MoveOverride(List<TickNametagData> _)
        {
            RAGE.Elements.Player.LocalPlayer.SetMoveRateOverride(_moveOverrideValue);
        }

        private void ExplosiveAmmo(List<TickNametagData> _)
        {
            RAGE.Game.Misc.SetExplosiveAmmoThisFrame(RAGE.Elements.Player.LocalPlayer.Handle);
        }

        private void ExplosiveMelee(List<TickNametagData> _)
        {
            RAGE.Game.Misc.SetExplosiveMeleeThisFrame(RAGE.Elements.Player.LocalPlayer.Handle);
        }

        private void FireAmmo(List<TickNametagData> _)
        {
            RAGE.Game.Misc.SetFireAmmoThisFrame(RAGE.Elements.Player.LocalPlayer.Handle);
        }

        private void SuperJump(List<TickNametagData> _)
        {
            RAGE.Game.Misc.SetSuperJumpThisFrame(RAGE.Elements.Player.LocalPlayer.Handle);
        }
    }
}
