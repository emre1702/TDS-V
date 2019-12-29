using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;

namespace TDS_Server.Instance
{
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

        private readonly Dictionary<EWeaponHash, DamageDto> _damagesDict = new Dictionary<EWeaponHash, DamageDto>();
        private readonly Dictionary<TDSPlayer, Dictionary<TDSPlayer, int>> _allHitters = new Dictionary<TDSPlayer, Dictionary<TDSPlayer, int>>();

#pragma warning disable IDE0060 // Remove unused parameter
		public void DamagePlayer(TDSPlayer target, EWeaponHash weapon, int? bone, TDSPlayer? source, int damage)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (target.Client is null)
				return;

			if (NAPI.Player.IsPlayerDead(target.Client))
                return;
            if (source != null)
            {
                if (target.CurrentLobby != source.CurrentLobby)
                    return;
                if (target.Team == source.Team)
                    return;

                UpdateLastHitter(target, source, damage);
                if (source.CurrentRoundStats != null)
                    source.CurrentRoundStats.Damage += damage;

                NAPI.ClientEvent.TriggerClientEvent(source.Client, DToClientEvent.HitOpponent, target.Client.Handle.Value, damage);  
            }
        }

        public void UpdateLastHitter(TDSPlayer target, TDSPlayer? source, int damage)
        {
            if (source is null)
                return;
            if (!_allHitters.TryGetValue(target, out Dictionary<TDSPlayer, int>? lasthitterdict))
            {
                lasthitterdict = new Dictionary<TDSPlayer, int>();
                _allHitters[target] = lasthitterdict;
            }
            lasthitterdict.TryGetValue(source, out int currentDamage);
            lasthitterdict[source] = currentDamage + damage;
            target.LastHitter = source;
        }

        public int GetDamage(EWeaponHash hash, bool headshot = false)
        {
            if (!_damagesDict.ContainsKey(hash))
                return 0;
            int damage = _damagesDict[hash].Damage;
            if (headshot)
                damage = (int)(damage * _damagesDict[hash].HeadMultiplier);
            return damage;
        }
    }
}