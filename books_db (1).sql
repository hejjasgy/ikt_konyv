-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: db
-- Létrehozás ideje: 2023. Okt 02. 07:30
-- Kiszolgáló verziója: 8.1.0
-- PHP verzió: 8.2.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `books_db`
--
CREATE DATABASE IF NOT EXISTS `books_db` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `books_db`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `books`
--

CREATE TABLE `books` (
  `id` int NOT NULL,
  `booktitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `author` varchar(255) NOT NULL,
  `publisher` varchar(255) NOT NULL,
  `price` int NOT NULL,
  `stock` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- A tábla adatainak kiíratása `books`
--

INSERT INTO `books` (`id`, `booktitle`, `author`, `publisher`, `price`, `stock`) VALUES
(1, 'A kis herceg', 'Antoine de Saint-Exupéry', 'Európa Könyvkiadó', 1999, 14),
(2, 'Harry Potter és a bölcsek köve', 'J. K. Rowling', 'Animus Kiadó', 2999, 5),
(3, 'Az ember tragédiája', 'Madách Imre', 'Magyar Helikon', 1499, 8),
(4, '1984', 'George Orwell', 'Európa Könyvkiadó', 2499, 6),
(5, 'A Gyűrűk Ura', 'J. R. R. Tolkien', 'Alexandra Kiadó', 9999, 3),
(6, 'A három testőr', 'Alexandre Dumas', 'Móra Könyvkiadó', 2999, 7),
(7, 'A katedrális', 'Ken Follett', 'Gabriella Kiadó', 3999, 4),
(8, 'A láthatatlan ember', 'H. G. Wells', 'Gondolat Kiadó', 1999, 6),
(10, 'A nagy Gatsby', 'F. Scott Fitzgerald', 'Európa Könyvkiadó', 3000, 4),
(13, 'A Kandó legendája', '2/14.D', 'Dimény Soma', 0, 1);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `books`
--
ALTER TABLE `books`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
