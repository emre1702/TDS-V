using TDS_Client.Enum;

namespace TDS_Client.Manager.MapCreator
{
    class Main
    {
        public static void ToggleFreecam(EKey _ = EKey.A)
        {
            if (Freecam.IsActive)
            {
                Freecam.Stop();
                Foot.Start();
            }
            else
            {
                Foot.Stop();
                Freecam.Start();
            }
        }
    }
}
