using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectLoading
    {
        public static bool LoadObjectHash(uint hash)
        {
            if (!RAGE.Game.Streaming.IsModelInCdimage(hash) || !RAGE.Game.Streaming.IsModelValid(hash))
            {
                ClientUtils.Notify(Settings.Language.OBJECT_MODEL_INVALID);
                return false;
            }
            if (!LoadObjectModel(hash))
            {
                ClientUtils.Notify(Settings.Language.COULD_NOT_LOAD_OBJECT);
                return false;
            }
            return true;
        }

        private static bool LoadObjectModel(uint hash)
        {
            RAGE.Game.Utils.Settimera(0);
            RAGE.Game.Streaming.RequestModel(hash);
            while (!RAGE.Game.Streaming.HasModelLoaded(hash))
            {
                RAGE.Game.Invoker.Wait(0);
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
                    return false;
            }
            return true;
        }
    }
}
