-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server Version:               10.2.11-MariaDB - mariadb.org binary distribution
-- Server Betriebssystem:        Win64
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Exportiere Datenbank Struktur für tdsv
CREATE DATABASE IF NOT EXISTS `tdsv` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `tdsv`;

-- Exportiere Struktur von Tabelle tdsv.ban
CREATE TABLE IF NOT EXISTS `ban` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) NOT NULL,
  `socialclubname` varchar(100) NOT NULL,
  `address` varchar(50) NOT NULL,
  `type` varchar(10) NOT NULL,
  `startsec` int(11) NOT NULL,
  `startoptic` varchar(100) NOT NULL,
  `endsec` int(11) NOT NULL DEFAULT 0,
  `endoptic` varchar(100) NOT NULL DEFAULT '-',
  `admin` varchar(100) NOT NULL,
  `reason` varchar(200) NOT NULL DEFAULT 'unknown',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle tdsv.log
CREATE TABLE IF NOT EXISTS `log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) NOT NULL DEFAULT 0,
  `name` varchar(50) NOT NULL DEFAULT '-',
  `targetuid` int(11) NOT NULL DEFAULT 0,
  `type` varchar(50) NOT NULL,
  `info` text NOT NULL,
  `lobby` varchar(100) NOT NULL,
  `date` varchar(50) NOT NULL DEFAULT '-',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=250 DEFAULT CHARSET=utf8mb4;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle tdsv.player
CREATE TABLE IF NOT EXISTS `player` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(25) NOT NULL,
  `password` varchar(500) NOT NULL,
  `email` varchar(200) NOT NULL,
  `adminlvl` int(11) NOT NULL DEFAULT 0,
  `donatorlvl` int(11) NOT NULL DEFAULT 0,
  `isVip` tinyint(1) NOT NULL DEFAULT 0,
  `playtime` int(11) NOT NULL DEFAULT 0,
  `money` int(11) NOT NULL DEFAULT 0,
  `registerdate` varchar(50) NOT NULL DEFAULT '-',
  PRIMARY KEY (`uid`),
  UNIQUE KEY `UID` (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle tdsv.playerarenastats
CREATE TABLE IF NOT EXISTS `playerarenastats` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `arenakills` int(11) NOT NULL DEFAULT 0,
  `arenaassists` int(11) NOT NULL DEFAULT 0,
  `arenadeaths` int(11) NOT NULL DEFAULT 0,
  `arenadamage` int(11) NOT NULL DEFAULT 0,
  `arenatotalkills` int(11) NOT NULL DEFAULT 0,
  `arenatotalassists` int(11) NOT NULL DEFAULT 0,
  `arenatotaldeaths` int(11) NOT NULL DEFAULT 0,
  `arenatotaldamage` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid` (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle tdsv.playersetting
CREATE TABLE IF NOT EXISTS `playersetting` (
  `uid` int(11) NOT NULL,
  `hitsound` tinyint(4) NOT NULL DEFAULT 1,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle tdsv.season
CREATE TABLE IF NOT EXISTS `season` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `month` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Daten Export vom Benutzer nicht ausgewählt
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
