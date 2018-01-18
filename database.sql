-- phpMyAdmin SQL Dump
-- version 4.5.4.1deb2ubuntu2
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Erstellungszeit: 16. Okt 2017 um 22:53
-- Server-Version: 10.0.31-MariaDB-0ubuntu0.16.04.2
-- PHP-Version: 7.0.22-0ubuntu0.16.04.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `TDSV`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ban`
--

CREATE TABLE `ban` (
  `id` int(11) NOT NULL,
  `UID` int(11) NOT NULL,
  `socialclubname` varchar(100) NOT NULL,
  `address` varchar(50) NOT NULL,
  `type` varchar(10) NOT NULL,
  `startsec` int(11) NOT NULL,
  `startoptic` varchar(100) NOT NULL,
  `endsec` int(11) NOT NULL DEFAULT '0',
  `endoptic` varchar(100) NOT NULL DEFAULT '-',
  `admin` varchar(100) NOT NULL,
  `reason` varchar(200) NOT NULL DEFAULT 'unknown'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `log`
--

CREATE TABLE `log` (
  `ID` int(11) NOT NULL,
  `UID` int(11) NOT NULL DEFAULT '0',
  `name` varchar(50) NOT NULL DEFAULT '-',
  `targetUID` int(11) NOT NULL DEFAULT '0',
  `type` varchar(50) NOT NULL,
  `info` text NOT NULL,
  `lobby` varchar(100) NOT NULL,
  `date` varchar(50) NOT NULL DEFAULT '-'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `player`
--

CREATE TABLE `player` (
  `UID` int(11) NOT NULL,
  `name` varchar(25) NOT NULL,
  `password` varchar(500) NOT NULL,
  `email` varchar(200) NOT NULL,
  `adminlvl` int(11) NOT NULL DEFAULT '0',
  `donatorlvl` int(11) NOT NULL DEFAULT '0',
  `isVip` tinyint(1) NOT NULL DEFAULT '0',
  `playtime` int(11) NOT NULL DEFAULT '0',
  `money` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0',
  `assists` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `damage` int(11) NOT NULL DEFAULT '0',
  `registerdate` varchar(50) NOT NULL DEFAULT '-'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `playersetting`
--

CREATE TABLE `playersetting` (
  `UID` int(11) NOT NULL,
  `hitsound` tinyint(4) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `ban`
--
ALTER TABLE `ban`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `log`
--
ALTER TABLE `log`
  ADD PRIMARY KEY (`ID`);

--
-- Indizes für die Tabelle `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`UID`),
  ADD UNIQUE KEY `UID` (`UID`);

--
-- Indizes für die Tabelle `playersetting`
--
ALTER TABLE `playersetting`
  ADD PRIMARY KEY (`UID`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `ban`
--
ALTER TABLE `ban`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `log`
--
ALTER TABLE `log`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `player`
--
ALTER TABLE `player`
  MODIFY `UID` int(11) NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
