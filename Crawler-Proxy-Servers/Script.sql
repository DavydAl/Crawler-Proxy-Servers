-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: crawler
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `crawler`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `crawler` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `crawler`;

--
-- Table structure for table `crawler`
--

DROP TABLE IF EXISTS `crawler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `crawler` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dtInicio` datetime DEFAULT NULL,
  `dtTermino` datetime DEFAULT NULL,
  `qtdPaginas` int DEFAULT NULL,
  `qtdLinhas` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `crawler`
--

LOCK TABLES `crawler` WRITE;
/*!40000 ALTER TABLE `crawler` DISABLE KEYS */;
/*!40000 ALTER TABLE `crawler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proxyserver`
--

DROP TABLE IF EXISTS `proxyserver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proxyserver` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ipAdress` varchar(20) DEFAULT NULL,
  `port` varchar(20) DEFAULT NULL,
  `country` varchar(50) DEFAULT NULL,
  `protocol` varchar(20) DEFAULT NULL,
  `idCrawler` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_arquivojson_proxyservers` (`idCrawler`),
  CONSTRAINT `fk_arquivojson_proxyservers` FOREIGN KEY (`idCrawler`) REFERENCES `crawler` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proxyserver`
--

LOCK TABLES `proxyserver` WRITE;
/*!40000 ALTER TABLE `proxyserver` DISABLE KEYS */;
/*!40000 ALTER TABLE `proxyserver` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-27 21:38:09
