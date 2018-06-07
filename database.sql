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

-- Exportiere Datenbank Struktur für tds
CREATE DATABASE IF NOT EXISTS `tds` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `tds`;


-- Exportiere Struktur von Tabelle tds.adminlog
CREATE TABLE IF NOT EXISTS `adminlog` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `adminuid` int(10) DEFAULT NULL,
  `targetuid` int(10) DEFAULT NULL,
  `type` tinyint(2) DEFAULT NULL,
  `info` text,
  `date` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='type:\r\npermaban = 0\r\ntimeban = 1\r\nunban = 2\r\npermamute = 3\r\ntimemute = 4\r\nunmute = 5\r\nnext = 6\r\nkick = 7\r\nlobbykick = 8\r\npermabanlobby = 9\r\ntimebanlobby = 10 \r\nunbanlobby = 11';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.ban
CREATE TABLE IF NOT EXISTS `ban` (
  `uid` int(11) NOT NULL,
  `socialclubname` varchar(100) NOT NULL,
  `address` varchar(50) NOT NULL,
  `type` varchar(10) NOT NULL,
  `startsec` int(11) NOT NULL,
  `startoptic` varchar(100) NOT NULL,
  `endsec` int(11) NOT NULL DEFAULT '-1',
  `endoptic` varchar(100) NOT NULL DEFAULT '-',
  `admin` int(11) NOT NULL,
  `reason` varchar(200) NOT NULL DEFAULT 'unknown',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='type:\r\npermaban = 0\r\ntimeban = 1';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.banhistory
CREATE TABLE IF NOT EXISTS `banhistory` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `uid` int(10) NOT NULL DEFAULT '0',
  `shouldtime` int(10) NOT NULL DEFAULT '0',
  `reason` text NOT NULL,
  `date` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.gang
CREATE TABLE IF NOT EXISTS `gang` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL DEFAULT '0',
  `shortname` varchar(10) NOT NULL DEFAULT '0',
  `owneruid` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.gangmember
CREATE TABLE IF NOT EXISTS `gangmember` (
  `memberuid` int(10) NOT NULL,
  `ganguid` int(10) NOT NULL,
  `rank` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`memberuid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.lobbyban
CREATE TABLE IF NOT EXISTS `lobbyban` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `uid` int(10) NOT NULL,
  `lobbyid` int(10) NOT NULL,
  `type` tinyint(1) NOT NULL,
  `startsec` int(10) NOT NULL,
  `startoptic` varchar(50) NOT NULL,
  `endsec` int(10) NOT NULL DEFAULT '-1',
  `endoptic` varchar(50) NOT NULL DEFAULT '-',
  `admin` int(10) NOT NULL,
  `reason` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='type:\r\npermaban = 0\r\ntimeban = 1';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.lobbybanhistory
CREATE TABLE IF NOT EXISTS `lobbybanhistory` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `lobbyid` int(10) NOT NULL DEFAULT '0',
  `shouldtime` int(10) NOT NULL DEFAULT '0',
  `reason` text NOT NULL,
  `date` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.log
CREATE TABLE IF NOT EXISTS `log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) NOT NULL DEFAULT '0',
  `name` varchar(50) NOT NULL DEFAULT '-',
  `targetuid` int(11) NOT NULL DEFAULT '0',
  `type` tinyint(1) NOT NULL,
  `info` text NOT NULL,
  `lobby` varchar(100) NOT NULL,
  `date` varchar(50) NOT NULL DEFAULT '-',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='type:\r\nlogin = 0\r\nregister = 1\r\nchat = 2\r\nerror = 3\r\nlobbyowner = 4\r\nlobbyjoin = 5\r\nvip = 6';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.offlinemsg
CREATE TABLE IF NOT EXISTS `offlinemsg` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `uid` int(10) NOT NULL,
  `message` text NOT NULL,
  `by` varchar(100) NOT NULL,
  `loadedonce` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.player
CREATE TABLE IF NOT EXISTS `player` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(500) NOT NULL,
  `email` varchar(200) NOT NULL,
  `adminlvl` int(11) NOT NULL DEFAULT '0',
  `donatorlvl` int(11) NOT NULL DEFAULT '0',
  `isvip` tinyint(1) NOT NULL DEFAULT '0',
  `playtime` int(11) NOT NULL DEFAULT '0',
  `mutetime` int(11) NOT NULL DEFAULT '0',
  `money` int(11) NOT NULL DEFAULT '0',
  `registerdate` varchar(50) NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.playerarenastats
CREATE TABLE IF NOT EXISTS `playerarenastats` (
  `uid` int(10) NOT NULL,
  `currentkills` int(10) DEFAULT '0',
  `currentassists` int(10) DEFAULT '0',
  `currentdeaths` int(10) DEFAULT '0',
  `currentdamage` int(10) DEFAULT '0',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.playermaprating
CREATE TABLE IF NOT EXISTS `playermaprating` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `uid` int(10) NOT NULL,
  `mapname` varchar(250) NOT NULL,
  `rating` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.playeronline
CREATE TABLE IF NOT EXISTS `playeronline` (
  `uid` int(10) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='List the players online or offline.\r\nUsed for extern usage (control panel).';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.playervehicles
CREATE TABLE IF NOT EXISTS `playervehicles` (
  `uid` int(10) NOT NULL,
  `vehicle` varchar(50) NOT NULL,
  `amount` int(3) NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.reports
CREATE TABLE IF NOT EXISTS `reports` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `authoruid` int(10) NOT NULL,
  `foradminlvl` int(1) NOT NULL,
  `title` varchar(250) NOT NULL,
  `open` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.reportslog
CREATE TABLE IF NOT EXISTS `reportslog` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `reportuid` int(10) DEFAULT NULL,
  `useruid` int(10) DEFAULT NULL,
  `info` text,
  `date` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.reporttexts
CREATE TABLE IF NOT EXISTS `reporttexts` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `reportid` int(10) NOT NULL,
  `authoruid` int(10) NOT NULL,
  `text` text NOT NULL,
  `date` char(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.season
CREATE TABLE IF NOT EXISTS `season` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `month` int(2) NOT NULL DEFAULT '0',
  `topkiller` int(10) NOT NULL DEFAULT '0',
  `topkills` int(10) NOT NULL DEFAULT '0',
  `topassister` int(10) NOT NULL DEFAULT '0',
  `topassists` int(10) NOT NULL DEFAULT '0',
  `topdamager` int(10) NOT NULL DEFAULT '0',
  `topdamage` int(10) NOT NULL DEFAULT '0',
  `topkder` int(10) NOT NULL DEFAULT '0',
  `topkd` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.suggestions
CREATE TABLE IF NOT EXISTS `suggestions` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `authoruid` int(10) NOT NULL,
  `topic` varchar(50) NOT NULL,
  `title` varchar(500) NOT NULL,
  `state` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='state:\r\nopen = 0\r\naccepted = 1\r\ndone = 2\r\nrejected = 3';

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.suggestiontexts
CREATE TABLE IF NOT EXISTS `suggestiontexts` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `suggestionid` int(10) NOT NULL,
  `authoruid` int(10) NOT NULL,
  `text` varchar(500) NOT NULL,
  `date` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt


-- Exportiere Struktur von Tabelle tds.suggestionvotes
CREATE TABLE IF NOT EXISTS `suggestionvotes` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `uid` int(10) NOT NULL DEFAULT '0',
  `suggestionid` int(10) NOT NULL DEFAULT '0',
  `vote` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
