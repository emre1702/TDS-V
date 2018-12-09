namespace TDS.Instance
{

    using System;
    using System.Collections.Generic;
    using GTANetworkAPI;
    using TDS.Default;
    using TDS.Dto;
    using TDS.Instance.Player;

    partial class Damagesys
    {

        /*private static readonly Dictionary<WeaponHash, int> sDamageDictionary = new Dictionary<WeaponHash, int>
        {
            //[ Handguns ]//
            [WeaponHash.Pistol] = 26, //Pistol
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
			[API.Shared.GetHashKey ( "WEAPON_COMBATMG_MK2" )] = 28  
        };*/

        private readonly Dictionary<WeaponHash, DamageDto> damagesDict = new Dictionary<WeaponHash, DamageDto>();
        private readonly Dictionary<Character, Dictionary<Character, int>> lastHittersDict = new Dictionary<Character, Dictionary<Character, int>>();

        public void DamagePlayer(Character target, WeaponHash weapon, bool headshot, Character source = null)
        {
            if (NAPI.Player.IsPlayerDead(target.Player))
                return;
            if (source != null)
            {
                if (target.CurrentLobby != source.CurrentLobby)
                    return;
                if (target.Team.Id == source.Team.Id)
                    return;
            }

            int damage = GetDamage(weapon, headshot);
            target.Damage(ref damage);

            if (source != null)
            {
                target.LastHitter = source;
                UpdateLastHitter(target, source, damage);
                source.CurrentRoundStats.Damage += (uint) damage;

                if (source.Entity.Playersettings.HitsoundOn)
                    NAPI.ClientEvent.TriggerClientEvent(source.Player, DCustomEvent.ClientPlayerHitOpponent);
            }
            
            if (target.Player.Health == 0)
            {
#warning todo Check if we need that
                //target.Player.Kill();
                //OnPlayerDeath(hittedcharacter.Player, player, (uint)weapon);
            }
        }

        private void UpdateLastHitter(Character target, Character source, int damage)
        {
            if (!lastHittersDict.TryGetValue(target, out Dictionary<Character, int> lasthitterdict))
            {
                lasthitterdict = new Dictionary<Character, int>();
                lastHittersDict[target] = lasthitterdict;
            }
            lasthitterdict.TryGetValue(source, out int currentDamage);
            lasthitterdict[source] = currentDamage + damage;
        }

        private int GetDamage(WeaponHash hash, bool headshot)
        {
            if (!damagesDict.ContainsKey(hash))
                return 0;
            int damage = damagesDict[hash].Damage;
            if (headshot)
                damage = (int)(damage * damagesDict[hash].HeadMultiplier);
            return damage;
        }        
    }
}
