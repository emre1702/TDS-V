using RAGE.Elements;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class Settings
    {
        public const int ScreenFadeInTimeAfterSpawn = 2000;
        public const int ScreenFadeOutTimeAfterSpawn = 2000;

        public static ELanguage MyLanguage = ELanguage.English;
        public static bool ShowBloodscreen = true;

        public static void Load()
        {
            Player.LocalPlayer.SetCanAttackFriendly(false, false);
        }
    }
}
