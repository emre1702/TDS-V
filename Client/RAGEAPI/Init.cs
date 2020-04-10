using TDS_Client.Core.Init;

namespace TDS_Client.RAGEAPI
{
    class Init : RAGE.Events.Script
    {
        public Init()
        {
            var modAPI = new BaseAPI();
            new Program(modAPI);
        }


        /*private void TestWeapon()
        {
            RAGE.Ui.Console.Clear();

            var weaponHash = RAGE.Game.Misc.GetHashKey("weapon_carbinerifle");
            RAGE.Ui.Console.Log(RAGE.Ui.ConsoleVerbosity.Info, "Weapon hash: " + weaponHash, false, false);

            RAGE.Game.Weapon.RequestWeaponAsset(weaponHash, 31, 0);

            RAGE.Game.Weapon.RequestWeaponHighDetailModel((int)weaponHash);

            var pos = RAGE.Elements.Player.LocalPlayer.Position;
            var weapon = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 3, pos.Z, true, 1f, 0, 0, 1);

            RAGE.Game.Weapon.SetWeaponObjectTintIndex(weapon, 2);

            var componentHash = RAGE.Game.Misc.GetHashKey("COMPONENT_AT_AR_SUPP");

            var componentModel = RAGE.Game.Weapon.GetWeaponComponentTypeModel(componentHash);
            RAGE.Game.Streaming.RequestModel(componentModel);

            RAGE.Game.Weapon.GiveWeaponComponentToWeaponObject(weapon, componentHash);
            //RAGE.Game.Weapon.RemoveWeaponAsset(weaponHash);
            //RAGE.Game.Streaming.SetModelAsNoLongerNeeded(componentModel);
            RAGE.Ui.Console.Log(RAGE.Ui.ConsoleVerbosity.Info, "Has component: " + RAGE.Game.Weapon.HasWeaponGotWeaponComponent(weapon, componentHash), false, false);

            RAGE.Game.Entity.SetEntityRotation(weapon, 180, 180, 180, 2, true);

            var weapon2 = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 1, pos.Z, true, 1f, 0, 0, 1);
            RAGE.Game.Entity.SetEntityHeading(weapon2, 180);


            RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 5, pos.Z, true, 1f, 0, 0, 180);
            RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 7, pos.Z, true, 1f, 0, 180, 1);
        }*/

        private void TestWeapon2()
        {
            var weaponHash = RAGE.Game.Misc.GetHashKey("weapon_carbinerifle");
            RAGE.Game.Weapon.RequestWeaponAsset(weaponHash, 31, 0);
            RAGE.Game.Weapon.RequestWeaponHighDetailModel((int)weaponHash);
            var pos = RAGE.Elements.Player.LocalPlayer.Position;

            var weapon1 = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 1, pos.Z, true, 1f, 0, 0, 1);
            RAGE.Game.Entity.SetEntityHeading(weapon1, 45);

            var weapon2 = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 1, pos.Z, true, 1f, 0, 0, 1);
            RAGE.Game.Entity.SetEntityHeading(weapon2, 110);

            var weapon3 = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 1, pos.Z, true, 1f, 0, 0, 1);
            RAGE.Game.Invoker.Invoke(RAGE.Game.Natives.SetEntityHeading, weapon3, 190);

            var weapon4 = RAGE.Game.Weapon.CreateWeaponObject(weaponHash, -1, pos.X, pos.Y + 1, pos.Z, true, 1f, 0, 0, 1);
            RAGE.Game.Invoker.Invoke(RAGE.Game.Natives.SetEntityHeading, weapon4, 250);

        }
    }
}
