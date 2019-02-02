-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server Version:               10.3.12-MariaDB - mariadb.org binary distribution
-- Server Betriebssystem:        Win64
-- HeidiSQL Version:             9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Exportiere Datenbank Struktur für tdsnew
CREATE DATABASE IF NOT EXISTS `tdsnew` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `tdsnew`;

-- Exportiere Struktur von Tabelle tdsnew.admin_levels
CREATE TABLE IF NOT EXISTS `admin_levels` (
  `Level` tinyint(1) unsigned NOT NULL,
  `ColorR` tinyint(3) unsigned NOT NULL,
  `ColorG` tinyint(3) unsigned NOT NULL,
  `ColorB` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`Level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.admin_levels: ~4 rows (ungefähr)
/*!40000 ALTER TABLE `admin_levels` DISABLE KEYS */;
INSERT INTO `admin_levels` (`Level`, `ColorR`, `ColorG`, `ColorB`) VALUES
	(0, 220, 220, 220),
	(1, 113, 202, 113),
	(2, 253, 132, 85),
	(3, 222, 50, 50);
/*!40000 ALTER TABLE `admin_levels` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.admin_level_names
CREATE TABLE IF NOT EXISTS `admin_level_names` (
  `Level` tinyint(1) unsigned NOT NULL,
  `Language` tinyint(3) unsigned NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`Level`,`Language`),
  KEY `FK_adminlevels_languages` (`Language`),
  CONSTRAINT `FK_adminlevelnames_adminlevels` FOREIGN KEY (`Level`) REFERENCES `admin_levels` (`Level`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `admin_level_names_ibfk_1` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.admin_level_names: ~8 rows (ungefähr)
/*!40000 ALTER TABLE `admin_level_names` DISABLE KEYS */;
INSERT INTO `admin_level_names` (`Level`, `Language`, `Name`) VALUES
	(0, 7, 'User'),
	(0, 9, 'User'),
	(1, 7, 'Supporter'),
	(1, 9, 'Supporter'),
	(2, 7, 'Administrator'),
	(2, 9, 'Administrator'),
	(3, 7, 'Projektleiter'),
	(3, 9, 'Projectleader');
/*!40000 ALTER TABLE `admin_level_names` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.commands
CREATE TABLE IF NOT EXISTS `commands` (
  `ID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Command` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `NeededAdminLevel` tinyint(1) unsigned DEFAULT NULL,
  `NeededDonation` tinyint(1) unsigned DEFAULT NULL,
  `VipCanUse` bit(1) DEFAULT NULL,
  `LobbyOwnerCanUse` bit(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_commands_adminlevels` (`NeededAdminLevel`),
  CONSTRAINT `FK_commands_adminlevels` FOREIGN KEY (`NeededAdminLevel`) REFERENCES `admin_levels` (`Level`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.commands: ~11 rows (ungefähr)
/*!40000 ALTER TABLE `commands` DISABLE KEYS */;
INSERT INTO `commands` (`ID`, `Command`, `NeededAdminLevel`, `NeededDonation`, `VipCanUse`, `LobbyOwnerCanUse`) VALUES
	(1, 'AdminSay', 1, NULL, b'0', b'0'),
	(2, 'AdminChat', 1, NULL, b'0', b'0'),
	(3, 'Ban', 2, NULL, b'0', b'0'),
	(4, 'Goto', 2, NULL, b'0', b'1'),
	(5, 'Kick', 1, NULL, b'1', b'0'),
	(6, 'LobbyBan', 1, NULL, b'0', b'1'),
	(7, 'LobbyKick', 1, NULL, b'1', b'1'),
	(8, 'Mute', 1, NULL, b'1', b'0'),
	(9, 'NextMap', 1, NULL, b'1', b'1'),
	(10, 'LobbyLeave', NULL, NULL, NULL, NULL),
	(11, 'Suicide', NULL, NULL, NULL, NULL),
	(12, 'GlobalChat', NULL, NULL, NULL, NULL),
	(13, 'TeamChat', NULL, NULL, NULL, NULL),
	(14, 'PrivateChat', NULL, NULL, NULL, NULL),
	(15, 'Position', NULL, NULL, NULL, NULL);
/*!40000 ALTER TABLE `commands` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.commands_alias
CREATE TABLE IF NOT EXISTS `commands_alias` (
  `Alias` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Command` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`Command`,`Alias`),
  CONSTRAINT `FK_commands_alias_commands` FOREIGN KEY (`Command`) REFERENCES `commands` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.commands_alias: ~31 rows (ungefähr)
/*!40000 ALTER TABLE `commands_alias` DISABLE KEYS */;
INSERT INTO `commands_alias` (`Alias`, `Command`) VALUES
	('Announce', 1),
	('Announcement', 1),
	('ASay', 1),
	('OChat', 1),
	('OSay', 1),
	('AChat', 2),
	('ChatAdmin', 2),
	('InternChat', 2),
	('WriteAdmin', 2),
	('PBan', 3),
	('Permaban', 3),
	('RBan', 3),
	('TBan', 3),
	('Timeban', 3),
	('UBan', 3),
	('UnBan', 3),
	('GotoPlayer', 4),
	('GotoXYZ', 4),
	('Warp', 4),
	('WarpTo', 4),
	('WarpToPlayer', 4),
	('XYZ', 4),
	('RKick', 5),
	('BanLobby', 6),
	('KickLobby', 7),
	('PermaMute', 8),
	('PMute', 8),
	('RMute', 8),
	('TimeMute', 8),
	('TMute', 8),
	('EndRound', 9),
	('Back', 10),
	('Leave', 10),
	('LeaveLobby', 10),
	('Mainmenu', 10),
	('Dead', 11),
	('Death', 11),
	('Die', 11),
	('Kill', 11),
	('AllChat', 12),
	('AllSay', 12),
	('G', 12),
	('GChat', 12),
	('PublicChat', 12),
	('PublicSay', 12),
	('TChat', 13),
	('TeamSay', 13),
	('TSay', 13),
	('MSG', 14),
	('PChat', 14),
	('PM', 14),
	('PrivateSay', 14),
	('Coord', 15),
	('Coordinate', 15),
	('Coordinates', 15),
	('CurrentPos', 15),
	('CurrentPosition', 15),
	('GetPos', 15),
	('GetPosition', 15),
	('Pos', 15);
/*!40000 ALTER TABLE `commands_alias` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.commands_info
CREATE TABLE IF NOT EXISTS `commands_info` (
  `ID` tinyint(3) unsigned NOT NULL,
  `Language` tinyint(3) unsigned NOT NULL,
  `Info` varchar(500) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`,`Language`),
  KEY `FK_commands_info_languages` (`Language`),
  CONSTRAINT `FK_commands_info_commands` FOREIGN KEY (`ID`) REFERENCES `commands` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_commands_info_languages` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.commands_info: ~17 rows (ungefähr)
/*!40000 ALTER TABLE `commands_info` DISABLE KEYS */;
INSERT INTO `commands_info` (`ID`, `Language`, `Info`) VALUES
	(1, 7, 'Schreibt öffentlich als ein Admin.'),
	(1, 9, 'Writes public as an admin.'),
	(2, 7, 'Schreibt intern nur den Admins.'),
	(2, 9, 'Writes intern to admins only.'),
	(3, 7, 'Bannt einen Spieler vom gesamten Server.'),
	(3, 9, 'Bans a player out of the server.'),
	(4, 7, 'Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten.'),
	(4, 9, 'Warps the user to another player (maybe in his vehicle) or to the defined coordinates.'),
	(5, 7, 'Kickt einen Spieler vom Server.'),
	(5, 9, 'Kicks a player out of the server.'),
	(6, 7, 'Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde.'),
	(6, 9, 'Bans a player out of the lobby in which the command was used.'),
	(7, 7, 'Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde.'),
	(7, 9, 'Kicks a player out of the lobby in which the command was used.'),
	(8, 7, 'Mutet einen Spieler im normalen Chat.'),
	(8, 9, 'Mutes a player in the normal chat.'),
	(9, 7, 'Beendet die jetzige Runde in der jeweiligen Lobby.'),
	(9, 9, 'Ends the current round in the lobby.'),
	(10, 7, 'Verlässt die jetzige Lobby.'),
	(10, 9, 'Leaves the current lobby.'),
	(11, 7, 'Tötet den Nutzer (Selbstmord).'),
	(11, 9, 'Kills the user (suicide).'),
	(12, 7, 'Globaler Chat, welcher überall gelesen werden kann.'),
	(12, 9, 'Global chat which can be read everywhere.'),
	(13, 7, 'Sendet die Nachricht nur zum eigenen Team.'),
	(13, 9, 'Sends the message to the current team only.'),
	(14, 7, 'Private Nachricht an einen bestimmten Spieler.'),
	(14, 9, 'Private message to a specific player.'),
	(15, 7, 'Gibt die Position des Spielers aus.'),
	(15, 9, 'Outputs the position of the player.');
/*!40000 ALTER TABLE `commands_info` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.killingspree_rewards
CREATE TABLE IF NOT EXISTS `killingspree_rewards` (
  `KillsAmount` int(11) NOT NULL,
  `HealthOrArmor` smallint(3) DEFAULT NULL,
  `OnlyHealth` smallint(3) DEFAULT NULL,
  `OnlyArmor` smallint(3) DEFAULT NULL,
  PRIMARY KEY (`KillsAmount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.killingspree_rewards: ~3 rows (ungefähr)
/*!40000 ALTER TABLE `killingspree_rewards` DISABLE KEYS */;
INSERT INTO `killingspree_rewards` (`KillsAmount`, `HealthOrArmor`, `OnlyHealth`, `OnlyArmor`) VALUES
	(3, 30, NULL, NULL),
	(5, 50, NULL, NULL),
	(10, 100, NULL, NULL),
	(15, 100, NULL, NULL);
/*!40000 ALTER TABLE `killingspree_rewards` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.languages
CREATE TABLE IF NOT EXISTS `languages` (
  `ID` tinyint(3) unsigned NOT NULL,
  `Language` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='https://docs.sdl.com/LiveContent/content/en-US/SDL%20Passolo-v1/GUID-445A86E0-C2E7-41A8-B57B-FCF768E0165D\r\n\r\nAlso add to Manager/Utility/Language.cs';

-- Exportiere Daten aus Tabelle tdsnew.languages: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `languages` DISABLE KEYS */;
INSERT INTO `languages` (`ID`, `Language`) VALUES
	(7, 'German'),
	(9, 'English');
/*!40000 ALTER TABLE `languages` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.lobbies
CREATE TABLE IF NOT EXISTS `lobbies` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Owner` int(10) unsigned DEFAULT NULL,
  `Type` tinyint(3) unsigned NOT NULL,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `StartHealth` tinyint(3) NOT NULL DEFAULT 100,
  `StartArmor` tinyint(3) NOT NULL DEFAULT 100,
  `AmountLifes` tinyint(3) unsigned DEFAULT 0,
  `DefaultSpawnX` float NOT NULL DEFAULT 0,
  `DefaultSpawnY` float NOT NULL DEFAULT 0,
  `DefaultSpawnZ` float NOT NULL DEFAULT 900,
  `AroundSpawnPoint` float NOT NULL DEFAULT 3,
  `DefaultSpawnRotation` float NOT NULL DEFAULT 0,
  `IsTemporary` bit(1) NOT NULL,
  `IsOfficial` bit(1) NOT NULL,
  `SpawnAgainAfterDeathMs` int(10) unsigned DEFAULT 400,
  `MoneyPerKill` float DEFAULT 20,
  `MoneyPerAssist` float DEFAULT 10,
  `MoneyPerDamage` float DEFAULT 0.1,
  `RoundTime` int(10) unsigned DEFAULT 240,
  `CountdownTime` int(10) unsigned DEFAULT 5,
  `BombDetonateTimeMs` int(10) unsigned DEFAULT 45000,
  `BombDefuseTimeMs` int(10) unsigned DEFAULT 8000,
  `BombPlantTimeMs` int(10) unsigned DEFAULT 3000,
  `MixTeamsAfterRound` bit(1) DEFAULT b'0',
  `DieAfterOutsideMapLimitTime` int(10) unsigned DEFAULT 10,
  `CreateTimestamp` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_lobbies_players` (`Owner`),
  KEY `FK_lobbies_lobby_types` (`Type`),
  CONSTRAINT `FK_lobbies_lobby_types` FOREIGN KEY (`Type`) REFERENCES `lobby_types` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_lobbies_players` FOREIGN KEY (`Owner`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.lobbies: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `lobbies` DISABLE KEYS */;
INSERT INTO `lobbies` (`ID`, `Owner`, `Type`, `Name`, `Password`, `StartHealth`, `StartArmor`, `AmountLifes`, `DefaultSpawnX`, `DefaultSpawnY`, `DefaultSpawnZ`, `AroundSpawnPoint`, `DefaultSpawnRotation`, `IsTemporary`, `IsOfficial`, `SpawnAgainAfterDeathMs`, `MoneyPerKill`, `MoneyPerAssist`, `MoneyPerDamage`, `RoundTime`, `CountdownTime`, `BombDetonateTimeMs`, `BombDefuseTimeMs`, `BombPlantTimeMs`, `MixTeamsAfterRound`, `DieAfterOutsideMapLimitTime`, `CreateTimestamp`) VALUES
	(0, 0, 0, 'MainMenu', NULL, 100, 100, NULL, 0, 0, 900, 3, 0, b'0', b'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2019-01-19 19:34:22'),
	(1, 0, 2, 'Arena', NULL, 100, 100, 1, 0, 0, 900, 3, 0, b'0', b'1', 400, 20, 10, 0.1, 240, 5, 45000, 8000, 3000, b'0', 10, '2019-01-19 19:34:29');
/*!40000 ALTER TABLE `lobbies` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.lobby_maps
CREATE TABLE IF NOT EXISTS `lobby_maps` (
  `LobbyID` int(10) unsigned NOT NULL,
  `MapID` int(10) NOT NULL,
  PRIMARY KEY (`LobbyID`,`MapID`),
  KEY `FK_lobby_maps_maps` (`MapID`),
  CONSTRAINT `FK_lobby_maps_lobbies` FOREIGN KEY (`LobbyID`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_lobby_maps_maps` FOREIGN KEY (`MapID`) REFERENCES `maps` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.lobby_maps: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `lobby_maps` DISABLE KEYS */;
/*!40000 ALTER TABLE `lobby_maps` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.lobby_types
CREATE TABLE IF NOT EXISTS `lobby_types` (
  `ID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.lobby_types: ~5 rows (ungefähr)
/*!40000 ALTER TABLE `lobby_types` DISABLE KEYS */;
INSERT INTO `lobby_types` (`ID`, `Name`) VALUES
	(0, 'MainMenu'),
	(1, 'FightLobby'),
	(2, 'Arena'),
	(3, 'GangLobby'),
	(4, 'MapCreateLobby');
/*!40000 ALTER TABLE `lobby_types` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.lobby_weapons
CREATE TABLE IF NOT EXISTS `lobby_weapons` (
  `Hash` int(10) unsigned NOT NULL,
  `Lobby` int(10) unsigned NOT NULL,
  `Ammo` int(10) unsigned NOT NULL,
  `Damage` smallint(3) DEFAULT NULL,
  `HeadMultiplicator` float DEFAULT NULL,
  PRIMARY KEY (`Hash`,`Lobby`),
  KEY `FK_lobby_weapons_lobbies` (`Lobby`),
  CONSTRAINT `FK_lobby_weapons_lobbies` FOREIGN KEY (`Lobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_lobby_weapons_weapons` FOREIGN KEY (`Hash`) REFERENCES `weapons` (`Hash`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.lobby_weapons: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `lobby_weapons` DISABLE KEYS */;
INSERT INTO `lobby_weapons` (`Hash`, `Lobby`, `Ammo`, `Damage`, `HeadMultiplicator`) VALUES
	(3220176749, 1, 9999, NULL, NULL);
/*!40000 ALTER TABLE `lobby_weapons` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_admin
CREATE TABLE IF NOT EXISTS `logs_admin` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint(2) unsigned NOT NULL,
  `Source` int(10) unsigned NOT NULL,
  `Target` int(10) unsigned DEFAULT NULL,
  `Lobby` int(10) unsigned DEFAULT NULL,
  `AsDonator` bit(1) NOT NULL,
  `AsVIP` bit(1) NOT NULL,
  `Reason` varchar(500) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Also used for VIP and Donator';

-- Exportiere Daten aus Tabelle tdsnew.logs_admin: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `logs_admin` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs_admin` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_chat
CREATE TABLE IF NOT EXISTS `logs_chat` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Source` int(10) unsigned NOT NULL,
  `Target` int(10) unsigned DEFAULT NULL,
  `Message` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Lobby` int(10) unsigned DEFAULT NULL,
  `IsAdminChat` bit(1) NOT NULL,
  `IsTeamChat` bit(1) NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_chat: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `logs_chat` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs_chat` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_error
CREATE TABLE IF NOT EXISTS `logs_error` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Source` int(10) unsigned DEFAULT NULL,
  `Info` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `StackTrace` text COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_error: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `logs_error` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs_error` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_rest
CREATE TABLE IF NOT EXISTS `logs_rest` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint(3) unsigned NOT NULL,
  `Source` int(10) unsigned NOT NULL,
  `Serial` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `IP` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Lobby` int(10) unsigned DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_rest: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `logs_rest` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs_rest` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_types
CREATE TABLE IF NOT EXISTS `logs_types` (
  `ID` tinyint(2) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.logs_types: ~11 rows (ungefähr)
/*!40000 ALTER TABLE `logs_types` DISABLE KEYS */;
INSERT INTO `logs_types` (`ID`, `Name`) VALUES
	(1, 'Kick'),
	(2, 'Ban'),
	(3, 'Mute'),
	(4, 'Next'),
	(5, 'Login'),
	(6, 'Register'),
	(7, 'Lobby_Join'),
	(8, 'Lobby_Leave'),
	(9, 'Lobby_Kick'),
	(10, 'Lobby_Ban'),
	(11, 'Goto');
/*!40000 ALTER TABLE `logs_types` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.maps
CREATE TABLE IF NOT EXISTS `maps` (
  `ID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `CreatorID` int(10) unsigned DEFAULT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`),
  KEY `FK_maps_players` (`CreatorID`),
  CONSTRAINT `FK_maps_players` FOREIGN KEY (`CreatorID`) REFERENCES `players` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.maps: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `maps` DISABLE KEYS */;
/*!40000 ALTER TABLE `maps` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.offlinemessages
CREATE TABLE IF NOT EXISTS `offlinemessages` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TargetID` int(10) unsigned NOT NULL,
  `SourceID` int(10) unsigned NOT NULL,
  `Message` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `AlreadyLoadedOnce` bit(1) NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`),
  KEY `FK__offlinemessages_target` (`TargetID`),
  KEY `FK__offlinemessages_source` (`SourceID`),
  CONSTRAINT `FK__offlinemessages_source` FOREIGN KEY (`SourceID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK__offlinemessages_target` FOREIGN KEY (`TargetID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.offlinemessages: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `offlinemessages` DISABLE KEYS */;
/*!40000 ALTER TABLE `offlinemessages` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.players
CREATE TABLE IF NOT EXISTS `players` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `SCName` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Email` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `AdminLvl` tinyint(1) NOT NULL DEFAULT 0,
  `IsVIP` bit(1) NOT NULL,
  `Donation` tinyint(3) NOT NULL DEFAULT 0,
  `RegisterTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.players: ~1 rows (ungefähr)
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` (`ID`, `SCName`, `Name`, `Password`, `Email`, `AdminLvl`, `IsVIP`, `Donation`, `RegisterTimestamp`) VALUES
	(0, 'System', 'System', '-', NULL, 0, b'0', 0, '2018-10-20 11:49:50');
/*!40000 ALTER TABLE `players` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_bans
CREATE TABLE IF NOT EXISTS `player_bans` (
  `ID` int(10) unsigned NOT NULL,
  `ForLobby` int(10) unsigned NOT NULL DEFAULT 0,
  `Admin` int(10) unsigned DEFAULT NULL,
  `Reason` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `StartTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  `EndTimestamp` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`ID`,`ForLobby`),
  KEY `FK_playerbans_players` (`Admin`),
  KEY `FK_playerbans_lobbies` (`ForLobby`),
  CONSTRAINT `FK__players` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON UPDATE CASCADE,
  CONSTRAINT `FK_playerbans_lobbies` FOREIGN KEY (`ForLobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_playerbans_players` FOREIGN KEY (`Admin`) REFERENCES `players` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_bans: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_bans` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_bans` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_lobby_stats
CREATE TABLE IF NOT EXISTS `player_lobby_stats` (
  `ID` int(10) unsigned NOT NULL,
  `Lobby` int(10) unsigned NOT NULL,
  `Kills` int(10) unsigned NOT NULL DEFAULT 0,
  `Deaths` int(10) unsigned NOT NULL DEFAULT 0,
  `Assists` int(10) unsigned NOT NULL DEFAULT 0,
  `Damage` int(10) unsigned NOT NULL DEFAULT 0,
  `TotalKills` int(10) unsigned NOT NULL DEFAULT 0,
  `TotalDeaths` int(10) unsigned NOT NULL DEFAULT 0,
  `TotalAssists` int(10) unsigned NOT NULL DEFAULT 0,
  `TotalDamage` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`,`Lobby`),
  KEY `FK_playerlobbystats_lobbies` (`Lobby`),
  CONSTRAINT `FK_playerlobbystats_lobbies` FOREIGN KEY (`Lobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_playerlobbystats_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.player_lobby_stats: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_lobby_stats` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_lobby_stats` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_map_ratings
CREATE TABLE IF NOT EXISTS `player_map_ratings` (
  `ID` int(10) unsigned NOT NULL,
  `MapName` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Rating` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`MapName`),
  CONSTRAINT `FK_playermapratings_players` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_map_ratings: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_map_ratings` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_map_ratings` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_settings
CREATE TABLE IF NOT EXISTS `player_settings` (
  `ID` int(10) unsigned NOT NULL,
  `HitsoundOn` bit(1) NOT NULL,
  `Language` tinyint(3) unsigned NOT NULL DEFAULT 9,
  `AllowDataTransfer` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_playersettings_languages` (`Language`),
  CONSTRAINT `FK_playersettings_languages` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON UPDATE CASCADE,
  CONSTRAINT `FK_playersettings_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_settings: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_settings` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_stats
CREATE TABLE IF NOT EXISTS `player_stats` (
  `ID` int(10) unsigned NOT NULL,
  `Money` int(10) unsigned NOT NULL DEFAULT 0,
  `PlayTime` int(10) unsigned NOT NULL,
  `MuteTime` int(10) unsigned DEFAULT NULL,
  `LoggedIn` bit(1) NOT NULL,
  `LastLoginTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`),
  CONSTRAINT `FK__playerstats_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_stats: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_stats` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_stats` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `GamemodeName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `MapsPath` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `NewMapsPath` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ErrorToPlayerOnNonExistentCommand` bit(1) NOT NULL,
  `ToChatOnNonExistentCommand` bit(1) NOT NULL,
  `DistanceToSpotToPlant` int(11) NOT NULL,
  `DistanceToSpotToDefuse` int(11) NOT NULL,
  `SavePlayerDataCooldownMinutes` int(11) NOT NULL,
  `SaveLogsCooldownMinutes` int(11) NOT NULL,
  `SaveSeasonsCooldownMinutes` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.settings: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` (`ID`, `GamemodeName`, `MapsPath`, `NewMapsPath`, `ErrorToPlayerOnNonExistentCommand`, `ToChatOnNonExistentCommand`, `DistanceToSpotToPlant`, `DistanceToSpotToDefuse`, `SavePlayerDataCooldownMinutes`, `SaveLogsCooldownMinutes`, `SaveSeasonsCooldownMinutes`) VALUES
	(1, 'tdm', 'bridge/resources/TDS-V/maps/', 'bridge/resources/TDS-V/newmaps/', b'1', b'0', 3, 3, 1, 1, 1);
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.teams
CREATE TABLE IF NOT EXISTS `teams` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Index` tinyint(2) unsigned NOT NULL,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Lobby` int(10) unsigned DEFAULT NULL,
  `ColorR` tinyint(3) unsigned NOT NULL,
  `ColorG` tinyint(3) unsigned NOT NULL,
  `ColorB` tinyint(3) unsigned NOT NULL,
  `BlipColor` tinyint(3) unsigned NOT NULL,
  `SkinHash` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_teams_lobbies` (`Lobby`),
  CONSTRAINT `FK_teams_lobbies` FOREIGN KEY (`Lobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.teams: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `teams` DISABLE KEYS */;
INSERT INTO `teams` (`ID`, `Index`, `Name`, `Lobby`, `ColorR`, `ColorG`, `ColorB`, `BlipColor`, `SkinHash`) VALUES
	(0, 0, 'Spectator', NULL, 255, 255, 255, 4, 1004114196),
	(1, 1, 'SWAT', 1, 0, 150, 0, 52, -1920001264),
	(2, 2, 'Terrorist', 1, 150, 0, 0, 1, 275618457);
/*!40000 ALTER TABLE `teams` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.weapons
CREATE TABLE IF NOT EXISTS `weapons` (
  `Hash` int(10) unsigned NOT NULL,
  `Name` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Type` tinyint(1) NOT NULL,
  `DefaultDamage` smallint(6) NOT NULL,
  `DefaultHeadMultiplicator` float NOT NULL DEFAULT 1,
  PRIMARY KEY (`Hash`),
  KEY `FK_weapons_weapon_types` (`Type`),
  CONSTRAINT `FK_weapons_weapon_types` FOREIGN KEY (`Type`) REFERENCES `weapon_types` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.weapons: ~76 rows (ungefähr)
/*!40000 ALTER TABLE `weapons` DISABLE KEYS */;
INSERT INTO `weapons` (`Hash`, `Name`, `Type`, `DefaultDamage`, `DefaultHeadMultiplicator`) VALUES
	(100416529, 'SniperRifle', 5, 101, 2),
	(101631238, 'FireExtinguisher', 8, 0, 1),
	(125959754, 'CompactGrenadeLauncher', 7, 100, 1),
	(126349499, 'Snowball', 8, 10, 1),
	(137902532, 'VintagePistol', 2, 34, 1),
	(171789620, 'CombatPDW', 3, 28, 1),
	(205991906, 'HeavySniper', 5, 216, 2),
	(317205821, 'SweeperShotgun', 6, 162, 1),
	(324215364, 'MicroSMG', 3, 21, 1),
	(419712736, 'Wrench', 1, 40, 1),
	(453432689, 'Pistol', 2, 26, 1),
	(487013001, 'PumpShotgun', 6, 58, 1),
	(584646201, 'APPistol', 2, 28, 1),
	(600439132, 'Ball', 8, 0, 1),
	(615608432, 'Molotov', 8, 10, 1),
	(736523883, 'SMG', 3, 22, 1),
	(741814745, 'StickyBomb', 8, 100, 1),
	(883325847, 'PetrolCan', 8, 0, 1),
	(911657153, 'StunGun', 2, 0, 1),
	(984333226, 'HeavyShotgun', 6, 117, 1),
	(1119849093, 'Minigun', 7, 30, 1),
	(1141786504, 'GolfClub', 1, 40, 1),
	(1198879012, 'FlareGun', 2, 50, 1),
	(1233104067, 'Flare', 8, 0, 1),
	(1305664598, 'GrenadeLauncherSmoke', 7, 0, 1),
	(1317494643, 'Hammer', 1, 40, 1),
	(1593441988, 'CombatPistol', 2, 27, 1),
	(1627465347, 'Gusenberg', 3, 34, 1),
	(1649403952, 'CompactRifle', 4, 34, 1),
	(1672152130, 'HomingLauncher', 7, 150, 1),
	(1737195953, 'Nightstick', 1, 35, 1),
	(1834241177, 'Railgun', 7, 50, 1),
	(2017895192, 'SawnOffShotgun', 6, 160, 1),
	(2132975508, 'BullpupRifle', 4, 32, 1),
	(2138347493, 'Firework', 7, 100, 1),
	(2144741730, 'CombatMG', 3, 28, 1),
	(2210333304, 'CarbineRifle', 4, 32, 1),
	(2227010557, 'Crowbar', 1, 40, 1),
	(2343591895, 'Flashlight', 1, 30, 1),
	(2460120199, 'Dagger', 1, 45, 1),
	(2481070269, 'Grenade', 8, 100, 1),
	(2484171525, 'PoolCue', 1, 40, 1),
	(2508868239, 'Bat', 1, 40, 1),
	(2578377531, 'Pistol50', 2, 51, 1),
	(2578778090, 'Knife', 1, 45, 1),
	(2634544996, 'MG', 3, 40, 1),
	(2640438543, 'BullpupShotgun', 6, 112, 1),
	(2694266206, 'BZGas', 8, 0, 1),
	(2725352035, 'Unarmed', 1, 15, 1),
	(2726580491, 'GrenadeLauncher', 7, 100, 1),
	(2803906140, 'NightVision', 9, 0, 1),
	(2828843422, 'Musket', 6, 165, 1),
	(2874559379, 'ProximityMine', 8, 100, 1),
	(2937143193, 'AdvancedRifle', 4, 30, 1),
	(2982836145, 'RPG', 7, 100, 1),
	(3125143736, 'PipeBomb', 8, 100, 1),
	(3173288789, 'MiniSMG', 3, 22, 1),
	(3218215474, 'SNSPistol', 2, 28, 1),
	(3220176749, 'AssaultRifle', 4, 30, 1),
	(3231910285, 'SpecialCarbine', 4, 32, 1),
	(3249783761, 'Revolver', 2, 110, 1),
	(3342088282, 'MarksmanRifle', 5, 65, 2),
	(3441901897, 'BattleAxe', 1, 50, 1),
	(3523564046, 'HeavyPistol', 2, 40, 1),
	(3638508604, 'KnuckleDuster', 1, 30, 1),
	(3675956304, 'MachinePistol', 3, 20, 1),
	(3696079510, 'MarksmanPistol', 2, 150, 1),
	(3713923289, 'Machete', 1, 45, 1),
	(3756226112, 'SwitchBlade', 1, 50, 1),
	(3800352039, 'AssaultShotgun', 6, 192, 1),
	(4019527611, 'DoubleBarrelShotgun', 6, 166, 1),
	(4024951519, 'AssaultSMG', 3, 23, 1),
	(4191993645, 'Hatchet', 1, 50, 1),
	(4192643659, 'Bottle', 8, 10, 1),
	(4222310262, 'Parachute', 9, 0, 1),
	(4256991824, 'SmokeGrenade', 8, 0, 1);
/*!40000 ALTER TABLE `weapons` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.weapon_types
CREATE TABLE IF NOT EXISTS `weapon_types` (
  `ID` tinyint(1) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.weapon_types: ~8 rows (ungefähr)
/*!40000 ALTER TABLE `weapon_types` DISABLE KEYS */;
INSERT INTO `weapon_types` (`ID`, `Name`) VALUES
	(1, 'Melee'),
	(2, 'Handgun'),
	(3, 'Machine Gun'),
	(4, 'Assault Rifle'),
	(5, 'Sniper Rifle'),
	(6, 'Shotgun'),
	(7, 'Heavy Weapon'),
	(8, 'Thrown Weapon'),
	(9, 'Rest');
/*!40000 ALTER TABLE `weapon_types` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
