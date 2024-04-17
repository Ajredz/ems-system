CREATE DATABASE  IF NOT EXISTS `clean_ems_ipm` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_bin */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clean_ems_ipm`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: clean_ems_ipm
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `old_employee_id` varchar(5) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `suffix` varchar(10) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `nickname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_id` int(11) NOT NULL DEFAULT '0',
  `position_id` int(11) NOT NULL DEFAULT '0',
  `gender` varchar(2) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `employment_status` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `date_hired` date DEFAULT NULL,
  `system_user_id` int(11) NOT NULL DEFAULT '0',
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `code_index` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_movement`
--

DROP TABLE IF EXISTS `employee_movement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_movement` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `sync_id` bigint(20) NOT NULL,
  `sync_date` datetime NOT NULL,
  `employee_id` int(11) NOT NULL,
  `employee_field` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `movement_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `reason` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `from` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `from_id` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `to` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `to_id` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `table_source` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `date_effective_from` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `date_effective_to` datetime DEFAULT NULL,
  `hrd_comments` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_latest` bit(1) NOT NULL DEFAULT b'1',
  `is_applied` bit(1) NOT NULL DEFAULT b'1',
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_movement`
--

LOCK TABLES `employee_movement` WRITE;
/*!40000 ALTER TABLE `employee_movement` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_movement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_roving`
--

DROP TABLE IF EXISTS `employee_roving`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_roving` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `org_group_id` int(11) DEFAULT NULL,
  `position_id` int(11) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_roving`
--

LOCK TABLES `employee_roving` WRITE;
/*!40000 ALTER TABLE `employee_roving` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_roving` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_score`
--

DROP TABLE IF EXISTS `employee_score`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_score` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `t_id` int(11) NOT NULL,
  `tdate_from` date DEFAULT NULL,
  `tdate_to` date DEFAULT NULL,
  `employee_id` int(11) NOT NULL,
  `org_group_id` int(11) NOT NULL,
  `position_id` int(11) NOT NULL,
  `kpi_id` int(11) NOT NULL,
  `kpi_weight` decimal(16,5) NOT NULL,
  `is_editable` bit(1) NOT NULL DEFAULT b'0',
  `kpi_score` decimal(16,5) NOT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `level_of_approval` int(11) DEFAULT '0',
  `approver_id` int(11) DEFAULT NULL,
  `approver_position_id` int(11) DEFAULT NULL,
  `approver_org_group_id` int(11) DEFAULT NULL,
  `pdate_from` date DEFAULT NULL,
  `pdate_to` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_kpi_score` (`kpi_score`),
  KEY `idx_kpi_weight` (`kpi_weight`),
  KEY `idx_tdate_from` (`tdate_from`),
  KEY `idx_tdate_to` (`tdate_to`),
  KEY `idx_pdate_from` (`pdate_from`),
  KEY `idx_pdate_to` (`pdate_to`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_score`
--

LOCK TABLES `employee_score` WRITE;
/*!40000 ALTER TABLE `employee_score` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_score` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `error_log`
--

DROP TABLE IF EXISTS `error_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `error_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `layer` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `class` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `method` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `error_message` varchar(5000) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `inner_exception` varchar(5000) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `user_id` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `error_log`
--

LOCK TABLES `error_log` WRITE;
/*!40000 ALTER TABLE `error_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `error_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi`
--

DROP TABLE IF EXISTS `kpi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `old_kpi_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `name` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `description` mediumtext CHARACTER SET latin1 COLLATE latin1_bin,
  `guidelines` mediumtext CHARACTER SET latin1 COLLATE latin1_bin,
  `kra_group` int(11) DEFAULT NULL,
  `kra_sub_group` int(11) DEFAULT NULL,
  `kpi_type` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `source_type` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `is_active` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi`
--

LOCK TABLES `kpi` WRITE;
/*!40000 ALTER TABLE `kpi` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi_position`
--

DROP TABLE IF EXISTS `kpi_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi_position` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tdate` date NOT NULL,
  `position_id` int(11) NOT NULL,
  `kpi_id` int(11) NOT NULL,
  `weight` decimal(8,5) NOT NULL,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `is_latest` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi_position`
--

LOCK TABLES `kpi_position` WRITE;
/*!40000 ALTER TABLE `kpi_position` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi_position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi_score`
--

DROP TABLE IF EXISTS `kpi_score`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi_score` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `org_group_id` int(11) NOT NULL,
  `kpi_id` int(11) NOT NULL,
  `period` date DEFAULT NULL,
  `target` decimal(16,5) NOT NULL,
  `actual` decimal(16,5) NOT NULL,
  `rate` decimal(16,5) NOT NULL,
  `formula` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `modified_by` int(11) NOT NULL,
  `modified_date` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_org_group_id` (`org_group_id`),
  KEY `idx_kpi_id` (`kpi_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi_score`
--

LOCK TABLES `kpi_score` WRITE;
/*!40000 ALTER TABLE `kpi_score` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi_score` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kpi_score_per_employee`
--

DROP TABLE IF EXISTS `kpi_score_per_employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kpi_score_per_employee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_id` int(11) NOT NULL,
  `kpi_id` int(11) NOT NULL,
  `period` date DEFAULT NULL,
  `target` decimal(16,5) NOT NULL,
  `actual` decimal(16,5) NOT NULL,
  `rate` decimal(16,5) NOT NULL,
  `formula` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `modified_by` int(11) NOT NULL,
  `modified_date` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_employee_id` (`employee_id`),
  KEY `idx_kpi_id` (`kpi_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kpi_score_per_employee`
--

LOCK TABLES `kpi_score_per_employee` WRITE;
/*!40000 ALTER TABLE `kpi_score_per_employee` DISABLE KEYS */;
/*!40000 ALTER TABLE `kpi_score_per_employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kra_group`
--

DROP TABLE IF EXISTS `kra_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kra_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kra_group`
--

LOCK TABLES `kra_group` WRITE;
/*!40000 ALTER TABLE `kra_group` DISABLE KEYS */;
INSERT INTO `kra_group` VALUES (1,'ATTITUDE','BRANCH_OPS'),(2,'BRANCH MANAGEMENT','BRANCH_OPS'),(3,'BUDGET MANAGEMENT','BRANCH_OPS'),(4,'CASH MANAGEMENT','BRANCH_OPS'),(5,'CLUSTER / BRANCH MANAGEMENT','BRANCH_OPS'),(6,'CLUSTER MANAGEMENT','BRANCH_OPS'),(7,'COLLECTION MANAGEMENT','BRANCH_OPS'),(8,'COMPLIANCE','BRANCH_OPS'),(9,'CUSTOMER MANAGEMENT AND CUSTOMER SATISFACTION','BRANCH_OPS'),(10,'DEMAND AND CREATION MANAGEMENT','BRANCH_OPS'),(11,'FINANCIAL','BRANCH_OPS'),(12,'INTEGRITY','BRANCH_OPS'),(13,'INVENTORY MANAGEMENT','BRANCH_OPS'),(14,'KNOWLEDGE','BRANCH_OPS'),(15,'KNOWLEDGE AND SKILLS IMPROVEMENT','BRANCH_OPS'),(16,'MALASAKIT','BRANCH_OPS'),(17,'MANPOWER MANAGEMENT','BRANCH_OPS'),(18,'OPERATION EFFICIENCY','BRANCH_OPS'),(19,'OPERATIONAL EFFICEINCY','BRANCH_OPS'),(20,'POSITIVE ATTITUDE','BRANCH_OPS'),(21,'REGISTRATION MANAGEMENT','BRANCH_OPS'),(22,'REPO MANAGEMENT','BRANCH_OPS'),(23,'SALES','BRANCH_OPS'),(24,'SERVICE MANAGEMENT','BRANCH_OPS'),(25,'SKILLS','BRANCH_OPS'),(26,'SPECIAL PROJECTS','BRANCH_OPS'),(27,'SYSTEM MANAGEMENT','BRANCH_OPS'),(28,'TEAMWORK','BRANCH_OPS');
/*!40000 ALTER TABLE `kra_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kra_sub_group`
--

DROP TABLE IF EXISTS `kra_sub_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `kra_sub_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `kra_group_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kra_sub_group`
--

LOCK TABLES `kra_sub_group` WRITE;
/*!40000 ALTER TABLE `kra_sub_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `kra_sub_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `org_group`
--

DROP TABLE IF EXISTS `org_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `org_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `parent_org_id` int(11) DEFAULT '0',
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `org_type` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`),
  KEY `idx_sync_id` (`sync_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `org_group`
--

LOCK TABLES `org_group` WRITE;
/*!40000 ALTER TABLE `org_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `org_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `org_group_position`
--

DROP TABLE IF EXISTS `org_group_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `org_group_position` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `org_group_id` int(11) NOT NULL,
  `position_id` int(11) NOT NULL,
  `is_head` bit(1) DEFAULT NULL,
  `reporting_position_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `org_group_position`
--

LOCK TABLES `org_group_position` WRITE;
/*!40000 ALTER TABLE `org_group_position` DISABLE KEYS */;
/*!40000 ALTER TABLE `org_group_position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `position`
--

DROP TABLE IF EXISTS `position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `position` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `position_level_id` int(11) NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `parent_position_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position`
--

LOCK TABLES `position` WRITE;
/*!40000 ALTER TABLE `position` DISABLE KEYS */;
/*!40000 ALTER TABLE `position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `psgc_city`
--

DROP TABLE IF EXISTS `psgc_city`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `psgc_region_id` int(11) NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `psgc_city`
--

LOCK TABLES `psgc_city` WRITE;
/*!40000 ALTER TABLE `psgc_city` DISABLE KEYS */;
/*!40000 ALTER TABLE `psgc_city` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `psgc_region`
--

DROP TABLE IF EXISTS `psgc_region`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_region` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `psgc_region`
--

LOCK TABLES `psgc_region` WRITE;
/*!40000 ALTER TABLE `psgc_region` DISABLE KEYS */;
/*!40000 ALTER TABLE `psgc_region` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rating_table`
--

DROP TABLE IF EXISTS `rating_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `rating_table` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) NOT NULL,
  `description` varchar(255) NOT NULL,
  `min_score` decimal(8,2) DEFAULT NULL,
  `max_score` decimal(8,2) DEFAULT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rating_table`
--

LOCK TABLES `rating_table` WRITE;
/*!40000 ALTER TABLE `rating_table` DISABLE KEYS */;
INSERT INTO `rating_table` VALUES (1,'EE','Exceeds Expectation',96.00,120.99,'2020-11-13 17:24:43',NULL),(2,'ME','Meets Expectation',85.00,95.99,'2020-11-13 17:24:43',NULL),(3,'SBE','Slightly Below Expectation',75.00,84.99,'2020-11-13 17:24:43',NULL),(4,'BE','Below Expectation',0.00,74.99,'2020-11-13 17:24:43',NULL);
/*!40000 ALTER TABLE `rating_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reference`
--

DROP TABLE IF EXISTS `reference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `reference` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_maintainable` bit(1) DEFAULT b'0',
  `user_id` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_reference` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
INSERT INTO `reference` VALUES (1,'KPI_TYPE','KPI Type',_binary '\0',1,'2020-09-01 17:44:12'),(2,'KRA_TYPE','KRA Type',_binary '\0',1,'2020-09-01 17:44:12'),(3,'ORGGROUPTYPE','Types of Organization Group',_binary '',1,'2020-09-25 11:05:12'),(4,'KPI_CODE_COUNTER','Latest Counter for KPI Code',_binary '',1,'2020-09-28 16:50:20');
/*!40000 ALTER TABLE `reference` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reference_value`
--

DROP TABLE IF EXISTS `reference_value`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `reference_value` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_code` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `value` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `user_id` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_reference_value` (`value`,`ref_code`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference_value`
--

LOCK TABLES `reference_value` WRITE;
/*!40000 ALTER TABLE `reference_value` DISABLE KEYS */;
INSERT INTO `reference_value` VALUES (1,'KPI_TYPE','QUALITATIVE','QUALITATIVE',1,'2020-09-01 17:46:35'),(2,'KPI_TYPE','QUANTITATIVE','QUANTITATIVE',1,'2020-09-01 17:46:35'),(3,'KRA_TYPE','SALES','SALES',1,'2020-09-01 17:46:35'),(4,'KRA_TYPE','KSA','KSA',1,'2020-09-01 17:46:35'),(5,'KRA_TYPE','BRANCH_OPS','BRANCH OPERATIONS',1,'2020-09-01 17:46:35'),(6,'KRA_TYPE','HO','HEAD OFFICE',1,'2020-09-01 17:46:36'),(7,'KRA_TYPE','FINANCE_OPS','FINANCE OPERATIONS',1,'2020-09-01 17:46:36'),(8,'ORGGROUPTYPE','TOP','1.0 TOP LEVEL',1,'2020-09-25 11:07:14'),(9,'ORGGROUPTYPE','BRN','3.5 BRANCH',1,'2020-09-25 11:07:14'),(10,'ORGGROUPTYPE','DEPT','4.1 DEPARTMENT',1,'2020-09-25 11:07:14'),(11,'ORGGROUPTYPE','REG','3.2 REGION',1,'2020-09-25 11:07:14'),(12,'ORGGROUPTYPE','AREA','3.3 AREA',1,'2020-09-25 11:07:14'),(13,'ORGGROUPTYPE','ZONE','3.1 ZONE',1,'2020-09-25 11:07:14'),(14,'ORGGROUPTYPE','CLUS','3.4 CLUSTER',1,'2020-09-25 11:07:14'),(15,'ORGGROUPTYPE','OPS','2.0 GROUP',1,'2020-09-25 11:07:14'),(16,'ORGGROUPTYPE','SECTION','4.2 SECTION',1,'2020-09-25 11:07:14'),(17,'ORGGROUPTYPE','UNIT','4.3 UNIT',1,'2020-09-25 11:07:14'),(23,'KPI_CODE_COUNTER','1','COUNTER',1,'2020-09-28 16:51:53'),(27,'EMP_SCORE_STATUS','NEW','NEW',1,'2020-10-20 13:07:03'),(28,'EMP_SCORE_STATUS','WIP','WIP',1,'2020-10-20 13:07:03'),(29,'EMP_SCORE_STATUS','FOR_APPROVAL','FOR_APPROVAL',1,'2020-10-20 13:07:03'),(30,'EMP_SCORE_STATUS','APPROVED','APPROVED',1,'2020-10-20 13:07:03'),(31,'EMP_SCORE_STATUS','FOR_REVISION','FOR_REVISION',1,'2020-10-20 13:07:03'),(32,'EMP_SCORE_STATUS','FINALIZED','FINALIZED',1,'2020-10-20 13:07:03'),(33,'DEFAULT_PASS_SCORE','85','Default passing score',1,'2020-10-30 16:55:54'),(34,'DASHBOARD','SUMMARY_FOR_EVAL','SUMMARY FOR EVALUATION',1,'2020-11-13 17:24:44'),(35,'DASHBOARD','SUMMARY_FOR_APPR','SUMMARY FOR APPROVAL (REGION)',1,'2020-11-13 17:24:44'),(36,'DASHBOARD','SUMMARY_RESULTS_REG','SUMMARY OF RESULTS REGIONAL WITH POSITION',1,'2020-11-13 17:24:44'),(37,'DASHBOARD','SUMMARY_RESULTS_BRN','SUMMARY OF RESULTS BRANCHES WITH POSITION',1,'2020-11-13 17:24:44'),(38,'DASHBOARD','SUMMARY_RESULTS_POS','SUMMARY OF RESULTS POSITION ONLY',1,'2020-11-13 17:24:44'),(39,'KPI_SCORE_MAX_VALUE','1.2','KPI Score maximum value',1,'2020-11-27 17:32:57'),(40,'KPI_SOURCE_TYPE','AUTOCREATE','AUTOCREATE',1,'2020-11-27 17:32:57'),(41,'KPI_SOURCE_TYPE','KEY-IN','KEY-IN',1,'2020-11-27 17:32:57'),(42,'DASHBOARD','SUMMARY_FOR_APPR_BRN','SUMMARY FOR APPROVAL (BRANCH)',1,'2021-01-15 10:56:59'),(43,'DASHBOARD','SUMMARY_FOR_APPR_CLU','SUMMARY FOR APPROVAL (CLUSTER)',1,'2021-01-15 10:56:59'),(44,'EMP_SCORE_STATUS','VOID','VOID',1,'2021-02-01 12:37:29');
/*!40000 ALTER TABLE `reference_value` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trans_employee_score`
--

DROP TABLE IF EXISTS `trans_employee_score`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `trans_employee_score` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `trans_summary_id` int(11) NOT NULL,
  `employee_id` int(11) NOT NULL,
  `org_group_id` int(11) NOT NULL,
  `position_id` int(11) NOT NULL,
  `score` decimal(16,5) DEFAULT NULL,
  `pdate_from` date DEFAULT NULL,
  `pdate_to` date DEFAULT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `level_of_approval` int(11) DEFAULT '0',
  `requestor_id` int(11) DEFAULT NULL,
  `approver_ids` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `approver_position_id` int(11) DEFAULT NULL,
  `approver_org_group_id` int(11) DEFAULT NULL,
  `approver_role_ids` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `employee_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `lastname` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `firstname` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `middlename` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `suffix` varchar(10) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `position_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `position_title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `area` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `region_department` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_org_group_id` (`org_group_id`),
  KEY `idx_position_id` (`position_id`),
  KEY `idx_pdate_from` (`pdate_from`),
  KEY `idx_pdate_to` (`pdate_to`),
  KEY `idx_area` (`area`),
  KEY `idx_region_department` (`region_department`),
  KEY `idx_employee_id` (`employee_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trans_employee_score`
--

LOCK TABLES `trans_employee_score` WRITE;
/*!40000 ALTER TABLE `trans_employee_score` DISABLE KEYS */;
/*!40000 ALTER TABLE `trans_employee_score` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trans_employee_score_details`
--

DROP TABLE IF EXISTS `trans_employee_score_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `trans_employee_score_details` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `trans_id` int(11) NOT NULL,
  `kpi_id` int(11) NOT NULL,
  `kpi_weight` decimal(8,5) NOT NULL,
  `kpi_target` decimal(16,5) DEFAULT NULL,
  `kpi_actual` decimal(16,5) DEFAULT NULL,
  `is_editable` bit(1) NOT NULL DEFAULT b'0',
  `kpi_score` decimal(16,5) DEFAULT NULL,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_kpi_score` (`kpi_score`),
  KEY `idx_kpi_weight` (`kpi_weight`),
  KEY `idx_trans_id` (`trans_id`),
  KEY `idx_kpi_id` (`kpi_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trans_employee_score_details`
--

LOCK TABLES `trans_employee_score_details` WRITE;
/*!40000 ALTER TABLE `trans_employee_score_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `trans_employee_score_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trans_employee_score_summary`
--

DROP TABLE IF EXISTS `trans_employee_score_summary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `trans_employee_score_summary` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `description` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `filter_by` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `filter_org_group` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `filter_include_lvl_below` bit(1) DEFAULT NULL,
  `filter_position` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `tdate_from` date DEFAULT NULL,
  `tdate_to` date DEFAULT NULL,
  `filter_override` bit(1) DEFAULT NULL,
  `filter_use_current` bit(1) DEFAULT NULL,
  `filter_employee` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `processed_employees` bigint(20) NOT NULL,
  `total_num_of_employees` bigint(20) NOT NULL,
  `employees_with_ipm` int(11) NOT NULL DEFAULT '0',
  `rating_ee_employees` int(11) NOT NULL DEFAULT '0',
  `rating_me_employees` int(11) NOT NULL DEFAULT '0',
  `rating_sbe_employees` int(11) NOT NULL DEFAULT '0',
  `rating_be_employees` int(11) NOT NULL DEFAULT '0',
  `is_done` bit(1) NOT NULL DEFAULT b'0',
  `is_trans_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trans_employee_score_summary`
--

LOCK TABLES `trans_employee_score_summary` WRITE;
/*!40000 ALTER TABLE `trans_employee_score_summary` DISABLE KEYS */;
/*!40000 ALTER TABLE `trans_employee_score_summary` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `tv_autocomplete`
--

DROP TABLE IF EXISTS `tv_autocomplete`;
/*!50001 DROP VIEW IF EXISTS `tv_autocomplete`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_autocomplete` AS SELECT 
 1 AS `id`,
 1 AS `description`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'clean_ems_ipm'
--

--
-- Dumping routines for database 'clean_ems_ipm'
--
/*!50003 DROP FUNCTION IF EXISTS `fn_employee_get_subordinates` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_employee_get_subordinates`(
    param_employee_id INT
) RETURNS text CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des_p TEXT;
    DECLARE local_start_from_ids TEXT;
    DECLARE local_org_group_descendants TEXT;
    
	DECLARE tmp_child_id_des_p int;
	SET @hierarchy_level_des_p = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des_p;
	DROP TEMPORARY TABLE IF EXISTS param_des_p;
	DROP TEMPORARY TABLE IF EXISTS results_des_p;
	CREATE TEMPORARY TABLE tmp_des_p (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE results_des_p (child_id INT NOT NULL, org_group_id INT,  hierarchy_level INT NOT NULL);
	
    SET GROUP_CONCAT_MAX_LEN = 4294967295;
    SET local_start_from_ids = (SELECT CONCAT(position_id,'|',org_group_id) FROM employee WHERE sync_id = param_employee_id AND is_active = 1);
    
    IF EXISTS(SELECT 1 FROM employee_roving WHERE employee_id = param_employee_id AND is_active = 1)
    THEN 
		SET local_start_from_ids = CONCAT(local_start_from_ids,',',
        (
			SELECT GROUP_CONCAT(DISTINCT CONCAT(position_id,'|',org_group_id) SEPARATOR ',') 
			FROM employee_roving 
			WHERE employee_id = param_employee_id AND is_active = 1
		));
    END IF;
    
    INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1;
	
    SET local_org_group_descendants = (SELECT GROUP_CONCAT(DISTINCT org_group_id SEPARATOR ',')  FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1);
    SET local_org_group_descendants = (SELECT fn_org_group_get_descendants(local_org_group_descendants));
    
	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_des_p) > 0 DO
		SET tmp_child_id_des_p = (SELECT MIN(child_id) FROM tmp_des_p);
		DELETE FROM tmp_des_p WHERE child_id = tmp_child_id_des_p;
        IF EXISTS (SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants))
        THEN
			SET @hierarchy_level_des_p = IFNULL((SELECT hierarchy_level FROM results_des_p WHERE child_id = tmp_child_id_des_p LIMIT 1),1);
			SET @hierarchy_level_des_p := @hierarchy_level_des_p + 1;
        END IF;
        
        INSERT INTO results_des_p SELECT position_id, org_group_id, @hierarchy_level_des_p FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
		INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
	END WHILE;

    
	SET returned_result_des_p = 
    (
		SELECT 
			GROUP_CONCAT(DISTINCT CONCAT(child_id,'|',org_group_id) SEPARATOR ',') 
		FROM results_des_p 
    );
    
    RETURN returned_result_des_p;
    -- FIND_IN_SET(CONCAT(position_id,'|',org_group_id), start_from_ids)
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_employee_get_subordinates_one_level` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_employee_get_subordinates_one_level`(
    param_employee_id INT
) RETURNS text CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des_p TEXT;
    DECLARE local_start_from_ids TEXT;
    DECLARE local_org_group_descendants TEXT;
    
	DECLARE tmp_child_id_des_p int;
	SET @hierarchy_level_des_p = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des_p;
	DROP TEMPORARY TABLE IF EXISTS param_des_p;
	DROP TEMPORARY TABLE IF EXISTS results_des_p;
	CREATE TEMPORARY TABLE tmp_des_p (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE results_des_p (child_id INT NOT NULL, org_group_id INT,  hierarchy_level INT NOT NULL);
	
    SET GROUP_CONCAT_MAX_LEN = 4294967295;
    SET local_start_from_ids = (SELECT CONCAT(position_id,'|',org_group_id) FROM employee WHERE sync_id = param_employee_id AND is_active = 1);
    
    IF EXISTS(SELECT 1 FROM employee_roving WHERE employee_id = param_employee_id AND is_active = 1)
    THEN 
		SET local_start_from_ids = CONCAT(local_start_from_ids,',',
        (
			SELECT GROUP_CONCAT(DISTINCT CONCAT(position_id,'|',org_group_id) SEPARATOR ',') 
			FROM employee_roving 
			WHERE employee_id = param_employee_id AND is_active = 1
		));
    END IF;
    
    INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1;
	
    SET local_org_group_descendants = (SELECT GROUP_CONCAT(DISTINCT org_group_id SEPARATOR ',')  FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1);
    SET local_org_group_descendants = (SELECT fn_org_group_get_descendants(local_org_group_descendants));
    
	SET SQL_SAFE_UPDATES = 0;
	IF (SELECT COUNT(*) FROM tmp_des_p) > 0 THEN
		SET tmp_child_id_des_p = (SELECT MIN(child_id) FROM tmp_des_p);
		DELETE FROM tmp_des_p WHERE child_id = tmp_child_id_des_p;
        IF EXISTS (SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants))
        THEN
			SET @hierarchy_level_des_p = IFNULL((SELECT hierarchy_level FROM results_des_p WHERE child_id = tmp_child_id_des_p LIMIT 1),1);
			SET @hierarchy_level_des_p := @hierarchy_level_des_p + 1;
        END IF;
        
        INSERT INTO results_des_p SELECT position_id, org_group_id, @hierarchy_level_des_p FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
		INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
	END IF;

    
	SET returned_result_des_p = 
    (
		SELECT 
			GROUP_CONCAT(DISTINCT CONCAT(child_id,'|',org_group_id) SEPARATOR ',') 
		FROM results_des_p 
    );
    
    RETURN returned_result_des_p;
    -- FIND_IN_SET(CONCAT(position_id,'|',org_group_id), start_from_ids)
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_employee_get_subordinates_two_levels` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_employee_get_subordinates_two_levels`(
    param_employee_id INT
) RETURNS text CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des_p TEXT;
    DECLARE local_start_from_ids TEXT;
    DECLARE local_org_group_descendants TEXT;
    
	DECLARE tmp_child_id_des_p int;
	SET @hierarchy_level_des_p = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des_p;
	DROP TEMPORARY TABLE IF EXISTS param_des_p;
	DROP TEMPORARY TABLE IF EXISTS results_des_p;
	CREATE TEMPORARY TABLE tmp_des_p (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE results_des_p (child_id INT NOT NULL, org_group_id INT,  hierarchy_level INT NOT NULL);
	
    SET GROUP_CONCAT_MAX_LEN = 4294967295;
    SET local_start_from_ids = (SELECT CONCAT(position_id,'|',org_group_id) FROM employee WHERE sync_id = param_employee_id AND is_active = 1);
    
    IF EXISTS(SELECT 1 FROM employee_roving WHERE employee_id = param_employee_id AND is_active = 1)
    THEN 
		SET local_start_from_ids = CONCAT(local_start_from_ids,',',
        (
			SELECT GROUP_CONCAT(DISTINCT CONCAT(position_id,'|',org_group_id) SEPARATOR ',') 
			FROM employee_roving 
			WHERE employee_id = param_employee_id AND is_active = 1
		));
    END IF;
    
    INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1;
	
    SET local_org_group_descendants = (SELECT GROUP_CONCAT(DISTINCT org_group_id SEPARATOR ',')  FROM org_group_position 
		WHERE FIND_IN_SET(CONCAT(position_id,'|',org_group_id), local_start_from_ids)
        AND is_active = 1);
    SET local_org_group_descendants = (SELECT fn_org_group_get_descendants(local_org_group_descendants));
    
	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_des_p ) < 7  DO
		SET tmp_child_id_des_p = (SELECT MIN(child_id) FROM tmp_des_p);
		DELETE FROM tmp_des_p WHERE child_id = tmp_child_id_des_p;
        IF EXISTS (SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants))
        THEN
			SET @hierarchy_level_des_p = IFNULL((SELECT hierarchy_level FROM results_des_p WHERE child_id = tmp_child_id_des_p LIMIT 1), 1);
			SET @hierarchy_level_des_p := @hierarchy_level_des_p + 1;
        END IF;
        
        INSERT INTO results_des_p SELECT position_id, org_group_id, @hierarchy_level_des_p FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
		INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
	END WHILE;

    
	SET returned_result_des_p = 
    (
		SELECT 
			GROUP_CONCAT(DISTINCT CONCAT(child_id,'|',org_group_id) SEPARATOR ',') 
		FROM results_des_p 
    );
    
    RETURN returned_result_des_p;
    -- FIND_IN_SET(CONCAT(position_id,'|',org_group_id), start_from_ids)
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_org_group_get_chart_upward_by_org_type` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_org_group_get_chart_upward_by_org_type`(
    start_from INT
    , param_org_type VARCHAR(50)
) RETURNS varchar(16383) CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_uptype VARCHAR(16383);
    DECLARE tmp_child_id_uptype int;
	SET @hierarchy_level_uptype = 00000;
	DROP TEMPORARY TABLE IF EXISTS tmp_uptype;
	DROP TEMPORARY TABLE IF EXISTS results_uptype;
	DROP TEMPORARY TABLE IF EXISTS param_uptype;
	CREATE TEMPORARY TABLE tmp_uptype (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE results_uptype (child_id INT NOT NULL, org_type VARCHAR(50));
	INSERT INTO tmp_uptype SELECT start_from;

	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_uptype) > 0 
     AND (SELECT COUNT(*) FROM results_uptype WHERE org_type = param_org_type) = 0 
    DO
		SET tmp_child_id_uptype = (SELECT child_id FROM tmp_uptype LIMIT 1);
		DELETE FROM tmp_uptype WHERE child_id = tmp_child_id_uptype;
        INSERT INTO results_uptype 
			SELECT sync_id, org_type
            FROM org_group 
            WHERE sync_id = tmp_child_id_uptype 
				AND is_active = 1 AND org_type = param_org_type;
		INSERT INTO tmp_uptype 
			SELECT parent_org_id 
            FROM org_group 
            WHERE sync_id = tmp_child_id_uptype 
            AND is_active = 1;
	END WHILE;

	SET returned_result_uptype =
	(
		SELECT child_id FROM results_uptype limit 1
	);
    RETURN returned_result_uptype;    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_org_group_get_descendants` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_org_group_get_descendants`(
    start_from_ids TEXT
) RETURNS text CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des TEXT;
    
	DECLARE tmp_child_id_des int;
	SET @hierarchy_level_des = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des;
	DROP TEMPORARY TABLE IF EXISTS param_des;
	DROP TEMPORARY TABLE IF EXISTS results_des;
	CREATE TEMPORARY TABLE tmp_des (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE param_des (child_id INT NOT NULL);
-- 	CREATE INDEX tmp_child_id_idx ON tmp (child_id);
	CREATE TEMPORARY TABLE results_des (child_id INT NOT NULL, hierarchy_level INT NOT NULL);
	INSERT INTO tmp_des SELECT sync_id FROM org_group WHERE FIND_IN_SET(sync_id, start_from_ids);
	INSERT INTO param_des SELECT child_id FROM tmp_des;
	
	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_des) > 0 DO
		SET tmp_child_id_des = (SELECT MIN(child_id) FROM tmp_des);
		DELETE FROM tmp_des WHERE child_id = tmp_child_id_des;
        IF EXISTS (SELECT sync_id FROM org_group WHERE parent_org_id = tmp_child_id_des AND id <> parent_org_id AND is_active = 1)
        THEN
			SET @hierarchy_level_des = IFNULL((SELECT hierarchy_level FROM results_des WHERE child_id = tmp_child_id_des LIMIT 1),1);
			SET @hierarchy_level_des := @hierarchy_level_des + 1;
        END IF;
        
        INSERT INTO results_des SELECT sync_id, @hierarchy_level_des FROM org_group WHERE parent_org_id = tmp_child_id_des AND is_active = 1;
		INSERT INTO tmp_des SELECT sync_id FROM org_group WHERE parent_org_id = tmp_child_id_des AND id <> parent_org_id AND is_active = 1;
	END WHILE;

	SET GROUP_CONCAT_MAX_LEN = 4294967295;
    
	SET returned_result_des = 
    (SELECT GROUP_CONCAT(DISTINCT r.child_id SEPARATOR ',') FROM org_group o
    JOIN (SELECT child_id, hierarchy_level FROM results_des UNION SELECT child_id, 1 FROM param_des) r 
		ON o.sync_id = r.child_id AND o.id <> o.parent_org_id AND o.is_active = 1);
    
    RETURN returned_result_des;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_position_get_descendants` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_position_get_descendants`(
    start_from_ids VARCHAR(16383),
    param_org_group_id INT
) RETURNS varchar(16383) CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des_p VARCHAR(16383);
    DECLARE local_org_group_descendants VARCHAR(16383);
    
	DECLARE tmp_child_id_des_p int;
	SET @hierarchy_level_des_p = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des_p;
	DROP TEMPORARY TABLE IF EXISTS param_des_p;
	DROP TEMPORARY TABLE IF EXISTS results_des_p;
	CREATE TEMPORARY TABLE tmp_des_p (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE param_des_p (child_id INT NOT NULL);
-- 	CREATE INDEX tmp_child_id_idx ON tmp (child_id);
	CREATE TEMPORARY TABLE results_des_p (child_id INT NOT NULL, hierarchy_level INT NOT NULL);
	INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE FIND_IN_SET(position_id, start_from_ids) AND org_group_id = param_org_group_id;
	INSERT INTO param_des_p SELECT child_id FROM tmp_des_p;
	
    SET local_org_group_descendants = (SELECT fn_org_group_get_descendants(param_org_group_id));
    
	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_des_p) > 0 DO
		SET tmp_child_id_des_p = (SELECT MIN(child_id) FROM tmp_des_p);
		DELETE FROM tmp_des_p WHERE child_id = tmp_child_id_des_p;
        IF EXISTS (SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants))
        THEN
			SET @hierarchy_level_des_p = IFNULL((SELECT hierarchy_level FROM results_des_p WHERE child_id = tmp_child_id_des_p LIMIT 1),1);
			SET @hierarchy_level_des_p := @hierarchy_level_des_p + 1;
        END IF;
        
        INSERT INTO results_des_p SELECT position_id, @hierarchy_level_des_p FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
		INSERT INTO tmp_des_p SELECT position_id FROM org_group_position 
		WHERE reporting_position_id = tmp_child_id_des_p AND position_id <> reporting_position_id 
		AND is_active = 1 AND FIND_IN_SET(org_group_id, local_org_group_descendants);
	END WHILE;

	SET GROUP_CONCAT_MAX_LEN = 16383;
    
	SET returned_result_des_p = 
    (SELECT GROUP_CONCAT(DISTINCT r.child_id SEPARATOR ',') FROM org_group_position p
    JOIN (SELECT child_id, hierarchy_level FROM results_des_p UNION SELECT child_id, 1 FROM param_des_p) r 
		ON p.position_id = r.child_id AND p.position_id <> p.reporting_position_id AND p.is_active = 1);
    
    RETURN returned_result_des_p;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_branches_with_position` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_branches_with_position`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_branch_delimited VARCHAR(500),
	IN param_position_delimited VARCHAR(500),
	IN param_kra_group_delimited VARCHAR(500),
	IN param_kpi_delimited VARCHAR(500),
    IN param_ee_min INT,
    IN param_ee_max INT,
    IN param_me_min INT,
    IN param_me_max INT,
    IN param_sbe_min INT,
    IN param_sbe_max INT,
    IN param_be_min INT,
    IN param_be_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_branch_delimited VARCHAR(1000);
	DECLARE local_param_position_delimited VARCHAR(1000);
	DECLARE local_param_kra_group_delimited VARCHAR(1000);
	DECLARE local_param_kpi_delimited VARCHAR(1000);
    DECLARE local_param_ee_min VARCHAR(100);
    DECLARE local_param_ee_max VARCHAR(100);
    DECLARE local_param_me_min VARCHAR(100);
    DECLARE local_param_me_max VARCHAR(100);
    DECLARE local_param_sbe_min VARCHAR(100);
    DECLARE local_param_sbe_max VARCHAR(100);
    DECLARE local_param_be_min VARCHAR(100);
    DECLARE local_param_be_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_branch_delimited = CASE WHEN param_branch_delimited = '' THEN 'branch_id' ELSE param_branch_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'position_id' ELSE param_position_delimited END;
    SET local_param_kra_group_delimited = CASE WHEN param_kra_group_delimited = '' THEN 'kra_group_id' ELSE param_kra_group_delimited END;
    SET local_param_kpi_delimited = CASE WHEN param_kpi_delimited = '' THEN 'kpi_id' ELSE param_kpi_delimited END;
    
    SET local_param_ee_min = CASE WHEN param_ee_min < 0 THEN 'ee' ELSE CONVERT(param_ee_min, CHAR(100)) END;
    SET local_param_ee_max = CASE WHEN param_ee_max < 0 THEN 'ee' ELSE CONVERT(param_ee_max, CHAR(100)) END;
    SET local_param_me_min = CASE WHEN param_me_min < 0 THEN 'me' ELSE CONVERT(param_me_min, CHAR(100)) END;
    SET local_param_me_max = CASE WHEN param_me_max < 0 THEN 'me' ELSE CONVERT(param_me_max, CHAR(100)) END;
    SET local_param_sbe_min = CASE WHEN param_sbe_min < 0 THEN 'sbe' ELSE CONVERT(param_sbe_min, CHAR(100)) END;
    SET local_param_sbe_max = CASE WHEN param_sbe_max < 0 THEN 'sbe' ELSE CONVERT(param_sbe_max, CHAR(100)) END;
    SET local_param_be_min = CASE WHEN param_be_min < 0 THEN 'be' ELSE CONVERT(param_be_min, CHAR(100)) END;
    SET local_param_be_max = CASE WHEN param_be_max < 0 THEN 'be' ELSE CONVERT(param_be_max, CHAR(100)) END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    -- ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' branch_id IN (', local_param_branch_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' position_id IN (', local_param_position_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kra_group_id IN (', local_param_kra_group_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kpi_id IN (', local_param_kpi_delimited, ')'
    ,' )'
    ,' AND'
	,' ('
		,' ee BETWEEN ', local_param_ee_min, ' AND ', local_param_ee_max
	,' )'
    ,' AND'
	,' ('
		,' me BETWEEN ', local_param_me_min, ' AND ', local_param_me_max
	,' )'
     ,' AND'
	,' ('
		,' sbe BETWEEN ', local_param_sbe_min, ' AND ', local_param_sbe_max
	,' )'
    ,' AND'
	,' ('
		,' be BETWEEN ', local_param_be_min, ' AND ', local_param_be_max
	,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'branch ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Branch'	THEN CONCAT('branch ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KRAGroup'	THEN CONCAT('kra_group ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPI'		THEN CONCAT('kpi ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'EE'		THEN CONCAT('ee ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ME'		THEN CONCAT('me ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SBE'		THEN CONCAT('sbe ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'BE'		THEN CONCAT('be ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
								SELECT 
									trans_summary_id
									, CONCAT(b.code, '' - '', b.description) `branch`
									, IFNULL(b.sync_id, 0) `branch_id`
									, a.position_id
									, `position`
									, a.kpi_id
									, CONCAT(d.code, '' - '', d.name) `kpi`
									, e.id `kra_group_id`
									, e.name `kra_group`
									, SUM(ee) `ee`
									, SUM(me) `me`
									, SUM(sbe) `sbe`
									, SUM(be) `be`
									
								FROM
								(
									SELECT 
										a.trans_summary_id
										, a.employee_id
										, a.position_id
                                        , CONCAT(a.position_code, '' - '', a.position_title) `position`
										, a.org_group_code
										, b.kpi_id
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(e.min_score, 0) AND IFNULL(e.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `ee`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(f.min_score, 0) AND IFNULL(f.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `me`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(g.min_score, 0) AND IFNULL(g.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `sbe`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(h.min_score, 0) AND IFNULL(h.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `be` 
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
                                        INNER JOIN rating_table e ON e.code = ''EE''
                                        INNER JOIN rating_table f ON f.code = ''ME''
                                        INNER JOIN rating_table g ON g.code = ''SBE''
                                        INNER JOIN rating_table h ON h.code = ''BE''
										WHERE a.is_active = 1 
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.employee_id, a.position_id
                                        , a.position_code, a.position_title, c.tdate_from, c.tdate_to
								) a 
								INNER JOIN org_group b ON b.org_type = ''BRN'' AND a.org_group_code = b.code
								LEFT JOIN position c ON a.position_id = c.sync_id
								LEFT JOIN kpi d ON a.kpi_id = d.id
								LEFT JOIN kra_group e ON d.kra_group = e.id
								GROUP BY b.sync_id, a.position_id, a.kpi_id
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, branch
		, branch_id
        , position
        , position_id
        , kpi
        , kpi_id
        , kra_group
        , kra_group_id
        , ee
		, me
		, sbe
        , be
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_position_only` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_position_only`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_position_delimited VARCHAR(500),
	IN param_kra_group_delimited VARCHAR(500),
	IN param_kpi_delimited VARCHAR(500),
    IN param_ee_min INT,
    IN param_ee_max INT,
    IN param_me_min INT,
    IN param_me_max INT,
    IN param_sbe_min INT,
    IN param_sbe_max INT,
    IN param_be_min INT,
    IN param_be_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_position_delimited VARCHAR(1000);
	DECLARE local_param_kra_group_delimited VARCHAR(1000);
	DECLARE local_param_kpi_delimited VARCHAR(1000);
    DECLARE local_param_ee_min VARCHAR(100);
    DECLARE local_param_ee_max VARCHAR(100);
    DECLARE local_param_me_min VARCHAR(100);
    DECLARE local_param_me_max VARCHAR(100);
    DECLARE local_param_sbe_min VARCHAR(100);
    DECLARE local_param_sbe_max VARCHAR(100);
    DECLARE local_param_be_min VARCHAR(100);
    DECLARE local_param_be_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'position_id' ELSE param_position_delimited END;
    SET local_param_kra_group_delimited = CASE WHEN param_kra_group_delimited = '' THEN 'kra_group_id' ELSE param_kra_group_delimited END;
    SET local_param_kpi_delimited = CASE WHEN param_kpi_delimited = '' THEN 'kpi_id' ELSE param_kpi_delimited END;
    
    SET local_param_ee_min = CASE WHEN param_ee_min < 0 THEN 'ee' ELSE CONVERT(param_ee_min, CHAR(100)) END;
    SET local_param_ee_max = CASE WHEN param_ee_max < 0 THEN 'ee' ELSE CONVERT(param_ee_max, CHAR(100)) END;
    SET local_param_me_min = CASE WHEN param_me_min < 0 THEN 'me' ELSE CONVERT(param_me_min, CHAR(100)) END;
    SET local_param_me_max = CASE WHEN param_me_max < 0 THEN 'me' ELSE CONVERT(param_me_max, CHAR(100)) END;
    SET local_param_sbe_min = CASE WHEN param_sbe_min < 0 THEN 'sbe' ELSE CONVERT(param_sbe_min, CHAR(100)) END;
    SET local_param_sbe_max = CASE WHEN param_sbe_max < 0 THEN 'sbe' ELSE CONVERT(param_sbe_max, CHAR(100)) END;
    SET local_param_be_min = CASE WHEN param_be_min < 0 THEN 'be' ELSE CONVERT(param_be_min, CHAR(100)) END;
    SET local_param_be_max = CASE WHEN param_be_max < 0 THEN 'be' ELSE CONVERT(param_be_max, CHAR(100)) END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
	 ,' ('
		,' position_id IN (', local_param_position_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kra_group_id IN (', local_param_kra_group_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kpi_id IN (', local_param_kpi_delimited, ')'
    ,' )'
    ,' AND'
	,' ('
		,' ee BETWEEN ', local_param_ee_min, ' AND ', local_param_ee_max
	,' )'
    ,' AND'
	,' ('
		,' me BETWEEN ', local_param_me_min, ' AND ', local_param_me_max
	,' )'
     ,' AND'
	,' ('
		,' sbe BETWEEN ', local_param_sbe_min, ' AND ', local_param_sbe_max
	,' )'
    ,' AND'
	,' ('
		,' be BETWEEN ', local_param_be_min, ' AND ', local_param_be_max
	,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'position ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KRAGroup'	THEN CONCAT('kra_group ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPI'		THEN CONCAT('kpi ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'EE'		THEN CONCAT('ee ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ME'		THEN CONCAT('me ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SBE'		THEN CONCAT('sbe ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'BE'		THEN CONCAT('be ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
								SELECT 
									trans_summary_id
									, a.position_id
									, a.position
									, a.kpi_id
									, CONCAT(d.code, '' - '', d.name) `kpi`
									, e.id `kra_group_id`
									, e.name `kra_group`
									, SUM(ee) `ee`
									, SUM(me) `me`
									, SUM(sbe) `sbe`
									, SUM(be) `be`
									
								FROM
								(
									SELECT 
										a.trans_summary_id
										, a.employee_id
										, a.position_id
										, CONCAT(a.position_code, '' - '', a.position_title) `position`
										, b.kpi_id
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(e.min_score, 0) AND IFNULL(e.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `ee`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(f.min_score, 0) AND IFNULL(f.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `me`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(g.min_score, 0) AND IFNULL(g.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `sbe`
										, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(h.min_score, 0) AND IFNULL(h.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `be` 
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
                                        INNER JOIN rating_table e ON e.code = ''EE''
                                        INNER JOIN rating_table f ON f.code = ''ME''
                                        INNER JOIN rating_table g ON g.code = ''SBE''
                                        INNER JOIN rating_table h ON h.code = ''BE''
										WHERE a.is_active = 1 
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.employee_id, a.position_id
                                        , a.position_code, a.position_title, c.tdate_from, c.tdate_to, b.kpi_id
								) a 
								LEFT JOIN kpi d ON a.kpi_id = d.id
								LEFT JOIN kra_group e ON d.kra_group = e.id
								GROUP BY a.position_id, a.position_id, a.kpi_id
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, position
        , position_id
        , kpi
        , kpi_id
        , kra_group
        , kra_group_id
        , ee
		, me
		, sbe
        , be
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_regional_with_position` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_regional_with_position`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_region_delimited VARCHAR(500),
	IN param_position_delimited VARCHAR(500),
	IN param_kra_group_delimited VARCHAR(500),
	IN param_kpi_delimited VARCHAR(500),
    IN param_ee_min INT,
    IN param_ee_max INT,
    IN param_me_min INT,
    IN param_me_max INT,
    IN param_sbe_min INT,
    IN param_sbe_max INT,
    IN param_be_min INT,
    IN param_be_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_region_delimited VARCHAR(1000);
	DECLARE local_param_position_delimited VARCHAR(1000);
	DECLARE local_param_kra_group_delimited VARCHAR(1000);
	DECLARE local_param_kpi_delimited VARCHAR(1000);
    DECLARE local_param_ee_min VARCHAR(100);
    DECLARE local_param_ee_max VARCHAR(100);
    DECLARE local_param_me_min VARCHAR(100);
    DECLARE local_param_me_max VARCHAR(100);
    DECLARE local_param_sbe_min VARCHAR(100);
    DECLARE local_param_sbe_max VARCHAR(100);
    DECLARE local_param_be_min VARCHAR(100);
    DECLARE local_param_be_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_region_delimited = CASE WHEN param_region_delimited = '' THEN 'region_id' ELSE param_region_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'position_id' ELSE param_position_delimited END;
    SET local_param_kra_group_delimited = CASE WHEN param_kra_group_delimited = '' THEN 'kra_group_id' ELSE param_kra_group_delimited END;
    SET local_param_kpi_delimited = CASE WHEN param_kpi_delimited = '' THEN 'kpi_id' ELSE param_kpi_delimited END;
    
    SET local_param_ee_min = CASE WHEN param_ee_min < 0 THEN 'ee' ELSE CONVERT(param_ee_min, CHAR(100)) END;
    SET local_param_ee_max = CASE WHEN param_ee_max < 0 THEN 'ee' ELSE CONVERT(param_ee_max, CHAR(100)) END;
    SET local_param_me_min = CASE WHEN param_me_min < 0 THEN 'me' ELSE CONVERT(param_me_min, CHAR(100)) END;
    SET local_param_me_max = CASE WHEN param_me_max < 0 THEN 'me' ELSE CONVERT(param_me_max, CHAR(100)) END;
    SET local_param_sbe_min = CASE WHEN param_sbe_min < 0 THEN 'sbe' ELSE CONVERT(param_sbe_min, CHAR(100)) END;
    SET local_param_sbe_max = CASE WHEN param_sbe_max < 0 THEN 'sbe' ELSE CONVERT(param_sbe_max, CHAR(100)) END;
    SET local_param_be_min = CASE WHEN param_be_min < 0 THEN 'be' ELSE CONVERT(param_be_min, CHAR(100)) END;
    SET local_param_be_max = CASE WHEN param_be_max < 0 THEN 'be' ELSE CONVERT(param_be_max, CHAR(100)) END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    -- ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' region_id IN (', local_param_region_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' position_id IN (', local_param_position_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kra_group_id IN (', local_param_kra_group_delimited, ')'
    ,' )'
	,' AND'
	 ,' ('
		,' kpi_id IN (', local_param_kpi_delimited, ')'
    ,' )'
    ,' AND'
	,' ('
		,' ee BETWEEN ', local_param_ee_min, ' AND ', local_param_ee_max
	,' )'
    ,' AND'
	,' ('
		,' me BETWEEN ', local_param_me_min, ' AND ', local_param_me_max
	,' )'
     ,' AND'
	,' ('
		,' sbe BETWEEN ', local_param_sbe_min, ' AND ', local_param_sbe_max
	,' )'
    ,' AND'
	,' ('
		,' be BETWEEN ', local_param_be_min, ' AND ', local_param_be_max
	,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'region ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Region'	THEN CONCAT('region ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KRAGroup'	THEN CONCAT('kra_group ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPI'		THEN CONCAT('kpi ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'EE'		THEN CONCAT('ee ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ME'		THEN CONCAT('me ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SBE'		THEN CONCAT('sbe ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'BE'		THEN CONCAT('be ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
							SELECT 
								trans_summary_id
								, region
								, a.position_id
								, `position`
								, a.kpi_id
								, CONCAT(d.code, '' - '', d.name) `kpi`
								, e.id `kra_group_id`
								, e.name `kra_group`
								, IFNULL(b.sync_id, 0) `region_id`
								, SUM(ee) `ee`
								, SUM(me) `me`
								, SUM(sbe) `sbe`
								, SUM(be) `be`
								
							FROM
							(
								SELECT 
									a.trans_summary_id
									, a.employee_id
									, a.position_id
									, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
									, b.kpi_id
                                    , CONCAT(a.position_code, '' - '', a.position_title) `position`
									, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(e.min_score, 0) AND IFNULL(e.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `ee`
									, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(f.min_score, 0) AND IFNULL(f.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `me`
									, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(g.min_score, 0) AND IFNULL(g.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `sbe`
									, CASE WHEN ROUND(b.kpi_score * 100, 2) BETWEEN IFNULL(h.min_score, 0) AND IFNULL(h.max_score, ROUND(b.kpi_score * 100, 2)) THEN 1 ELSE 0 END `be` 
									, c.tdate_from, c.tdate_to, a.is_active, a.status
									FROM trans_employee_score a
									INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
									INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
                                    INNER JOIN rating_table e ON e.code = ''EE''
									INNER JOIN rating_table f ON f.code = ''ME''
									INNER JOIN rating_table g ON g.code = ''SBE''
									INNER JOIN rating_table h ON h.code = ''BE''
                                    AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
									WHERE a.is_active = 1 
									GROUP BY a.trans_summary_id, a.employee_id, a.position_id
                                        , a.position_code, a.position_title, c.tdate_from, c.tdate_to
							) a 
							LEFT JOIN org_group b ON b.org_type = ''REG'' AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
							LEFT JOIN position c ON a.position_id = c.sync_id
							LEFT JOIN kpi d ON a.kpi_id = d.id
							LEFT JOIN kra_group e ON d.kra_group = e.id
							GROUP BY a.region, a.position_id, a.kpi_id
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, region
		, region_id
        , position
        , position_id
        , kpi
        , kpi_id
        , kra_group
        , kra_group_id
        , ee
		, me
		, sbe
        , be
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_summary_for_approval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_summary_for_approval`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_region_delimited VARCHAR(500),
    IN param_no_key_in_min INT,
    IN param_no_key_in_max INT,
    IN param_for_approval_min INT,
    IN param_for_approval_max INT,
    IN param_finalized_min INT,
    IN param_finalized_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_region_delimited VARCHAR(1000);
    DECLARE local_param_no_key_in_min VARCHAR(100);
    DECLARE local_param_no_key_in_max VARCHAR(100);
    DECLARE local_param_for_approval_min VARCHAR(100);
    DECLARE local_param_for_approval_max VARCHAR(100);
    DECLARE local_param_finalized_min VARCHAR(100);
    DECLARE local_param_finalized_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_region_delimited = CASE WHEN param_region_delimited = '' THEN 'region_id' ELSE param_region_delimited END;
    
    SET local_param_no_key_in_min = CASE WHEN param_no_key_in_min < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_min, CHAR(100)) END;
    SET local_param_no_key_in_max = CASE WHEN param_no_key_in_max < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_max, CHAR(100)) END;
    SET local_param_for_approval_min = CASE WHEN param_for_approval_min < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_min, CHAR(100)) END;
    SET local_param_for_approval_max = CASE WHEN param_for_approval_max < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_max, CHAR(100)) END;
    SET local_param_finalized_min = CASE WHEN param_finalized_min < 0 THEN 'finalized' ELSE CONVERT(param_finalized_min, CHAR(100)) END;
    SET local_param_finalized_max = CASE WHEN param_finalized_max < 0 THEN 'finalized' ELSE CONVERT(param_finalized_max, CHAR(100)) END;
    
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
   --  ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' region_id IN (', local_param_region_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' no_key_in BETWEEN ', local_param_no_key_in_min, ' AND ', local_param_no_key_in_max
	,' )'
    ,' AND'
	,' ('
		,' for_approval BETWEEN ', local_param_for_approval_min, ' AND ', local_param_for_approval_max
	,' )'
    ,' AND'
	,' ('
		,' finalized BETWEEN ', local_param_finalized_min, ' AND ', local_param_finalized_max
	,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'region ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Region'				THEN CONCAT('region ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoKeyIn'		THEN CONCAT('no_key_in ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ForApproval'		THEN CONCAT('for_approval ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Finalized'				THEN CONCAT('finalized ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
							SELECT 
									trans_summary_id
									, IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'') `region`
									, IFNULL(b.sync_id, 0) `region_id`
									, SUM(no_key_in) `no_key_in`
                                    , SUM(for_approval) `for_approval`
									, SUM(finalized) `finalized`
									
								FROM
                                (
									SELECT 
										a.trans_summary_id
										, a.employee_id
										, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
										 , b.kpi_id
										 , CASE WHEN a.status = ''NEW''
											 THEN 1 ELSE 0 END `no_key_in`
										 , CASE WHEN a.status = ''FOR_APPROVAL''
												THEN 1 ELSE 0 END `for_approval`
										 , CASE WHEN a.status = ''FINALIZED'' OR a.status = ''APPROVED'' 
											 THEN 1 ELSE 0 END `finalized`
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
										WHERE a.is_active = 1 -- AND a.status NOT IN (''NEW'')
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.id -- , a.employee_id, c.tdate_from, c.tdate_to
									) a 
									LEFT JOIN org_group b ON b.org_type = ''REG'' 
									AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
								 GROUP BY a.region
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, region
		, region_id
        , no_key_in
		, for_approval
		, finalized
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_summary_for_approval_brn` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_summary_for_approval_brn`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_region_delimited VARCHAR(500),
	IN param_branch_delimited VARCHAR(500),
    IN param_no_key_in_min INT,
    IN param_no_key_in_max INT,
    IN param_for_approval_min INT,
    IN param_for_approval_max INT,
    IN param_finalized_min INT,
    IN param_finalized_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_region_delimited VARCHAR(1000);
	DECLARE local_param_branch_delimited VARCHAR(1000);
    DECLARE local_param_no_key_in_min VARCHAR(100);
    DECLARE local_param_no_key_in_max VARCHAR(100);
    DECLARE local_param_for_approval_min VARCHAR(100);
    DECLARE local_param_for_approval_max VARCHAR(100);
    DECLARE local_param_finalized_min VARCHAR(100);
    DECLARE local_param_finalized_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_region_delimited = CASE WHEN param_region_delimited = '' THEN 'region_id' ELSE param_region_delimited END;
    SET local_param_branch_delimited = CASE WHEN param_branch_delimited = '' THEN 'branch_id' ELSE param_branch_delimited END;
    
    SET local_param_no_key_in_min = CASE WHEN param_no_key_in_min < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_min, CHAR(100)) END;
    SET local_param_no_key_in_max = CASE WHEN param_no_key_in_max < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_max, CHAR(100)) END;
    SET local_param_for_approval_min = CASE WHEN param_for_approval_min < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_min, CHAR(100)) END;
    SET local_param_for_approval_max = CASE WHEN param_for_approval_max < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_max, CHAR(100)) END;
    SET local_param_finalized_min = CASE WHEN param_finalized_min < 0 THEN 'finalized' ELSE CONVERT(param_finalized_min, CHAR(100)) END;
    SET local_param_finalized_max = CASE WHEN param_finalized_max < 0 THEN 'finalized' ELSE CONVERT(param_finalized_max, CHAR(100)) END;
    
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
   --  ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' region_id IN (', local_param_region_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' branch_id IN (', local_param_branch_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' no_key_in BETWEEN ', local_param_no_key_in_min, ' AND ', local_param_no_key_in_max
	,' )'
    ,' AND'
	,' ('
		,' for_approval BETWEEN ', local_param_for_approval_min, ' AND ', local_param_for_approval_max
	,' )'
    ,' AND'
	,' ('
		,' finalized BETWEEN ', local_param_finalized_min, ' AND ', local_param_finalized_max
	,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'region ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Region'				THEN CONCAT('region ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Branch'				THEN CONCAT('branch ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoKeyIn'		THEN CONCAT('no_key_in ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ForApproval'		THEN CONCAT('for_approval ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Finalized'				THEN CONCAT('finalized ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
							SELECT 
									trans_summary_id
									, IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'') `region`
									, IFNULL(CONCAT(c.code, '' - '', c.description), ''OTHERS'') `branch`
									, IFNULL(b.sync_id, 0) `region_id`
									, IFNULL(c.sync_id, 0) `branch_id`
									, SUM(no_key_in) `no_key_in`
                                    , SUM(for_approval) `for_approval`
									, SUM(finalized) `finalized`
									
								FROM
                                (
									SELECT 
										a.trans_summary_id
										, a.employee_id
                                        , a.org_group_id
										, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
										 , b.kpi_id
										 , CASE WHEN a.status = ''NEW''
											 THEN 1 ELSE 0 END `no_key_in`
										 , CASE WHEN a.status = ''FOR_APPROVAL''
												THEN 1 ELSE 0 END `for_approval`
										 , CASE WHEN a.status = ''FINALIZED'' OR a.status = ''APPROVED'' 
											 THEN 1 ELSE 0 END `finalized`
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
										WHERE a.is_active = 1 -- AND a.status NOT IN (''NEW'')
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.id, a.employee_id, a.org_group_id
									) a 
									LEFT JOIN org_group b ON b.org_type = ''REG'' 
									AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
                                    LEFT JOIN org_group c ON c.org_type = ''BRN'' 
									AND a.org_group_id = c.sync_id
								GROUP BY a.region, c.sync_id
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, region
        , branch
		, region_id
        , no_key_in
		, for_approval
		, finalized
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_summary_for_approval_clu` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_summary_for_approval_clu`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_region_delimited VARCHAR(500),
	IN param_cluster_delimited VARCHAR(500),
    IN param_no_key_in_min INT,
    IN param_no_key_in_max INT,
    IN param_for_approval_min INT,
    IN param_for_approval_max INT,
    IN param_finalized_min INT,
    IN param_finalized_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table_sum VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_region_delimited VARCHAR(1000);
	DECLARE local_param_cluster_delimited VARCHAR(1000);
    DECLARE local_param_no_key_in_min VARCHAR(100);
    DECLARE local_param_no_key_in_max VARCHAR(100);
    DECLARE local_param_for_approval_min VARCHAR(100);
    DECLARE local_param_for_approval_max VARCHAR(100);
    DECLARE local_param_finalized_min VARCHAR(100);
    DECLARE local_param_finalized_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_region_delimited = CASE WHEN param_region_delimited = '' THEN 'region_id' ELSE param_region_delimited END;
    SET local_param_cluster_delimited = CASE WHEN param_cluster_delimited = '' THEN 'cluster_id' ELSE param_cluster_delimited END;
    
    SET local_param_no_key_in_min = CASE WHEN param_no_key_in_min < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_min, CHAR(100)) END;
    SET local_param_no_key_in_max = CASE WHEN param_no_key_in_max < 0 THEN 'no_key_in' ELSE CONVERT(param_no_key_in_max, CHAR(100)) END;
    SET local_param_for_approval_min = CASE WHEN param_for_approval_min < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_min, CHAR(100)) END;
    SET local_param_for_approval_max = CASE WHEN param_for_approval_max < 0 THEN 'for_approval' ELSE CONVERT(param_for_approval_max, CHAR(100)) END;
    SET local_param_finalized_min = CASE WHEN param_finalized_min < 0 THEN 'finalized' ELSE CONVERT(param_finalized_min, CHAR(100)) END;
    SET local_param_finalized_max = CASE WHEN param_finalized_max < 0 THEN 'finalized' ELSE CONVERT(param_finalized_max, CHAR(100)) END;
    
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
   --  ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' region_id IN (', local_param_region_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' cluster_id IN (', local_param_cluster_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' no_key_in BETWEEN ', local_param_no_key_in_min, ' AND ', local_param_no_key_in_max
	,' )'
    ,' AND'
	,' ('
		,' for_approval BETWEEN ', local_param_for_approval_min, ' AND ', local_param_for_approval_max
	,' )'
    ,' AND'
	,' ('
		,' finalized BETWEEN ', local_param_finalized_min, ' AND ', local_param_finalized_max
	,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'region ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Region'				THEN CONCAT('region ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Cluster'				THEN CONCAT('cluster ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoKeyIn'		THEN CONCAT('no_key_in ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ForApproval'		THEN CONCAT('for_approval ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Finalized'				THEN CONCAT('finalized ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table_sum = CONCAT('(
							SELECT 
									trans_summary_id
									, IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'') `region`
									, IFNULL(CONCAT(d.code, '' - '', d.description), ''OTHERS'') `cluster`
									, IFNULL(b.sync_id, 0) `region_id`
									, IFNULL(d.sync_id, 0) `cluster_id`
									, SUM(no_key_in) `no_key_in`
                                    , SUM(for_approval) `for_approval`
									, SUM(finalized) `finalized`
									
								FROM
                                (
									SELECT 
										a.trans_summary_id
										, a.employee_id
                                        , a.org_group_id
										, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
										 , b.kpi_id
										 , CASE WHEN a.status = ''NEW''
											 THEN 1 ELSE 0 END `no_key_in`
										 , CASE WHEN a.status = ''FOR_APPROVAL''
												THEN 1 ELSE 0 END `for_approval`
										 , CASE WHEN a.status = ''FINALIZED'' OR a.status = ''APPROVED'' 
											 THEN 1 ELSE 0 END `finalized`
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
										WHERE a.is_active = 1 -- AND a.status NOT IN (''NEW'')
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.id, a.employee_id, a.org_group_id
									) a 
									LEFT JOIN org_group b ON b.org_type = ''REG'' 
									AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
                                    LEFT JOIN temp_org_group_cluster c ON a.org_group_id = c.org_group_id
                                    LEFT JOIN org_group d ON d.org_type = ''CLUS'' AND c.cluster_id = d.sync_id
								GROUP BY a.region, c.cluster_id, d.sync_id
								)');
                                
	SET join_table = CONCAT('(
							SELECT 
									trans_summary_id
									, IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'') `region`
									, IFNULL(CONCAT(d.code, '' - '', d.description), ''OTHERS'') `cluster`
									, IFNULL(b.sync_id, 0) `region_id`
									, IFNULL(d.sync_id, 0) `cluster_id`
									, SUM(no_key_in) `no_key_in`
                                    , SUM(for_approval) `for_approval`
									, SUM(finalized) `finalized`
									
								FROM
                                (
									SELECT 
										a.trans_summary_id
										, a.employee_id
                                        , a.org_group_id
										, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
										 , b.kpi_id
										 , CASE WHEN a.status = ''NEW''
											 THEN 1 ELSE 0 END `no_key_in`
										 , CASE WHEN a.status = ''FOR_APPROVAL''
												THEN 1 ELSE 0 END `for_approval`
										 , CASE WHEN a.status = ''FINALIZED'' OR a.status = ''APPROVED'' 
											 THEN 1 ELSE 0 END `finalized`
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
										WHERE a.is_active = 1 -- AND a.status NOT IN (''NEW'')
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.id, a.employee_id, a.org_group_id
									) a 
									LEFT JOIN org_group b ON b.org_type = ''REG'' 
									AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
                                    LEFT JOIN temp_org_group_cluster1 c ON a.org_group_id = c.org_group_id
                                    LEFT JOIN org_group d ON d.org_type = ''CLUS'' AND c.cluster_id = d.sync_id
								GROUP BY a.region, c.cluster_id, d.sync_id
								)');
    
    
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_org_group_cluster (org_group_id INT, cluster_id INT);
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_org_group_cluster1 (org_group_id INT, cluster_id INT);
    
    INSERT INTO temp_org_group_cluster 
    SELECT sync_id, IFNULL(fn_org_group_get_chart_upward_by_org_type(sync_id, 'CLUS'),0) 
    FROM org_group WHERE is_active = 1;
    
    INSERT INTO temp_org_group_cluster1 SELECT * FROM temp_org_group_cluster; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table_sum,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, region
        , cluster
		, region_id
        , no_key_in
		, for_approval
		, finalized
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_org_group_cluster; 
    DROP TEMPORARY TABLE IF EXISTS temp_org_group_cluster1; 
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_dashboard_summary_for_evaluation` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_dashboard_summary_for_evaluation`(
	IN param_run_id_delimited VARCHAR(500),
	IN param_region_delimited VARCHAR(500),
    IN param_with_complete_score_min INT,
    IN param_with_complete_score_max INT,
    IN param_with_missing_score_min INT,
    IN param_with_missing_score_max INT,
    IN param_no_score_min INT,
    IN param_no_score_max INT,
    IN param_on_going_eval_min INT,
    IN param_on_going_eval_max INT,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT   
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE join_table VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_run_id_delimited VARCHAR(1000);
	DECLARE local_param_region_delimited VARCHAR(1000);
    DECLARE local_param_with_complete_score_min VARCHAR(100);
    DECLARE local_param_with_complete_score_max VARCHAR(100);
    DECLARE local_param_with_missing_score_min VARCHAR(100);
    DECLARE local_param_with_missing_score_max VARCHAR(100);
    DECLARE local_param_no_score_min VARCHAR(100);
    DECLARE local_param_no_score_max VARCHAR(100);
    DECLARE local_param_on_going_eval_min VARCHAR(100);
    DECLARE local_param_on_going_eval_max VARCHAR(100);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_run_id_delimited = CASE WHEN param_run_id_delimited = '' THEN '0' ELSE param_run_id_delimited END;
    SET local_param_region_delimited = CASE WHEN param_region_delimited = '' THEN 'region_id' ELSE param_region_delimited END;
    
    SET local_param_with_complete_score_min = CASE WHEN param_with_complete_score_min < 0 THEN 'with_complete_score' ELSE CONVERT(param_with_complete_score_min, CHAR(100)) END;
    SET local_param_with_complete_score_max = CASE WHEN param_with_complete_score_max < 0 THEN 'with_complete_score' ELSE CONVERT(param_with_complete_score_max, CHAR(100)) END;
    SET local_param_with_missing_score_min = CASE WHEN param_with_missing_score_min < 0 THEN 'with_missing_score' ELSE CONVERT(param_with_missing_score_min, CHAR(100)) END;
    SET local_param_with_missing_score_max = CASE WHEN param_with_missing_score_max < 0 THEN 'with_missing_score' ELSE CONVERT(param_with_missing_score_max, CHAR(100)) END;
    SET local_param_no_score_min = CASE WHEN param_no_score_min < 0 THEN 'no_score' ELSE CONVERT(param_no_score_min, CHAR(100)) END;
    SET local_param_no_score_max = CASE WHEN param_no_score_max < 0 THEN 'no_score' ELSE CONVERT(param_no_score_max, CHAR(100)) END;
    SET local_param_on_going_eval_min = CASE WHEN param_on_going_eval_min < 0 THEN 'on_going_evaluation' ELSE CONVERT(param_on_going_eval_min, CHAR(100)) END;
    SET local_param_on_going_eval_max = CASE WHEN param_on_going_eval_max < 0 THEN 'on_going_evaluation' ELSE CONVERT(param_on_going_eval_max, CHAR(100)) END;
    
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    -- ,' ('
-- 		,' IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')'
--     ,' )'
-- 	,' AND'
	 ,' ('
		,' region_id IN (', local_param_region_delimited, ')'
    ,' )'
	,' AND'
	,' ('
		,' with_complete_score BETWEEN ', local_param_with_complete_score_min, ' AND ', local_param_with_complete_score_max
	,' )'
    ,' AND'
	,' ('
		,' with_missing_score BETWEEN ', local_param_with_missing_score_min, ' AND ', local_param_with_missing_score_max
	,' )'
    ,' AND'
	,' ('
		,' no_score BETWEEN ', local_param_no_score_min, ' AND ', local_param_no_score_max
	,' )'
    ,' AND'
	,' ('
		,' on_going_evaluation BETWEEN ', local_param_on_going_eval_min, ' AND ', local_param_on_going_eval_max
	,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'region ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Region'				THEN CONCAT('region ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'WithCompleteScore'		THEN CONCAT('with_complete_score ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'WithMissingScore'		THEN CONCAT('with_missing_score ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoScore'				THEN CONCAT('no_score ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OnGoingEvaluation'		THEN CONCAT('on_going_evaluation ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
    
    SET join_table = CONCAT('(
								SELECT 
									trans_summary_id
									, IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'') `region`
									, IFNULL(b.sync_id, 0) `region_id`
									, SUM(with_complete_score) `with_complete_score`
									, SUM(with_missing_score) `with_missing_score`
									, SUM(no_score) `no_score`
									, SUM(on_going_evaluation) `on_going_evaluation`
								FROM
                                (
									SELECT 
										a.trans_summary_id
										, a.employee_id
										, CASE WHEN IFNULL(a.region_department, '''') = '''' THEN ''HEAD OFFICE'' ELSE a.region_department END `region`
										 , b.kpi_id
										 , CASE WHEN SUM(CASE WHEN IFNULL(b.kpi_score, 0) > 0 THEN 1 ELSE 0 END) = SUM(1)
											 THEN 1 ELSE 0 END `with_complete_score`
										 , CASE WHEN SUM(CASE WHEN IFNULL(b.kpi_score, 0) > 0 THEN 1 ELSE 0 END) < SUM(1) 
											AND a.status = ''NEW'' AND SUM(CASE WHEN IFNULL(b.kpi_score, 0) > 0 THEN 1 ELSE 0 END)  != 0
												THEN 1 ELSE 0 END `with_missing_score`
										 , CASE WHEN SUM(CASE WHEN IFNULL(b.kpi_score, 0) = 0 THEN 1 ELSE 0 END) = SUM(1)
											 THEN 1 ELSE 0 END `no_score`
										 , CASE WHEN SUM(CASE WHEN IFNULL(b.kpi_score, 0) > 0 THEN 1 ELSE 0 END) > 0 AND a.status != ''NEW''
											 THEN 1 ELSE 0 END `on_going_evaluation`
										, c.tdate_from, c.tdate_to, a.is_active, a.status
										FROM trans_employee_score a
										INNER JOIN trans_employee_score_details b ON a.id = b.trans_id
										INNER JOIN trans_employee_score_summary c ON a.trans_summary_id = c.id
										WHERE a.is_active = 1 AND a.status IN (''NEW'',''WIP'')
                                        AND IFNULL(a.trans_summary_id,0) IN (', local_param_run_id_delimited, ')
										GROUP BY a.trans_summary_id, a.id, a.employee_id, a.org_group_id
									) a 
									LEFT JOIN org_group b ON b.org_type = ''REG'' 
									AND a.region = IFNULL(CONCAT(b.code, '' - '', b.description), ''HEAD OFFICE'')
								 GROUP BY a.region
								)');
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_ipm_dashboard
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1  
							FROM ',join_table,' a
                            ', where_clause, ' ) x) `total_num`
		, trans_summary_id `id`
		, region
		, region_id
		, with_complete_score
		, with_missing_score
		, no_score
		, on_going_evaluation
	FROM ',join_table,'	a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_ipm_dashboard) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_ipm_dashboard; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
        , sync_id
        , sync_date
        , code
        , old_employee_id
        , lastname
        , firstname
        , middlename
        , suffix
        , nickname
		, INSTR(LOWER(CONCAT(lastname, '', '', firstname, '' '', middlename, IFNULL(CONCAT('' '', suffix),''''), '' ('', code, '') '')), LOWER(', local_term, ')) `index`
        , gender
        , employment_status
        , date_hired
        , org_group_id
        , position_id
        -- , psgc_region_id
        -- , psgc_city_id
        , system_user_id
        , is_active
        , created_date
	FROM employee
	WHERE LOWER(CONCAT(lastname, '', '', firstname, '' '', middlename, IFNULL(CONCAT('' '', suffix),''''), '' ('', code, '') '')) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
	ORDER BY `index`
		, CONCAT(lastname, '', '', firstname, '' '', middlename, IFNULL(CONCAT('' '', suffix),''''), '' ('', code, '') '') ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_kpi_score_get_list_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_kpi_score_get_list_by_id`(
	IN param_id INT,
    IN param_kra_group VARCHAR(255),
    IN param_kpi_code varchar(50),
    IN param_kpi_name varchar(255),
    IN param_kpi_description varchar(500),
    IN param_kpi_guidelines varchar(500),
    IN param_weight_min DECIMAL(16, 5),
    IN param_weight_max DECIMAL(16, 5),
    IN param_target_min DECIMAL(16, 5),
    IN param_target_max DECIMAL(16, 5),
    IN param_actual_min DECIMAL(16, 5),
    IN param_actual_max DECIMAL(16, 5),
    IN param_rate_min DECIMAL(16, 5),
    IN param_rate_max DECIMAL(16, 5),
    IN param_total_min DECIMAL(16, 5),
    IN param_total_max DECIMAL(16, 5),
    IN param_grade_delimited VARCHAR(255),
    IN param_source_type_delimited varchar(255),
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_kra_group VARCHAR(1000);
    DECLARE local_param_kpi_code VARCHAR(1000);
    DECLARE local_param_kpi_name VARCHAR(1000);
    DECLARE local_param_kpi_description VARCHAR(1000);
    DECLARE local_param_kpi_guidelines VARCHAR(1000);
    DECLARE local_param_weight_min VARCHAR(50);
    DECLARE local_param_weight_max VARCHAR(50);
    DECLARE local_param_target_min VARCHAR(50);
    DECLARE local_param_target_max VARCHAR(50);
    DECLARE local_param_actual_min VARCHAR(50);
    DECLARE local_param_actual_max VARCHAR(50);
    DECLARE local_param_rate_min VARCHAR(50);
    DECLARE local_param_rate_max VARCHAR(50);
    DECLARE local_param_total_min VARCHAR(50);
    DECLARE local_param_total_max VARCHAR(50);
    DECLARE local_param_grade_delimited VARCHAR(255);
    DECLARE local_param_source_type_delimited VARCHAR(255);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_kra_group = CASE WHEN param_kra_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kra_group,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_code = CASE WHEN param_kpi_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_code, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_name = CASE WHEN param_kpi_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_name, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_description = CASE WHEN param_kpi_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_description, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_guidelines = CASE WHEN param_kpi_guidelines = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_guidelines, '\\','\\\\'), '''', ''''''), '''') END;
    
    SET local_param_weight_min = CASE WHEN param_weight_min < 0 THEN 'IFNULL(a.kpi_weight, 0)' ELSE CONVERT(param_weight_min, CHAR(20)) END;
    SET local_param_weight_max = CASE WHEN param_weight_max < 0 THEN 'IFNULL(a.kpi_weight, 0)' ELSE CONVERT(param_weight_max, CHAR(20)) END;
    SET local_param_target_min = CASE WHEN param_target_min < 0 THEN 'IFNULL(a.kpi_target, 0)' ELSE CONVERT(param_target_min, CHAR(20)) END;
    SET local_param_target_max = CASE WHEN param_target_max < 0 THEN 'IFNULL(a.kpi_target, 0)' ELSE CONVERT(param_target_max, CHAR(20)) END;
    SET local_param_actual_min = CASE WHEN param_actual_min < 0 THEN 'IFNULL(a.kpi_actual, 0)' ELSE CONVERT(param_actual_min, CHAR(20)) END;
    SET local_param_actual_max = CASE WHEN param_actual_max < 0 THEN 'IFNULL(a.kpi_actual, 0)' ELSE CONVERT(param_actual_max, CHAR(20)) END;
    SET local_param_rate_min = CASE WHEN param_rate_min < 0 THEN 'IFNULL(a.kpi_score, 0)' ELSE CONVERT(param_rate_min, CHAR(20)) END;
    SET local_param_rate_max = CASE WHEN param_rate_max < 0 THEN 'IFNULL(a.kpi_score, 0)' ELSE CONVERT(param_rate_max, CHAR(20)) END;
    SET local_param_total_min = CASE WHEN param_total_min < 0 THEN 'IFNULL(a.kpi_score, 0) * IFNULL(a.kpi_weight, 0)' ELSE CONVERT(param_total_min, CHAR(20)) END;
    SET local_param_total_max = CASE WHEN param_total_max < 0 THEN 'IFNULL(a.kpi_score, 0) * IFNULL(a.kpi_weight, 0)' ELSE CONVERT(param_total_max, CHAR(20)) END;
    SET local_param_grade_delimited = CASE WHEN param_grade_delimited = '' THEN '''''' ELSE CONCAT('''',param_grade_delimited, '''') END;
    SET local_param_source_type_delimited = CASE WHEN param_source_type_delimited = '' THEN 'IFNULL(F.id, 0)' ELSE param_source_type_delimited END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
		, ' a.trans_id = ', local_param_id
		,' AND'
        ,' ('
			, local_param_kra_group , '= '''''
			, ' OR LOWER(c.name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kra_group , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_code , '= '''''
			, ' OR LOWER(b.code) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_code , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_name , '= '''''
			, ' OR LOWER(b.name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_name , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_description , '= '''''
			, ' OR LOWER(b.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_description , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_guidelines , '= '''''
			, ' OR LOWER(b.guidelines) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_guidelines , '), '' '', ''%''), ''%'')'
		,' )'
		,' AND'
		,' ('
			,' IFNULL(a.kpi_weight, 0) BETWEEN ', local_param_weight_min, ' AND ', local_param_weight_max
		,' )'
		,' AND'
		,' ('
			,' IFNULL(a.kpi_target, 0) BETWEEN ', local_param_target_min, ' AND ', local_param_target_max
		,' )'
		,' AND'
		,' ('
			,' IFNULL(a.kpi_actual, 0) BETWEEN ', local_param_actual_min, ' AND ', local_param_actual_max
		,' )'
		,' AND'
		,' ('
			,' IFNULL(a.kpi_score, 0) BETWEEN ', local_param_rate_min, ' AND ', local_param_rate_max
		,' )'
		,' AND'
		,' ('
			,' (IFNULL(a.kpi_score, 0) * IFNULL(a.kpi_weight, 0)) BETWEEN ', local_param_total_min, ' AND ', local_param_total_max
		,' )'
        ,' AND'
		,' ('
			, local_param_grade_delimited , ' = '''''
			, ' OR FIND_IN_SET(IFNULL(
			(SELECT code FROM rating_table WHERE (IFNULL(a.kpi_score, 0) * 100) BETWEEN IFNULL(min_score,0) AND IFNULL(max_score,0))
			, ''N/A''), (', local_param_grade_delimited, '))'
		,' )'
        ,' AND'
			,' ('
				,' IFNULL(F.id, 0) IN (', local_param_source_type_delimited, ')'
			,' )'
		);
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ' ,  
	IFNULL(CASE WHEN sidx = ''					THEN 'b.code ASC,' END, ''),
	IFNULL(CASE WHEN sidx = 'KRAGroup'			THEN CONCAT('c.name ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPICode'			THEN CONCAT('b.code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIName'			THEN CONCAT('b.name ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIDescription'	THEN CONCAT('b.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIGuidelines'		THEN CONCAT('b.guidelines ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Weight'		THEN CONCAT('IFNULL(a.kpi_weight, 0) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Target'		THEN CONCAT('IFNULL(a.kpi_target, 0) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Actual'		THEN CONCAT('IFNULL(a.kpi_actual, 0) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Rate'		THEN CONCAT('IFNULL(a.kpi_score, 0) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Total'		THEN CONCAT('IFNULL(a.kpi_score, 0) * IFNULL(a.kpi_weight, 0) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Grade'		THEN CONCAT('IFNULL(
			(SELECT code FROM rating_table WHERE (IFNULL(a.kpi_score, 0) * 100) BETWEEN IFNULL(min_score,0) AND IFNULL(max_score,0))
			, ''N/A'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'SourceType'		THEN CONCAT('IFNULL(F.id, 0) ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score_details; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score_details
    SELECT '
	, ' (SELECT COUNT(*) 
				FROM trans_employee_score_details a 
				-- INNER JOIN trans_employee_score head ON a.trans_id = head.id
				INNER JOIN kpi b ON a.kpi_id = b.id
				INNER JOIN kra_group c ON b.kra_group = c.id 
                INNER JOIN reference_value F ON b.source_type = F.value 
                ', where_clause, '
		) `total_num`
		, a.id `id`
        , b.id `kpi_id`
		, c.name `kra_group`
        , b.code `kpi_code`
        , b.name `kpi_name`
        , b.description `kpi_description`
        , b.guidelines `kpi_guidelines`
        , IFNULL(a.kpi_weight, 0) `weight`
        , IFNULL(a.kpi_target, 0) `target`
        , IFNULL(a.kpi_actual, 0) `actual`
        , IFNULL(a.kpi_score, 0) `rate` 
        , IFNULL(a.kpi_score, 0) * IFNULL(a.kpi_weight, 0) `total`
        , IFNULL(
			(SELECT code FROM rating_table WHERE (IFNULL(a.kpi_score, 0) * 100) BETWEEN IFNULL(min_score,0) AND IFNULL(max_score,0))
			, ''N/A'') `grade`
		,  b.source_type 
        , CASE WHEN b.source_type = ''KEY-IN'' THEN 1 ELSE 0 END `is_editable`
		FROM trans_employee_score_details a 
		-- INNER JOIN trans_employee_score head ON a.trans_id = head.id
		INNER JOIN kpi b ON a.kpi_id = b.id
		INNER JOIN kra_group c ON b.kra_group = c.id 
        INNER JOIN reference_value F ON b.source_type = F.value 
        '
    , where_clause, order_by_clause, ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score_details) a;');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score_details; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_details_get_list_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_details_get_list_by_id`(
	IN param_id INT,
    IN param_kra_group VARCHAR(255),
    IN param_kpi_code varchar(50),
    IN param_kpi_name varchar(255),
    IN param_kpi_description varchar(500),
    IN param_kpi_guidelines varchar(500),
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_kra_group VARCHAR(1000);
    DECLARE local_param_kpi_code VARCHAR(1000);
    DECLARE local_param_kpi_name VARCHAR(1000);
    DECLARE local_param_kpi_description VARCHAR(1000);
    DECLARE local_param_kpi_guidelines VARCHAR(1000);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_kra_group = CASE WHEN param_kra_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kra_group,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_code = CASE WHEN param_kpi_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_code, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_name = CASE WHEN param_kpi_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_name, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_description = CASE WHEN param_kpi_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_description, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_guidelines = CASE WHEN param_kpi_guidelines = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_kpi_guidelines, '\\','\\\\'), '''', ''''''), '''') END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
		, ' a.trans_id = ', local_param_id
		,' AND'
        ,' ('
			, local_param_kra_group , '= '''''
			, ' OR LOWER(c.name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kra_group , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_code , '= '''''
			, ' OR LOWER(b.code) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_code , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_name , '= '''''
			, ' OR LOWER(b.name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_name , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_description , '= '''''
			, ' OR LOWER(b.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_description , '), '' '', ''%''), ''%'')'
		,' )' 
		,' AND'
        ,' ('
			, local_param_kpi_guidelines , '= '''''
			, ' OR LOWER(b.guidelines) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_kpi_guidelines , '), '' '', ''%''), ''%'')'
		,' )'
		);
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ' ,  
	IFNULL(CASE WHEN sidx = ''					THEN 'a.trans_id ASC,' END, ''),
	IFNULL(CASE WHEN sidx = 'KRAGroup'			THEN CONCAT('c.name ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPICode'			THEN CONCAT('b.code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIName'			THEN CONCAT('b.name ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIDescription'	THEN CONCAT('b.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'KPIGuidelines'		THEN CONCAT('b.guidelines ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score_details; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score_details
    SELECT '
	, ' (SELECT COUNT(*) 
				FROM trans_employee_score_details a 
				INNER JOIN trans_employee_score head ON a.trans_id = head.id
				INNER JOIN kpi b ON a.kpi_id = b.id
				INNER JOIN kra_group c ON b.kra_group = c.id ', where_clause, '
		) `total_num`
		, a.id
		, c.name `kra_group`
        , b.code `kpi_code`
        , b.name `kpi_name`
        , b.description `kpi_description`
        , b.guidelines `kpi_guidelines`
        , a.kpi_weight `weight`
        , a.kpi_target `target`
        , a.kpi_actual `actual`
        , a.kpi_score `rate` 
		FROM trans_employee_score_details a 
		INNER JOIN trans_employee_score head ON a.trans_id = head.id
		INNER JOIN kpi b ON a.kpi_id = b.id
		INNER JOIN kra_group c ON b.kra_group = c.id '
    , where_clause, order_by_clause
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score_details) a;');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score_details; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_by_id`(
	IN param_id INT,
    IN param_role_delimited VARCHAR(500),
    IN param_user_id INT
)
BEGIN
	DECLARE local_param_role_delimited VARCHAR(1000);
    SET local_param_role_delimited = CASE WHEN param_role_delimited = '' THEN '0' ELSE REPLACE(param_role_delimited,',','|') END;
    
	SELECT 		
		-- Employee Score details
         a.id `id`
		, head.trans_summary_id
		, c.name `kra_group`
        , b.code `kpi_code`
        , b.name `kpi_name`
        , b.description `kpi_description`
        , b.guidelines `kpi_guidelines`
        , a.kpi_weight `weight`
        , a.kpi_target `target`
        , a.kpi_actual `actual`
        , a.kpi_score `rate`
        -- Employee details
        , CONCAT( d.lastname, ', ', d.firstname, ' ', d.middlename) `employee`
        , d.code `employee_code`
        , CONCAT(e.code, ' - ', e.description) `org_group`
		, CONCAT(f.code, ' - ', f.title) `position`
        -- Employee Score columns
        , DATE_FORMAT(g.tdate_from, '%m/%d/%Y') `tdate_from`
        , DATE_FORMAT(g.tdate_to, '%m/%d/%Y') `tdate_to`
        , DATE_FORMAT(head.pdate_from, '%m/%d/%Y') `pdate_from`
        , DATE_FORMAT(head.pdate_to, '%m/%d/%Y') `pdate_to`
        , head.employee_id
        , head.org_group_id
        , a.kpi_id
        , head.position_id
        , head.approver_ids
        , head.status
        , a.kpi_score
        , a.kpi_weight
        , source_type
        ,IFNULL(
			(SELECT code FROM rating_table WHERE (IFNULL(a.kpi_score, 0) * 100) BETWEEN IFNULL(min_score,0) AND IFNULL(max_score,0))
			, 'N/A') `grade`
        , a.is_editable
        ,CASE WHEN head.approver_role_ids REGEXP CONCAT('(^|,)(',local_param_role_delimited,')(,|$)') 
			AND !IFNULL(FIND_IN_SET(param_user_id, head.approver_ids),0)
        THEN 1 ELSE 0 END `has_edit_access`
		, IFNULL(CONCAT( h.lastname, ', ', h.firstname, ' ', h.middlename),'') `requestor`
	FROM trans_employee_score_details a
    INNER JOIN trans_employee_score head ON a.trans_id = head.id
    INNER JOIN kpi b ON a.kpi_id = b.id
    INNER JOIN kra_group c ON b.kra_group = c.id
    INNER JOIN employee d ON head.employee_id = d.sync_id
    INNER JOIN org_group e ON head.org_group_id = e.sync_id
	INNER JOIN position f ON head.position_id = f.sync_id
    INNER JOIN trans_employee_score_summary g ON head.trans_summary_id = g.id
	LEFT JOIN employee h ON head.requestor_id = h.system_user_id
	WHERE a.trans_id = param_id
    ORDER BY b.code;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_count_by_area` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_count_by_area`(
	IN param_area VARCHAR(500),
    IN param_count_min INT, 
    IN param_count_max INT,
	IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 
	DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383); 
    
	-- Define parameters for conversion to string
	DECLARE local_param_area VARCHAR(1000);
    DECLARE local_param_count_min VARCHAR(70);
    DECLARE local_param_count_max VARCHAR(70);
    
	DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;

    -- Convert passed parameters into string, to be concatenated into query portions
	SET local_param_area = CASE WHEN param_area = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_area,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_count_min = CASE WHEN param_count_min = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_min,CHAR(20)) END;
    SET local_param_count_max = CASE WHEN param_count_max = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_max,CHAR(20)) END;
    
	SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;

	-- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_area , '= '''''
		, ' OR LOWER(a.area) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_area , '), '' '', ''%''), ''%'')'
    ,' )'
    );

     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('a.area) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Count'		THEN CONCAT('IFNULL(COUNT(a.id), 0) ', local_sord, ',') END, ''));
    
    
	-- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
	    
	SET group_clause = " GROUP BY a.area ";
    
    SET having_clause = CONCAT(' HAVING (',' IFNULL(COUNT(a.id), 0) BETWEEN ', local_param_count_min, ' AND ', local_param_count_max,' )');
    
    -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_employee_score
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM trans_employee_score ', where_clause, group_clause, having_clause, ') x) `total_num`
			, a.area
			, IFNULL(COUNT(DISTINCT a.employee_id, ''-'', b.tdate_from, ''-'', b.tdate_to), 0) `count`
		FROM trans_employee_score a
        INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id ', where_clause
		, group_clause
        , having_clause
        , order_by_clause
        , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
 		CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_count_by_branch` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_count_by_branch`(
	IN param_branch VARCHAR(500),
	IN param_position VARCHAR(500),
    IN param_count_min INT, 
    IN param_count_max INT,
	IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 
	DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383); 
    
	-- Define parameters for conversion to string
	DECLARE local_param_branch VARCHAR(1000);
	DECLARE local_param_position VARCHAR(1000);
    DECLARE local_param_count_min VARCHAR(70);
    DECLARE local_param_count_max VARCHAR(70);
    
	DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;

    -- Convert passed parameters into string, to be concatenated into query portions
	SET local_param_branch = CASE WHEN param_branch = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_branch,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_position = CASE WHEN param_position = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_count_min = CASE WHEN param_count_min = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_min,CHAR(20)) END;
    SET local_param_count_max = CASE WHEN param_count_max = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_max,CHAR(20)) END;
    
	SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;

	-- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_branch , '= '''''
		, ' OR LOWER(CONCAT(a.org_group_code, '' - '', a.org_group_description)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_branch , '), '' '', ''%''), ''%'')'
    ,' )'
    , ' AND '
    ,' ('
		, local_param_position , '= '''''
		, ' OR LOWER(a.position_title) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position , '), '' '', ''%''), ''%'')'
    ,' )'
    );

     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Branch'	THEN CONCAT('CONCAT(a.org_group_code, '' - '', a.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('a.position_title) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Count'		THEN CONCAT('IFNULL(COUNT(a.id), 0) ', local_sord, ',') END, ''));
    
    
	-- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
	    
	SET group_clause = " GROUP BY CONCAT(a.org_group_code, '' - '', a.org_group_description) ";
    
    SET having_clause = CONCAT(' HAVING (',' IFNULL(COUNT(a.id), 0) BETWEEN ', local_param_count_min, ' AND ', local_param_count_max,' )');
    
    -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_employee_score
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM trans_employee_score ', where_clause, group_clause, having_clause, ') x) `total_num`
			, UPPER(CONCAT(a.org_group_code, '' - '', a.org_group_description)) `branch`
			, UPPER(a.position_title) `position`
			, IFNULL(COUNT(DISTINCT a.employee_id, ''-'', b.tdate_from, ''-'', b.tdate_to), 0) `count`
		FROM trans_employee_score a
        INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id ', where_clause
		, group_clause
        , having_clause
        , order_by_clause
        , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
 		CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_count_by_position` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_count_by_position`(
	IN param_position VARCHAR(500),
    IN param_count_min INT, 
    IN param_count_max INT,
	IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 
	DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383); 
    
	-- Define parameters for conversion to string
	DECLARE local_param_position VARCHAR(1000);
    DECLARE local_param_count_min VARCHAR(70);
    DECLARE local_param_count_max VARCHAR(70);
    
	DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;

    -- Convert passed parameters into string, to be concatenated into query portions
	SET local_param_position = CASE WHEN param_position = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_count_min = CASE WHEN param_count_min = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_min,CHAR(20)) END;
    SET local_param_count_max = CASE WHEN param_count_max = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_max,CHAR(20)) END;
    
	SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;

	-- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_position , '= '''''
		, ' OR LOWER(CONCAT(a.position_code, '' - '', a.position_title)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position , '), '' '', ''%''), ''%'')'
    ,' )'
    );

     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('CONCAT(a.position_code, '' - '', a.position_title) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Count'		THEN CONCAT('IFNULL(COUNT(a.id), 0) ', local_sord, ',') END, ''));
    
    
	-- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
	    
	SET group_clause = " GROUP BY CONCAT(a.position_code, '' - '', a.position_title) ";
    
    SET having_clause = CONCAT(' HAVING (',' IFNULL(COUNT(a.id), 0) BETWEEN ', local_param_count_min, ' AND ', local_param_count_max,' )');
    
    -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_employee_score
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM trans_employee_score ', where_clause, group_clause, having_clause, ') x) `total_num`
			, CONCAT(a.position_code, '' - '', a.position_title) `position`
			, IFNULL(COUNT(DISTINCT a.employee_id, ''-'', b.tdate_from, ''-'', b.tdate_to), 0) `count`
		FROM trans_employee_score a
        INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id ', where_clause
		, group_clause
        , having_clause
        , order_by_clause
        , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
 		CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_count_by_region` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_count_by_region`(
	IN param_region VARCHAR(500),
    IN param_count_min INT, 
    IN param_count_max INT,
	IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 
	DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383); 
    
	-- Define parameters for conversion to string
	DECLARE local_param_region VARCHAR(1000);
    DECLARE local_param_count_min VARCHAR(70);
    DECLARE local_param_count_max VARCHAR(70);
    
	DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;

    -- Convert passed parameters into string, to be concatenated into query portions
	SET local_param_region = CASE WHEN param_region = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_region,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_count_min = CASE WHEN param_count_min = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_min,CHAR(20)) END;
    SET local_param_count_max = CASE WHEN param_count_max = 0 THEN 'IFNULL(COUNT(a.id), 0)' ELSE CONVERT(param_count_max,CHAR(20)) END;
    
	SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;

	-- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_region , '= '''''
		, ' OR LOWER(a.region_department) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_region , '), '' '', ''%''), ''%'')'
    ,' )'
    );

     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('a.region_department) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Count'		THEN CONCAT('IFNULL(COUNT(a.id), 0) ', local_sord, ',') END, ''));
    
    
	-- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
	    
	SET group_clause = " GROUP BY a.region_department ";
    
    SET having_clause = CONCAT(' HAVING (',' IFNULL(COUNT(a.id), 0) BETWEEN ', local_param_count_min, ' AND ', local_param_count_max,' )');
    
    -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_employee_score
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM trans_employee_score ', where_clause, group_clause, having_clause, ') x) `total_num`
			, a.region_department `region`
			, IFNULL(COUNT(DISTINCT a.employee_id, ''-'', b.tdate_from, ''-'', b.tdate_to), 0) `count`
		FROM trans_employee_score a
        INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id ', where_clause
		, group_clause
        , having_clause
        , order_by_clause
        , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
 		CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_keyin_approval_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_keyin_approval_list`(
	IN param_trans_summary_id_delimited VARCHAR(500),
    IN param_description VARCHAR(500),
    IN param_name_delimited VARCHAR(255),
    IN param_parent_org_group VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_date_from_from VARCHAR(10),
    IN param_date_from_to VARCHAR(10),
    IN param_date_to_from VARCHAR(10),
    IN param_date_to_to VARCHAR(10),
    IN param_score_from DECIMAL(8, 5),
    IN param_score_to DECIMAL(8, 5),
    IN param_status_delimited VARCHAR(255),
    IN param_show_for_evaluation BOOLEAN,
    IN param_show_no_score BOOLEAN,
    IN param_is_export BOOLEAN,
    IN param_user_id INT,
    IN param_is_approval BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    -- DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_trans_summary_id_delimited VARCHAR(1000);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_name_delimited VARCHAR(1000);
    DECLARE local_param_parent_org_group VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_date_from_from VARCHAR(200);
    DECLARE local_param_date_from_to VARCHAR(200);
    DECLARE local_param_date_to_from VARCHAR(200);
    DECLARE local_param_date_to_to VARCHAR(200);
    DECLARE local_param_score_from VARCHAR(20);
    DECLARE local_param_score_to VARCHAR(20);
    DECLARE local_param_status_delimited VARCHAR(255);
    DECLARE local_param_show_for_evaluation VARCHAR(50);
    DECLARE local_param_show_no_score VARCHAR(50);
    DECLARE local_param_employee_id VARCHAR(20);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
	DECLARE local_param_check_position VARCHAR(1000);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_trans_summary_id_delimited = CASE WHEN param_trans_summary_id_delimited = '' THEN '0' ELSE param_trans_summary_id_delimited END;
    SET local_param_id = '0'; -- CONVERT(param_id, CHAR(20));
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_name_delimited = CASE WHEN param_name_delimited = '' THEN 'IFNULL(a.employee_id, 0)' ELSE param_name_delimited END;
    SET local_param_parent_org_group = CASE WHEN param_parent_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_parent_org_group, '\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'IFNULL(a.org_group_id, 0)' ELSE param_org_group_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(a.position_id, 0)' ELSE param_position_delimited END;
    SET local_param_score_from = CASE WHEN param_score_from < 0 THEN 'a.score' ELSE CONVERT(param_score_from, CHAR(20)) END;
    SET local_param_score_to = CASE WHEN param_score_to < 0 THEN 'a.score' ELSE CONVERT(param_score_to, CHAR(20)) END;
    
    SET local_param_date_from_from = 
	CASE WHEN STR_TO_DATE(param_date_from_from, '%m/%d/%Y') IS NULL OR param_date_from_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_from_to = 
	CASE WHEN STR_TO_DATE(param_date_from_to, '%m/%d/%Y') IS NULL OR param_date_from_to = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_from = 
	CASE WHEN STR_TO_DATE(param_date_to_from, '%m/%d/%Y') IS NULL OR param_date_to_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_to = 
	CASE WHEN STR_TO_DATE(param_date_to_to, '%m/%d/%Y') IS NULL OR param_date_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(d.tdate_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    -- SET local_param_score_from = CONVERT(param_score_from, CHAR(20));
--     SET local_param_score_to = CONVERT(param_score_to, CHAR(20));

    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'c.id' ELSE param_status_delimited END;
    
    SET local_param_show_for_evaluation = CASE WHEN param_show_for_evaluation = 1 THEN 'true' ELSE 'false' END;
    SET local_param_show_no_score = CASE WHEN param_show_no_score = 1 THEN 'true' ELSE 'false' END;
        
	SET local_param_employee_id = (SELECT sync_id FROM employee WHERE system_user_id = param_user_id);

    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    SET local_param_check_position = (SELECT B.code FROM employee A LEFT JOIN position B ON A.position_id = B.sync_id WHERE A.system_user_id = param_user_id);
    
   -- SET @subordinates = (SELECT IFNULL((SELECT ems_ipm.fn_employee_get_subordinates(local_param_employee_id)), ''));
   
    -- Construct where clause then concat the converted parameters
    SET @subordinates = (SELECT CASE WHEN param_is_approval = 0 THEN  (SELECT IFNULL((SELECT fn_employee_get_subordinates_one_level(local_param_employee_id)), ''))
									   WHEN local_param_check_position = 'RM' THEN  (SELECT IFNULL((SELECT fn_employee_get_subordinates_two_levels(local_param_employee_id)), '')) 
																			   ELSE (SELECT IFNULL((SELECT fn_employee_get_subordinates(local_param_employee_id)), '')) END);
    
     -- SET @subordinates = (SELECT CASE WHEN param_is_approval = 1 THEN  (SELECT IFNULL((SELECT ems_ipm.fn_employee_get_subordinates_two_levels(local_param_employee_id)), ''))ELSE 
-- 						 (SELECT IFNULL((SELECT ems_ipm.fn_employee_get_subordinates_one_level(local_param_employee_id)), '')) END);
    
    SET where_clause = CONCAT('WHERE'
     ,' a.is_active = 1'
     ,' AND'
     ,' ('
		,' is_trans_active = 1 '
     ,' )'
     -- ,' AND'
--      ,' ('
-- 		,' IFNULL(a.trans_summary_id, 0) IN (', local_param_trans_summary_id_delimited, ')'
--      ,' )'
     ,' AND'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
     ,' )'
      ,' AND'
     ,' ('
		 , local_param_description , '= '''''
		 , ' OR LOWER(d.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
     ,' )' 
     ,' AND'
     ,' ('
		 ,' IFNULL(a.employee_id, 0) IN (', local_param_name_delimited, ')'
     ,' )' 
     ,' AND'
    ,' ('
		, local_param_parent_org_group , '= '''''
		, ' OR LOWER(CONCAT(a.area, '' - '', a.region_department))
			LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_parent_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
      ' ('
		 ,' IFNULL(a.org_group_id, 0) IN (', local_param_org_group_delimited, ')'
     ,' )'
     ,' AND'
      ' ('
	 	,' IFNULL(a.position_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'     
     ,' AND'
     ,' ('
		,' IFNULL(CONVERT(d.tdate_from, DATE), '''') BETWEEN ', local_param_date_from_from, ' AND ', local_param_date_from_to
     ,' )'
     ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(d.tdate_to, DATE), '''') BETWEEN ', local_param_date_to_from, ' AND ', local_param_date_to_to
     ,' )'
     ,' AND'
     ,' ('
		,' c.id IN (', local_param_status_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
			,' a.score BETWEEN ', local_param_score_from, ' AND ', local_param_score_to
     ,' )'
     ,' AND'
     ,' ('
		,'	!',local_param_show_no_score
		,' 	OR '
		,'	('
			,local_param_show_no_score, ' AND score <= 0'
		,'	)'
     ,' )'
   --   ,' AND'
--      ,' ('
-- 		,'	!',local_param_show_for_evaluation
-- 		,' 	OR '
-- 		,'	('
-- 			,local_param_show_for_evaluation, ' AND b.min_score = 0 AND score > 0'
-- 		,'	)'
--      ,' )'
 ,
     CASE WHEN param_show_for_evaluation = 1
		 THEN
			CONCAT(' AND'
		 ,' ('
			,'	!',local_param_show_for_evaluation
			,' 	OR '
			,'	('
				,local_param_show_for_evaluation, ' AND b.min_score = 0 AND score > 0'
			,'	)'
		 ,' )')
		 ELSE ''
     END     
    --  ,' AND'
-- 		, ' FIND_IN_SET(CONCAT(position_id, ''|'', org_group_id), ''', @subordinates ,''' )'
	 ,' AND' 
		, ' employee_id != ', local_param_employee_id
     );
     
     IF (param_is_approval = 1)
     THEN
		SET where_clause = CONCAT(where_clause, 
           ' AND' 
			, ' requestor_id != ', param_user_id
		 , ' AND'  
		 ,' ( '
			-- , 'status IN (''FOR_APPROVAL'', ''APPROVED'', ''FINALIZED'', ''FOR_REVISION'')'
			, 'status NOT IN (''NEW'', ''WIP'')'
		 ,' ) '
         );
     END IF;
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ' ,  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.trans_summary_id DESC, a.id ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'TransSummaryID'		THEN CONCAT('a.trans_summary_id ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'		THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Employee'	THEN CONCAT('LOWER(CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename, IFNULL(CONCAT('' '', a.suffix),''''), '' ('', a.employee_code, '') '')) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'ParentOrgGroup' THEN CONCAT('LOWER(CONCAT(a.area, '' - '', a.region_department)) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OrgGroup'	THEN CONCAT('LOWER(CONCAT(a.org_group_code, '' - '',a.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('LOWER(CONCAT(a.position_code, '' - '',a.position_title)) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'TDateFrom'	THEN CONCAT('d.tdate_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'TDateTo'	THEN CONCAT('IFNULL(d.tdate_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Score' 	THEN CONCAT('a.score ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Status'	THEN CONCAT('a.status ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    -- Set Group By & Having clause
    -- SET group_by_clause = ' GROUP BY A.t_id';
	-- SET having_clause = CONCAT(
    -- COALESCE(
		-- CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from = 0 OR param_score_to = 0)
			-- THEN ' HAVING SUM(kpi_weight) < 100 ' END,
		-- CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from > 0 OR param_score_to > 0)
			-- THEN CONCAT(' HAVING SUM(kpi_weight) < 100 AND SUM(IFNULL(kpi_score, 0) * (kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		-- CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from > 0 OR param_score_to > 0)
			-- THEN CONCAT(' HAVING SUM(IFNULL(kpi_score, 0) * (kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		-- CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from = 0 OR param_score_to = 0)
			-- THEN '' END
    -- ));
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT a.id
				FROM (SELECT * FROM trans_employee_score WHERE FIND_IN_SET(CONCAT(position_id, ''|'', org_group_id), ''', @subordinates ,''' ) ) a
               ', 
				CASE WHEN param_show_for_evaluation = 1
					THEN ' INNER JOIN 
						(
							SELECT trans_id
								, MIN(IFNULL(kpi_score,0)) `min_score` 
							FROM trans_employee_score_details 
							GROUP BY trans_id
						) b ON a.id = b.trans_id '
					ELSE ''
				END ,'
				INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
                INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
                ', where_clause, '
			) x
		) `total_num`
		, a.id as id
		, a.trans_summary_id
		, CASE WHEN LENGTH(d.description) > 50 
				THEN CONCAT(SUBSTRING(d.description, 1, 50), ''...'') 
				ELSE IFNULL(d.description,'''') 
			END `description`
		, CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename, IFNULL(CONCAT('' '', a.suffix),''''), '' ('', a.employee_code, '') '') as employee
        , CONCAT(a.area, '' - '', a.region_department) as parent_org_group
        , CONCAT(a.org_group_code, '' - '',a.org_group_description) as org_group
	    , CONCAT(a.position_code, '' - '',a.position_title) as position
        , a.score
	    , DATE_FORMAT(d.tdate_from, ''%m/%d/%Y'') as tdate_from
        , DATE_FORMAT(d.tdate_to, ''%m/%d/%Y'') as tdate_to
        , DATE_FORMAT(a.pdate_from, ''%m/%d/%Y'') as pdate_from
		, DATE_FORMAT(a.pdate_to, ''%m/%d/%Y'') as pdate_to
        , a.status AS status
        , CASE WHEN d.is_trans_active = 1 THEN ''YES'' ELSE ''NO'' END `is_active`
		from (SELECT * FROM trans_employee_score WHERE FIND_IN_SET(CONCAT(position_id, ''|'', org_group_id), ''', @subordinates ,''' ) )  a
        ', 
        CASE WHEN param_show_for_evaluation = 1
			THEN ' INNER JOIN 
				(
					SELECT trans_id
						, MIN(IFNULL(kpi_score,0)) `min_score` 
					FROM trans_employee_score_details 
					GROUP BY trans_id
				) b ON a.id = b.trans_id '
			ELSE ''
		END ,'
		INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
        INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
        '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_list`(
	IN param_trans_summary_id_delimited VARCHAR(500),
    IN param_description VARCHAR(500),
    IN param_is_active_delimited VARCHAR(500),
    IN param_name_delimited VARCHAR(255),
    IN param_parent_org_group VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_date_from_from VARCHAR(10),
    IN param_date_from_to VARCHAR(10),
    IN param_date_to_from VARCHAR(10),
    IN param_date_to_to VARCHAR(10),
	IN param_date_effective_from_from VARCHAR(10),
    IN param_date_effective_from_to VARCHAR(10),
    IN param_date_effective_to_from VARCHAR(10),
    IN param_date_effective_to_to VARCHAR(10),
    IN param_score_from DECIMAL(8, 5),
    IN param_score_to DECIMAL(8, 5),
    IN param_status_delimited VARCHAR(255),
    IN param_show_for_evaluation BOOLEAN,
    IN param_show_no_score BOOLEAN,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    -- DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_trans_summary_id_delimited VARCHAR(1000);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_is_active_delimited VARCHAR(1000);
    DECLARE local_param_name_delimited VARCHAR(1000);
    DECLARE local_param_parent_org_group VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_date_from_from VARCHAR(200);
    DECLARE local_param_date_from_to VARCHAR(200);
    DECLARE local_param_date_to_from VARCHAR(200);
    DECLARE local_param_date_to_to VARCHAR(200);
	DECLARE local_param_date_effective_from_from VARCHAR(200);
    DECLARE local_param_date_effective_from_to VARCHAR(200);
    DECLARE local_param_date_effective_to_from VARCHAR(200);
    DECLARE local_param_date_effective_to_to VARCHAR(200);
    DECLARE local_param_score_from VARCHAR(20);
    DECLARE local_param_score_to VARCHAR(20);
    DECLARE local_param_status_delimited VARCHAR(255);
    DECLARE local_param_show_for_evaluation VARCHAR(50);
    DECLARE local_param_show_no_score VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_trans_summary_id_delimited = CASE WHEN param_trans_summary_id_delimited = '' THEN 'IFNULL(a.trans_summary_id, 0)' ELSE param_trans_summary_id_delimited END;
    SET local_param_id = '0'; -- CONVERT(param_id, CHAR(20));
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_is_active_delimited = CASE WHEN param_is_active_delimited = '' THEN '' ELSE param_is_active_delimited END;
    SET local_param_name_delimited = CASE WHEN param_name_delimited = '' THEN 'IFNULL(a.employee_id, 0)' ELSE param_name_delimited END;
    SET local_param_parent_org_group = CASE WHEN param_parent_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_parent_org_group, '\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'IFNULL(a.org_group_id, 0)' ELSE param_org_group_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(a.position_id, 0)' ELSE param_position_delimited END;
    SET local_param_score_from = CASE WHEN param_score_from < 0 THEN 'a.score' ELSE CONVERT(param_score_from, CHAR(20)) END;
    SET local_param_score_to = CASE WHEN param_score_to < 0 THEN 'a.score' ELSE CONVERT(param_score_to, CHAR(20)) END;
    
    SET local_param_date_from_from = 
	CASE WHEN STR_TO_DATE(param_date_from_from, '%m/%d/%Y') IS NULL OR param_date_from_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_from_to = 
	CASE WHEN STR_TO_DATE(param_date_from_to, '%m/%d/%Y') IS NULL OR param_date_from_to = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_from = 
	CASE WHEN STR_TO_DATE(param_date_to_from, '%m/%d/%Y') IS NULL OR param_date_to_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_to = 
	CASE WHEN STR_TO_DATE(param_date_to_to, '%m/%d/%Y') IS NULL OR param_date_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(d.tdate_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_param_date_effective_from_from = 
	CASE WHEN STR_TO_DATE(param_date_effective_from_from, '%m/%d/%Y') IS NULL OR param_date_effective_from_from = ''
		THEN 'IFNULL(CONVERT(em.date_effective_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_effective_from_to = 
	CASE WHEN STR_TO_DATE(param_date_effective_from_to, '%m/%d/%Y') IS NULL OR param_date_effective_from_to = ''
		THEN 'IFNULL(CONVERT(em.date_effective_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_effective_to_from = 
	CASE WHEN STR_TO_DATE(param_date_effective_to_from, '%m/%d/%Y') IS NULL OR param_date_effective_to_from = ''
		THEN 'IFNULL(CONVERT(em.date_effective_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_effective_to_to = 
	CASE WHEN STR_TO_DATE(param_date_effective_to_to, '%m/%d/%Y') IS NULL OR param_date_effective_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(em.date_effective_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    -- SET local_param_score_from = CONVERT(param_score_from, CHAR(20));
--     SET local_param_score_to = CONVERT(param_score_to, CHAR(20));

    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'c.id' ELSE param_status_delimited END;
	
    SET local_param_show_for_evaluation = CASE WHEN param_show_for_evaluation = 1 THEN 'true' ELSE 'false' END;
    SET local_param_show_no_score = CASE WHEN param_show_no_score = 1 THEN 'true' ELSE 'false' END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' a.is_active = 1'
     ,' AND'
     ,' ('
		,' IFNULL(a.trans_summary_id, 0) IN (', local_param_trans_summary_id_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
     ,' )'
     ,' AND'
     ,' ('
		 , local_param_description , '= '''''
		 , ' OR LOWER(d.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
     ,' )' 
       ,' AND'
    ,' ('
        ,''''' = ''', local_param_is_active_delimited, ''''
		,' OR '
		,' FIND_IN_SET((CASE WHEN d.is_trans_active = 1 THEN ''YES'' ELSE ''NO'' END),''', local_param_is_active_delimited, ''')'
    ,' )'
     ,' AND'
     ,' ('
		 ,' IFNULL(a.employee_id, 0) IN (', local_param_name_delimited, ')'
     ,' )' 
     ,' AND'
    ,' ('
		, local_param_parent_org_group , '= '''''
		, ' OR LOWER(CONCAT(a.area, '' - '', a.region_department))
			LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_parent_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
      ' ('
		 ,' IFNULL(a.org_group_id, 0) IN (', local_param_org_group_delimited, ')'
     ,' )'
     ,' AND'
      ' ('
	 	,' IFNULL(a.position_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'     
     ,' AND'
     ,' ('
		,' IFNULL(CONVERT(d.tdate_from, DATE), '''') BETWEEN ', local_param_date_from_from, ' AND ', local_param_date_from_to
     ,' )'
     ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(d.tdate_to, DATE), '''') BETWEEN ', local_param_date_to_from, ' AND ', local_param_date_to_to
     ,' )'
     ,' AND'
    ,' ('
		,' IFNULL(CONVERT(em.date_effective_from, DATE), '''') BETWEEN ', local_param_date_effective_from_from, ' AND ', local_param_date_effective_from_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(em.date_effective_to, DATE), '''') BETWEEN ', local_param_date_effective_to_from, ' AND ', local_param_date_effective_to_to
    ,' )'
     ,' AND'
     ,' ('
		,' c.id IN (', local_param_status_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
			,' a.score BETWEEN ', local_param_score_from, ' AND ', local_param_score_to
     ,' )'
    ,' AND'
     ,' ('
		,'	!',local_param_show_no_score
		,' 	OR '
		,'	('
			,local_param_show_no_score, ' AND score <= 0'
		,'	)'
     ,' )'
     ,
     CASE WHEN param_show_for_evaluation = 1
		 THEN
			CONCAT(' AND'
		 ,' ('
			,'	!',local_param_show_for_evaluation
			,' 	OR '
			,'	('
				,local_param_show_for_evaluation, ' AND b.min_score = 0 AND score > 0'
			,'	)'
		 ,' )')
		 ELSE ''
     END     
     );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ' ,  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.trans_summary_id DESC, a.id ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'TransSummaryID'		THEN CONCAT('a.trans_summary_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'		THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'IsActive'		THEN CONCAT('CASE WHEN d.is_trans_active = 1 THEN ''YES'' ELSE ''NO'' END ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Employee'	THEN CONCAT('LOWER(CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename, IFNULL(CONCAT('' '', a.suffix),''''), '' ('', a.employee_code, '') '')) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'ParentOrgGroup' THEN CONCAT('LOWER(CONCAT(a.area, '' - '', a.region_department)) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OrgGroup'	THEN CONCAT('LOWER(CONCAT(a.org_group_code, '' - '',a.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('LOWER(CONCAT(a.position_code, '' - '',a.position_title)) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'TDateFrom'	THEN CONCAT('d.tdate_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'TDateTo'	THEN CONCAT('IFNULL(d.tdate_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'DateEffectiveFrom'	    THEN CONCAT('em.date_effective_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateEffectiveTo'		THEN CONCAT('IFNULL(em.date_effective_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Score' 	THEN CONCAT('a.score ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Status'	THEN CONCAT('a.status ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    -- Set Group By & Having clause
    -- SET group_by_clause = ' GROUP BY a.t_id';
	-- SET having_clause = CONCAT(
--     COALESCE(
-- 		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from = 0 OR param_score_to = 0)
-- 			THEN ' HAVING SUM(a.kpi_weight) < 100 ' END,
-- 		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from > 0 OR param_score_to > 0)
-- 			THEN CONCAT(' HAVING SUM(a.kpi_weight) < 100 AND SUM(IFNULL(a.kpi_score, 0) * (a.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
-- 		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from > 0 OR param_score_to > 0)
-- 			THEN CONCAT(' HAVING SUM(IFNULL(a.kpi_score, 0) * (a.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
-- 		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from = 0 OR param_score_to = 0)
-- 			THEN '' END
--     ));
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT a.id
				FROM trans_employee_score a
                ', 
				CASE WHEN param_show_for_evaluation = 1
					THEN ' INNER JOIN 
						(
							SELECT trans_id
								, MIN(IFNULL(kpi_score,0)) `min_score` 
							FROM trans_employee_score_details 
							GROUP BY trans_id
						) b ON a.id = b.trans_id '
					ELSE ''
				END ,'
				INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
                INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
                INNER JOIN employee_movement em ON a.trans_summary_id = em.id
                ', where_clause, '
			) x
		) `total_num`
		, a.id AS id
		, a.trans_summary_id
        , CASE WHEN LENGTH(d.description) > 50 
						THEN CONCAT(SUBSTRING(d.description, 1, 50), ''...'') 
						ELSE IFNULL(d.description,'''') 
					END `description`
		, CASE WHEN d.is_trans_active = 1 THEN ''YES'' ELSE ''NO'' END `is_active`
		, CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename, IFNULL(CONCAT('' '', a.suffix),''''), '' ('', a.employee_code, '') '') AS employee
        , CONCAT(a.area, '' - '', a.region_department) AS parent_org_group
        , CONCAT(a.org_group_code, '' - '',a.org_group_description) AS org_group
	    , CONCAT(a.position_code, '' - '',a.position_title) AS position
        , a.score
	    , DATE_FORMAT(d.tdate_from, ''%m/%d/%Y'') AS tdate_from
        , DATE_FORMAT(d.tdate_to, ''%m/%d/%Y'') AS tdate_to
        , DATE_FORMAT(a.pdate_from, ''%m/%d/%Y'') AS pdate_from
		, DATE_FORMAT(a.pdate_to, ''%m/%d/%Y'') AS pdate_to
		, IFNULL(DATE_FORMAT(em.date_effective_from, ''%m/%d/%Y''), '''') `date_effective_from`
		, IFNULL(DATE_FORMAT(em.date_effective_to, ''%m/%d/%Y''), ''CURRENT'') `date_effective_to`
        , a.status AS status
		from trans_employee_score a
        ', 
        CASE WHEN param_show_for_evaluation = 1
			THEN ' INNER JOIN 
				(
					SELECT trans_id
						, MIN(IFNULL(kpi_score,0)) `min_score` 
					FROM trans_employee_score_details 
					GROUP BY trans_id
				) b ON a.id = b.trans_id '
			ELSE ''
		END ,'
		INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
        INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
        INNER JOIN employee_movement em ON a.trans_summary_id = em.id
        '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_list_by_or_group_approval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_list_by_or_group_approval`(
	IN param_id INT,
    IN param_name VARCHAR(255),
    IN param_parent_org_group VARCHAR(500),
    IN param_org_group_delimited VARCHAR(16383),
    IN param_position_delimited VARCHAR(16383),
    IN param_date_from_from VARCHAR(10),
    IN param_date_from_to VARCHAR(10),
    IN param_date_to_from VARCHAR(10),
    IN param_date_to_to VARCHAR(10),
    IN param_score_from DECIMAL(8, 5),
    IN param_score_to DECIMAL(8, 5),
    IN param_status VARCHAR(255),
    IN param_show_missing_scores BOOLEAN,
    IN param_is_export BOOLEAN,
    IN param_user_org_group_delimited VARCHAR(16383),
    IN param_user_org_group_descendants_delimited TEXT,
    IN param_user_id INT,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    -- DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_name VARCHAR(1000);
    DECLARE local_param_parent_org_group VARCHAR(16383);
    DECLARE local_param_org_group_delimited VARCHAR(16383);
    DECLARE local_param_position_delimited VARCHAR(16383);
    DECLARE local_param_user_org_group_delimited VARCHAR(16383);
    DECLARE local_param_date_from_from VARCHAR(200);
    DECLARE local_param_date_from_to VARCHAR(200);
    DECLARE local_param_date_to_from VARCHAR(200);
    DECLARE local_param_date_to_to VARCHAR(200);
    DECLARE local_param_score_from VARCHAR(20);
    DECLARE local_param_score_to VARCHAR(20);
    DECLARE local_param_status VARCHAR(255);
    DECLARE local_param_show_missing_scores VARCHAR(50);
    DECLARE local_param_user_org_group_id VARCHAR(20);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_name = CASE WHEN param_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_parent_org_group = CASE WHEN param_parent_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_parent_org_group, '\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'IFNULL(A.org_group_id, 0)' ELSE param_org_group_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(A.position_id, 0)' ELSE param_position_delimited END;
    SET local_param_user_org_group_delimited = CASE WHEN param_user_org_group_delimited = '' THEN '0' ELSE param_user_org_group_delimited END;
    SET local_param_score_from = CASE WHEN param_score_from < 0 THEN 'A.score' ELSE CONVERT(param_score_from, CHAR(20)) END;
    SET local_param_score_to = CASE WHEN param_score_to < 0 THEN 'A.score' ELSE CONVERT(param_score_to, CHAR(20)) END;
    
    SET local_param_date_from_from = 
	CASE WHEN STR_TO_DATE(param_date_from_from, '%m/%d/%Y') IS NULL OR param_date_from_from = ''
		THEN 'IFNULL(CONVERT(A.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_from_to = 
	CASE WHEN STR_TO_DATE(param_date_from_to, '%m/%d/%Y') IS NULL OR param_date_from_to = ''
		THEN 'IFNULL(CONVERT(A.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_from = 
	CASE WHEN STR_TO_DATE(param_date_to_from, '%m/%d/%Y') IS NULL OR param_date_to_from = ''
		THEN 'IFNULL(CONVERT(A.tdate_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_to = 
	CASE WHEN STR_TO_DATE(param_date_to_to, '%m/%d/%Y') IS NULL OR param_date_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(A.tdate_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    -- SET local_param_score_from = CONVERT(param_score_from, CHAR(20));
--     SET local_param_score_to = CONVERT(param_score_to, CHAR(20));

    SET local_param_status = CASE WHEN param_status = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_status,'\\','\\\\'), '''', ''''''), '''') END;
    
    SET local_param_show_missing_scores = 
		CASE WHEN param_show_missing_scores = 1 THEN 'true'
        ELSE 'false'
        END;
        
	SET local_param_user_org_group_id = (SELECT org_group_id FROM employee WHERE system_user_id = param_user_id);

    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR A.id = ', local_param_id
     ,' )'
     ,' AND'
     ,' ('
		 , local_param_name , '= '''''
		 , ' OR LOWER(CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '')) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_name , '), '' '', ''%''), ''%'')'
     ,' )' 
     ,' AND'
    ,' ('
		, local_param_parent_org_group , '= '''''
		, ' OR LOWER(CONCAT(A.area, '' - '', A.region_department))
			LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_parent_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
      ' ('
		 ,' IFNULL(A.org_group_id, 0) IN (', local_param_org_group_delimited, ')'
     ,' )'
     ,' AND'
      ' ('
	 	,' IFNULL(A.position_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'     
     ,' AND'
     ,' ('
		,' IFNULL(CONVERT(A.tdate_from, DATE), '''') BETWEEN ', local_param_date_from_from, ' AND ', local_param_date_from_to
     ,' )'
     ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(A.tdate_to, DATE), '''') BETWEEN ', local_param_date_to_from, ' AND ', local_param_date_to_to
     ,' )'
     ,' AND'
     ,' ('
		, local_param_status , '= '''''
		, ' OR A.status LIKE CONCAT(''%'', REPLACE(' , local_param_status , ', '' '', ''%''), ''%'')'
     ,' )'
     ,' AND'
     ,' ('
			,' A.score BETWEEN ', local_param_score_from, ' AND ', local_param_score_to
     ,' )'
     ,' AND'
     ,' ('
		,'	!',local_param_show_missing_scores
		,' 	OR '
		,'	('
		,local_param_show_missing_scores, ' AND A.score <= 0'
		,'	)'
     ,' )'
     ,' AND'
		, ' FIND_IN_SET(A.org_group_id, (''' , param_user_org_group_descendants_delimited , '''))'
	 ,' AND' 
		, ' A.org_group_id != ', local_param_user_org_group_delimited
	 ,' AND' 
		, ' A.requestor_id != ', param_user_id
	 ,' AND'  
	 ,' ( '
		, 'A.status IN (''FOR_APPROVAL'', ''APPROVED'', ''FINALIZED'', ''FOR_REVISION'')'
	 ,' ) '
     );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    CASE WHEN sidx = '' THEN '' ELSE ' ORDER BY ' END,  
	IFNULL(CASE WHEN sidx = ''			THEN '' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'		THEN CONCAT('A.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Employee'	THEN CONCAT('LOWER(CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '')) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'ParentOrgGroup' THEN CONCAT('LOWER(CONCAT(A.area, '' - '', A.region_department)) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OrgGroup'	THEN CONCAT('LOWER(CONCAT(A.org_group_code, '' - '',A.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('LOWER(CONCAT(A.position_code, '' - '',A.position_title)) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'TDateFrom'	THEN CONCAT('A.tdate_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'TDateTo'	THEN CONCAT('IFNULL(A.tdate_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Score' 	THEN CONCAT('A.score ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Status'	THEN CONCAT('A.status ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    -- Set Group By & Having clause
    -- SET group_by_clause = ' GROUP BY A.t_id';
	SET having_clause = CONCAT(
    COALESCE(
		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from = 0 OR param_score_to = 0)
			THEN ' HAVING SUM(A.kpi_weight) < 100 ' END,
		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from > 0 OR param_score_to > 0)
			THEN CONCAT(' HAVING SUM(A.kpi_weight) < 100 AND SUM(IFNULL(A.kpi_score, 0) * (A.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from > 0 OR param_score_to > 0)
			THEN CONCAT(' HAVING SUM(IFNULL(A.kpi_score, 0) * (A.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from = 0 OR param_score_to = 0)
			THEN '' END
    ));
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT A.id
				FROM trans_employee_score A
                ', where_clause, '
			) x
		) `total_num`
		, A.id AS id
		, CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '') AS employee
        , CONCAT(A.area, '' - '', A.region_department) AS parent_org_group
        , CONCAT(A.org_group_code, '' - '',A.org_group_description) AS org_group
	    , CONCAT(A.position_code, '' - '',A.position_title) AS position
        , A.score
	    , DATE_FORMAT(A.tdate_from, ''%m/%d/%Y'') AS tdate_from
        , DATE_FORMAT(A.tdate_to, ''%m/%d/%Y'') AS tdate_to
        , DATE_FORMAT(A.pdate_from, ''%m/%d/%Y'') AS pdate_from
		, DATE_FORMAT(A.pdate_to, ''%m/%d/%Y'') AS pdate_to
        , A.status AS status
		from trans_employee_score A
        '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_get_list_by_or_group_keyin` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_get_list_by_or_group_keyin`(
	IN param_id INT,
    IN param_name VARCHAR(255),
    IN param_parent_org_group VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_date_from_from VARCHAR(10),
    IN param_date_from_to VARCHAR(10),
    IN param_date_to_from VARCHAR(10),
    IN param_date_to_to VARCHAR(10),
    IN param_score_from DECIMAL(8, 5),
    IN param_score_to DECIMAL(8, 5),
    IN param_status VARCHAR(255),
    IN param_show_missing_scores BOOLEAN,
    IN param_is_export BOOLEAN,
    IN param_user_org_group_delimited VARCHAR(500),
    IN param_user_org_group_descendants_delimited TEXT,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    -- DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_name VARCHAR(1000);
    DECLARE local_param_parent_org_group VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_date_from_from VARCHAR(200);
    DECLARE local_param_date_from_to VARCHAR(200);
    DECLARE local_param_date_to_from VARCHAR(200);
    DECLARE local_param_date_to_to VARCHAR(200);
    DECLARE local_param_score_from VARCHAR(20);
    DECLARE local_param_score_to VARCHAR(20);
    DECLARE local_param_status VARCHAR(255);
    DECLARE local_param_show_missing_scores VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_name = CASE WHEN param_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_parent_org_group = CASE WHEN param_parent_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_parent_org_group, '\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'IFNULL(A.org_group_id, 0)' ELSE param_org_group_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(A.position_id, 0)' ELSE param_position_delimited END;
    SET local_param_score_from = CASE WHEN param_score_from < 0 THEN 'A.score' ELSE CONVERT(param_score_from, CHAR(20)) END;
    SET local_param_score_to = CASE WHEN param_score_to < 0 THEN 'A.score' ELSE CONVERT(param_score_to, CHAR(20)) END;
    
    SET local_param_date_from_from = 
	CASE WHEN STR_TO_DATE(param_date_from_from, '%m/%d/%Y') IS NULL OR param_date_from_from = ''
		THEN 'IFNULL(CONVERT(A.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_from_to = 
	CASE WHEN STR_TO_DATE(param_date_from_to, '%m/%d/%Y') IS NULL OR param_date_from_to = ''
		THEN 'IFNULL(CONVERT(A.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_from = 
	CASE WHEN STR_TO_DATE(param_date_to_from, '%m/%d/%Y') IS NULL OR param_date_to_from = ''
		THEN 'IFNULL(CONVERT(A.tdate_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_to = 
	CASE WHEN STR_TO_DATE(param_date_to_to, '%m/%d/%Y') IS NULL OR param_date_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(A.tdate_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    -- SET local_param_score_from = CONVERT(param_score_from, CHAR(20));
--     SET local_param_score_to = CONVERT(param_score_to, CHAR(20));

    SET local_param_status = CASE WHEN param_status = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_status,'\\','\\\\'), '''', ''''''), '''') END;
    
    SET local_param_show_missing_scores = 
		CASE WHEN param_show_missing_scores = 1 THEN 'true'
        ELSE 'false'
        END;
        
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR A.id = ', local_param_id
     ,' )'
     ,' AND'
     ,' ('
		 , local_param_name , '= '''''
		 , ' OR LOWER(CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '')) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_name , '), '' '', ''%''), ''%'')'
     ,' )' 
     ,' AND'
    ,' ('
		, local_param_parent_org_group , '= '''''
		, ' OR LOWER(CONCAT(A.area, '' - '', A.region_department))
			LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_parent_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
      ' ('
		 ,' IFNULL(A.org_group_id, 0) IN (', local_param_org_group_delimited, ')'
     ,' )'
     ,' AND'
      ' ('
	 	,' IFNULL(A.position_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'     
     ,' AND'
     ,' ('
		,' IFNULL(CONVERT(A.tdate_from, DATE), '''') BETWEEN ', local_param_date_from_from, ' AND ', local_param_date_from_to
     ,' )'
     ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(A.tdate_to, DATE), '''') BETWEEN ', local_param_date_to_from, ' AND ', local_param_date_to_to
     ,' )'
     ,' AND'
     ,' ('
		, local_param_status , '= '''''
		, ' OR A.status LIKE CONCAT(''%'', REPLACE(' , local_param_status , ', '' '', ''%''), ''%'')'
     ,' )'
     ,' AND'
     ,' ('
			,' A.score BETWEEN ', local_param_score_from, ' AND ', local_param_score_to
     ,' )'
     ,' AND'
     ,' ('
		,'	!',local_param_show_missing_scores
		,' 	OR '
		,'	('
		,local_param_show_missing_scores, ' AND A.score <= 0'
		,'	)'
     ,' )'
	 ,' AND'
		, ' FIND_IN_SET(A.org_group_id, (''' , param_user_org_group_descendants_delimited , '''))'
	 ,' AND' 
		, ' A.org_group_id != ', param_user_org_group_delimited
     );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    CASE WHEN sidx = '' THEN '' ELSE ' ORDER BY ' END,  
	IFNULL(CASE WHEN sidx = ''			THEN '' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'		THEN CONCAT('A.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Employee'	THEN CONCAT('LOWER(CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '')) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'ParentOrgGroup' THEN CONCAT('LOWER(CONCAT(A.area, '' - '', A.region_department)) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OrgGroup'	THEN CONCAT('LOWER(CONCAT(A.org_group_code, '' - '',A.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('LOWER(CONCAT(A.position_code, '' - '',A.position_title)) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'TDateFrom'	THEN CONCAT('A.tdate_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'TDateTo'	THEN CONCAT('IFNULL(A.tdate_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Score' 	THEN CONCAT('A.score ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Status'	THEN CONCAT('A.status ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    -- Set Group By & Having clause
    -- SET group_by_clause = ' GROUP BY A.t_id';
	SET having_clause = CONCAT(
    COALESCE(
		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from = 0 OR param_score_to = 0)
			THEN ' HAVING SUM(A.kpi_weight) < 100 ' END,
		CASE WHEN local_param_show_missing_scores = 'true' AND (param_score_from > 0 OR param_score_to > 0)
			THEN CONCAT(' HAVING SUM(A.kpi_weight) < 100 AND SUM(IFNULL(A.kpi_score, 0) * (A.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from > 0 OR param_score_to > 0)
			THEN CONCAT(' HAVING SUM(IFNULL(A.kpi_score, 0) * (A.kpi_weight * .01)) BETWEEN ', param_score_from, ' AND ', param_score_to) END,
		CASE WHEN local_param_show_missing_scores = 'false' AND (param_score_from = 0 OR param_score_to = 0)
			THEN '' END
    ));
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT A.id
				FROM trans_employee_score A
                ', where_clause, '
			) x
		) `total_num`
		, A.id AS id
		, CONCAT(A.lastname, '', '', A.firstname, '' '', A.middlename, IFNULL(CONCAT('' '', A.suffix),''''), '' ('', A.employee_code, '') '') AS employee
        , CONCAT(A.area, '' - '', A.region_department) AS parent_org_group
        , CONCAT(A.org_group_code, '' - '',A.org_group_description) AS org_group
	    , CONCAT(A.position_code, '' - '',A.position_title) AS position
        , A.score
	    , DATE_FORMAT(A.tdate_from, ''%m/%d/%Y'') AS tdate_from
        , DATE_FORMAT(A.tdate_to, ''%m/%d/%Y'') AS tdate_to
        , DATE_FORMAT(A.pdate_from, ''%m/%d/%Y'') AS pdate_from
		, DATE_FORMAT(A.pdate_to, ''%m/%d/%Y'') AS pdate_to
        , A.status AS status
		from trans_employee_score A
        '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_error_log_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_error_log_get_list`(
	IN param_id INT,
	IN param_class varchar(500),
    IN param_method VARCHAR(16383),
    IN param_error_message VARCHAR(16383),
    IN param_user_id_delimited VARCHAR(16383),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_id VARCHAR(20);
	DECLARE local_param_class VARCHAR(16383);
	DECLARE local_param_method VARCHAR(16383);
	DECLARE local_param_error_message VARCHAR(16383);
	DECLARE local_param_user_id_delimited VARCHAR(16383);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_class = CASE WHEN param_class = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_class,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_method = CASE WHEN param_method = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_method,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_error_message = CASE WHEN param_error_message = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_error_message,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_user_id_delimited = CASE WHEN param_user_id_delimited = '' THEN 'IFNULL(a.user_id, 0)' ELSE param_user_id_delimited END;
    
    SET local_param_date_created_from = 
	CASE WHEN STR_TO_DATE(param_date_created_from, '%m/%d/%Y') IS NULL OR param_date_created_from = ''
		THEN 'IFNULL(CONVERT(a.created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_from, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_param_date_created_to = 
	CASE WHEN STR_TO_DATE(param_date_created_to, '%m/%d/%Y') IS NULL OR param_date_created_to = ''
		THEN 'IFNULL(CONVERT(a.created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_to, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
     ,' )'
    ,' AND' 
    ,' ('
		, local_param_class , '= '''''
		, ' OR LOWER(class) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_class , '), '' '', ''%''), ''%'')'
    ,' )'
  --   ,' AND'
--     ,' ('
-- 		, local_param_method , '= '''''
-- 		, ' OR LOWER(method) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_method , '), '' '', ''%''), ''%'')'
--     ,' )'
    ,' AND'
    ,' ('
		, local_param_error_message , '= '''''
		, ' OR LOWER(error_message) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_error_message , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
      ' ('
	 	,' IFNULL(a.user_id, 0) IN (', local_param_user_id_delimited, ')'
     ,' )'     
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('a.id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Class'			THEN CONCAT('class ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Method'		THEN CONCAT('method ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'ErrorMessage'			THEN CONCAT('error_message ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'CreatedDate'	THEN CONCAT('created_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_error_log; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_error_log
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM error_log a ', where_clause, ') x) `total_num`
		, a.id
        , class
        , CASE WHEN LENGTH(method) > 50 
			THEN CONCAT(SUBSTRING(method, 1, 50), ''...'') 
			ELSE method 
		END `method`
        , layer
        , inner_exception
        , CASE WHEN LENGTH(error_message) > 50 
			THEN CONCAT(SUBSTRING(error_message, 1, 50), ''...'') 
			ELSE error_message 
		END `error_message`
		, IFNULL(DATE_FORMAT(a.created_date, ''%m/%d/%Y %r''), '''') `created_date`
        , '''' `user`
        , a.user_id
	FROM error_log a
    ', where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_error_log) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_error_log; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_filtered_employee_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_filtered_employee_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT,
    IN filter VARCHAR(50),
    IN filter_id VARCHAR(200)
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 

	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    DECLARE local_filter VARCHAR(100);
    DECLARE local_filter_id VARCHAR(16383);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    SET local_filter = CASE WHEN filter = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(filter,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_filter_id = CASE WHEN filter_id = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(filter_id,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    
    SET local_filter_id = 
    CASE WHEN filter != 'POS'
			 THEN 
				CASE WHEN filter_id = '' OR filter_id = 'All'
					THEN local_filter_id 
                    ELSE CONCAT('''',(SELECT fn_org_group_get_descendants(filter_id)),'''')
				END
             ELSE local_filter_id
		END;
    
    SET where_clause =
		CASE WHEN filter = 'POS'
			 THEN CONCAT(' AND find_in_set(position_id, ', local_filter_id, ')')
             WHEN filter != ''
             THEN CONCAT(' AND find_in_set(org_group_id, ', local_filter_id, ')')
             ELSE ''
		END;
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
        , sync_id
        , sync_date
        , code
        , lastname
        , firstname
        , middlename
		, INSTR(CONCAT(lastname, '', '', firstname, '' '', middlename, '' '', '' ('', code, '')''), ', local_term, ') `index`
        , is_active
        , org_group_id
        , position_id
        , created_date
        , gender
        , nickname
        , old_employee_id
        -- , psgc_city_id
--         , psgc_region_id
        , suffix
        , system_user_id
        , date_hired
        , employment_status
	FROM employee
	WHERE LOWER(CONCAT(lastname, '', '', firstname, '' '', middlename, '' '', '' ('', code, '')'')) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1 ',
    where_clause,
	' ORDER BY `index`
		, CONCAT(lastname, '', '', firstname, '' '', middlename, '' '', '' ('', code, '')'') ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_filtered_org_group_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_filtered_org_group_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT,
    IN filter VARCHAR(50)
)
BEGIN
	-- Define variables for query portions 
	DECLARE where_clause VARCHAR(16383); 

	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    DECLARE local_filter VARCHAR(100);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    SET local_filter = CASE WHEN filter = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(filter,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    
    SET where_clause =
		CASE WHEN filter != ''
			 THEN CONCAT(' AND org_type = ', local_filter)
             ELSE ''
		END;
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
        , sync_id
        , sync_date
        , code
		, INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
        , description
        , parent_org_id
        , is_active
        , org_type
	FROM org_group
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1 ',
    where_clause,
	' ORDER BY `index`
		, CONCAT(code, '' - '', description) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    

     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_average_scores` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_average_scores`(
	IN param_org INT,
	IN param_kpi VARCHAR(16383)
)
BEGIN
	SELECT param_org `org_group_id`, kpi_id, AVG(rate) `average`
	FROM kpi_score
	WHERE find_in_set(org_group_id, (select fn_org_group_get_descendants(CONVERT(param_org, CHAR(50)))))
    AND org_group_id != param_org
	AND find_in_set(kpi_id, param_kpi)
	GROUP BY kpi_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_autocomplete`(
	IN term VARCHAR(255),
    IN top_Results INT
)
BEGIN

	-- Define parameters for conversion to string
    DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
        , INSTR(CONCAT(code, '' - '', name), ', local_term, ') `index`
        , code 
        , name
        , description
        , guidelines
        , kra_group
        , kra_sub_group
        , modified_by
        , modified_date
        , is_active
        , old_kpi_code
        , kpi_type
        , source_type
	FROM kpi
	WHERE LOWER(CONCAT(code, '' - '', name)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
	ORDER BY `index`
		, CONCAT(code, '' - '', name) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_details` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_details`(
)
BEGIN
	SELECT 		
		a.id `id`
		, b.name `kra_group`
        , IFNULL(c.name, '') `kra_sub_group`
        , a.code `kpi_code`
        , a.name `kpi_name`
        , a.description `kpi_description`
        , a.old_kpi_code
    FROM kpi a
    INNER JOIN kra_group b ON a.kra_group = b.id
    LEFT JOIN kra_sub_group c ON a.kra_sub_group = c.id
    WHERE a.is_active = 1
    ORDER BY a.id ASC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_get_list`(
	IN param_id INT,
    IN param_code varchar(50),
    IN param_kra_type_delimited varchar(255),
    IN param_kra_group varchar(255),
    IN param_kra_sub_group varchar(255),
    IN param_name varchar(255),
    IN param_old_code varchar(50),
    IN param_kpi_type_delimited varchar(255),
    IN param_source_type_delimited varchar(255),
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_code VARCHAR(50);
    DECLARE local_param_kra_type_delimited VARCHAR(255);
    DECLARE local_param_kra_group VARCHAR(255);
    DECLARE local_param_kra_sub_group VARCHAR(255);
    DECLARE local_param_name VARCHAR(255);
    DECLARE local_param_old_code VARCHAR(50);
    DECLARE local_param_kpi_type_delimited VARCHAR(255);
    DECLARE local_param_source_type_delimited VARCHAR(255);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_code = CASE WHEN param_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_code,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kra_type_delimited = CASE WHEN param_kra_type_delimited = '' THEN 'IFNULL(D.id, 0)' ELSE param_kra_type_delimited END;
    SET local_param_kra_group = CASE WHEN param_kra_group = '' THEN '''''' ELSE UPPER(CONCAT('''', REPLACE(REPLACE(param_kra_group,'\\','\\\\'), '''', ''''''), '''')) END;
    SET local_param_kra_sub_group = CASE WHEN param_kra_sub_group = '' THEN '''''' ELSE UPPER(CONCAT('''', REPLACE(REPLACE(param_kra_sub_group,'\\','\\\\'), '''', ''''''), '''')) END;
    SET local_param_name = CASE WHEN param_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_old_code = CASE WHEN param_old_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_old_code,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_kpi_type_delimited = CASE WHEN param_kpi_type_delimited = '' THEN 'IFNULL(E.id, 0)' ELSE param_kpi_type_delimited END;
    SET local_param_source_type_delimited = CASE WHEN param_source_type_delimited = '' THEN 'IFNULL(F.id, 0)' ELSE param_source_type_delimited END;
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
	,' is_active = 1'
    ,' AND'
    ,' ('
		, local_param_id , '= 0'
		, ' OR A.id = ', local_param_id
    ,' )'
    ,' AND'
    ,' ('
		, local_param_code , '= '''''
		, ' OR LOWER(A.code) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_code , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(D.id, 0) IN (', local_param_kra_type_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_kra_group , '= '''''
		, ' OR B.name LIKE CONCAT(''%'', REPLACE(' , local_param_kra_group , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_kra_sub_group , '= '''''
		, ' OR IFNULL(C.name, '''') LIKE CONCAT(''%'', REPLACE(' , local_param_kra_sub_group , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_old_code , '= '''''
		, ' OR LOWER(A.old_kpi_code) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_old_code , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_name , '= '''''
		, ' OR LOWER(A.name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(E.id, 0) IN (', local_param_kpi_type_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(F.id, 0) IN (', local_param_source_type_delimited, ')'
    ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'A.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('A.id ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'Code'			THEN CONCAT('A.code ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'KRAType'		THEN CONCAT('D.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'KRAGroup'		THEN CONCAT('B.name ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'KRASubGroup'	THEN CONCAT('IFNULL(C.name, '''') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Name' 			THEN CONCAT('A.name ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'OldKPICode'	THEN CONCAT('A.old_kpi_code ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_kpi; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_kpi
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 
								FROM kpi A 
								LEFT JOIN kra_group B ON A.kra_group = B.id
								LEFT JOIN kra_sub_group C ON A.kra_sub_group = C.id
								LEFT JOIN reference_value D ON B.type = D.value 
								LEFT JOIN reference_value E ON A.kpi_type = E.value
								LEFT JOIN reference_value F ON A.source_type = F.value
								', where_clause, ') x) `total_num`
		, A.id
        , D.description AS kra_type
        , B.name AS kra_group
        , IFNULL(C.name, '''') AS kra_sub_group
        , A.code
        , A.name
        , A.old_kpi_code
        , A.kpi_type
        , A.source_type
	FROM kpi A 
	LEFT JOIN kra_group B ON A.kra_group = B.id
	LEFT JOIN kra_sub_group C ON A.kra_sub_group = C.id
    LEFT JOIN reference_value D ON B.type = D.value
    LEFT JOIN reference_value E ON A.kpi_type = E.value
    LEFT JOIN reference_value F ON A.source_type = F.value '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_kpi) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_kpi; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_get_new_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_get_new_code`(
	IN param_add_counter INT
)
BEGIN
	DECLARE kpi_code_counter INT;
	SET kpi_code_counter = (SELECT value FROM reference_value WHERE ref_code = 'KPI_CODE_COUNTER');
	
    SET kpi_code_counter = kpi_code_counter + param_add_counter;
    
	UPDATE reference_value 
	SET value =  kpi_code_counter
	WHERE ref_code = 'KPI_CODE_COUNTER';
    
	SELECT 0 `id`, LPAD(kpi_code_counter, 3, '0') `new_kpi_code`;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_position_details` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_position_details`(
)
BEGIN
	SELECT 		
		a.id `id`
		, c.name `kra_group`
        , d.name `kra_sub_group`
        , b.code `kpi_code`
        , b.name `kpi_name`
        , b.description `kpi_description`
        , a.kpi_id `kpi_id`
        , CONCAT(b.code, ' - ' , b.name) `kpi`
        , a.weight `weight`
        , a.position_id `position_id`
	FROM kpi_position a
    INNER JOIN kpi b ON a.kpi_id = b.id
    INNER JOIN kra_group c ON b.kra_group = c.id
    INNER JOIN kra_sub_group d ON b.kra_sub_group = d.id
    AND YEAR(a.tdate) = YEAR(CURRENT_TIMESTAMP());
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_position_get_by_position` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_position_get_by_position`(
	IN param_position_id INT,
    IN param_tdate VARCHAR(20)
)
BEGIN
	SELECT 		
		a.id `id`
		, c.name `kra_group`
        , IFNULL(d.name, '') `kra_sub_group`
        , b.code `kpi_code`
        , b.name `kpi_name`
        , b.description `kpi_description`
        , a.kpi_id `kpi_id`
        , CONCAT(b.code, ' - ' , b.name) `kpi`
        , a.weight `weight`
        , a.position_id `position_id`
	FROM kpi_position a
    INNER JOIN kpi b ON a.kpi_id = b.id
    INNER JOIN kra_group c ON b.kra_group = c.id
    LEFT JOIN kra_sub_group d ON b.kra_sub_group = d.id
	WHERE a.position_id = param_position_id
    AND DATE(a.tdate) = IFNULL(STR_TO_DATE(param_tdate, '%m/%d/%Y'), '');
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_position_get_export_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_position_get_export_list`(
	IN param_id INT,
    IN param_position_delimited VARCHAR(500),
    IN param_date_effective_from VARCHAR(10),
    IN param_date_effective_to VARCHAR(10),
    IN param_show_recent_only BOOLEAN,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_date_effective_from VARCHAR(200);
    DECLARE local_param_date_effective_to VARCHAR(200);
    DECLARE local_param_show_recent_only VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(B.sync_id, 0)' ELSE param_position_delimited END;
    
    SET local_param_date_effective_from = 
	CASE WHEN STR_TO_DATE(param_date_effective_from, '%m/%d/%Y') IS NULL OR param_date_effective_from = ''
		THEN 'IFNULL(CONVERT(A.tdate, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_effective_to = 
	CASE WHEN STR_TO_DATE(param_date_effective_to, '%m/%d/%Y') IS NULL OR param_date_effective_to = ''
		THEN 'IFNULL(CONVERT(A.tdate, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_show_recent_only = 
		CASE WHEN param_show_recent_only 
			THEN CONVERT(param_show_recent_only, CHAR(20))
		ELSE 'A.is_latest'
        END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' (B.is_active = 1)'
     ,' AND'
     ,' ('
		, local_param_id , '= 0'
		, ' OR B.sync_id = ', local_param_id
     ,' )'
     ,' AND'
     ' ('
		,' IFNULL(B.sync_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
		 ,' IFNULL(CONVERT(A.tdate, DATE), '''') BETWEEN ', local_param_date_effective_from, ' AND ', local_param_date_effective_to
     ,' )'
     ,' AND'
     ,' ('
        ,' A.is_latest = ', local_param_show_recent_only
     ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'B.sync_id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('B.sync_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position' 		  	    THEN CONCAT('B.title ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'EffectiveDate' 		THEN CONCAT('A.tdate ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_kpi_position; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_kpi_position
    SELECT DISTINCT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT DISTINCT B.sync_id, 
					   IFNULL(DATE_FORMAT(A.tdate, ''%m/%d/%Y''), '''')
				FROM kpi_position A 
				INNER JOIN position B ON A.position_id = B.sync_id ', where_clause, '
			) x
		) `total_num`
		, B.sync_id AS id
        , CONCAT(B.code, '' - '', B.title) AS position
        , CONCAT(C.code, '' - '', C.name) AS kpi
        , A.weight AS weight
        , IFNULL(DATE_FORMAT(A.tdate, ''%m/%d/%Y''), '''') AS effective_date
	FROM kpi_position A 
    INNER JOIN position B ON A.position_id = B.sync_id
    INNER JOIN kpi C ON A.kpi_id = C.id '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_kpi_position) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_kpi_position; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_position_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_position_get_list`(
	IN param_id INT,
    IN param_position_delimited VARCHAR(500),
    IN param_weight DECIMAL(8, 5),
    IN param_date_effective_from VARCHAR(10),
    IN param_date_effective_to VARCHAR(10),
    IN param_show_recent_only BOOLEAN,
    IN param_is_export BOOLEAN,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_weight VARCHAR(20);
    DECLARE local_param_date_effective_from VARCHAR(200);
    DECLARE local_param_date_effective_to VARCHAR(200);
    DECLARE local_param_show_recent_only VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(B.sync_id, 0)' ELSE param_position_delimited END;
    SET local_param_weight = CONVERT(param_weight, CHAR(20));
    
    SET local_param_date_effective_from = 
	CASE WHEN STR_TO_DATE(param_date_effective_from, '%m/%d/%Y') IS NULL OR param_date_effective_from = ''
		THEN 'IFNULL(CONVERT(A.tdate, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_effective_to = 
	CASE WHEN STR_TO_DATE(param_date_effective_to, '%m/%d/%Y') IS NULL OR param_date_effective_to = ''
		THEN 'IFNULL(CONVERT(A.tdate, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_effective_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_show_recent_only = 
		CASE WHEN param_show_recent_only 
			THEN CONVERT(param_show_recent_only, CHAR(20))
		ELSE 'A.is_latest'
        END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' (B.is_active = 1)'
     ,' AND'
     ,' ('
		, local_param_id , '= 0'
		, ' OR B.sync_id = ', local_param_id
     ,' )'
     ,' AND'
     ' ('
		,' IFNULL(B.sync_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
		 ,' IFNULL(CONVERT(A.tdate, DATE), '''') BETWEEN ', local_param_date_effective_from, ' AND ', local_param_date_effective_to
     ,' )'
     ,' AND'
     ,' ('
        ,' A.is_latest = ', local_param_show_recent_only
     ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'B.sync_id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('B.sync_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position' 		  	    THEN CONCAT('B.title ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Weight' 		  	    THEN CONCAT('SUM(A.weight) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'EffectiveDate' 		THEN CONCAT('A.tdate ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    -- Set Group By & Having clause
    SET group_by_clause = ' GROUP BY B.sync_id, B.code, B.title, A.tdate';
    SET having_clause = 
		CASE WHEN param_weight = 0
			THEN '' 
			ELSE CONCAT(' HAVING SUM(A.weight) = ', param_weight)
		END;
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_kpi_position; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_kpi_position
    SELECT DISTINCT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT DISTINCT B.sync_id, 
					   IFNULL(DATE_FORMAT(A.tdate, ''%m/%d/%Y''), '''')
				FROM kpi_position A 
				INNER JOIN position B ON A.position_id = B.sync_id ', where_clause, group_by_clause, having_clause, '
			) x
		) `total_num`
		, B.sync_id AS id
        , CONCAT(B.code, '' - '', B.title) AS position
        , SUM(A.weight) AS weight
        , IFNULL(DATE_FORMAT(A.tdate, ''%m/%d/%Y''), '''') AS effective_date
	FROM kpi_position A 
    INNER JOIN position B ON A.position_id = B.sync_id '
    , where_clause, group_by_clause, having_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_kpi_position) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_kpi_position; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_score_get_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_score_get_by_id`(
	IN param_id INT,
    IN param_kpi_type VARCHAR(20)
)
BEGIN
	IF param_kpi_type = 'QUANTITATIVE'
    THEN 
		SELECT 		
			-- KPI Score details
			a.id `id`
			, b.id `kpi_id`
			, c.name `kra_group`
			, b.code `kpi_code`
			, b.name `kpi_name`
			, b.description `kpi_description`
			, a.target `target`
			, a.actual `actual`
			, a.rate `rate`
			, a.org_group_id `org_group_id`
			, 0 `employee_id`
		FROM kpi_score a 
		INNER JOIN kpi b ON a.kpi_id = b.id
		INNER JOIN kra_group c ON b.kra_group = c.id
		WHERE a.id = param_id;
    ELSE	
		SELECT 		
			-- KPI Score details
			a.id `id`
			, b.id `kpi_id`
			, c.name `kra_group`
			, b.code `kpi_code`
			, b.name `kpi_name`
			, b.description `kpi_description`
			, a.target `target`
			, a.actual `actual`
			, a.rate `rate`
			, 0 `org_group_id`
			, a.employee_id `employee_id`
		FROM kpi_score_per_employee a 
		INNER JOIN kpi b ON a.kpi_id = b.id
		INNER JOIN kra_group c ON b.kra_group = c.id
		WHERE a.id = param_id;
    END IF;
	
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_score_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_score_get_list`(
	IN param_id VARCHAR(50),
    IN param_parent_org_group VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_kpi_delimited VARCHAR(500),
    IN param_target DECIMAL(8, 5),
    IN param_actual DECIMAL(8, 5),
    IN param_rate DECIMAL(8, 5),
    IN param_period_from VARCHAR(10),
    IN param_period_to VARCHAR(10),
    IN param_is_export BOOLEAN,
    IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(50);
    DECLARE local_param_parent_org_group VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_kpi_delimited VARCHAR(1000);
    DECLARE local_param_target VARCHAR(50);
    DECLARE local_param_actual VARCHAR(50);
    DECLARE local_param_rate VARCHAR(50);
	DECLARE local_param_period_from VARCHAR(200);
    DECLARE local_param_period_to VARCHAR(200);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CASE WHEN param_id = '' THEN '''''' ELSE param_id END;
    SET local_param_parent_org_group = CASE WHEN param_parent_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_parent_org_group, '\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'b.sync_id' ELSE param_org_group_delimited END;
    SET local_param_kpi_delimited = CASE WHEN param_kpi_delimited = '' THEN 'c.id' ELSE param_kpi_delimited END;
    SET local_param_target = CASE WHEN param_target = '' THEN '''''' ELSE param_target END;
    SET local_param_actual = CASE WHEN param_actual = '' THEN '''''' ELSE param_actual END;
    SET local_param_rate = CASE WHEN param_rate = '' THEN '''''' ELSE param_rate END;
    
    SET local_param_period_from = 
	CASE WHEN STR_TO_DATE(REPLACE(param_period_from,'/','/01/'), '%m/%d/%Y') IS NULL OR param_period_from = ''
		THEN 'IFNULL(CONVERT(period, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', REPLACE(param_period_from,'/','/01/'), ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_period_to = 
	CASE WHEN STR_TO_DATE(REPLACE(param_period_to,'/','/01/'), '%m/%d/%Y') IS NULL OR param_period_to = ''
		THEN 'IFNULL(CONVERT(period, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', REPLACE(param_period_to,'/','/01/'), ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_id , '= '''''
		, ' OR a.id LIKE CONCAT(''%'', REPLACE(' , local_param_id , ', '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
     ,' ('
		,' b.sync_id IN (', local_param_org_group_delimited, ')'
     ,' )'
    ,' AND'
    ,' ('
		, local_param_parent_org_group , '= '''''
		, ' OR LOWER(IFNULL(parent_org_group,'''')) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_parent_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
     ,' ('
		,' c.id IN (', local_param_kpi_delimited, ')'
     ,' )'
    ,' AND'
    ,' ('
		, local_param_target , '= '''''
		, ' OR a.target LIKE CONCAT(''%'', REPLACE(' , local_param_target , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_actual , '= '''''
		, ' OR a.actual LIKE CONCAT(''%'', REPLACE(' , local_param_actual , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(period, DATE), '''') BETWEEN ', local_param_period_from, ' AND ', local_param_period_to
     ,' )'
    ,' AND'
    ,' ('
		, local_param_rate , '= '''''
		, ' OR a.rate LIKE CONCAT(''%'', REPLACE(' , local_param_rate , ', '' '', ''%''), ''%'')'
    ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				 THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			 THEN CONCAT('a.id ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'ParentOrgGroup' THEN CONCAT('IFNULL(parent_org_group,'''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'OrgGroup'	 	 THEN CONCAT('CONCAT(b.code, '' - '', b.description) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'KPI' 			 THEN CONCAT('CONCAT(c.code, '' - '', c.name) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Target'		 THEN CONCAT('a.target ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Actual'		 THEN CONCAT('a.actual ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Rate'			 THEN CONCAT('a.rate ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Period'			 THEN CONCAT('IFNULL(CONVERT(period, DATE), '''') ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_kpi_score; 
    DROP TEMPORARY TABLE IF EXISTS temp_parent; 
    DROP TEMPORARY TABLE IF EXISTS temp_parent1; 
    
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_parent (parent_org_group VARCHAR(1000), sync_id INT);
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_parent1 (parent_org_group VARCHAR(1000), sync_id INT);

	INSERT INTO temp_parent 
	SELECT 
		CONCAT(IFNULL(CONCAT(a.code, ' - ', a.description, ' > '), '')
			, IFNULL(CONCAT(b.code, ' - ', b.description, ' > '), '')
			, IFNULL(CONCAT(c.code, ' - ', c.description), '')
            ) AS parent_org_group 
		, a.sync_id
	FROM org_group a
	LEFT JOIN org_group b ON a.parent_org_id = b.sync_id
	LEFT JOIN org_group c ON b.parent_org_id = c.sync_id
	WHERE a.is_active = 1;
    
    INSERT INTO temp_parent1 SELECT * FROM temp_parent;
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_kpi_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
		(
			SELECT a.id FROM kpi_score a 
			INNER JOIN org_group b ON a.org_group_id = b.sync_id
			INNER JOIN kpi c on a.kpi_id = c.id
			INNER JOIN 
			(
				SELECT * FROM temp_parent1 
				UNION
				SELECT '''' `parent_org_group`, 0 `sync_id`
			) d ON b.parent_org_id = d.sync_id 
				', where_clause, '
		) x) `total_num`
        , a.id
        , IFNULL(parent_org_group,'''') AS parent_org_group
		, CONCAT(b.code, '' - '', b.description) AS org_group
        , CONCAT(c.code, '' - '', c.name) AS kpi
		, a.target AS target
		, a.actual AS actual
		, a.rate AS rate
        , DATE_FORMAT(a.period, ''%m/%Y'') AS period
        , '''' `employee`
	FROM kpi_score a
	INNER JOIN org_group b ON a.org_group_id = b.sync_id
    INNER JOIN kpi c on a.kpi_id = c.id
    INNER JOIN 
	(
		SELECT * FROM temp_parent 
		UNION
		SELECT '''' `parent_org_group`, 0 `sync_id`
	) d ON b.parent_org_id = d.sync_id '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_kpi_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- SELECT @create_temp_table;
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_kpi_score; 
    DROP TEMPORARY TABLE IF EXISTS temp_parent;
    DROP TEMPORARY TABLE IF EXISTS temp_parent1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kpi_score_per_employee_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kpi_score_per_employee_get_list`(
	IN param_id VARCHAR(50),
    IN param_employee_delimited VARCHAR(500),
    IN param_kpi_delimited VARCHAR(500),
    IN param_target DECIMAL(8, 5),
    IN param_actual DECIMAL(8, 5),
    IN param_rate DECIMAL(8, 5),
    IN param_period_from VARCHAR(10),
    IN param_period_to VARCHAR(10),
    IN param_is_export BOOLEAN,
    IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_id VARCHAR(50);
    DECLARE local_param_employee_delimited VARCHAR(1000);
    DECLARE local_param_kpi_delimited VARCHAR(1000);
    DECLARE local_param_target VARCHAR(50);
    DECLARE local_param_actual VARCHAR(50);
    DECLARE local_param_rate VARCHAR(50);
	DECLARE local_param_period_from VARCHAR(200);
    DECLARE local_param_period_to VARCHAR(200);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CASE WHEN param_id = '' THEN '''''' ELSE param_id END;
    SET local_param_employee_delimited = CASE WHEN param_employee_delimited = '' THEN 'b.sync_id' ELSE param_employee_delimited END;
    SET local_param_kpi_delimited = CASE WHEN param_kpi_delimited = '' THEN 'c.id' ELSE param_kpi_delimited END;
    SET local_param_target = CASE WHEN param_target = '' THEN '''''' ELSE param_target END;
    SET local_param_actual = CASE WHEN param_actual = '' THEN '''''' ELSE param_actual END;
    SET local_param_rate = CASE WHEN param_rate = '' THEN '''''' ELSE param_rate END;
    
    SET local_param_period_from = 
	CASE WHEN STR_TO_DATE(REPLACE(param_period_from,'/','/01/'), '%m/%d/%Y') IS NULL OR param_period_from = ''
		THEN 'IFNULL(CONVERT(period, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', REPLACE(param_period_from,'/','/01/'), ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_period_to = 
	CASE WHEN STR_TO_DATE(REPLACE(param_period_to,'/','/01/'), '%m/%d/%Y') IS NULL OR param_period_to = ''
		THEN 'IFNULL(CONVERT(period, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', REPLACE(param_period_to,'/','/01/'), ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_id , '= '''''
		, ' OR a.id LIKE CONCAT(''%'', REPLACE(' , local_param_id , ', '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
     ,' ('
		,' b.sync_id IN (', local_param_employee_delimited, ')'
     ,' )'
     ,' AND'
     ,' ('
		,' c.id IN (', local_param_kpi_delimited, ')'
     ,' )'
    ,' AND'
    ,' ('
		, local_param_target , '= '''''
		, ' OR a.target LIKE CONCAT(''%'', REPLACE(' , local_param_target , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_actual , '= '''''
		, ' OR a.actual LIKE CONCAT(''%'', REPLACE(' , local_param_actual , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(period, DATE), '''') BETWEEN ', local_param_period_from, ' AND ', local_param_period_to
     ,' )'
    ,' AND'
    ,' ('
		, local_param_rate , '= '''''
		, ' OR a.rate LIKE CONCAT(''%'', REPLACE(' , local_param_rate , ', '' '', ''%''), ''%'')'
    ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				 THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			 THEN CONCAT('a.id ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Employee'	 	 THEN CONCAT('CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename, IFNULL(CONCAT('' '', b.suffix),''''), '' ('', b.code, '') '') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'KPI' 			 THEN CONCAT('CONCAT(c.code, '' - '', c.name) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Target'		 THEN CONCAT('a.target ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Actual'		 THEN CONCAT('a.actual ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Rate'			 THEN CONCAT('a.rate ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Period'			 THEN CONCAT('IFNULL(CONVERT(period, DATE), '''') ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_kpi_score; 
    
    -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_kpi_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
		(
			SELECT a.id FROM kpi_score_per_employee a 
			INNER JOIN employee b ON a.employee_id = b.sync_id
            INNER JOIN kpi c on a.kpi_id = c.id
				', where_clause, '
		) x) `total_num`
        , a.id
		, CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename, IFNULL(CONCAT('' '', b.suffix),''''), '' ('', b.code, '') '') AS employee
        , CONCAT(c.code, '' - '', c.name) AS kpi
		, a.target AS target
		, a.actual AS actual
		, a.rate AS rate
        , DATE_FORMAT(a.period, ''%m/%Y'') AS period
        , '''' `org_group` 
        , '''' `parent_org_group` 
	FROM kpi_score_per_employee a 
	INNER JOIN employee b ON a.employee_id = b.sync_id
    INNER JOIN kpi c on a.kpi_id = c.id '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_kpi_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- SELECT @create_temp_table;
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_kpi_score;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kra_group_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kra_group_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT 
		INSTR(LOWER(name), LOWER(', local_term, ')) `index`
        , CONVERT(iD, CHAR) `id`
        , name `description`
	FROM kra_group
	WHERE LOWER(name) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, LOWER(name) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_kra_sub_group_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kra_sub_group_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT 
		INSTR(LOWER(name), LOWER(', local_term, ')) `index`
        , CONVERT(iD, CHAR) `id`
        , name `description`
	FROM kra_sub_group
	WHERE LOWER(name) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, LOWER(name) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_my_kpi_scores_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_my_kpi_scores_get_list`(
	IN param_id INT,
	IN param_trans_summary_id_delimited VARCHAR(500),
    IN param_description VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_date_from_from VARCHAR(10),
    IN param_date_from_to VARCHAR(10),
    IN param_date_to_from VARCHAR(10),
    IN param_date_to_to VARCHAR(10),
    IN param_score_from DECIMAL(8, 5),
    IN param_score_to DECIMAL(8, 5),
    IN param_is_export BOOLEAN,
    IN param_user_id INT,
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    -- DECLARE group_by_clause VARCHAR(16383);
    DECLARE having_clause VARCHAR(16383);
    
    -- Define parameters for conversion to string
	DECLARE local_param_trans_summary_id_delimited VARCHAR(1000);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_date_from_from VARCHAR(200);
    DECLARE local_param_date_from_to VARCHAR(200);
    DECLARE local_param_date_to_from VARCHAR(200);
    DECLARE local_param_date_to_to VARCHAR(200);
    DECLARE local_param_score_from VARCHAR(20);
    DECLARE local_param_score_to VARCHAR(20);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_trans_summary_id_delimited = CASE WHEN param_trans_summary_id_delimited = '' THEN '0' ELSE param_trans_summary_id_delimited END;
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'IFNULL(a.org_group_id, 0)' ELSE param_org_group_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'IFNULL(a.position_id, 0)' ELSE param_position_delimited END;
    SET local_param_score_from = CASE WHEN param_score_from < 0 THEN 'a.score' ELSE CONVERT(param_score_from, CHAR(20)) END;
    SET local_param_score_to = CASE WHEN param_score_to < 0 THEN 'a.score' ELSE CONVERT(param_score_to, CHAR(20)) END;
    
    SET local_param_date_from_from = 
	CASE WHEN STR_TO_DATE(param_date_from_from, '%m/%d/%Y') IS NULL OR param_date_from_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_from_to = 
	CASE WHEN STR_TO_DATE(param_date_from_to, '%m/%d/%Y') IS NULL OR param_date_from_to = ''
		THEN 'IFNULL(CONVERT(d.tdate_from, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_from_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_from = 
	CASE WHEN STR_TO_DATE(param_date_to_from, '%m/%d/%Y') IS NULL OR param_date_to_from = ''
		THEN 'IFNULL(CONVERT(d.tdate_to, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_from, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_param_date_to_to = 
	CASE WHEN STR_TO_DATE(param_date_to_to, '%m/%d/%Y') IS NULL OR param_date_to_to = ''
		THEN 'IFNULL(CONVERT(IFNULL(d.tdate_to, CURRENT_TIMESTAMP), DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_to_to, ''',''%m/%d/%Y''), '''')')  
	END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' a.is_active = 1'
       ,' AND'
     ,' ('
		,' is_trans_active = 1 '
     ,' )'
     -- ,' AND'
--      ,' ('
-- 		,' IFNULL(a.trans_summary_id, 0) IN (', local_param_trans_summary_id_delimited, ')'
--      ,' )'
     ,' AND'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
     ,' )'
     ,' AND'
     ,' ('
		 , local_param_description , '= '''''
		 , ' OR LOWER(d.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
     ,' )' 
     ,' AND'
      ' ('
		 ,' IFNULL(a.org_group_id, 0) IN (', local_param_org_group_delimited, ')'
     ,' )'
     ,' AND'
      ' ('
	 	,' IFNULL(a.position_id, 0) IN (', local_param_position_delimited, ')'
     ,' )'     
     ,' AND'
     ,' ('
		,' IFNULL(CONVERT(d.tdate_from, DATE), '''') BETWEEN ', local_param_date_from_from, ' AND ', local_param_date_from_to
     ,' )'
     ,' AND'
     ,' ('
	 	,' IFNULL(CONVERT(d.tdate_to, DATE), '''') BETWEEN ', local_param_date_to_from, ' AND ', local_param_date_to_to
     ,' )'
     ,' AND'
     ,' ('
			,' a.score BETWEEN ', local_param_score_from, ' AND ', local_param_score_to
     ,' )'
     ,' AND'
		,' e.system_user_id = ', param_user_id
     ,' AND'
		-- ,' a.status = ''FINALIZED'''
        ,' a.status IN(''FINALIZED'',''FOR_APPROVAL'',''APPROVED'',''FOR_REVISION'',''NEW'',''WIP'')'
     );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ' ,  
	IFNULL(CASE WHEN sidx = ''			THEN 'a.trans_summary_id DESC, a.id ASC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'TransSummaryID'		THEN CONCAT('a.trans_summary_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'		THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'OrgGroup'	THEN CONCAT('LOWER(CONCAT(a.org_group_code, '' - '',a.org_group_description)) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Position'	THEN CONCAT('LOWER(CONCAT(a.position_code, '' - '',a.position_title)) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'TDateFrom'	THEN CONCAT('d.tdate_from ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'TDateTo'	THEN CONCAT('IFNULL(d.tdate_to, ''CURRENT'') ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Score' 	THEN CONCAT('a.score ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_score
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT a.id
				FROM trans_employee_score a
                -- INNER JOIN 
-- 				(
-- 					SELECT trans_id
-- 						, MIN(IFNULL(kpi_score,0)) `min_score` 
-- 					FROM trans_employee_score_details 
-- 					GROUP BY trans_id
-- 				) b ON a.id = b.trans_id 
				INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
                INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
                INNER JOIN employee e ON a.employee_id = e.sync_id
                ', where_clause, '
			) x
		) `total_num`
		, a.id AS id
		, a.trans_summary_id
        , CASE WHEN LENGTH(d.description) > 50 
				THEN CONCAT(SUBSTRING(d.description, 1, 50), ''...'') 
				ELSE IFNULL(d.description,'''') 
			END `description`
        , CONCAT(a.org_group_code, '' - '',a.org_group_description) AS org_group
	    , CONCAT(a.position_code, '' - '',a.position_title) AS position
        , a.score
	    , DATE_FORMAT(d.tdate_from, ''%m/%d/%Y'') AS tdate_from
        , DATE_FORMAT(d.tdate_to, ''%m/%d/%Y'') AS tdate_to
        , DATE_FORMAT(a.pdate_from, ''%m/%d/%Y'') AS pdate_from
		, DATE_FORMAT(a.pdate_to, ''%m/%d/%Y'') AS pdate_to
        , CASE WHEN d.is_trans_active = 1 THEN ''YES'' ELSE ''NO'' END `is_active`
		from trans_employee_score a
      --   INNER JOIN 
-- 		(
-- 			SELECT trans_id
-- 				, MIN(IFNULL(kpi_score,0)) `min_score` 
-- 			FROM trans_employee_score_details 
-- 			GROUP BY trans_id
-- 		) b ON a.id = b.trans_id 
		INNER JOIN reference_value c ON LOWER(a.status) = LOWER(c.value) AND c.ref_code = ''EMP_SCORE_STATUS''
        INNER JOIN trans_employee_score_summary d ON a.trans_summary_id = d.id
        INNER JOIN employee e ON a.employee_id = e.sync_id
        '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
    , ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_score) a',
    (CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- SELECT @create_temp_table;

    DROP TEMPORARY TABLE IF EXISTS temp_employee_score; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
        , sync_id
        , sync_date
        , code
		, INSTR(LOWER(CONCAT(code, '' - '', description)), LOWER(', local_term, ')) `index`
        , description
        , parent_org_id
        , is_active
        , org_type
	FROM org_group
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
	ORDER BY `index`
		, CONCAT(code, '' - '', description) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_branch_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_branch_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT CAST(sync_id AS CHAR(20)) `id`
        , INSTR(LOWER(`description`), LOWER(', local_term, ')) `index`
        , `description`
	FROM (
		SELECT sync_id, CONCAT(code, '' - '' ,description) `description` FROM org_group WHERE org_type = ''BRN'' AND is_active = 1
		UNION
		SELECT 0 `sync_id`, ''HEAD OFFICE'' `description`
    ) a
	WHERE LOWER(description) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, description ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_region_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_region_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT CAST(sync_id AS CHAR(20)) `id`
        , INSTR(LOWER(`description`), LOWER(', local_term, ')) `index`
        , `description`
	FROM (
		SELECT sync_id, CONCAT(code, '' - '' ,description) `description` FROM org_group WHERE org_type = ''REG'' AND is_active = 1
		UNION
		SELECT 0 `sync_id`, ''HEAD OFFICE'' `description`
    ) a
	WHERE LOWER(description) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, description ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_autocomplete`(
	IN term VARCHAR(255),
    IN top_Results INT
)
BEGIN

	-- Define parameters for conversion to string
    DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
		, sync_id
        , sync_date
        , INSTR(CONCAT(code, '' - '', title), ', local_term, ') `index`
        , code
        , position_level_id
        , title
        , parent_position_id
        , is_active
	FROM position
	WHERE LOWER(CONCAT(code, '' - '', title)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
	ORDER BY `index`
		, CONCAT(code, '' - '', title) ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_get_by_orggroup` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_get_by_orggroup`(
	IN param_orggroup_id INT
)
BEGIN
	SELECT 		
		a.id
		, a.sync_id
        , a.sync_date
        , a.code
        , a.title
        , a.is_active
        , a.parent_position_id
        , a.position_level_id
	FROM position a
    INNER JOIN org_group_position b ON a.sync_id = b.position_id
	WHERE b.org_group_id = param_orggroup_id
    AND a.is_active = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_rerun_trans_employee_score` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_rerun_trans_employee_score`(
    IN param_date_from VARCHAR(20),
    IN param_date_to VARCHAR(20),
    IN param_trans_summary_id INT,
    IN param_role_ids VARCHAR(1000),
    IN param_created_by INT
)
BEGIN
	DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_filter VARCHAR(50);
    DECLARE local_param_ids VARCHAR(16383);
    DECLARE local_param_employees VARCHAR(16383);
    DECLARE local_param_date_from VARCHAR(20);
    DECLARE local_param_date_to VARCHAR(20);
    DECLARE local_param_use_current BOOLEAN;
    DECLARE local_param_include_lvl_below BOOLEAN;
    DECLARE local_param_override BOOLEAN;
    DECLARE local_param_include_sec_desig BOOLEAN;
    DECLARE local_param_regular_only BOOLEAN;
    
    
	SELECT description
    , filter_by
    , CASE WHEN filter_by = 'OPS' OR filter_by = 'REG' OR filter_by = 'AREA' OR
		filter_by = 'CLUS' OR filter_by = 'BRN' 
			THEN filter_org_group
        WHEN filter_by = 'POS' 
			THEN filter_position
		ELSE ''
	END
	, filter_employee	
    , param_date_from
    , param_date_to
    , filter_use_current
    , true /* regular only */
    , filter_include_lvl_below
    , filter_override
    , true /* include_sec_desig */
    INTO 
		local_param_description 
		, local_param_filter 
		, local_param_ids 
		, local_param_employees 
		, local_param_date_from 
		, local_param_date_to 
		, local_param_use_current 
		, local_param_regular_only 
		, local_param_include_lvl_below 
		, local_param_override 
		, local_param_include_sec_desig
    FROM trans_employee_score_summary WHERE id = param_trans_summary_id;

	CALL sp_run_trans_employee_score(
		local_param_description
		, local_param_filter
		, local_param_ids
		, local_param_employees
		, local_param_date_from
		, local_param_date_to
		, local_param_use_current
		, local_param_regular_only
		, local_param_include_lvl_below
		, true
		, param_trans_summary_id
		, local_param_include_sec_desig
		, param_role_ids
		, param_created_by
	);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_run_employee_score` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_run_employee_score`(
	IN param_filter VARCHAR(50),
    IN param_ids VARCHAR(16383),
    IN param_employees VARCHAR(16383),
    IN param_date_from VARCHAR(20),
    IN param_date_to VARCHAR(20),
    IN param_use_current BOOLEAN
)
BEGIN

	DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);

	DECLARE local_param_filter VARCHAR(200);
    DECLARE local_param_ids VARCHAR(200);
    DECLARE local_param_employees VARCHAR(200);
	DECLARE local_param_date_from VARCHAR(200);
    DECLARE local_param_date_to VARCHAR(200);
    
    DECLARE local_where_filter VARCHAR(16383);
    DECLARE local_date_to_display VARCHAR(16383);
    
    SET local_param_filter = 
		CASE 
			 WHEN (param_filter = 'OPS' OR
				  param_filter = 'REG' OR
                  param_filter = 'AREA' OR
                  param_filter = 'CLUS' OR
                  param_filter = 'BRN') AND
                  param_ids = 'All'
				  THEN CONCAT(' AND find_in_set(g.org_type, ''', param_filter, ''')')
			 WHEN param_filter = 'OPS' OR
				  param_filter = 'REG' OR
                  param_filter = 'AREA' OR
                  param_filter = 'CLUS' OR
                  param_filter = 'BRN'
				  THEN CONCAT(' AND find_in_set(a.g_to_id, ''', param_ids, ''')')
             WHEN param_filter = 'POS' THEN CONCAT(' AND find_in_set(a.p_to_id, ''', param_ids, ''')')
			 ELSE ''
		END;
        
    SET local_param_date_from = CONCAT('STR_TO_DATE(''', param_date_from, ''', ''%m/%d/%Y'')');
    SET local_param_date_to = CONCAT('STR_TO_DATE(''', param_date_to, ''', ''%m/%d/%Y'')');
	
    SET local_where_filter =
		CASE
			WHEN param_use_current = false 
            THEN CONCAT('IFNULL((SELECT DATE_SUB(MIN(tdate), INTERVAL 1 DAY) FROM kpi_position WHERE position_id = a.p_to_id AND tdate > kpi_pos.tdate), ', local_param_date_to, ') <= ', local_param_date_to)
            -- ELSE 'kpi_pos.tdate = (SELECT MAX(tdate) FROM kpi_position WHERE position_id = a.p_to_id)'
            ELSE '1=1'
		END;
	
    SET local_param_employees =
		CASE WHEN param_employees = 'All'
			 THEN ''
			 ELSE 
             CONCAT(' AND find_in_set(a.g_employee_id, ''', param_employees, ''')')
		END;
    
    SET local_date_to_display =
		CASE
			WHEN param_use_current = false THEN CONCAT('IFNULL((SELECT DATE_SUB(MIN(tdate), INTERVAL 1 DAY) FROM kpi_position WHERE position_id = a.p_to_id AND tdate > kpi_pos.tdate), ', local_param_date_to , ')')
            ELSE local_param_date_to
		END;
        
    
    SET where_clause = 
    CONCAT(' WHERE 
			(
				a.p_date_effective_from BETWEEN a.g_date_effective_from AND a.g_date_effective_to
				OR
				a.g_date_effective_from BETWEEN a.p_date_effective_from AND a.p_date_effective_to
			)
			AND ',
			'(
				(a.p_date_effective_from >= ', local_param_date_from, ')
				AND
				(kpi_pos.tdate BETWEEN ', local_param_date_from, ' AND ', local_param_date_to, '
                AND 
                ', local_where_filter, ')
			  )
    ', local_param_filter, local_param_employees);
	
	SET order_by_clause = ' ORDER BY employee_id, org_group_id, position_id, kpi_id';

	SET @select_query = 
    CONCAT('
		SELECT DISTINCT
			COALESCE(g_employee_id, p_employee_id) `employee_id`
			, ', local_param_date_from ,' `tdate_from`
            , ', local_param_date_to ,' `tdate_to`
			, CONVERT(a.g_to_id, UNSIGNED) `org_group_id`
			, CONVERT(a.p_to_id, UNSIGNED) `position_id`
            , kpi_pos.kpi_id `kpi_id`
            , kpi_pos.weight `kpi_weight`
            , kpi_scr.rate `kpi_score`
            , kpi_pos.id `kpi_position_id`
            , kpi_pos.tdate `pdate_from`
			, ', local_date_to_display, ' `pdate_to`
		FROM
		(
			SELECT * FROM
			(
				SELECT 
					  employee_id `g_employee_id`
					, ''org_group''
					, to_id `g_to_id`
					, date_effective_from `g_date_effective_from`
					, IFNULL(DATE_SUB(date_effective_to, INTERVAL 1 DAY), CURRENT_TIMESTAMP) `g_date_effective_to`
				FROM employee_movement 
				WHERE employee_field = ''ORG_GROUP''
				
				UNION
				
				SELECT 
					a.sync_id `g_employee_id`
				  , ''org_group''
				  , a.org_group_id `g_to_id`
				  , a.created_date `g_date_effective_from`
				  , IFNULL(
							(SELECT MIN(date_effective_from) 
							 FROM employee_movement 
							 WHERE date_effective_from > a.created_date 
							 AND employee_id = a.sync_id 
							 AND employee_field = ''ORG_GROUP''
							 ), CURRENT_TIMESTAMP()
						  ) `g_date_effective_to`
				FROM employee a
				LEFT JOIN employee_movement b on a.sync_id = b.employee_id
				WHERE (SELECT MIN(date_effective_from) 
					   FROM employee_movement 
					   WHERE date_effective_from > a.created_date 
					   AND employee_id = a.sync_id 
					   AND employee_field = ''ORG_GROUP''
					  ) IS NULL AND a.is_active = 1
			) a
			CROSS JOIN
			(
				SELECT 
					  employee_id `p_employee_id`
					, ''position''
					, to_id `p_to_id`
					, date_effective_from `p_date_effective_from`
					, IFNULL(DATE_SUB(date_effective_to, INTERVAL 1 DAY), CURRENT_TIMESTAMP) `p_date_effective_to` 
				FROM employee_movement 
				WHERE employee_field = ''POSITION''
				
				UNION
				
				SELECT 
					a.sync_id `p_employee_id`
				  , ''position''
				  , a.position_id `p_to_id`
				  , a.created_date `p_date_effective_from`
				  , IFNULL(
							(SELECT MIN(date_effective_from) 
							 FROM employee_movement 
							 WHERE date_effective_from > a.created_date 
							 AND employee_id = a.sync_id 
							 AND employee_field = ''POSITION''
							 ), CURRENT_TIMESTAMP()
						  ) `p_date_effective_to`
				FROM employee a
				LEFT JOIN employee_movement b on a.sync_id = b.employee_id
				WHERE (SELECT MIN(date_effective_from) 
					   FROM employee_movement 
					   WHERE date_effective_from > a.created_date 
					   AND employee_id = a.sync_id 
					   AND employee_field = ''POSITION''
					  ) IS NULL AND a.is_active = 1
			) b
            WHERE a.g_employee_id = b.p_employee_id
		) a 
		LEFT JOIN org_group g ON a.g_to_id = g.sync_id AND g.is_active = 1
		-- LEFT JOIN position p ON a.p_to_id = p.sync_id
		INNER JOIN kpi_position kpi_pos ON a.p_to_id = kpi_pos.position_id
        LEFT JOIN kpi_score kpi_scr ON a.g_to_id = kpi_scr.org_group_id AND kpi_pos.kpi_id = kpi_scr.kpi_id
        -- LEFT JOIN employee emp ON COALESCE(a.g_employee_id, a.p_employee_id) = emp.sync_id
    ', where_clause, order_by_clause);

   
    -- PREPARE stmt FROM @select_query;
-- 	EXECUTE stmt;
-- 	DEALLOCATE PREPARE stmt;

	SELECT @select_query;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_run_trans_employee_score` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_run_trans_employee_score`(
	IN param_description VARCHAR(1000),
    IN param_filter VARCHAR(50),
    IN param_ids VARCHAR(16383),
    IN param_employees VARCHAR(16383),
    IN param_date_from VARCHAR(20),
    IN param_date_to VARCHAR(20),
    IN param_use_current BOOLEAN,
    IN param_regular_only BOOLEAN,
    IN param_include_lvl_below BOOLEAN,
    IN param_override BOOLEAN,
    IN param_trans_summary_id INT,
    IN param_include_sec_desig BOOLEAN,
    IN param_role_ids VARCHAR(1000),
    IN param_created_by INT
)
BEGIN

	DECLARE main_table VARCHAR(16383); 
	DECLARE secondary_designation_table VARCHAR(16383); 
	DECLARE regular_employee_table VARCHAR(16383); 
	DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE group_concat_ids VARCHAR(16383);

	DECLARE local_param_description VARCHAR(16383);
	DECLARE local_param_filter VARCHAR(16383);
    DECLARE local_param_ids VARCHAR(200);
    DECLARE local_param_employees VARCHAR(200);
	DECLARE local_param_date_from VARCHAR(200);
    DECLARE local_param_date_to VARCHAR(200);
    
    DECLARE local_where_filter VARCHAR(16383);
    DECLARE local_date_to_display VARCHAR(16383);
    DECLARE local_current_date DATETIME;
    DECLARE local_special_computation_ids VARCHAR(200);
    
    SET local_current_date = CURRENT_TIMESTAMP();

	SET GROUP_CONCAT_MAX_LEN = 16383;
    
	SET local_param_description = CASE WHEN param_description = '' THEN '' ELSE CONCAT('', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '') END;
    
    /* SPECIAL computation for AMs */
	SET local_special_computation_ids = (SELECT IFNULL((SELECT sync_id FROM position WHERE code = 'AMA-CSO'),0));
    
    SET group_concat_ids = 
    CASE WHEN (param_filter = 'OPS' OR
				  param_filter = 'REG' OR
                  param_filter = 'AREA' OR
                  param_filter = 'CLUS' OR
                  param_filter = 'BRN') AND
                  param_ids = 'All' 
	THEN
		CASE WHEN param_include_lvl_below = true
			THEN (SELECT fn_org_group_get_descendants((SELECT fn_org_group_get_descendants((
					SELECT GROUP_CONCAT(DISTINCT sync_id SEPARATOR ',') 
					FROM org_group WHERE org_type =  param_filter )))))
			ELSE (SELECT GROUP_CONCAT(DISTINCT sync_id SEPARATOR ',') FROM org_group WHERE org_type =  param_filter)
		END
	ELSE ''''
    END;

    SET local_param_filter = 
		CASE 
			 WHEN (param_filter = 'OPS' OR
				  param_filter = 'REG' OR
                  param_filter = 'AREA' OR
                  param_filter = 'CLUS' OR
                  param_filter = 'BRN') AND
                  param_ids = 'All'
				  THEN 
					CASE WHEN param_include_lvl_below = true
							THEN 
								CONCAT(' AND find_in_set(a.g_to_id, ''',group_concat_ids,''')')
                            ELSE CONCAT(' AND find_in_set(a.g_to_id, ''', group_concat_ids, ''')')
						END
			 WHEN param_filter = 'OPS' OR
				  param_filter = 'REG' OR
                  param_filter = 'AREA' OR
                  param_filter = 'CLUS' OR
                  param_filter = 'BRN'
				  THEN 
						CASE WHEN param_include_lvl_below = true
							THEN CONCAT(' AND find_in_set(a.g_to_id, (SELECT fn_org_group_get_descendants(''', param_ids, ''')))')
                            ELSE CONCAT(' AND find_in_set(a.g_to_id, ''', param_ids, ''')')
						END
             WHEN param_filter = 'POS' THEN CONCAT(' AND find_in_set(a.p_to_id, ''', param_ids, ''')')
			 ELSE ''
		END;
        
    SET local_param_date_from = CONCAT('STR_TO_DATE(''', param_date_from, ''', ''%m/%d/%Y'')');
    SET local_param_date_to = CONCAT('STR_TO_DATE(''', param_date_to, ''', ''%m/%d/%Y'')');
	
    SET local_where_filter =
		CASE
			WHEN param_use_current = false 
            THEN 
            CONCAT('INNER JOIN 
					(
						SELECT DISTINCT DATE_SUB(tdate, INTERVAL 1 DAY) `tdate_m1`, position_id, tdate
						FROM kpi_position
					) date_kpi_pos ON date_kpi_pos.position_id = a.p_to_id 
						AND date_kpi_pos.tdate_m1 <= ',local_param_date_to
				)
            ELSE 'INNER JOIN 
				(
					SELECT MAX(tdate) `max_date`, position_id 
					FROM kpi_position 
					GROUP BY position_id
				) kp_max ON kp_max.max_date = kpi_pos.tdate 
					AND kpi_pos.position_id = kp_max.position_id
            '
		END;
	
    SET local_param_employees =
		CASE WHEN param_employees = 'All'
			 THEN ''
			 ELSE 
             CONCAT(' AND find_in_set(a.g_employee_id, ''', param_employees, ''')')
		END;
    
    SET local_date_to_display =
		CASE
			WHEN param_use_current = false THEN CONCAT('IFNULL((SELECT DATE_SUB(MIN(tdate), INTERVAL 1 DAY) FROM kpi_position WHERE position_id = a.p_to_id AND tdate > kpi_pos.tdate), ', local_param_date_to , ')')
            ELSE local_param_date_to
		END;
        
    
    SET where_clause = 
    CONCAT(' WHERE 
			(
                a.p_date_effective_from BETWEEN a.g_date_effective_from AND a.g_date_effective_to
				OR
				a.p_date_effective_to BETWEEN a.g_date_effective_from AND a.g_date_effective_to
                OR 
                (
					a.g_date_effective_from BETWEEN a.p_date_effective_from AND a.p_date_effective_to
                    AND 
                    a.g_date_effective_to BETWEEN a.p_date_effective_from AND a.p_date_effective_to
                )
			)
			AND ',
			'(
                (
					(
						(a.p_date_effective_from BETWEEN ', local_param_date_from, ' AND ', local_param_date_to, ')
						OR
						(a.p_date_effective_to BETWEEN ', local_param_date_from, ' AND ', local_param_date_to, ')
                    )
                    AND
                    (
						(a.g_date_effective_from BETWEEN ', local_param_date_from, ' AND ', local_param_date_to, ')
						OR
						(a.g_date_effective_to BETWEEN ', local_param_date_from, ' AND ', local_param_date_to, ')
                    )
                )
                AND a.p_date_effective_from <= a.p_date_effective_to 
				AND (kpi_pos.tdate <= ', local_param_date_to, ')
			  )
    ');
	
	SET order_by_clause = ' ORDER BY employee_id, org_group_id, position_id, kpi_id';
	
    /* Temporary table for the results */
	CREATE TEMPORARY TABLE IF NOT EXISTS results_raw
	(
		id bigint(20) NOT NULL AUTO_INCREMENT
		, employee_id INT NOT NULL
		, org_group_id INT NOT NULL
		, position_id INT NOT NULL
		, kpi_id INT NOT NULL
		, kpi_weight DECIMAL(16,5) NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
		, kpi_score_sum DECIMAL(16,5) NULL
		, kpi_position_id INT NOT NULL
		, pdate_from DATE NOT NULL
		, pdate_to DATE NOT NULL
		, gdate_from DATE NOT NULL
		, gdate_to DATE NOT NULL
        , is_autocreate BIT
		, PRIMARY KEY (`id`)
		, KEY `idx_employee_id` (`employee_id`)
		, KEY `idx_org_group_id` (`org_group_id`)
		, KEY `idx_kpi_id` (`kpi_id`)
        , KEY `idx_gdate_from` (`gdate_from`)
		, KEY `idx_gdate_to` (`gdate_to`)
	);
    
    /* Temporary table for the results */
	CREATE TEMPORARY TABLE IF NOT EXISTS results_raw_1
	(
		id bigint(20) NOT NULL AUTO_INCREMENT
		, employee_id INT NOT NULL
		, org_group_id INT NOT NULL
		, position_id INT NOT NULL
		, kpi_id INT NOT NULL
		, kpi_weight DECIMAL(16,5) NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
		, kpi_score_sum DECIMAL(16,5) NULL
		, kpi_position_id INT NOT NULL
		, pdate_from DATE NOT NULL
		, pdate_to DATE NOT NULL
		, gdate_from DATE NOT NULL
		, gdate_to DATE NOT NULL
        , is_autocreate BIT
		, PRIMARY KEY (`id`)
		, KEY `idx_employee_id` (`employee_id`)
		, KEY `idx_org_group_id` (`org_group_id`)
		, KEY `idx_kpi_id` (`kpi_id`)
        , KEY `idx_gdate_from` (`gdate_from`)
		, KEY `idx_gdate_to` (`gdate_to`)
	);
    
      /* Temporary table for the results */
	CREATE TEMPORARY TABLE IF NOT EXISTS results_raw_exceptions
	(
		id bigint(20) NOT NULL AUTO_INCREMENT
		, employee_id INT NOT NULL
		, org_group_id INT NOT NULL
		, position_id INT NOT NULL
		, kpi_id INT NOT NULL
		, kpi_weight DECIMAL(16,5) NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
		, kpi_score_sum DECIMAL(16,5) NULL
		, kpi_position_id INT NOT NULL
		, pdate_from DATE NOT NULL
		, pdate_to DATE NOT NULL
		, gdate_from DATE NOT NULL
		, gdate_to DATE NOT NULL
        , is_autocreate BIT
		, PRIMARY KEY (`id`)
		, KEY `idx_employee_id` (`employee_id`)
		, KEY `idx_org_group_id` (`org_group_id`)
		, KEY `idx_kpi_id` (`kpi_id`)
        , KEY `idx_gdate_from` (`gdate_from`)
		, KEY `idx_gdate_to` (`gdate_to`)
	);
    
	CREATE TEMPORARY TABLE IF NOT EXISTS temp_kpi_score_quantitative
	(
		org_group_id INT NOT NULL
		, kpi_id INT NOT NULL
		, position_id INT NOT NULL
		, employee_id INT NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
        , KEY `idx_org_group_id` (`org_group_id`)
		, KEY `idx_kpi_id` (`kpi_id`)
	);
    
	CREATE TEMPORARY TABLE IF NOT EXISTS temp_kpi_score_qualitative
	(
		employee_id INT NOT NULL
		, kpi_id INT NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
		, KEY `idx_employee_id` (`employee_id`)
        , KEY `idx_kpi_id` (`kpi_id`)
	);
	
    CREATE TEMPORARY TABLE IF NOT EXISTS header_ref_employee
    (employee_id INT,`employee_code` varchar(50) NULL,`lastname` varchar(100) NULL
    ,`firstname` varchar(100) NULL,`middlename` varchar(100) NULL,`suffix` varchar(10) NULL);
	
    CREATE TEMPORARY TABLE IF NOT EXISTS header_ref_position
    (position_id INT,`position_code` varchar(50) NULL,`position_title` varchar(255) NULL);

	CREATE TEMPORARY TABLE IF NOT EXISTS header_ref_org_group
		(org_group_id INT, `org_group_code` varchar(50) NULL,`org_group_description` varchar(255) NULL
        , area_id INT NULL
        ,`area` varchar(500) NULL
        , region_id INT NULL
        , department_id INT NULL
        ,`region_department` varchar(500));
	
    /* Temporary table for org_groups with children */
    CREATE TEMPORARY TABLE IF NOT EXISTS head_score 
		(org_group_id INT NOT NULL, org_children TEXT NULL, is_head BIT);
	/* Temporary table for org_groups with children */
    CREATE TEMPORARY TABLE IF NOT EXISTS head_score_1 
		(org_group_id INT NOT NULL, org_children TEXT NULL, is_head BIT);
        
	/* Temporary table for AM-CSOs */
    CREATE TEMPORARY TABLE IF NOT EXISTS secondary_desig_children 
		(employee_id INT NOT NULL, org_group_id INT NOT NULL, position_id INT NOT NULL, org_children TEXT NULL);

	DELETE FROM results_raw;
	DELETE FROM results_raw_1;
	DELETE FROM results_raw_exceptions;
	DELETE FROM temp_kpi_score_quantitative;
	DELETE FROM temp_kpi_score_qualitative;
	
	SET main_table = CONCAT('
			SELECT * FROM
			(
				/* employee movements */
				SELECT employee_id `g_employee_id`, ''org_group'', to_id `g_to_id`, date_effective_from `g_date_effective_from`
                , IFNULL(DATE_SUB(date_effective_to, INTERVAL 1 DAY), ', local_param_date_to, ') `g_date_effective_to`
				FROM employee_movement WHERE employee_field = ''ORG_GROUP''  AND is_active = 1
				
				UNION
				
                /* employee w/o movements*/
				SELECT a.sync_id `g_employee_id`, ''org_group'', a.org_group_id `g_to_id`
				  , a.date_hired `g_date_effective_from`, ', local_param_date_to, ' `g_date_effective_to`
				FROM employee a
				LEFT JOIN (SELECT employee_id FROM employee_movement WHERE employee_field = ''ORG_GROUP'' AND is_active = 1 GROUP BY employee_id) b on a.sync_id = b.employee_id
				WHERE b.employee_id IS NULL 
                
               UNION
				
                /* employee w/ movements*/
				SELECT a.sync_id `g_employee_id`, ''org_group'', b.from_id `g_to_id`, a.date_hired `g_date_effective_from`
				  , IFNULL(DATE_SUB(b.date_effective_from, INTERVAL 1 DAY), ', local_param_date_to, ') `g_date_effective_to`
				FROM employee a
				INNER JOIN (SELECT * FROM employee_movement WHERE employee_field = ''ORG_GROUP'' AND is_active = 1) b on a.sync_id = b.employee_id
                INNER JOIN
				(
					SELECT employee_id, MIN(date_effective_from) `min_date_effective_from` 
					FROM employee_movement WHERE employee_field = ''ORG_GROUP'' AND is_active = 1 GROUP BY employee_id
                ) c ON b.employee_id = c.employee_id AND b.date_effective_from = c.min_date_effective_from
			) a
			CROSS JOIN
			(
				/* employee movements */
				SELECT employee_id `p_employee_id`, ''position'', to_id `p_to_id`, date_effective_from `p_date_effective_from`
                , IFNULL(DATE_SUB(date_effective_to, INTERVAL 1 DAY), ', local_param_date_to, ') `p_date_effective_to`
				FROM employee_movement WHERE employee_field = ''POSITION'' AND is_active = 1
				
				UNION
				
                /* employee w/o movements*/
				SELECT a.sync_id `p_employee_id`, ''position'', a.position_id `p_to_id`
				  , a.date_hired `p_date_effective_from`, ', local_param_date_to, ' `p_date_effective_to`
				FROM employee a
				LEFT JOIN (SELECT employee_id FROM employee_movement WHERE employee_field = ''POSITION'' AND is_active = 1 GROUP BY employee_id) b on a.sync_id = b.employee_id
				WHERE b.employee_id IS NULL 
                
               UNION
				
                /* employee w/ movements*/
				SELECT a.sync_id `p_employee_id`, ''position'', b.from_id `p_to_id`, a.date_hired `p_date_effective_from`
				  , IFNULL(DATE_SUB(b.date_effective_from, INTERVAL 1 DAY), ', local_param_date_to, ') `p_date_effective_to`
				FROM employee a
				INNER JOIN (SELECT * FROM employee_movement WHERE employee_field = ''POSITION'' AND is_active = 1) b on a.sync_id = b.employee_id
                INNER JOIN
				(
					SELECT employee_id, MIN(date_effective_from) `min_date_effective_from` 
					FROM employee_movement WHERE employee_field = ''POSITION'' AND is_active = 1 GROUP BY employee_id
                ) c ON b.employee_id = c.employee_id AND b.date_effective_from = c.min_date_effective_from
			) b ON a.g_employee_id = b.p_employee_id
		');
	
    SET regular_employee_table = CONCAT(' INNER JOIN 
            (
				/* REGULAR EMPLOYEES 1 MONTH BEFORE RUN */
				SELECT a.sync_id `employee_id` from employee a 
                LEFT JOIN 
					(
						SELECT a.* FROM employee_movement a
                        INNER JOIN
                        (
							SELECT employee_id, employee_field, MIN(created_date) `max_created_date` 
							FROM employee_movement WHERE is_active = 1 GROUP BY employee_id, employee_field 
                        ) b ON a.employee_field = b.employee_field AND a.employee_id = b.employee_id 
                        AND a.created_date = b.max_created_date AND is_active = 1
					) b ON a.sync_id = b.employee_id 
					AND b.employee_field = ''EMPLOYMENT_STATUS'' AND to_id IN (''REGULAR'', ''PROBATIONARY_PROM'')
				WHERE a.employment_status IN (''REGULAR'', ''PROBATIONARY_PROM'') 
				AND /* MUST BE REGULAR 1 MONTH BEFORE RUN */
					CAST(DATE_ADD(IFNULL(b.date_effective_from, DATE_ADD(date_hired, INTERVAL 5 MONTH))
                    , INTERVAL 1 MONTH) AS DATE) <= CAST(', local_param_date_to, ' AS DATE)
			) c ON a.g_employee_id = c.employee_id');
    
    SET secondary_designation_table = 
		-- CASE WHEN param_include_sec_desig = true OR 1 = 1 /* Default to TRUE */
		CASE WHEN 1 = 1 /* Default to TRUE */
			THEN CONCAT('
			
			UNION ALL
			
			SELECT a.sync_id `g_employee_id`
			, ''org_group'' `org_group`
			, b.org_group_id `g_to_id`
			, IFNULL(c.date_effective_from, ', local_param_date_from, ') `g_date_effective_from`
			, IFNULL(c.date_effective_to, ', local_param_date_to, ') `g_date_effective_to`
			, a.sync_id `p_employee_id`
			, ''position'' `position`
			, b.position_id `p_to_id`
			, IFNULL(c.date_effective_from, ', local_param_date_from, ') `p_date_effective_from`
			, IFNULL(c.date_effective_to, ', local_param_date_to, ') `p_date_effective_to`
			, a.sync_id `employee_id`
			FROM employee a
			INNER JOIN employee_roving b ON a.sync_id = b.employee_id AND a.is_active = 1 AND b.is_active = 1
			INNER JOIN (SELECT * FROM employee_movement WHERE employee_field = ''SECONDARY_DESIG'' 
				AND is_active = 1 AND movement_type = ''ADD_BRANCH_ASSIGN'') c 
				ON b.employee_id = c.employee_id
			AND b.org_group_id = substring_index(c.to,'','',1)
			AND b.position_id = substring_index(substring_index(c.to,'','',2),'','',-1)
			INNER JOIN 
            (
				/* BCH can only have multiple account via Secondary Designation*/
				SELECT * FROM position WHERE code IN (''BCH'')
			) d ON d.sync_id = a.position_id OR d.sync_id = substring_index(substring_index(c.`to`,'','',2),'','',-1)
            ')
			ELSE ''
		END;
            
	SET @select_query = 
    CONCAT(' 
		INSERT INTO results_raw (employee_id, org_group_id, position_id, kpi_id
		, kpi_weight, kpi_position_id, pdate_from, pdate_to, gdate_from, gdate_to)
		SELECT DISTINCT
			IFNULL(g_employee_id, p_employee_id) `employee_id`
			, CONVERT(a.g_to_id, UNSIGNED) `org_group_id`
			, CONVERT(a.p_to_id, UNSIGNED) `position_id`
            , kpi_pos.kpi_id `kpi_id`
            , kpi_pos.weight `kpi_weight`
             , kpi_pos.id `kpi_position_id`
			, (p_date_effective_from) `pdate_from`
			, (p_date_effective_to) `pdate_to`
            , (g_date_effective_from) `gdate_from`
			, (g_date_effective_to) `gdate_to`
		FROM (', main_table, regular_employee_table, secondary_designation_table ,') a 
		INNER JOIN kpi_position kpi_pos ON a.p_to_id = kpi_pos.position_id
        ',local_where_filter,' ', where_clause, local_param_filter, local_param_employees, ' ', order_by_clause);

    PREPARE stmt FROM @select_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;

	INSERT INTO results_raw_1 SELECT * FROM results_raw;
	
	/*Insert exceptions into results_raw */
	SET @select_query_exceptions = 
    CONCAT(' 
		INSERT INTO results_raw_exceptions (employee_id, org_group_id, position_id, kpi_id
		, kpi_weight, kpi_position_id, pdate_from, pdate_to, gdate_from, gdate_to)
		SELECT DISTINCT
			a.employee_id, a.org_group_id, a.position_id
			, kpi_pos.kpi_id, kpi_pos.weight, kpi_pos.id
			, ', local_param_date_from ,' `pdate_from`
            , ', local_param_date_to ,' `pdate_to`
			, ', local_param_date_from ,' `gdate_from`
            , ', local_param_date_to ,' `gdate_to`
		FROM (
				SELECT a.employee_id, a.org_group_id, a.position_id
				FROM 
				(
					SELECT b.employee_id, b.position_id, b.org_group_id FROM results_raw_1 a
					INNER JOIN 
					(
						SELECT a.employee_id, a.position_id, b.org_group_id
						, GROUP_CONCAT((select fn_org_group_get_descendants(a.org_group_id)) SEPARATOR '','') `org_children`
						FROM employee_roving a
						INNER JOIN employee b ON a.employee_id = b.sync_id AND a.position_id = b.position_id AND a.is_active = 1
						WHERE b.position_id IN (',local_special_computation_ids,')
						GROUP BY a.employee_id, a.position_id, b.org_group_id
					) b ON find_in_set(a.org_group_id, org_children) AND a.employee_id != b.employee_id
					GROUP BY b.employee_id, b.position_id, b.org_group_id
				) a
				LEFT JOIN results_raw b ON a.employee_id = b.employee_id AND a.position_id = b.position_id 
				AND a.org_group_id = b.org_group_id
				WHERE b.id IS NULL
        ) a 
		INNER JOIN kpi_position kpi_pos ON a.position_id = kpi_pos.position_id
        ',local_where_filter,' WHERE (kpi_pos.tdate <= ', local_param_date_to, ') ', ' ', order_by_clause);

    PREPARE stmt_exceptions FROM @select_query_exceptions;
	EXECUTE stmt_exceptions;
	DEALLOCATE PREPARE stmt_exceptions;

	INSERT INTO results_raw (employee_id, org_group_id, position_id, kpi_id
		, kpi_weight, kpi_position_id, pdate_from, pdate_to, gdate_from, gdate_to) 
        SELECT employee_id, org_group_id, position_id, kpi_id
		, kpi_weight, kpi_position_id, pdate_from, pdate_to, gdate_from, gdate_to FROM results_raw_exceptions;

	/* Temporary table for the results */
	CREATE TEMPORARY TABLE IF NOT EXISTS results_raw_batch
	(
		id bigint(20) NOT NULL AUTO_INCREMENT
		, employee_id INT NOT NULL
		, org_group_id INT NOT NULL
		, position_id INT NOT NULL
		, kpi_id INT NOT NULL
		, kpi_weight DECIMAL(16,5) NOT NULL
		, kpi_target DECIMAL(16,5) NULL
		, kpi_actual DECIMAL(16,5) NULL
		, kpi_score DECIMAL(16,5) NULL
		, kpi_score_sum DECIMAL(16,5) NULL
		, kpi_position_id INT NOT NULL
		, pdate_from DATE NOT NULL
		, pdate_to DATE NOT NULL
		, gdate_from DATE NOT NULL
		, gdate_to DATE NOT NULL
        , is_autocreate BIT
		, PRIMARY KEY (`id`)
		, KEY `results_raw_org_group_id` (`org_group_id`)
	);

    CREATE TEMPORARY TABLE IF NOT EXISTS `staging_trans_employee_score` (
		  `id` int(11) NOT NULL AUTO_INCREMENT,
		  `employee_id` int(11) NOT NULL,
		  `org_group_id` int(11) NOT NULL,
		  `position_id` int(11) NOT NULL,
		  `score` decimal(16,5) DEFAULT NULL,
		  `pdate_from` date DEFAULT NULL,
		  `pdate_to` date DEFAULT NULL,
		  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `level_of_approval` int(11) DEFAULT '0',
		  `approver_id` int(11) DEFAULT NULL,
		  `approver_position_id` int(11) DEFAULT NULL,
		  `approver_org_group_id` int(11) DEFAULT NULL,
		  `employee_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `lastname` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `firstname` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `middlename` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `suffix` varchar(10) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `org_group_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `org_group_description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `position_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `position_title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `area` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `region_department` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
		  `created_by` int(11) NOT NULL,
		  PRIMARY KEY (`id`),
		  KEY `idx_employee_id` (`employee_id`),
		  KEY `idx_org_group_id` (`org_group_id`),
		  KEY `idx_position_id` (`position_id`),
		  KEY `idx_pdate_from` (`pdate_from`),
		  KEY `idx_pdate_to` (`pdate_to`)
	);

	CREATE TEMPORARY TABLE IF NOT EXISTS `staging_trans_employee_score_details` (
	  `id` bigint(20) NOT NULL AUTO_INCREMENT,
	  `trans_id` int(11) NOT NULL,
	  `kpi_id` int(11) NOT NULL,
	  `kpi_weight` decimal(16,5) NOT NULL,
	  `kpi_target` decimal(16,5) NULL,
	  `kpi_actual` decimal(16,5) NULL,
	  `is_editable` bit(1) NOT NULL DEFAULT b'0',
	  `kpi_score` decimal(16,5) DEFAULT NULL,
	  `modified_by` int(11) DEFAULT NULL,
	  `modified_date` datetime DEFAULT NULL,
	  PRIMARY KEY (`id`),
	  KEY `idx_trans_id` (`trans_id`),
	  KEY `idx_kpi_id` (`kpi_id`)
	);

     CREATE TEMPORARY TABLE IF NOT EXISTS employee_list
		(row_num INT, employee_id INT);
	
    DELETE FROM employee_list;
    DELETE FROM staging_trans_employee_score;
    DELETE FROM staging_trans_employee_score_details;
    
    SET @get_total_employee = 0;
    SET @get_total_employee_query = 
    CONCAT('SET @get_total_employee = (
		SELECT COUNT(DISTINCT a.g_employee_id) FROM (', main_table ,') a 
		INNER JOIN kpi_position kpi_pos ON a.p_to_id = kpi_pos.position_id 
		',local_where_filter, where_clause , local_param_filter, local_param_employees ,'
    );');
    
    PREPARE stmt1 FROM @get_total_employee_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;
    
	SET @get_total_employee = (@get_total_employee + (SELECT COUNT(DISTINCT employee_id) FROM results_raw_exceptions));
    
    SET @row_num = 0;
    SET @ctr = 0;
    SET @num_per_batch = 150;
    
    INSERT INTO employee_list
    SELECT @row_num:= @row_num + 1, employee_id FROM (SELECT DISTINCT employee_id FROM results_raw) A;
    
    SET SQL_SAFE_UPDATES = 0;
    
    SET @trans_summary_id = 0;
    
	IF(param_override = true)
    THEN
    
		IF param_trans_summary_id = 0 OR param_trans_summary_id  IS NULL
        THEN
			SET @trans_summary_id = (
			SELECT MAX(id) FROM trans_employee_score_summary
			WHERE filter_by = param_filter
			AND filter_org_group =  CASE WHEN param_filter = 'OPS' OR param_filter = 'REG' OR param_filter = 'AREA' OR
					  param_filter = 'CLUS' OR param_filter = 'BRN' 
					  THEN param_ids ELSE '' 
				  END
			AND filter_include_lvl_below = param_include_lvl_below
			AND filter_position = CASE WHEN param_filter = 'POS' THEN param_ids ELSE '' END
			AND filter_employee = param_employees
			AND tdate_from = STR_TO_DATE(param_date_from, '%m/%d/%Y')
			AND tdate_to = STR_TO_DATE(param_date_to, '%m/%d/%Y')
			AND filter_use_current = param_use_current
			AND is_trans_active = 1
			GROUP BY filter_by, filter_org_group, filter_include_lvl_below
			, filter_position, filter_employee, tdate_from, tdate_to, filter_use_current);
            
        ELSE
			SET @trans_summary_id = param_trans_summary_id;
        END IF;
        
        UPDATE trans_employee_score_summary 
        SET	processed_employees = 0
            , total_num_of_employees = @get_total_employee
            , employees_with_ipm = (SELECT COUNT(1) FROM employee_list)
            , is_done = b'0'
            , filter_override = param_override
            , tdate_from = STR_TO_DATE(param_date_from, '%m/%d/%Y')
            , tdate_to = STR_TO_DATE(param_date_to, '%m/%d/%Y')
            , created_date = local_current_date
            , created_by = param_created_by
        WHERE id = @trans_summary_id;
        
    ELSE
		INSERT INTO trans_employee_score_summary 
			(description, filter_by, filter_org_group, filter_include_lvl_below, filter_position, filter_employee
			,tdate_from, tdate_to, filter_override, filter_use_current, processed_employees, total_num_of_employees
            , employees_with_ipm, created_by)
		SELECT local_param_description, param_filter
			, CASE WHEN param_filter = 'OPS' OR param_filter = 'REG' OR param_filter = 'AREA' OR
				  param_filter = 'CLUS' OR param_filter = 'BRN' 
				  THEN param_ids ELSE '' 
			  END
			, param_include_lvl_below, CASE WHEN param_filter = 'POS' THEN param_ids ELSE '' END
			, param_employees, STR_TO_DATE(param_date_from, '%m/%d/%Y'), STR_TO_DATE(param_date_to, '%m/%d/%Y')
			, param_override, param_use_current
			,0, @get_total_employee, (SELECT COUNT(1) FROM employee_list), param_created_by;
		
		SET @trans_summary_id =  (SELECT LAST_INSERT_ID());
	END IF;
    
    IF @trans_summary_id = 0 OR @trans_summary_id IS NULL 
	THEN
		SELECT * FROM trans_employee_score WHERE 1 = 0;
	ELSE
		
			/*	KPI-QUANTITATIVE */
			INSERT INTO temp_kpi_score_quantitative (org_group_id, kpi_id, position_id, employee_id, kpi_target, kpi_actual, kpi_score)
			SELECT
				a.org_group_id, a.kpi_id, a.position_id, a.employee_id
				, CONVERT(SUM(kpi_scr.target), DECIMAL(16, 5)) `kpi_target`
				, CONVERT(SUM(kpi_scr.actual), DECIMAL(16, 5)) `kpi_actual`
				, CONVERT(
					CASE WHEN kpi_scr.kra_group = 'SALES'
						THEN 
							CASE WHEN SUM(kpi_scr.target) = 0 THEN 0 ELSE SUM(kpi_scr.actual) / SUM(kpi_scr.target) END
						ELSE 
							AVG(kpi_scr.rate)
					END
					, DECIMAL(16, 5)
				) `kpi_score`
			FROM (
				SELECT DISTINCT
					employee_id
					, org_group_id
					, position_id
					, kpi_id
					, kpi_weight
					 , kpi_position_id
					, MIN(pdate_from) `pdate_from`
					, MAX(pdate_to) `pdate_to`
					, MIN(gdate_from) `gdate_from`
					, MAX(gdate_to) `gdate_to`
				FROM results_raw
				GROUP BY employee_id, org_group_id, position_id, kpi_id, kpi_weight, kpi_position_id
            ) a
			INNER JOIN (
				SELECT a.*, c.name `kra_group` FROM kpi_score a INNER JOIN kpi b ON a.kpi_id = b.id INNER JOIN kra_group c ON b.kra_group = c.id
				WHERE b.kpi_type = 'QUANTITATIVE' AND b.source_type = 'AUTOCREATE'
				AND a.period BETWEEN STR_TO_DATE(param_date_from, '%m/%d/%Y') AND STR_TO_DATE(param_date_to, '%m/%d/%Y')
			 ) kpi_scr ON a.org_group_id = kpi_scr.org_group_id AND a.kpi_id = kpi_scr.kpi_id
				AND kpi_scr.period BETWEEN a.gdate_from AND a.gdate_to
				AND a.org_group_id = kpi_scr.org_group_id
			 GROUP BY a.org_group_id, a.kpi_id, kpi_scr.kpi_id, a.employee_id, a.position_id;
		  
			UPDATE results_raw a INNER JOIN temp_kpi_score_quantitative b
				ON a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
                AND a.employee_id = b.employee_id AND a.kpi_id = b.kpi_id
			SET a.kpi_target = b.kpi_target
			, a.kpi_actual = b.kpi_actual
			, a.kpi_score = b.kpi_score;
			
			/*	KPI-QUALITATIVE */
			INSERT INTO temp_kpi_score_qualitative (employee_id, kpi_id, kpi_target, kpi_actual, kpi_score)
			SELECT
				a.employee_id, a.kpi_id
				, CONVERT(SUM(kpi_scr.target), DECIMAL(16, 5)) `kpi_target`
				, CONVERT(SUM(kpi_scr.actual), DECIMAL(16, 5)) `kpi_actual`
				, CONVERT(
					CASE WHEN kpi_scr.kra_group = 'SALES'
						THEN 
							CASE WHEN SUM(kpi_scr.target) = 0 THEN 0 ELSE SUM(kpi_scr.actual) / SUM(kpi_scr.target) END
						ELSE 
							AVG(kpi_scr.rate)
					END
					, DECIMAL(16, 5)
				) `kpi_score`
			FROM (SELECT DISTINCT employee_id, kpi_id FROM results_raw) a
			INNER JOIN (
				SELECT a.*, c.name `kra_group` FROM kpi_score_per_employee a INNER JOIN kpi b ON a.kpi_id = b.id INNER JOIN kra_group c ON b.kra_group = c.id
				WHERE b.kpi_type = 'QUALITATIVE' AND b.source_type = 'AUTOCREATE'
				AND a.period BETWEEN STR_TO_DATE(param_date_from, '%m/%d/%Y') AND STR_TO_DATE(param_date_to, '%m/%d/%Y')
			 ) kpi_scr ON a.employee_id = kpi_scr.employee_id AND a.kpi_id = kpi_scr.kpi_id
			 GROUP BY a.employee_id, a.kpi_id, kpi_scr.kpi_id;
		  
			UPDATE results_raw a INNER JOIN temp_kpi_score_qualitative b
				ON a.employee_id = b.employee_id AND a.kpi_id = b.kpi_id 
			SET a.kpi_target = b.kpi_target
			, a.kpi_actual = b.kpi_actual
			, a.kpi_score = b.kpi_score;
			
			UPDATE results_raw a 
			INNER JOIN kpi b ON a.kpi_id = b.id AND b.source_type = 'AUTOCREATE'
			SET is_autocreate = b'1';
        
            
		SET @kpi_score_max_value = (SELECT value FROM reference_value WHERE ref_code = 'KPI_SCORE_MAX_VALUE');
		
		WHILE @ctr <= (SELECT COUNT(1) FROM employee_list) DO
			CALL sp_run_trans_employee_score_per_batch
            (
				param_created_by
                , @ctr, @num_per_batch, @kpi_score_max_value
				, STR_TO_DATE(param_date_from, '%m/%d/%Y') 
				, STR_TO_DATE(param_date_to, '%m/%d/%Y') 
                , local_special_computation_ids /* SPECIAL computation for AMs */
            );
			
			UPDATE trans_employee_score_summary 
			SET processed_employees = 
				CASE WHEN (processed_employees + @num_per_batch) <= employees_with_ipm
					THEN (processed_employees + @num_per_batch)
					ELSE employees_with_ipm
				END
			WHERE id = @trans_summary_id;
			
			SET @ctr = @ctr + @num_per_batch;
		END WHILE;
		
		
		IF(param_override = true)
		THEN 
        
			/*add new employees - update old*/
            UPDATE staging_trans_employee_score a
			INNER JOIN trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
				and b.is_active = 0 AND b.trans_summary_id = @trans_summary_id
			LEFT JOIN trans_employee_score c ON a.employee_id = c.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = c.position_id 
				and c.is_active = 1 AND c.trans_summary_id = @trans_summary_id
			SET b.is_active = 1
            WHERE c.id IS NULL;
			
			/*add new employees*/
			INSERT INTO trans_employee_score
					(trans_summary_id, employee_id, org_group_id, position_id, score, pdate_from, pdate_to, `status`, created_by
					, employee_code, lastname, firstname, middlename, suffix, org_group_code, org_group_description
					, position_code, position_title, area, region_department, created_date, approver_role_ids)
			SELECT @trans_summary_id, a.employee_id, a.org_group_id, a.position_id, a.score, a.pdate_from, a.pdate_to, a.`status`, a.created_by
					, a.employee_code, a.lastname, a.firstname, a.middlename, a.suffix, a.org_group_code, a.org_group_description
					, a.position_code, a.position_title, a.area, a.region_department, local_current_date, param_role_ids
			FROM staging_trans_employee_score a
			LEFT JOIN trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
				and b.is_active = 1 AND b.trans_summary_id = @trans_summary_id
			WHERE b.id IS NULL;
            
           
			/*remove old employees*/
			UPDATE trans_employee_score a
			LEFT JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
            SET a.is_active = b'0'
			WHERE /*a.status = 'NEW' AND*/ b.employee_id IS NULL AND a.trans_summary_id = @trans_summary_id ;
			
			
			/* Update existing kpi */
			UPDATE trans_employee_score a
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id 
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
            INNER JOIN staging_trans_employee_score_details c ON b.id = c.trans_id
			INNER JOIN trans_employee_score_details d ON a.id = d.trans_id AND c.kpi_id = d.kpi_id
			SET d.kpi_weight = c.kpi_weight, d.kpi_target = c.kpi_target
				, d.kpi_actual = c.kpi_actual, d.kpi_score = c.kpi_score
                , d.is_editable = c.is_editable
				, d.modified_by = param_created_by , d.modified_date = local_current_date
			WHERE 
            (
				(c.is_editable = 0 AND d.is_editable = 0) 
				OR (c.is_editable = 0 AND d.is_editable = 1) 
				OR (c.is_editable = 1 AND d.is_editable = 0)
			) AND a.trans_summary_id = @trans_summary_id AND a.is_active = 1;
		
			/* Add new kpi */
			INSERT INTO trans_employee_score_details (trans_id, kpi_id, kpi_weight, kpi_target
			, kpi_actual, is_editable, kpi_score, modified_by, modified_date)
			SELECT a.id, c.kpi_id, c.kpi_weight, c.kpi_target, c.kpi_actual
			, c.is_editable, 
            CASE WHEN c.kpi_score > @kpi_score_max_value
				THEN @kpi_score_max_value
				ELSE c.kpi_score  
			END
            , param_created_by, local_current_date
			FROM trans_employee_score a
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id 
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id
			INNER JOIN staging_trans_employee_score_details c ON b.id = c.trans_id
			LEFT JOIN trans_employee_score_details d ON a.id = d.trans_id AND c.kpi_id = d.kpi_id
			WHERE d.id IS NULL AND a.trans_summary_id = @trans_summary_id AND a.is_active = 1;
				
			/* Delete removed kpi */
			DELETE d FROM trans_employee_score a
			INNER JOIN trans_employee_score_details d ON a.id = d.trans_id
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id 
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id
			LEFT JOIN staging_trans_employee_score_details c ON b.id = c.trans_id AND c.kpi_id = d.kpi_id
			WHERE c.id IS NULL AND a.trans_summary_id = @trans_summary_id AND a.is_active = 1;
		
			UPDATE
			(
				SELECT 
				a.employee_id, a.org_group_id, a.position_id
					, CONVERT(SUM(
					CASE WHEN IFNULL(kpi_score, 0) > @kpi_score_max_value
						THEN @kpi_score_max_value
						ELSE IFNULL(kpi_score, 0)  
					END
					* (kpi_weight)), DECIMAL(16, 5)) `score`
				FROM trans_employee_score a
				INNER JOIN trans_employee_score_details b ON a.id = b.trans_id 
				WHERE a.trans_summary_id = @trans_summary_id AND a.is_active = 1
				GROUP BY a.employee_id, a.org_group_id, a.position_id, a.pdate_from, a.pdate_to
            ) a
            INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
			SET b.score = a.score;
            
			UPDATE trans_employee_score a
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id 
			SET a.score = b.score
			WHERE a.trans_summary_id = @trans_summary_id AND a.is_active = 1;
        
			SELECT a.* FROM trans_employee_score a WHERE a.trans_summary_id = @trans_summary_id AND a.is_active = 1;
			
		ELSE
			INSERT INTO trans_employee_score
					(trans_summary_id, employee_id, org_group_id, position_id, score, pdate_from, pdate_to, `status`, created_by
					, employee_code, lastname, firstname, middlename, suffix, org_group_code, org_group_description
					, position_code, position_title, area, region_department, created_date, approver_role_ids)
			SELECT @trans_summary_id, employee_id, org_group_id, position_id, score, pdate_from, pdate_to, `status`, created_by
					, employee_code, lastname, firstname, middlename, suffix, org_group_code, org_group_description
					, position_code, position_title, area, region_department, local_current_date, param_role_ids
			FROM staging_trans_employee_score;
			
			INSERT INTO trans_employee_score_details
			(trans_id, kpi_id, kpi_weight, kpi_target, kpi_actual, is_editable, kpi_score)
			SELECT a.id, c.kpi_id, c.kpi_weight, c.kpi_target, c.kpi_actual, c.is_editable, 
             CASE WHEN c.kpi_score > @kpi_score_max_value
				THEN @kpi_score_max_value
				ELSE c.kpi_score  
			END
			FROM trans_employee_score a
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
				AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id
			INNER JOIN staging_trans_employee_score_details c ON b.id = c.trans_id
			WHERE a.created_date = local_current_date;
			
			SELECT a.* FROM trans_employee_score a 
			INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
			AND a.org_group_id = b.org_group_id AND a.position_id = b.position_id
			WHERE a.created_date = local_current_date;
		
		END IF;
		
		SET @rating_ee_employees = (
				SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
				FROM (
					SELECT 
						(
							SELECT 
								CASE WHEN MAX(score) BETWEEN IFNULL(min_score, MAX(score)) AND IFNULL(max_score, MAX(score)) 
									THEN 1 
									ELSE 0 
								END 
							FROM rating_table WHERE code = 'EE'
						) `score`, trans_summary_id, employee_id 
					FROM trans_employee_score 
					WHERE trans_summary_id = @trans_summary_id AND is_active = 1
					GROUP BY trans_summary_id, employee_id
				) a
				INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
				GROUP BY b.id
			);
			
		SET @rating_me_employees = (
				SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
				FROM (
					SELECT 
						(
							SELECT 
								CASE WHEN MAX(score) BETWEEN IFNULL(min_score, MAX(score)) AND IFNULL(max_score, MAX(score)) 
									THEN 1 
									ELSE 0 
								END 
							FROM rating_table WHERE code = 'ME'
						) `score`, trans_summary_id, employee_id 
					FROM trans_employee_score 
					WHERE trans_summary_id = @trans_summary_id AND is_active = 1
					GROUP BY trans_summary_id, employee_id
				) a
				INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
				GROUP BY b.id
			);
		
		SET @rating_sbe_employees = (
				SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
				FROM (
					SELECT 
						(
							SELECT 
								CASE WHEN MAX(score) BETWEEN IFNULL(min_score, MAX(score)) AND IFNULL(max_score, MAX(score)) 
									THEN 1 
									ELSE 0 
								END 
							FROM rating_table WHERE code = 'SBE'
						) `score`, trans_summary_id, employee_id 
					FROM trans_employee_score 
					WHERE trans_summary_id = @trans_summary_id AND is_active = 1
					GROUP BY trans_summary_id, employee_id
				) a
				INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
				GROUP BY b.id
			);
		
		SET @rating_be_employees = (
				SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
				FROM (
					SELECT 
						(
							SELECT 
								CASE WHEN MAX(score) BETWEEN IFNULL(min_score, MAX(score)) AND IFNULL(max_score, MAX(score)) 
									THEN 1 
									ELSE 0 
								END 
							FROM rating_table WHERE code = 'BE'
						) `score`, trans_summary_id, employee_id 
					FROM trans_employee_score 
					WHERE trans_summary_id = @trans_summary_id AND is_active = 1
					GROUP BY trans_summary_id, employee_id
				) a
				INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
				GROUP BY b.id
			);
			
		
		UPDATE trans_employee_score_summary 
		SET rating_ee_employees = IFNULL(@rating_ee_employees,0)
			, rating_me_employees = IFNULL(@rating_me_employees,0)
			, rating_sbe_employees = IFNULL(@rating_sbe_employees,0)
			, rating_be_employees = IFNULL(@rating_be_employees,0)
			, is_done = b'1'   
		WHERE id = @trans_summary_id AND is_done = 0;
         
	END IF;
    
    DROP TABLE IF EXISTS staging_trans_employee_score;
    DROP TABLE IF EXISTS staging_trans_employee_score_details;
    DROP TABLE IF EXISTS results_raw_batch;
    DROP TABLE IF EXISTS results_raw;
    DROP TABLE IF EXISTS results_raw_1;
    DROP TABLE IF EXISTS results_raw_exceptions;
    DROP TABLE IF EXISTS temp_kpi_score_quantitative;
	DROP TABLE IF EXISTS temp_kpi_score_qualitative;
	DROP TABLE IF EXISTS results_header;
	DROP TABLE IF EXISTS results_details;
	DROP TABLE IF EXISTS head_score;
	DROP TABLE IF EXISTS head_score_1;
	DROP TABLE IF EXISTS header_ref_position;
	DROP TABLE IF EXISTS header_ref_org_group;
	DROP TABLE IF EXISTS secondary_desig_children;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_run_trans_employee_score_per_batch` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_run_trans_employee_score_per_batch`(
    IN param_created_by INT,
    IN param_ctr INT,
    IN param_num_per_batch INT,
    IN param_kpi_score_max_value DECIMAL(16, 5),
    IN param_date_from DATE,
    IN param_date_to DATE, 
    IN param_special_computation_ids VARCHAR(200) /* SPECIAL computation for AMs */
)
BEGIN	
		SET SQL_SAFE_UPDATES = 0;
		DELETE FROM results_raw_batch;
		DELETE FROM head_score;
		DELETE FROM head_score_1;
		DELETE FROM header_ref_employee;
		DELETE FROM header_ref_position;
		DELETE FROM header_ref_org_group;
		DELETE FROM secondary_desig_children;
      
        INSERT INTO results_raw_batch
        SELECT MAX(id) `id`, employee_id, org_group_id, position_id, kpi_id
			, MAX(kpi_weight) `kpi_weight`
			, MAX(kpi_target) `kpi_target`
			, MAX(kpi_actual) `kpi_actual`
			, MAX(kpi_score) `kpi_score`
            , NULL `kpi_score_sum`
            , kpi_position_id
            , MIN(pdate_from) `pdate_from`
            , MAX(pdate_to) `pdate_to`
            , MIN(gdate_from) `gdate_from`
            , MAX(gdate_to) `gdate_to`
            , MAX(is_autocreate) `is_autocreate`
            FROM results_raw 
		WHERE employee_id IN 
		(
			SELECT employee_id FROM employee_list 
			WHERE row_num BETWEEN param_ctr+1 AND param_num_per_batch + param_ctr
		) GROUP BY employee_id, org_group_id, position_id, kpi_id;
        
		/* insert distinct org_group_id to temporary table, to be processed later */
		INSERT INTO head_score (org_group_id)
		SELECT DISTINCT org_group_id FROM results_raw_batch;
		
		/* Get children of org_groups, is_head = 1 if there's any, else 0 */
		UPDATE head_score
		SET org_children = (select fn_org_group_get_descendants(org_group_id)),
		is_head = 
			-- CASE WHEN org_group_id != (select fn_org_group_get_descendants(org_group_id)) 
			CASE WHEN POSITION(',' IN (select fn_org_group_get_descendants(org_group_id)))
			THEN 1 ELSE 0 END;
		
		/* 
			Duplicate head_score to head_score_1, to be used in a single query
			, because on mysql, it is not allowed to use a temp table in a single query more than once.
		*/
		INSERT INTO head_score_1 (org_group_id, org_children, is_head) SELECT org_group_id, org_children, is_head FROM head_score;
        
		/*
			[NORMAL] Compute KPI Score for employees with subordinates
		*/
		UPDATE results_raw_batch a
		INNER JOIN 
		(
			SELECT b.org_group_id, a.kpi_id, a.kpi_score_sum, `sum_actual`, `sum_target`
			FROM
			(
				SELECT a.kpi_id, org_children, 
					CONVERT(
						CASE WHEN a.kra_group = 'SALES'
						THEN 
							CASE WHEN SUM(target) = 0 THEN 0 ELSE SUM(actual)/SUM(target) END
						ELSE 
							AVG(rate) 
						END
                        , DECIMAL(16,5)
					) `kpi_score_sum`
                , CONVERT(SUM(actual), DECIMAL(16,5)) `sum_actual`, CONVERT(SUM(target), DECIMAL(16,5)) `sum_target`
				FROM (
					SELECT a.*, c.name `kra_group` FROM kpi_score a INNER JOIN kpi b ON a.kpi_id = b.id 
                    INNER JOIN kra_group c ON b.kra_group = c.id
                    WHERE b.kpi_type = 'QUANTITATIVE' AND b.source_type = 'AUTOCREATE'
                    AND a.period BETWEEN param_date_from AND param_date_to
				) a
				INNER JOIN head_score b ON find_in_set(a.org_group_id, org_children)
				AND a.org_group_id != b.org_group_id
				GROUP BY a.kpi_id, org_children, a.kra_group
			) a
			INNER JOIN head_score_1 b ON a.org_children = b.org_children AND b.is_head = 1
		) b ON a.org_group_id = b.org_group_id AND a.kpi_id = b.kpi_id 
        
		SET a.kpi_score_sum = CONVERT(b.kpi_score_sum, DECIMAL(16,5))
        , a.kpi_actual = CONVERT(b.sum_actual, DECIMAL(16,5))
		, a.kpi_target = CONVERT(b.sum_target, DECIMAL(16,5))
        WHERE a.position_id NOT IN (param_special_computation_ids);
        
        /* [EXCEPTION - AM-CSO] Get Org Group Children */
        INSERT INTO secondary_desig_children (employee_id, position_id, org_group_id, org_children)
        SELECT DISTINCT employee_id, position_id, org_group_id, NULL
        FROM results_raw_batch
        WHERE position_id IN (param_special_computation_ids);
        
        UPDATE secondary_desig_children a
        INNER JOIN 
        (
			SELECT employee_id, position_id
            , GROUP_CONCAT((select fn_org_group_get_descendants(org_group_id)) SEPARATOR ',') `org_children`
			FROM employee_roving 
            WHERE position_id IN (param_special_computation_ids) AND is_active = 1
			GROUP BY employee_id, position_id
        ) b ON a.employee_id = b.employee_id AND a.position_id = b.position_id 
        SET a.org_children = b.org_children;
        
        /*
			[EXCEPTION - AM-CSO] Compute KPI Score for employees with subordinates FOR AM-CSOs
		*/
		UPDATE results_raw_batch a
		INNER JOIN 
		(
		
			SELECT b.employee_id, b.position_id, b.org_group_id, a.kpi_id, a.kra_group, 
				CONVERT(
					CASE WHEN a.kra_group = 'SALES'
					THEN 
						CASE WHEN SUM(target) = 0 THEN 0 ELSE SUM(actual)/SUM(target) END
					ELSE 
						AVG(rate) 
					END
					, DECIMAL(16,5)
				) `kpi_score_sum`
			, CONVERT(SUM(actual), DECIMAL(16,5)) `sum_actual`, CONVERT(SUM(target), DECIMAL(16,5)) `sum_target`
			FROM (
				SELECT a.*, c.name `kra_group` FROM kpi_score a INNER JOIN kpi b ON a.kpi_id = b.id 
				INNER JOIN kra_group c ON b.kra_group = c.id
				WHERE b.kpi_type = 'QUANTITATIVE' AND b.source_type = 'AUTOCREATE'
				AND a.period BETWEEN param_date_from AND param_date_to
			) a
			INNER JOIN secondary_desig_children b ON find_in_set(a.org_group_id, org_children)
			AND a.org_group_id != b.org_group_id
			GROUP BY b.employee_id, b.position_id, b.org_group_id, a.kpi_id, a.kra_group, b.org_children
		) b ON a.employee_id = b.employee_id AND a.position_id = b.position_id 
				AND a.org_group_id = b.org_group_id AND a.kpi_id = b.kpi_id 
		SET a.kpi_score_sum = CONVERT(b.kpi_score_sum, DECIMAL(16,5))
        , a.kpi_actual = CONVERT(b.sum_actual, DECIMAL(16,5))
		, a.kpi_target = CONVERT(b.sum_target, DECIMAL(16,5))
        WHERE a.position_id IN (param_special_computation_ids);
        
        INSERT INTO staging_trans_employee_score
		(
			employee_id
			, org_group_id
			, position_id
			, score
			, pdate_from
			, pdate_to
			, `status`
			, created_by
		)
		SELECT employee_id
			, org_group_id
			, position_id
			, CONVERT(SUM(
				CASE WHEN IFNULL(IFNULL(kpi_score_sum, kpi_score), 0) > param_kpi_score_max_value 
					THEN param_kpi_score_max_value 
					ELSE IFNULL(IFNULL(kpi_score_sum, kpi_score), 0)  
				END
                * (kpi_weight)), DECIMAL(16, 5)) `score`
			, pdate_from
			, pdate_to
			, 'NEW' `status`
			, param_created_by `created_by`
		FROM results_raw_batch
		GROUP BY employee_id, org_group_id, position_id;
		
		INSERT INTO header_ref_employee (employee_id) 
			SELECT DISTINCT employee_id FROM results_raw_batch;
		INSERT INTO header_ref_position (position_id) 
			SELECT DISTINCT position_id FROM results_raw_batch;
		INSERT INTO header_ref_org_group (org_group_id) 
			SELECT DISTINCT org_group_id FROM results_raw_batch;
		
		UPDATE header_ref_org_group 
		SET area_id = fn_org_group_get_chart_upward_by_org_type(org_group_id, 'AREA')
			, region_id = fn_org_group_get_chart_upward_by_org_type(org_group_id, 'REG')
			, department_id = fn_org_group_get_chart_upward_by_org_type(org_group_id, 'DEPT');
		
		UPDATE header_ref_employee a INNER JOIN employee e ON a.employee_id = e.sync_id AND e.is_active = 1
		SET employee_code = e.code, a.lastname = e.lastname, a.firstname = e.firstname
		, a.middlename = e.middlename, a.suffix = e.suffix;
		
		UPDATE header_ref_position a INNER JOIN position p ON a.position_id = p.sync_id AND p.is_active = 1
		SET position_code = p.code, position_title = p.title;
		
		 UPDATE header_ref_org_group a INNER JOIN org_group og ON a.org_group_id = og.sync_id AND og.is_active = 1
		SET org_group_code = og.code, org_group_description = og.description;  
	  
		UPDATE header_ref_org_group a
		LEFT JOIN (SELECT sync_id, code, description FROM org_group WHERE is_active = 1 AND org_type = 'AREA') area 
		ON area.sync_id = a.area_id
		SET area = IFNULL(CONCAT(area.code, ' - ', area.description),'');
		
		UPDATE header_ref_org_group a
		LEFT JOIN (
			SELECT sync_id, code, description  
			FROM org_group WHERE is_active = 1 AND org_type = 'REG'
		) reg ON reg.sync_id = a.region_id
		LEFT JOIN (
			SELECT sync_id, code, description  
			FROM org_group WHERE is_active = 1 AND org_type = 'DEPT'
		) dept ON dept.sync_id = a.department_id
		SET region_department = IFNULL(CONCAT(reg.code, ' - ', reg.description, IFNULL(CONCAT(dept.code, ' - ', dept.description),'')),'');              
		
		UPDATE staging_trans_employee_score a
		INNER JOIN header_ref_employee b ON a.employee_id = b.employee_id
		SET a.employee_code = b.employee_code
			, a.lastname = b.lastname
			, a.firstname = b.firstname
			, a.middlename = b.middlename
			, a.suffix = b.suffix;
			
		UPDATE staging_trans_employee_score a
		INNER JOIN header_ref_position b ON a.position_id = b.position_id
		SET a.position_code = b.position_code
			, a.position_title = b.position_title;
			
		UPDATE staging_trans_employee_score a
		INNER JOIN header_ref_org_group b ON a.org_group_id = b.org_group_id
		SET a.org_group_code = b.org_group_code
			, a.org_group_description = b.org_group_description
			, a.area = b.area
			, a.region_department = b.region_department;
		
		INSERT INTO staging_trans_employee_score_details
		(
			trans_id
			, kpi_id
			, kpi_weight
			, kpi_target
            , kpi_actual
			, is_editable
			, kpi_score
		)
		SELECT 
			b.id
			, a.kpi_id
            , a.kpi_weight
            , (kpi_target)
            , (kpi_actual)
            , CASE WHEN IFNULL(is_autocreate, b'0') = b'0' THEN b'1' ELSE b'0' END
			, (CASE WHEN IFNULL(IFNULL(kpi_score_sum, kpi_score), 0) > param_kpi_score_max_value 
				THEN param_kpi_score_max_value 
				ELSE IFNULL(IFNULL(kpi_score_sum, kpi_score), 0)  
			END)
		FROM results_raw_batch a
		INNER JOIN staging_trans_employee_score b ON a.employee_id = b.employee_id
			AND a.org_group_id = b.org_group_id
			AND a.position_id = b.position_id;
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_trans_employee_score_summary_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_trans_employee_score_summary_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT,
    IN is_admin BIT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT CAST(id AS CHAR(20)) `id`
        , INSTR(CONCAT(CASE WHEN is_trans_active = 0 THEN ''[X] '' ELSE '''' END, LOWER(LPAD(id,7,''0''))), LOWER(', local_term, ')) `index`
        , CONCAT(CASE WHEN is_trans_active = 0 THEN ''[X] '' ELSE '''' END, LPAD(id,7,''0''), 
			CASE WHEN IFNULL(description,'''') = '''' 
				THEN '''' 
                ELSE 
					CONCAT('' - '', CASE WHEN LENGTH(description) > 50 
						THEN CONCAT(SUBSTRING(description, 1, 50), ''...'') 
						ELSE description 
					END )
            END 
		) `description`
	FROM trans_employee_score_summary 
	WHERE LOWER(
		CONCAT(CASE WHEN is_trans_active = 0 THEN ''[X] '' ELSE '''' END, LPAD(id,7,''0''), 
				CASE WHEN IFNULL(description,'''') = '''' 
					THEN '''' 
					ELSE 
						CONCAT('' - '', CASE WHEN LENGTH(description) > 50 
							THEN CONCAT(SUBSTRING(description, 1, 50), ''...'') 
							ELSE description 
						END )
				END 
			)
    ) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
    AND 
    (
		is_trans_active = 1
        OR
        1 =  ', CASE WHEN is_admin = 1 THEN '1' ELSE '0' END ,'
    )
	ORDER BY `index`
		, CONCAT(CASE WHEN is_trans_active = 0 THEN ''0 '' ELSE '''' END, LPAD(id,7,''0'')) DESC ',
    CASE WHEN local_top_results = '0'
		THEN ''
		ELSE CONCAT('LIMIT ', local_top_results)
    END
    ,';');
    
     -- Execute query
    PREPARE stmt FROM @dynamic_query;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_trans_employee_score_summary_get_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_trans_employee_score_summary_get_by_id`(
	IN param_trans_summary_id INT
)
BEGIN
	SET GROUP_CONCAT_MAX_LEN = 16383;
    SELECT 
		a.id
		, IFNULL(a.description, '') `description`
        , IFNULL(ref.description, a.filter_by) `filter_by`
        , CASE WHEN filter_org_group = 'All' THEN filter_org_group ELSE IFNULL(GROUP_CONCAT(DISTINCT CONCAT(o.code,' - ',o.description) SEPARATOR ', '),'') END `filter_org_group`
        , CASE WHEN filter_include_lvl_below = 1 THEN true ELSE false END  `filter_include_lvl_below`
        , IFNULL(GROUP_CONCAT(DISTINCT CONCAT(p.code,' - ',p.title) SEPARATOR ', '),'') `filter_position`
        , CASE WHEN filter_employee = 'All' THEN filter_employee ELSE IFNULL(GROUP_CONCAT(DISTINCT CONCAT(e.lastname, ', ', e.firstname, IFNULL(CONCAT(' ', e.middlename), ''), ' (',e.code, ')') SEPARATOR ' | '),'') END `filter_employee`
        , DATE_FORMAT(tdate_from, '%m/%d/%Y') `tdate_from`
        , DATE_FORMAT(tdate_to, '%m/%d/%Y') `tdate_to`
        , CASE WHEN filter_override = 1 THEN true ELSE false END `filter_override`
        , CASE WHEN filter_use_current = 1 THEN true ELSE false END `filter_use_current`
        , processed_employees
        , total_num_of_employees
        , employees_with_ipm
		, rating_ee_employees
        , rating_me_employees
        , rating_sbe_employees
        , rating_be_employees
        , (SELECT CAST(FLOOR(IFNULL(min_score,0.00)) AS CHAR(20)) FROM rating_table WHERE code = 'EE' LIMIT 1) `rating_ee_min`
        , (SELECT CAST(FLOOR(IFNULL(max_score,0.00)) AS CHAR(20)) FROM rating_table WHERE code = 'EE' LIMIT 1) `rating_ee_max`
		, (SELECT CAST(FLOOR(IFNULL(min_score,0.00)) AS CHAR(20)) FROM rating_table WHERE code = 'ME' LIMIT 1) `rating_me_min`
        , (SELECT CAST(FLOOR(IFNULL(max_score,0.00)) AS CHAR(20))FROM rating_table WHERE code = 'ME' LIMIT 1) `rating_me_max`
		, (SELECT CAST(FLOOR(IFNULL(min_score,0.00)) AS CHAR(20))FROM rating_table WHERE code = 'SBE' LIMIT 1) `rating_sbe_min`
        , (SELECT CAST(FLOOR(IFNULL(max_score,0.00)) AS CHAR(20))FROM rating_table WHERE code = 'SBE' LIMIT 1) `rating_sbe_max`
		, (SELECT CAST(FLOOR(IFNULL(min_score,0.00)) AS CHAR(20))FROM rating_table WHERE code = 'BE' LIMIT 1) `rating_be_min`
        , (SELECT CAST(FLOOR(IFNULL(max_score,0.00)) AS CHAR(20))FROM rating_table WHERE code = 'BE' LIMIT 1) `rating_be_max`
        , CASE WHEN is_done = 1 THEN true ELSE false END `is_done`
        , CASE WHEN is_trans_active = 1 THEN true ELSE false END `is_trans_active`
        , a.created_by
        , DATE_FORMAT(a.created_date, '%m/%d/%Y %h:%i:%s %p') `created_date`
    FROM trans_employee_score_summary a
    LEFT JOIN reference_value ref ON ref.ref_code = 'ORGGROUPTYPE' AND filter_by = ref.value
    LEFT JOIN org_group o ON FIND_IN_SET(o.sync_id, a.filter_org_group) AND o.is_active = 1 
    LEFT JOIN position p ON FIND_IN_SET(p.sync_id, a.filter_position) AND p.is_active = 1 
    LEFT JOIN employee e ON FIND_IN_SET(e.sync_id, a.filter_employee) AND e.is_active = 1 
    WHERE a.id = param_trans_summary_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_trans_employee_score_summary` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_update_trans_employee_score_summary`(
	IN param_trans_summary_id INT
)
BEGIN

	SET @rating_ee_employees = (
			SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
			FROM (
				SELECT 
					(
						SELECT 
							CASE WHEN MAX(score) BETWEEN IFNULL(min_score, 0) AND IFNULL(max_score, MAX(score)) 
								THEN 1 
                                ELSE 0 
							END 
						FROM rating_table WHERE code = 'EE'
					) `score`, trans_summary_id, employee_id 
                FROM trans_employee_score 
                WHERE trans_summary_id = param_trans_summary_id
                GROUP BY trans_summary_id, employee_id
            ) a
			INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
			GROUP BY b.id
		);
        
	SET @rating_me_employees = (
			SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
			FROM (
				SELECT 
					(
						SELECT 
							CASE WHEN MAX(score) BETWEEN IFNULL(min_score, 0) AND IFNULL(max_score, MAX(score)) 
								THEN 1 
                                ELSE 0 
							END 
						FROM rating_table WHERE code = 'ME'
					) `score`, trans_summary_id, employee_id 
                FROM trans_employee_score 
                WHERE trans_summary_id = param_trans_summary_id
                GROUP BY trans_summary_id, employee_id
            ) a
			INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
			GROUP BY b.id
		);
	
    SET @rating_sbe_employees = (
			SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
			FROM (
				SELECT 
					(
						SELECT 
							CASE WHEN MAX(score) BETWEEN IFNULL(min_score, 0) AND IFNULL(max_score, MAX(score)) 
								THEN 1 
                                ELSE 0 
							END 
						FROM rating_table WHERE code = 'SBE'
					) `score`, trans_summary_id, employee_id 
                FROM trans_employee_score 
                WHERE trans_summary_id = param_trans_summary_id
                GROUP BY trans_summary_id, employee_id
            ) a
			INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
			GROUP BY b.id
		);
	
    SET @rating_be_employees = (
			SELECT SUM(CASE WHEN a.score > 0 THEN 1 ELSE 0 END)
			FROM (
				SELECT 
					(
						SELECT 
							CASE WHEN MAX(score) BETWEEN IFNULL(min_score, 0) AND IFNULL(max_score, MAX(score)) 
								THEN 1 
                                ELSE 0 
							END 
						FROM rating_table WHERE code = 'BE'
					) `score`, trans_summary_id, employee_id 
                FROM trans_employee_score 
                WHERE trans_summary_id = param_trans_summary_id
                GROUP BY trans_summary_id, employee_id
            ) a
			INNER JOIN trans_employee_score_summary b ON a.trans_summary_id = b.id
			GROUP BY b.id
		);
        

	UPDATE trans_employee_score_summary 
	SET rating_ee_employees = @rating_ee_employees
		, rating_me_employees = @rating_me_employees
		, rating_sbe_employees = @rating_sbe_employees
		, rating_be_employees = @rating_be_employees
	WHERE id = param_trans_summary_id;
    
    SELECT 1 `id` FROM dual;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `tv_autocomplete`
--

/*!50001 DROP VIEW IF EXISTS `tv_autocomplete`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_autocomplete` AS select '' AS `id`,'' AS `description` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-04-06 17:58:28
