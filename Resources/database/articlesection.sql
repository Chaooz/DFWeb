-- Adminer 4.8.1 MySQL 5.5.5-10.9.8-MariaDB-1:10.9.8+maria~ubu2204 dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

SET NAMES utf8mb4;

DROP TABLE IF EXISTS `articlesection`;
CREATE TABLE `articlesection` (
  `id` int(11) NOT NULL,
  `pageid` int(11) NOT NULL,
  `text` text NOT NULL,
  `imageid` int(11) NOT NULL,
  `sort` int(11) NOT NULL,
  `layout` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `articlesection` (`id`, `pageid`, `text`, `imageid`, `sort`, `layout`) VALUES
(1,	134,	'Blabla bla more on section two',	0,	0,	0),
(2,	134,	'',	18,	1,	1),
(3,	134,	'',	27,	2,	1);

-- 2024-05-26 20:52:44
