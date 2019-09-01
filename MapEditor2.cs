using RAGE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace client_core
{
    public class MapEditor2 : Events.Script
    {
        public static bool canScroll = true;
        public static bool enableScroll = false;
        public static bool cantype = false;
        public class ListGUI
        {
            public List<GUI.ButtonObject> btns = new List<GUI.ButtonObject>();
            public int index = 0;
            public int displaySize = 10;
            public int previndex = 9;
            float time = 0.15f;
            GUI.ButtonObject obj1Top = null;

            public List<string> objs = new List<string>();

            GUI.ButtonObject search = null;
            GUI.ButtonObject clear = null;

            bool canClickItem = false;
            public void ClickItem()
            {
                if (canClickItem)
                {
                    if (curHov != null)
                    {
                        //RAGE.Chat.Output("Cur -> " + curHov.TextObject.Text);
                        if (!RAGE.Game.Streaming.IsModelInCdimage(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text)) || !RAGE.Game.Streaming.IsModelValid(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text)))
                        {
                            //RAGE.Chat.Output("Model Invalid");
                            return;
                        }

                        RAGE.Game.Utils.Settimera(0);
                        RAGE.Game.Streaming.RequestModel(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text));
                        while (!RAGE.Game.Streaming.HasModelLoaded(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text)))
                        {
                            RAGE.Game.Invoker.Wait(0);
                            GUI.Update();
                            RAGE.Game.Ui.HideHudAndRadarThisFrame();
                            RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Loading...");
                            RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                            RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                            RAGE.Game.Ui.SetTextCentre(true);
                            RAGE.Game.Ui.SetTextJustification(0);
                            RAGE.Game.Ui.SetTextFont(0);
                            RAGE.Game.Ui.SetTextDropShadow();
                            RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                            if (RAGE.Game.Utils.Timera() > 1000)
                            {
                                //RAGE.Chat.Output("Failed to load model");
                                return;
                            }
                        }
                        Vector3 e_p = EditorCamera.GetPosition();
                        Vector3 ep_2 = RageMath.ScreenToWorld(0.5f, 0.5f);
                        Vector3 ep_3 = ep_2 - e_p;
                        Vector3 from = e_p + ep_3 * 0.05f;
                        Vector3 to = e_p + ep_3 * 1000;
                        Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                        if (hit.Hit)
                        {
                            if (hit.EndCoords.DistanceTo(from) <= ClampDistance)
                            {
                                RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text), hit.EndCoords, new Vector3());
                                HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                                HighlightedEntity.SetPosition(en.Position);
                                HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                                HighlightedEntity.FreezePosition(true);
                                HasObjectHighlighted = true;
                            }
                            else
                            {
                                Vector3 t = to - from;
                                t.Normalize();
                                t *= ClampDistance;
                                Vector3 v = e_p + t;
                                RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text), v, new Vector3());
                                HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                                HighlightedEntity.SetPosition(en.Position);
                                HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                                HighlightedEntity.FreezePosition(true);
                                HasObjectHighlighted = true;
                            }
                        }
                        else
                        {
                            Vector3 t = to - from;
                            t.Normalize();
                            t *= ClampDistance;
                            Vector3 v = e_p + t;
                            RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text), v, new Vector3());
                            HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                            HighlightedEntity.SetPosition(en.Position);
                            HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                            HighlightedEntity.FreezePosition(true);
                            HasObjectHighlighted = true;
                        }

                        Vector3 a = new Vector3();
                        Vector3 b = new Vector3();
                        RAGE.Game.Misc.GetModelDimensions(RAGE.Game.Misc.GetHashKey(curHov.TextObject.Text), a, b);
                        Vector3 c = b - a;
                        HighlightedEntity.Size = c;
                    }
                }
            }

            public void DisableAll()
            {
                foreach (var x in btns)
                {
                    x.Disable();
                }

                if (obj1Top != null)
                    obj1Top.Disable();

                if (search != null)
                    search.Disable();

                if (clear != null)
                    clear.Disable();
            }

            public void EnableAll()
            {
                foreach (GUI.ButtonObject obj in btns)
                {
                    obj.Enable();
                }

                if (obj1Top != null)
                    obj1Top.Enable();

                if (search != null)
                    search.Enable();

                if (clear != null)
                    clear.Enable();
            }

            public void Initialize(string id)
            {
                objs.Clear();
                for (int i = 0; i < 20; i++)
                {
                    objs.Add(Objects.Hashes[i]);
                }

                foreach (var x in btns)
                {
                    GUI.DeleteButtonById(x.Id);
                }

                if (obj1Top != null)
                    GUI.DeleteButtonById(obj1Top.Id);
                if (search != null)
                    GUI.DeleteButtonById(search.Id);
                if (clear != null)
                    GUI.DeleteButtonById(clear.Id);
                obj1Top = null;
                search = null;
                clear = null;
                btns.Clear();
                if (id == "obj_list")
                {
                    search = GUI.CreateButton("lst_search", "Search...", new GUI.Vector2(0.885f, 0.025f), new GUI.Vector2(0.170f, 0.05f), new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(127, 140, 141, 255)).OnEnabled(new GUI.Actions()
                        .ShowOnScreen()
                        .IgnoreMouse())
                        .OnDisabled(new GUI.Actions()
                        .HideFromScreenWhenDone()
                        .IgnoreMouse())
                        .OnMouseEnter(() =>
                        {
                            GUI.GetButtonById("lst_search").LerpTextColorFromTo(new GUI.RGBA(127, 140, 141, 255), new GUI.RGBA(0, 0, 0, 255), 0.15f);
                            cantype = true;
                        })
                        .OnMouseExit(() =>
                        {
                            GUI.GetButtonById("lst_search").LerpTextColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(127, 140, 141, 255), 0.15f);
                            cantype = false;
                        });

                    clear = GUI.CreateButton("clear", "X", new GUI.Vector2(0.985f, 0.025f), new GUI.Vector2(0.03f, 0.05f), new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(255, 255, 255, 255)).OnEnabled(new GUI.Actions()
                        .ShowOnScreen()
                        .IgnoreMouse())
                        .OnDisabled(new GUI.Actions()
                        .HideFromScreenWhenDone()
                        .IgnoreMouse())
                        .OnMouseEnter(() =>
                        {

                        })
                        .OnMouseExit(() =>
                        {

                        })
                        .OnMouseUp(() =>
                        {
                            for (int i = 0; i < displaySize; i++)
                            {
                                btns[btns.Count - 1 - i].TextObject.Text = Objects.Hashes[i];
                            }
                            index = displaySize - 1;
                            GUI.GetButtonById("lst_search").TextObject.Text = "Search...";
                        });

                    for (int i = 0; i < displaySize; i++)
                    {
                        btns.Add(GUI.CreateButton("lst_" + i, "", new GUI.Vector2(0.9f, 0.075f + i * 0.05f), new GUI.Vector2(0.2f, 0.05f), new GUI.RGBA(0, 0, 0, 180)).OnEnabled(new GUI.Actions()
                        .ShowOnScreen()
                        .IgnoreMouse())
                        .OnDisabled(new GUI.Actions()
                        .HideFromScreenWhenDone()
                        .IgnoreMouse())
                        .OnMouseEnter(() =>
                        {
                            enableScroll = true;
                            canClickItem = true;
                        })
                        .OnMouseUp(() =>
                        {
                            ClickItem();
                        })
                        .OnMouseExit(() =>
                        {
                            enableScroll = false;
                            canClickItem = false;
                        }));
                    }

                    for (int i = 0; i < displaySize; i++)
                    {
                        btns[btns.Count - 1 - i].TextObject.Text = objs[i];
                    }

                    obj1Top = GUI.CreateButton("lst_-1", "", new GUI.Vector2(0f, 0f), new GUI.Vector2(0f, 0f), new GUI.RGBA(0, 0, 0, 180)).OnEnabled(new GUI.Actions()
                        .ShowOnScreen()
                        .IgnoreMouse())
                        .OnDisabled(new GUI.Actions()
                        .HideFromScreenWhenDone()
                        .IgnoreMouse())
                        .OnMouseEnter(() =>
                        {
                            enableScroll = true;
                            canClickItem = true;
                        })
                        .OnMouseUp(() =>
                        {
                            ClickItem();
                        })
                        .OnMouseExit(() =>
                        {
                            enableScroll = false;
                            canClickItem = false;
                        });
                }
            }

            public GUI.ButtonObject curHov = null;
            bool PrevM = false;
            public void Draw()
            {
                if (IsEditorActive)
                {
                    if (cantype)
                    {
                        List<string> lst = GetKeyInput();
                        if (GUI.GetButtonById("lst_search").TextObject.Text == "Search...")
                            GUI.GetButtonById("lst_search").TextObject.Text = "";
                        foreach (string c in lst)
                        {
                            if (GUI.GetButtonById("lst_search").TextObject.Text.Length < 15)
                            {
                                GUI.GetButtonById("lst_search").TextObject.Text += c;
                            }
                        }

                        if (lst.Count > 0)
                        {
                            string s = GUI.GetButtonById("lst_search").TextObject.Text.ToLower();
                            List<string> xd = new List<string>();
                            foreach (string str in Objects.Hashes)
                            {
                                if (xd.Count >= 50)
                                    break;
                                if (str.Contains(s))
                                {
                                    if (!xd.Contains(str))
                                    {
                                        xd.Add(str);
                                    }
                                }
                            }
                            lstGUI.objs = xd;
                            lstGUI.index = 0;
                            if (xd.Count < lstGUI.btns.Count)
                            {
                                int i = 0;
                                for (i = 0; i < xd.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = xd[i];
                                    lstGUI.index++;
                                }
                                for (; i < lstGUI.btns.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = "";
                                    lstGUI.index++;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < lstGUI.btns.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = xd[i];
                                    lstGUI.index++;
                                }
                            }
                        }
                    }
                    if (PrevM == true && !cantype)
                    {
                        if (GUI.GetButtonById("lst_search").TextObject.Text == "")
                        {
                            GUI.GetButtonById("lst_search").TextObject.Text = "Search...";
                        }
                    }

                    PrevM = cantype;

                    if (GUI.IsCursorEnabled())
                    {
                        foreach (GUI.ButtonObject obj in btns)
                        {
                            if (obj.RectObject.DidMouseEnter)
                            {
                                if (curHov != obj)
                                {
                                    obj.LerpColorFromTo(new GUI.RGBA(0, 0, 0, 180), new GUI.RGBA(0, 0, 0, 255), 0.25f);
                                    obj.LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.25f);
                                    curHov = obj;
                                }
                            }
                            else if (obj.RectObject.DidMouseLeave)
                            {
                                obj.LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 180), 0.25f);
                                obj.LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.25f);
                                if (curHov == obj)
                                    curHov = null;
                            }
                        }
                    }

                    if (enableScroll)
                    {
                        if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollUp))
                        {

                            if (canScroll)
                            {
                                if (objs.Count < displaySize)
                                    return;
                                previndex++;
                                string txt = GUI.GetButtonById("lst_search").TextObject.Text;
                                if (txt == "" || txt == "Search...")
                                {
                                    if (previndex >= Objects.Hashes.Count)
                                    {
                                        previndex = 0;
                                    }
                                }
                                else
                                {
                                    if (previndex >= lstGUI.objs.Count)
                                    {
                                        previndex = 0;
                                    }
                                }
                                GUI.Vector2 c = btns[0].GetPosition();
                                for (int i = 0; i < btns.Count - 1; i++)
                                {
                                    GUI.Vector2 pos = btns[i + 1].GetPosition();
                                    btns[i].LerpFromTo(btns[i].GetPosition(), pos, time);
                                }
                                btns.Insert(0, obj1Top);
                                GUI.Vector2 cr = new GUI.Vector2(c);
                                cr.Y -= 0.05f;
                                if (txt == "" || txt == "Search...")
                                    obj1Top.TextObject.Text = Objects.Hashes[previndex];
                                else
                                    obj1Top.TextObject.Text = lstGUI.objs[previndex];
                                obj1Top.RectObject.Size = new GUI.Vector2(0.2f, 0.05f);
                                obj1Top.LerpFromTo(cr, c, time, () =>
                                {
                                    canScroll = true;
                                });
                                obj1Top.LerpColorFromTo(new GUI.RGBA(0, 0, 0, 0), new GUI.RGBA(0, 0, 0, 180), time);
                                obj1Top.LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 0), new GUI.RGBA(255, 255, 255, 255), time);
                                obj1Top.IgnoreMouse(false);

                                GUI.Vector2 s = btns[btns.Count - 1].GetPosition();
                                s.Y += 0.05f;
                                btns[btns.Count - 1].LerpFromTo(btns[btns.Count - 1].GetPosition(), s, time);
                                btns[btns.Count - 1].LerpColorFromTo(new GUI.RGBA(0, 0, 0, 180), new GUI.RGBA(0, 0, 0, 0), time);
                                btns[btns.Count - 1].LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 255, 0), time);
                                //btns[btns.Count - 1].IgnoreMouse(true);
                                obj1Top = btns[btns.Count - 1];
                                btns.Remove(btns[btns.Count - 1]);
                            }

                            canScroll = false;
                        }
                        else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollDown))
                        {
                            if (objs.Count < displaySize)
                                return;
                            if (canScroll)
                            {
                                string txt = GUI.GetButtonById("lst_search").TextObject.Text;

                                GUI.Vector2 c = btns[btns.Count - 1].GetPosition();
                                for (int i = btns.Count - 1; i > 0; i--)
                                {
                                    GUI.Vector2 pos = btns[i - 1].GetPosition();
                                    btns[i].LerpFromTo(btns[i].GetPosition(), pos, time);
                                }
                                btns.Add(obj1Top);
                                GUI.Vector2 cr = new GUI.Vector2(c);
                                cr.Y += 0.05f;
                                if (txt == "" || txt == "Search...")
                                {
                                    if (previndex - displaySize < 0)
                                    {
                                        obj1Top.TextObject.Text = Objects.Hashes[Objects.Hashes.Count - Math.Abs(previndex - displaySize)];
                                    }
                                    else
                                    {
                                        obj1Top.TextObject.Text = Objects.Hashes[previndex - displaySize];
                                    }
                                }
                                else
                                {
                                    if (previndex - displaySize < 0)
                                    {
                                        obj1Top.TextObject.Text = lstGUI.objs[lstGUI.objs.Count - Math.Abs(previndex - displaySize)];
                                    }
                                    else
                                    {
                                        obj1Top.TextObject.Text = lstGUI.objs[previndex - displaySize];
                                    }
                                }
                                //obj1Top.TextObject.Text = Objects.Hashes[previndex];
                                obj1Top.RectObject.Size = new GUI.Vector2(0.2f, 0.05f);
                                obj1Top.LerpFromTo(cr, c, time, () =>
                                {
                                    canScroll = true;
                                });
                                obj1Top.LerpColorFromTo(new GUI.RGBA(0, 0, 0, 0), new GUI.RGBA(0, 0, 0, 180), time);
                                obj1Top.LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 0), new GUI.RGBA(255, 255, 255, 255), time);
                                obj1Top.IgnoreMouse(false);

                                GUI.Vector2 s = btns[0].GetPosition();
                                s.Y -= 0.05f;
                                btns[0].LerpFromTo(btns[0].GetPosition(), s, time);
                                btns[0].LerpColorFromTo(new GUI.RGBA(0, 0, 0, 180), new GUI.RGBA(0, 0, 0, 0), time);
                                btns[0].LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 255, 0), time);
                                //btns[0].IgnoreMouse(true);
                                obj1Top = btns[0];
                                btns.Remove(btns[0]);


                                previndex--;

                                if (previndex < 0)
                                {
                                    if (txt == "" || txt == "Search...")
                                    {
                                        previndex = Objects.Hashes.Count - 1;
                                    }
                                    else
                                    {
                                        previndex = lstGUI.objs.Count - 1;
                                    }
                                }
                            }

                            canScroll = false;
                        }
                    }
                }
            }
        }

        public class MapEditorObject
        {
            public RAGE.Elements.GameEntity Entity = null;
            public Vector3 Rotation = new Vector3();
            public Vector3 Position = new Vector3();
            public Vector3 TempRotation = new Vector3();
            public Vector3 TempPosition = new Vector3();
            public Vector3 Size = new Vector3();
            public bool Frozen = false;

            public void SetRotation(Vector3 vec, int rotationOrder = 2)
            {
                TempRotation = vec;
                Rotation = vec;
                if (Entity != null)
                {
                    Entity.SetData<float>("X_MP", Rotation.X);
                    Entity.SetData<float>("Y_MP", Rotation.Y);
                    Entity.SetData<float>("Z_MP", Rotation.Z);
                    RAGE.Game.Entity.SetEntityRotation(Entity.Handle, vec.X, vec.Y, vec.Z, rotationOrder, false);
                }
            }

            public void SetPosition(Vector3 vec)
            {
                TempPosition = vec;
                Position = vec;
                if (Entity != null)
                    Entity.Position = vec;
            }

            public void SetTempRotation(Vector3 vec, int rotationOrder = 2)
            {
                TempRotation = vec;
                if (Entity != null)
                    RAGE.Game.Entity.SetEntityRotation(Entity.Handle, vec.X, vec.Y, vec.Z, rotationOrder, false);
            }

            public void SetTempPosition(Vector3 vec)
            {
                TempPosition = vec;
                if (Entity != null)
                    Entity.Position = vec;
            }

            public void FreezePosition(bool value)
            {
                Frozen = value;
                if (Entity != null)
                    RAGE.Game.Entity.FreezeEntityPosition(Entity.Handle, value);
            }

            public void FreezeEntity(bool value)
            {
                if (Entity != null)
                    RAGE.Game.Entity.FreezeEntityPosition(Entity.Handle, value);
            }

            public void ActivatePhysics()
            {
                if (Entity != null)
                    RAGE.Game.Physics.ActivatePhysics(Entity.Handle);
            }

            public Vector3 GetRotation(int order = 2)
            {
                if (Entity != null)
                    return RAGE.Game.Entity.GetEntityRotation(Entity.Handle, order);
                return Rotation;
            }

            public Vector3 GetPosition()
            {
                if (Entity != null)
                    return Entity.Position;
                return Position;
            }
        }

        public static class VehiclePool
        {
            public static HashSet<RAGE.Elements.Vehicle> vehs = new HashSet<RAGE.Elements.Vehicle>();
            public static RAGE.Elements.Vehicle CreateVehicle(uint model, Vector3 pos)
            {
                RAGE.Elements.Vehicle v = new RAGE.Elements.Vehicle(model, pos);
                vehs.Add(v);
                return v;
            }

            public static void DeleteVehicle(RAGE.Elements.Vehicle veh)
            {
                vehs.Remove(veh);
                veh.Destroy();
            }

            public static RAGE.Elements.Vehicle GetVehicleByHandle(int handle)
            {
                return vehs.FirstOrDefault(x => x.Handle == handle);
            }
        }

        public static class MapEditorObjectPool
        {
            public static Dictionary<RAGE.Elements.GameEntity, MapEditorObject> AttachedEntities = new Dictionary<RAGE.Elements.GameEntity, MapEditorObject>();

            public static void ClearAll()
            {
                foreach(RAGE.Elements.GameEntity ent in AttachedEntities.Keys)
                {
                    ent.Destroy();
                }
                AttachedEntities.Clear();
            }
            public static void SaveMap(string name)
            {
                RAGE.Events.CallRemote("TrySaveMap", name);
            }

            public static void LoadMap(string name)
            {
                RAGE.Events.CallRemote("TryLoadMap", name);
            }

            public static MapEditorObject AttachEntity(RAGE.Elements.GameEntity ent)
            {
                if (ent == null)
                    return null;

                ent.SetData<bool>("isMapEditor", true);
                if (AttachedEntities.ContainsKey(ent))
                    return AttachedEntities[ent];

                MapEditorObject obj = new MapEditorObject();
                obj.Entity = ent;
                obj.Rotation = obj.GetRotation();
                ent.SetData<float>("X_MP", obj.Rotation.X);
                ent.SetData<float>("Y_MP", obj.Rotation.Y);
                ent.SetData<float>("Z_MP", obj.Rotation.Z);
                obj.TempRotation = obj.Rotation;
                obj.Position = ent.Position;
                obj.TempPosition = obj.Position;
                AttachedEntities[ent] = obj;
                return obj;
            }

            public static bool IsMapEditorObject(RAGE.Elements.GameEntity ent)
            {
                return AttachedEntities.ContainsKey(ent);
            }

            public static MapEditorObject GetMapEditorObject(RAGE.Elements.GameEntity ent)
            {
                return AttachedEntities.FirstOrDefault(x => x.Key == ent).Value;
            }

            public static MapEditorObject GetMapEditorObjectByHandle(int handle)
            {
                return AttachedEntities.FirstOrDefault(x => x.Key.Handle == handle).Value;
            }

            public static void DetachEntity(RAGE.Elements.GameEntity ent)
            {
                AttachedEntities.Remove(ent);
            }

            public static void DeleteEntity(RAGE.Elements.GameEntity ent)
            {
                AttachedEntities.Remove(ent);
                ent.Destroy();
            }
        }

        public static bool IsSaving = false;
        public static bool IsLoading = false;
        public static bool IsSaveHover = false;
        public static bool IsLoadHover = false;
        public static bool IsGroupHover = false;
        public static Dictionary<uint, string> FavoriteObjects = new Dictionary<uint, string>();
        public static RAGE.Elements.Player LocalPlayer = RAGE.Elements.Player.LocalPlayer;
        public static CameraObject EditorCamera = null;
        public static bool IsEditorActive = false;
        public static MapEditorObject HighlightedEntity = null;
        public static bool HasObjectHighlighted = false;
        public static bool HideHUD = false;
        public static int ScreenX = 0;
        public static int ScreenY = 0;
        public static bool RotatePreviewLeft = false;
        public static bool RotatePreviewRight = false;
        public static bool RotatePreviewUp = false;
        public static bool RotatePreviewDown = false;
        public static bool ZoomPreviewIn = false;
        public static bool ZoomPreviewOut = false;
        public static bool PlaceOnGroundProperly = false;
        public static List<GUI.ButtonObject> EditorButtons = new List<GUI.ButtonObject>();
        public static Vector3 Mem1 = new Vector3();
        public static Vector3 Mem2 = new Vector3();
        public static Vector3 Mem3 = new Vector3();
        public static ListGUI lstGUI = new ListGUI();
        public MapEditor2()
        {
            //RAGE.Game.Cam.RenderScriptCams(false, false, 0, true, false, 0);
            //RAGE.Game.Cam.DestroyAllCams(false);

            GUI.SetCursorDefaults(0.5f, 0.5f);

            EditorButtons.Add(GUI.CreateButton("side_1", "", new GUI.Vector2(0f, 0.5f), new GUI.Vector2(0.2f, 1f), new GUI.RGBA(0, 0, 0, 120))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 1f), new GUI.Vector2(0.2f, 1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.5f), new GUI.Vector2(0.1f, 0.5f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {
                        CanChangeEditor = true;
                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 1f), new GUI.Vector2(0f, 1.0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.5f), new GUI.Vector2(0f, 0.5f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {
                        HideHUD = false;
                        RAGE.Chat.Show(true);
                        CanChangeEditor = true;
                    }))
                .IgnoreMouse(true)
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("side_2", "", new GUI.Vector2(1f, 0.5f), new GUI.Vector2(0.2f, 1f), new GUI.RGBA(0, 0, 0, 120))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 1f), new GUI.Vector2(0.2f, 1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(1f, 0.5f), new GUI.Vector2(0.9f, 0.5f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {

                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 1f), new GUI.Vector2(0f, 1.0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.9f, 0.5f), new GUI.Vector2(1f, 0.5f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {

                    }))
                .IgnoreMouse(true)
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("place_properly", "Place Objects On Ground", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0f), new GUI.Vector2(0.2f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 1f), new GUI.Vector2(0.5f, 1f - 0.025f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    PlaceOnGroundProperly = !PlaceOnGroundProperly;

                    if(PlaceOnGroundProperly)
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(255, 255, 0, 255), 0f);
                    }
                    else
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(0, 0, 0, 255), 0f);
                    }
                })
                .OnMouseEnter(() =>
                {
                    if (PlaceOnGroundProperly)
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 150), new GUI.RGBA(255, 255, 0, 255), 0.75f);
                    }
                    else
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    }
                })
                .OnMouseExit(() =>
                {
                    if(PlaceOnGroundProperly)
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 0, 150), 0.75f);
                    }
                    else
                    {
                        GUI.GetButtonById("place_properly").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    }
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.05f), new GUI.Vector2(0.2f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 1f - 0.025f), new GUI.Vector2(0.5f, 1f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("clear_map", "Clear Map", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0f), new GUI.Vector2(0.1f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.3f, 1f), new GUI.Vector2(0.3f, 1f - 0.025f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    MapEditorObjectPool.ClearAll();
                })
                .OnMouseEnter(() =>
                {
                    if (PlaceOnGroundProperly)
                    {
                        GUI.GetButtonById("clear_map").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 150), new GUI.RGBA(255, 255, 0, 255), 0.75f);
                    }
                    else
                    {
                        GUI.GetButtonById("clear_map").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    }
                })
                .OnMouseExit(() =>
                {
                    if (PlaceOnGroundProperly)
                    {
                        GUI.GetButtonById("clear_map").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 0, 150), 0.75f);
                    }
                    else
                    {
                        GUI.GetButtonById("clear_map").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    }
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0.05f), new GUI.Vector2(0.1f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.3f, 1f - 0.025f), new GUI.Vector2(0.3f, 1f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );


            EditorButtons.Add(GUI.CreateButton("teleport_here", "Exit Here", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0f), new GUI.Vector2(0.1f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.55f, 0f), new GUI.Vector2(0.55f, 0.025f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    StopEditor(true);
                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("teleport_here").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("teleport_here").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("teleport_here").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("teleport_here").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0.05f), new GUI.Vector2(0.1f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.55f, 0.025f), new GUI.Vector2(0.55f, 0f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("teleport_back", "Exit Back", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0f), new GUI.Vector2(0.1f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.45f, 0f), new GUI.Vector2(0.45f, 0.025f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    StopEditor(false);
                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("teleport_back").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("teleport_back").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("teleport_back").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("teleport_back").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0.05f), new GUI.Vector2(0.1f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.45f, 0.025f), new GUI.Vector2(0.45f, 0f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("title", "~y~Map Editor 2", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.05f), new GUI.Vector2(0.1f, 0.05f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.05f), new GUI.Vector2(0f, 0.05f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .IgnoreMouse(true)
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("about_back", "Map Editor [BETA] v2.0.0", new GUI.Vector2(0f, 0f), new GUI.Vector2(0f, 0f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.4f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.4f, 0.1f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .IgnoreMouse(true)
                );

            EditorButtons.Add(GUI.CreateButton("about", "About", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.95f), new GUI.Vector2(0.1f, 0.95f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("about").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("about").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseUp(() =>
                {
                    if (GUI.GetButtonById("about_back").IsEnabled)
                        GUI.GetButtonById("about_back").Disable();
                    else
                        GUI.GetButtonById("about_back").Enable();
                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("about").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("about").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.95f), new GUI.Vector2(0f, 0.95f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("settings", "UPCOMING", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.85f), new GUI.Vector2(0.1f, 0.85f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("settings").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("settings").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("settings").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("settings").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.85f), new GUI.Vector2(0f, 0.85f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("share", "UPCOMING", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.75f), new GUI.Vector2(0.1f, 0.75f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("share").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("share").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("share").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("share").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.75f), new GUI.Vector2(0f, 0.75f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("save", "Save Map", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.15f), new GUI.Vector2(0.1f, 0.15f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if(GUI.GetButtonById("savemap_name").IsEnabled)
                    {
                        GUI.GetButtonById("savemap_name").Disable();
                        GUI.GetButtonById("savemap_add").Disable();
                    }
                    else
                    {
                        GUI.GetButtonById("savemap_name").TextObject.Text = "Enter Map Name";
                        GUI.GetButtonById("savemap_add").TextObject.Text = "Save Map";
                        GUI.GetButtonById("savemap_name").Enable();
                        GUI.GetButtonById("savemap_add").Enable();
                    }
                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("save").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("save").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("save").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("save").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.15f), new GUI.Vector2(0f, 0.15f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("load", "Load Map", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.25f), new GUI.Vector2(0.1f, 0.25f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (GUI.GetButtonById("loadmap_name").IsEnabled)
                    {
                        GUI.GetButtonById("loadmap_name").Disable();
                        GUI.GetButtonById("loadmap_add").Disable();
                    }
                    else
                    {
                        GUI.GetButtonById("loadmap_name").TextObject.Text = "Enter Map Name";
                        GUI.GetButtonById("loadmap_add").TextObject.Text = "Load Map";
                        GUI.GetButtonById("loadmap_name").Enable();
                        GUI.GetButtonById("loadmap_add").Enable();
                    }
                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("load").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("load").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("load").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("load").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.25f), new GUI.Vector2(0f, 0.25f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("controls", "UPCOMING", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.35f), new GUI.Vector2(0.1f, 0.35f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("controls").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("controls").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("controls").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("controls").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.35f), new GUI.Vector2(0f, 0.35f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("objects", "Object Database", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.1f), new GUI.Vector2(0.2f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0f, 0.45f), new GUI.Vector2(0.1f, 0.45f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("objects").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("objects").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("objects").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("objects").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnMouseUp(() =>
                {
                    IsPreviewerActive = !IsPreviewerActive;
                    if (IsPreviewerActive)
                    {
                        Mem1 = EditorCamera.GetPosition();
                        Mem2 = EditorCamera.GetRotation();
                        Mem3 = LocalPlayer.Position;
                        //PreviewIndex = 0;
                        PreviewObj = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), new Vector3(-2430.6f, -1198.6f, 1236.9f), new Vector3(0, 0, 180f), 255);
                        Vector3 a = new Vector3();
                        Vector3 b = new Vector3();
                        RAGE.Game.Misc.GetModelDimensions(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), a, b);
                        PreviewSize = b - a;
                        RotatePreviewLeft = false;
                        RotatePreviewRight = false;
                        ZoomPreviewIn = false;
                        ZoomPreviewOut = false;
                        RotatePreviewUp = false;
                        RotatePreviewDown = false;
                        GUI.GetButtonById("teleport_here").Disable();
                        GUI.GetButtonById("teleport_back").Disable();
                        GUI.GetButtonById("place_properly").Disable();
                        GUI.GetButtonById("clear_map").Disable();
                        GUI.GetButtonById("previewer_left").Enable();
                        GUI.GetButtonById("previewer_use").Enable();
                        GUI.GetButtonById("previewer_right").Enable();
                        GUI.GetButtonById("previewer_zoomin").Enable();
                        GUI.GetButtonById("previewer_zoomout").Enable();
                        GUI.GetButtonById("previewer_favoriteadd").Enable();
                        if (FavoriteObjects.ContainsKey(PreviewObj.Model))
                        {
                            GUI.GetButtonById("previewer_group").Enable();
                            GUI.GetButtonById("previewer_group").TextObject.Text = FavoriteObjects[PreviewObj.Model];
                            GUI.GetButtonById("previewer_group").RectObject.Color = new GUI.RGBA(0, 0, 0, 125);
                        }
                        GUI.GetButtonById("previewer_down").Enable();
                        GUI.GetButtonById("previewer_up").Enable();
                    }
                    else
                    {
                        if (PreviewObj != null)
                        {
                            PreviewObj.Destroy();
                            PreviewObj = null;
                        }
                        GUI.GetButtonById("teleport_here").Enable();
                        GUI.GetButtonById("teleport_back").Enable();
                        GUI.GetButtonById("place_properly").Enable();
                        GUI.GetButtonById("clear_map").Enable();
                        GUI.GetButtonById("previewer_left").Disable();
                        GUI.GetButtonById("previewer_use").Disable();
                        GUI.GetButtonById("previewer_right").Disable();
                        GUI.GetButtonById("previewer_zoomin").Disable();
                        GUI.GetButtonById("previewer_zoomout").Disable();
                        GUI.GetButtonById("previewer_favoriteadd").Disable();
                        GUI.GetButtonById("previewer_group").Disable();
                        GUI.GetButtonById("previewer_down").Disable();
                        GUI.GetButtonById("previewer_up").Disable();
                        EditorCamera.SetPosition(Mem1);
                        EditorCamera.SetRotation(Mem2);
                        LocalPlayer.Position = Mem3;
                    }
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.1f), new GUI.Vector2(0f, 0.1f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.1f, 0.45f), new GUI.Vector2(0f, 0.45f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .SetGroup("main_menu")
                );

            EditorButtons.Add(GUI.CreateButton("entity_info", "Entity Info", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.05f), new GUI.Vector2(0.2f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(1f, 0.7f), new GUI.Vector2(0.9f, 0.7f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.4f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.05f), new GUI.Vector2(0f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.9f, 0.7f), new GUI.Vector2(1f, 0.7f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.4f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("entity_rightclick", "Clone Object", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 150))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.075f, 0.03f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.25f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (HighlightedEntity != null)
                    {
                        if (HighlightedEntity.Entity.Type == RAGE.Elements.Type.Vehicle)
                        {
                            RAGE.Elements.Vehicle ent = VehiclePool.CreateVehicle(HighlightedEntity.Entity.Model, HighlightedEntity.GetPosition());
                            ent.SetRotation(HighlightedEntity.Rotation.X, HighlightedEntity.Rotation.Y, HighlightedEntity.Rotation.Z, 2, false);
                            MapEditorObjectPool.AttachEntity(ent).FreezePosition(true);
                        }
                        else if (HighlightedEntity.Entity.Type == RAGE.Elements.Type.Object)
                        {
                            RAGE.Elements.MapObject obj = new RAGE.Elements.MapObject(HighlightedEntity.Entity.Model, HighlightedEntity.GetPosition(), HighlightedEntity.GetRotation());
                            MapEditorObjectPool.AttachEntity(obj).FreezePosition(true);
                        }
                    }

                    GUI.GetButtonById("entity_rightclick").Disable();
                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("entity_rightclick").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 150), new GUI.RGBA(0, 0, 0, 255), 0.75f);
                    GUI.GetButtonById("entity_rightclick").LerpTextColorFromTo(new GUI.RGBA(255, 255, 255, 255), new GUI.RGBA(255, 255, 0, 255), 0.75f);

                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("entity_rightclick").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 150), 0.75f);
                    GUI.GetButtonById("entity_rightclick").LerpTextColorFromTo(new GUI.RGBA(255, 255, 0, 255), new GUI.RGBA(255, 255, 255, 255), 0.75f);
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.075f, 0.03f), new GUI.Vector2(0f, 0f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.25f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("entity_info_type", "", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 0))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.05f), new GUI.Vector2(0.2f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(1f, 0.75f), new GUI.Vector2(0.9f, 0.75f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.4f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.05f), new GUI.Vector2(0f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.9f, 0.75f), new GUI.Vector2(1f, 0.75f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.4f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("entity_info_frozen", "", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 180))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0.05f), new GUI.Vector2(0.2f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(1f, 0.8f), new GUI.Vector2(0.9f, 0.8f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.4f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (HighlightedEntity != null)
                    {
                        HighlightedEntity.FreezeEntity(true);
                        HighlightedEntity.Frozen = !HighlightedEntity.Frozen;
                    }
                    if (HighlightedEntity.Frozen)
                    {
                        GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                    }
                    else
                    {
                        GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                    }
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.2f, 0.05f), new GUI.Vector2(0f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.9f, 0.8f), new GUI.Vector2(1f, 0.8f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.4f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("entity_info_frozen_state", "", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.015f, 0.025f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(1f, 0.8f), new GUI.Vector2(0.95f, 0.8f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {

                    }), () =>
                    {
                        if (HighlightedEntity != null)
                        {
                            if (HighlightedEntity.Frozen)
                            {
                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                            }
                            else
                            {
                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                            }
                        }
                    })
                .OnMouseUp(() =>
                {
                    if (HighlightedEntity != null)
                    {
                        HighlightedEntity.FreezeEntity(true);
                        HighlightedEntity.Frozen = !HighlightedEntity.Frozen;
                    }
                    if (HighlightedEntity.Frozen)
                    {
                        GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                    }
                    else
                    {
                        GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                    }
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.015f, 0.025f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.95f, 0.8f), new GUI.Vector2(1f, 0.8f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_left", "<", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.25f, 0.8f), new GUI.Vector2(0.25f, 0.8f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseDown(() =>
                {
                    RotatePreviewLeft = true;
                })
                .OnMouseUp(() =>
                {
                    RotatePreviewLeft = false;
                })
                .OnMouseExit(() =>
                {
                    RotatePreviewLeft = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.25f, 0.8f), new GUI.Vector2(0.25f, 0.8f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        RotatePreviewLeft = false;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_use", "Use", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.1f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.75f), new GUI.Vector2(0.5f, 0.75f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseDown(() =>
                {

                })
                .OnMouseUp(() =>
                {
                    if (PreviewObj != null)
                    {
                        uint hash = PreviewObj.Model;
                        PreviewObj.Destroy();
                        PreviewObj = null;
                        EditorCamera.SetPosition(Mem1);
                        EditorCamera.SetRotation(Mem2);
                        LocalPlayer.Position = Mem3;

                        GUI.GetButtonById("teleport_here").Enable();
                        GUI.GetButtonById("teleport_back").Enable();
                        GUI.GetButtonById("place_properly").Enable();
                        GUI.GetButtonById("clear_map").Enable();
                        GUI.GetButtonById("previewer_left").Disable();
                        GUI.GetButtonById("previewer_use").Disable();
                        GUI.GetButtonById("previewer_right").Disable();
                        GUI.GetButtonById("previewer_zoomin").Disable();
                        GUI.GetButtonById("previewer_zoomout").Disable();
                        GUI.GetButtonById("previewer_favoriteadd").Disable();
                        GUI.GetButtonById("previewer_group").Disable();
                        GUI.GetButtonById("previewer_down").Disable();
                        GUI.GetButtonById("previewer_up").Disable();

                        RAGE.Game.Utils.Settimera(0);
                        while (true)
                        {
                            RAGE.Game.Invoker.Wait(0);
                            GUI.Update();
                            RAGE.Game.Ui.HideHudAndRadarThisFrame();
                            RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Loading...");
                            RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                            RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                            RAGE.Game.Ui.SetTextCentre(true);
                            RAGE.Game.Ui.SetTextJustification(0);
                            RAGE.Game.Ui.SetTextFont(0);
                            RAGE.Game.Ui.SetTextDropShadow();
                            RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                            if (RAGE.Game.Utils.Timera() > 25)
                                break;
                        }

                        if (HighlightedEntity != null)
                        {
                            HighlightedEntity.Position = HighlightedEntity.TempPosition;
                            HighlightedEntity.SetRotation(HighlightedEntity.TempRotation, 2);
                            HighlightedEntity.FreezePosition(HighlightedEntity.Frozen);
                            HighlightedEntity.ActivatePhysics();
                        }

                        Vector3 e_p = EditorCamera.GetPosition();
                        Vector3 ep_2 = RageMath.ScreenToWorld(0.5f, 0.5f);
                        Vector3 ep_3 = ep_2 - e_p;
                        Vector3 from = e_p + ep_3 * 0.05f;
                        Vector3 to = e_p + ep_3 * 1000;
                        Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                        if (hit.Hit)
                        {
                            if (hit.EndCoords.DistanceTo(from) <= ClampDistance)
                            {
                                RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(hash, hit.EndCoords, new Vector3());
                                HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                                HighlightedEntity.SetPosition(en.Position);
                                HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                                HighlightedEntity.FreezePosition(true);
                                HasObjectHighlighted = true;
                            }
                            else
                            {
                                Vector3 t = to - from;
                                t.Normalize();
                                t *= ClampDistance;
                                Vector3 v = e_p + t;
                                RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(hash, v, new Vector3());
                                HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                                HighlightedEntity.SetPosition(en.Position);
                                HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                                HighlightedEntity.FreezePosition(true);
                                HasObjectHighlighted = true;
                            }
                        }
                        else
                        {
                            Vector3 t = to - from;
                            t.Normalize();
                            t *= ClampDistance;
                            Vector3 v = e_p + t;
                            RAGE.Elements.GameEntity en = new RAGE.Elements.MapObject(hash, v, new Vector3());
                            HighlightedEntity = MapEditorObjectPool.AttachEntity(en);
                            HighlightedEntity.SetPosition(en.Position);
                            HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(en.Handle, 2));
                            HighlightedEntity.FreezePosition(true);
                        }
                        Vector3 a = new Vector3();
                        Vector3 b = new Vector3();
                        RAGE.Game.Misc.GetModelDimensions(hash, a, b);
                        Vector3 c = b - a;
                        HighlightedEntity.Size = c;
                        HasObjectHighlighted = true;

                        GUI.GetButtonById("entity_info").Enable();
                        GUI.GetButtonById("entity_info_type").TextObject.Text = "Type: " + HighlightedEntity.Entity.Type.ToString();
                        GUI.GetButtonById("entity_info_type").Enable();
                        GUI.GetButtonById("entity_info_frozen").TextObject.Text = "Frozen";
                        GUI.GetButtonById("entity_info_frozen").Enable();
                        GUI.GetButtonById("entity_info_frozen_state").Enable();

                        if (HighlightedEntity.Frozen)
                        {
                            GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                        }
                        else
                        {
                            GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                        }

                        IsPreviewerActive = false;
                    }
                })
                .OnMouseExit(() =>
                {

                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.1f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.75f), new GUI.Vector2(0.5f, 0.75f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_right", ">", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.75f, 0.8f), new GUI.Vector2(0.75f, 0.8f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    RotatePreviewRight = false;
                })
                .OnMouseDown(() =>
                {
                    RotatePreviewRight = true;
                })
                .OnMouseExit(() =>
                {
                    RotatePreviewRight = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.75f, 0.8f), new GUI.Vector2(0.75f, 0.8f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        RotatePreviewRight = false;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_zoomin", "+", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.65f, 0.85f), new GUI.Vector2(0.65f, 0.85f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseDown(() =>
                {
                    ZoomPreviewIn = true;
                })
                .OnMouseUp(() =>
                {
                    ZoomPreviewIn = false;
                })
                .OnMouseExit(() =>
                {
                    ZoomPreviewIn = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.65f, 0.85f), new GUI.Vector2(0.65f, 0.85f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        ZoomPreviewIn = false;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_zoomout", "-", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.35f, 0.85f), new GUI.Vector2(0.35f, 0.85f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    ZoomPreviewOut = false;
                })
                .OnMouseDown(() =>
                {
                    ZoomPreviewOut = true;
                })
                .OnMouseExit(() =>
                {
                    ZoomPreviewOut = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.35f, 0.85f), new GUI.Vector2(0.35f, 0.85f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        ZoomPreviewOut = false;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_favoriteadd", "Add to favorites", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.85f), new GUI.Vector2(0.5f, 0.85f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (!FavoriteObjects.ContainsKey(PreviewObj.Model))
                    {
                        FavoriteObjects.Add(PreviewObj.Model, "");
                        GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                        GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Favorited!";
                        GUI.GetButtonById("previewer_group").Enable();
                        GUI.GetButtonById("previewer_group").TextObject.Text = "Enter Group Name";
                    }
                    else
                    {
                        FavoriteObjects.Remove(PreviewObj.Model);
                        GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                        GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Add to favorites";
                        GUI.GetButtonById("previewer_group").Disable();
                    }
                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseExit(() =>
                {

                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.85f), new GUI.Vector2(0.5f, 0.85f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_group", "Enter Group Name", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.9f), new GUI.Vector2(0.5f, 0.9f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {

                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("previewer_group").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(0, 0, 0, 255), 0.1f);
                    IsGroupHover = true;
                    CanChangeEditor = false;
                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("previewer_group").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                    IsGroupHover = false;
                    CanChangeEditor = true;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.9f), new GUI.Vector2(0.5f, 0.9f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        GUI.GetButtonById("previewer_group").RectObject.Color = new GUI.RGBA(0, 0, 0, 125);
                        GUI.GetButtonById("previewer_group").TextObject.Text = "Enter Group Name";
                        IsGroupHover = false;
                        CanChangeEditor = true;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_up", "^", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.35f, 0.75f), new GUI.Vector2(0.35f, 0.75f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseDown(() =>
                {
                    RotatePreviewUp = true;
                })
                .OnMouseUp(() =>
                {
                    RotatePreviewUp = false;
                })
                .OnMouseExit(() =>
                {
                    RotatePreviewUp = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.35f, 0.75f), new GUI.Vector2(0.35f, 0.75f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        RotatePreviewUp = false;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("previewer_down", "v", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.03f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.65f, 0.75f), new GUI.Vector2(0.65f, 0.75f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    RotatePreviewDown = false;
                })
                .OnMouseDown(() =>
                {
                    RotatePreviewDown = true;
                })
                .OnMouseExit(() =>
                {
                    RotatePreviewDown = false;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.03f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.65f, 0.75f), new GUI.Vector2(0.65f, 0.75f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        RotatePreviewDown = false;
                    }))
                );
            //previewer_group
            EditorButtons.Add(GUI.CreateButton("savemap_name", "Enter Map Name", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {

                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("savemap_name").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(0, 0, 0, 255), 0.1f);
                    IsSaveHover = true;
                    CanChangeEditor = false;
                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("savemap_name").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                    IsSaveHover = false;
                    CanChangeEditor = true;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        GUI.GetButtonById("savemap_name").RectObject.Color = new GUI.RGBA(0, 0, 0, 125);
                        GUI.GetButtonById("savemap_name").TextObject.Text = "Enter Map Name";
                        IsSaveHover = false;
                        CanChangeEditor = true;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("savemap_add", "Save Map", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.6f), new GUI.Vector2(0.5f, 0.6f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (!IsSaving)
                    {
                        IsSaving = true;
                        GUI.GetButtonById("savemap_add").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                        GUI.GetButtonById("savemap_add").TextObject.Text = "Saving...";
                        MapEditorObjectPool.SaveMap(GUI.GetButtonById("savemap_name").TextObject.Text);
                    }
                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseExit(() =>
                {

                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.6f), new GUI.Vector2(0.5f, 0.6f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            EditorButtons.Add(GUI.CreateButton("loadmap_name", "Enter Map Name", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.35f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {

                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseEnter(() =>
                {
                    GUI.GetButtonById("loadmap_name").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(0, 0, 0, 255), 0.1f);
                    IsLoadHover = true;
                    CanChangeEditor = false;
                })
                .OnMouseExit(() =>
                {
                    GUI.GetButtonById("loadmap_name").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 255), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                    IsLoadHover = false;
                    CanChangeEditor = true;
                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.5f), new GUI.Vector2(0.5f, 0.5f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.35f, 0f, 0.25f)
                    .OnFinish(() =>
                    {
                        GUI.GetButtonById("loadmap_name").RectObject.Color = new GUI.RGBA(0, 0, 0, 125);
                        GUI.GetButtonById("loadmap_name").TextObject.Text = "Enter Map Name";
                        IsLoadHover = false;
                        CanChangeEditor = true;
                    }))
                );

            EditorButtons.Add(GUI.CreateButton("loadmap_add", "Load Map", new GUI.Vector2(0f, 0f), new GUI.Vector2(0.2f, 0.1f), new GUI.RGBA(0, 0, 0, 125))
                .OnEnabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0f, 0f), new GUI.Vector2(0.15f, 0.05f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.6f), new GUI.Vector2(0.5f, 0.6f), 0.25f)
                    .ShowOnScreen()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0f, 0.5f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                .OnMouseUp(() =>
                {
                    if (!IsLoading)
                    {
                        IsLoading = true;
                        GUI.GetButtonById("loadmap_add").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                        GUI.GetButtonById("loadmap_add").TextObject.Text = "Loading...";
                        MapEditorObjectPool.LoadMap(GUI.GetButtonById("loadmap_name").TextObject.Text);
                    }
                })
                .OnMouseDown(() =>
                {

                })
                .OnMouseExit(() =>
                {

                })
                .OnDisabled(new GUI.Actions()
                    .SizeFromTo(new GUI.Vector2(0.15f, 0.05f), new GUI.Vector2(0f, 0f), 0.25f)
                    .MoveFromTo(new GUI.Vector2(0.5f, 0.6f), new GUI.Vector2(0.5f, 0.6f), 0.25f)
                    .HideFromScreenWhenDone()
                    .IgnoreMouse()
                    .ScaleTextFromTo(0.5f, 0f, 0.25f)
                    .OnFinish(() =>
                    {

                    }))
                );

            Events.OnEntityStreamIn += OnEntityStreamIn;
            Events.Tick += Tick;
            Events.Add("CanSaveMap", CanSaveMap);
            Events.Add("MapSaved", MapSaved);
            Events.Add("CanLoadMap", CanLoadMap);
        }

        public void CanLoadMap(object[] args)
        {
            if((bool)args[0] == false)
            {
                GUI.GetButtonById("loadmap_add").TextObject.Text = "Map Doesn't Exist!";
                IsLoading = false;
            }
            else
            {
                MapEditorObjectPool.ClearAll();
                string data = (string)args[1];
                List<string> dat = data.Split('@').ToList();
                dat.RemoveAt(0);
                foreach(string obj in dat)
                {
                    if (obj.Length == 0)
                        continue;
                    List<string> info = obj.Split('|').ToList();
                    if (info.Count == 0)
                        continue;
                    uint model = Convert.ToUInt32(info[1]);
                    float x = float.Parse(info[2]);
                    float y = float.Parse(info[3]);
                    float z = float.Parse(info[4]);
                    float rx = float.Parse(info[5]);
                    float ry = float.Parse(info[6]);
                    float rz = float.Parse(info[7]);
                    bool frozen = bool.Parse(info[8]);

                    if(info[0] == "obj")
                    {
                        RAGE.Elements.MapObject map = new RAGE.Elements.MapObject(model, new Vector3(x, y, z), new Vector3(rx, ry, rz));
                        MapEditorObject mapper = MapEditorObjectPool.AttachEntity(map);
                        mapper.SetPosition(new Vector3(x, y, z));
                        mapper.SetRotation(new Vector3(rx, ry, rz));
                        mapper.FreezePosition(frozen);
                    }
                    else if(info[1] == "veh")
                    {
                        RAGE.Elements.Vehicle veh = new RAGE.Elements.Vehicle(model, new Vector3(x, y, z));
                        veh.SetRotation(rx, ry, rz, 2, false);
                        MapEditorObject mapper = MapEditorObjectPool.AttachEntity(veh);
                        mapper.SetPosition(new Vector3(x, y, z));
                        mapper.SetRotation(new Vector3(rx, ry, rz));
                        mapper.FreezePosition(frozen);
                    }
                }

                GUI.GetButtonById("loadmap_add").TextObject.Text = "Loaded!";
                IsLoading = false;
            }
        }

        public void CanSaveMap(object[] args)
        {
            if((bool)args[0] == false)
            {
                GUI.GetButtonById("savemap_add").TextObject.Text = "Map Already Exists!";
                IsSaving = false;
            }
            else
            {
                //save map
                string name = (string)args[1];
                string data = "";
                foreach(var x in MapEditorObjectPool.AttachedEntities)
                {
                    if(x.Key.Type == RAGE.Elements.Type.Vehicle)
                    {
                        data += "veh|" + x.Key.Model + "|" + x.Value.Position.X + "|" + x.Value.Position.Y + "|" + x.Value.Position.Z +
                        "|" + x.Value.Rotation.X + "|" + x.Value.Rotation.Y + "|" + x.Value.Rotation.Z + "|" + x.Value.Frozen + "@";
                    }
                    else if(x.Key.Type == RAGE.Elements.Type.Object)
                    {
                        data += "obj|" + x.Key.Model + "|" + x.Value.Position.X + "|" + x.Value.Position.Y + "|" + x.Value.Position.Z +
                        "|" + x.Value.Rotation.X + "|" + x.Value.Rotation.Y + "|" + x.Value.Rotation.Z + "|" + x.Value.Frozen + "@";
                    }
                }
                RAGE.Events.CallRemote("SaveMap", name, data);
            }
        }

        public void MapSaved(object[] args)
        {
            GUI.GetButtonById("savemap_add").TextObject.Text = "Map Saved!";
            IsSaving = false;
        }

        float scrollSpeed = 1f;
        public void ProcessCamera()
        {

            //if (GUI.IsCursorEnabled())
            //    return;

            //Vector3 Pos = EditorCamera.GetPosition();
            //Vector3 Dir = EditorCamera.GetDirection();
            //Vector3 Rot = EditorCamera.GetRotation();
            //Vector3 vector = new Vector3();

            //float rightAxisX = RAGE.Game.Pad.GetDisabledControlNormal(0, 220) * 2f; //behave weird, fix
            //float rightAxisY = RAGE.Game.Pad.GetDisabledControlNormal(0, 221) * 2f;

            //float leftAxisX = RAGE.Game.Pad.GetDisabledControlNormal(0, 218);
            //float leftAxisY = RAGE.Game.Pad.GetDisabledControlNormal(0, 219);

            //float slowMult = 1f;
            //float fastMult = 1f;

            //if (KeyboardInput.IsKeyPressed("MapEditor_SlowMode"))
            //    slowMult = 0.5f;
            //if (KeyboardInput.IsKeyPressed("MapEditor_FastMode"))
            //    fastMult = 3f;

            //if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollUp))
            //{
            //    scrollSpeed *= 2f;
            //    //RAGE.Chat.Output("Scroll: " + scrollSpeed);
            //}
            //else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollDown))
            //{
            //    scrollSpeed /= 2f;
            //    //RAGE.Chat.Output("Scroll: " + scrollSpeed);
            //}

            //vector.X = Dir.X * leftAxisY * slowMult * fastMult * scrollSpeed;
            //vector.Y = Dir.Y * leftAxisY * slowMult * fastMult * scrollSpeed;
            //vector.Z = Dir.Z * leftAxisY * slowMult * fastMult * scrollSpeed;

            //Vector3 upVector = new Vector3(0, 0, 1);
            //Vector3 rightVector = RageMath.GetCrossProduct(RageMath.GetNormalizedVector(Dir), RageMath.GetNormalizedVector(upVector));

            //rightVector.X *= leftAxisX * 0.5f * slowMult * fastMult * scrollSpeed;
            //rightVector.Y *= leftAxisX * 0.5f * slowMult * fastMult * scrollSpeed;
            //rightVector.Z *= leftAxisX * 0.5f * slowMult * fastMult * scrollSpeed;

            //float GoUp = 0f;
            //float GoDown = 0f;

            //if (KeyboardInput.IsKeyPressed("MapEditor_Up"))
            //    GoUp = 0.5f;
            //if (KeyboardInput.IsKeyPressed("MapEditor_Down"))
            //    GoDown = 0.5f;

            //LocalPlayer.Position = new Vector3(Pos.X + vector.X + 1, Pos.Y + vector.Y + 1, Pos.Z + vector.Z + 1);
            //LocalPlayer.SetRotation(Rot.X, Rot.Y, Rot.Z, 2, false);
            ////LocalPlayer.SetHeading(Dir.Z);
            //EditorCamera.SetPosition(new Vector3(Pos.X - vector.X + rightVector.X, Pos.Y - vector.Y + rightVector.Y, Pos.Z - vector.Z + rightVector.Z + GoUp - GoDown));
            //if (RAGE.Game.Pad.IsControlPressed(0, (int)RAGE.Game.Control.Aim))
            //{
            //    float rotx = Rot.X + rightAxisY * -5f;
            //    if (rotx < -89f)
            //        rotx = -89;
            //    if (rotx > 89)
            //        rotx = 89;

            //    EditorCamera.SetRotation(new Vector3(rotx, 0.0f, Rot.Z + rightAxisX * -5.0f));
            //}
        }

        Vector3 _plrMemAxis = new Vector3();
        bool CanChangeEditor = true;
        public void StartEditor()
        {
            if (!CanChangeEditor)
                return;

            if (RAGE.Ui.Cursor.Visible)
                return;

            CanChangeEditor = false;

            LocalPlayer.FreezePosition(true);
            LocalPlayer.SetInvincible(true);
            LocalPlayer.SetVisible(false, false);
            LocalPlayer.SetCollision(false, false);

            EditorCamera = new CameraObject();
            EditorCamera.SetActive(true);
            EditorCamera.RenderScriptCam(true);

            _plrMemAxis = LocalPlayer.Position;

            GUI.EnableAllButtonsByGroup("main_menu");
            GUI.ResetCursor();

            HideHUD = true;
            RAGE.Chat.Show(false);

            RAGE.Game.Graphics.RequestStreamedTextureDict("commonmenu", false);
            while (!RAGE.Game.Graphics.HasStreamedTextureDictLoaded("commonmenu"))
                RAGE.Game.Invoker.Wait(0);

            RAGE.Game.Graphics.GetScreenResolution(ref ScreenX, ref ScreenY);
            GUI.EnableCursor();
            IsEditorActive = true;
            lstGUI.Initialize("obj_list");
            lstGUI.EnableAll();
        }

        public void StopEditor(bool exitHere)
        {
            if (!CanChangeEditor)
                return;

            CanChangeEditor = false;

            if (HighlightedEntity != null)
            {
                HighlightedEntity.FreezePosition(HighlightedEntity.Frozen);
                HighlightedEntity.ActivatePhysics();
            }

            if (exitHere)
                LocalPlayer.Position = EditorCamera.GetPosition();
            else
                LocalPlayer.Position = _plrMemAxis;

            EditorCamera.RenderScriptCam(false);
            EditorCamera.SetActive(false);

            RAGE.Game.Cam.DestroyAllCams(false);

            LocalPlayer.FreezePosition(false);
            LocalPlayer.SetInvincible(false);
            LocalPlayer.SetVisible(true, false);
            LocalPlayer.SetCollision(true, false);
            HasObjectHighlighted = false;
            HighlightedEntity = null;
            EditorCamera = null;
            IsEditorActive = false;

            Marker_X_Rotate_Highlighted = false;
            Marker_Y_Rotate_Highlighted = false;
            Marker_Z_Rotate_Highlighted = false;
            Marker_X_Move_Highlighted = false;
            Marker_Y_Move_Highlighted = false;
            Marker_Z_Move_Highlighted = false;

            Marker_X_Rotate_Clicked = false;
            Marker_Y_Rotate_Clicked = false;
            Marker_Z_Rotate_Clicked = false;
            Marker_X_Move_Clicked = false;
            Marker_Y_Move_Clicked = false;
            Marker_Z_Move_Clicked = false;
            EntityFollowMouse = false;

            GUI.DisableCursor();
            GUI.DisableAllButtons();
            lstGUI.DisableAll();
        }

        public class RageMath
        {
            public static float Determinent(Vector3 a, Vector3 b, Vector3 c)
            {
                return a.X * b.Y * c.Z + a.Y * b.Z * c.X + a.Z * b.X * c.Y - c.X * b.Y * a.Z - c.Y * b.Z * a.X - c.Z * b.X * a.Y;
            }

            public static Vector3 ClampPointToLine(Vector3 pointToClamp, Vector3 lineToClampToa, Vector3 lineToClampTob)
            {
                Vector3 clampedPoint = new Vector3();
                float minX, minY, minZ, maxX, maxY, maxZ;
                if (lineToClampToa.X <= lineToClampTob.X)
                {
                    minX = lineToClampToa.X;
                    maxX = lineToClampTob.X;
                }
                else
                {
                    minX = lineToClampTob.X;
                    maxX = lineToClampToa.X;
                }
                if (lineToClampToa.Y <= lineToClampTob.Y)
                {
                    minY = lineToClampToa.Y;
                    maxY = lineToClampTob.Y;
                }
                else
                {
                    minY = lineToClampTob.Y;
                    maxY = lineToClampToa.Y;
                }
                if (lineToClampToa.Z <= lineToClampTob.Z)
                {
                    minZ = lineToClampToa.Z;
                    maxZ = lineToClampTob.Z;
                }
                else
                {
                    minZ = lineToClampTob.Z;
                    maxZ = lineToClampToa.Z;
                }
                clampedPoint.X = (pointToClamp.X < minX) ? minX : (pointToClamp.X > maxX) ? maxX : pointToClamp.X;
                clampedPoint.Y = (pointToClamp.Y < minY) ? minY : (pointToClamp.Y > maxY) ? maxY : pointToClamp.Y;
                clampedPoint.Z = (pointToClamp.Z < minZ) ? minZ : (pointToClamp.Z > maxZ) ? maxZ : pointToClamp.Z;
                return clampedPoint;
            }

            public static Tuple<Vector3, Vector3> closestDistanceBetweenLines(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
            {
                var A = a1 - a0;
                var B = b1 - b0;
                float magA = A.Length();
                float magB = B.Length();

                var _A = A / magA;
                var _B = B / magB;

                var cross = GetCrossProduct(_A, _B);
                var denom = cross.Length() * cross.Length();

                Vector3 closest1, closest2;
                if (denom == 0)
                {
                    var d0 = GetDotProduct(_A, (b0 - a0));
                    var d1 = GetDotProduct(_A, (b1 - a0));
                    if (d0 <= 0 && 0 >= d1)
                    {
                        if (MathF.Abs(d0) < MathF.Abs(d1))
                        {
                            closest1 = a0;
                            closest2 = b0;
                            return new Tuple<Vector3, Vector3>(closest1, closest2);
                        }
                        closest1 = a0;
                        closest2 = b1;
                        return new Tuple<Vector3, Vector3>(closest1, closest2);
                    }
                    else if (d0 >= magA && magA <= d1)
                    {
                        if (MathF.Abs(d0) < MathF.Abs(d1))

                        {
                            closest1 = a1;
                            closest2 = b0;
                            return new Tuple<Vector3, Vector3>(closest1, closest2);
                        }
                        closest1 = a1;
                        closest2 = b1;
                        return new Tuple<Vector3, Vector3>(closest1, closest2);
                    }
                    closest1 = Vector3.Zero;
                    closest2 = Vector3.Zero;
                    return new Tuple<Vector3, Vector3>(closest1, closest2);
                }

                var t = (b0 - a0);
                var detA = Determinent(t, _B, cross);
                var detB = Determinent(t, _A, cross);

                var t0 = detA / denom;
                var t1 = detB / denom;

                var pA = a0 + (_A * t0);
                var pB = b0 + (_B * t1);

                if (t0 < 0)
                    pA = a0;
                else if (t0 > magA)
                    pA = a1;

                if (t1 < 0)
                    pB = b0;
                else if (t1 > magB)
                    pB = b1;

                float dot;
                if (t0 < 0 || t0 > magA)
                {
                    dot = GetDotProduct(_B, (pA - b0));
                    if (dot < 0)
                        dot = 0;
                    else if (dot > magB)
                        dot = magB;
                    pB = b0 + (_B * dot);
                }

                if (t1 < 0 || t1 > magB)
                {
                    dot = GetDotProduct(_A, (pB - a0));
                    if (dot < 0)
                        dot = 0;
                    else if (dot > magA)
                        dot = magA;
                    pA = a0 + (_A * dot);
                }

                closest1 = pA;
                closest2 = pB;
                return new Tuple<Vector3, Vector3>(closest1, closest2);
            }

            public static Vector3 GeneratePlaneNormal(Vector3 point, Vector3 rotation)
            {
                Vector3 v2 = new Vector3(point.X, point.Y, point.Z + 1000f);
                Vector3 v3 = new Vector3(point.X - 1000f, point.Y, point.Z);

                v2 -= point;
                v2 = RotateZ(v2, rotation.Z);
                v2 += point;

                v3 -= point;
                v3 = RotateZ(v3, rotation.Z);
                v3 += point;

                v2 -= point;
                v2 = RotateX(v2, rotation.X);
                v2 += point;

                v3 -= point;
                v3 = RotateX(v3, rotation.X);
                v3 += point;

                v2 -= point;
                v2 = RotateY(v2, rotation.Y);
                v2 += point;

                v3 -= point;
                v3 = RotateY(v3, rotation.Y);
                v3 += point;

                Vector3 four = v2 - point;
                Vector3 five = v3 - point;

                Vector3 cross = GetCrossProduct(four, five);
                cross.Normalize();
                return cross;
            }

            public static float AngleBetweenVectors(Vector3 a, Vector3 b, Vector3 planeNorm)
            {
                float angle = GetDotProduct(a, b);
                angle = MathF.Acos(angle);
                Vector3 cross = GetCrossProduct(a, b);
                cross.Normalize();
                if (GetDotProduct(planeNorm, cross) < 0)
                    angle = -angle;
                return angle;
            }

            public static Vector3 NearestPointOnPlane(Vector3 planePos, Vector3 planeNorm, Vector3 point)
            {
                Vector3 v = point - planePos;
                float dist = GetDotProduct(v, planeNorm.Normalized);
                return point - (planeNorm.Normalized * dist);
            }

            public static Vector3 NearestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
            {
                Vector3 v = end - start;
                float len = v.Length();
                v.Normalize();

                Vector3 ln = point - start;
                float d = GetDotProduct(ln, v);
                //d = RageMath.ClampValue(d, 0f, len);
                return start + v * d;
            }

            public static bool LineIntersectingBox(Vector3 LineStart, Vector3 LineEnd, Vector3 min, Vector3 max)
            {
                float tmin = (min.X - LineStart.X) / LineEnd.X;
                float tmax = (max.X - LineStart.X) / LineEnd.X;

                if (tmin > tmax)
                {
                    float temp = tmin;
                    tmin = tmax;
                    tmax = temp;
                }

                float tymin = (min.Y - LineStart.Y) / LineEnd.Y;
                float tymax = (max.Y - LineStart.Y) / LineEnd.Y;

                if (tymin > tymax)
                {
                    float temp = tymin;
                    tymin = tymax;
                    tymax = temp;
                }

                if ((tmin > tymax) || (tymin > tmax))
                    return false;

                if (tymin > tmin)
                    tmin = tymin;

                if (tymax < tmax)
                    tmax = tymax;

                float tzmin = (min.Z - LineStart.Z) / LineEnd.Z;
                float tzmax = (max.Z - LineStart.Z) / LineEnd.Z;

                if (tzmin > tzmax)
                {
                    float temp = tzmin;
                    tzmin = tzmax;
                    tzmax = temp;
                }

                if ((tmin > tzmax) || (tzmin > tmax))
                    return false;

                if (tzmin > tmin)
                    tmin = tzmin;

                if (tzmax < tmax)
                    tmax = tzmax;

                return true;
            }

            public static bool LineIntersectingPlane(Vector3 PlaneNorm, Vector3 PlanePoint, Vector3 LineStart, Vector3 LineEnd, ref Vector3 HitPosition)
            {
                Vector3 u = LineEnd - LineStart;
                float dot = GetDotProduct(PlaneNorm, u);
                if (MathF.Abs(dot) > float.Epsilon)
                {
                    Vector3 w = LineStart - PlanePoint;
                    float fac = -GetDotProduct(PlaneNorm, w) / dot;
                    u = u * fac;
                    HitPosition = LineStart + u;
                    return true;
                }
                return false;
            }

            public static Vector3 GetOffsetFromCameraCoord(Vector3 offset)
            {
                Vector3 Pos = EditorCamera.GetPosition();
                Vector3 Dir = EditorCamera.GetDirection();
                Vector3 Rot = EditorCamera.GetRotation();

                Vector3 upVector = new Vector3(0, 0, 1);
                Vector3 rightVector = GetCrossProduct(GetNormalizedVector(Dir), GetNormalizedVector(upVector));
                rightVector.X *= offset.X;
                rightVector.Y *= offset.Y;
                rightVector.Z *= offset.Z;
                return new Vector3(Pos.X + rightVector.X, Pos.Y + rightVector.Y, Pos.Z + rightVector.Z);
            }

            //public static Tuple<float, float> ProcessCoordinates(float x, float y)
            //{
            //    float relativeX = (x * 2f) - 1f;
            //    float relativeY = (y * 2f) - 1f;
            //    return new Tuple<float, float>(relativeX, relativeY);
            //}

            //public static Vector3 ScreenToWorld(float x, float y)
            //{
            //    Tuple<float, float> vex = ProcessCoordinates(x, y);
            //    x = vex.Item1;
            //    y = vex.Item2;
            //    Vector3 pos = EditorCamera.GetPosition();
            //    Vector3 rot = EditorCamera.GetRotation(0);
            //    Vector3 forward = RotationToDirection(rot);
            //    Vector3 rotup = rot + new Vector3(10, 0, 0);
            //    Vector3 rotdown = rot + new Vector3(-10, 0, 0);
            //    Vector3 rotleft = rot + new Vector3(0, 0, -10);
            //    Vector3 rotright = rot + new Vector3(0, 0, 10);

            //    Vector3 camRight = RotationToDirection(rotright) - RotationToDirection(rotleft);
            //    Vector3 camUp = RotationToDirection(rotup) - RotationToDirection(rotdown);
            //    float rollRad = -DegreesToRad(rot.Y);
            //    Vector3 camRightRoll = (camRight * MathF.Cos(rollRad)) - (camUp * MathF.Sin(rollRad));
            //    Vector3 camUpRoll = (camRight * MathF.Sin(rollRad)) + (camUp * MathF.Cos(rollRad));
            //    Vector3 point3D = pos + (forward * 10) + camRightRoll + camUpRoll;
            //    Vector3 point2D = WorldToScreen(point3D);
            //    if (point2D == null)
            //        return pos + (forward * 10);

            //    Vector3 point3DZero = pos + (forward * 10);
            //    Vector3 point2DZero = WorldToScreen(point3DZero);

            //    if (point2DZero == null)
            //        return pos + (forward * 10);

            //    if (Math.Abs(point2D.X - point2DZero.X) < float.Epsilon || Math.Abs(point2D.Y - point2DZero.Y) < float.Epsilon)
            //        return pos + (forward * 10);

            //    float scaleX = (x - point2DZero.X) / (point2D.X - point2DZero.X);
            //    float scaleY = (y - point2DZero.Y) / (point2D.Y - point2DZero.Y);

            //    Vector3 point3Dret = pos + (forward * 10) + (camRightRoll * scaleX) + (camUpRoll * scaleY);
            //    return point3Dret;
            //}

            public static Vector3 GetDirectionFromTwoVectors(Vector3 a, Vector3 b)
            {
                return b - a;
            }

            public static Vector3 WorldToScreen(Vector3 vec)
            {
                float x = 0;
                float y = 0;
                if (RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(vec.X, vec.Y, vec.Z, ref x, ref y))
                    return new Vector3((x - 0.5f) * 2f, (y - 0.5f) * 2f, 0f);
                else
                    return null;
            }

            public static Vector3 WorldToScreen2(Vector3 vec)
            {
                float x = 0;
                float y = 0;
                if (RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(vec.X, vec.Y, vec.Z, ref x, ref y))
                    return new Vector3(x, y, 0f);
                else
                    return null;
            }

            public static Vector3 RotateX(Vector3 point, float angle)
            {
                Vector3 f1 = new Vector3(1, 0, 0);
                Vector3 f2 = new Vector3(0, MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)));
                Vector3 f3 = new Vector3(0, MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)));

                Vector3 final = new Vector3();
                final.X = (f1.X * point.X + f1.Y * point.Y + f1.Z * point.Z);
                final.Y = (f2.X * point.X + f2.Y * point.Y + f2.Z * point.Z);
                final.Z = (f3.X * point.X + f3.Y * point.Y + f3.Z * point.Z);

                return final;
            }

            public static Vector3 RotateZ(Vector3 point, float angle)
            {
                Vector3 f7 = new Vector3(MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)), 0);
                Vector3 f8 = new Vector3(MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)), 0);
                Vector3 f9 = new Vector3(0, 0, 1);

                Vector3 final = new Vector3();
                final.X = (f7.X * point.X + f7.Y * point.Y + f7.Z * point.Z);
                final.Y = (f8.X * point.X + f8.Y * point.Y + f8.Z * point.Z);
                final.Z = (f9.X * point.X + f9.Y * point.Y + f9.Z * point.Z);
                return final;
            }

            public static Vector3 RotateY(Vector3 point, float angle)
            {


                Vector3 f4 = new Vector3(MathF.Cos(DegreesToRad(angle)), 0, MathF.Sin(DegreesToRad(angle)));
                Vector3 f5 = new Vector3(0, 1, 0);
                Vector3 f6 = new Vector3(-MathF.Sin(DegreesToRad(angle)), 0, MathF.Cos(DegreesToRad(angle)));

                Vector3 final = new Vector3();
                final.X = (f4.X * point.X + f4.Y * point.Y + f4.Z * point.Z);
                final.Y = (f5.X * point.X + f5.Y * point.Y + f5.Z * point.Z);
                final.Z = (f6.X * point.X + f6.Y * point.Y + f6.Z * point.Z);
                return final;
            }


            public static bool LineIntersectingCircle(Vector3 CircleCenter, Vector3 CircleRotation, float CircleRadius, Vector3 LineStart, Vector3 LineEnd, ref Vector3 HitPosition, float threshold, ref Vector3 planeNorm)
            {
                Vector3 v2 = new Vector3(CircleCenter.X, CircleCenter.Y, CircleCenter.Z + CircleRadius);
                Vector3 v3 = new Vector3(CircleCenter.X - CircleRadius, CircleCenter.Y, CircleCenter.Z);

                v2 -= CircleCenter;
                v2 = RotateZ(v2, CircleRotation.Z);
                v2 += CircleCenter;

                v3 -= CircleCenter;
                v3 = RotateZ(v3, CircleRotation.Z);
                v3 += CircleCenter;

                v2 -= CircleCenter;
                v2 = RotateX(v2, CircleRotation.X);
                v2 += CircleCenter;

                v3 -= CircleCenter;
                v3 = RotateX(v3, CircleRotation.X);
                v3 += CircleCenter;

                v2 -= CircleCenter;
                v2 = RotateY(v2, CircleRotation.Y);
                v2 += CircleCenter;

                v3 -= CircleCenter;
                v3 = RotateY(v3, CircleRotation.Y);
                v3 += CircleCenter;

                //RAGE.Game.Graphics.DrawPoly(CircleCenter.X, CircleCenter.Y, CircleCenter.Z, v2.X, v2.Y, v2.Z, v3.X, v3.Y, v3.Z, 0, 255, 0, 255);

                Vector3 four = v2 - CircleCenter;
                Vector3 five = v3 - CircleCenter;

                Vector3 cross = GetCrossProduct(four, five);
                planeNorm = new Vector3(cross.X, cross.Y, cross.Z);
                cross.Normalize();
                bool hit = LineIntersectingPlane(cross, CircleCenter, LineStart, LineEnd, ref HitPosition);
                if (hit)
                {
                    if (HitPosition.DistanceTo(CircleCenter) <= CircleRadius + threshold)
                    {
                        return true;
                    }
                }
                return false;
            }

            public static bool LineIntersectingSphere(Vector3 StartLine, Vector3 LineEnd, Vector3 SphereCenter, float SphereRadius)
            {
                Vector3 d = LineEnd - StartLine;
                Vector3 f = StartLine - SphereCenter;

                float c = GetDotProduct(f, f) - SphereRadius * SphereRadius;
                if (c <= 0f)
                    return true;

                float b = GetDotProduct(f, d);
                if (b >= 0f)
                    return false;

                float a = GetDotProduct(d, d);
                if (b * b - a * c < 0f)
                    return false;

                return true;
            }

            public static float DegreesToRad(float deg)
            {
                return MathF.PI * deg / 180.0f;
            }

            public static float RadToDegrees(float rad)
            {
                return rad * (180.0f / MathF.PI);
            }

            public static Vector3 Multiply(Vector3 a, Vector3 b)
            {
                return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
            }

            public static Vector3 Divide(Vector3 a, Vector3 b)
            {
                return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
            }

            public static Vector3 Projection(Vector3 a, Vector3 b)
            {
                return Multiply(Divide(Multiply(a, b), Multiply(b, b)), b);
            }

            public static Vector3 RotationToDirection(Vector3 rot)
            {
                float num = rot.Z * 0.0174532924f;
                float num2 = rot.X * 0.0174532924f;
                float num3 = MathF.Abs(MathF.Cos(num2));
                return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
            }

            public static Vector3 DirectionToRotation(Vector3 dir, float RotationY)
            {
                Vector3 dirnew = GetNormalizedVector(dir);
                Vector3 vector1 = new Vector3(dirnew.X, dirnew.Y, 0f);
                Vector3 vector2 = new Vector3(dirnew.Z, vector1.Length(), 0f);
                Vector3 vector3 = GetNormalizedVector(vector2);
                return new Vector3((MathF.Atan2(vector3.X, vector3.Y) * 57.295779513082323f), RotationY, (System.MathF.Atan2(dirnew.X, dirnew.Y) * -57.295779513082323f));
            }

            public static Vector3 GetNormalizedVector(Vector3 vec)
            {
                float mag = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
                return new Vector3(vec.X / mag, vec.Y / mag, vec.Z / mag);
            }

            public static Vector3 GetCrossProduct(Vector3 v1, Vector3 v2)
            {
                Vector3 vec = new Vector3();
                vec.X = v1.Y * v2.Z - v1.Z * v2.Y;
                vec.Y = v1.Z * v2.X - v1.X * v2.Z;
                vec.Z = v1.X * v2.Y - v1.Y * v2.X;
                return vec;
            }

            public static float ClampValue(float num, float min, float max)
            {
                return num <= min ? min : num >= max ? max : num;
            }

            public static float GetDotProduct(Vector3 v1, Vector3 v2)
            {
                return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            }

            public static float GetVectorLength(Vector3 vec)
            {
                return MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
            }

            public static float GetAngleBetweenVectors(Vector3 v1, Vector3 v2)
            {
                return MathF.Acos(GetDotProduct(GetNormalizedVector(v1), GetNormalizedVector(v2)));
            }

            public static float GetDistanceBetweenVectors(Vector3 v1, Vector3 v2)
            {
                return MathF.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y) + (v1.Z - v2.Z) * (v1.Z - v2.Z));
            }
        }

        public class Raycasting
        {
            public class RaycastHit
            {
                public int Ray = -1;
                public bool Hit = false;
                public Vector3 EndCoords = new Vector3();
                public Vector3 SurfaceNormal = new Vector3();
                public int EntityHit = -1;
                public int MaterialHash = -1;
                public int ShapeResult = -1;
            }

            public static RaycastHit RaycastFromTo(Vector3 from, Vector3 to, int ignoreEntity, int flags)
            {
                int ray = RAGE.Game.Shapetest.StartShapeTestRay(from.X, from.Y, from.Z, to.X, to.Y, to.Z, flags, ignoreEntity, 0);
                RaycastHit cast = new RaycastHit();
                int curtemp = 0;
                cast.ShapeResult = RAGE.Game.Shapetest.GetShapeTestResultEx(ray, ref curtemp, cast.EndCoords, cast.SurfaceNormal, ref cast.MaterialHash, ref cast.EntityHit);
                cast.Hit = Convert.ToBoolean(curtemp);
                return cast;
            }
        }

        public class CameraObject
        {
            public int Cam = -1;

            public CameraObject()
            {
                Cam = RAGE.Game.Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
                Vector3 cord = RAGE.Game.Cam.GetGameplayCamCoord();
                RAGE.Game.Cam.SetCamCoord(Cam, cord.X, cord.Y, cord.Z);
                Vector3 rot = RAGE.Game.Cam.GetGameplayCamRot(2);
                RAGE.Game.Cam.SetCamRot(Cam, rot.X, rot.Y, rot.Z, 2);
                SetFOV(RAGE.Game.Cam.GetGameplayCamFov());
            }

            public void SetActive(bool val)
            {
                RAGE.Game.Cam.SetCamActive(Cam, val);
            }

            public void RenderScriptCam(bool val)
            {
                RAGE.Game.Cam.RenderScriptCams(val, false, 0, true, false, 0);
            }

            public void SetFOV(float val)
            {
                RAGE.Game.Cam.SetCamFov(Cam, val);
            }

            public void SetPosition(Vector3 pos)
            {
                RAGE.Game.Cam.SetCamCoord(Cam, pos.X, pos.Y, pos.Z);
            }

            public void SetRotation(Vector3 rot)
            {
                RAGE.Game.Cam.SetCamRot(Cam, rot.X, rot.Y, rot.Z, 2);
            }

            public Vector3 GetDirection()
            {
                return RageMath.RotationToDirection(GetRotation());
            }

            public Vector3 GetPosition()
            {
                return RAGE.Game.Cam.GetCamCoord(Cam);
            }

            public Vector3 GetRotation(int order = 2)
            {
                return RAGE.Game.Cam.GetCamRot(Cam, order);
            }

            ~CameraObject()
            {
                RAGE.Game.Cam.DestroyCam(Cam, false);
            }

            public Raycasting.RaycastHit GetHitCoord()
            {
                Vector3 position = RAGE.Game.Cam.GetCamCoord(Cam);
                Vector3 direction = RAGE.Game.Cam.GetCamRot(Cam, 2);
                Vector3 farAwar = new Vector3((direction.X * 150) + position.X, (direction.Y * 150) + position.Y, (direction.Z * 150) + position.Z);
                return Raycasting.RaycastFromTo(position, farAwar, LocalPlayer.Handle, 1 | 16 | 256);
            }
        }

        public class KeyboardInput
        {
            public static int KeyToCode(string key)
            {
                if (key == "M")
                    return 0x4D;
                else if (key == "W")
                    return 0x57;
                else if (key == "A")
                    return 0x41;
                else if (key == "S")
                    return 0x53;
                else if (key == "D")
                    return 0x44;
                else if (key == "Q")
                    return 0x51;
                else if (key == "E")
                    return 0x45;
                else if (key == "N")
                    return 0x4E;
                else if (key == "LCtrl")
                    return 0xA2;
                else if (key == "LShift")
                    return 0xA0;
                else if (key == "F")
                    return 0x46;
                else if (key == "G")
                    return 0x47;
                return -1;
            }

            public static Dictionary<string, string> KeyBinds = new Dictionary<string, string>()
            {
                { "M", "MapEditor_Toggle" },
                { "W", "MapEditor_Forward" },
                { "A", "MapEditor_Left" },
                { "S", "MapEditor_Back" },
                { "D", "MapEditor_Right" },
                { "Q", "MapEditor_Down" },
                { "E", "MapEditor_Up" },
                { "LCtrl", "MapEditor_SlowMode" },
                { "LShift", "MapEditor_FastMode" },
                { "F", "MapEditor_LookAt" },
                { "G", "MapEditor_GoTo" }
            };

            public static Dictionary<int, bool> KeyBindsStates_Prev = new Dictionary<int, bool>();
            public static Dictionary<int, bool> KeyBindsStates_Curr = new Dictionary<int, bool>();

            public static bool IsKeyJustReleased(string key)
            {
                int code = KeyToCode(KeyBinds.FirstOrDefault(x => x.Value == key).Key);
                if (code == -1)
                    return false;

                if (KeyBindsStates_Curr.ContainsKey(code))
                    if (KeyBindsStates_Prev[code] == true && KeyBindsStates_Curr[code] == false)
                        return true;
                return false;
            }

            public static bool IsKeyPressed(string key)
            {
                int code = KeyToCode(KeyBinds.FirstOrDefault(x => x.Value == key).Key);
                if (code == -1)
                    return false;

                if (KeyBindsStates_Curr.ContainsKey(code))
                    if (KeyBindsStates_Curr[code])
                        return true;
                return false;
            }

            public static bool IsKeyJustPressed(string key)
            {
                int code = KeyToCode(key);
                if (code == -1)
                    return false;

                if (KeyBindsStates_Curr.ContainsKey(code))
                    if (KeyBindsStates_Prev[code] == false && KeyBindsStates_Curr[code] == true)
                        return true;
                return false;
            }

            public static void ProcessInput()
            {
                foreach (var key_pair in KeyBinds)
                {
                    if (RAGE.Input.IsDown(KeyToCode(key_pair.Key)))
                    {
                        if (KeyBindsStates_Prev.ContainsKey(KeyToCode(key_pair.Key)))
                        {
                            KeyBindsStates_Prev[KeyToCode(key_pair.Key)] = KeyBindsStates_Curr[KeyToCode(key_pair.Key)];
                            KeyBindsStates_Curr[KeyToCode(key_pair.Key)] = true;
                        }
                        else
                        {
                            KeyBindsStates_Prev[KeyToCode(key_pair.Key)] = false;
                            KeyBindsStates_Curr[KeyToCode(key_pair.Key)] = true;
                        }
                    }
                    else
                    {
                        if (KeyBindsStates_Prev.ContainsKey(KeyToCode(key_pair.Key)))
                        {
                            KeyBindsStates_Prev[KeyToCode(key_pair.Key)] = KeyBindsStates_Curr[KeyToCode(key_pair.Key)];
                            KeyBindsStates_Curr[KeyToCode(key_pair.Key)] = false;
                        }
                        else
                        {
                            KeyBindsStates_Prev[KeyToCode(key_pair.Key)] = false;
                            KeyBindsStates_Curr[KeyToCode(key_pair.Key)] = false;
                        }
                    }
                }
            }
        }

        public static void DrawSomeMarker(Vector3 pos, RGBA col, float scale)
        {
            RAGE.Events.CallLocal("drawMarker", 28, pos.X, pos.Y, pos.Z,
                    0f, 0f, 0f, 0f, 0f, 0f, scale, scale, scale,
                    col.Red, col.Green, col.Blue, col.Alpha,
                    false, false, 0, false, "", "", false);
        }

        public static void DrawSomeMarker2(Vector3 pos, Vector3 rot, float scale)
        {
            RAGE.Events.CallLocal("drawMarker", 27, pos.X, pos.Y, pos.Z,
                    0f, 0f, 0f, rot.X, rot.Y, rot.Z, scale, scale, scale,
                    255, 255, 255, 255,
                    false, false, 0, false, "", "", false);
        }

        public class Marker
        {
            public int Type;
            public Vector3 Position;
            public Vector3 Direction;
            public Vector3 Rotation;
            public Vector3 Scale;
            public RGBA Color;

            public Marker(int type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, RGBA col)
            {
                Type = type;
                Position = pos;
                Direction = dir;
                Rotation = rot;
                Color = col;
                Scale = scale;
            }

            public void Draw()
            {
                RAGE.Events.CallLocal("drawMarker", Type, Position.X, Position.Y, Position.Z,
                    Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                    Color.Red, Color.Green, Color.Blue, Color.Alpha,
                    false, false, 2, false, "", "", false);
            }
        }

        class AxisMarker
        {
            public Marker Marker;

            public AxisMarker(RGBA color, int type)
            {
                Marker = new Marker(type, new Vector3(), new Vector3(), new Vector3(), new Vector3(), color);
            }

            public bool IsRaycasted(ref Vector3 hitPoint, ref Vector3 norm, float threshold = 0.1f, bool ignoreDistance = false)
            {
                if (!IsEditorActive)
                    return false;

                Vector3 test1 = EditorCamera.GetPosition();
                Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                Vector3 test3 = test2 - test1;
                Vector3 from = test1 + test3 * 0.05f;
                Vector3 to = test1 + test3 * 1000f;

                if (!ignoreDistance)
                {
                    if (RageMath.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), Marker.Scale.X / 2f, from, to, ref hitPoint, threshold, ref norm))
                        if (Marker.Position.DistanceTo(hitPoint) >= (Marker.Scale.X / 2f) * 0.775f - threshold)
                            return true;
                }
                else
                {
                    if (RageMath.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), threshold, from, to, ref hitPoint, 0f, ref norm))
                        return true;
                }
                return false;
            }

            public bool IsSphereCasted()
            {
                Vector3 test1 = EditorCamera.GetPosition();
                Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                Vector3 test3 = test2 - test1;
                Vector3 from = test1 + test3 * 0.05f;
                Vector3 to = test1 + test3 * 1000f;

                return RageMath.LineIntersectingSphere(from, to, Marker.Position, Marker.Scale.X);
            }

            public void Draw()
            {
                if (!IsEditorActive)
                    return;
                Marker.Draw();
                Vector3 v = RageMath.WorldToScreen2(Marker.Position);
                if (v != null)
                {
                    float dist = Marker.Position.DistanceTo(EditorCamera.GetPosition());
                    if (Marker.Type == 28)
                        RAGE.Game.Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * ((float)ScreenY / (float)ScreenX), Marker.Scale.X * 4 / dist, 0, (int)Marker.Color.Red, (int)Marker.Color.Green, (int)Marker.Color.Blue, (int)Marker.Color.Alpha, 0);
                }
            }
        }

        AxisMarker Marker_X_Rotate = new AxisMarker(new RGBA(255, 0, 0), 27);
        AxisMarker Marker_Y_Rotate = new AxisMarker(new RGBA(0, 255, 0), 27);
        AxisMarker Marker_Z_Rotate = new AxisMarker(new RGBA(0, 0, 255), 27);
        AxisMarker Marker_X_Move = new AxisMarker(new RGBA(255, 0, 0), 28);
        AxisMarker Marker_Y_Move = new AxisMarker(new RGBA(0, 255, 0), 28);
        AxisMarker Marker_Z_Move = new AxisMarker(new RGBA(0, 0, 255), 28);
        public static bool Marker_X_Rotate_Highlighted = false;
        public static bool Marker_Y_Rotate_Highlighted = false;
        public static bool Marker_Z_Rotate_Highlighted = false;
        public static bool Marker_X_Move_Highlighted = false;
        public static bool Marker_Y_Move_Highlighted = false;
        public static bool Marker_Z_Move_Highlighted = false;

        public static bool Marker_X_Rotate_Clicked = false;
        public static bool Marker_Y_Rotate_Clicked = false;
        public static bool Marker_Z_Rotate_Clicked = false;
        public static bool Marker_X_Move_Clicked = false;
        public static bool Marker_Y_Move_Clicked = false;
        public static bool Marker_Z_Move_Clicked = false;
        float tempX = 0.5f;
        float tempY = 0.5f;

        float prevX = 0.5f;
        float prevY = 0.5f;
        float prevAngle = 0f;
        Vector3 originalHitPoint = new Vector3();
        //Vector3 diffP = new Vector3();
        RGBA HighlightColor_Edge = new RGBA(255, 255, 255, 255);
        RGBA HighlightColor_Full = new RGBA(255, 255, 255, 35);

        public static Dictionary<RAGEInput.VirtualKey, bool> KeyState = new Dictionary<RAGEInput.VirtualKey, bool>()
        {
            {RAGEInput.VirtualKey.KEY_A, false },
            {RAGEInput.VirtualKey.KEY_B, false },
            {RAGEInput.VirtualKey.KEY_C, false },
            {RAGEInput.VirtualKey.KEY_D, false },
            {RAGEInput.VirtualKey.KEY_E, false },
            {RAGEInput.VirtualKey.KEY_F, false },
            {RAGEInput.VirtualKey.KEY_G, false },
            {RAGEInput.VirtualKey.KEY_H, false },
            {RAGEInput.VirtualKey.KEY_I, false },
            {RAGEInput.VirtualKey.KEY_J, false },
            {RAGEInput.VirtualKey.KEY_K, false },
            {RAGEInput.VirtualKey.KEY_L, false },
            {RAGEInput.VirtualKey.KEY_M, false },
            {RAGEInput.VirtualKey.KEY_N, false },
            {RAGEInput.VirtualKey.KEY_O, false },
            {RAGEInput.VirtualKey.KEY_P, false },
            {RAGEInput.VirtualKey.KEY_Q, false },
            {RAGEInput.VirtualKey.KEY_R, false },
            {RAGEInput.VirtualKey.KEY_S, false },
            {RAGEInput.VirtualKey.KEY_T, false },
            {RAGEInput.VirtualKey.KEY_U, false },
            {RAGEInput.VirtualKey.KEY_V, false },
            {RAGEInput.VirtualKey.KEY_W, false },
            {RAGEInput.VirtualKey.KEY_X, false },
            {RAGEInput.VirtualKey.KEY_Y, false },
            {RAGEInput.VirtualKey.KEY_Z, false },

            {RAGEInput.VirtualKey.KEY_0, false },
            {RAGEInput.VirtualKey.KEY_1, false },
            {RAGEInput.VirtualKey.KEY_2, false },
            {RAGEInput.VirtualKey.KEY_3, false },
            {RAGEInput.VirtualKey.KEY_4, false },
            {RAGEInput.VirtualKey.KEY_5, false },
            {RAGEInput.VirtualKey.KEY_6, false },
            {RAGEInput.VirtualKey.KEY_7, false },
            {RAGEInput.VirtualKey.KEY_8, false },
            {RAGEInput.VirtualKey.KEY_9, false },

            {RAGEInput.VirtualKey.OEM_MINUS, false }
        };

        public static bool BackPress = false;
        public static List<string> GetKeyInput()
        {
            List<string> lst = new List<string>();
            foreach (RAGEInput.VirtualKey c in KeyState.Keys.ToList())
            {
                if (!RAGE.Input.IsDown((int)c) && KeyState[c])
                {
                    string r = c.ToString();
                    if (r == "OEM_MINUS")
                    {
                        r = "_";
                    }
                    else if (r.Length > 1)
                        r = r.Replace("KEY_", "");
                    lst.Add(r);
                }

                if (RAGE.Input.IsDown((int)c))
                {
                    KeyState[c] = true;
                }
                else
                {
                    KeyState[c] = false;
                }
            }

            if (!RAGE.Input.IsDown((int)RAGEInput.VirtualKey.BACK) && BackPress)
            {
                GUI.ButtonObject obj = GUI.GetButtonById("previewer_group");
                if (obj != null)
                {
                    if (obj.IsEnabled)
                    {
                        if (obj.TextObject.Text.Length > 0)
                            obj.TextObject.Text = obj.TextObject.Text.Remove(obj.TextObject.Text.Length - 1);
                    }
                }
                obj = GUI.GetButtonById("savemap_name");
                if(obj != null)
                {
                    if(obj.IsEnabled)
                    {
                        if (obj.TextObject.Text.Length > 0)
                            obj.TextObject.Text = obj.TextObject.Text.Remove(obj.TextObject.Text.Length - 1);
                    }
                }
                obj = GUI.GetButtonById("loadmap_name");
                if (obj != null)
                {
                    if (obj.IsEnabled)
                    {
                        if (obj.TextObject.Text.Length > 0)
                            obj.TextObject.Text = obj.TextObject.Text.Remove(obj.TextObject.Text.Length - 1);
                    }
                }
                obj = GUI.GetButtonById("lst_search");
                if (obj != null)
                {
                    if (obj.IsEnabled && cantype)
                    {
                        if (obj.TextObject.Text.Length > 0)
                        {
                            obj.TextObject.Text = obj.TextObject.Text.Remove(obj.TextObject.Text.Length - 1);
                            string s = obj.TextObject.Text.ToLower();
                            List<string> xd = new List<string>();
                            foreach (string str in Objects.Hashes)
                            {
                                if (xd.Count > 50)
                                    break;
                                if (str.Contains(s))
                                {
                                    if (!xd.Contains(str))
                                    {
                                        xd.Add(str);
                                    }
                                }
                            }

                            lstGUI.objs.Clear();
                            lstGUI.objs.AddRange(xd);
                            lstGUI.index = 0;
                            if (xd.Count < lstGUI.btns.Count)
                            {
                                int i = 0;
                                for (i = xd.Count; i < xd.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = xd[i];
                                    lstGUI.index++;
                                }
                                for (; i < lstGUI.btns.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = "";
                                    lstGUI.index++;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < lstGUI.btns.Count; i++)
                                {
                                    lstGUI.btns[lstGUI.btns.Count - 1 - i].TextObject.Text = xd[i];
                                    lstGUI.index++;
                                }
                            }
                        }
                    }
                }
            }

            if (RAGE.Input.IsDown((int)RAGEInput.VirtualKey.BACK))
            {
                BackPress = true;
            }
            else
            {
                BackPress = false;
            }

            return lst;
        }

        void SetHighlightEdgeColor(RGBA col)
        {

        }

        void SetHighlightColor(RGBA col)
        {

        }

        bool isObjectMouseOver = false;
        bool EntityFollowMouse = false;
        bool IsPreviewerActive = false;
        public static int PreviewIndex = 0;
        bool PrevM = false;
        bool PrevM2 = false;
        bool PrevM3 = false;
        public static float ClampDistance = 50f;
        RAGE.Elements.MapObject PreviewObj = null;
        Vector3 PreviewSize = new Vector3();
        public void OnEntityStreamIn(RAGE.Elements.Entity entity)
        {
            //RAGE.Chat.Output("Ent streamin");
            RAGE.Elements.GameEntity ent = (RAGE.Elements.GameEntity)entity;
            if (ent != null)
            {
                MapEditorObject obj = MapEditorObjectPool.GetMapEditorObjectByHandle(ent.Handle);
                if (obj != null)
                {
                    obj.SetRotation(obj.Rotation, 2);
                }
            }
        }

        public void Tick(List<Events.TickNametagData> nametags)
        {
            if (HideHUD)
                RAGE.Game.Ui.HideHudAndRadarThisFrame();
            if (IsEditorActive)
            {
                RAGE.Game.Pad.DisableControlAction(0, 199, true);
                RAGE.Game.Ui.HideHudComponentThisFrame(19);
            }
            KeyboardInput.ProcessInput();
            GUI.Update();
            lstGUI.Draw();
            if (IsGroupHover)
            {
                List<string> lst = GetKeyInput();
                if (GUI.GetButtonById("previewer_group").TextObject.Text == "Enter Group Name")
                    GUI.GetButtonById("previewer_group").TextObject.Text = "";
                foreach (string c in lst)
                {
                    if (GUI.GetButtonById("previewer_group").TextObject.Text.Length < 15)
                        GUI.GetButtonById("previewer_group").TextObject.Text += c;
                }
            }
            if(IsSaveHover)
            {
                List<string> lst = GetKeyInput();
                if (GUI.GetButtonById("savemap_name").TextObject.Text == "Enter Map Name")
                    GUI.GetButtonById("savemap_name").TextObject.Text = "";
                foreach (string c in lst)
                {
                    if (GUI.GetButtonById("savemap_name").TextObject.Text.Length < 15)
                        GUI.GetButtonById("savemap_name").TextObject.Text += c;
                }
            }
            if(IsLoadHover)
            {
                List<string> lst = GetKeyInput();
                if (GUI.GetButtonById("loadmap_name").TextObject.Text == "Enter Map Name")
                    GUI.GetButtonById("loadmap_name").TextObject.Text = "";
                foreach (string c in lst)
                {
                    if (GUI.GetButtonById("loadmap_name").TextObject.Text.Length < 15)
                        GUI.GetButtonById("loadmap_name").TextObject.Text += c;
                }
            }
            if (PrevM == true && !IsGroupHover)
            {
                if (GUI.GetButtonById("previewer_group").TextObject.Text == "")
                    GUI.GetButtonById("previewer_group").TextObject.Text = "Enter Group Name";
            }
            if (PrevM2 == true && !IsSaveHover)
            {
                if (GUI.GetButtonById("savemap_name").TextObject.Text == "")
                    GUI.GetButtonById("savemap_name").TextObject.Text = "Enter Map Name";
            }
            if(PrevM3 == true && !IsLoadHover)
            {
                if (GUI.GetButtonById("loadmap_name").TextObject.Text == "")
                    GUI.GetButtonById("loadmap_name").TextObject.Text = "Enter Map Name";
            }

            PrevM = IsGroupHover;
            PrevM2 = IsSaveHover;
            PrevM3 = IsLoadHover;
            if (IsPreviewerActive)
            {
                LocalPlayer.Position = new Vector3(-2435.6f, -1193.6f, 1241.9f);
                EditorCamera.SetPosition(new Vector3(-2435.6f, -1193.6f, 1241.9f));
                Vector3 dir = RageMath.GetDirectionFromTwoVectors(EditorCamera.GetPosition(), new Vector3(-2430.6f, -1198.6f, 1236.9f));
                Vector3 rot = RageMath.DirectionToRotation(dir, 0f);
                EditorCamera.SetRotation(rot);
                if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollUp))
                {
                    if (PreviewObj != null)
                    {
                        if (FavoriteObjects.ContainsKey(PreviewObj.Model))
                        {
                            if (GUI.GetButtonById("previewer_group").TextObject.Text != "Enter Group Name")
                                FavoriteObjects[PreviewObj.Model] = GUI.GetButtonById("previewer_group").TextObject.Text;
                        }
                    }
                    IsGroupHover = false;

                    if (PreviewObj != null)
                        PreviewObj.Destroy();

                    PreviewIndex++;
                    if (PreviewIndex >= Objects.Hashes.Count)
                        PreviewIndex = 0;

                    int cnt = 0;
                    while (!RAGE.Game.Streaming.IsModelInCdimage(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])) || !RAGE.Game.Streaming.IsModelValid(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])))
                    {
                        PreviewIndex++;
                        if (PreviewIndex >= Objects.Hashes.Count)
                            PreviewIndex = 0;
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        RAGE.Game.Invoker.Wait(0);
                        RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                        RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                        if (cnt > 2)
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Indexing... (" + cnt + ")\n(Corrupt Model)");
                        else
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Indexing...");
                        RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                        RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                        RAGE.Game.Ui.SetTextCentre(true);
                        RAGE.Game.Ui.SetTextJustification(0);
                        RAGE.Game.Ui.SetTextFont(0);
                        RAGE.Game.Ui.SetTextDropShadow();
                        RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        GUI.Update();
                        RAGE.Game.Ui.HideHudAndRadarThisFrame();
                        cnt++;
                    }

                    RAGE.Game.Utils.Settimera(0);
                    RAGE.Game.Streaming.RequestModel(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]));
                    while (!RAGE.Game.Streaming.HasModelLoaded(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])))
                    {
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        RAGE.Game.Invoker.Wait(0);
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        GUI.Update();
                        RAGE.Game.Ui.HideHudAndRadarThisFrame();
                        RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                        RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Loading...");
                        RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                        RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                        RAGE.Game.Ui.SetTextCentre(true);
                        RAGE.Game.Ui.SetTextJustification(0);
                        RAGE.Game.Ui.SetTextFont(0);
                        RAGE.Game.Ui.SetTextDropShadow();
                        RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                        if (RAGE.Game.Utils.Timera() > 1000)
                        {
                            //RAGE.Chat.Output("Failed to load model");
                            return;
                        }
                    }

                    PreviewObj = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), new Vector3(-2430.6f, -1198.6f, 1236.9f), new Vector3(0, 0, 180f), 255);

                    if (FavoriteObjects.ContainsKey(PreviewObj.Model))
                    {
                        if (GUI.GetButtonById("previewer_favoriteadd").RectObject.Color.R == 0)
                        {
                            GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                            GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Favorited!";
                            GUI.GetButtonById("previewer_group").Enable();
                            if (FavoriteObjects[PreviewObj.Model] == "")
                                GUI.GetButtonById("previewer_group").TextObject.Text = "Enter Group Name";
                            else
                                GUI.GetButtonById("previewer_group").TextObject.Text = FavoriteObjects[PreviewObj.Model];
                        }
                    }
                    else
                    {
                        if (GUI.GetButtonById("previewer_favoriteadd").RectObject.Color.R == 255)
                        {
                            GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                            GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Add to favorites";
                            GUI.GetButtonById("previewer_group").Disable();
                        }
                    }

                    Vector3 a = new Vector3();
                    Vector3 b = new Vector3();
                    RAGE.Game.Misc.GetModelDimensions(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), a, b);
                    PreviewSize = b - a;
                    for (int i = PreviewIndex; i < PreviewIndex + 5; i++)
                        if (i < Objects.Hashes.Count)
                            RAGE.Game.Streaming.RequestModel(RAGE.Game.Misc.GetHashKey(Objects.Hashes[i]));
                }
                else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollDown))
                {
                    if (PreviewObj != null)
                    {
                        if (FavoriteObjects.ContainsKey(PreviewObj.Model))
                        {
                            if (GUI.GetButtonById("previewer_group").TextObject.Text != "Enter Group Name")
                                FavoriteObjects[PreviewObj.Model] = GUI.GetButtonById("previewer_group").TextObject.Text;
                        }
                    }
                    IsGroupHover = false;

                    if (PreviewObj != null)
                        PreviewObj.Destroy();

                    PreviewIndex--;
                    if (PreviewIndex < 0)
                        PreviewIndex = Objects.Hashes.Count - 1;

                    int cnt = 0;
                    while (!RAGE.Game.Streaming.IsModelInCdimage(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])) || !RAGE.Game.Streaming.IsModelValid(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])))
                    {
                        PreviewIndex--;
                        if (PreviewIndex < 0)
                            PreviewIndex = Objects.Hashes.Count - 1;
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        RAGE.Game.Invoker.Wait(0);
                        RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                        if (cnt > 2)
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Indexing... (" + cnt + ")\n(Corrupt Model)");
                        else
                            RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Indexing...");
                        RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                        RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                        RAGE.Game.Ui.SetTextCentre(true);
                        RAGE.Game.Ui.SetTextJustification(0);
                        RAGE.Game.Ui.SetTextFont(0);
                        RAGE.Game.Ui.SetTextDropShadow();
                        RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        GUI.Update();
                        RAGE.Game.Ui.HideHudAndRadarThisFrame();
                        cnt++;
                    }

                    RAGE.Game.Utils.Settimera(0);
                    RAGE.Game.Streaming.RequestModel(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]));
                    while (!RAGE.Game.Streaming.HasModelLoaded(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])))
                    {
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        RAGE.Game.Invoker.Wait(0);
                        DrawBox(LocalPlayer.Position, 100f, 100f, 100f, 100, 149, 237, 255);
                        GUI.Update();
                        RAGE.Game.Ui.HideHudAndRadarThisFrame();
                        RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                        RAGE.Game.Ui.AddTextComponentSubstringPlayerName("Loading...");
                        RAGE.Game.Ui.SetTextScale(1.0f, 0.45f);
                        RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                        RAGE.Game.Ui.SetTextCentre(true);
                        RAGE.Game.Ui.SetTextJustification(0);
                        RAGE.Game.Ui.SetTextFont(0);
                        RAGE.Game.Ui.SetTextDropShadow();
                        RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.9f, 0);
                        if (RAGE.Game.Utils.Timera() > 1000)
                        {
                            //RAGE.Chat.Output("Failed to load model");
                            return;
                        }
                    }

                    PreviewObj = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), new Vector3(-2430.6f, -1198.6f, 1236.9f), new Vector3(0, 0, 180f), 255);

                    if (FavoriteObjects.ContainsKey(PreviewObj.Model))
                    {
                        if (GUI.GetButtonById("previewer_favoriteadd").RectObject.Color.R == 0)
                        {
                            GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                            GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Favorited!";
                            GUI.GetButtonById("previewer_group").Enable();
                            if (FavoriteObjects[PreviewObj.Model] == "")
                                GUI.GetButtonById("previewer_group").TextObject.Text = "Enter Group Name";
                            else
                                GUI.GetButtonById("previewer_group").TextObject.Text = FavoriteObjects[PreviewObj.Model];
                        }
                    }
                    else
                    {
                        if (GUI.GetButtonById("previewer_favoriteadd").RectObject.Color.R == 255)
                        {
                            GUI.GetButtonById("previewer_favoriteadd").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                            GUI.GetButtonById("previewer_favoriteadd").TextObject.Text = "Add to favorites";
                            GUI.GetButtonById("previewer_group").Disable();
                        }
                    }

                    Vector3 a = new Vector3();
                    Vector3 b = new Vector3();
                    RAGE.Game.Misc.GetModelDimensions(RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex]), a, b);
                    PreviewSize = b - a;
                    for (int i = PreviewIndex; i > PreviewIndex - 5; i--)
                        if (i > 0)
                            RAGE.Game.Streaming.RequestModel(RAGE.Game.Misc.GetHashKey(Objects.Hashes[i]));
                }

                DrawBox(LocalPlayer.Position, 250f, 250f, 250f, 100, 149, 237, 255);
                if (PreviewObj != null)
                {
                    if (PreviewObj.Exists)
                    {
                        if (RotatePreviewRight)
                            PreviewObj.SetRotation(PreviewObj.GetRotation(2).X, PreviewObj.GetRotation(2).Y, PreviewObj.GetRotation(2).Z + 1f, 2, false);
                        else if (RotatePreviewLeft)
                            PreviewObj.SetRotation(PreviewObj.GetRotation(2).X, PreviewObj.GetRotation(2).Y, PreviewObj.GetRotation(2).Z - 1f, 2, false);
                        if (ZoomPreviewOut)
                            PreviewObj.Position = new Vector3(PreviewObj.Position.X + 0.025f, PreviewObj.Position.Y - 0.025f, PreviewObj.Position.Z - 0.025f);
                        else if (ZoomPreviewIn)
                            PreviewObj.Position = new Vector3(PreviewObj.Position.X - 0.025f, PreviewObj.Position.Y + 0.025f, PreviewObj.Position.Z + 0.025f);
                        if (RotatePreviewDown)
                            PreviewObj.SetRotation(PreviewObj.GetRotation(0).X + 1f, PreviewObj.GetRotation(0).Y, PreviewObj.GetRotation(0).Z, 0, false);
                        else if (RotatePreviewUp)
                            PreviewObj.SetRotation(PreviewObj.GetRotation(0).X - 1f, PreviewObj.GetRotation(0).Y, PreviewObj.GetRotation(0).Z, 0, false);
                        DrawSkeleton(PreviewObj.Position, PreviewSize, PreviewObj.GetRotation(2));
                    }
                }

                RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                RAGE.Game.Ui.AddTextComponentSubstringPlayerName($"[{Objects.Hashes[PreviewIndex]}]<=>[{RAGE.Game.Misc.GetHashKey(Objects.Hashes[PreviewIndex])}]");
                RAGE.Game.Ui.SetTextScale(1.0f, 0.25f);
                RAGE.Game.Ui.SetTextColour(255, 255, 255, 255);
                RAGE.Game.Ui.SetTextCentre(true);
                RAGE.Game.Ui.SetTextJustification(0);
                RAGE.Game.Ui.SetTextFont(0);
                RAGE.Game.Ui.SetTextDropShadow();
                RAGE.Game.Ui.EndTextCommandDisplayText(0.5f, 0.95f, 0);
            }
            else
            {
                if (KeyboardInput.IsKeyJustReleased("MapEditor_CursorToggle"))
                    GUI.ToggleCursor();

                if (KeyboardInput.IsKeyJustReleased("MapEditor_Toggle"))
                {
                    if (!cantype)
                    {
                        if (IsEditorActive)
                        {
                            StopEditor(false);
                        }
                        else
                        {
                            StartEditor();
                        }
                    }
                }

                if (IsEditorActive)
                {
                    if(RAGE.Input.IsDown((int)RAGEInput.VirtualKey.DELETE) && HasObjectHighlighted)
                    {
                        GUI.GetButtonById("entity_info").Disable();
                        GUI.GetButtonById("entity_info_type").Disable();
                        GUI.GetButtonById("entity_info_frozen").Disable();
                        GUI.GetButtonById("entity_info_frozen_state").Disable();
                        if (HighlightedEntity != null)
                        {
                            MapEditorObjectPool.DeleteEntity(HighlightedEntity.Entity);
                        }
                        HasObjectHighlighted = false;
                        HighlightedEntity = null;
                    }

                    RAGE.Game.Graphics.DrawSprite("commonmenu", "common_medal", 0.5f, 0.5f, 0.0125f * ((float)ScreenY / (float)ScreenX), 0.0125f, 0, 255, 255, 255, 255, 0);
                    ProcessCamera();
                    if (RAGE.Game.Pad.IsControlJustPressed(0, (int)RAGE.Game.Control.Aim) && !GUI.WasAnyButtonPressedThisFrame)
                    {
                        tempX = GUI.GetCursorX();
                        tempY = GUI.GetCursorY();
                        GUI.DisableCursor();
                        if (GUI.GetButtonById("entity_rightclick").IsEnabled)
                            GUI.GetButtonById("entity_rightclick").Disable();
                    }
                    else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Aim) && !GUI.WasAnyButtonPressedThisFrame)
                    {
                        GUI.SetCursor(tempX, tempY);
                        GUI.EnableCursor();
                    }

                    if (HasObjectHighlighted && RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Aim) && !GUI.WasAnyButtonPressedThisFrame)
                    {
                        if (!EntityFollowMouse && !Marker_X_Move_Clicked && !Marker_Y_Move_Clicked && !Marker_Z_Move_Clicked && !Marker_X_Rotate_Clicked && !Marker_Y_Rotate_Clicked && !Marker_Z_Rotate_Clicked)
                        {
                            Vector3 test1 = EditorCamera.GetPosition();
                            Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                            Vector3 test3 = test2 - test1;
                            Vector3 from = test1 + test3 * 0.05f;
                            Vector3 to = test1 + test3 * 1000f;
                            Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                            if (hit.Hit)
                            {
                                if (HighlightedEntity != null)
                                {
                                    if (HighlightedEntity.Entity.Handle == hit.EntityHit)
                                    {
                                        Vector3 pos = RageMath.WorldToScreen2(hit.EndCoords);
                                        if (pos != null)
                                        {
                                            GUI.GetButtonById("entity_rightclick").Enable();
                                            GUI.GetButtonById("entity_rightclick").SetPosition(new GUI.Vector2(pos.X, pos.Y));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!GUI.IsCursorEnabled())
                    {
                        GUI.SetCursor(tempX, tempY);
                    }

                    if (KeyboardInput.IsKeyJustReleased("MapEditor_LookAt"))
                    {
                        if (!cantype)
                        {
                            if (HasObjectHighlighted)
                            {
                                Vector3 dir = RageMath.GetDirectionFromTwoVectors(EditorCamera.GetPosition(), HighlightedEntity.GetPosition());
                                Vector3 rot = RageMath.DirectionToRotation(dir, 0f);
                                EditorCamera.SetRotation(rot);
                            }
                        }
                    }
                    if (KeyboardInput.IsKeyJustReleased("MapEditor_GoTo"))
                    {
                        if (!cantype)
                        {
                            if (HasObjectHighlighted)
                            {
                                EditorCamera.SetPosition(HighlightedEntity.GetPosition());
                            }
                        }
                    }


                    if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack) && !Marker_X_Rotate_Clicked && !Marker_Y_Rotate_Clicked && !Marker_Z_Rotate_Clicked && !Marker_X_Move_Clicked && !Marker_Y_Move_Clicked && !Marker_Z_Move_Clicked && !GUI.WasAnyButtonPressedThisFrame)
                    {
                        if (GUI.GetButtonById("entity_rightclick").IsEnabled)
                            GUI.GetButtonById("entity_rightclick").Disable();

                        Vector3 test1 = EditorCamera.GetPosition();
                        Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                        Vector3 test3 = test2 - test1;
                        Vector3 from = test1 + test3 * 0.05f;
                        Vector3 to = test1 + test3 * 300;
                        Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                        if (hit.Hit)
                        {
                            if (hit.EntityHit != 0)
                            {
                                MapEditorObject obj = MapEditorObjectPool.GetMapEditorObjectByHandle(hit.EntityHit);
                                if (obj != null)
                                {
                                    if (!obj.Entity.IsNull && obj.Entity.Exists)
                                    {
                                        if (obj.Entity.Type == RAGE.Elements.Type.Vehicle)
                                        {
                                            obj.FreezeEntity(true);
                                            Vector3 a = new Vector3();
                                            Vector3 b = new Vector3();
                                            RAGE.Game.Misc.GetModelDimensions(obj.Entity.Model, a, b);
                                            Vector3 c = b - a;

                                            if (HighlightedEntity != null)
                                            {
                                                HighlightedEntity.FreezePosition(HighlightedEntity.Frozen);
                                                HighlightedEntity.ActivatePhysics();
                                            }
                                            HighlightedEntity = obj;
                                            HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(HighlightedEntity.Entity.Handle, 2));
                                            HighlightedEntity.Position = obj.Position;
                                            HighlightedEntity.TempPosition = obj.Position;
                                            HighlightedEntity.Size = c;
                                            HasObjectHighlighted = true;

                                            GUI.GetButtonById("entity_info").Enable();
                                            GUI.GetButtonById("entity_info_type").TextObject.Text = "Type: " + HighlightedEntity.Entity.Type.ToString();
                                            GUI.GetButtonById("entity_info_type").Enable();
                                            GUI.GetButtonById("entity_info_frozen").TextObject.Text = "Frozen";
                                            GUI.GetButtonById("entity_info_frozen").Enable();
                                            GUI.GetButtonById("entity_info_frozen_state").Enable();
                                            if (HighlightedEntity.Frozen)
                                            {
                                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                                            }
                                            else
                                            {
                                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                                            }
                                        }
                                        else if (obj.Entity.Type == RAGE.Elements.Type.Object)
                                        {
                                            obj.FreezeEntity(true);
                                            Vector3 a = new Vector3();
                                            Vector3 b = new Vector3();
                                            RAGE.Game.Misc.GetModelDimensions(obj.Entity.Model, a, b);
                                            Vector3 c = b - a;

                                            if (HighlightedEntity != null)
                                            {
                                                HighlightedEntity.FreezePosition(HighlightedEntity.Frozen);
                                                HighlightedEntity.ActivatePhysics();
                                            }
                                            HighlightedEntity = obj;
                                            HighlightedEntity.SetRotation(RAGE.Game.Entity.GetEntityRotation(HighlightedEntity.Entity.Handle, 2));
                                            HighlightedEntity.Position = obj.Position;
                                            HighlightedEntity.TempPosition = obj.Position;
                                            HighlightedEntity.Size = c;
                                            HasObjectHighlighted = true;

                                            GUI.GetButtonById("entity_info").Enable();
                                            GUI.GetButtonById("entity_info_type").TextObject.Text = "Type: " + HighlightedEntity.Entity.Type.ToString();
                                            GUI.GetButtonById("entity_info_type").Enable();
                                            GUI.GetButtonById("entity_info_frozen").TextObject.Text = "Frozen";
                                            GUI.GetButtonById("entity_info_frozen").Enable();
                                            GUI.GetButtonById("entity_info_frozen_state").Enable();
                                            if (HighlightedEntity.Frozen)
                                            {
                                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(0, 0, 0, 125), new GUI.RGBA(255, 255, 0, 125), 0.1f);
                                            }
                                            else
                                            {
                                                GUI.GetButtonById("entity_info_frozen_state").LerpColorFromTo(new GUI.RGBA(255, 255, 0, 125), new GUI.RGBA(0, 0, 0, 125), 0.1f);
                                            }
                                        }
                                        else
                                        {
                                            GUI.GetButtonById("entity_info").Disable();
                                            GUI.GetButtonById("entity_info_type").Disable();
                                            GUI.GetButtonById("entity_info_frozen").Disable();
                                            GUI.GetButtonById("entity_info_frozen_state").Disable();
                                            if (HighlightedEntity != null)
                                            {
                                                if (!HighlightedEntity.Frozen)
                                                    HighlightedEntity.FreezePosition(false);
                                                HighlightedEntity.ActivatePhysics();
                                            }
                                            HasObjectHighlighted = false;
                                            HighlightedEntity = null;
                                        }
                                    }
                                    else
                                    {
                                        GUI.GetButtonById("entity_info").Disable();
                                        GUI.GetButtonById("entity_info_type").Disable();
                                        GUI.GetButtonById("entity_info_frozen").Disable();
                                        GUI.GetButtonById("entity_info_frozen_state").Disable();
                                        if (HighlightedEntity != null)
                                        {
                                            if (!HighlightedEntity.Frozen)
                                                HighlightedEntity.FreezePosition(false);
                                            HighlightedEntity.ActivatePhysics();
                                        }
                                        HasObjectHighlighted = false;
                                        HighlightedEntity = null;
                                    }
                                }
                                else
                                {
                                    GUI.GetButtonById("entity_info").Disable();
                                    GUI.GetButtonById("entity_info_type").Disable();
                                    GUI.GetButtonById("entity_info_frozen").Disable();
                                    GUI.GetButtonById("entity_info_frozen_state").Disable();
                                    if (HighlightedEntity != null)
                                    {
                                        if (!HighlightedEntity.Frozen)
                                            HighlightedEntity.FreezePosition(false);
                                        HighlightedEntity.ActivatePhysics();
                                    }
                                    HasObjectHighlighted = false;
                                    HighlightedEntity = null;
                                }
                            }
                            else
                            {
                                GUI.GetButtonById("entity_info").Disable();
                                GUI.GetButtonById("entity_info_type").Disable();
                                GUI.GetButtonById("entity_info_frozen").Disable();
                                GUI.GetButtonById("entity_info_frozen_state").Disable();
                                if (HighlightedEntity != null)
                                {
                                    if (!HighlightedEntity.Frozen)
                                        HighlightedEntity.FreezePosition(false);
                                    HighlightedEntity.ActivatePhysics();
                                }
                                HasObjectHighlighted = false;
                                HighlightedEntity = null;
                            }
                        }
                        else
                        {
                            GUI.GetButtonById("entity_info").Disable();
                            GUI.GetButtonById("entity_info_type").Disable();
                            GUI.GetButtonById("entity_info_frozen").Disable();
                            GUI.GetButtonById("entity_info_frozen_state").Disable();
                            if (HighlightedEntity != null)
                            {
                                if (!HighlightedEntity.Frozen)
                                    HighlightedEntity.FreezePosition(false);
                                HighlightedEntity.ActivatePhysics();
                            }
                            HasObjectHighlighted = false;
                            HighlightedEntity = null;
                        }
                    }

                    if (HasObjectHighlighted && !EntityFollowMouse)
                    {
                        if (Marker_X_Rotate_Clicked || Marker_Y_Rotate_Clicked || Marker_Z_Rotate_Clicked || Marker_X_Move_Clicked || Marker_Y_Move_Clicked || Marker_Z_Move_Clicked)
                        {
                            if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack) && !GUI.WasAnyButtonPressedThisFrame)
                            {
                                Marker_X_Rotate_Clicked = false;
                                Marker_Y_Rotate_Clicked = false;
                                Marker_Z_Rotate_Clicked = false;
                                Marker_X_Move_Clicked = false;
                                Marker_Y_Move_Clicked = false;
                                Marker_Z_Move_Clicked = false;

                                HighlightedEntity.SetRotation(HighlightedEntity.TempRotation);
                                HighlightedEntity.SetPosition(HighlightedEntity.TempPosition);
                            }

                            if (Marker_X_Rotate_Clicked)
                            {
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x *= 1.25f;
                                Marker_X_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                                Marker_X_Rotate.Marker.Scale = new Vector3(x, x, x);
                                Marker_X_Rotate.Marker.Rotation = new Vector3(-HighlightedEntity.TempRotation.Z + 90f, 90, 0);
                                Marker_X_Rotate.Draw();
                            }
                            else if (Marker_Y_Rotate_Clicked)
                            {
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x *= 1.25f;
                                Marker_Y_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                                Marker_Y_Rotate.Marker.Scale = new Vector3(x, x, x);
                                Marker_Y_Rotate.Marker.Rotation = new Vector3(-HighlightedEntity.TempRotation.Z, 90, 0);
                                Marker_Y_Rotate.Draw();
                            }
                            else if (Marker_Z_Rotate_Clicked)
                            {
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x *= 1.25f;
                                Marker_Z_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                                Marker_Z_Rotate.Marker.Scale = new Vector3(x, x, x);
                                Marker_Z_Rotate.Marker.Rotation = new Vector3(0, 0, 0);
                                Marker_Z_Rotate.Draw();
                            }
                            else if (Marker_X_Move_Clicked)
                            {
                                Marker_X_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, HighlightedEntity.Size.X / 2f + (HighlightedEntity.Size.X / 4f), 0f, 0f);
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x /= 15f;
                                Marker_X_Move.Marker.Scale = new Vector3(x, x, x);
                                Marker_X_Move.Draw();
                            }
                            else if (Marker_Y_Move_Clicked)
                            {
                                Marker_Y_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, HighlightedEntity.Size.Y / 2f + (HighlightedEntity.Size.Y / 4f), 0f);
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x /= 15f;
                                Marker_Y_Move.Marker.Scale = new Vector3(x, x, x);
                                Marker_Y_Move.Draw();
                            }
                            else if (Marker_Z_Move_Clicked)
                            {
                                Marker_Z_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, 0f, HighlightedEntity.Size.Z / 2f + (HighlightedEntity.Size.Z / 4f));
                                float x = 0f;
                                if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                                {
                                    x = HighlightedEntity.Size.Y;
                                }
                                else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                                {
                                    x = HighlightedEntity.Size.Z;
                                }
                                else
                                {
                                    x = HighlightedEntity.Size.X;
                                }
                                x /= 15f;
                                Marker_Z_Move.Marker.Scale = new Vector3(x, x, x);
                                Marker_Z_Move.Draw();
                            }
                        }
                        else
                        {
                            float x = 0f;
                            if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                            {
                                x = HighlightedEntity.Size.X;
                            }
                            else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                            {
                                x = HighlightedEntity.Size.Y;
                            }
                            else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                            {
                                x = HighlightedEntity.Size.Z;
                            }
                            else
                            {
                                x = HighlightedEntity.Size.X;
                            }
                            x *= 1.25f;
                            Marker_X_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                            Marker_X_Rotate.Marker.Scale = new Vector3(x, x, x);
                            Marker_X_Rotate.Marker.Rotation = new Vector3(-HighlightedEntity.TempRotation.Z + 90f, 90, 0);
                            Vector3 norm = new Vector3();
                            Vector3 x_hit_pos = new Vector3();
                            bool x_hit = Marker_X_Rotate.IsRaycasted(ref x_hit_pos, ref norm);

                            Marker_Y_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                            Marker_Y_Rotate.Marker.Scale = new Vector3(x, x, x);
                            Marker_Y_Rotate.Marker.Rotation = new Vector3(-HighlightedEntity.TempRotation.Z, 90, 0);

                            Vector3 y_hit_pos = new Vector3();
                            bool y_hit = Marker_Y_Rotate.IsRaycasted(ref y_hit_pos, ref norm);

                            Marker_Z_Rotate.Marker.Position = HighlightedEntity.GetPosition();
                            Marker_Z_Rotate.Marker.Scale = new Vector3(x, x, x);
                            Marker_Z_Rotate.Marker.Rotation = new Vector3(0, 0, 0);

                            Vector3 z_hit_pos = new Vector3();
                            bool z_hit = Marker_Z_Rotate.IsRaycasted(ref z_hit_pos, ref norm);

                            Marker_Y_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, HighlightedEntity.Size.Y / 2f + (HighlightedEntity.Size.Y / 4f), 0f);
                            Marker_X_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, HighlightedEntity.Size.X / 2f + (HighlightedEntity.Size.X / 4f), 0f, 0f);
                            Marker_Z_Move.Marker.Position = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, 0f, HighlightedEntity.Size.Z / 2f + (HighlightedEntity.Size.Z / 4f));
                            x = 0f;
                            if (HighlightedEntity.Size.X > HighlightedEntity.Size.Y && HighlightedEntity.Size.X > HighlightedEntity.Size.Z)
                            {
                                x = HighlightedEntity.Size.X;
                            }
                            else if (HighlightedEntity.Size.Y > HighlightedEntity.Size.X && HighlightedEntity.Size.Y > HighlightedEntity.Size.Z)
                            {
                                x = HighlightedEntity.Size.Y;
                            }
                            else if (HighlightedEntity.Size.Z > HighlightedEntity.Size.X && HighlightedEntity.Size.Z > HighlightedEntity.Size.Y)
                            {
                                x = HighlightedEntity.Size.Z;
                            }
                            else
                            {
                                x = HighlightedEntity.Size.X;
                            }
                            x /= 15f;
                            Marker_X_Move.Marker.Scale = new Vector3(x, x, x);
                            Marker_Y_Move.Marker.Scale = new Vector3(x, x, x);
                            Marker_Z_Move.Marker.Scale = new Vector3(x, x, x);

                            bool x_hit_m = Marker_X_Move.IsSphereCasted();
                            bool y_hit_m = Marker_Y_Move.IsSphereCasted();
                            bool z_hit_m = Marker_Z_Move.IsSphereCasted();

                            float distClosest = 999999f;
                            int whichMarker = -1;

                            if (x_hit)
                            {
                                float dist = x_hit_pos.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 1;
                                }
                            }

                            if (y_hit)
                            {
                                float dist = y_hit_pos.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 2;
                                }
                            }

                            if (z_hit)
                            {
                                float dist = z_hit_pos.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 3;
                                }
                            }

                            if (x_hit_m)
                            {
                                float dist = Marker_X_Move.Marker.Position.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 4;
                                }
                                else
                                {
                                    if (x_hit || y_hit || z_hit)
                                    {
                                        whichMarker = 4;
                                        distClosest = dist;
                                    }
                                }
                            }

                            if (y_hit_m)
                            {
                                float dist = Marker_Y_Move.Marker.Position.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 5;
                                }
                                if (x_hit || y_hit || z_hit)
                                {
                                    whichMarker = 5;
                                    distClosest = dist;
                                }
                            }

                            if (z_hit_m)
                            {
                                float dist = Marker_Z_Move.Marker.Position.DistanceTo(EditorCamera.GetPosition());
                                if (dist < distClosest)
                                {
                                    distClosest = dist;
                                    whichMarker = 6;
                                }
                                if (x_hit || y_hit || z_hit)
                                {
                                    whichMarker = 6;
                                    distClosest = dist;
                                }
                            }

                            if (whichMarker == 1)
                            {
                                originalHitPoint = x_hit_pos;
                                Marker_X_Rotate_Highlighted = true;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = false;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = false;
                            }
                            else if (whichMarker == 2)
                            {
                                originalHitPoint = y_hit_pos;
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = true;
                                Marker_Z_Rotate_Highlighted = false;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = false;
                            }
                            else if (whichMarker == 3)
                            {
                                originalHitPoint = z_hit_pos;
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = true;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = false;
                            }
                            else if (whichMarker == 4)
                            {
                                originalHitPoint = Marker_X_Move.Marker.Position;
                                //diffP = new Vector3(MathF.Abs(originalHitPoint.X - HighlightedEntity.Entity.Position.X), MathF.Abs(originalHitPoint.Y - HighlightedEntity.Entity.Position.Y), MathF.Abs(originalHitPoint.Z - HighlightedEntity.Entity.Position.Z));
                                Marker_X_Move_Highlighted = true;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = false;
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = false;
                            }
                            else if (whichMarker == 5)
                            {
                                originalHitPoint = Marker_Y_Move.Marker.Position;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = true;
                                Marker_Z_Move_Highlighted = false;
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = false;
                            }
                            else if (whichMarker == 6)
                            {
                                originalHitPoint = Marker_Z_Move.Marker.Position;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = true;
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = false;
                            }
                            else
                            {
                                Marker_X_Rotate_Highlighted = false;
                                Marker_Y_Rotate_Highlighted = false;
                                Marker_Z_Rotate_Highlighted = false;
                                Marker_X_Move_Highlighted = false;
                                Marker_Y_Move_Highlighted = false;
                                Marker_Z_Move_Highlighted = false;
                            }

                            if (Marker_X_Rotate_Highlighted)
                            {
                                Marker_X_Move.Draw();
                                Marker_Y_Move.Draw();
                                Marker_Z_Move.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_X_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                            }
                            else if (Marker_Y_Rotate_Highlighted)
                            {
                                Marker_X_Move.Draw();
                                Marker_Y_Move.Draw();
                                Marker_Z_Move.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                                Marker_X_Rotate.Draw();
                            }
                            else if (Marker_Z_Rotate_Highlighted)
                            {
                                Marker_X_Move.Draw();
                                Marker_Y_Move.Draw();
                                Marker_Z_Move.Draw();
                                Marker_X_Rotate.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                            }
                            else if (Marker_X_Move_Highlighted)
                            {
                                Marker_X_Rotate.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                                Marker_Y_Move.Draw();
                                Marker_Z_Move.Draw();
                                Marker_X_Move.Draw();
                            }
                            else if (Marker_Y_Move_Highlighted)
                            {
                                Marker_X_Rotate.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                                Marker_Z_Move.Draw();
                                Marker_X_Move.Draw();
                                Marker_Y_Move.Draw();
                            }
                            else if (Marker_Z_Move_Highlighted)
                            {
                                Marker_X_Rotate.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                                Marker_Z_Move.Draw();
                                Marker_Y_Move.Draw();
                                Marker_X_Move.Draw();
                            }
                            else
                            {
                                Marker_X_Rotate.Draw();
                                Marker_Z_Rotate.Draw();
                                Marker_Y_Rotate.Draw();
                                Marker_X_Move.Draw();
                                Marker_Y_Move.Draw();
                                Marker_Z_Move.Draw();
                            }

                            if (Marker_X_Rotate_Highlighted)
                                Marker_X_Rotate.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_X_Rotate.Marker.Color = new RGBA(255, 0, 0);

                            if (Marker_Y_Rotate_Highlighted)
                                Marker_Y_Rotate.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_Y_Rotate.Marker.Color = new RGBA(0, 255, 0);

                            if (Marker_Z_Rotate_Highlighted)
                                Marker_Z_Rotate.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_Z_Rotate.Marker.Color = new RGBA(0, 0, 255);

                            if (Marker_X_Move_Highlighted)
                                Marker_X_Move.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_X_Move.Marker.Color = new RGBA(255, 0, 0);

                            if (Marker_Y_Move_Highlighted)
                                Marker_Y_Move.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_Y_Move.Marker.Color = new RGBA(0, 255, 0);

                            if (Marker_Z_Move_Highlighted)
                                Marker_Z_Move.Marker.Color = new RGBA(255, 255, 0);
                            else
                                Marker_Z_Move.Marker.Color = new RGBA(0, 0, 255);

                            if (RAGE.Game.Pad.IsControlJustPressed(0, (int)RAGE.Game.Control.Attack) && !GUI.WasAnyButtonPressedThisFrame)
                            {
                                if (GUI.GetButtonById("entity_rightclick").IsEnabled)
                                    GUI.GetButtonById("entity_rightclick").Disable();

                                Marker_X_Rotate_Clicked = false;
                                Marker_Y_Rotate_Clicked = false;
                                Marker_Z_Rotate_Clicked = false;
                                Marker_X_Move_Clicked = false;
                                Marker_Y_Move_Clicked = false;
                                Marker_Z_Move_Clicked = false;

                                if (Marker_X_Rotate_Highlighted)
                                {
                                    Marker_X_Rotate_Clicked = true;
                                }
                                else if (Marker_Y_Rotate_Highlighted)
                                {
                                    Marker_Y_Rotate_Clicked = true;
                                }
                                else if (Marker_Z_Rotate_Highlighted)
                                {
                                    Marker_Z_Rotate_Clicked = true;
                                }
                                else if (Marker_X_Move_Highlighted)
                                {
                                    Marker_X_Move_Clicked = true;
                                }
                                else if (Marker_Y_Move_Highlighted)
                                {
                                    Marker_Y_Move_Clicked = true;
                                }
                                else if (Marker_Z_Move_Highlighted)
                                {
                                    Marker_Z_Move_Clicked = true;
                                }
                            }
                        }
                    }

                    bool mouseOver = false;
                    if (HasObjectHighlighted && !Marker_X_Rotate_Clicked && !Marker_Y_Rotate_Clicked && !Marker_Z_Rotate_Clicked && !Marker_X_Move_Clicked && !Marker_Y_Move_Clicked && !Marker_Z_Move_Clicked)
                    {
                        //if (RAGE.Game.Pad.IsControlJustPressed(0, (int)RAGE.Game.Control.Attack) && !GUI.WasAnyButtonPressedThisFrame)
                        //{
                            if (GUI.GetButtonById("entity_rightclick").IsEnabled)
                                GUI.GetButtonById("entity_rightclick").Disable();

                            if (HighlightedEntity != null && HighlightedEntity.Entity.Exists)
                            {
                                Vector3 e_p = EditorCamera.GetPosition();
                                Vector3 ep_2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                                Vector3 ep_3 = ep_2 - e_p;
                                Vector3 from = e_p + ep_3 * 0.05f;
                                Vector3 to = e_p + ep_3 * 300;
                                Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                                if (hit.Hit)
                                {
                                    if (hit.EntityHit != 0)
                                    {
                                        MapEditorObject obj = MapEditorObjectPool.GetMapEditorObjectByHandle(hit.EntityHit);
                                        if (obj != null && obj == HighlightedEntity)
                                        {
                                            if (HighlightedEntity != null)
                                            {
                                                HighlightColor_Edge = new RGBA(255, 255, 0, 35);
                                                HighlightColor_Full = new RGBA(255, 255, 0, 35);
                                                EntityFollowMouse = true;
                                            }
                                        }
                                    }
                                }
                            }
                        //}
                        //else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack) && EntityFollowMouse && !GUI.WasAnyButtonPressedThisFrame)
                        //{
                        //    EntityFollowMouse = false;
                            HighlightedEntity.SetRotation(HighlightedEntity.TempRotation, 2);
                        //}
                        //else if (!EntityFollowMouse)
                        //{
                            Vector3 e_p = EditorCamera.GetPosition();
                            Vector3 ep_2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                            Vector3 ep_3 = ep_2 - e_p;
                            Vector3 from = e_p + ep_3 * 0.05f;
                            Vector3 to = e_p + ep_3 * 300;
                            Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, LocalPlayer.Handle, -1);
                            if (hit.Hit)
                            {
                                if (hit.EntityHit != 0)
                                {
                                    MapEditorObject obj = MapEditorObjectPool.GetMapEditorObjectByHandle(hit.EntityHit);
                                    if (obj != null)
                                    {
                                        if (!RAGE.Game.Pad.IsControlPressed(0, (int)RAGE.Game.Control.Aim))
                                        {
                                            HighlightColor_Edge = new RGBA(255, 255, 0, 35);
                                            HighlightColor_Full = new RGBA(255, 255, 255, 35);
                                            mouseOver = true;
                                        }
                                        else
                                        {
                                            HighlightColor_Edge = new RGBA(255, 255, 255, 35);
                                            HighlightColor_Full = new RGBA(255, 255, 255, 35);
                                        }
                                    }
                                    else
                                    {
                                        HighlightColor_Edge = new RGBA(255, 255, 255, 35);
                                        HighlightColor_Full = new RGBA(255, 255, 255, 35);
                                    }
                                }
                                else
                                {
                                    HighlightColor_Edge = new RGBA(255, 255, 255, 35);
                                    HighlightColor_Full = new RGBA(255, 255, 255, 35);
                                }
                            }
                            else
                            {
                                HighlightColor_Edge = new RGBA(255, 255, 255, 35);
                                HighlightColor_Full = new RGBA(255, 255, 255, 35);
                            }
                        }

                    //    if (EntityFollowMouse)
                    //    {
                    //        if (HighlightedEntity != null && HighlightedEntity.Entity.Exists)
                    //        {
                    //            if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollUp))
                    //            {
                    //                ClampDistance += 5f;
                    //                if (ClampDistance > 500f)
                    //                    ClampDistance = 500f;
                    //            }
                    //            else if (RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.CursorScrollDown))
                    //            {
                    //                ClampDistance -= 5f;
                    //                if (ClampDistance < 10f)
                    //                    ClampDistance = 10f;
                    //            }
                    //            Vector3 e_p = EditorCamera.GetPosition();
                    //            Vector3 ep_2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                    //            Vector3 ep_3 = ep_2 - e_p;
                    //            Vector3 from = e_p + ep_3 * 0.05f;
                    //            Vector3 to = e_p + ep_3 * 1000;
                    //            Raycasting.RaycastHit hit = Raycasting.RaycastFromTo(from, to, HighlightedEntity.Entity.Handle, -1);
                    //            if (hit.Hit)
                    //            {
                    //                if (hit.EndCoords.DistanceTo(from) <= ClampDistance)
                    //                {
                    //                    HighlightedEntity.SetPosition(new Vector3(hit.EndCoords.X, hit.EndCoords.Y, hit.EndCoords.Z));
                    //                    HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    //                    if(PlaceOnGroundProperly)
                    //                    {
                    //                        if (HighlightedEntity.Entity.Type == RAGE.Elements.Type.Object)
                    //                        {
                    //                            RAGE.Game.Object.PlaceObjectOnGroundProperly(HighlightedEntity.Entity.Handle);
                    //                        }
                    //                        else if (HighlightedEntity.Entity.Type == RAGE.Elements.Type.Vehicle)
                    //                        {
                    //                            RAGE.Game.Vehicle.SetVehicleOnGroundProperly(HighlightedEntity.Entity.Handle, 0);
                    //                        }
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    Vector3 t = to - from;
                    //                    t.Normalize();
                    //                    t *= ClampDistance;
                    //                    Vector3 v = e_p + t;
                    //                    HighlightedEntity.SetPosition(new Vector3(v.X, v.Y, v.Z));
                    //                    HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                Vector3 t = to - from;
                    //                t.Normalize();
                    //                t *= ClampDistance;
                    //                Vector3 v = e_p + t;
                    //                HighlightedEntity.SetPosition(new Vector3(v.X, v.Y, v.Z));
                    //                HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    //            }
                    //        }
                    //    }
                    //}

                    if (Marker_X_Rotate_Clicked)
                    {
                        Vector3 norm = new Vector3();
                        Vector3 vref = new Vector3();
                        if (Marker_X_Rotate.IsRaycasted(ref vref, ref norm, 999999f, true))
                        {
                            //RAGE.Game.Graphics.DrawLine(Marker_X_Rotate.Marker.Position.X, Marker_X_Rotate.Marker.Position.Y, Marker_X_Rotate.Marker.Position.Z, originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(Marker_X_Rotate.Marker.Position.X, Marker_X_Rotate.Marker.Position.Y, Marker_X_Rotate.Marker.Position.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);

                            float angle = RageMath.RadToDegrees(RageMath.GetAngleBetweenVectors(vref - Marker_X_Rotate.Marker.Position, originalHitPoint - Marker_X_Rotate.Marker.Position));
                            float sign = MathF.Sign(RageMath.GetDotProduct(norm, RageMath.GetCrossProduct(vref - Marker_X_Rotate.Marker.Position, originalHitPoint - Marker_X_Rotate.Marker.Position)));
                            angle = angle * sign;
                            HighlightedEntity.SetTempRotation(new Vector3(HighlightedEntity.Rotation.X, HighlightedEntity.Rotation.Y + angle, HighlightedEntity.Rotation.Z), 2);
                            //RageMath.GetAngleBetweenVectors()

                            HighlightColor_Edge = new RGBA(255, 255, 0, 255);
                            HighlightColor_Full = new RGBA(255, 255, 0, 35);
                        }
                    }
                    else if (Marker_X_Move_Clicked)
                    {
                        Vector3 test1 = EditorCamera.GetPosition();
                        Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                        Vector3 test3 = test2 - test1;
                        Vector3 from = test1 + test3 * 0.05f;
                        Vector3 to = test1 + test3 * 1000f;

                        // Vector3 from1 = HighlightedEntity.GetPosition();
                        Vector3 from1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, -1000f, 0f, 0f);
                        Vector3 to1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 1000f, 0f, 0f);
                        RAGE.Game.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                        RAGE.Game.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                        Tuple<Vector3, Vector3> drawme = RageMath.closestDistanceBetweenLines(from, to, from1, to1);
                        //RAGE.Game.Graphics.DrawLine(drawme.Item1.X, drawme.Item1.Y, drawme.Item1.Z, drawme.Item2.X, drawme.Item2.Y, drawme.Item2.Z, 255, 0, 0, 255);
                        //DrawSomeMarker(drawme.Item2, new RGBA(255, 255, 255), 0.25f);
                        HighlightedEntity.SetTempPosition(drawme.Item2 - (Marker_X_Move.Marker.Position - HighlightedEntity.GetPosition()));
                        HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    }
                    else if (Marker_Y_Move_Clicked)
                    {
                        Vector3 test1 = EditorCamera.GetPosition();
                        Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                        Vector3 test3 = test2 - test1;
                        Vector3 from = test1 + test3 * 0.05f;
                        Vector3 to = test1 + test3 * 1000f;

                        // Vector3 from1 = HighlightedEntity.GetPosition();
                        Vector3 from1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, -1000f, 0f);
                        Vector3 to1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, 1000f, 0f);
                        RAGE.Game.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                        RAGE.Game.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                        Tuple<Vector3, Vector3> drawme = RageMath.closestDistanceBetweenLines(from, to, from1, to1);
                        //RAGE.Game.Graphics.DrawLine(drawme.Item1.X, drawme.Item1.Y, drawme.Item1.Z, drawme.Item2.X, drawme.Item2.Y, drawme.Item2.Z, 255, 0, 0, 255);
                        //DrawSomeMarker(drawme.Item2, new RGBA(255, 255, 255), 0.25f);
                        HighlightedEntity.SetTempPosition(drawme.Item2 - (Marker_Y_Move.Marker.Position - HighlightedEntity.GetPosition()));
                        HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    }
                    else if (Marker_Z_Move_Clicked)
                    {
                        Vector3 test1 = EditorCamera.GetPosition();
                        Vector3 test2 = RageMath.ScreenToWorld(GUI.GetCursorX(), GUI.GetCursorY());
                        Vector3 test3 = test2 - test1;
                        Vector3 from = test1 + test3 * 0.05f;
                        Vector3 to = test1 + test3 * 1000f;

                        // Vector3 from1 = HighlightedEntity.GetPosition();
                        Vector3 from1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, 0f, -1000f);
                        Vector3 to1 = RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(HighlightedEntity.Entity.Handle, 0f, 0f, 1000f);
                        RAGE.Game.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                        RAGE.Game.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                        Tuple<Vector3, Vector3> drawme = RageMath.closestDistanceBetweenLines(from, to, from1, to1);
                        //RAGE.Game.Graphics.DrawLine(drawme.Item1.X, drawme.Item1.Y, drawme.Item1.Z, drawme.Item2.X, drawme.Item2.Y, drawme.Item2.Z, 255, 0, 0, 255);
                        //DrawSomeMarker(drawme.Item2, new RGBA(255, 255, 255), 0.25f);
                        HighlightedEntity.SetTempPosition(drawme.Item2 - (Marker_Z_Move.Marker.Position - HighlightedEntity.GetPosition()));
                        HighlightedEntity.SetRotation(HighlightedEntity.GetRotation(), 2);
                    }
                    else if (Marker_Y_Rotate_Clicked)
                    {
                        Vector3 norm = new Vector3();
                        Vector3 vref = new Vector3();
                        if (Marker_Y_Rotate.IsRaycasted(ref vref, ref norm, 999999f, true))
                        {
                            //RAGE.Game.Graphics.DrawLine(Marker_Y_Rotate.Marker.Position.X, Marker_Y_Rotate.Marker.Position.Y, Marker_Y_Rotate.Marker.Position.Z, originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(Marker_Y_Rotate.Marker.Position.X, Marker_Y_Rotate.Marker.Position.Y, Marker_Y_Rotate.Marker.Position.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);

                            float angle = RageMath.RadToDegrees(RageMath.GetAngleBetweenVectors(vref - Marker_Y_Rotate.Marker.Position, originalHitPoint - Marker_Y_Rotate.Marker.Position));
                            float sign = MathF.Sign(RageMath.GetDotProduct(norm, RageMath.GetCrossProduct(vref - Marker_Y_Rotate.Marker.Position, originalHitPoint - Marker_Y_Rotate.Marker.Position)));
                            angle = angle * sign;
                            HighlightedEntity.SetTempRotation(new Vector3(HighlightedEntity.Rotation.X - angle, HighlightedEntity.Rotation.Y, HighlightedEntity.Rotation.Z), 2);
                            //RageMath.GetAngleBetweenVectors()

                            HighlightColor_Edge = new RGBA(255, 255, 0, 255);
                            HighlightColor_Full = new RGBA(255, 255, 0, 35);
                        }
                    }
                    else if (Marker_Z_Rotate_Clicked)
                    {
                        Vector3 norm = new Vector3();
                        Vector3 vref = new Vector3();
                        if (Marker_Z_Rotate.IsRaycasted(ref vref, ref norm, 999999f, true))
                        {
                            //RAGE.Game.Graphics.DrawLine(Marker_Z_Rotate.Marker.Position.X, Marker_Z_Rotate.Marker.Position.Y, Marker_Z_Rotate.Marker.Position.Z, originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(originalHitPoint.X, originalHitPoint.Y, originalHitPoint.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);
                            //RAGE.Game.Graphics.DrawLine(Marker_Z_Rotate.Marker.Position.X, Marker_Z_Rotate.Marker.Position.Y, Marker_Z_Rotate.Marker.Position.Z, vref.X, vref.Y, vref.Z, 255, 0, 0, 255);

                            float angle = RageMath.RadToDegrees(RageMath.GetAngleBetweenVectors(vref - Marker_Z_Rotate.Marker.Position, originalHitPoint - Marker_Z_Rotate.Marker.Position));
                            float sign = MathF.Sign(RageMath.GetDotProduct(norm, RageMath.GetCrossProduct(vref - Marker_Z_Rotate.Marker.Position, originalHitPoint - Marker_Z_Rotate.Marker.Position)));
                            angle = angle * sign;
                            HighlightedEntity.SetTempRotation(new Vector3(HighlightedEntity.Rotation.X, HighlightedEntity.Rotation.Y, HighlightedEntity.Rotation.Z - angle), 2);
                            //RageMath.GetAngleBetweenVectors()

                            HighlightColor_Edge = new RGBA(255, 255, 0, 255);
                            HighlightColor_Full = new RGBA(255, 255, 0, 35);
                        }
                    }
                    else
                    {
                        if (!mouseOver && !EntityFollowMouse)
                        {
                            HighlightColor_Edge = new RGBA(255, 255, 255, 255);
                            HighlightColor_Full = new RGBA(255, 255, 255, 35);
                        }
                    }

                    //if (HasObjectHighlighted)
                    //{
                    //    DrawSkeleton(HighlightedEntity.GetPosition(), HighlightedEntity.Size, HighlightedEntity.GetRotation());
                    //}
                }
            }
        }

        //public void DrawSkeleton(Vector3 pos, Vector3 size, Vector3 rot)
        //{
        //    Vector3 p1 = pos + new Vector3(size.X / 2, size.Y / 2, size.Z / 2);
        //    Vector3 p2 = pos + new Vector3(size.X / 2, -size.Y / 2, size.Z / 2);
        //    Vector3 p3 = pos + new Vector3(size.X / 2, -size.Y / 2, -size.Z / 2);
        //    Vector3 p4 = pos + new Vector3(size.X / 2, size.Y / 2, -size.Z / 2);

        //    Vector3 p5 = pos + new Vector3(-size.X / 2, size.Y / 2, size.Z / 2);
        //    Vector3 p6 = pos + new Vector3(-size.X / 2, -size.Y / 2, size.Z / 2);
        //    Vector3 p7 = pos + new Vector3(-size.X / 2, -size.Y / 2, -size.Z / 2);
        //    Vector3 p8 = pos + new Vector3(-size.X / 2, size.Y / 2, -size.Z / 2);

        //    p1 -= pos;
        //    p1 = RageMath.RotateY(p1, rot.Y);
        //    p1 = RageMath.RotateX(p1, rot.X);
        //    p1 = RageMath.RotateZ(p1, rot.Z);
        //    p1 += pos;

        //    p2 -= pos;
        //    p2 = RageMath.RotateY(p2, rot.Y);
        //    p2 = RageMath.RotateX(p2, rot.X);
        //    p2 = RageMath.RotateZ(p2, rot.Z);
        //    p2 += pos;

        //    p3 -= pos;
        //    p3 = RageMath.RotateY(p3, rot.Y);
        //    p3 = RageMath.RotateX(p3, rot.X);
        //    p3 = RageMath.RotateZ(p3, rot.Z);
        //    p3 += pos;

        //    p4 -= pos;
        //    p4 = RageMath.RotateY(p4, rot.Y);
        //    p4 = RageMath.RotateX(p4, rot.X);
        //    p4 = RageMath.RotateZ(p4, rot.Z);
        //    p4 += pos;

        //    p5 -= pos;
        //    p5 = RageMath.RotateY(p5, rot.Y);
        //    p5 = RageMath.RotateX(p5, rot.X);
        //    p5 = RageMath.RotateZ(p5, rot.Z);
        //    p5 += pos;

        //    p6 -= pos;
        //    p6 = RageMath.RotateY(p6, rot.Y);
        //    p6 = RageMath.RotateX(p6, rot.X);
        //    p6 = RageMath.RotateZ(p6, rot.Z);
        //    p6 += pos;

        //    p7 -= pos;
        //    p7 = RageMath.RotateY(p7, rot.Y);
        //    p7 = RageMath.RotateX(p7, rot.X);
        //    p7 = RageMath.RotateZ(p7, rot.Z);
        //    p7 += pos;

        //    p8 -= pos;
        //    p8 = RageMath.RotateY(p8, rot.Y);
        //    p8 = RageMath.RotateX(p8, rot.X);
        //    p8 = RageMath.RotateZ(p8, rot.Z);
        //    p8 += pos;
        //    RAGE.Game.Graphics.DrawLine(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p3.X, p3.Y, p3.Z, p4.X, p4.Y, p4.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p4.X, p4.Y, p4.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

        //    RAGE.Game.Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p4.X, p4.Y, p4.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z, p1.X, p1.Y, p1.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

        //    RAGE.Game.Graphics.DrawLine(p5.X, p5.Y, p5.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p6.X, p6.Y, p6.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p7.X, p7.Y, p7.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p8.X, p8.Y, p8.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

        //    RAGE.Game.Graphics.DrawPoly(p8.X, p8.Y, p8.Z, p7.X, p7.Y, p7.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p7.X, p7.Y, p7.Z, p6.X, p6.Y, p6.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

        //    RAGE.Game.Graphics.DrawLine(p1.X, p1.Y, p1.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p2.X, p2.Y, p2.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p3.X, p3.Y, p3.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);
        //    RAGE.Game.Graphics.DrawLine(p4.X, p4.Y, p4.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Edge.Red, (int)HighlightColor_Edge.Green, (int)HighlightColor_Edge.Blue, 255);

        //    RAGE.Game.Graphics.DrawPoly(p1.X, p1.Y, p1.Z, p4.X, p4.Y, p4.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p5.X, p5.Y, p5.Z, p4.X, p4.Y, p4.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

        //    RAGE.Game.Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p5.X, p5.Y, p5.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p2.X, p2.Y, p2.Z, p1.X, p1.Y, p1.Z, p5.X, p5.Y, p5.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

        //    RAGE.Game.Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p2.X, p2.Y, p2.Z, p6.X, p6.Y, p6.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p6.X, p6.Y, p6.Z, p7.X, p7.Y, p7.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);

        //    RAGE.Game.Graphics.DrawPoly(p3.X, p3.Y, p3.Z, p7.X, p7.Y, p7.Z, p8.X, p8.Y, p8.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //    RAGE.Game.Graphics.DrawPoly(p8.X, p8.Y, p8.Z, p4.X, p4.Y, p4.Z, p3.X, p3.Y, p3.Z, (int)HighlightColor_Full.Red, (int)HighlightColor_Full.Green, (int)HighlightColor_Full.Blue, (int)HighlightColor_Full.Alpha);
        //}

        public void DrawBox(Vector3 vec, float radx, float rady, float radz, int r, int g, int b, int a)
        {
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z + radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);

            //y
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z - radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z - radz, r, g, b, a);

            //x
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);

            //x
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z - radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z - radz, r, g, b, a);

            //z
            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y + rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X + radx, vec.Y + rady, vec.Z + radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z + radz, vec.X + radx, vec.Y - rady, vec.Z + radz, vec.X - radx, vec.Y - rady, vec.Z + radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y + rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z - radz, r, g, b, a);

            RAGE.Game.Graphics.DrawPoly(vec.X - radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X + radx, vec.Y + rady, vec.Z - radz, r, g, b, a);
            RAGE.Game.Graphics.DrawPoly(vec.X + radx, vec.Y + rady, vec.Z - radz, vec.X + radx, vec.Y - rady, vec.Z - radz, vec.X - radx, vec.Y - rady, vec.Z - radz, r, g, b, a);
        }

        public static class GUI
        {
            public class RGBA
            {
                public int R, G, B, A = 255;
                public RGBA(int r, int g, int b, int a)
                {
                    R = r;
                    G = g;
                    B = b;
                    A = a;
                }

                public RGBA(RGBA clone)
                {
                    R = clone.R;
                    G = clone.G;
                    B = clone.B;
                    A = clone.A;
                }

                public RGBA()
                {

                }
            }

            public class Vector2
            {
                public float X, Y = 0.0f;
                public Vector2(float x, float y)
                {
                    X = x;
                    Y = y;
                }

                public Vector2(Vector2 clone)
                {
                    X = clone.X;
                    Y = clone.Y;
                }

                public Vector2()
                {

                }
            }

            private static bool CursorEnabled = false;
            private static Dictionary<string, RectObject> DrawObjects_Rect = new Dictionary<string, RectObject>();
            private static Dictionary<string, TextObject> DrawObjects_Text = new Dictionary<string, TextObject>();
            private static Dictionary<string, ButtonObject> DrawObjects_Button = new Dictionary<string, ButtonObject>();
            private static RectObject HoveredRect = null;
            public static float Lerp(float a, float b, float t)
            {
                if (t > 1.0f)
                    t = 1.0f;
                if (t < 0.0f)
                    t = 0.0f;
                return (1f - t) * a + t * b;
            }

            public static RectObject CreateBox(string Id, Vector2 Position, Vector2 Size, RGBA Color)
            {
                if (DrawObjects_Rect.ContainsKey(Id))
                    return null;
                RectObject obj = new RectObject(Id, Position, Size, Color);
                DrawObjects_Rect.Add(Id, obj);
                return obj;
            }

            public static TextObject CreateText(string Id, string Text, Vector2 Position, float Scale)
            {
                if (DrawObjects_Text.ContainsKey(Id))
                    return null;
                TextObject obj = new TextObject(Id, Text, Position, Scale);
                DrawObjects_Text.Add(Id, obj);
                return obj;
            }

            public static ButtonObject CreateButton(string Id, string Text, Vector2 Position, Vector2 Size, RGBA Color, RGBA TextColor = null)
            {
                if (DrawObjects_Button.ContainsKey(Id))
                    return null;
                ButtonObject obj = new ButtonObject(Id, Text, Position, Size, Color);
                if (TextColor != null)
                    obj.TextObject.Color = TextColor;
                DrawObjects_Button.Add(Id, obj);
                return obj;
            }

            public static void EnableAllButtons()
            {
                foreach (ButtonObject obj in DrawObjects_Button.Values)
                    obj.Enable();
            }

            public static void DisableAllButtons()
            {
                foreach (ButtonObject obj in DrawObjects_Button.Values)
                    obj.Disable();
            }

            public static void EnableAllButtonsByGroup(string group)
            {
                foreach (ButtonObject obj in DrawObjects_Button.Values)
                    if (obj.Group == group)
                        obj.Enable();
            }

            public static void StopAllAnimations()
            {
                foreach (ButtonObject obj in DrawObjects_Button.Values)
                    obj.StopAllAnimations();
            }

            public static void DisableAllButtonsByGroup(string group)
            {
                foreach (ButtonObject obj in DrawObjects_Button.Values)
                    if (obj.Group == group)
                        obj.Disable();
            }

            public static RectObject GetBoxById(string id)
            {
                RectObject obj;
                if (DrawObjects_Rect.TryGetValue(id, out obj))
                    return obj;
                return null;
            }

            public static TextObject GetTextById(string id)
            {
                TextObject obj;
                if (DrawObjects_Text.TryGetValue(id, out obj))
                    return obj;
                return null;
            }

            public static ButtonObject GetButtonById(string id)
            {
                ButtonObject obj;
                if (DrawObjects_Button.TryGetValue(id, out obj))
                    return obj;
                return null;
            }

            public static void DeleteBoxById(string id)
            {
                DrawObjects_Rect.Remove(id);
            }

            public static void DeleteTextById(string id)
            {
                DrawObjects_Text.Remove(id);
            }

            public static void DeleteButtonById(string id)
            {
                DrawObjects_Button.Remove(id);
                DrawObjects_Rect.Remove(id + "_Box");
                DrawObjects_Text.Remove(id + "_Text");
            }

            public static float GetDeltaTime()
            {
                return RAGE.Game.Misc.GetFrameTime();
            }

            public static float CursorDefaultX { get; private set; } = 0.5f;
            public static float CursorDefaultY { get; private set; } = 0.5f;

            public static void SetCursorDefaults(float x, float y)
            {
                CursorDefaultX = x;
                CursorDefaultY = y;
            }

            public static void EnableCursor()
            {
                CursorEnabled = true;
            }

            public static void ResetCursor()
            {
                RAGE.Game.Pad.SetCursorLocation(CursorDefaultX, CursorDefaultY);
            }

            public static void SetCursor(float x, float y)
            {
                RAGE.Game.Pad.SetCursorLocation(x, y);
            }

            public static void ToggleCursor()
            {
                CursorEnabled = !CursorEnabled;
                if (CursorEnabled)
                    EnableCursor();
                else
                    DisableCursor();
            }

            public static void DisableCursor()
            {
                CursorEnabled = false;
            }

            public static bool IsCursorEnabled()
            {
                return CursorEnabled;
            }

            public static float GetCursorX()
            {
                return RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.CursorX);
            }

            public static float GetCursorY()
            {
                return RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.CursorY);
            }

            public static bool WasAnyButtonPressedThisFrame = false;
            public static void Update()
            {
                if (CursorEnabled)
                {
                    RAGE.Game.Pad.DisableControlAction(0, (int)RAGE.Game.Control.LookLeftRight, true);
                    RAGE.Game.Pad.DisableControlAction(0, (int)RAGE.Game.Control.LookUpDown, true);
                    RAGE.Game.Ui.SetCursorSprite(1);
                    RAGE.Game.Ui.ShowCursorThisFrame();
                }

                float mouseX = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.CursorX);
                float mouseY = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.CursorY);
                bool Click = RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack);
                bool JustClick = RAGE.Game.Pad.IsControlJustPressed(0, (int)RAGE.Game.Control.Attack);
                WasAnyButtonPressedThisFrame = false;
                foreach (RectObject obj in DrawObjects_Rect.Values)
                {
                    obj.Draw(mouseX, mouseY, Click, JustClick, ref WasAnyButtonPressedThisFrame);
                }
                foreach (TextObject obj in DrawObjects_Text.Values)
                {
                    obj.Draw(mouseX, mouseY, Click, JustClick);
                }
            }

            public enum AnimationNames
            {
                MoveFromTo,
                SizeFromTo,
                HideFromScreenWhenDone,
                HideFromScreen,
                ShowOnScreen,
                ScaleTextFromTo,
                ColorFromTo,
                ColorTextFromTo,
                IgnoreMouse,
            }

            public class Anim
            {
                public AnimationNames AnimationType = 0;
                public Vector2 Data1;
                public Vector2 Data2;
                public float Data3;
                public float Data4;
                public RGBA Data5;
                public RGBA Data6;

                public Anim(AnimationNames type, Vector2 dat1 = null, Vector2 dat2 = null, float dat3 = 0f, float dat4 = 0f, RGBA dat5 = null, RGBA dat6 = null)
                {
                    AnimationType = type;
                    Data1 = dat1;
                    Data2 = dat2;
                    Data3 = dat3;
                    Data4 = dat4;
                    Data5 = dat5;
                    Data6 = dat6;
                }
            }

            public class Actions
            {
                public string AnimSetID = "";
                public List<Anim> Animations = new List<Anim>();
                public Action Invoker = null;
                public Actions MoveFromTo(Vector2 from, Vector2 to, float time)
                {
                    Animations.Add(new Anim(AnimationNames.MoveFromTo, new Vector2(from.X, from.Y), new Vector2(to.X, to.Y), time));
                    return this;
                }

                public Actions OnFinish(Action act)
                {
                    Invoker = act;
                    return this;
                }

                public Actions SetAnimName(string id)
                {
                    AnimSetID = id;
                    return this;
                }

                public Actions SizeFromTo(Vector2 from, Vector2 to, float time)
                {
                    Animations.Add(new Anim(AnimationNames.SizeFromTo, new Vector2(from.X, from.Y), new Vector2(to.X, to.Y), time));
                    return this;
                }

                public Actions HideFromScreen()
                {
                    Animations.Add(new Anim(AnimationNames.HideFromScreen));
                    return this;
                }

                public Actions IgnoreMouse()
                {
                    Animations.Add(new Anim(AnimationNames.IgnoreMouse));
                    return this;
                }

                public Actions HideFromScreenWhenDone()
                {
                    Animations.Add(new Anim(AnimationNames.HideFromScreenWhenDone));
                    return this;
                }

                public Actions ShowOnScreen()
                {
                    Animations.Add(new Anim(AnimationNames.ShowOnScreen));
                    return this;
                }

                public Actions ScaleTextFromTo(float from, float to, float time)
                {
                    Animations.Add(new Anim(AnimationNames.ScaleTextFromTo, new GUI.Vector2(from, to), null, time));
                    return this;
                }

                public Actions ColorFromTo(GUI.RGBA rgba1, GUI.RGBA rgba2, float time)
                {
                    Animations.Add(new Anim(AnimationNames.ColorFromTo, null, null, time, 0f, rgba1, rgba2));
                    return this;
                }

                public Actions ColorTextFromTo(GUI.RGBA rgba1, GUI.RGBA rgba2, float time)
                {
                    Animations.Add(new Anim(AnimationNames.ColorTextFromTo, null, null, time, 0f, rgba1, rgba2));
                    return this;
                }

                public Actions(string id = "")
                {
                    AnimSetID = id;
                }
            }

            public class ButtonObject
            {
                public string Id;
                public TextObject TextObject;
                public RectObject RectObject;
                public string Group;
                private List<Anim> AttachedAnimations_Enable = new List<Anim>();
                private List<Anim> AttachedAnimations_Disable = new List<Anim>();
                private Dictionary<string, List<Anim>> AttachedAnimations_Custom = new Dictionary<string, List<Anim>>();
                public bool IsEnabled = false;

                public void StopAllAnimations()
                {
                    RectObject.CurrentAnimID = "";
                    RectObject.StopAnimations();
                }

                public void FinishAllAnimations()
                {
                    RectObject.CurrentAnimID = "";
                    RectObject.FinishAnimations();
                }

                public ButtonObject SetGroup(string group)
                {
                    Group = group;
                    return this;
                }

                public void ExecuteCustomAnimation(string id)
                {
                    if (AttachedAnimations_Custom.ContainsKey(id))
                    {
                        RectObject.CurrentAnimID = id;
                        foreach (Anim anim in AttachedAnimations_Custom[id])
                        {
                            if (anim.AnimationType == AnimationNames.MoveFromTo)
                            {
                                RectObject.Position.X = anim.Data1.X;
                                RectObject.Position.Y = anim.Data1.Y;
                                RectObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                                TextObject.Position.X = anim.Data1.X;
                                TextObject.Position.Y = anim.Data1.Y;
                                TextObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                            }
                            else if (anim.AnimationType == AnimationNames.SizeFromTo)
                            {
                                RectObject.Size.X = anim.Data1.X;
                                RectObject.Size.Y = anim.Data1.Y;
                                RectObject.LerpWHTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                            }
                            else if (anim.AnimationType == AnimationNames.HideFromScreen)
                            {
                                TextObject.Enable = false;
                                RectObject.Enable = false;
                            }
                            else if (anim.AnimationType == AnimationNames.HideFromScreenWhenDone)
                            {
                                TextObject.SignalForDisable = true;
                                RectObject.SignalForDisable = true;
                            }
                            else if (anim.AnimationType == AnimationNames.ShowOnScreen)
                            {
                                TextObject.Enable = true;
                                RectObject.Enable = true;
                            }
                            else if (anim.AnimationType == AnimationNames.ScaleTextFromTo)
                            {
                                TextObject.Scale = anim.Data1.X;
                                TextObject.LerpScaleTo(anim.Data1.Y, anim.Data3, true);
                            }
                            else if (anim.AnimationType == AnimationNames.ColorFromTo)
                            {
                                RectObject.Color.R = anim.Data5.R;
                                RectObject.Color.G = anim.Data5.G;
                                RectObject.Color.B = anim.Data5.B;
                                RectObject.Color.A = anim.Data5.A;
                                RectObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                            }
                            else if (anim.AnimationType == AnimationNames.ColorTextFromTo)
                            {
                                //TextObject.Color = new RGBA(anim.Data5);
                                TextObject.Color.R = anim.Data5.R;
                                TextObject.Color.G = anim.Data5.G;
                                TextObject.Color.B = anim.Data5.B;
                                TextObject.Color.A = anim.Data5.A;
                                TextObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                            }
                        }
                    }
                }

                public ButtonObject IgnoreMouse(bool value)
                {
                    RectObject.IgnoreMouse = value;
                    return this;
                }

                public ButtonObject OnDisabled(Actions animation)
                {
                    foreach (Anim anim in animation.Animations)
                        AttachedAnimations_Disable.Add(anim);
                    RectObject.IgnoreMouseForIDs["Disabled"] = animation.Animations.Any(x => x.AnimationType == AnimationNames.IgnoreMouse);
                    RectObject.OnSetFinish["Disabled"] = animation.Invoker;
                    return this;
                }

                public ButtonObject OnEnabled(Actions animation, Action onStart = null)
                {
                    foreach (Anim anim in animation.Animations)
                        AttachedAnimations_Enable.Add(anim);
                    RectObject.IgnoreMouseForIDs["Enabled"] = animation.Animations.Any(x => x.AnimationType == AnimationNames.IgnoreMouse);
                    RectObject.OnSetFinish["Enabled"] = animation.Invoker;
                    if (onStart != null)
                        onStart.Invoke();
                    return this;
                }

                public ButtonObject OnCustomAnim(Actions animation)
                {
                    foreach (Anim anim in animation.Animations)
                        AttachedAnimations_Enable.Add(anim);
                    RectObject.IgnoreMouseForIDs[animation.AnimSetID] = animation.Animations.Any(x => x.AnimationType == AnimationNames.IgnoreMouse);
                    RectObject.OnSetFinish[animation.AnimSetID] = animation.Invoker;
                    return this;
                }

                public ButtonObject OnMouseEnter(Action act)
                {
                    RectObject.OnMouseEnter = act;
                    return this;
                }

                public ButtonObject OnMouseExit(Action act)
                {
                    RectObject.OnMouseLeave = act;
                    return this;
                }

                public ButtonObject OnMouseDown(Action act)
                {
                    RectObject.OnMouseDown = act;
                    return this;
                }

                public ButtonObject OnMouseUp(Action act)
                {
                    RectObject.OnMouseClick = act;
                    return this;
                }

                public ButtonObject OnDraw(Action act)
                {
                    RectObject.OnDraw = act;
                    return this;
                }

                public ButtonObject SetPosition(GUI.Vector2 Position)
                {
                    RectObject.Position = Position;
                    TextObject.Position = Position;
                    return this;
                }

                public ButtonObject SetLayer(int layer)
                {
                    RectObject.Layer = layer;
                    TextObject.Layer = layer;
                    return this;
                }

                public void LerpWHFromTo(Vector2 from, Vector2 to, float time)
                {
                    RectObject.Size.X = from.X;
                    RectObject.Size.Y = from.Y;
                    RectObject.LerpWHTo(to.X, to.Y, time, true);
                }

                public void LerpFromTo(Vector2 from, Vector2 to, float time, Action act = null)
                {
                    RectObject.Position.X = from.X;
                    RectObject.Position.Y = from.Y;
                    RectObject.LerpXYTo(to.X, to.Y, time, true, act);
                    TextObject.LerpXYTo(to.X, to.Y, time, true);
                }

                public void LerpColorFromTo(RGBA from, RGBA to, float time)
                {
                    RectObject.Color.R = from.R;
                    RectObject.Color.G = from.G;
                    RectObject.Color.B = from.B;
                    RectObject.Color.A = from.A;
                    RectObject.LerpRGBATo(to.R, to.G, to.B, to.A, time, true);
                }

                public void LerpTextColorFromTo(RGBA from, RGBA to, float time)
                {
                    TextObject.Color.R = from.R;
                    TextObject.Color.G = from.G;
                    TextObject.Color.B = from.B;
                    TextObject.Color.A = from.A;
                    TextObject.LerpRGBATo(to.R, to.G, to.B, to.A, time, true);
                }

                public GUI.Vector2 GetPosition()
                {
                    return new GUI.Vector2(RectObject.Position);
                }

                public void Disable()
                {
                    IsEnabled = false;
                    RectObject.CurrentAnimID = "Disabled";
                    if (AttachedAnimations_Disable.Any(x => x.AnimationType == AnimationNames.IgnoreMouse))
                    {
                        if (RectObject.OnMouseLeave != null && RectObject.DidMouseEnter)
                        {
                            RectObject.OnMouseLeave.Invoke();
                            HoveredRect = null;
                        }
                    }

                    foreach (Anim anim in AttachedAnimations_Disable)
                    {
                        if (anim.AnimationType == AnimationNames.MoveFromTo)
                        {
                            RectObject.Position.X = anim.Data1.X;
                            RectObject.Position.Y = anim.Data1.Y;
                            RectObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                            TextObject.Position.X = anim.Data1.X;
                            TextObject.Position.Y = anim.Data1.Y;
                            TextObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.SizeFromTo)
                        {
                            RectObject.Size.X = anim.Data1.X;
                            RectObject.Size.Y = anim.Data1.Y;
                            RectObject.LerpWHTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.HideFromScreen)
                        {
                            TextObject.Enable = false;
                            RectObject.Enable = false;
                        }
                        else if (anim.AnimationType == AnimationNames.HideFromScreenWhenDone)
                        {
                            TextObject.SignalForDisable = true;
                            RectObject.SignalForDisable = true;
                        }
                        else if (anim.AnimationType == AnimationNames.ShowOnScreen)
                        {
                            TextObject.Enable = true;
                            RectObject.Enable = true;
                        }
                        else if (anim.AnimationType == AnimationNames.ScaleTextFromTo)
                        {
                            TextObject.Scale = anim.Data1.X;
                            TextObject.LerpScaleTo(anim.Data1.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.ColorFromTo)
                        {
                            RectObject.Color.R = anim.Data5.R;
                            RectObject.Color.G = anim.Data5.G;
                            RectObject.Color.B = anim.Data5.B;
                            RectObject.Color.A = anim.Data5.A;
                            RectObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.ColorTextFromTo)
                        {
                            //TextObject.Color = new RGBA(anim.Data5);
                            TextObject.Color.R = anim.Data5.R;
                            TextObject.Color.G = anim.Data5.G;
                            TextObject.Color.B = anim.Data5.B;
                            TextObject.Color.A = anim.Data5.A;
                            TextObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                        }
                    }
                }

                public void Enable()
                {
                    IsEnabled = true;
                    RectObject.CurrentAnimID = "Enabled";
                    foreach (Anim anim in AttachedAnimations_Enable)
                    {
                        if (anim.AnimationType == AnimationNames.MoveFromTo)
                        {
                            RectObject.Position.X = anim.Data1.X;
                            RectObject.Position.Y = anim.Data1.Y;
                            RectObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                            TextObject.Position.X = anim.Data1.X;
                            TextObject.Position.Y = anim.Data1.Y;
                            TextObject.LerpXYTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.SizeFromTo)
                        {
                            //RectObject.Size = new Vector2(anim.Data1);
                            RectObject.Size.X = anim.Data1.X;
                            RectObject.Size.Y = anim.Data1.Y;
                            RectObject.LerpWHTo(anim.Data2.X, anim.Data2.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.HideFromScreen)
                        {
                            TextObject.Enable = false;
                            RectObject.Enable = false;
                        }
                        else if (anim.AnimationType == AnimationNames.HideFromScreenWhenDone)
                        {
                            TextObject.SignalForDisable = true;
                            RectObject.SignalForDisable = true;
                        }
                        else if (anim.AnimationType == AnimationNames.ShowOnScreen)
                        {
                            TextObject.Enable = true;
                            RectObject.Enable = true;
                        }
                        else if (anim.AnimationType == AnimationNames.ScaleTextFromTo)
                        {
                            TextObject.Scale = anim.Data1.X;
                            TextObject.LerpScaleTo(anim.Data1.Y, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.ColorFromTo)
                        {
                            //RectObject.Color = new RGBA(anim.Data5);
                            RectObject.Color.R = anim.Data5.R;
                            RectObject.Color.G = anim.Data5.G;
                            RectObject.Color.B = anim.Data5.B;
                            RectObject.Color.A = anim.Data5.A;
                            RectObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                        }
                        else if (anim.AnimationType == AnimationNames.ColorTextFromTo)
                        {
                            //TextObject.Color = new RGBA(anim.Data5);
                            TextObject.Color.R = anim.Data5.R;
                            TextObject.Color.G = anim.Data5.G;
                            TextObject.Color.B = anim.Data5.B;
                            TextObject.Color.A = anim.Data5.A;
                            TextObject.LerpRGBATo(anim.Data6.R, anim.Data6.G, anim.Data6.B, anim.Data6.A, anim.Data3, true);
                        }
                    }
                }

                public ButtonObject(string id, string text, GUI.Vector2 pos, GUI.Vector2 size, GUI.RGBA color)
                {
                    TextObject = GUI.CreateText(id + "_Text", text, pos, 0.25f);
                    RectObject = GUI.CreateBox(id + "_Box", pos, size, color);
                    Id = id;
                }
            }

            public class TextObject
            {
                public string Id;
                public int Layer = 0;
                public GUI.Vector2 Position = new GUI.Vector2(0.0f, 0.0f);
                public string Text;
                public float Scale;
                public bool Center = true;
                public int Font = 0;
                public bool DropShadow = false;
                public bool TextOutline = false;
                public GUI.RGBA Color = new GUI.RGBA(255, 255, 255, 255);

                public bool Enable = false;
                public bool SignalForDisable = false;

                Action OnScaleLerp = null;
                Action OnXLerp = null;
                Action OnYLerp = null;

                private bool RetainScaleLerp = false;
                private bool RetainXLerp = false;
                private bool RetainYLerp = false;

                private float CurScaleLerp = 0.0f;
                private float CurXLerp = 0.0f;
                private float CurYLerp = 0.0f;

                private float TimeScaleLerp = 0.0f;
                private float TimeXLerp = 0.0f;
                private float TimeYLerp = 0.0f;

                private float LerpToScale = 0.0f;
                private float LerpToX = 0.0f;
                private float LerpToY = 0.0f;

                private bool IsScaleLerp = false;
                private bool IsXLerp = false;
                private bool IsYLerp = false;

                private bool RetainALerp = false;
                private bool RetainRLerp = false;
                private bool RetainGLerp = false;
                private bool RetainBLerp = false;

                private float CurALerp = 0.0f;
                private float CurRLerp = 0.0f;
                private float CurGLerp = 0.0f;
                private float CurBLerp = 0.0f;

                private float TimeALerp = 0.0f;
                private float TimeRLerp = 0.0f;
                private float TimeGLerp = 0.0f;
                private float TimeBLerp = 0.0f;

                private int LerpToA = 0;
                private int LerpToR = 0;
                private int LerpToG = 0;
                private int LerpToB = 0;

                private bool IsALerp = false;
                private bool IsRLerp = false;
                private bool IsGLerp = false;
                private bool IsBLerp = false;

                public void LerpScaleTo(float scale, float sec, bool retainLerp, Action invoke = null)
                {
                    IsScaleLerp = true;
                    CurScaleLerp = 0;
                    TimeScaleLerp = sec;
                    LerpToScale = scale;
                    OnScaleLerp = invoke;
                    RetainScaleLerp = retainLerp;
                }

                public void LerpXTo(float x, float sec, bool retainLerp, Action invoke = null)
                {
                    IsXLerp = true;
                    CurXLerp = 0;
                    TimeXLerp = sec;
                    LerpToX = x;
                    OnXLerp = invoke;
                    RetainXLerp = retainLerp;
                }

                public void LerpYTo(float y, float sec, bool retainLerp, Action invoke = null)
                {
                    IsYLerp = true;
                    CurYLerp = 0;
                    TimeYLerp = sec;
                    LerpToY = y;
                    OnYLerp = invoke;
                    RetainYLerp = retainLerp;
                }

                public void LerpXYTo(float x, float y, float sec, bool retainLerp, Action invoke = null)
                {
                    LerpXTo(x, sec, retainLerp, invoke);
                    LerpYTo(y, sec, retainLerp);
                }

                public void LerpATo(int a, float sec, bool retainLerp)
                {
                    IsALerp = true;
                    CurALerp = 0;
                    TimeALerp = sec;
                    LerpToA = a;
                    RetainALerp = retainLerp;
                }

                public void LerpRTo(int r, float sec, bool retainLerp)
                {
                    IsRLerp = true;
                    CurRLerp = 0;
                    TimeRLerp = sec;
                    LerpToR = r;
                    RetainRLerp = retainLerp;
                }

                public void LerpGTo(int g, float sec, bool retainLerp)
                {
                    IsGLerp = true;
                    CurGLerp = 0;
                    TimeGLerp = sec;
                    LerpToG = g;
                    RetainGLerp = retainLerp;
                }

                public void LerpBTo(int b, float sec, bool retainLerp)
                {
                    IsBLerp = true;
                    CurBLerp = 0;
                    TimeBLerp = sec;
                    LerpToB = b;
                    RetainBLerp = retainLerp;
                }

                public void LerpRGBATo(int r, int g, int b, int a, float sec, bool retainLerp)
                {
                    LerpRTo(r, sec, retainLerp);
                    LerpGTo(g, sec, retainLerp);
                    LerpBTo(b, sec, retainLerp);
                    LerpATo(a, sec, retainLerp);
                }

                public TextObject(string id, string text, GUI.Vector2 pos, float scale)
                {
                    Id = id;
                    Text = text;
                    Position = pos;
                    Scale = scale;
                }

                public void Draw(float mouseX, float mouseY, bool click, bool justClick)
                {
                    //Always update lerps
                    float curScale = Scale;
                    float curX = Position.X;
                    float curY = Position.Y;
                    int curA = Color.A;
                    int curR = Color.R;
                    int curB = Color.B;
                    int curG = Color.G;
                    if (IsScaleLerp)
                    {
                        if (CurScaleLerp >= TimeScaleLerp)
                        {
                            IsScaleLerp = false;
                            if (RetainScaleLerp)
                            {
                                Scale = LerpToScale;
                                curScale = Scale;
                            }
                            if (OnScaleLerp != null)
                                OnScaleLerp.Invoke();
                        }
                        else
                        {
                            CurScaleLerp += GUI.GetDeltaTime();
                            curScale = GUI.Lerp(Scale, LerpToScale, CurScaleLerp / TimeScaleLerp);
                        }
                    }
                    if (IsXLerp)
                    {
                        if (CurXLerp >= TimeXLerp)
                        {
                            IsXLerp = false;
                            if (RetainXLerp)
                            {
                                Position.X = LerpToX;
                                curX = Position.X;
                            }
                            if (OnXLerp != null)
                                OnXLerp.Invoke();
                        }
                        else
                        {
                            CurXLerp += GUI.GetDeltaTime();
                            curX = GUI.Lerp(Position.X, LerpToX, CurXLerp / TimeXLerp);
                        }
                    }

                    if (IsYLerp)
                    {
                        if (CurYLerp >= TimeYLerp)
                        {
                            IsYLerp = false;
                            if (RetainYLerp)
                            {
                                Position.Y = LerpToY;
                                curY = Position.Y;
                            }
                            if (OnYLerp != null)
                                OnYLerp.Invoke();
                        }
                        else
                        {
                            CurYLerp += GUI.GetDeltaTime();
                            curY = GUI.Lerp(Position.Y, LerpToY, CurYLerp / TimeYLerp);
                        }
                    }

                    if (IsALerp)
                    {
                        if (CurALerp >= TimeALerp)
                        {
                            IsALerp = false;
                            if (RetainALerp)
                            {
                                Color.A = LerpToA;
                                curA = Color.A;
                            }
                        }
                        else
                        {
                            CurALerp += GUI.GetDeltaTime();
                            curA = Convert.ToInt32(GUI.Lerp(Color.A, LerpToA, CurALerp / TimeALerp));
                        }
                    }

                    if (IsRLerp)
                    {
                        if (CurRLerp >= TimeRLerp)
                        {
                            IsRLerp = false;
                            if (RetainRLerp)
                            {
                                Color.R = LerpToR;
                                curR = Color.R;
                            }
                        }
                        else
                        {
                            CurRLerp += GUI.GetDeltaTime();
                            curR = Convert.ToInt32(GUI.Lerp(Color.R, LerpToR, CurRLerp / TimeRLerp));
                        }
                    }

                    if (IsGLerp)
                    {
                        if (CurGLerp >= TimeGLerp)
                        {
                            IsGLerp = false;
                            if (RetainGLerp)
                            {
                                Color.G = LerpToG;
                                curG = Color.G;
                            }
                        }
                        else
                        {
                            CurGLerp += GUI.GetDeltaTime();
                            curG = Convert.ToInt32(GUI.Lerp(Color.G, LerpToG, CurGLerp / TimeGLerp));
                        }
                    }

                    if (IsBLerp)
                    {
                        if (CurBLerp >= TimeBLerp)
                        {
                            IsBLerp = false;
                            if (RetainBLerp)
                            {
                                Color.B = LerpToB;
                                curB = Color.B;
                            }
                        }
                        else
                        {
                            CurBLerp += GUI.GetDeltaTime();
                            curB = Convert.ToInt32(GUI.Lerp(Color.B, LerpToB, CurBLerp / TimeBLerp));
                        }
                    }

                    if (SignalForDisable && !IsALerp && !IsBLerp && !IsRLerp && !IsGLerp && !IsScaleLerp && !IsXLerp && !IsYLerp)
                    {
                        Enable = false;
                        SignalForDisable = false;
                    }

                    if (!Enable)
                        return;

                    RAGE.Game.Ui.BeginTextCommandDisplayText("STRING");
                    RAGE.Game.Ui.AddTextComponentSubstringPlayerName(Text);
                    RAGE.Game.Ui.SetTextScale(1.0f, curScale);
                    RAGE.Game.Ui.SetTextColour(curR, curG, curB, curA);
                    RAGE.Game.Ui.SetTextCentre(Center);
                    RAGE.Game.Ui.SetTextJustification(0);
                    RAGE.Game.Ui.SetTextFont(Font);

                    if (TextOutline)
                        RAGE.Game.Ui.SetTextOutline();
                    if (DropShadow)
                        RAGE.Game.Ui.SetTextDropShadow();

                    float size = RAGE.Game.Ui.GetTextScaleHeight(curScale, Font);
                    RAGE.Game.Graphics.Set2dLayer(Layer);
                    RAGE.Game.Ui.EndTextCommandDisplayText(curX, curY - (size / 2.0f) - (0.00138888f * 5.0f), 0);
                }
            }

            public class RectObject
            {
                public string Id;
                public int Layer = 0;
                public GUI.Vector2 Position = new GUI.Vector2(0.0f, 0.0f);
                public GUI.Vector2 Size = new GUI.Vector2(0.0f, 0.0f);
                public GUI.RGBA Color = new GUI.RGBA(255, 255, 255, 255);
                public float Rotation = 0f;

                public bool SignalForDisable = false;
                public bool Enable = false;
                public bool IgnoreMouse = false;

                public bool DidMouseEnter = false;
                public bool DidMouseLeave = false;
                private bool DidMouseClick = false;

                public string CurrentAnimID = "";
                public Dictionary<string, bool> IgnoreMouseForIDs = new Dictionary<string, bool>();
                public Dictionary<string, Action> OnSetFinish = new Dictionary<string, Action>();

                public Action OnMouseEnter = null;
                public Action OnMouseLeave = null;
                public Action OnMouseClick = null;
                public Action OnMouseDown = null;
                public Action OnDraw = null;

                private Action OnXLerp = null;
                private Action OnYLerp = null;
                private Action OnWLerp = null;
                private Action OnHLerp = null;
                private Action OnALerp = null;
                private Action OnRLerp = null;
                private Action OnGLerp = null;
                private Action OnBLerp = null;
                private Action OnRotLerp = null;

                private bool RetainXLerp = false;
                private bool RetainYLerp = false;
                private bool RetainWLerp = false;
                private bool RetainHLerp = false;
                private bool RetainALerp = false;
                private bool RetainRLerp = false;
                private bool RetainGLerp = false;
                private bool RetainBLerp = false;
                private bool RetainRotLerp = false;

                private float CurXLerp = 0.0f;
                private float CurYLerp = 0.0f;
                private float CurWLerp = 0.0f;
                private float CurHLerp = 0.0f;
                private float CurALerp = 0.0f;
                private float CurRLerp = 0.0f;
                private float CurGLerp = 0.0f;
                private float CurBLerp = 0.0f;
                private float CurRotLerp = 0.0f;

                private float TimeXLerp = 0.0f;
                private float TimeYLerp = 0.0f;
                private float TimeWLerp = 0.0f;
                private float TimeHLerp = 0.0f;
                private float TimeALerp = 0.0f;
                private float TimeRLerp = 0.0f;
                private float TimeGLerp = 0.0f;
                private float TimeBLerp = 0.0f;
                private float TimeRotLerp = 0.0f;

                private float LerpToX = 0.0f;
                private float LerpToY = 0.0f;
                private float LerpToWidth = 0.0f;
                private float LerpToHeight = 0.0f;
                private float LerpToRotation = 0.0f;

                private int LerpToA = 0;
                private int LerpToR = 0;
                private int LerpToG = 0;
                private int LerpToB = 0;

                private bool IsXLerp = false;
                private bool IsYLerp = false;
                private bool IsWLerp = false;
                private bool IsHLerp = false;
                private bool IsALerp = false;
                private bool IsRLerp = false;
                private bool IsGLerp = false;
                private bool IsBLerp = false;
                private bool IsRotLerp = false;

                public void StopAnimations()
                {
                    IsXLerp = false;
                    IsYLerp = false;
                    IsWLerp = false;
                    IsHLerp = false;
                    IsALerp = false;
                    IsRLerp = false;
                    IsGLerp = false;
                    IsBLerp = false;
                    IsRotLerp = false;
                }

                public void FinishAnimations()
                {
                    CurXLerp = TimeXLerp;
                    CurYLerp = TimeYLerp;
                    CurWLerp = TimeWLerp;
                    CurHLerp = TimeHLerp;
                    CurALerp = TimeALerp;
                    CurRLerp = TimeRLerp;
                    CurGLerp = TimeGLerp;
                    CurBLerp = TimeBLerp;
                    CurRotLerp = TimeRotLerp;
                }

                public void LerpXTo(float x, float sec, bool retainLerp, Action invoke = null)
                {
                    IsXLerp = true;
                    CurXLerp = 0;
                    TimeXLerp = sec;
                    OnXLerp = invoke;
                    LerpToX = x;
                    RetainXLerp = retainLerp;
                }

                public void LerpYTo(float y, float sec, bool retainLerp, Action invoke = null)
                {
                    IsYLerp = true;
                    CurYLerp = 0;
                    TimeYLerp = sec;
                    OnYLerp = invoke;
                    LerpToY = y;
                    RetainYLerp = retainLerp;
                }

                public void LerpXYTo(float x, float y, float sec, bool retainLerp, Action invoke = null)
                {
                    LerpXTo(x, sec, retainLerp, invoke);
                    LerpYTo(y, sec, retainLerp);
                }

                public void LerpWTo(float w, float sec, bool retainLerp, Action invoke = null)
                {
                    IsWLerp = true;
                    CurWLerp = 0;
                    TimeWLerp = sec;
                    OnWLerp = invoke;
                    LerpToWidth = w;
                    RetainWLerp = retainLerp;
                }

                public void LerpHTo(float h, float sec, bool retainLerp, Action invoke = null)
                {
                    IsHLerp = true;
                    CurHLerp = 0;
                    TimeHLerp = sec;
                    OnHLerp = invoke;
                    LerpToHeight = h;
                    RetainHLerp = retainLerp;
                }

                public void LerpWHTo(float w, float h, float sec, bool retainLerp, Action invoke = null)
                {
                    LerpWTo(w, sec, retainLerp, invoke);
                    LerpHTo(h, sec, retainLerp);
                }

                public void LerpATo(int a, float sec, bool retainLerp, Action invoke = null)
                {
                    IsALerp = true;
                    CurALerp = 0;
                    TimeALerp = sec;
                    OnALerp = invoke;
                    LerpToA = a;
                    RetainALerp = retainLerp;
                }

                public void LerpRTo(int r, float sec, bool retainLerp, Action invoke = null)
                {
                    IsRLerp = true;
                    CurRLerp = 0;
                    TimeRLerp = sec;
                    OnRLerp = invoke;
                    LerpToR = r;
                    RetainRLerp = retainLerp;
                }

                public void LerpGTo(int g, float sec, bool retainLerp, Action invoke = null)
                {
                    IsGLerp = true;
                    CurGLerp = 0;
                    TimeGLerp = sec;
                    OnGLerp = invoke;
                    LerpToG = g;
                    RetainGLerp = retainLerp;
                }

                public void LerpBTo(int b, float sec, bool retainLerp, Action invoke = null)
                {
                    IsBLerp = true;
                    CurBLerp = 0;
                    TimeBLerp = sec;
                    OnBLerp = invoke;
                    LerpToB = b;
                    RetainBLerp = retainLerp;
                }

                public void LerpRotTo(float r, float sec, bool retainLerp, Action invoke = null)
                {
                    IsRotLerp = true;
                    CurRotLerp = 0;
                    TimeRotLerp = sec;
                    OnRotLerp = invoke;
                    LerpToRotation = r;
                    RetainRotLerp = retainLerp;
                }

                public void LerpRGBATo(int r, int g, int b, int a, float sec, bool retainLerp, Action invoke = null)
                {
                    LerpRTo(r, sec, retainLerp, invoke);
                    LerpGTo(g, sec, retainLerp);
                    LerpBTo(b, sec, retainLerp);
                    LerpATo(a, sec, retainLerp);
                }

                public RectObject(string id, GUI.Vector2 pos, GUI.Vector2 size, GUI.RGBA color)
                {
                    Id = id;
                    Position = pos;
                    Size = size;
                    Color = color;
                }

                public void Draw(float mouseX, float mouseY, bool click, bool justClick, ref bool pressed)
                {
                    //Always update lerps
                    float curX = Position.X;
                    float curY = Position.Y;
                    float curW = Size.X;
                    float curH = Size.Y;
                    int curA = Color.A;
                    int curR = Color.R;
                    int curG = Color.G;
                    int curB = Color.B;
                    float curRot = Rotation;

                    if (IsXLerp)
                    {
                        if (CurXLerp >= TimeXLerp)
                        {
                            IsXLerp = false;
                            if (RetainXLerp)
                            {
                                Position.X = LerpToX;
                                curX = Position.X;
                            }
                            if (OnXLerp != null)
                                OnXLerp.Invoke();
                        }
                        else
                        {
                            CurXLerp += GUI.GetDeltaTime();
                            curX = GUI.Lerp(Position.X, LerpToX, CurXLerp / TimeXLerp);
                        }
                    }

                    if (IsYLerp)
                    {
                        if (CurYLerp >= TimeYLerp)
                        {
                            IsYLerp = false;
                            if (RetainYLerp)
                            {
                                Position.Y = LerpToY;
                                curY = Position.Y;
                            }
                            if (OnYLerp != null)
                                OnYLerp.Invoke();
                        }
                        else
                        {
                            CurYLerp += GUI.GetDeltaTime();
                            curY = GUI.Lerp(Position.Y, LerpToY, CurYLerp / TimeYLerp);
                        }
                    }

                    if (IsWLerp)
                    {
                        if (CurWLerp >= TimeWLerp)
                        {
                            IsWLerp = false;
                            if (RetainWLerp)
                            {
                                Size.X = LerpToWidth;
                                curW = Size.X;
                            }
                            if (OnWLerp != null)
                                OnWLerp.Invoke();
                        }
                        else
                        {
                            CurWLerp += GUI.GetDeltaTime();
                            curW = GUI.Lerp(Size.X, LerpToWidth, CurWLerp / TimeWLerp);
                        }
                    }

                    if (IsHLerp)
                    {
                        if (CurHLerp >= TimeHLerp)
                        {
                            IsHLerp = false;
                            if (RetainHLerp)
                            {
                                Size.Y = LerpToHeight;
                                curH = Size.Y;
                            }
                            if (OnHLerp != null)
                                OnHLerp.Invoke();
                        }
                        else
                        {
                            CurHLerp += GUI.GetDeltaTime();
                            curH = GUI.Lerp(Size.Y, LerpToHeight, CurHLerp / TimeHLerp);
                        }
                    }

                    if (IsALerp)
                    {
                        if (CurALerp >= TimeALerp)
                        {
                            IsALerp = false;
                            if (RetainALerp)
                            {
                                Color.A = LerpToA;
                                curA = Color.A;
                            }
                            if (OnALerp != null)
                                OnALerp.Invoke();
                        }
                        else
                        {
                            CurALerp += GUI.GetDeltaTime();
                            curA = Convert.ToInt32(GUI.Lerp(Color.A, LerpToA, CurALerp / TimeALerp));
                        }
                    }

                    if (IsRLerp)
                    {
                        if (CurRLerp >= TimeRLerp)
                        {
                            IsRLerp = false;
                            if (RetainRLerp)
                            {
                                Color.R = LerpToR;
                                curR = Color.R;
                            }
                            if (OnRLerp != null)
                                OnRLerp.Invoke();
                        }
                        else
                        {
                            CurRLerp += GUI.GetDeltaTime();
                            curR = Convert.ToInt32(GUI.Lerp(Color.R, LerpToR, CurRLerp / TimeRLerp));
                        }
                    }

                    if (IsGLerp)
                    {
                        if (CurGLerp >= TimeGLerp)
                        {
                            IsGLerp = false;
                            if (RetainGLerp)
                            {
                                Color.G = LerpToG;
                                curG = Color.G;
                            }
                            if (OnGLerp != null)
                                OnGLerp.Invoke();
                        }
                        else
                        {
                            CurGLerp += GUI.GetDeltaTime();
                            curG = Convert.ToInt32(GUI.Lerp(Color.G, LerpToG, CurGLerp / TimeGLerp));
                        }
                    }

                    if (IsBLerp)
                    {
                        if (CurBLerp >= TimeBLerp)
                        {
                            IsBLerp = false;
                            if (RetainBLerp)
                            {
                                Color.B = LerpToB;
                                curB = Color.B;
                            }
                            if (OnBLerp != null)
                                OnBLerp.Invoke();
                        }
                        else
                        {
                            CurBLerp += GUI.GetDeltaTime();
                            curB = Convert.ToInt32(GUI.Lerp(Color.B, LerpToB, CurBLerp / TimeBLerp));
                        }
                    }

                    if (IsRotLerp)
                    {
                        if (CurRotLerp >= TimeRotLerp)
                        {
                            IsRotLerp = false;
                            if (RetainRotLerp)
                            {
                                Rotation = LerpToRotation;
                                curRot = Rotation;
                            }
                            if (OnRotLerp != null)
                                OnRotLerp.Invoke();
                        }
                        else
                        {
                            CurRotLerp += GUI.GetDeltaTime();
                            curRot = GUI.Lerp(Rotation, LerpToRotation, CurRotLerp / TimeRotLerp);
                        }
                    }

                    if (SignalForDisable && !IsALerp && !IsBLerp && !IsRLerp && !IsGLerp && !IsWLerp && !IsHLerp && !IsXLerp && !IsYLerp)
                    {
                        Enable = false;
                        SignalForDisable = false;
                    }

                    if (!IsALerp && !IsBLerp && !IsRLerp && !IsGLerp && !IsWLerp && !IsHLerp && !IsXLerp && !IsYLerp)
                    {
                        if (OnSetFinish.ContainsKey(CurrentAnimID))
                            if (OnSetFinish[CurrentAnimID] != null)
                                OnSetFinish[CurrentAnimID].Invoke();
                        CurrentAnimID = "";
                    }

                    if (!Enable)
                        return;

                    DidMouseLeave = false;

                    if (!Marker_X_Rotate_Clicked && !Marker_Y_Rotate_Clicked && !Marker_Z_Rotate_Clicked && !Marker_X_Move_Clicked && !Marker_Y_Move_Clicked && !Marker_Z_Move_Clicked)
                    {
                        if (CurrentAnimID != "" && IgnoreMouseForIDs.ContainsKey(CurrentAnimID))
                        {
                            if (!IgnoreMouseForIDs[CurrentAnimID] && !IgnoreMouse)
                            {
                                if (mouseX > curX - curW / 2 && mouseX < curX + curW / 2)
                                {
                                    if (mouseY > curY - curH / 2 && mouseY < curY + curH / 2)
                                    {
                                        if (!DidMouseEnter)
                                        {
                                            if (HoveredRect == null)
                                            {
                                                if (OnMouseEnter != null)
                                                    OnMouseEnter.Invoke();
                                                DidMouseEnter = true;
                                                DidMouseLeave = false;
                                                HoveredRect = this;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (DidMouseEnter)
                                        {
                                            DidMouseEnter = false;
                                            DidMouseLeave = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (DidMouseEnter)
                                    {
                                        DidMouseEnter = false;
                                        DidMouseLeave = true;
                                    }
                                }

                                if (DidMouseLeave)
                                {
                                    if (OnMouseLeave != null)
                                        OnMouseLeave.Invoke();
                                    DidMouseEnter = false;
                                    HoveredRect = null;
                                }

                                if (DidMouseEnter)
                                {
                                    if (justClick)
                                    {
                                        if (OnMouseDown != null)
                                            OnMouseDown.Invoke();
                                        pressed = true;
                                    }
                                    if (click)
                                    {
                                        if (OnMouseClick != null)
                                            OnMouseClick.Invoke();
                                        pressed = true;
                                    }
                                }
                            }
                            else
                            {
                                DidMouseEnter = false;
                            }
                        }
                        else
                        {
                            if (!IgnoreMouse)
                            {
                                if (mouseX > curX - curW / 2 && mouseX < curX + curW / 2)
                                {
                                    if (mouseY > curY - curH / 2 && mouseY < curY + curH / 2)
                                    {
                                        if (!DidMouseEnter)
                                        {
                                            if (HoveredRect == null)
                                            {
                                                if (OnMouseEnter != null)
                                                    OnMouseEnter.Invoke();
                                                DidMouseEnter = true;
                                                DidMouseLeave = false;
                                                HoveredRect = this;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (DidMouseEnter)
                                        {
                                            DidMouseEnter = false;
                                            DidMouseLeave = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (DidMouseEnter)
                                    {
                                        DidMouseEnter = false;
                                        DidMouseLeave = true;
                                    }
                                }

                                if (DidMouseLeave)
                                {
                                    if (OnMouseLeave != null)
                                        OnMouseLeave.Invoke();
                                    DidMouseEnter = false;
                                    HoveredRect = null;
                                }

                                if (DidMouseEnter)
                                {
                                    if (justClick)
                                    {
                                        if (OnMouseDown != null)
                                            OnMouseDown.Invoke();
                                        pressed = true;
                                    }
                                    if (click)
                                    {
                                        if (OnMouseClick != null)
                                            OnMouseClick.Invoke();
                                        pressed = true;
                                    }
                                }
                            }
                        }
                    }

                    RAGE.Game.Graphics.Set2dLayer(Layer);
                    RAGE.Game.Graphics.DrawSprite("invalid_dict", "invalid_text", curX, curY, curW, curH, curRot, curR, curG, curB, curA, 0);

                    if (OnDraw != null)
                        OnDraw.Invoke();
                    //RAGE.Game.Graphics.DrawRect(curX, curY, curW, curH, curR, curG, curB, curA, 0);
                }
            }
        }
    }

    public static class RAGEInput
    {
        public static Dictionary<VirtualKey, bool> PreviousStates = new Dictionary<VirtualKey, bool>();
        public static Dictionary<VirtualKey, bool> CurrentStates = new Dictionary<VirtualKey, bool>();
        public static bool LostFocus = false;

        static RAGEInput()
        {
            foreach (VirtualKey key in Enum.GetValues(typeof(VirtualKey)))
            {
                PreviousStates[key] = false;
                CurrentStates[key] = false;
            }
        }

        public static void Update()
        {
            PreviousStates = CurrentStates;
            CurrentStates = new Dictionary<VirtualKey, bool>();
            foreach (VirtualKey key in PreviousStates.Keys)
                CurrentStates[key] = RAGE.Input.IsDown((int)key);
        }

        public static bool IsMouseLeftJustReleased()
        {
            return RAGE.Game.Pad.IsControlJustReleased(0, (int)RAGE.Game.Control.Attack);
        }

        public static bool IsMouseLeftJustPressed()
        {
            return RAGE.Game.Pad.IsControlJustPressed(0, (int)RAGE.Game.Control.Attack);
        }

        public static bool IsMouseLeftPressed()
        {
            return RAGE.Game.Pad.IsControlPressed(0, (int)RAGE.Game.Control.Attack);
        }

        public static bool IsKeyJustReleased(VirtualKey key)
        {
            if (LostFocus)
                return false;
            return PreviousStates[key] == true && CurrentStates[key] == false;
        }

        public static bool IsKeyJustPressed(VirtualKey key)
        {
            if (LostFocus)
                return false;
            return PreviousStates[key] == false && CurrentStates[key] == true;
        }

        public static bool IsKeyPressed(VirtualKey key)
        {
            if (LostFocus)
                return false;
            return CurrentStates[key];
        }

        public static bool IsKeyReleased(VirtualKey key)
        {
            return !IsKeyPressed(key);
        }

        public enum VirtualKey : int
        {
            ///<summary>
            ///Left mouse button
            ///</summary>
            LBUTTON = 0x01,
            ///<summary>
            ///Right mouse button
            ///</summary>
            RBUTTON = 0x02,
            ///<summary>
            ///Control-break processing
            ///</summary>
            CANCEL = 0x03,
            ///<summary>
            ///Middle mouse button (three-button mouse)
            ///</summary>
            MBUTTON = 0x04,
            ///<summary>
            ///Windows 2000/XP: X1 mouse button
            ///</summary>
            XBUTTON1 = 0x05,
            ///<summary>
            ///Windows 2000/XP: X2 mouse button
            ///</summary>
            XBUTTON2 = 0x06,
            ///<summary>
            ///BACKSPACE key
            ///</summary>
            BACK = 0x08,
            ///<summary>
            ///TAB key
            ///</summary>
            TAB = 0x09,
            ///<summary>
            ///CLEAR key
            ///</summary>
            CLEAR = 0x0C,
            ///<summary>
            ///ENTER key
            ///</summary>
            RETURN = 0x0D,
            ///<summary>
            ///SHIFT key
            ///</summary>
            SHIFT = 0x10,
            ///<summary>
            ///CTRL key
            ///</summary>
            CONTROL = 0x11,
            ///<summary>
            ///ALT key
            ///</summary>
            MENU = 0x12,
            ///<summary>
            ///PAUSE key
            ///</summary>
            PAUSE = 0x13,
            ///<summary>
            ///CAPS LOCK key
            ///</summary>
            CAPITAL = 0x14,
            ///<summary>
            ///Input Method Editor (IME) Kana mode
            ///</summary>
            KANA = 0x15,
            ///<summary>
            ///IME Hangul mode
            ///</summary>
            HANGUL = 0x15,
            ///<summary>
            ///IME Junja mode
            ///</summary>
            JUNJA = 0x17,
            ///<summary>
            ///IME final mode
            ///</summary>
            FINAL = 0x18,
            ///<summary>
            ///IME Hanja mode
            ///</summary>
            HANJA = 0x19,
            ///<summary>
            ///IME Kanji mode
            ///</summary>
            KANJI = 0x19,
            ///<summary>
            ///ESC key
            ///</summary>
            ESCAPE = 0x1B,
            ///<summary>
            ///IME convert
            ///</summary>
            CONVERT = 0x1C,
            ///<summary>
            ///IME nonconvert
            ///</summary>
            NONCONVERT = 0x1D,
            ///<summary>
            ///IME accept
            ///</summary>
            ACCEPT = 0x1E,
            ///<summary>
            ///IME mode change request
            ///</summary>
            MODECHANGE = 0x1F,
            ///<summary>
            ///SPACEBAR
            ///</summary>
            SPACE = 0x20,
            ///<summary>
            ///PAGE UP key
            ///</summary>
            PRIOR = 0x21,
            ///<summary>
            ///PAGE DOWN key
            ///</summary>
            NEXT = 0x22,
            ///<summary>
            ///END key
            ///</summary>
            END = 0x23,
            ///<summary>
            ///HOME key
            ///</summary>
            HOME = 0x24,
            ///<summary>
            ///LEFT ARROW key
            ///</summary>
            LEFT = 0x25,
            ///<summary>
            ///UP ARROW key
            ///</summary>
            UP = 0x26,
            ///<summary>
            ///RIGHT ARROW key
            ///</summary>
            RIGHT = 0x27,
            ///<summary>
            ///DOWN ARROW key
            ///</summary>
            DOWN = 0x28,
            ///<summary>
            ///SELECT key
            ///</summary>
            SELECT = 0x29,
            ///<summary>
            ///PRINT key
            ///</summary>
            PRINT = 0x2A,
            ///<summary>
            ///EXECUTE key
            ///</summary>
            EXECUTE = 0x2B,
            ///<summary>
            ///PRINT SCREEN key
            ///</summary>
            SNAPSHOT = 0x2C,
            ///<summary>
            ///INS key
            ///</summary>
            INSERT = 0x2D,
            ///<summary>
            ///DEL key
            ///</summary>
            DELETE = 0x2E,
            ///<summary>
            ///HELP key
            ///</summary>
            HELP = 0x2F,
            ///<summary>
            ///0 key
            ///</summary>
            KEY_0 = 0x30,
            ///<summary>
            ///1 key
            ///</summary>
            KEY_1 = 0x31,
            ///<summary>
            ///2 key
            ///</summary>
            KEY_2 = 0x32,
            ///<summary>
            ///3 key
            ///</summary>
            KEY_3 = 0x33,
            ///<summary>
            ///4 key
            ///</summary>
            KEY_4 = 0x34,
            ///<summary>
            ///5 key
            ///</summary>
            KEY_5 = 0x35,
            ///<summary>
            ///6 key
            ///</summary>
            KEY_6 = 0x36,
            ///<summary>
            ///7 key
            ///</summary>
            KEY_7 = 0x37,
            ///<summary>
            ///8 key
            ///</summary>
            KEY_8 = 0x38,
            ///<summary>
            ///9 key
            ///</summary>
            KEY_9 = 0x39,
            ///<summary>
            ///A key
            ///</summary>
            KEY_A = 0x41,
            ///<summary>
            ///B key
            ///</summary>
            KEY_B = 0x42,
            ///<summary>
            ///C key
            ///</summary>
            KEY_C = 0x43,
            ///<summary>
            ///D key
            ///</summary>
            KEY_D = 0x44,
            ///<summary>
            ///E key
            ///</summary>
            KEY_E = 0x45,
            ///<summary>
            ///F key
            ///</summary>
            KEY_F = 0x46,
            ///<summary>
            ///G key
            ///</summary>
            KEY_G = 0x47,
            ///<summary>
            ///H key
            ///</summary>
            KEY_H = 0x48,
            ///<summary>
            ///I key
            ///</summary>
            KEY_I = 0x49,
            ///<summary>
            ///J key
            ///</summary>
            KEY_J = 0x4A,
            ///<summary>
            ///K key
            ///</summary>
            KEY_K = 0x4B,
            ///<summary>
            ///L key
            ///</summary>
            KEY_L = 0x4C,
            ///<summary>
            ///M key
            ///</summary>
            KEY_M = 0x4D,
            ///<summary>
            ///N key
            ///</summary>
            KEY_N = 0x4E,
            ///<summary>
            ///O key
            ///</summary>
            KEY_O = 0x4F,
            ///<summary>
            ///P key
            ///</summary>
            KEY_P = 0x50,
            ///<summary>
            ///Q key
            ///</summary>
            KEY_Q = 0x51,
            ///<summary>
            ///R key
            ///</summary>
            KEY_R = 0x52,
            ///<summary>
            ///S key
            ///</summary>
            KEY_S = 0x53,
            ///<summary>
            ///T key
            ///</summary>
            KEY_T = 0x54,
            ///<summary>
            ///U key
            ///</summary>
            KEY_U = 0x55,
            ///<summary>
            ///V key
            ///</summary>
            KEY_V = 0x56,
            ///<summary>
            ///W key
            ///</summary>
            KEY_W = 0x57,
            ///<summary>
            ///X key
            ///</summary>
            KEY_X = 0x58,
            ///<summary>
            ///Y key
            ///</summary>
            KEY_Y = 0x59,
            ///<summary>
            ///Z key
            ///</summary>
            KEY_Z = 0x5A,
            ///<summary>
            ///Left Windows key (Microsoft Natural keyboard) 
            ///</summary>
            LWIN = 0x5B,
            ///<summary>
            ///Right Windows key (Natural keyboard)
            ///</summary>
            RWIN = 0x5C,
            ///<summary>
            ///Applications key (Natural keyboard)
            ///</summary>
            APPS = 0x5D,
            ///<summary>
            ///Computer Sleep key
            ///</summary>
            SLEEP = 0x5F,
            ///<summary>
            ///Numeric keypad 0 key
            ///</summary>
            NUMPAD0 = 0x60,
            ///<summary>
            ///Numeric keypad 1 key
            ///</summary>
            NUMPAD1 = 0x61,
            ///<summary>
            ///Numeric keypad 2 key
            ///</summary>
            NUMPAD2 = 0x62,
            ///<summary>
            ///Numeric keypad 3 key
            ///</summary>
            NUMPAD3 = 0x63,
            ///<summary>
            ///Numeric keypad 4 key
            ///</summary>
            NUMPAD4 = 0x64,
            ///<summary>
            ///Numeric keypad 5 key
            ///</summary>
            NUMPAD5 = 0x65,
            ///<summary>
            ///Numeric keypad 6 key
            ///</summary>
            NUMPAD6 = 0x66,
            ///<summary>
            ///Numeric keypad 7 key
            ///</summary>
            NUMPAD7 = 0x67,
            ///<summary>
            ///Numeric keypad 8 key
            ///</summary>
            NUMPAD8 = 0x68,
            ///<summary>
            ///Numeric keypad 9 key
            ///</summary>
            NUMPAD9 = 0x69,
            ///<summary>
            ///Multiply key
            ///</summary>
            MULTIPLY = 0x6A,
            ///<summary>
            ///Add key
            ///</summary>
            ADD = 0x6B,
            ///<summary>
            ///Separator key
            ///</summary>
            SEPARATOR = 0x6C,
            ///<summary>
            ///Subtract key
            ///</summary>
            SUBTRACT = 0x6D,
            ///<summary>
            ///Decimal key
            ///</summary>
            DECIMAL = 0x6E,
            ///<summary>
            ///Divide key
            ///</summary>
            DIVIDE = 0x6F,
            ///<summary>
            ///F1 key
            ///</summary>
            F1 = 0x70,
            ///<summary>
            ///F2 key
            ///</summary>
            F2 = 0x71,
            ///<summary>
            ///F3 key
            ///</summary>
            F3 = 0x72,
            ///<summary>
            ///F4 key
            ///</summary>
            F4 = 0x73,
            ///<summary>
            ///F5 key
            ///</summary>
            F5 = 0x74,
            ///<summary>
            ///F6 key
            ///</summary>
            F6 = 0x75,
            ///<summary>
            ///F7 key
            ///</summary>
            F7 = 0x76,
            ///<summary>
            ///F8 key
            ///</summary>
            F8 = 0x77,
            ///<summary>
            ///F9 key
            ///</summary>
            F9 = 0x78,
            ///<summary>
            ///F10 key
            ///</summary>
            F10 = 0x79,
            ///<summary>
            ///F11 key
            ///</summary>
            F11 = 0x7A,
            ///<summary>
            ///F12 key
            ///</summary>
            F12 = 0x7B,
            ///<summary>
            ///F13 key
            ///</summary>
            F13 = 0x7C,
            ///<summary>
            ///F14 key
            ///</summary>
            F14 = 0x7D,
            ///<summary>
            ///F15 key
            ///</summary>
            F15 = 0x7E,
            ///<summary>
            ///F16 key
            ///</summary>
            F16 = 0x7F,
            ///<summary>
            ///F17 key  
            ///</summary>
            F17 = 0x80,
            ///<summary>
            ///F18 key  
            ///</summary>
            F18 = 0x81,
            ///<summary>
            ///F19 key  
            ///</summary>
            F19 = 0x82,
            ///<summary>
            ///F20 key  
            ///</summary>
            F20 = 0x83,
            ///<summary>
            ///F21 key  
            ///</summary>
            F21 = 0x84,
            ///<summary>
            ///F22 key, (PPC only) Key used to lock device.
            ///</summary>
            F22 = 0x85,
            ///<summary>
            ///F23 key  
            ///</summary>
            F23 = 0x86,
            ///<summary>
            ///F24 key  
            ///</summary>
            F24 = 0x87,
            ///<summary>
            ///NUM LOCK key
            ///</summary>
            NUMLOCK = 0x90,
            ///<summary>
            ///SCROLL LOCK key
            ///</summary>
            SCROLL = 0x91,
            ///<summary>
            ///Left SHIFT key
            ///</summary>
            LSHIFT = 0xA0,
            ///<summary>
            ///Right SHIFT key
            ///</summary>
            RSHIFT = 0xA1,
            ///<summary>
            ///Left CONTROL key
            ///</summary>
            LCONTROL = 0xA2,
            ///<summary>
            ///Right CONTROL key
            ///</summary>
            RCONTROL = 0xA3,
            ///<summary>
            ///Left MENU key
            ///</summary>
            LMENU = 0xA4,
            ///<summary>
            ///Right MENU key
            ///</summary>
            RMENU = 0xA5,
            ///<summary>
            ///Windows 2000/XP: Browser Back key
            ///</summary>
            BROWSER_BACK = 0xA6,
            ///<summary>
            ///Windows 2000/XP: Browser Forward key
            ///</summary>
            BROWSER_FORWARD = 0xA7,
            ///<summary>
            ///Windows 2000/XP: Browser Refresh key
            ///</summary>
            BROWSER_REFRESH = 0xA8,
            ///<summary>
            ///Windows 2000/XP: Browser Stop key
            ///</summary>
            BROWSER_STOP = 0xA9,
            ///<summary>
            ///Windows 2000/XP: Browser Search key 
            ///</summary>
            BROWSER_SEARCH = 0xAA,
            ///<summary>
            ///Windows 2000/XP: Browser Favorites key
            ///</summary>
            BROWSER_FAVORITES = 0xAB,
            ///<summary>
            ///Windows 2000/XP: Browser Start and Home key
            ///</summary>
            BROWSER_HOME = 0xAC,
            ///<summary>
            ///Windows 2000/XP: Volume Mute key
            ///</summary>
            VOLUME_MUTE = 0xAD,
            ///<summary>
            ///Windows 2000/XP: Volume Down key
            ///</summary>
            VOLUME_DOWN = 0xAE,
            ///<summary>
            ///Windows 2000/XP: Volume Up key
            ///</summary>
            VOLUME_UP = 0xAF,
            ///<summary>
            ///Windows 2000/XP: Next Track key
            ///</summary>
            MEDIA_NEXT_TRACK = 0xB0,
            ///<summary>
            ///Windows 2000/XP: Previous Track key
            ///</summary>
            MEDIA_PREV_TRACK = 0xB1,
            ///<summary>
            ///Windows 2000/XP: Stop Media key
            ///</summary>
            MEDIA_STOP = 0xB2,
            ///<summary>
            ///Windows 2000/XP: Play/Pause Media key
            ///</summary>
            MEDIA_PLAY_PAUSE = 0xB3,
            ///<summary>
            ///Windows 2000/XP: Start Mail key
            ///</summary>
            LAUNCH_MAIL = 0xB4,
            ///<summary>
            ///Windows 2000/XP: Select Media key
            ///</summary>
            LAUNCH_MEDIA_SELECT = 0xB5,
            ///<summary>
            ///Windows 2000/XP: Start Application 1 key
            ///</summary>
            LAUNCH_APP1 = 0xB6,
            ///<summary>
            ///Windows 2000/XP: Start Application 2 key
            ///</summary>
            LAUNCH_APP2 = 0xB7,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_1 = 0xBA,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '+' key
            ///</summary>
            OEM_PLUS = 0xBB,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the ',' key
            ///</summary>
            OEM_COMMA = 0xBC,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '-' key
            ///</summary>
            OEM_MINUS = 0xBD,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '.' key
            ///</summary>
            OEM_PERIOD = 0xBE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_2 = 0xBF,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_3 = 0xC0,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_4 = 0xDB,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_5 = 0xDC,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_6 = 0xDD,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_7 = 0xDE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_8 = 0xDF,
            ///<summary>
            ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
            ///</summary>
            OEM_102 = 0xE2,
            ///<summary>
            ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
            ///</summary>
            PROCESSKEY = 0xE5,
            ///<summary>
            ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            ///</summary>
            PACKET = 0xE7,
            ///<summary>
            ///Attn key
            ///</summary>
            ATTN = 0xF6,
            ///<summary>
            ///CrSel key
            ///</summary>
            CRSEL = 0xF7,
            ///<summary>
            ///ExSel key
            ///</summary>
            EXSEL = 0xF8,
            ///<summary>
            ///Erase EOF key
            ///</summary>
            EREOF = 0xF9,
            ///<summary>
            ///Play key
            ///</summary>
            PLAY = 0xFA,
            ///<summary>
            ///Zoom key
            ///</summary>
            ZOOM = 0xFB,
            ///<summary>
            ///Reserved 
            ///</summary>
            NONAME = 0xFC,
            ///<summary>
            ///PA1 key
            ///</summary>
            PA1 = 0xFD,
            ///<summary>
            ///Clear key
            ///</summary>
            OEM_CLEAR = 0xFE
        }
    }
}
