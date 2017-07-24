﻿using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using System;
using GrandTheftMultiplayer.Server.Constant;

namespace Class {
	class Damage : Script {
		//static attributes

		private static Dictionary<int, int> damageDictionary = new Dictionary<int, int> {
			//[ Handguns ]//
			{ 453432689, 26 },		//Pistol
			{ 1593441988, 27 },     //CombatPistol
			{ -1716589765, 51 },    //Pistol50
			{ -1076751822, 28 },    //SNSPistol
			{ -771403250, 40 },     //HeavyPistol
			{ 137902532, 34 },      //VintagePistol
			{ -598887786, 220 },    //MarksmanPistol
			{ -1045183535, 160 },   //Revolver
			{ 584646201, 28 },      //ApBistol
			{ 911657153, 0 },       //Stun Gun	- Geändert
			{ 1198879012, 0 },      //Flare Gun	- Geändert

			//[ Machine Guns ]//
			{ 324215364, 21 },		//MicroSMG
			{ -619010992, 20 },     //MachinePistol - Geändert
			{ 736523883, 22 },      //SMG
			{ -270015777, 23 },     //AssaultSMG
			{ 171789620, 28 },      //CombatPWD
			{ -1660422300, 40 },    //MG
			{ 2144741730, 28 },     //CombatMG
			{ 1627465347, 34 },		//Gusenberg
			{ -1121678507, 22 },    //MiniSMG

			//[ Assault Rifles ]//
			{ -1074790547, 30 },	//AssaultRifle
			{ -2084633992, 32 },    //CarbineRifle
			{ -1357824103, 34 },    //AdvancedRifle
			{ -1063057011, 32 },    //SpecialCarbine
			{ 2132975508, 32 },     //BullpupRifle
			{ 1649403952, 34 },     //CompactRifle

			//[ Sniper Rifles ]//
			{ 100416529, 101 },     //SniperRifle
			{ 205991906, 216 },     //HeavySniper
			{ -952879014, 65 },     //MarksmanRifle

			//[ Shotguns ]//
			{ 487013001, 2*29 },    //PumpShotgun
			{ 2017895192, 8*40 },   //SawnoffShotgun
			{ -1654528753, 8*14 },  //BullpupShotgun
			{ -494615257, 6*32 },   //AssaultShotgun
			{ -1466123874, 165 },   //Musket
			{ 984333226, 117 },     //HeavyShotgun
			{ -275439685, 166 },    //DoubleBarrelShotgun - Geändert
			{ 317205821, 6*27 },    //SweeperShotgun

			//[ Heavy Weapons ]//
			{ -1568386805, 0 },     //GrenadeLauncher - Geändert, da kA
			{ -1312131151, 0 },     //RPG - Geändert, da kA
			{ 1119849093, 30 },		//Minigun
			{ 2138347493, 0 },		//Firework - Geändert, da kA
			{ 1834241177, 0 },		//Railgun - Geändert, da kA
			{ 1672152130, 0 },		//HomingLauncher - Geändert, da kA
			{ 1305664598, 0 },		//GrenadeLauncherSmoke - Geändert, da kA
			{ 125959754, 0 },       //CompactLauncher - Geändert, da kA

			//[ Thrown Weapons ]//
			{ -1813897027, 0 },     //Grenade - Geändert, da kA
			{ 741814745, 0 },		//StickyBomb - Geändert, da kA
			{ -1420407917, 0 },     //ProximityMine - Geändert, da kA
			{ -1600701090, 0 },     //BZGas - Geändert, da kA
			{ 615608432, 0 },		//Molotov - Geändert, da kA
			{ 101631238, 0 },		//FireExtinguisher - Geändert, da kA
			{ 883325847, 0 },		//PetrolCan - Geändert, da kA
			{ 1233104067, 0 },		//Flare - Geändert, da kA
			{ 600439132, 0 },		//Ball - Geändert, da kA
			{ 126349499, 0 },		//Snowball - Geändert, da kA
			{ -37975472, 0 },		//SmokeGrenade - Geändert, da kA
			{ -1169823560, 0 },     //Pipebomb - Geändert, da kA
		};
		private Dictionary<int, int> customDamageDictionary = new Dictionary<int, int> ();

		private static Dictionary<int, double> headMultiplicator = new Dictionary<int, double> {
			{ 100416529, 5.0 },
			{ 205991906, 5.0 },
			{ -952879014, 5.0 }
		};
		private Dictionary<int, double> customHeadMultiplicator = new Dictionary<int, double> ();

		public static Dictionary<Client, Dictionary<Client, int>> allHitters = new Dictionary<Client, Dictionary<Client, int>> ();
		public static Dictionary<Client, Client> lastHitterDictionary = new Dictionary<Client, Client> ();


		//private attributes

		//public attributes

		//constructor
		public Damage ( ) {
			API.onClientEventTrigger += this.OnPlayerHitOtherPlayer;
			API.onPlayerDeath += this.OnPlayerDeath;
		}

		public Damage ( bool notusedvariable, Dictionary<int, int> customdamage = null, Dictionary<int, double> customheadmult = null ) {
			if ( customdamage == null )
				this.customDamageDictionary = new Dictionary<int, int> ();
			else
				this.customDamageDictionary = customdamage;
			if ( customheadmult == null )
				this.customHeadMultiplicator = new Dictionary<int, double> ();
			else
				this.customHeadMultiplicator = customheadmult;
		}

		//destructor

		//private methods
		private int GetDamage ( int hash, bool headshot ) {
			int damage = 0;
			if ( this.customDamageDictionary.ContainsKey ( hash ) )
				damage = this.customDamageDictionary[hash];
			else if ( damageDictionary.ContainsKey ( hash ) )
				damage = damageDictionary[hash];
			if ( damage > 0 ) {

				// Headshot //
				if ( headshot ) {
					if ( this.customHeadMultiplicator.ContainsKey ( hash ) )
						damage = (int) Math.Floor ( (double) damage * this.customHeadMultiplicator[hash] );
					else if ( headMultiplicator.ContainsKey ( hash ) )
						damage = (int) Math.Floor ( (double) damage * headMultiplicator[hash] );
				}
			}
			return damage;
		}

		private void DamagePlayer ( Client player, Client hitted, int damage ) {
			Character character = player.GetChar ();
			if ( hitted.armor + hitted.health < damage )
				damage = hitted.armor + hitted.health;
			int leftdamage = damage;
			if ( hitted.armor > 0 ) {
				if ( hitted.armor >= leftdamage ) {
					hitted.armor -= leftdamage;
					leftdamage = 0;
				} else {
					leftdamage -= hitted.armor;
					hitted.armor = 0;
				}
			}
			if ( leftdamage > 0 ) {
				hitted.health -= leftdamage;
			}

			//hitted.triggerEvent ( "onClientPlayerDamage" );

			// Stats //
			if ( character.lobby == Manager.Arena.lobby ) {
				character.damage += damage;
			}

			// Last-Hitter //
			lastHitterDictionary[hitted] = player;
			if ( !allHitters.ContainsKey ( hitted ) ) {
				allHitters[hitted] = new Dictionary<Client, int> ();
			}
			if ( !allHitters[hitted].ContainsKey ( player ) ) {
				allHitters[hitted][player] = 0;
			}
			allHitters[hitted][player] += damage;
		}

		private void DamagedPlayer ( Client player, Client hitted, int hash, bool headshot ) {
			if ( API.isPlayerDead ( hitted ) == false && hitted.dimension == player.dimension ) {
				if ( player.GetChar ().team != hitted.GetChar ().team ) {
					long tickCount = API.TickCount;
					int damage = this.GetDamage ( hash, headshot );

					if ( damage > 0 ) {
						this.DamagePlayer ( player, hitted, damage );
						if ( hitted.health == 0 ) {
							hitted.kill ();
							OnPlayerDeath ( hitted, player, hash );
						}
					}
				}
			}
		}


		private void OnPlayerHitOtherPlayer ( Client player, string name, dynamic args ) {
			if ( name == "onPlayerHitOtherPlayer" ) {
				Client hitted = API.getPlayerFromHandle ( args[0] );
				if ( hitted != null ) {
					Class.Lobby lobby = player.GetChar ().lobby;
					lobby.damageSys.DamagedPlayer ( player, hitted, args[1], args[2] );
				}
			}
		}

		private void SpawnAfterDeath ( Client player ) {
			if ( player.exists ) {
				API.sendNativeToPlayer ( player, Hash._RESET_LOCALPLAYER_STATE, player );
				API.sendNativeToPlayer ( player, Hash.NETWORK_REQUEST_CONTROL_OF_ENTITY, player );
				API.sendNativeToPlayer ( player, Hash.NETWORK_RESURRECT_LOCAL_PLAYER, 0, 0, 2000, player.rotation.Z, false, false );
				API.sendNativeToPlayer ( player, Hash.RESURRECT_PED, player );
				API.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 2000 );
			}
		}

		public static void CheckLastHitter ( Client player, Character character ) {
			if ( lastHitterDictionary.ContainsKey ( player ) ) {
				Client lasthitter = lastHitterDictionary[player];
				if ( lasthitter.exists ) {
					Character lasthittercharacter = lasthitter.GetChar ();
					if ( character.lobby == lasthittercharacter.lobby ) {
						if ( lasthittercharacter.lifes > 0 ) {
							lasthittercharacter.kills++;
							lasthitter.SendLangNotification ( "got_last_hitted_kill", player.name );
						}
					}
				}
			}
			lastHitterDictionary.Remove ( player );
		}

		private static void CheckForAssist ( Client player, Character character ) {
			if ( allHitters.ContainsKey ( player ) ) {
				int halfarmorhp = ( character.lobby.armor + character.lobby.health ) / 2;
				foreach ( KeyValuePair<Client, int> entry in allHitters[player] ) {
					if ( entry.Value >= halfarmorhp ) {
						Character targetcharacter = entry.Key.GetChar ();
						if ( entry.Key.exists && targetcharacter.lobby == character.lobby ) {
							targetcharacter.assists++;
							entry.Key.SendLangNotification ( "got_assist", player.name );
						}
						return;
					}
				}
			}
		}

		private void OnPlayerDeath ( Client player, NetHandle entityKiller, int weapon ) {
			Character character = player.GetChar ();
			API.triggerClientEventForLobby ( character.lobby, "onClientPlayerDeath", -1, player );

			API.sendNativeToPlayer ( player, Hash._DISABLE_AUTOMATIC_RESPAWN, true );
			API.sendNativeToPlayer ( player, Hash.IGNORE_NEXT_RESTART, true );
			API.sendNativeToPlayer ( player, Hash.SET_FADE_OUT_AFTER_DEATH, false );
			API.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_OUT, 2000 );

			player.freeze ( true );
			Timer.SetTimer ( () => SpawnAfterDeath ( player ), 2000, 1 );
			Client killer = API.getPlayerFromHandle ( entityKiller );

			if ( character.lifes > 0 ) {
				character.lobby.OnPlayerDeath ( player, entityKiller, weapon, character );

				// Kill //
				if ( killer != null ) {
					Console.WriteLine ( player.name + " got killed by " + killer.name );
					if ( character.lobby == Manager.Arena.lobby )
						killer.GetChar ().kills++;
				} else {
					CheckLastHitter ( player, character );
					Console.WriteLine ( player.name + " died" );
				}

				// Death //
				if ( character.lobby == Manager.Arena.lobby )
					character.deaths++;

				// Assist //
				if ( character.lobby == Manager.Arena.lobby )
					CheckForAssist ( player, character );
			}
		}
	}
}