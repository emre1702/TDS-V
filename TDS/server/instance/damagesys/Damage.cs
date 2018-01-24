namespace TDS.server.instance.damagesys {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using lobby;
	using player;
	using extend;

	partial class Damagesys {

		private static readonly Dictionary<WeaponHash, int> sDamageDictionary = new Dictionary<WeaponHash, int> {
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
			[API.Shared.GetHashKey ( "WEAPON_COMBATMG_MK2" )] = 28  */
		};
		private static readonly Dictionary<WeaponHash, float> sHeadMultiplicator = new Dictionary<WeaponHash, float> {
			[WeaponHash.SniperRifle] = 5.0f,
			[WeaponHash.HeavySniper] = 5.0f,
			[WeaponHash.MarksmanRifle] = 5.0f
		};
		private readonly Dictionary<WeaponHash, int> customDamageDictionary = new Dictionary<WeaponHash, int> ();
		private readonly Dictionary<WeaponHash, float> customHeadMultiplicator = new Dictionary<WeaponHash, float> ();

		public Dictionary<Client, Dictionary<Client, int>> AllHitters = new Dictionary<Client, Dictionary<Client, int>> ();
		public Dictionary<Client, Client> LastHitterDictionary = new Dictionary<Client, Client> ();
		public Dictionary<Client, int> PlayerDamage = new Dictionary<Client, int> ();


		private int GetDamage ( WeaponHash hash, bool headshot ) {
			int damage = 0;
			if ( customDamageDictionary.ContainsKey ( hash ) )
				damage = customDamageDictionary[hash];
			else if ( sDamageDictionary.ContainsKey ( hash ) )
				damage = sDamageDictionary[hash];
			if ( damage > 0 )
				if ( headshot )
					if ( customHeadMultiplicator.ContainsKey ( hash ) )
						damage = (int) Math.Floor ( damage * customHeadMultiplicator[hash] );
					else if ( sHeadMultiplicator.ContainsKey ( hash ) )
						damage = (int) Math.Floor ( damage * sHeadMultiplicator[hash] );
			return damage;
		}

		private void DamagePlayer ( Client player, Client hitted, int damage ) {
			Character character = player.GetChar ();
			if ( hitted.Armor + hitted.Health < damage )
				damage = hitted.Armor + hitted.Health;
			int leftdamage = damage;
			if ( hitted.Armor > 0 )
				if ( hitted.Armor >= leftdamage ) {
					hitted.Armor -= leftdamage;
					leftdamage = 0;
				} else {
					leftdamage -= hitted.Armor;
					hitted.Armor = 0;
				}
			if ( leftdamage > 0 )
				hitted.Health -= leftdamage;

			//hitted.triggerEvent ( "onClientPlayerDamage" );

			if ( lobby.IsOfficial )
				character.Damage += (uint) damage;

			// Reward //
			if ( !PlayerDamage.ContainsKey ( player ) )
				PlayerDamage[player] = 0;
			PlayerDamage[player] += damage;

			// Last-Hitter //
			LastHitterDictionary[hitted] = player;
			if ( !AllHitters.ContainsKey ( hitted ) )
				AllHitters.TryAdd ( hitted, new Dictionary<Client, int> () );
			if ( !AllHitters[hitted].ContainsKey ( player ) )
				AllHitters[hitted][player] += damage;
			else
				AllHitters[hitted][player] = damage;
		}

		private void DamagedPlayer ( Client player, Client hitted, uint weapon, bool headshot ) {
			if ( !NAPI.Player.IsPlayerDead ( hitted ) && hitted.Dimension == player.Dimension ) {
				Character character = player.GetChar ();
				if ( character.Team != hitted.GetChar ().Team ) {
                    WeaponHash hash = (WeaponHash) weapon;

                    int damage = GetDamage ( hash, headshot );

					if ( damage > 0 ) {
						DamagePlayer ( player, hitted, damage );
						if ( character.HitsoundOn )
                            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerHittedOpponent" );
						if ( hitted.Health == 0 ) {
							hitted.Kill ();
							OnPlayerDeath ( hitted, player.Handle, weapon, null );
						}
					}
				}
			}
		}

		private void OnPlayerHitOtherPlayer ( Client player, string name, dynamic[] args ) {
			if ( name == "onPlayerHitOtherPlayer" ) {
				Client hitted = NAPI.Player.GetPlayerFromHandle ( args[0] );
				if ( hitted != null ) {
					Lobby playerlobby = player.GetChar ().Lobby;
					if ( playerlobby is FightLobby fightlobby )
                        fightlobby.DmgSys.DamagedPlayer ( player, hitted, args[1], args[2] );
				}
			}
		}
	}
}
