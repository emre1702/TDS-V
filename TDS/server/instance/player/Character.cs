namespace TDS.server.instance.player {

    using GTANetworkAPI;
    using lobby;
	using System;
	using TDS.server.enums;
    using TDS.server.instance.lobby.ganglobby;
    using TDS.server.manager.database;
    using TDS.server.manager.logs;

    public class LobbyDeathmatchStats {
        public uint Kills = 0;
        public uint Assists = 0;
        public uint Deaths = 0;
        public uint Damage = 0;
        public uint TotalKills = 0;
        public uint TotalAssists = 0;
        public uint TotalDeaths = 0;
        public uint TotalDamage = 0;
    }

    public class Character {
        public Client Player;
        public uint UID;
        public uint AdminLvl = 0;
        public uint DonatorLvl = 0;
        public uint Playtime = 0;
        public uint Money = 0;
        public LobbyDeathmatchStats ArenaStats;
        public LobbyDeathmatchStats TempStats;
        public LobbyDeathmatchStats CurrentStats;
        public int Team = 0;
        public Gang Gang;
        public uint GangRank = 0;
        public uint Lifes = 0;
        public Language Language = Language.ENGLISH;
        public Lobby Lobby = manager.lobby.MainMenu.TheLobby;
        public Character Spectating;
        public bool LoggedIn {
			get;
			private set;
		}
        public bool IsLobbyOwner = false;
        public bool IsVIP = false;

        public bool HitsoundOn = true;

		public int StartTick;
		public int LastSave = 0;

        public Character ( Client player ) {
            Player = player;
        }

		public void Login ( ) {
			StartTick = Environment.TickCount;
			LoggedIn = true;
		}

        public void GiveMoney ( short money ) {
            if ( money > 0 || Money > money * -1 ) {
                Money = (uint) checked(Money + money);
                NAPI.ClientEvent.TriggerClientEvent ( Player, "onClientMoneyChange", Money );
            } else
                Log.Error ( $"Player {Player.Name} should have went to minus money! Current: {Money} | Substracted money: {money}" );

        }

        public void GiveKill ( ) {
            ++CurrentStats.Kills;
            ++CurrentStats.TotalKills;
        }

        public void GiveAssist ( ) {
            ++CurrentStats.Assists;
            ++CurrentStats.TotalAssists;
        }

        public void GiveDeath ( ) {
            ++CurrentStats.Deaths;
            ++CurrentStats.TotalDeaths;
        }

        public void GiveDamage ( uint thedmg ) {
            CurrentStats.Damage += thedmg;
            CurrentStats.TotalDeaths += thedmg;
        }

        public void Damage ( ref int damage ) {
            if ( Player.Armor + Player.Health < damage )
				damage = Player.Armor + Player.Health;
            int leftdamage = damage;
            if ( Player.Armor > 0 ) {
                if ( Player.Armor >= leftdamage ) {
                    Player.Armor -= leftdamage;
                    leftdamage = 0;
                } else {
                    leftdamage -= Player.Armor;
                    Player.Armor = 0;
                }
            }
            if ( leftdamage > 0 )
                Player.Health -= leftdamage;
        }

        public void SaveData ( ) {
			++LastSave;
            if ( LoggedIn ) {
                Database.Exec ( $"UPDATE player SET playtime = {Playtime}, money = {Money} WHERE uid = {UID}" );
                Database.Exec ( $"UPDATE playerarenastats SET arenakills = {ArenaStats.Kills}, arenaassists = {ArenaStats.Assists}, arenadeaths = {ArenaStats.Deaths}" +
                    $", arenadamage = {ArenaStats.Damage}, arenatotalkills = {ArenaStats.TotalKills}, arenatotalassists = {ArenaStats.TotalAssists}, arenatotaldeaths = {ArenaStats.TotalDeaths}" +
                    $", arenatotaldamage = {ArenaStats.TotalDamage} WHERE uid = {UID}" );
            }
        }

    }

}
