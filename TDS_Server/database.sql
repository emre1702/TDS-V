-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server Version:               5.5.5-10.0.10-MariaDB - mariadb.org binary distribution
-- Server Betriebssystem:        Win64
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Exportiere Datenbank Struktur für tdsnew
CREATE DATABASE IF NOT EXISTS `tdsnew` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `tdsnew`;


-- Exportiere Struktur von Tabelle tdsnew.adminlevelnames
CREATE TABLE IF NOT EXISTS `adminlevelnames` (
  `Level` tinyint(1) unsigned NOT NULL,
  `Language` tinyint(3) unsigned NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`Level`,`Language`),
  KEY `FK_adminlevels_languages` (`Language`),
  CONSTRAINT `adminlevelnames_ibfk_1` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_adminlevelnames_adminlevels` FOREIGN KEY (`Level`) REFERENCES `adminlevels` (`Level`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.adminlevelnames: ~8 rows (ungefähr)
/*!40000 ALTER TABLE `adminlevelnames` DISABLE KEYS */;
INSERT INTO `adminlevelnames` (`Level`, `Language`, `Name`) VALUES
	(0, 7, 'User'),
	(0, 9, 'User'),
	(1, 7, 'Supporter'),
	(1, 9, 'Supporter'),
	(2, 7, 'Administrator'),
	(2, 9, 'Administrator'),
	(3, 7, 'Projektleiter'),
	(3, 9, 'Projectleader');
/*!40000 ALTER TABLE `adminlevelnames` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.adminlevels
CREATE TABLE IF NOT EXISTS `adminlevels` (
  `Level` tinyint(1) unsigned NOT NULL,
  `FontColor` char(2) COLLATE utf8mb4_unicode_ci DEFAULT 'w',
  PRIMARY KEY (`Level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.adminlevels: ~4 rows (ungefähr)
/*!40000 ALTER TABLE `adminlevels` DISABLE KEYS */;
INSERT INTO `adminlevels` (`Level`, `FontColor`) VALUES
	(0, 's'),
	(1, 'g'),
	(2, 'o'),
	(3, 'r');
/*!40000 ALTER TABLE `adminlevels` ENABLE KEYS */;


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
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `StartHealth` tinyint(3) NOT NULL DEFAULT '100',
  `StartArmor` tinyint(3) NOT NULL DEFAULT '100',
  `DefaultSpawnX` float NOT NULL DEFAULT '0',
  `DefaultSpawnY` float NOT NULL DEFAULT '0',
  `DefaultSpawnZ` float NOT NULL DEFAULT '900',
  `AroundSpawnPoint` float NOT NULL DEFAULT '3',
  `DefaultSpawnRotation` float NOT NULL DEFAULT '0',
  `IsTemporary` bit(1) NOT NULL,
  `IsOfficial` bit(1) NOT NULL,
  `CreateTimestamp` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_lobbies_players` (`Owner`),
  CONSTRAINT `FK_lobbies_players` FOREIGN KEY (`Owner`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.lobbies: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `lobbies` DISABLE KEYS */;
/*!40000 ALTER TABLE `lobbies` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.logs_admin
CREATE TABLE IF NOT EXISTS `logs_admin` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint(2) unsigned NOT NULL,
  `Source` int(10) unsigned NOT NULL,
  `Target` int(10) unsigned DEFAULT NULL,
  `Lobby` int(10) DEFAULT NULL,
  `AsDonator` bit(1) NOT NULL,
  `AsVIP` bit(1) NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
  `Lobby` int(10) unsigned NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
  `StackTrace` text COLLATE utf8mb4_unicode_ci,
  `Timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
  `Timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.logs_types: ~14 rows (ungefähr)
/*!40000 ALTER TABLE `logs_types` DISABLE KEYS */;
INSERT INTO `logs_types` (`ID`, `Name`) VALUES
	(1, 'Permaban'),
	(2, 'Permaban_Lobby'),
	(3, 'Timeban'),
	(4, 'Timeban_Lobby'),
	(5, 'Unban'),
	(6, 'Unban_Lobby'),
	(7, 'Kick'),
	(8, 'Kick_Lobby'),
	(9, 'Permamute'),
	(10, 'Timemute'),
	(11, 'Unmute'),
	(12, 'Next'),
	(13, 'Login'),
	(14, 'Register');
/*!40000 ALTER TABLE `logs_types` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.offlinemessages
CREATE TABLE IF NOT EXISTS `offlinemessages` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TargetID` int(10) unsigned NOT NULL,
  `SourceID` int(10) unsigned NOT NULL,
  `Message` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `AlreadyLoadedOnce` bit(1) NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `FK__offlinemessages_target` (`TargetID`),
  KEY `FK__offlinemessages_source` (`SourceID`),
  CONSTRAINT `FK__offlinemessages_source` FOREIGN KEY (`SourceID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK__offlinemessages_target` FOREIGN KEY (`TargetID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.offlinemessages: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `offlinemessages` DISABLE KEYS */;
/*!40000 ALTER TABLE `offlinemessages` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.playerbans
CREATE TABLE IF NOT EXISTS `playerbans` (
  `ID` int(10) unsigned NOT NULL,
  `ForLobby` int(10) unsigned NOT NULL DEFAULT '0',
  `Scname` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Serial` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `IP` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Admin` int(10) unsigned DEFAULT NULL,
  `Reason` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `StartTimestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `EndTimestamp` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`ID`,`ForLobby`),
  KEY `FK_playerbans_players` (`Admin`),
  KEY `FK_playerbans_lobbies` (`ForLobby`),
  CONSTRAINT `FK_playerbans_lobbies` FOREIGN KEY (`ForLobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_playerbans_players` FOREIGN KEY (`Admin`) REFERENCES `players` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `FK__players` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.playerbans: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `playerbans` DISABLE KEYS */;
/*!40000 ALTER TABLE `playerbans` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.playerlobbystats
CREATE TABLE IF NOT EXISTS `playerlobbystats` (
  `ID` int(10) unsigned NOT NULL,
  `Lobby` int(10) unsigned NOT NULL,
  `Kills` int(10) unsigned NOT NULL DEFAULT '0',
  `Deaths` int(10) unsigned NOT NULL DEFAULT '0',
  `Assists` int(10) unsigned NOT NULL DEFAULT '0',
  `Damage` int(10) unsigned NOT NULL DEFAULT '0',
  `TotalKills` int(10) unsigned NOT NULL DEFAULT '0',
  `TotalDeaths` int(10) unsigned NOT NULL DEFAULT '0',
  `TotalAssists` int(10) unsigned NOT NULL DEFAULT '0',
  `TotalDamage` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`,`Lobby`),
  KEY `FK_playerlobbystats_lobbies` (`Lobby`),
  CONSTRAINT `FK_playerlobbystats_lobbies` FOREIGN KEY (`Lobby`) REFERENCES `lobbies` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_playerlobbystats_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.playerlobbystats: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `playerlobbystats` DISABLE KEYS */;
/*!40000 ALTER TABLE `playerlobbystats` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.playermapratings
CREATE TABLE IF NOT EXISTS `playermapratings` (
  `ID` int(10) unsigned NOT NULL,
  `MapName` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Rating` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`MapName`),
  CONSTRAINT `FK_playermapratings_players` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.playermapratings: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `playermapratings` DISABLE KEYS */;
/*!40000 ALTER TABLE `playermapratings` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.players
CREATE TABLE IF NOT EXISTS `players` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `SCName` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Email` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `AdminLvl` tinyint(1) NOT NULL DEFAULT '0',
  `IsVIP` bit(1) NOT NULL,
  `Donation` tinyint(3) NOT NULL DEFAULT '0',
  `LoggedIn` bit(1) NOT NULL,
  `RegisterTimestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.players: ~1 rows (ungefähr)
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` (`ID`, `SCName`, `Name`, `Password`, `Email`, `AdminLvl`, `IsVIP`, `Donation`, `LoggedIn`, `RegisterTimestamp`) VALUES
	(0, 'System', 'System', '-', NULL, 0, b'00000000', 0, b'00000000', '2018-10-20 11:49:50');
/*!40000 ALTER TABLE `players` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.playersettings
CREATE TABLE IF NOT EXISTS `playersettings` (
  `ID` int(10) unsigned NOT NULL,
  `HitsoundOn` bit(1) NOT NULL,
  `Language` tinyint(3) unsigned NOT NULL DEFAULT '9',
  `AllowDataTransfer` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_playersettings_languages` (`Language`),
  CONSTRAINT `FK_playersettings_languages` FOREIGN KEY (`Language`) REFERENCES `languages` (`ID`) ON UPDATE CASCADE,
  CONSTRAINT `FK_playersettings_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.playersettings: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `playersettings` DISABLE KEYS */;
/*!40000 ALTER TABLE `playersettings` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.playerstats
CREATE TABLE IF NOT EXISTS `playerstats` (
  `ID` int(10) unsigned NOT NULL,
  `Money` int(10) unsigned NOT NULL DEFAULT '0',
  `LastLoginTimestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  CONSTRAINT `FK__playerstats_player` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Exportiere Daten aus Tabelle tdsnew.playerstats: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `playerstats` DISABLE KEYS */;
/*!40000 ALTER TABLE `playerstats` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle tdsnew.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `GamemodeName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `MapsPath` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `NewMapsPath` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=COMPACT;

-- Exportiere Daten aus Tabelle tdsnew.settings: ~1 rows (ungefähr)
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` (`ID`, `GamemodeName`, `MapsPath`, `NewMapsPath`) VALUES
	(1, 'tdm', 'bridge/resources/TDS-V/maps/', 'bridge/resources/TDS-V/newmaps/');
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
