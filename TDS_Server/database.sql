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

-- Exportiere Daten aus Tabelle tdsnew.commands: ~15 rows (ungefähr)
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

-- Exportiere Daten aus Tabelle tdsnew.commands_alias: ~55 rows (ungefähr)
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
	('Next', 9),
	('Skip', 9),
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
	('GlobalSay', 12),
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

-- Exportiere Daten aus Tabelle tdsnew.commands_info: ~28 rows (ungefähr)
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
	(1, 0, 2, 'Arena', NULL, 100, 100, 1, 0, 0, 900, 3, 0, b'0', b'1', 400, 20, 10, 0.1, 240, 5, 45000, 8000, 3000, b'1', 10, '2019-01-19 19:34:29');
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

-- Exportiere Daten aus Tabelle tdsnew.lobby_maps: ~1 rows (ungefähr)
/*!40000 ALTER TABLE `lobby_maps` DISABLE KEYS */;
INSERT INTO `lobby_maps` (`LobbyID`, `MapID`) VALUES
	(1, -1);
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
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Also used for VIP and Donator';

-- Exportiere Daten aus Tabelle tdsnew.logs_admin: ~119 rows (ungefähr)
/*!40000 ALTER TABLE `logs_admin` DISABLE KEYS */;
INSERT INTO `logs_admin` (`ID`, `Type`, `Source`, `Target`, `Lobby`, `AsDonator`, `AsVIP`, `Reason`, `Timestamp`) VALUES
	(1, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 19:58:30'),
	(2, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 19:58:45'),
	(3, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:33:34'),
	(4, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:33:49'),
	(5, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:34:05'),
	(6, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:34:19'),
	(7, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:35:16'),
	(8, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:35:27'),
	(9, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:35:40'),
	(10, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:35:52'),
	(11, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:36:05'),
	(12, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:36:17'),
	(13, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:36:28'),
	(14, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:36:42'),
	(15, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:44:33'),
	(16, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:44:44'),
	(17, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:44:54'),
	(18, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:45:08'),
	(19, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:45:21'),
	(20, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:45:35'),
	(21, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:45:47'),
	(22, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:46:46'),
	(23, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:46:58'),
	(24, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:55:27'),
	(25, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 20:55:36'),
	(26, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:01:20'),
	(27, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:01:31'),
	(28, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:01:51'),
	(29, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:02:07'),
	(30, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:02:18'),
	(31, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:02:33'),
	(32, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:02:45'),
	(33, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:02:57'),
	(34, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:03:09'),
	(35, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:03:20'),
	(36, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:03:31'),
	(37, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:07:21'),
	(38, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:07:33'),
	(39, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:07:45'),
	(40, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:07:57'),
	(41, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:08:09'),
	(42, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:08:20'),
	(43, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:08:32'),
	(44, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:11:50'),
	(45, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:12:01'),
	(46, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:18:52'),
	(47, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:19:04'),
	(48, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:19:15'),
	(49, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-02-28 21:19:26'),
	(50, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:40:33'),
	(51, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:40:56'),
	(52, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:43:49'),
	(53, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:44:13'),
	(54, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:51:33'),
	(55, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 11:59:18'),
	(56, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:02:44'),
	(57, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:02:55'),
	(58, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:02:59'),
	(59, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:03:18'),
	(60, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:03:35'),
	(61, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:04:25'),
	(62, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:04:53'),
	(63, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:05:30'),
	(64, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:08:00'),
	(65, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:08:11'),
	(66, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:08:24'),
	(67, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:08:40'),
	(68, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:09:00'),
	(69, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:09:18'),
	(70, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:09:46'),
	(71, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:09:58'),
	(72, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:10:11'),
	(73, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:10:31'),
	(74, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:10:55'),
	(75, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:11:07'),
	(76, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:11:23'),
	(77, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:11:36'),
	(78, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:11:52'),
	(79, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:12:05'),
	(80, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:12:21'),
	(81, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:12:35'),
	(82, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:31:01'),
	(83, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:31:14'),
	(84, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:31:30'),
	(85, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:35:36'),
	(86, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:35:50'),
	(87, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:36:14'),
	(88, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 12:36:32'),
	(89, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:14:44'),
	(90, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:14:58'),
	(91, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:15:12'),
	(92, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:15:26'),
	(93, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:15:38'),
	(94, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:15:56'),
	(95, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:16:12'),
	(96, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:16:31'),
	(97, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:16:50'),
	(98, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:17:09'),
	(99, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:17:21'),
	(100, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:35:09'),
	(101, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 14:35:57'),
	(102, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:07:52'),
	(103, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:08:10'),
	(104, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:08:28'),
	(105, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:08:43'),
	(106, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:23:38'),
	(107, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:23:56'),
	(108, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 15:24:11'),
	(109, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:26:25'),
	(110, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:26:36'),
	(111, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:26:50'),
	(112, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:47:42'),
	(113, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:48:03'),
	(114, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:48:16'),
	(115, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:48:28'),
	(116, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-01 23:49:00'),
	(117, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:12:08'),
	(118, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:12:22'),
	(119, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:12:41'),
	(120, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:12:56'),
	(121, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:13:12'),
	(122, 4, 1, NULL, 1, b'0', b'0', NULL, '2019-03-02 11:21:47');
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
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_chat: ~31 rows (ungefähr)
/*!40000 ALTER TABLE `logs_chat` DISABLE KEYS */;
INSERT INTO `logs_chat` (`ID`, `Source`, `Target`, `Message`, `Lobby`, `IsAdminChat`, `IsTeamChat`, `Timestamp`) VALUES
	(1, 1, NULL, 'asd$normal$', 1, b'0', b'0', '2019-02-03 02:15:52'),
	(2, 1, NULL, 'TEST', NULL, b'0', b'0', '2019-02-03 02:34:36'),
	(3, 1, NULL, 'asdasd$normal$', 1, b'0', b'0', '2019-02-03 17:10:52'),
	(4, 1, NULL, 'z$normal$', 1, b'0', b'0', '2019-02-03 17:10:56'),
	(5, 1, NULL, '123$normal$', 1, b'0', b'0', '2019-02-03 17:11:00'),
	(6, 1, NULL, '123$normal$', 1, b'0', b'0', '2019-02-03 17:11:01'),
	(7, 1, NULL, 'asdasd', 1, b'0', b'1', '2019-02-03 17:11:12'),
	(8, 1, NULL, 'asdasd', 1, b'0', b'1', '2019-02-03 17:11:24'),
	(9, 1, NULL, 'asdasd', 1, b'0', b'1', '2019-02-03 17:11:29'),
	(10, 1, NULL, 'asdasd$normal$', 1, b'0', b'0', '2019-02-03 19:32:24'),
	(11, 1, NULL, 'asdasd', 1, b'0', b'1', '2019-02-24 17:37:00'),
	(12, 1, NULL, 'asdasd', 1, b'0', b'1', '2019-02-24 17:37:01'),
	(13, 1, NULL, 'asdasdyasd', 1, b'0', b'1', '2019-02-24 17:37:02'),
	(14, 1, NULL, 'asdasdyasds', 1, b'0', b'1', '2019-02-24 17:37:07'),
	(15, 1, NULL, 'adasd', 1, b'0', b'1', '2019-02-24 17:37:24'),
	(16, 1, NULL, 'adasdu', 1, b'0', b'1', '2019-02-24 17:37:25'),
	(17, 1, NULL, '', 1, b'0', b'1', '2019-02-24 17:37:29'),
	(18, 1, NULL, '', 1, b'0', b'1', '2019-02-24 17:37:39'),
	(19, 1, NULL, 'asdas', 1, b'0', b'1', '2019-02-24 17:43:34'),
	(20, 1, NULL, 'asdasd', 1, b'0', b'0', '2019-02-24 18:09:23'),
	(21, 1, NULL, 'asd', 1, b'0', b'0', '2019-02-24 20:40:04'),
	(22, 1, NULL, 'asd', 1, b'0', b'0', '2019-02-24 22:18:49'),
	(23, 1, NULL, 'd', 1, b'0', b'0', '2019-02-24 22:18:56'),
	(24, 1, NULL, '', 1, b'0', b'1', '2019-02-28 19:46:05'),
	(25, 1, NULL, 'zasd', 1, b'0', b'1', '2019-02-28 19:57:26'),
	(26, 1, NULL, 'asd', 1, b'0', b'0', '2019-02-28 20:00:06'),
	(27, 1, NULL, 'a', 1, b'0', b'0', '2019-02-28 20:00:10'),
	(28, 1, NULL, 't', 1, b'0', b'0', '2019-02-28 20:55:51'),
	(29, 1, NULL, 'TEASD', NULL, b'1', b'0', '2019-02-28 21:01:46'),
	(30, 1, NULL, 'asdasd', 1, b'1', b'0', '2019-02-28 21:01:49'),
	(31, 1, NULL, 'a', 1, b'0', b'0', '2019-02-28 21:05:03'),
	(32, 1, NULL, '', 1, b'0', b'1', '2019-03-01 11:43:30'),
	(33, 1, NULL, 'aasdasd', 1, b'0', b'0', '2019-03-01 23:47:46');
/*!40000 ALTER TABLE `logs_chat` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_error
CREATE TABLE IF NOT EXISTS `logs_error` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Source` int(10) unsigned DEFAULT NULL,
  `Info` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `StackTrace` text COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_error: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `logs_error` DISABLE KEYS */;
INSERT INTO `logs_error` (`ID`, `Source`, `Info`, `StackTrace`, `Timestamp`) VALUES
	(5, 1, 'The entity type \'HashSet<PlayerLobbyStats>\' was not found. Ensure that the entity type has been added to the model.', '   at System.Environment.get_StackTrace()\r\n   at TDS_Server.Manager.Utility.MinuteTimer.Execute() in B:\\Users\\EmreKara\\Desktop\\Tools\\GitHub\\TDS-V\\TDS_Server\\Manager\\Utility\\MinuteTimer.cs:line 40\r\n   at System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start[TStateMachine](TStateMachine& stateMachine)\r\n   at TDS_Server.Manager.Utility.MinuteTimer.Execute()\r\n   at TDS_Common.Instance.Utility.TDSTimer.ExecuteMe()\r\n   at TDS_Common.Instance.Utility.TDSTimer.OnUpdateFunc()\r\n   at TDS_Server.Manager.Utility.TimerManager.OnUpdateFunc() in B:\\Users\\EmreKara\\Desktop\\Tools\\GitHub\\TDS-V\\TDS_Server\\Manager\\Utility\\TimerManager.cs:line 18\r\n   at System.RuntimeMethodHandle.InvokeMethod(Object target, Object[] arguments, Signature sig, Boolean constructor, Boolean wrapExceptions)\r\n   at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)\r\n   at GTANetworkInternals.EventHandler.Parse(Event _event, Object[] arguments)', '2019-03-05 19:27:34');
/*!40000 ALTER TABLE `logs_error` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.logs_rest
CREATE TABLE IF NOT EXISTS `logs_rest` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint(3) unsigned NOT NULL,
  `Source` int(10) unsigned NOT NULL,
  `Serial` varchar(200) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `IP` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Lobby` int(10) unsigned DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2147484892 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.logs_rest: ~218 rows (ungefähr)
/*!40000 ALTER TABLE `logs_rest` DISABLE KEYS */;
INSERT INTO `logs_rest` (`ID`, `Type`, `Source`, `Serial`, `IP`, `Lobby`, `Timestamp`) VALUES
	(2147484651, 7, 1, NULL, NULL, 0, '2019-02-03 02:31:06'),
	(2147484652, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 02:31:06'),
	(2147484653, 7, 1, NULL, NULL, 1, '2019-02-03 02:32:32'),
	(2147484654, 7, 1, NULL, NULL, 0, '2019-02-03 02:36:51'),
	(2147484655, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 02:36:51'),
	(2147484656, 7, 1, NULL, NULL, 1, '2019-02-03 02:37:01'),
	(2147484657, 7, 1, NULL, NULL, 0, '2019-02-03 13:30:07'),
	(2147484658, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 13:30:07'),
	(2147484659, 7, 1, NULL, NULL, 1, '2019-02-03 13:30:09'),
	(2147484660, 8, 1, NULL, NULL, NULL, '2019-02-03 13:48:49'),
	(2147484661, 7, 1, NULL, NULL, 0, '2019-02-03 13:50:26'),
	(2147484662, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 13:50:26'),
	(2147484663, 7, 1, NULL, NULL, 1, '2019-02-03 13:50:28'),
	(2147484664, 7, 1, NULL, NULL, 0, '2019-02-03 14:02:19'),
	(2147484665, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 14:02:19'),
	(2147484666, 7, 1, NULL, NULL, 1, '2019-02-03 14:02:40'),
	(2147484667, 7, 1, NULL, NULL, 0, '2019-02-03 14:28:16'),
	(2147484668, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 14:28:16'),
	(2147484669, 7, 1, NULL, NULL, 1, '2019-02-03 14:28:18'),
	(2147484670, 7, 1, NULL, NULL, 0, '2019-02-03 14:50:32'),
	(2147484671, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 14:50:32'),
	(2147484672, 7, 1, NULL, NULL, 1, '2019-02-03 14:50:35'),
	(2147484673, 7, 1, NULL, NULL, 0, '2019-02-03 14:53:58'),
	(2147484674, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 14:53:58'),
	(2147484675, 7, 1, NULL, NULL, 1, '2019-02-03 14:54:19'),
	(2147484676, 7, 1, NULL, NULL, 0, '2019-02-03 14:59:14'),
	(2147484677, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 14:59:14'),
	(2147484678, 7, 1, NULL, NULL, 1, '2019-02-03 14:59:17'),
	(2147484679, 7, 1, NULL, NULL, 0, '2019-02-03 16:13:23'),
	(2147484680, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 16:13:23'),
	(2147484681, 7, 1, NULL, NULL, 1, '2019-02-03 16:13:27'),
	(2147484682, 8, 1, NULL, NULL, NULL, '2019-02-03 16:15:25'),
	(2147484683, 7, 1, NULL, NULL, 0, '2019-02-03 16:17:17'),
	(2147484684, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 16:17:17'),
	(2147484685, 7, 1, NULL, NULL, 1, '2019-02-03 16:17:19'),
	(2147484686, 7, 1, NULL, NULL, 0, '2019-02-03 16:23:33'),
	(2147484687, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 16:23:33'),
	(2147484688, 7, 1, NULL, NULL, 1, '2019-02-03 16:23:35'),
	(2147484689, 7, 1, NULL, NULL, 0, '2019-02-03 16:54:28'),
	(2147484690, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 16:54:28'),
	(2147484691, 7, 1, NULL, NULL, 1, '2019-02-03 16:54:36'),
	(2147484692, 7, 1, NULL, NULL, 0, '2019-02-03 16:59:16'),
	(2147484693, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 16:59:16'),
	(2147484694, 7, 1, NULL, NULL, 1, '2019-02-03 16:59:18'),
	(2147484695, 7, 1, NULL, NULL, 0, '2019-02-03 17:10:48'),
	(2147484696, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 17:10:48'),
	(2147484697, 7, 1, NULL, NULL, 1, '2019-02-03 17:10:50'),
	(2147484698, 7, 1, NULL, NULL, 0, '2019-02-03 17:25:18'),
	(2147484699, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 17:25:18'),
	(2147484700, 7, 1, NULL, NULL, 1, '2019-02-03 17:25:23'),
	(2147484701, 7, 1, NULL, NULL, 0, '2019-02-03 18:07:58'),
	(2147484702, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 18:07:58'),
	(2147484703, 7, 1, NULL, NULL, 1, '2019-02-03 18:08:02'),
	(2147484704, 7, 1, NULL, NULL, 0, '2019-02-03 19:00:10'),
	(2147484705, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 19:00:10'),
	(2147484706, 7, 1, NULL, NULL, 1, '2019-02-03 19:00:13'),
	(2147484707, 7, 1, NULL, NULL, 0, '2019-02-03 19:13:42'),
	(2147484708, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 19:13:42'),
	(2147484709, 7, 1, NULL, NULL, 1, '2019-02-03 19:13:45'),
	(2147484710, 7, 1, NULL, NULL, 0, '2019-02-03 19:23:29'),
	(2147484711, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 19:23:29'),
	(2147484712, 7, 1, NULL, NULL, 1, '2019-02-03 19:23:31'),
	(2147484713, 7, 1, NULL, NULL, 0, '2019-02-03 19:32:01'),
	(2147484714, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 19:32:01'),
	(2147484715, 7, 1, NULL, NULL, 1, '2019-02-03 19:32:04'),
	(2147484716, 7, 1, NULL, NULL, 0, '2019-02-03 19:34:29'),
	(2147484717, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-03 19:34:29'),
	(2147484718, 7, 1, NULL, NULL, 1, '2019-02-03 19:34:30'),
	(2147484719, 7, 1, NULL, NULL, 0, '2019-02-24 17:35:51'),
	(2147484720, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 17:35:51'),
	(2147484721, 7, 1, NULL, NULL, 1, '2019-02-24 17:35:54'),
	(2147484722, 7, 1, NULL, NULL, 0, '2019-02-24 17:43:21'),
	(2147484723, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 17:43:21'),
	(2147484724, 7, 1, NULL, NULL, 1, '2019-02-24 17:43:24'),
	(2147484725, 7, 1, NULL, NULL, 0, '2019-02-24 17:45:03'),
	(2147484726, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 17:45:03'),
	(2147484727, 7, 1, NULL, NULL, 1, '2019-02-24 17:45:19'),
	(2147484728, 7, 1, NULL, NULL, 0, '2019-02-24 17:46:41'),
	(2147484729, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 17:46:41'),
	(2147484730, 7, 1, NULL, NULL, 1, '2019-02-24 17:46:43'),
	(2147484731, 7, 1, NULL, NULL, 0, '2019-02-24 17:51:10'),
	(2147484732, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 17:51:10'),
	(2147484733, 7, 1, NULL, NULL, 1, '2019-02-24 17:51:12'),
	(2147484734, 7, 1, NULL, NULL, 0, '2019-02-24 18:08:41'),
	(2147484735, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 18:08:41'),
	(2147484736, 7, 1, NULL, NULL, 1, '2019-02-24 18:08:44'),
	(2147484737, 7, 1, NULL, NULL, 0, '2019-02-24 18:54:17'),
	(2147484738, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-24 18:54:17'),
	(2147484739, 7, 1, NULL, NULL, 1, '2019-02-24 18:54:19'),
	(2147484740, 7, 1, NULL, NULL, 0, '2019-02-24 20:38:51'),
	(2147484741, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.206.16', NULL, '2019-02-24 20:38:51'),
	(2147484742, 7, 1, NULL, NULL, 1, '2019-02-24 20:38:57'),
	(2147484743, 8, 1, NULL, NULL, NULL, '2019-02-24 20:51:59'),
	(2147484744, 7, 1, NULL, NULL, 0, '2019-02-24 20:52:07'),
	(2147484745, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.206.16', NULL, '2019-02-24 20:52:07'),
	(2147484746, 7, 1, NULL, NULL, 1, '2019-02-24 20:52:11'),
	(2147484747, 7, 1, NULL, NULL, 0, '2019-02-24 22:08:25'),
	(2147484748, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.206.16', NULL, '2019-02-24 22:08:25'),
	(2147484749, 7, 1, NULL, NULL, 1, '2019-02-24 22:08:27'),
	(2147484750, 7, 1, NULL, NULL, 0, '2019-02-24 22:12:29'),
	(2147484751, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.206.16', NULL, '2019-02-24 22:12:29'),
	(2147484752, 7, 1, NULL, NULL, 1, '2019-02-24 22:12:33'),
	(2147484753, 8, 1, NULL, NULL, NULL, '2019-02-24 22:22:42'),
	(2147484754, 7, 1, NULL, NULL, 0, '2019-02-24 22:22:42'),
	(2147484755, 7, 1, NULL, NULL, 1, '2019-02-24 22:23:06'),
	(2147484756, 8, 1, NULL, NULL, NULL, '2019-02-24 22:23:35'),
	(2147484757, 7, 1, NULL, NULL, 0, '2019-02-24 22:25:46'),
	(2147484758, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.206.16', NULL, '2019-02-24 22:25:46'),
	(2147484759, 7, 1, NULL, NULL, 1, '2019-02-24 22:25:48'),
	(2147484760, 8, 1, NULL, NULL, NULL, '2019-02-24 22:26:03'),
	(2147484761, 7, 1, NULL, NULL, 0, '2019-02-24 22:26:03'),
	(2147484762, 7, 1, NULL, NULL, 1, '2019-02-24 22:26:05'),
	(2147484763, 8, 1, NULL, NULL, NULL, '2019-02-24 22:27:39'),
	(2147484764, 7, 1, NULL, NULL, 0, '2019-02-24 22:27:39'),
	(2147484765, 7, 1, NULL, NULL, 1, '2019-02-24 22:27:40'),
	(2147484766, 8, 1, NULL, NULL, NULL, '2019-02-24 22:28:11'),
	(2147484767, 7, 1, NULL, NULL, 0, '2019-02-24 22:28:11'),
	(2147484768, 7, 1, NULL, NULL, 1, '2019-02-24 22:28:13'),
	(2147484769, 7, 1, NULL, NULL, 0, '2019-02-28 19:21:00'),
	(2147484770, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 19:21:00'),
	(2147484771, 7, 1, NULL, NULL, 1, '2019-02-28 19:21:11'),
	(2147484772, 7, 1, NULL, NULL, 0, '2019-02-28 19:46:01'),
	(2147484773, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 19:46:01'),
	(2147484774, 7, 1, NULL, NULL, 1, '2019-02-28 19:46:03'),
	(2147484775, 7, 1, NULL, NULL, 0, '2019-02-28 19:57:11'),
	(2147484776, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 19:57:11'),
	(2147484777, 7, 1, NULL, NULL, 1, '2019-02-28 19:57:13'),
	(2147484778, 7, 1, NULL, NULL, 0, '2019-02-28 19:59:48'),
	(2147484779, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 19:59:48'),
	(2147484780, 7, 1, NULL, NULL, 1, '2019-02-28 19:59:49'),
	(2147484781, 7, 1, NULL, NULL, 0, '2019-02-28 20:33:13'),
	(2147484782, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 20:33:13'),
	(2147484783, 7, 1, NULL, NULL, 1, '2019-02-28 20:33:15'),
	(2147484784, 7, 1, NULL, NULL, 0, '2019-02-28 20:44:10'),
	(2147484785, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 20:44:10'),
	(2147484786, 7, 1, NULL, NULL, 1, '2019-02-28 20:44:12'),
	(2147484787, 7, 1, NULL, NULL, 0, '2019-02-28 20:49:24'),
	(2147484788, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 20:49:24'),
	(2147484789, 7, 1, NULL, NULL, 1, '2019-02-28 20:49:26'),
	(2147484790, 7, 1, NULL, NULL, 0, '2019-02-28 20:55:09'),
	(2147484791, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 20:55:09'),
	(2147484792, 7, 1, NULL, NULL, 1, '2019-02-28 20:55:11'),
	(2147484793, 7, 1, NULL, NULL, 0, '2019-02-28 21:01:01'),
	(2147484794, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 21:01:01'),
	(2147484795, 7, 1, NULL, NULL, 1, '2019-02-28 21:01:03'),
	(2147484796, 7, 1, NULL, NULL, 0, '2019-02-28 21:04:50'),
	(2147484797, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 21:04:50'),
	(2147484798, 7, 1, NULL, NULL, 1, '2019-02-28 21:04:52'),
	(2147484799, 7, 1, NULL, NULL, 0, '2019-02-28 21:07:07'),
	(2147484800, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 21:07:07'),
	(2147484801, 7, 1, NULL, NULL, 1, '2019-02-28 21:07:09'),
	(2147484802, 7, 1, NULL, NULL, 0, '2019-02-28 21:11:27'),
	(2147484803, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 21:11:27'),
	(2147484804, 7, 1, NULL, NULL, 1, '2019-02-28 21:11:29'),
	(2147484805, 7, 1, NULL, NULL, 0, '2019-02-28 21:18:39'),
	(2147484806, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-02-28 21:18:39'),
	(2147484807, 7, 1, NULL, NULL, 1, '2019-02-28 21:18:41'),
	(2147484808, 7, 1, NULL, NULL, 0, '2019-03-01 11:36:27'),
	(2147484809, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 11:36:27'),
	(2147484810, 7, 1, NULL, NULL, 1, '2019-03-01 11:36:29'),
	(2147484811, 7, 1, NULL, NULL, 0, '2019-03-01 11:40:13'),
	(2147484812, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 11:40:13'),
	(2147484813, 7, 1, NULL, NULL, 1, '2019-03-01 11:40:14'),
	(2147484814, 7, 1, NULL, NULL, 0, '2019-03-01 11:42:44'),
	(2147484815, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 11:42:44'),
	(2147484816, 7, 1, NULL, NULL, 1, '2019-03-01 11:42:45'),
	(2147484817, 7, 1, NULL, NULL, 0, '2019-03-01 11:47:23'),
	(2147484818, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 11:47:23'),
	(2147484819, 7, 1, NULL, NULL, 1, '2019-03-01 11:47:24'),
	(2147484820, 7, 1, NULL, NULL, 0, '2019-03-01 11:58:47'),
	(2147484821, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 11:58:47'),
	(2147484822, 7, 1, NULL, NULL, 1, '2019-03-01 11:58:51'),
	(2147484823, 7, 1, NULL, NULL, 0, '2019-03-01 12:02:28'),
	(2147484824, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 12:02:28'),
	(2147484825, 7, 1, NULL, NULL, 1, '2019-03-01 12:02:30'),
	(2147484826, 7, 1, NULL, NULL, 0, '2019-03-01 12:07:40'),
	(2147484827, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 12:07:40'),
	(2147484828, 7, 1, NULL, NULL, 1, '2019-03-01 12:07:42'),
	(2147484829, 7, 1, NULL, NULL, 0, '2019-03-01 12:30:43'),
	(2147484830, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 12:30:43'),
	(2147484831, 7, 1, NULL, NULL, 1, '2019-03-01 12:30:47'),
	(2147484832, 7, 1, NULL, NULL, 0, '2019-03-01 12:35:21'),
	(2147484833, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 12:35:21'),
	(2147484834, 7, 1, NULL, NULL, 1, '2019-03-01 12:35:22'),
	(2147484835, 7, 1, NULL, NULL, 0, '2019-03-01 14:14:31'),
	(2147484836, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 14:14:31'),
	(2147484837, 7, 1, NULL, NULL, 1, '2019-03-01 14:14:33'),
	(2147484838, 7, 1, NULL, NULL, 0, '2019-03-01 14:33:57'),
	(2147484839, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 14:33:57'),
	(2147484840, 7, 1, NULL, NULL, 1, '2019-03-01 14:34:13'),
	(2147484841, 7, 1, NULL, NULL, 0, '2019-03-01 14:56:58'),
	(2147484842, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 14:56:58'),
	(2147484843, 7, 1, NULL, NULL, 1, '2019-03-01 14:56:59'),
	(2147484844, 7, 1, NULL, NULL, 0, '2019-03-01 15:05:57'),
	(2147484845, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 15:05:57'),
	(2147484846, 7, 1, NULL, NULL, 1, '2019-03-01 15:05:59'),
	(2147484847, 7, 1, NULL, NULL, 0, '2019-03-01 15:07:27'),
	(2147484848, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 15:07:27'),
	(2147484849, 7, 1, NULL, NULL, 1, '2019-03-01 15:07:29'),
	(2147484850, 7, 1, NULL, NULL, 0, '2019-03-01 15:16:48'),
	(2147484851, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 15:16:48'),
	(2147484852, 7, 1, NULL, NULL, 1, '2019-03-01 15:16:49'),
	(2147484853, 8, 1, NULL, NULL, NULL, '2019-03-01 15:19:15'),
	(2147484854, 7, 1, NULL, NULL, 0, '2019-03-01 15:23:18'),
	(2147484855, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 15:23:18'),
	(2147484856, 7, 1, NULL, NULL, 1, '2019-03-01 15:23:20'),
	(2147484857, 7, 1, NULL, NULL, 0, '2019-03-01 22:21:10'),
	(2147484858, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 22:21:10'),
	(2147484859, 7, 1, NULL, NULL, 1, '2019-03-01 22:21:37'),
	(2147484860, 7, 1, NULL, NULL, 0, '2019-03-01 23:21:52'),
	(2147484861, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 23:21:52'),
	(2147484862, 7, 1, NULL, NULL, 1, '2019-03-01 23:21:54'),
	(2147484863, 7, 1, NULL, NULL, 0, '2019-03-01 23:47:22'),
	(2147484864, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-01 23:47:22'),
	(2147484865, 7, 1, NULL, NULL, 1, '2019-03-01 23:47:25'),
	(2147484866, 7, 1, NULL, NULL, 0, '2019-03-02 11:11:52'),
	(2147484867, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 11:11:52'),
	(2147484868, 7, 1, NULL, NULL, 1, '2019-03-02 11:11:54'),
	(2147484869, 7, 1, NULL, NULL, 0, '2019-03-02 11:16:58'),
	(2147484870, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 11:16:58'),
	(2147484871, 7, 1, NULL, NULL, 1, '2019-03-02 11:16:59'),
	(2147484872, 7, 1, NULL, NULL, 0, '2019-03-02 11:21:18'),
	(2147484873, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 11:21:18'),
	(2147484874, 7, 1, NULL, NULL, 1, '2019-03-02 11:21:20'),
	(2147484875, 7, 1, NULL, NULL, 0, '2019-03-02 11:53:22'),
	(2147484876, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 11:53:22'),
	(2147484877, 7, 1, NULL, NULL, 1, '2019-03-02 11:53:23'),
	(2147484878, 7, 1, NULL, NULL, 0, '2019-03-02 11:57:22'),
	(2147484879, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 11:57:22'),
	(2147484880, 7, 1, NULL, NULL, 1, '2019-03-02 11:57:26'),
	(2147484881, 7, 1, NULL, NULL, 0, '2019-03-02 12:03:05'),
	(2147484882, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '127.0.0.1', NULL, '2019-03-02 12:03:05'),
	(2147484883, 7, 1, NULL, NULL, 1, '2019-03-02 12:03:07'),
	(2147484884, 7, 1, NULL, NULL, 0, '2019-03-05 18:52:22'),
	(2147484885, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.195.68', NULL, '2019-03-05 18:52:22'),
	(2147484886, 7, 1, NULL, NULL, 1, '2019-03-05 18:52:24'),
	(2147484887, 7, 2, NULL, NULL, 0, '2019-03-05 18:52:54'),
	(2147484888, 5, 2, 'D8903A045B82826879608728412649B075F40B0C9C0AF7B0A2F018C8DD2275C0AD02D07466BE3318E3120E0CDBE8A130724408A056B6E968FEF28B4061BC9A00', '217.224.184.196', NULL, '2019-03-05 18:52:54'),
	(2147484889, 7, 2, NULL, NULL, 1, '2019-03-05 18:53:12'),
	(2147484890, 7, 1, NULL, NULL, 0, '2019-03-05 19:26:35'),
	(2147484891, 5, 1, 'D8903A045B766D8055286E98A06C94B075186A34DB20C9C8A2F018C8DD223760F07A8C68EAC0F6E0E31264E83572D0D0A34408A056B6E9F853609FF0621E0400', '91.45.195.68', NULL, '2019-03-05 19:26:36');
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
  KEY `Name` (`Name`),
  CONSTRAINT `FK_maps_players` FOREIGN KEY (`CreatorID`) REFERENCES `players` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.maps: ~6 rows (ungefähr)
/*!40000 ALTER TABLE `maps` DISABLE KEYS */;
INSERT INTO `maps` (`ID`, `Name`, `CreatorID`, `CreateTimestamp`) VALUES
	(-3, 'All Bombs', NULL, '2019-02-03 02:10:06'),
	(-2, 'All Normals', NULL, '2019-02-03 02:09:42'),
	(-1, 'All', NULL, '2019-02-03 02:09:34'),
	(1, 'Pluz. Investment', NULL, '2019-02-03 00:59:19'),
	(2, 'Pluz. Investment Bomb', NULL, '2019-02-03 00:59:19'),
	(3, 'Pluz. Tower', NULL, '2019-02-03 00:59:19'),
	(4, 'Test-Map', NULL, '2019-02-03 00:59:19');
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
  `AdminLvl` tinyint(1) unsigned NOT NULL DEFAULT 0,
  `IsVIP` bit(1) NOT NULL,
  `Donation` tinyint(3) NOT NULL DEFAULT 0,
  `RegisterTimestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`),
  KEY `FK_players_admin_levels` (`AdminLvl`),
  CONSTRAINT `FK_players_admin_levels` FOREIGN KEY (`AdminLvl`) REFERENCES `admin_levels` (`Level`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.players: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` (`ID`, `SCName`, `Name`, `Password`, `Email`, `AdminLvl`, `IsVIP`, `Donation`, `RegisterTimestamp`) VALUES
	(0, 'System', 'System', '-', NULL, 0, b'0', 0, '2018-10-20 11:49:50'),
	(1, 'Bonus1702', 'Bonus1702', '8fa23dab9fc54a8702a7b15fb36974711668e07d2509297e695e18c561e7c2b8cfaab9b1994909c5f08eb83c5da8b34c', 'emre1702@live.de', 3, b'0', 0, '2019-02-03 02:00:26'),
	(2, 'BattlexChamP', 'BattlexChamP', '9943405015bcc41d803f2e6df90fab37ce7d64c3809feb6fcbfbe9981f89e7d3a0c2c399a622a22440eb15a30fdeb93a', 'adrianpavlovic@gmx.net', 0, b'0', 0, '2019-03-05 18:43:34');
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
INSERT INTO `player_lobby_stats` (`ID`, `Lobby`, `Kills`, `Deaths`, `Assists`, `Damage`, `TotalKills`, `TotalDeaths`, `TotalAssists`, `TotalDamage`) VALUES
	(1, 1, 0, 1, 0, 0, 0, 1, 0, 0),
	(2, 1, 0, 0, 0, 0, 0, 0, 0, 0);
/*!40000 ALTER TABLE `player_lobby_stats` ENABLE KEYS */;

-- Exportiere Struktur von Tabelle tdsnew.player_map_favourites
CREATE TABLE IF NOT EXISTS `player_map_favourites` (
  `ID` int(11) unsigned NOT NULL,
  `MapID` int(11) NOT NULL,
  PRIMARY KEY (`ID`,`MapID`),
  KEY `FK_player_map_favourites_maps` (`MapID`),
  CONSTRAINT `FK_player_map_favourites_maps` FOREIGN KEY (`MapID`) REFERENCES `maps` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_player_map_favourites_players` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_map_favourites: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `player_map_favourites` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_map_favourites` ENABLE KEYS */;

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
  `Language` tinyint(3) unsigned NOT NULL DEFAULT 9,
  `Hitsound` bit(1) NOT NULL,
  `Bloodscreen` bit(1) NOT NULL,
  `FloatingDamageInfo` bit(1) NOT NULL,
  `AllowDataTransfer` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_playersettings_languages` (`Language`),
  CONSTRAINT `FK_playersettings_languages` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON UPDATE CASCADE,
  CONSTRAINT `FK_playersettings_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.player_settings: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `player_settings` DISABLE KEYS */;
INSERT INTO `player_settings` (`ID`, `Language`, `Hitsound`, `Bloodscreen`, `FloatingDamageInfo`, `AllowDataTransfer`) VALUES
	(1, 9, b'1', b'0', b'0', b'0'),
	(2, 9, b'1', b'0', b'0', b'0');
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

-- Exportiere Daten aus Tabelle tdsnew.player_stats: ~2 rows (ungefähr)
/*!40000 ALTER TABLE `player_stats` DISABLE KEYS */;
INSERT INTO `player_stats` (`ID`, `Money`, `PlayTime`, `MuteTime`, `LoggedIn`, `LastLoginTimestamp`) VALUES
	(1, 36, 6, NULL, b'1', '2019-02-03 02:00:26'),
	(2, 132, 6, NULL, b'0', '2019-03-05 18:43:34');
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
  `TeamOrderCooldownMs` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.settings: ~1 rows (ungefähr)
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` (`ID`, `GamemodeName`, `MapsPath`, `NewMapsPath`, `ErrorToPlayerOnNonExistentCommand`, `ToChatOnNonExistentCommand`, `DistanceToSpotToPlant`, `DistanceToSpotToDefuse`, `SavePlayerDataCooldownMinutes`, `SaveLogsCooldownMinutes`, `SaveSeasonsCooldownMinutes`, `TeamOrderCooldownMs`) VALUES
	(1, 'tdm', 'bridge/resources/tds/maps/', 'bridge/resources/tds/newmaps/', b'1', b'0', 3, 3, 1, 1, 1, 3000);
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.teams: ~4 rows (ungefähr)
/*!40000 ALTER TABLE `teams` DISABLE KEYS */;
INSERT INTO `teams` (`ID`, `Index`, `Name`, `Lobby`, `ColorR`, `ColorG`, `ColorB`, `BlipColor`, `SkinHash`) VALUES
	(0, 0, 'Spectator', 0, 255, 255, 255, 4, 1004114196),
	(1, 0, 'Spectator', 1, 255, 255, 255, 4, 1004114196),
	(2, 1, 'SWAT', 1, 0, 150, 0, 52, -1920001264),
	(3, 2, 'Terrorist', 1, 150, 0, 0, 1, 275618457);
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
