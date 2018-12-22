namespace TDS.Instance.Damagesys
{

    using System;
    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS.Instance.Lobby;
    using TDS.Instance.Player;
    using TDS.Manager.Player;

    partial class Damagesys
    {

        private static readonly Dictionary<WeaponHash, int> sDamageDictionary = new Dictionary<WeaponHash, int>
        {
            //[ Handguns ]//
            /*[WeaponHash.Pistol] = 26, //Pistol
			[WeaponHash.CombatPistol] = 27, //CombatPistol
			[WeaponHash.Pistol50] = 51, //Pistol50
			[WeaponHash.SNSPistol] = 28, //SNSPistol
			[WeaponHash.HeavyPistol] = 40, //HeavyPistol
			[WeaponHash.VintagePistol] = 34, //VintagePistol
			[WeaponHash.MarksmanPistol] = 150, //MarksmanPistol
			[WeaponHash.Revolver] = 110, //Revolver
			[WeaponHash.APPistol] = 28, //ApPistol
			[WeaponHash.StunGun] = 0, //Stun Gun	- Geändert
			[WeaponHash.FlareGun] = 0, //Flare Gun	- Geändert

			//[ Machine Guns ]//
			[WeaponHash.MicroSMG] = 21, //MicroSMG
			[WeaponHash.MachinePistol] = 20, //MachinePistol - Geändert
			[WeaponHash.SMG] = 22, //SMG
			[WeaponHash.AssaultSMG] = 23, //AssaultSMG
			[WeaponHash.CombatPDW] = 28, //CombatPWD
			[WeaponHash.MG] = 40, //MG
			[WeaponHash.CombatMG] = 28, //CombatMG
			[WeaponHash.Gusenberg] = 34, //Gusenberg
			[WeaponHash.MiniSMG] = 22, //MiniSMG

			//[ Assault Rifles ]//
			[WeaponHash.AssaultRifle] = 30, //AssaultRifle
			[WeaponHash.CarbineRifle] = 32, //CarbineRifle
			[WeaponHash.AdvancedRifle] = 34, //AdvancedRifle
			[WeaponHash.SpecialCarbine] = 32, //SpecialCarbine
			[WeaponHash.BullpupRifle] = 32, //BullpupRifle
			[WeaponHash.CompactRifle] = 34, //CompactRifle

			//[ Sniper Rifles ]//
			[WeaponHash.SniperRifle] = 101, //SniperRifle
			[WeaponHash.HeavySniper] = 216, //HeavySniper
			[WeaponHash.MarksmanRifle] = 65, //MarksmanRifle

			//[ Shotguns ]//
			[WeaponHash.PumpShotgun] = 2 * 29, //PumpShotgun
			[WeaponHash.SawnOffShotgun] = 8 * 40, //SawnoffShotgun
			[WeaponHash.BullpupShotgun] = 8 * 14, //BullpupShotgun
			[WeaponHash.AssaultShotgun] = 6 * 32, //AssaultShotgun
			[WeaponHash.Musket] = 165, //Musket
			[WeaponHash.HeavyShotgun] = 117, //HeavyShotgun
			[WeaponHash.DoubleBarrelShotgun] = 166, //DoubleBarrelShotgun - Geändert
			[WeaponHash.SweeperShotgun] = 6 * 27, //SweeperShotgun

			//[ Heavy Weapons ]//
			[WeaponHash.GrenadeLauncher] = 0, //GrenadeLauncher - Geändert, da kA
			[WeaponHash.RPG] = 0, //RPG - Geändert, da kA
			[WeaponHash.Minigun] = 30, //Minigun
			[WeaponHash.Firework] = 0, //Firework - Geändert, da kA
			[WeaponHash.Railgun] = 0, //Railgun - Geändert, da kA
			[WeaponHash.HomingLauncher] = 0, //HomingLauncher - Geändert, da kA
			[WeaponHash.GrenadeLauncherSmoke] = 0, //GrenadeLauncherSmoke - Geändert, da kA
			[WeaponHash.CompactGrenadeLauncher] = 0, //CompactLauncher - Geändert, da kA

			//[ Thrown Weapons ]//
			[WeaponHash.Grenade] = 0, //Grenade - Geändert, da kA
			[WeaponHash.StickyBomb] = 0, //StickyBomb - Geändert, da kA
			[WeaponHash.ProximityMine] = 0, //ProximityMine - Geändert, da kA
			[WeaponHash.BZGas] = 0, //BZGas - Geändert, da kA
			[WeaponHash.Molotov] = 0, //Molotov - Geändert, da kA
			[WeaponHash.FireExtinguisher] = 0, //FireExtinguisher - Geändert, da kA
			[WeaponHash.PetrolCan] = 0, //PetrolCan - Geändert, da kA
			[WeaponHash.Flare] = 0, //Flare - Geändert, da kA
			[WeaponHash.Ball] = 0, //Ball - Geändert, da kA
			[WeaponHash.Snowball] = 0, //Snowball - Geändert, da kA
			[WeaponHash.SmokeGrenade] = 0, //SmokeGrenade - Geändert, da kA
			[WeaponHash.PipeBomb] = 0, //Pipebomb - Geändert, da kA

			//[ Gunrunning ]//
			/*[API.Shared.GetHashKey ( "WEAPON_PISTOL_MK2" )] = 26,
			[API.Shared.GetHashKey ( "WEAPON_SMG_MK2" )] = 22,
			[API.Shared.GetHashKey ( "WEAPON_ASSAULTRIFLE_MK2" )] = 30,
			[API.Shared.GetHashKey ( "WEAPON_CARBINERIFLE_MK2" )] = 32,
			[API.Shared.GetHashKey ( "WEAPON_COMBATMG_MK2" )] = 28  */
        };
        private static readonly Dictionary<WeaponHash, float> sHeadMultiplicator = new Dictionary<WeaponHash, float>
        {
            [WeaponHash.SniperRifle] = 5.0f,
            [WeaponHash.HeavySniper] = 5.0f,
            [WeaponHash.MarksmanRifle] = 5.0f
        };
        private readonly Dictionary<WeaponHash, int> customDamageDictionary = new Dictionary<WeaponHash, int>();
        private readonly Dictionary<WeaponHash, float> customHeadMultiplicator = new Dictionary<WeaponHash, float>();

        public Dictionary<Character, Dictionary<Character, int>> AllHitters = new Dictionary<Character, Dictionary<Character, int>>();
        public Dictionary<Character, Character> LastHitterDictionary = new Dictionary<Character, Character>();
        public Dictionary<Character, int> PlayerDamage = new Dictionary<Character, int>();


        private int GetDamage(WeaponHash hash, bool headshot)
        {
            int damage = 0;
            if (customDamageDictionary.ContainsKey(hash))
                damage = customDamageDictionary[hash];
            else if (sDamageDictionary.ContainsKey(hash))
                damage = sDamageDictionary[hash];
            else
                NAPI.Util.ConsoleOutput("No damage-entry for " + hash.ToString());
            if (damage > 0)
                if (headshot)
                    if (customHeadMultiplicator.ContainsKey(hash))
                        damage = (int)Math.Floor(damage * customHeadMultiplicator[hash]);
                    else if (sHeadMultiplicator.ContainsKey(hash))
                        damage = (int)Math.Floor(damage * sHeadMultiplicator[hash]);
            return damage;
        }

        private void AddDamageToDicts(Character character, Character hittedcharacter, int damage)
        {
            if (!PlayerDamage.ContainsKey(character))
                PlayerDamage[character] = 0;
            PlayerDamage[character] += damage;

            LastHitterDictionary[hittedcharacter] = character;
            if (!AllHitters.ContainsKey(hittedcharacter))
                AllHitters.TryAdd(hittedcharacter, new Dictionary<Character, int>());
            if (!AllHitters[hittedcharacter].ContainsKey(character))
                AllHitters[hittedcharacter][character] += damage;
            else
                AllHitters[hittedcharacter][character] = damage;
        }

        private void DamagePlayer(Character character, Character hittedcharacter, int damage)
        {
            Client hitted = hittedcharacter.Player;
            hittedcharacter.Damage(ref damage);

            //hitted.triggerEvent ( "onClientPlayerDamage" );

            if (lobby is Arena)
                character.GiveDamage((uint)damage);

            AddDamageToDicts(character, hittedcharacter, damage);
        }

        private void DamagedPlayer(Character character, Character hittedcharacter, WeaponHash weapon, bool headshot)
        {
            Client player = character.Player;
            if (!NAPI.Player.IsPlayerDead(hittedcharacter.Player) && hittedcharacter.Player.Dimension == player.Dimension)
            {
                if (character.Team != hittedcharacter.Team)
                {

                    int damage = GetDamage(weapon, headshot);

                    if (damage > 0)
                    {
                        DamagePlayer(character, hittedcharacter, damage);
                        if (character.Entity.Playersettings.HitsoundOn)
                            NAPI.ClientEvent.TriggerClientEvent(player, "onClientPlayerHitOpponent", damage);
                        if (hittedcharacter.Player.Health == 0)
                        {
                            hittedcharacter.Player.Kill();
                            OnPlayerDeath(hittedcharacter.Player, player, (uint)weapon);
                        }
                    }
                }
            }
        }

        [RemoteEvent("onPlayerHitOtherPlayer")]
        public void OnPlayerHitOtherPlayer(Client player, NetHandle hittedhandle, bool headshot)
        {
            Client hitted = NAPI.Player.GetPlayerFromHandle(hittedhandle);
            if (hitted != null)
            {
                Character character = player.GetChar();
                if (character.CurrentLobby is FightLobby fightlobby)
                {
                    WeaponHash currentweapon = player.CurrentWeapon;
                    NAPI.Util.ConsoleOutput(currentweapon.ToString());
                    fightlobby.DmgSys.DamagedPlayer(character, hitted.GetChar(), currentweapon, headshot);
                }
            }
        }
    }
}
