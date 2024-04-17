CREATE DATABASE  IF NOT EXISTS `clean_ems_recruitment` /*!40100 DEFAULT CHARACTER SET latin1 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clean_ems_recruitment`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: clean_ems_recruitment
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
-- Table structure for table `applicant`
--

DROP TABLE IF EXISTS `applicant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `application_source` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `position_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `referred_by_user_id` int(11) DEFAULT NULL,
  `workflow_id` int(11) DEFAULT NULL,
  `expected_salary` decimal(13,2) DEFAULT '0.00',
  `date_applied` date NOT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `suffix` varchar(10) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `current_step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `current_step_description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `workflow_status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `current_step_approver_role_ids` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `approver_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `date_completed` date DEFAULT NULL,
  `date_scheduled` date DEFAULT NULL,
  `current_position` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `course` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `address_line_1` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `address_line_2` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `psgc_region_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `psgc_province_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `psgc_city_mun_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `psgc_barangay_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `birthdate` date NOT NULL,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `cellphone_number` varchar(15) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_hired` bit(1) NOT NULL DEFAULT b'0',
  `date_hired` date DEFAULT NULL,
  `mrf_transaction_id` varchar(70) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `failed_mrf_transaction_id` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicant`
--

LOCK TABLES `applicant` WRITE;
/*!40000 ALTER TABLE `applicant` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicant_attachment`
--

DROP TABLE IF EXISTS `applicant_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant_attachment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `applicant_id` int(11) NOT NULL,
  `attachment_type` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `source_file` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `server_file` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicant_attachment`
--

LOCK TABLES `applicant_attachment` WRITE;
/*!40000 ALTER TABLE `applicant_attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant_attachment` ENABLE KEYS */;
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
  PRIMARY KEY (`id`)
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_org_group_position` (`org_group_id`,`position_id`)
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
-- Table structure for table `position_level`
--

DROP TABLE IF EXISTS `position_level`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `position_level` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `company_id` smallint(6) NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position_level`
--

LOCK TABLES `position_level` WRITE;
/*!40000 ALTER TABLE `position_level` DISABLE KEYS */;
/*!40000 ALTER TABLE `position_level` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `psgc_barangay`
--

DROP TABLE IF EXISTS `psgc_barangay`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_barangay` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `region_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `province_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `city_mun_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `psgc_barangay`
--

LOCK TABLES `psgc_barangay` WRITE;
/*!40000 ALTER TABLE `psgc_barangay` DISABLE KEYS */;
/*!40000 ALTER TABLE `psgc_barangay` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `psgc_city`
--

DROP TABLE IF EXISTS `psgc_city`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `psgc_region_id` int(11) NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
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
-- Table structure for table `psgc_city_mun`
--

DROP TABLE IF EXISTS `psgc_city_mun`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_city_mun` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `region_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `province_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `psgc_city_mun`
--

LOCK TABLES `psgc_city_mun` WRITE;
/*!40000 ALTER TABLE `psgc_city_mun` DISABLE KEYS */;
/*!40000 ALTER TABLE `psgc_city_mun` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `psgc_province`
--

DROP TABLE IF EXISTS `psgc_province`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `psgc_province` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `region_prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `psgc_province`
--

LOCK TABLES `psgc_province` WRITE;
/*!40000 ALTER TABLE `psgc_province` DISABLE KEYS */;
/*!40000 ALTER TABLE `psgc_province` ENABLE KEYS */;
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
  `prefix` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
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
-- Table structure for table `recruiter_task`
--

DROP TABLE IF EXISTS `recruiter_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `recruiter_task` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `recruiter_user_id` int(11) NOT NULL,
  `applicant_id` int(11) NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `status` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_recruiter_task` (`recruiter_user_id`,`applicant_id`,`description`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recruiter_task`
--

LOCK TABLES `recruiter_task` WRITE;
/*!40000 ALTER TABLE `recruiter_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `recruiter_task` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
INSERT INTO `reference` VALUES (18,'SYSTEM_MODULE','EMS list of modules',_binary '\0',1,'2019-12-02 15:32:24'),(19,'APPROVE_REJECT','Result Type - Approve or Reject options',_binary '\0',1,'2019-12-02 15:32:24'),(20,'APPROVE_REJECT_SKIP','Result Type - Approve, Reject or Skip options',_binary '\0',1,'2019-12-02 15:32:24'),(21,'PASS_FAIL','Result Type - Pass or Fail options',_binary '\0',1,'2019-12-02 15:32:24'),(22,'PASS_FAIL_SKIP','Result Type - Pass, Fail or Skip options',_binary '\0',1,'2019-12-02 15:32:24'),(23,'RESULT_TYPE','Sets of responses for every step',_binary '\0',1,'2019-12-02 15:32:24'),(24,'NATURE_OF_EMPLOYMENT','Nature of Employment',_binary '',1,'2019-12-02 15:32:24'),(30,'APPLICATION_SOURCE','Source of Application',_binary '',1,'2019-12-11 15:12:28'),(31,'GEOGRAPHICAL_REGION','Geographical Region',_binary '',1,'2019-12-11 15:15:42'),(32,'REQUEST_TYPE','Recruitment Module Transactions',_binary '\0',1,'2019-12-11 16:11:12'),(33,'ATTACHMENT_TYPE','Types of Attachments',_binary '',1,'2019-12-13 15:48:32'),(34,'TASK_STATUS','Status for Recruiter Task',_binary '\0',1,'2020-01-06 14:28:50'),(35,'YES_NO','Result Type - Yes or No options',_binary '\0',1,'2020-01-28 15:00:36'),(36,'NEGATIVE_RESULT_TYPE','Negative Result Options',_binary '\0',1,'2020-01-29 10:59:21'),(37,'ORGLIST_FILTER','Org Type displayed filter on Org Group List',_binary '\0',1,'2020-06-09 15:36:40'),(38,'COURSE','List of Course',_binary '',1,'2020-06-09 15:36:40');
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
) ENGINE=InnoDB AUTO_INCREMENT=198 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference_value`
--

LOCK TABLES `reference_value` WRITE;
/*!40000 ALTER TABLE `reference_value` DISABLE KEYS */;
INSERT INTO `reference_value` VALUES (30,'SYSTEM_MODULE','SECURITY','SECURITY',1,'2019-12-02 15:33:01'),(31,'SYSTEM_MODULE','PLANTILLA','PLANTILLA',1,'2019-12-02 15:33:01'),(32,'SYSTEM_MODULE','MANPOWER','MANPOWER REQUISITION',1,'2019-12-02 15:33:01'),(33,'SYSTEM_MODULE','RECRUITMENT','RECRUITMENT MANAGEMENT',1,'2019-12-02 15:33:01'),(34,'REQUEST_TYPE','APPLICATION_PROCESS','RECRUITMENT PROCESS',1,'2019-12-02 15:33:01'),(35,'RESULT_TYPE','PASS_FAIL','PASS / FAIL',1,'2019-12-02 15:33:01'),(36,'RESULT_TYPE','APPROVE_REJECT','APPROVE / REJECT',1,'2019-12-02 15:33:01'),(37,'RESULT_TYPE','PASS_FAIL_SKIP','PASS / FAIL / SKIP',1,'2019-12-02 15:33:01'),(38,'RESULT_TYPE','APPROVE_REJECT_SKIP','APPROVE / REJECT / SKIP',1,'2019-12-02 15:33:01'),(39,'PASS_FAIL','PASS','PASS',1,'2019-12-02 15:33:01'),(40,'PASS_FAIL','FAIL','FAIL',1,'2019-12-02 15:33:01'),(41,'APPROVE_REJECT','APPROVE','APPROVE',1,'2019-12-02 15:33:01'),(42,'APPROVE_REJECT','REJECT','REJECT',1,'2019-12-02 15:33:01'),(43,'PASS_FAIL_SKIP','PASS','PASS',1,'2019-12-02 15:33:01'),(44,'PASS_FAIL_SKIP','FAIL','FAIL',1,'2019-12-02 15:33:01'),(45,'PASS_FAIL_SKIP','SKIP','SKIP',1,'2019-12-02 15:33:01'),(46,'APPROVE_REJECT_SKIP','APPROVE','APPROVE',1,'2019-12-02 15:33:01'),(47,'APPROVE_REJECT_SKIP','REJECT','REJECT',1,'2019-12-02 15:33:01'),(48,'APPROVE_REJECT_SKIP','SKIP','SKIP',1,'2019-12-02 15:33:01'),(49,'NATURE_OF_EMPLOYMENT','TRAINEE','TRAINEE',1,'2019-12-02 15:33:01'),(50,'NATURE_OF_EMPLOYMENT','REGULAR','REGULAR',1,'2019-12-02 15:33:01'),(58,'APPLICATION_SOURCE','JOBSTREET','JOBSTREET',1,'2019-12-11 15:14:20'),(59,'APPLICATION_SOURCE','WALK_IN','WALK-IN',1,'2019-12-11 15:14:20'),(60,'GEOGRAPHICAL_REGION','NCR','NATIONAL CAPITAL REGION (NCR)',1,'2019-12-11 15:38:44'),(61,'GEOGRAPHICAL_REGION','REGION1','ILOCOS REGION (REGION 1)',1,'2019-12-11 15:38:44'),(62,'GEOGRAPHICAL_REGION','CAR','CORDILLERA ADMINISTRATIVE REGION (CAR)',1,'2019-12-11 15:38:44'),(63,'GEOGRAPHICAL_REGION','REGION2','CAGAYAN VALLEY (REGION II)',1,'2019-12-11 15:38:44'),(64,'GEOGRAPHICAL_REGION','REGION3','CENTRAL LUZON (REGION III)',1,'2019-12-11 15:38:44'),(65,'GEOGRAPHICAL_REGION','REGION4A','REGION IV-A',1,'2019-12-11 15:38:44'),(66,'GEOGRAPHICAL_REGION','MIMAROPA','MIMAROPA REGION',1,'2019-12-11 15:38:44'),(67,'GEOGRAPHICAL_REGION','REGION5','REGION V',1,'2019-12-11 15:38:44'),(68,'GEOGRAPHICAL_REGION','REGION6','REGION VI',1,'2019-12-11 15:38:44'),(69,'GEOGRAPHICAL_REGION','REGION7','REGION VII',1,'2019-12-11 15:38:44'),(70,'GEOGRAPHICAL_REGION','REGION8','REGION VIII',1,'2019-12-11 15:38:44'),(71,'GEOGRAPHICAL_REGION','REGION9','REGION IX',1,'2019-12-11 15:38:44'),(72,'GEOGRAPHICAL_REGION','REGION10','REGION X',1,'2019-12-11 15:38:44'),(73,'GEOGRAPHICAL_REGION','REGION11','REGION XI',1,'2019-12-11 15:38:44'),(74,'GEOGRAPHICAL_REGION','REGION12','REGION XII',1,'2019-12-11 15:38:44'),(75,'GEOGRAPHICAL_REGION','REGION14','REGION XIII',1,'2019-12-11 15:38:44'),(76,'GEOGRAPHICAL_REGION','BARMM','BARMM',1,'2019-12-11 15:38:44'),(77,'ATTACHMENT_TYPE','RESUME','RESUME',1,'2019-12-13 15:49:09'),(78,'ATTACHMENT_TYPE','TRAINING_CERT','TRAINING CERTIFICATE',1,'2019-12-13 15:49:09'),(79,'ATTACHMENT_TYPE','OTHERS','OTHERS',1,'2019-12-13 15:49:09'),(80,'RESULT_TYPE','ACCEPT_DECLINE','ACCEPT / DECLINE',1,'2019-12-20 14:58:49'),(81,'ACCEPT_DECLINE','ACCEPT','ACCEPT',1,'2019-12-20 14:58:49'),(82,'ACCEPT_DECLINE','DECLINE','DECLINE',1,'2019-12-20 14:58:49'),(83,'TASK_STATUS','OPEN','OPEN',1,'2020-01-06 14:28:50'),(84,'TASK_STATUS','CLOSED','CLOSED',1,'2020-01-06 14:28:50'),(85,'YES_NO','YES','YES',1,'2020-01-28 15:01:35'),(86,'YES_NO','NO','NO',1,'2020-01-28 15:01:35'),(87,'RESULT_TYPE','YES_NO','YES / NO',1,'2020-01-28 15:01:35'),(88,'NEGATIVE_RESULT_TYPE','FAIL','FAIL',1,'2020-01-29 11:00:40'),(89,'NEGATIVE_RESULT_TYPE','DECLINE','DECLINE',1,'2020-01-29 11:00:40'),(90,'NEGATIVE_RESULT_TYPE','REJECT','REJECT',1,'2020-01-29 11:00:40'),(91,'NEGATIVE_RESULT_TYPE','NO','NO',1,'2020-01-29 11:00:40'),(92,'WORKFLOW_STATUS','COMPLETED','COMPLETED',1,'2020-06-09 15:36:40'),(93,'WORKFLOW_STATUS','IN_PROGRESS','IN-PROGRESS',1,'2020-06-09 15:36:40'),(94,'WORKFLOW_STATUS','FAILED','FAILED',1,'2020-06-09 15:36:40'),(95,'ORGLIST_FILTER','REG','ORG TYPE DISPLAYED FILTER ON ORG GROUP LIST',1,'2020-06-09 15:36:40'),(96,'COURSE','BSBA','BACHELOR OF BUSINESS MAJOR IN MANAGEMENT',1,'2020-06-09 15:36:40'),(97,'APPLICATION_SOURCE','ONLINE','ONLINE',13862,'2020-06-09 18:27:50'),(98,'APPLICATION_SOURCE','EMPLOYEE_REFERRAL','EMPLOYEE REFERRAL',10623,'2020-06-10 10:17:00'),(99,'APPLICATION_SOURCE','LINKEDIN','LINKEDIN',10623,'2020-06-10 12:44:45'),(100,'COURSE','BSIT','BACHELOR OF SCIENCE IN INFORMATION TECHNOLOGY',10623,'2020-06-10 15:50:32'),(101,'COURSE','BSCOMSCIE','BACHELOR OF SCIENCE IN COMPUTER SCIENCE',10623,'2020-06-10 16:19:54'),(102,'COURSE','BSPSY','BACHELOR OF SCIENCE IN PSYCHOLOGY',13864,'2020-06-11 11:15:49'),(103,'APPLICATION_SOURCE','INDEED','INDEED',13863,'2020-06-11 13:32:35'),(104,'COURSE','BSF','BACHELOR OF SCIENCE IN FORESTRY',13862,'2020-06-11 13:41:09'),(105,'COURSE','BSENTREP','BACHELOR OF SCIENCE IN ENTREPRENEURIAL MANAGEMENT ',13841,'2020-06-13 08:54:57'),(106,'COURSE','VOCAUTO','AUTOMOTIVE SERVICING',10622,'2020-06-13 08:57:52'),(107,'COURSE','BSMATH','BACHELOR OF SCIENCE IN MATHEMATICS',13841,'2020-06-13 10:12:57'),(108,'COURSE','TESDA_CULINARY','2YR-SEAMAN\'S CULINARY',13841,'2020-06-13 10:38:08'),(109,'COURSE','BSFOODSERVICEMGT','BACHELOR OF SCIENCE IN FOOD SERVICE MANAGEMENT',13841,'2020-06-13 10:53:11'),(110,'COURSE','BSSECEDUC','BACHELOR OF SECONDARY EDUCATION ',13841,'2020-06-13 13:03:24'),(111,'COURSE','BSELEMEDUC','BACHELOR OF ELEMENTARY EDUCATION',13841,'2020-06-13 13:13:14'),(112,'COURSE','BSOFFICEADMIN','BACHELOR OF SCIENCE IN OFFICE ADMINISTRATION',13841,'2020-06-13 13:22:09'),(113,'COURSE','HS_GRADUATE','HIGH SCHOOL GRADUATE',13841,'2020-06-13 13:35:44'),(114,'COURSE','BSAG','BACHELOR OF SCIENCE IN AGRICULTURE',10622,'2020-06-13 14:59:14'),(115,'COURSE','BSHRM','BACHELOR OF SCIENCE IN HOTEL AND RESTAURANT MANAGEMENT',10623,'2020-06-15 08:15:53'),(116,'COURSE','N/A','N/A',10623,'2020-06-15 08:43:51'),(117,'COURSE','3 YRS-GEN ENG','3 YRS-GENERAL ENGINEERING',10623,'2020-06-15 10:40:12'),(118,'APPLICATION_SOURCE','RETURNEE','RETURNEE',10623,'2020-06-15 10:47:06'),(119,'COURSE','BSEB','BACHELOR OF SECONDARY EDUCATION MAJOR IN BIOLOGICAL SCIENCE',10623,'2020-06-15 10:51:56'),(120,'COURSE','3 YRS CIVIL TECH','3 YEARS CIVIL TECHNOLOGY',10623,'2020-06-15 11:44:06'),(121,'COURSE','BSMAR','BACHELOR OF SCIENCE IN MARINE ENGINEER',10623,'2020-06-15 12:16:30'),(122,'APPLICATION_SOURCE','FACEBOOK','FACEBOOK',10623,'2020-06-15 14:03:44'),(123,'COURSE','BSHM','BACHELOR OF SCIENCE IN HOSPITALITY MANAGEMENT',10623,'2020-06-15 14:08:00'),(124,'COURSE','BSBA-BM','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN BUSINESS MANAGEMENT',13863,'2020-06-15 15:35:45'),(125,'COURSE','BSBA-AT','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN ACCOUNTING TECHNOLOGY',10622,'2020-06-16 10:42:17'),(126,'COURSE','BSBA-HRDM','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN HUMAN RESOURCE MANAGEMENT DEVELOPMENT',10622,'2020-06-16 10:52:45'),(127,'COURSE','BS-HTM','BACHELOR OF SCIENCE IN HOSPITALITY AND TOURISM MANAGEMENT',10622,'2020-06-16 11:00:52'),(128,'COURSE','BSBA-FM','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN FINANCIAL MANAGEMENT',10622,'2020-06-16 11:20:36'),(129,'COURSE','VOC-SMALLENGINE','VOCATIONAL SMALL ENGINE MECHANIC',13886,'2020-06-16 13:53:39'),(130,'COURSE','BS-OFFICEADMIN','BACHELOR OF SCIENCE IN OFFICE ADMINISTRATION',13886,'2020-06-16 13:57:41'),(131,'COURSE','NA','Not Applicable',13886,'2020-06-16 14:04:55'),(132,'COURSE','CSDP','COMPUTER SYSTEM DESIGN AND PROGRAMMING',13862,'2020-06-23 10:00:20'),(133,'COURSE','BAT','BACHELOR IN AGRICULTURAL TECHNOLOGY',13862,'2020-06-23 10:43:27'),(134,'COURSE','BPE','BACHELOR IN PHYSICAL EDUCATION MAJOR IN DANCE',13862,'2020-06-23 11:07:37'),(135,'COURSE','CG','COLLEGE GRADUATE',13862,'2020-06-23 11:39:38'),(136,'COURSE','BAEL','BACHELOR OF ARTS IN ENGLISH LANGUAGE',13862,'2020-06-23 14:37:24'),(137,'COURSE','BSDC','BACHELOR OF SCIENCE IN DEVELOPMENT COMMUNICATION',13862,'2020-06-23 16:51:19'),(138,'COURSE','AOA','ASSOCIATE IN OFFICE ADMINISTRATION',13862,'2020-06-24 08:30:37'),(139,'COURSE','BSBAOP','BACHELOR OF SCIENCE IN BUSINESS MANAGEMENT MAJOR IN OPERATIONS',13862,'2020-06-24 08:59:27'),(140,'COURSE','BSINDUST','BACHELOR OF SCIENCE IN INDUSTRIAL TECHNOLOGY',13862,'2020-06-24 09:20:52'),(141,'COURSE','AIRMAIN','TWO-YEAR ASSOCIATE IN AIRCRAFT MAINTENANCE TECHNOLOGY',13862,'2020-06-24 09:39:57'),(142,'COURSE','BST','BACHELOR OF SCIENCE IN THEOLOGY',13862,'2020-06-24 09:54:42'),(143,'COURSE','BSMT','BACHELOR OF SCIENCE IN MARINE TRANSPORTRATION',13862,'2020-06-24 10:39:07'),(144,'COURSE','BSBAMM','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN MARKETING MANAGEMENT',13862,'2020-06-24 13:45:56'),(145,'COURSE','BSRM','BACHELOR IN SPORTS & RECREATIONAL MANAGEMENT',13862,'2020-06-24 14:02:10'),(146,'COURSE','ET','ELECTRICAL TECHNOLOGY',13862,'2020-06-24 15:19:00'),(147,'COURSE','BSFISH','BACHELOR OF SCIENCE AND FISHERIES',13862,'2020-06-27 08:39:45'),(148,'COURSE','BSES','BACHELOR OF SCIENCE IN ENVIRONMENTAL SCIENCE',13862,'2020-06-27 09:29:40'),(149,'COURSE','MET','MANUFACTURING ENGINEERING TECHNOLOGY',13862,'2020-06-27 09:59:32'),(150,'COURSE','BSENT','BACHELOR OF SCIENCE IN ENTREPRENEURSHIP',13864,'2020-06-27 10:08:48'),(151,'COURSE','BSHAE','BACHELOR OF SCIENCE IN HOME ARTS AND ENTREPRENEURSHIP',13862,'2020-06-27 10:39:36'),(152,'ATTACHMENT_TYPE','CRS','COMPETENCY RATING SHEET',13864,'2020-06-27 10:48:06'),(153,'COURSE','BPA','BACHELOR IN PUBLIC ADMINISTRATION',10622,'2020-06-30 14:48:48'),(154,'COURSE','ABPSY','BACHELOR OF ARTS IN PSYCHOLOGY',13864,'2020-07-02 08:34:28'),(155,'COURSE','NSC','BACHELOR OF SCIENCE IN CRIMINOLOGY',16552,'2020-08-27 09:29:25'),(156,'COURSE','BSBAF','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN FISHERY BUSINESS MANAGEMENT',15968,'2020-08-27 17:05:58'),(157,'COURSE','CT','COMPUTER TECHNOLOGY',15968,'2020-09-02 17:38:42'),(158,'COURSE','BSA','BACHELOR OF SCIENCE IN ACCOUNTANCY',15521,'2020-09-03 10:30:30'),(159,'COURSE','BSCS','BACHELOR OF SCIENCE IN COMPUTER',16743,'2020-09-07 08:01:50'),(160,'COURSE','VOC-WEL','VOCATIONAL - WELDER',14500,'2020-09-08 14:28:46'),(161,'COURSE','VCAUTO','VOCATIONAL - AUTOMOTIVE SERVICING ',15968,'2020-09-10 18:08:07'),(162,'COURSE','SME','STATIONARY MARINE ENGINEERING',15968,'2020-09-10 18:18:19'),(163,'COURSE','BSMTECH','BACHELOR OF SCIENCE IN MARITIME TECH',15968,'2020-09-12 13:34:05'),(164,'COURSE','BICTECH','BACHELOR OF INFORMATION COMMUNICATION MANAGEMENT TECHNOLOGY',16743,'2020-09-14 08:54:11'),(165,'COURSE','BASS','BACHELOR OF ARTS IN SOCIAL SCIENCE',16552,'2020-11-27 10:18:31'),(166,'COURSE','BSBF','BACHELOR IN BANKING AND FINANCE',16743,'2020-11-27 10:19:28'),(167,'COURSE','BSAUTOTECH','BACHELOR OF SCIENCE MAJOR IN AUTOMOTIVE TECHNOLOGY',17473,'2020-11-27 10:20:42'),(168,'COURSE','AB-ECONOMICS','BACHELOR OF ARTS IN ECONOMICS',17473,'2020-11-27 10:23:54'),(169,'COURSE','BSC','BACHELOR OF SCIENCE IN COMMERCE',16552,'2020-11-27 13:42:55'),(170,'COURSE','BADC','BACHELOR OF ARTS IN DEVELOPMENT COMMUNICATION',15968,'2020-11-27 15:11:55'),(171,'COURSE','CEEC','CONSUMER ELECTRONICS AND ELECTRICAL COURSE',15968,'2020-11-27 15:44:20'),(172,'COURSE','BITGT','BACHELOR OF INDUSTRIAL TECHNOLOGY MAJOR IN GARMENTS TECHNOLOGY',15968,'2020-11-27 16:20:25'),(173,'COURSE','BSCUSTOMADMIN','BS IN CUSTOM ADMINISTRATION',17473,'2020-11-27 16:42:16'),(174,'COURSE','BSSOCIALWORK','BACHELOR OF SCIENCE IN SOCIAL WORK',17473,'2020-11-27 16:44:21'),(175,'COURSE','BSCD','BACHELOR OF SCIENCE IN COMMUNITY DEVELOPMENT',15968,'2020-11-27 16:55:09'),(176,'COURSE','BS NUR','BS NURSING',15968,'2020-12-02 08:22:59'),(177,'COURSE','SHS','SENIOR HIGH SCHOOL',15968,'2020-12-03 15:03:43'),(178,'COURSE','AB-POLSCIE','BACHELOR OF ARTS IN POLITICAL SCIENCE',17473,'2020-12-04 12:31:51'),(179,'COURSE','BS-FOODTECH','BS FOOD TECHNOLOGY',17473,'2020-12-04 12:34:55'),(180,'COURSE','BSME','BACHELOR OF MECHANICAL ENGINEERING',16743,'2020-12-14 09:23:31'),(181,'COURSE','ASCC','ASSOCIATE CONTACT-CENTER SERVICES',16743,'2020-12-14 10:40:46'),(182,'COURSE','WELDING','SHORT COURSE WELDING',16743,'2020-12-14 14:06:07'),(183,'COURSE','ELECTRICIAN II','ELECTRICIAN II',16743,'2020-12-14 14:30:34'),(184,'COURSE','BSBAMA','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN MANEGEMENT ACCOUNTING',14500,'2020-12-16 11:54:46'),(185,'COURSE','AB-ENGLANGUAGE','AB IN ENGLISH LANGUAGE',17473,'2020-12-18 10:48:27'),(186,'COURSE','BS_TOURISM','BS IN TOURISM MANAGEMENT',17473,'2020-12-18 14:42:02'),(187,'COURSE','BSBA-ENTREP','BACHELOR OF SCIENCE IN BUSINESS ADMINISTRATION MAJOR IN ENTREPRENEURSHIP',14500,'2021-01-05 08:06:13'),(188,'COURSE','COHS','CRUISELINE OPERATION IN HOTEL SERVICES',16743,'2021-01-08 15:54:55'),(189,'COURSE','BSWP','BS MAJOR IN WEB APPLICATION PROGRAMMING',16743,'2021-01-08 16:04:58'),(190,'COURSE','BTNF','BACHELOR OF TECHNOLOGY MAJOR IN NUTRITION AND FOOD TECHNOLOGY',16743,'2021-01-08 16:17:29'),(191,'COURSE','MT','DIPLOMA IN OFFICE MANAGMENT AND TECHNOLOGY',16743,'2021-01-08 16:23:23'),(192,'COURSE','BS REC MAN','BS RECREATIONAL MANAGEMENT',15968,'2021-01-12 12:02:20'),(193,'COURSE','BSBIO','BACHELOR OF SCIENCE IN BIOLOGY',14500,'2021-01-19 09:28:51'),(194,'COURSE','BSIS','BACHELOR OF SCIENCE IN INTERNATIONAL STUDIES',15968,'2021-01-22 09:58:13'),(195,'COURSE','COMP SEC','COMPUTER SECRETARIAL',15968,'2021-01-25 13:57:31'),(196,'COURSE','BAMC','BACHELOR OF ARTS IN MASS COMMUNICATION',15968,'2021-01-25 14:12:04'),(197,'COURSE','BSGC','BACHELOR OF SCIENCE MAJOR IN GUIDANCE COUNSELING',15968,'2021-01-25 14:55:08');
/*!40000 ALTER TABLE `reference_value` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_user`
--

DROP TABLE IF EXISTS `system_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime NOT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `username` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_user`
--

LOCK TABLES `system_user` WRITE;
/*!40000 ALTER TABLE `system_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `system_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `tv_applicant`
--

DROP TABLE IF EXISTS `tv_applicant`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `scope_org_group`,
 1 AS `lastname`,
 1 AS `firstname`,
 1 AS `middlename`,
 1 AS `suffix`,
 1 AS `application_source`,
 1 AS `mrf_transaction_id`,
 1 AS `workflow_status`,
 1 AS `current_step`,
 1 AS `date_scheduled`,
 1 AS `date_completed`,
 1 AS `approver_remarks`,
 1 AS `workflow_description`,
 1 AS `position_remarks`,
 1 AS `course`,
 1 AS `current_position`,
 1 AS `exptected_salary`,
 1 AS `date_applied`,
 1 AS `employee_id`,
 1 AS `birthdate`,
 1 AS `address_line_1`,
 1 AS `address_line_2`,
 1 AS `city`,
 1 AS `email`,
 1 AS `cellphone_number`,
 1 AS `referred_by`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_count_by_org_group`
--

DROP TABLE IF EXISTS `tv_applicant_count_by_org_group`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_count_by_org_group`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_count_by_org_group` AS SELECT 
 1 AS `org_group`,
 1 AS `position_title`,
 1 AS `applicant_count`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_history`
--

DROP TABLE IF EXISTS `tv_applicant_history`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_history`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_history` AS SELECT 
 1 AS `id`,
 1 AS `order`,
 1 AS `step`,
 1 AS `status`,
 1 AS `timestamp`,
 1 AS `remarks`,
 1 AS `result_type`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_name`
--

DROP TABLE IF EXISTS `tv_applicant_name`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_name`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_name` AS SELECT 
 1 AS `id`,
 1 AS `applicant_name`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_tagging`
--

DROP TABLE IF EXISTS `tv_applicant_tagging`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_tagging`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_tagging` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `applicant_name`,
 1 AS `application_source`,
 1 AS `position_remarks`,
 1 AS `desired_position`,
 1 AS `org_group_remarks`,
 1 AS `desired_org_group`,
 1 AS `referred_by_remarks`,
 1 AS `referred_by`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_application_approval`
--

DROP TABLE IF EXISTS `tv_application_approval`;
/*!50001 DROP VIEW IF EXISTS `tv_application_approval`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_application_approval` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `applicant_name`,
 1 AS `application_source`,
 1 AS `current_step`,
 1 AS `workflow_description`,
 1 AS `org_group_remarks`,
 1 AS `desired_org_group`,
 1 AS `position_remarks`,
 1 AS `desired_position`,
 1 AS `course`,
 1 AS `current_position`,
 1 AS `exptected_salary`,
 1 AS `has_approval`,
 1 AS `date_applied`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_existing_applicant`
--

DROP TABLE IF EXISTS `tv_mrf_existing_applicant`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_existing_applicant`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_existing_applicant` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `applicant_name`,
 1 AS `current_step`,
 1 AS `status`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_pending_task`
--

DROP TABLE IF EXISTS `tv_pending_task`;
/*!50001 DROP VIEW IF EXISTS `tv_pending_task`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_pending_task` AS SELECT 
 1 AS `id`,
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `applicant`,
 1 AS `description`,
 1 AS `status`,
 1 AS `created_date`,
 1 AS `modified_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_recruiter_task`
--

DROP TABLE IF EXISTS `tv_recruiter_task`;
/*!50001 DROP VIEW IF EXISTS `tv_recruiter_task`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_recruiter_task` AS SELECT 
 1 AS `id`,
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `recruiter`,
 1 AS `applicant`,
 1 AS `description`,
 1 AS `status`,
 1 AS `created_date`,
 1 AS `modified_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_system_user_name`
--

DROP TABLE IF EXISTS `tv_system_user_name`;
/*!50001 DROP VIEW IF EXISTS `tv_system_user_name`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_system_user_name` AS SELECT 
 1 AS `id`,
 1 AS `name`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_workflow`
--

DROP TABLE IF EXISTS `tv_workflow`;
/*!50001 DROP VIEW IF EXISTS `tv_workflow`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_workflow` AS SELECT 
 1 AS `id`,
 1 AS `code`,
 1 AS `description`,
 1 AS `created_by`,
 1 AS `created_date`,
 1 AS `modified_by`,
 1 AS `modified_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `workflow`
--

DROP TABLE IF EXISTS `workflow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workflow` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `module` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow` (`module`,`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow`
--

LOCK TABLES `workflow` WRITE;
/*!40000 ALTER TABLE `workflow` DISABLE KEYS */;
/*!40000 ALTER TABLE `workflow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workflow_step`
--

DROP TABLE IF EXISTS `workflow_step`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workflow_step` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `workflow_id` int(11) NOT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `previous_step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_required` bit(1) NOT NULL,
  `tat_days` int(11) NOT NULL,
  `allow_backflow` bit(1) NOT NULL,
  `result_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `order` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow_step` (`workflow_id`,`code`,`previous_step_code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow_step`
--

LOCK TABLES `workflow_step` WRITE;
/*!40000 ALTER TABLE `workflow_step` DISABLE KEYS */;
/*!40000 ALTER TABLE `workflow_step` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workflow_step_approver`
--

DROP TABLE IF EXISTS `workflow_step_approver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workflow_step_approver` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `workflow_id` int(11) NOT NULL,
  `step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `role_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow_step_approver` (`step_code`,`role_id`,`workflow_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow_step_approver`
--

LOCK TABLES `workflow_step_approver` WRITE;
/*!40000 ALTER TABLE `workflow_step_approver` DISABLE KEYS */;
/*!40000 ALTER TABLE `workflow_step_approver` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workflow_transaction`
--

DROP TABLE IF EXISTS `workflow_transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `workflow_transaction` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `workflow_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `request_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `step_order` int(11) NOT NULL,
  `max_step_order` int(11) NOT NULL,
  `record_id` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `result` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `approver_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `start_date_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `end_date_time` datetime DEFAULT NULL,
  `approved_by` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow_transaction`
--

LOCK TABLES `workflow_transaction` WRITE;
/*!40000 ALTER TABLE `workflow_transaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `workflow_transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'clean_ems_recruitment'
--

--
-- Dumping routines for database 'clean_ems_recruitment'
--
/*!50003 DROP FUNCTION IF EXISTS `fn_org_group_get_chart_upward_by_org_type` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
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
	CREATE TEMPORARY TABLE results_uptype (child_id INT NOT NULL, hierarchy_level INT NOT NULL, org_type VARCHAR(50));
	INSERT INTO tmp_uptype SELECT start_from;

	SET SQL_SAFE_UPDATES = 0;
	WHILE (SELECT COUNT(*) FROM tmp_uptype) > 0 DO
		SET tmp_child_id_uptype = (SELECT MIN(child_id) FROM tmp_uptype);
		DELETE FROM tmp_uptype WHERE child_id = tmp_child_id_uptype;
        SET @hierarchy_level_uptype = @hierarchy_level_uptype + 1;
        INSERT INTO results_uptype 
			SELECT sync_id, @hierarchy_level_uptype, org_type
            FROM org_group 
            WHERE sync_id = tmp_child_id_uptype 
				AND is_active = 1;
		INSERT INTO tmp_uptype 
			SELECT parent_org_id 
            FROM org_group 
            WHERE sync_id = tmp_child_id_uptype 
            AND is_active = 1;
	END WHILE;

	SET GROUP_CONCAT_MAX_LEN = 16383;
	SET returned_result_uptype =
	(
		SELECT GROUP_CONCAT(DISTINCT r.child_id SEPARATOR ',') 
        FROM results_uptype r 
        WHERE org_type = param_org_type
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
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_org_group_get_descendants`(
    start_from_ids VARCHAR(16383)
) RETURNS varchar(16383) CHARSET latin1
    READS SQL DATA
BEGIN
    DECLARE returned_result_des VARCHAR(16383);
    
	DECLARE tmp_child_id_des int;
	SET @hierarchy_level_des = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp_des;
	DROP TEMPORARY TABLE IF EXISTS param_des;
	DROP TEMPORARY TABLE IF EXISTS results_des;
	CREATE TEMPORARY TABLE tmp_des (child_id INT NOT NULL);
	CREATE TEMPORARY TABLE param_des (child_id INT NOT NULL);
-- 	CREATE INDEX tmp_child_id_idx ON tmp (child_id);
	CREATE TEMPORARY TABLE results_des (child_id INT NOT NULL, hierarchy_level INT NOT NULL);
	INSERT INTO tmp_des SELECT sync_id FROM org_group WHERE FIND_IN_SET(id, start_from_ids);
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

	SET GROUP_CONCAT_MAX_LEN = 16383;
    
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_count_by_org_group` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_count_by_org_group`()
BEGIN
	SELECT og.description as org_group, 
		   p.title as position_title, 
		   COUNT(a.id) as applicant_count
	FROM org_group og
		LEFT JOIN applicant a ON og.id = a.org_group_id
		LEFT JOIN position p ON a.position_id = p.sync_id
	WHERE is_hired = 0 
		AND og.is_active = 1
		-- AND a.is_active = 1
	GROUP BY og.description, p.title;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_get_history` */;
ALTER DATABASE `clean_ems_recruitment` CHARACTER SET latin1 COLLATE latin1_bin ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_get_history`(
	IN param_id INT
)
BEGIN
	DECLARE param_request_type VARCHAR(20);
    SET param_request_type = 'APPLICATION_PROCESS';
    SET @is_negative = 0;
    
    SELECT 
		a.id
        , c.order
		, c.description `step`
        , @is_negative := 
			CASE WHEN e.result IN (SELECT value FROM reference_value WHERE ref_code = 'NEGATIVE_RESULT_TYPE') /* negative scenarios */
				THEN 1
            WHEN @is_negative = 1
				THEN @is_negative
			ELSE 0
		END
		,	CASE WHEN d.step_order IS NULL
				THEN 
					CASE WHEN @is_negative = 1
						THEN 'N/A'
					WHEN e.step_order IS NULL AND c.order > 1
						THEN 'Pending'
					ELSE 'In-Progress'
					END
				ELSE d.result_description
			END `status`
		, IFNULL(DATE_FORMAT(d.end_date_time, '%m/%d/%Y %r'),'') `timestamp`
		, IFNULL(d.approver_remarks,'') `remarks`
        , c.result_type
	from applicant a
	INNER JOIN workflow b ON a.workflow_id = b.id
	INNER JOIN workflow_step c ON b.id = c.workflow_id
	LEFT JOIN 
	(
		SELECT 
			DISTINCT
			rv.description `result_description`
            , wft.*
        FROM workflow_transaction wft
        INNER JOIN reference_value rv ON wft.result = rv.value
		AND rv.ref_code IN (select value from reference_value WHERE ref_code = 'RESULT_TYPE')
    ) d ON d.request_type = param_request_type
		AND a.id = d.record_id
		AND d.workflow_code = b.code
		AND c.order = d.step_order
	LEFT JOIN workflow_transaction e ON e.request_type = param_request_type
		AND a.id = e.record_id
		AND e.workflow_code = b.code
		AND c.order = e.step_order + 1
	WHERE a.id = param_id 
	ORDER BY c.order;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
ALTER DATABASE `clean_ems_recruitment` CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_get_list`(
	IN param_id INT,
    IN param_last_name VARCHAR(255),
    IN param_first_name VARCHAR(255),
    IN param_middle_name VARCHAR(255),
    IN param_suffix VARCHAR(10),
    IN param_application_source_delimited VARCHAR(500),
    IN param_mrf_transaction_id VARCHAR(255),
    -- IN param_current_step_delimited VARCHAR(500),
    -- IN param_workflow_delimited VARCHAR(500),
    IN param_current_step_delimited VARCHAR(500),
    IN param_date_scheduled_from VARCHAR(10),
    IN param_date_scheduled_to VARCHAR(10),
    IN param_date_completed_from VARCHAR(10),
    IN param_date_completed_to VARCHAR(10),
    IN param_approver_remarks VARCHAR(500),
    IN param_position_remarks VARCHAR(255),
    IN param_course VARCHAR(255),
	IN param_current_position_title VARCHAR(255),    
    IN param_expected_salary_from DECIMAL(13, 2),
    IN param_expected_salary_to DECIMAL(13, 2),
    IN param_date_applied_from VARCHAR(10),
    IN param_date_applied_to VARCHAR(10),
	IN param_scope_org_type VARCHAR(50),
    IN param_scope_org_group_delimited VARCHAR(500),
    -- IN param_workflow_status_delimited VARCHAR(500),
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
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_id VARCHAR(20);
	DECLARE local_param_last_name VARCHAR(1000);
    DECLARE local_param_first_name VARCHAR(1000);
    DECLARE local_param_middle_name VARCHAR(1000);
    DECLARE local_param_suffix VARCHAR(1000);
	DECLARE local_param_application_source_delimited VARCHAR(1000);
    DECLARE local_param_mrf_transaction_id VARCHAR(1000);
    DECLARE local_param_current_step_delimited VARCHAR(1000);
    DECLARE local_param_date_scheduled_from VARCHAR(50);
	DECLARE local_param_date_scheduled_to VARCHAR(50);
	DECLARE local_param_date_completed_from VARCHAR(50);
	DECLARE local_param_date_completed_to VARCHAR(50);
	DECLARE local_param_approver_remarks VARCHAR(1000);
    -- DECLARE local_param_current_step_delimited VARCHAR(1000);
    -- DECLARE local_param_workflow_delimited VARCHAR(1000);
    DECLARE local_param_position_remarks VARCHAR(1000);
    DECLARE local_param_course VARCHAR(1000);
	DECLARE local_param_current_position_title VARCHAR(1000);    
    DECLARE local_param_expected_salary_from VARCHAR(50);
    DECLARE local_param_expected_salary_to VARCHAR(50);
    DECLARE local_param_date_applied_from VARCHAR(50);
    DECLARE local_param_date_applied_to VARCHAR(50);
    DECLARE local_param_scope_descendants_delimited VARCHAR(16383);
    -- DECLARE local_param_workflow_status_delimited VARCHAR(1000);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
	SET local_param_last_name = CASE WHEN param_last_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_last_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_first_name = CASE WHEN param_first_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_first_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_middle_name = CASE WHEN param_middle_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_middle_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_suffix = CASE WHEN param_suffix = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_suffix,'\\','\\\\'), '''', ''''''), '''') END;

    SET local_param_mrf_transaction_id = CASE WHEN param_mrf_transaction_id = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_mrf_transaction_id,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_application_source_delimited = CASE WHEN param_application_source_delimited = '' THEN 'b.id' ELSE param_application_source_delimited END;
    SET local_param_current_step_delimited = CASE WHEN param_current_step_delimited = '' THEN '' ELSE param_current_step_delimited END;
    
    SET local_param_date_scheduled_from = 
		CASE WHEN STR_TO_DATE(param_date_scheduled_from, '%m/%d/%Y') IS NULL OR param_date_scheduled_from = ''
			THEN 'IFNULL(CONVERT(a.date_scheduled, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_scheduled_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_scheduled_to = 
		CASE WHEN STR_TO_DATE(param_date_scheduled_to, '%m/%d/%Y') IS NULL OR param_date_scheduled_to = ''
			THEN 'IFNULL(CONVERT(a.date_scheduled, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_scheduled_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_date_completed_from = 
		CASE WHEN STR_TO_DATE(param_date_completed_from, '%m/%d/%Y') IS NULL OR param_date_completed_from = ''
			THEN 'IFNULL(CONVERT(a.date_completed, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_completed_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_completed_to = 
		CASE WHEN STR_TO_DATE(param_date_completed_to, '%m/%d/%Y') IS NULL OR param_date_completed_to = ''
			THEN 'IFNULL(CONVERT(a.date_completed, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_completed_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_approver_remarks = 
    CASE WHEN param_approver_remarks = '' 
		THEN ''''''
		ELSE CONCAT('''', REPLACE(REPLACE(param_approver_remarks,'\\','\\\\'), '''', ''''''), '''') 
    END;
    
    -- SET local_param_current_step_delimited = CASE WHEN param_current_step_delimited = '' THEN '' ELSE param_current_step_delimited END;
    -- SET local_param_workflow_delimited = CASE WHEN param_workflow_delimited = '' THEN 'IFNULL(wf.id, 0)' ELSE param_workflow_delimited END;
    SET local_param_position_remarks = CASE WHEN param_position_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_course = CASE WHEN param_course = '' THEN '''''' ELSE CONCAT('''', REPLACE(param_course, '''', ''''''), '''') END;
	SET local_param_current_position_title = CASE WHEN param_current_position_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_current_position_title,'\\','\\\\'), '''', ''''''), '''') END;   
    SET local_param_expected_salary_from = CASE WHEN param_expected_salary_from = 0 OR param_expected_salary_from IS NULL THEN 'IFNULL(a.expected_salary, 0)' ELSE CONVERT(param_expected_salary_from,CHAR(20)) END;
    SET local_param_expected_salary_to = CASE WHEN param_expected_salary_to = 0 OR param_expected_salary_to IS NULL THEN 'IFNULL(a.expected_salary, 0)' ELSE CONVERT(param_expected_salary_to,CHAR(20)) END;
    SET local_param_date_applied_from = 
		CASE WHEN STR_TO_DATE(param_date_applied_from, '%m/%d/%Y') IS NULL OR param_date_applied_from = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_applied_to = 
		CASE WHEN STR_TO_DATE(param_date_applied_to, '%m/%d/%Y') IS NULL OR param_date_applied_to = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_scope_descendants_delimited = CASE WHEN param_scope_org_group_delimited = '' THEN 'c.id' ELSE param_scope_org_group_delimited END;
        
	-- SET local_param_workflow_status_delimited = CASE WHEN param_workflow_status_delimited = '' THEN 'IFNULL(a.workflow_status, ''IN_PROGRESS'')' ELSE CONCAT('''', REPLACE(REPLACE(param_workflow_status_delimited,'\\','\\\\'),',',''','''), '''') END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
    ,' )'
-- ,' AND'
--     ,' ('
-- 		, local_param_applicant_name , '= '''''
-- 		, ' OR LOWER(CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '')) 
-- 				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant_name , '), '' '', ''%''), ''%'')'
--     ,' )'
	,' AND'
    ,' ('
		, local_param_last_name , '= '''''
		, ' OR IFNULL(LOWER(a.lastname),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_last_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_first_name , '= '''''
		, ' OR IFNULL(LOWER(a.firstname),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_first_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_middle_name , '= '''''
		, ' OR IFNULL(LOWER(a.middlename),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_middle_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_suffix , '= '''''
		, ' OR IFNULL(LOWER(a.suffix),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_suffix , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_mrf_transaction_id , '= '''''
		, ' OR LOWER(IFNULL(a.mrf_transaction_id, '''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_mrf_transaction_id , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' b.id IN (', local_param_application_source_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_current_step_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_approver_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.approver_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_approver_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_scheduled, DATE),'''') BETWEEN ', local_param_date_scheduled_from, ' AND ', local_param_date_scheduled_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_completed, DATE),'''') BETWEEN ', local_param_date_completed_from, ' AND ', local_param_date_completed_to
    ,' )'
    -- ,' AND'
--     ,' ('
-- 		,''''' = ''', local_param_current_step_delimited, ''''
-- 		,' OR '
-- 		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
--     ,' )'
    -- ,' AND'
--     ,' ('
-- 		,' IFNULL(wf.id, 0) IN (', local_param_workflow_delimited, ')'
--     ,' )'
    ,' AND'
    ,' ('
		, local_param_position_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.position_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_course , '= '''''
		, ' OR LOWER(a.course) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_course , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_current_position_title , '= '''''
		, ' OR LOWER(a.current_position) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_current_position_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(a.expected_salary, 0) BETWEEN ', local_param_expected_salary_from, ' AND ', local_param_expected_salary_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_applied, DATE),'''') BETWEEN ', local_param_date_applied_from, ' AND ', local_param_date_applied_to
    ,' )'
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    ,' AND'
    ,' ('
		,' c.id IN (', local_param_scope_descendants_delimited, ')'
    ,' )'
    -- ,' AND'
--     ,' ('
-- 		,' IFNULL(a.workflow_status, ''IN_PROGRESS'') IN (', local_param_workflow_status_delimited, ')'
--     ,' )'
    
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('a.id ', local_sord, ',') END, ''), 
-- 	IFNULL(CASE WHEN sidx = 'ApplicantName'			THEN CONCAT('CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'LastName'				THEN CONCAT('IFNULL(a.lastname,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'FirstName'				THEN CONCAT('IFNULL(a.firstname,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'MiddleName'			THEN CONCAT('IFNULL(a.middlename,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Suffix'				THEN CONCAT('IFNULL(a.suffix,'''') ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'MRFTransactionID'		THEN CONCAT('a.mrf_transaction_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicationSource'		THEN CONCAT('b.description ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('a.current_step_code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateScheduled'				THEN CONCAT('IFNULL(a.date_scheduled, '''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCompleted'				THEN CONCAT('IFNULL(a.date_completed, '''') ', local_sord, ',') END, ''),
	-- IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('CASE WHEN IFNULL(a.current_step_code, '''') = '''' 
-- 			THEN ''''
--             ELSE CONCAT(a.current_step_code, '' - '', a.current_step_description) 
-- 		END ', local_sord, ',') END, ''), 
	-- IFNULL(CASE WHEN sidx = 'WorkflowDescription'	THEN CONCAT('CONCAT(wf.code, '' - '', wf.description) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionRemarks'		THEN CONCAT('IFNULL(a.position_remarks,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Course'				THEN CONCAT('a.course ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentPosition'		THEN CONCAT('a.current_position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ExpectedSalary'		THEN CONCAT('a.expected_salary ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateApplied'			THEN CONCAT('a.date_applied ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
    
    SET joined_tables = "applicant a
	INNER JOIN reference_value b ON a.application_source = b.value 
		AND b.ref_code = ""APPLICATION_SOURCE""
	-- LEFT JOIN reference_value ref ON IFNULL(a.workflow_status, ""IN_PROGRESS"") = ref.value 
-- 		AND ref.ref_code = ""WORKFLOW_STATUS"" 
	LEFT JOIN psgc_region c ON a.psgc_region_code = c.code
    -- LEFT JOIN psgc_city d ON a.psgc_city_id = d.id AND d.psgc_region_id = c.id
    LEFT JOIN system_user e ON a.referred_by_user_id = e.sync_id ";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_applicant
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, 	a.id
-- 		, CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') `applicant_name`
		, IFNULL(a.lastname,'''') `lastname`
        , IFNULL(a.firstname,'''') `firstname`
        , IFNULL(a.middlename,'''') `middlename`
        , IFNULL(a.suffix,'''') `suffix`
        , b.description `application_source`
        , IFNULL(a.mrf_transaction_id, '''') `mrf_transaction_id`
		, CASE WHEN IFNULL(a.current_step_code, '''') = '''' 
			THEN ''''
            ELSE a.current_step_code
		END `current_step`
        , IFNULL(DATE_FORMAT(a.date_scheduled, ''%m/%d/%Y''), '''') `date_scheduled`
        , IFNULL(DATE_FORMAT(a.date_completed, ''%m/%d/%Y''), '''') `date_completed`
        , a.approver_remarks
		, IFNULL(a.position_remarks,'''') `position_remarks`
		, a.course
		, a.current_position
		, IFNULL(a.expected_salary, 0) `expected_salary`
        , IFNULL(DATE_FORMAT(a.date_applied, ''%m/%d/%Y''), '''') `date_applied`
        , IFNULL(a.employee_id, 0) `employee_id`
        , IFNULL(a.psgc_region_code, '''') `scope_org_group_code`
		, IFNULL(CONCAT(c.code, '' - '', c.description),'''') `scope_org_group`
        , IFNULL(DATE_FORMAT(a.birthdate, ''%m/%d/%Y''), '''') `birthdate`
        , a.address_line_1
        , a.address_line_2
        -- , IFNULL(CONCAT(d.code, '' - '', d.description),'''') `city`
        , email
        , cellphone_number
        , CONCAT(e.lastname, '', '', e.firstname, '' '', IFNULL(e.middlename, ''''), '' '') `referred_by`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_applicant) a',
 		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_get_picker_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_get_picker_list`(
	IN param_selected_id_delimited VARCHAR(500),
    IN param_is_hired BOOLEAN,
	IN param_is_tagged_to_mrf BOOLEAN,
	IN param_id INT,
    IN param_last_name VARCHAR(255),
    IN param_first_name VARCHAR(255),
    IN param_middle_name VARCHAR(255),
    IN param_suffix VARCHAR(10),
    IN param_application_source_delimited VARCHAR(500),
    IN param_mrf_transaction_id VARCHAR(255),
    -- IN param_current_step_delimited VARCHAR(500),
    -- IN param_workflow_delimited VARCHAR(500),
    IN param_current_step_delimited VARCHAR(500),
    IN param_date_scheduled_from VARCHAR(10),
    IN param_date_scheduled_to VARCHAR(10),
    IN param_date_completed_from VARCHAR(10),
    IN param_date_completed_to VARCHAR(10),
    IN param_approver_remarks VARCHAR(500),
    -- IN param_current_step_delimited VARCHAR(500),
    -- IN param_workflow_delimited VARCHAR(500),
    IN param_position_remarks VARCHAR(255),
    IN param_course_delimited VARCHAR(500),
	IN param_current_position_title VARCHAR(255),    
    IN param_expected_salary_from DECIMAL(13, 2),
    IN param_expected_salary_to DECIMAL(13, 2),
	IN param_date_applied_from VARCHAR(10),
    IN param_date_applied_to VARCHAR(10),
    IN param_scope_org_type VARCHAR(50),
    IN param_scope_org_group_delimited VARCHAR(500),
    -- IN param_workflow_status_delimited VARCHAR(500),
    IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_is_hired VARCHAR(20);
    DECLARE local_param_is_tagged_to_mrf VARCHAR(20);
    DECLARE local_param_id VARCHAR(20);
	DECLARE local_param_selected_id_delimited VARCHAR(1000);
	DECLARE local_param_last_name VARCHAR(1000);
    DECLARE local_param_first_name VARCHAR(1000);
    DECLARE local_param_middle_name VARCHAR(1000);
    DECLARE local_param_suffix VARCHAR(1000);
    DECLARE local_param_application_source_delimited VARCHAR(1000);
    DECLARE local_param_mrf_transaction_id VARCHAR(1000);
    DECLARE local_param_current_step_delimited VARCHAR(1000);
    DECLARE local_param_date_scheduled_from VARCHAR(50);
	DECLARE local_param_date_scheduled_to VARCHAR(50);
	DECLARE local_param_date_completed_from VARCHAR(50);
	DECLARE local_param_date_completed_to VARCHAR(50);
	DECLARE local_param_approver_remarks VARCHAR(1000);
    -- DECLARE local_param_current_step_delimited VARCHAR(1000);
    -- DECLARE local_param_workflow_delimited VARCHAR(1000);
    DECLARE local_param_position_remarks VARCHAR(1000);
    DECLARE local_param_course_delimited VARCHAR(1000);
	DECLARE local_param_current_position_title VARCHAR(1000);    
    DECLARE local_param_expected_salary_from VARCHAR(50);
    DECLARE local_param_expected_salary_to VARCHAR(50);
    DECLARE local_param_date_applied_from VARCHAR(50);
    DECLARE local_param_date_applied_to VARCHAR(50);
    DECLARE local_param_scope_descendants_delimited VARCHAR(16383);
    -- DECLARE local_param_workflow_status_delimited VARCHAR(1000);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_is_hired = CONVERT(param_is_hired, CHAR(1));
    SET local_param_is_tagged_to_mrf = CONVERT(param_is_tagged_to_mrf, CHAR(1));
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_selected_id_delimited = CONVERT(param_selected_id_delimited, CHAR(1000));
	SET local_param_last_name = CASE WHEN param_last_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_last_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_first_name = CASE WHEN param_first_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_first_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_middle_name = CASE WHEN param_middle_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_middle_name,'\\','\\\\'), '''', ''''''), '''') END;
	SET local_param_suffix = CASE WHEN param_suffix = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_suffix,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_application_source_delimited = CASE WHEN param_application_source_delimited = '' THEN 'b.id' ELSE param_application_source_delimited END;
    
    SET local_param_mrf_transaction_id = CASE WHEN param_mrf_transaction_id = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_mrf_transaction_id,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_step_delimited = CASE WHEN param_current_step_delimited = '' THEN '' ELSE param_current_step_delimited END;
    
    SET local_param_date_scheduled_from = 
		CASE WHEN STR_TO_DATE(param_date_scheduled_from, '%m/%d/%Y') IS NULL OR param_date_scheduled_from = ''
			THEN 'IFNULL(CONVERT(a.date_scheduled, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_scheduled_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_scheduled_to = 
		CASE WHEN STR_TO_DATE(param_date_scheduled_to, '%m/%d/%Y') IS NULL OR param_date_scheduled_to = ''
			THEN 'IFNULL(CONVERT(a.date_scheduled, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_scheduled_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_date_completed_from = 
		CASE WHEN STR_TO_DATE(param_date_completed_from, '%m/%d/%Y') IS NULL OR param_date_completed_from = ''
			THEN 'IFNULL(CONVERT(a.date_completed, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_completed_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_completed_to = 
		CASE WHEN STR_TO_DATE(param_date_completed_to, '%m/%d/%Y') IS NULL OR param_date_completed_to = ''
			THEN 'IFNULL(CONVERT(a.date_completed, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_completed_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_approver_remarks = 
    CASE WHEN param_approver_remarks = '' 
		THEN ''''''
		ELSE CONCAT('''', REPLACE(REPLACE(param_approver_remarks,'\\','\\\\'), '''', ''''''), '''') 
    END;
    SET local_param_position_remarks = CASE WHEN param_position_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    
    SET local_param_course_delimited = CASE WHEN param_course_delimited = '' THEN '' ELSE param_course_delimited END;
	SET local_param_application_source_delimited = CASE WHEN param_application_source_delimited = '' THEN 'b.id' ELSE param_application_source_delimited END;
    SET local_param_current_position_title = CASE WHEN param_current_position_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_current_position_title,'\\','\\\\'), '''', ''''''), '''') END;   
    SET local_param_expected_salary_from = CASE WHEN param_expected_salary_from = 0 THEN 'IFNULL(a.expected_salary,0)' ELSE CONVERT(param_expected_salary_from,CHAR(20)) END;
    SET local_param_expected_salary_to = CASE WHEN param_expected_salary_to = 0 THEN 'IFNULL(a.expected_salary,0)' ELSE CONVERT(param_expected_salary_to,CHAR(20)) END;
    SET local_param_date_applied_from = 
		CASE WHEN STR_TO_DATE(param_date_applied_from, '%m/%d/%Y') IS NULL OR param_date_applied_from = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_applied_to = 
		CASE WHEN STR_TO_DATE(param_date_applied_to, '%m/%d/%Y') IS NULL OR param_date_applied_to = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_scope_descendants_delimited = CASE WHEN param_scope_org_group_delimited = '' THEN 'c.id' ELSE param_scope_org_group_delimited END;
        
	-- SET local_param_workflow_status_delimited = CASE WHEN param_workflow_status_delimited = '' THEN 'IFNULL(a.workflow_status, ''IN_PROGRESS'')' ELSE CONCAT('''', REPLACE(REPLACE(param_workflow_status_delimited,'\\','\\\\'),',',''','''), '''') END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR a.id = ', local_param_id
    ,' )'
--     ,' AND'
--     ,' ('
-- 		, local_param_applicant_name , '= '''''
-- 		, ' OR LOWER(CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '')) 
-- 				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant_name , '), '' '', ''%''), ''%'')'
--     ,' )'
	,' AND'
    ,' ('
		, local_param_last_name , '= '''''
		, ' OR IFNULL(LOWER(a.lastname),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_last_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_first_name , '= '''''
		, ' OR IFNULL(LOWER(a.firstname),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_first_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_middle_name , '= '''''
		, ' OR IFNULL(LOWER(a.middlename),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_middle_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_suffix , '= '''''
		, ' OR IFNULL(LOWER(a.suffix),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_suffix , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' b.id IN (', local_param_application_source_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_mrf_transaction_id , '= '''''
		, ' OR LOWER(IFNULL(a.failed_mrf_transaction_id, '''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_mrf_transaction_id , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_current_step_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_approver_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.approver_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_approver_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_scheduled, DATE),'''') BETWEEN ', local_param_date_scheduled_from, ' AND ', local_param_date_scheduled_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_completed, DATE),'''') BETWEEN ', local_param_date_completed_from, ' AND ', local_param_date_completed_to
    ,' )'
    -- ,' AND'
--     ,' ('
-- 		,''''' = ''', local_param_current_step_delimited, ''''
-- 		,' OR '
-- 		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
--     ,' )'
    -- ,' AND'
--     ,' ('
-- 		,' IFNULL(wf.id, 0) IN (', local_param_workflow_delimited, ')'
--     ,' )'
    ,' AND'
    ,' ('
		, local_param_position_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.position_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
    ,' ('
		,''''' = ''', local_param_course_delimited, ''''
 		,' OR '
		,' FIND_IN_SET(IFNULL(a.course,''''), ''', local_param_course_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_current_position_title , '= '''''
		, ' OR LOWER(a.current_position) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_current_position_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(a.expected_salary, 0) BETWEEN ', local_param_expected_salary_from, ' AND ', local_param_expected_salary_to
    ,' )'
,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_applied, DATE),'''') BETWEEN ', local_param_date_applied_from, ' AND ', local_param_date_applied_to
    ,' )'
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    ,' AND'
    ,' ('
		,' (CASE WHEN a.date_hired IS NOT NULL THEN 1 ELSE 0 END) = ', local_param_is_hired
    ,' )'
   ,' AND'
    ,' ('
		,' (CASE WHEN IFNULL(a.failed_mrf_transaction_id, '''') = '''' THEN 0 ELSE 1 END) = ', local_param_is_tagged_to_mrf
		,' OR'
		,' (CASE WHEN IFNULL(a.failed_mrf_transaction_id, '''') = '''' THEN 0 ELSE 1 END) = 0'
    ,' )'
   ,' AND'
    ,' ('
		,' IFNULL(mrf_transaction_id, '''') = '''''
	,' )'
    ,' AND'
    ,' ('
		,' c.id IN (', local_param_scope_descendants_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' a.id NOT IN (', local_param_selected_id_delimited, ')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('a.id ', local_sord, ',') END, ''), 
-- 	IFNULL(CASE WHEN sidx = 'ApplicantName'			THEN CONCAT('CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'LastName'				THEN CONCAT('IFNULL(a.lastname,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'FirstName'				THEN CONCAT('IFNULL(a.firstname,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'MiddleName'			THEN CONCAT('IFNULL(a.middlename,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Suffix'				THEN CONCAT('IFNULL(a.suffix,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'MRFTransactionID'		THEN CONCAT('a.failed_mrf_transaction_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicationSource'		THEN CONCAT('b.description ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('a.current_step_code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateScheduled'				THEN CONCAT('IFNULL(a.date_scheduled, '''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCompleted'				THEN CONCAT('IFNULL(a.date_completed, '''') ', local_sord, ',') END, ''),
	-- IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('wfs.description ', local_sord, ',') END, ''), 
	-- IFNULL(CASE WHEN sidx = 'WorkflowDescription'	THEN CONCAT('CONCAT(wf.code, '' - '', wf.description) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'OrgGroupRemarks'		THEN CONCAT('IFNULL(a.org_group_remarks,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DesiredOrgGroup'		THEN CONCAT('IFNULL(CONCAT(og.code, '' - '',og.description), '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionRemarks'		THEN CONCAT('IFNULL(a.position_remarks,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DesiredPosition'		THEN CONCAT('IFNULL(CONCAT(pos.code, '' - '',pos.title), '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Course'				THEN CONCAT('ref_course.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentPosition'		THEN CONCAT('a.current_position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ExpectedSalary'		THEN CONCAT('a.expected_salary ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateApplied'			THEN CONCAT('a.date_applied ', local_sord, ',') END, '')
);
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
    
    SET joined_tables = "applicant a
	INNER JOIN reference_value b ON a.application_source = b.value 
		AND b.ref_code = ""APPLICATION_SOURCE""
	LEFT JOIN reference_value ref_course ON a.course = ref_course.value 
		AND ref_course.ref_code = ""COURSE""
	-- LEFT JOIN reference_value ref ON IFNULL(a.workflow_status, ""IN_PROGRESS"") = ref.value 
-- 		AND ref.ref_code = ""WORKFLOW_STATUS""
	LEFT JOIN psgc_region c ON a.psgc_region_code = c.code
    -- LEFT JOIN psgc_city d ON a.psgc_city_id = d.id AND d.psgc_region_id = c.id
    LEFT JOIN system_user e ON a.referred_by_user_id = e.sync_id ";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_applicant
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, 	a.id
-- 		, CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') `applicant_name`
		, IFNULL(a.lastname,'''') `lastname`
        , IFNULL(a.firstname,'''') `firstname`
        , IFNULL(a.middlename,'''') `middlename`
        , IFNULL(a.suffix,'''') `suffix`
		, b.description `application_source`
        , IFNULL(a.failed_mrf_transaction_id, '''') `mrf_transaction_id`
		, CASE WHEN IFNULL(a.current_step_code, '''') = '''' 
			THEN ''''
            ELSE a.current_step_code
		END `current_step`
        , IFNULL(DATE_FORMAT(a.date_scheduled, ''%m/%d/%Y''), '''') `date_scheduled`
        , IFNULL(DATE_FORMAT(a.date_completed, ''%m/%d/%Y''), '''') `date_completed`
        , a.approver_remarks
		, IFNULL(a.position_remarks,'''') `position_remarks`
		, IFNULL(ref_course.description, '''') `course`
		, a.current_position
		, IFNULL(a.expected_salary, 0.00) `expected_salary`
        , IFNULL(DATE_FORMAT(a.date_applied, ''%m/%d/%Y''), '''') `date_applied`
        , IFNULL(a.employee_id, 0) `employee_id`
        , IFNULL(c.id, 0) `scope_org_group_id`
		, IFNULL(CONCAT(c.code, '' - '', c.description),'''') `scope_org_group`
        , IFNULL(DATE_FORMAT(a.birthdate, ''%m/%d/%Y''), '''') `birthdate`
        , a.address_line_1
        , a.address_line_2
        -- , IFNULL(CONCAT(d.code, '' - '', d.description),'''') `city`
        , email
        , cellphone_number
        , CONCAT(e.lastname, '', '', e.firstname, '' '', IFNULL(e.middlename, ''''), '' '') `referred_by`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_applicant) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');

    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_name_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_name_autocomplete`(
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
    SELECT DISTINCT `id`
	    , CONCAT(lastname, '', '', firstname, '' '', middlename) `applicant_name`
	FROM applicant
    WHERE LOWER(CONCAT(lastname, '', '', firstname, '' '', middlename)) LIKE LOWER(CONCAT(''%'',', local_term, ',''%''))
	ORDER BY CONCAT(lastname, '', '', firstname, '' '', middlename)',
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_tagging_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_tagging_get_list`(
	IN param_id INT,
    IN param_applicant_name VARCHAR(255),
    IN param_application_source_delimited VARCHAR(500),
    IN param_position_remarks VARCHAR(500),
    IN param_referred_by_delimited VARCHAR(500),
    IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_id VARCHAR(20);
	DECLARE local_param_applicant_name VARCHAR(1000);
    DECLARE local_param_application_source_delimited VARCHAR(1000);
    DECLARE local_param_position_remarks VARCHAR(1000);
    DECLARE local_param_referred_by_delimited VARCHAR(1000);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
	SET local_param_applicant_name = CASE WHEN param_applicant_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_applicant_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_application_source_delimited = CASE WHEN param_application_source_delimited = '' THEN 'b.id' ELSE param_application_source_delimited END;
    SET local_param_position_remarks = CASE WHEN param_position_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_referred_by_delimited = CASE WHEN param_referred_by_delimited = '' THEN 'a.referred_by_user_id' ELSE param_referred_by_delimited END;
    
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
		, local_param_applicant_name , '= '''''
		, ' OR LOWER(CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' b.id IN (', local_param_application_source_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_position_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.position_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' a.referred_by_user_id IN (', local_param_referred_by_delimited, ')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					  THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'				  THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicantName'		  THEN CONCAT('CONCAT(a.lastname, '', '', a.firstname, '' '', a.middlename) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicationSource'	  THEN CONCAT('b.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionRemarks'	  THEN CONCAT('IFNULL(a.position_remarks,'''') ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'ReferredBy'	      THEN CONCAT('CONCAT(e.lastname, '', '', e.firstname, '' '', e.middlename) ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_applicant_tagging; 
    
    SET joined_tables = "applicant a
	INNER JOIN reference_value b ON a.application_source = b.value 
		AND b.ref_code = ""APPLICATION_SOURCE""
    LEFT JOIN system_user e on a.referred_by_user_id = e.sync_id ";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_applicant_tagging
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
	   , a.id
       , CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', a.middlename, '' '') `applicant_name`
	   , b.description `application_source`
       , a.position_remarks
       , CONCAT(e.lastname, '', '', e.firstname, '' '', e.middlename) as `referred_by`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_applicant_tagging) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_applicant_tagging; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_application_approval_add_transaction` */;
ALTER DATABASE `clean_ems_recruitment` CHARACTER SET latin1 COLLATE latin1_bin ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_application_approval_add_transaction`(
	IN param_approver_id INT,
    IN param_record_id INT,
    IN param_result VARCHAR(50),
    IN param_remarks VARCHAR(500)
)
BEGIN
    DECLARE param_request_type VARCHAR(20);
    DECLARE previous_step_order INT;
    
    SET param_request_type = 'APPLICATION_PROCESS';
    
    IF (SELECT COUNT(*) FROM workflow_transaction WHERE request_type = param_request_type AND record_id = param_record_id) = 0
    THEN
        INSERT INTO workflow_transaction
		(`workflow_code`,
		`request_type`,
		`step_code`,
		`step_order`,
		`max_step_order`,
		`record_id`,
		`result`,
        `start_date_time`,
        `end_date_time`,
		`approved_by`,
        `approver_remarks`)
        SELECT 
			b.code
			, param_request_type
			, c.code
			, c.order
			, max_step_order
			, param_record_id
			, param_result
			, a.created_date
            , CURRENT_TIMESTAMP
            , param_approver_id
            , param_remarks
		FROM applicant a
		INNER JOIN workflow b ON a.workflow_id = b.id
		INNER JOIN workflow_step c ON c.workflow_id = b.id AND c.order = 1
		INNER JOIN 
        (
			SELECT workflow_id 
				, max(`order`) `max_step_order` 
            FROM workflow_step 
            GROUP BY workflow_id
		) d ON b.id = d.workflow_id
        WHERE a.id = param_record_id;
        
    ELSE
		INSERT INTO workflow_transaction
		(`workflow_code`,
		`request_type`,
		`step_code`,
		`step_order`,
		`max_step_order`,
		`record_id`,
		`result`,
        `start_date_time`,
        `end_date_time`,
		`approved_by`,
        `approver_remarks`)
		SELECT 
			b.code
			, param_request_type
			, ws_nxt.code
			, ws_nxt.order
			, f.max_step_order
			, param_record_id
			, param_result
            , CASE WHEN g.end_date_time IS NOT NULL 
				THEN g.end_date_time
				ELSE a.created_date
			END
			, CURRENT_TIMESTAMP
			, param_approver_id
            , param_remarks
		FROM applicant a
		INNER JOIN workflow b ON a.workflow_id = b.id
		INNER JOIN workflow_step c ON c.workflow_id = b.id
        LEFT JOIN workflow_step ws_nxt ON c.workflow_id = ws_nxt.workflow_id
			AND ws_nxt.order = c.order + 1
		INNER JOIN (SELECT * FROM workflow_step_approver GROUP BY workflow_id, step_code) d ON c.code = d.step_code
		INNER JOIN 
		(
			SELECT 
				workflow_code
				, request_type
				, record_id
				, max_step_order
				, max(step_order) `current_step`
			FROM workflow_transaction 
			GROUP BY workflow_code, request_type, record_id, max_step_order
		) f ON b.code = f.workflow_code 
			AND f.request_type = param_request_type
			AND c.order = f.current_step
			AND a.id = f.record_id
		LEFT JOIN workflow_transaction g ON b.code = g.workflow_code 
			AND g.request_type = param_request_type
			AND c.order = g.step_order
			AND a.id = g.record_id
		WHERE a.id = param_record_id;
	
		/*Update modified by and date hired of applicant when record is approved by final approver*/
		IF(
			SELECT 
				CASE WHEN COUNT(*) = max_step_order 
					THEN TRUE 
					ELSE FALSE 
				END 
			FROM workflow_transaction WHERE request_type = param_request_type AND record_id = param_record_id
		)
		THEN
				UPDATE applicant 
					SET is_hired = true,
					date_hired = CURRENT_TIMESTAMP
				WHERE id = param_record_id;
		END IF;
        
    END IF;

		SELECT
			0 ID
            , 0 `workflow_id`
            , '' `step_code`
            , 0 `role_id`
		FROM dual
        WHERE 1=0;
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
ALTER DATABASE `clean_ems_recruitment` CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_application_approval_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_application_approval_get_list`(
	IN param_role_delimited VARCHAR(500),
    IN param_id INT,
    IN param_applicant_name VARCHAR(255),
    IN param_application_source_delimited VARCHAR(500),
    -- IN param_current_step_delimited VARCHAR(500),
    -- IN param_workflow_delimited VARCHAR(500),
    IN param_position_remarks VARCHAR(255),
    IN param_course VARCHAR(255),
	IN param_current_position_title VARCHAR(255),    
    IN param_expected_salary_from DECIMAL(13, 2),
    IN param_expected_salary_to DECIMAL(13, 2),
	IN param_date_applied_from VARCHAR(10),
    IN param_date_applied_to VARCHAR(10),
    IN sidx VARCHAR(255),
    IN sord VARCHAR(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_role_delimited VARCHAR(1000);
    DECLARE local_param_id VARCHAR(20);
	DECLARE local_param_applicant_name VARCHAR(1000);
    DECLARE local_param_application_source_delimited VARCHAR(1000);
    -- DECLARE local_param_current_step_delimited VARCHAR(1000);
    -- DECLARE local_param_workflow_delimited VARCHAR(1000);
    DECLARE local_param_position_remarks VARCHAR(1000);
    DECLARE local_param_course VARCHAR(1000);
	DECLARE local_param_current_position_title VARCHAR(1000);    
    DECLARE local_param_expected_salary_from VARCHAR(50);
    DECLARE local_param_expected_salary_to VARCHAR(50);
    DECLARE local_param_date_applied_from VARCHAR(50);
    DECLARE local_param_date_applied_to VARCHAR(50);

    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_role_delimited = 
		CASE WHEN param_role_delimited = '' 
			THEN 'REPLACE(''a.current_step_approver_role_ids'','','',''|'')' 
            ELSE REPLACE(param_role_delimited,',','|') 
		END;
    SET local_param_id = CONVERT(param_id, CHAR(20));
	SET local_param_applicant_name = CASE WHEN param_applicant_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_applicant_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_application_source_delimited = CASE WHEN param_application_source_delimited = '' THEN 'b.id' ELSE param_application_source_delimited END;
    -- SET local_param_current_step_delimited = CASE WHEN param_current_step_delimited = '' THEN '' ELSE param_current_step_delimited END;
    -- SET local_param_workflow_delimited = CASE WHEN param_workflow_delimited = '' THEN 'IFNULL(wf.id, 0)' ELSE param_workflow_delimited END;
    SET local_param_position_remarks = CASE WHEN param_position_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_position_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_course = CASE WHEN param_course = '' THEN '''''' ELSE CONCAT('''', REPLACE(param_course, '''', ''''''), '''') END;
	SET local_param_current_position_title = CASE WHEN param_current_position_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_current_position_title,'\\','\\\\'), '''', ''''''), '''') END;   
    SET local_param_expected_salary_from = CASE WHEN param_expected_salary_from = 0 THEN 'a.expected_salary' ELSE CONVERT(param_expected_salary_from,CHAR(20)) END;
    SET local_param_expected_salary_to = CASE WHEN param_expected_salary_to = 0 THEN 'a.expected_salary' ELSE CONVERT(param_expected_salary_to,CHAR(20)) END;
    SET local_param_date_applied_from = 
		CASE WHEN STR_TO_DATE(param_date_applied_from, '%m/%d/%Y') IS NULL OR param_date_applied_from = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_applied_to = 
		CASE WHEN STR_TO_DATE(param_date_applied_to, '%m/%d/%Y') IS NULL OR param_date_applied_to = ''
			THEN 'IFNULL(CONVERT(a.date_applied, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_applied_to, ''',''%m/%d/%Y''), '''')')  
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
		, local_param_applicant_name , '= '''''
		, ' OR LOWER(CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '')) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' b.id IN (', local_param_application_source_delimited, ')'
    ,' )'
   --  ,' AND'
--    ,' ('
-- 		,''''' = ''', local_param_current_step_delimited, ''''
-- 		,' OR '
-- 		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
--     ,' )'
   --  ,' AND'
--     ,' ('
-- 		,' IFNULL(wf.id, 0) IN (', local_param_workflow_delimited, ')'
--     ,' )'
    ,' AND'
    ,' ('
		, local_param_position_remarks , '= '''''
		, ' OR IFNULL(LOWER(a.position_remarks),'''') LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_position_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_course , '= '''''
		, ' OR LOWER(a.course) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_course , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_current_position_title , '= '''''
		, ' OR LOWER(a.current_position) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_current_position_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(a.expected_salary, 0) BETWEEN ', local_param_expected_salary_from, ' AND ', local_param_expected_salary_to
    ,' )'
	,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.date_applied, DATE),'''') BETWEEN ', local_param_date_applied_from, ' AND ', local_param_date_applied_to
    ,' )'
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    ,' AND'
    ,' ('
		,' a.current_step_approver_role_ids REGEXP CONCAT(''(^|,)(',local_param_role_delimited,')(,|$)'')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicantName'			THEN CONCAT('CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicationSource'		THEN CONCAT('b.description ', local_sord, ',') END, ''), 
	-- IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('wfs.description ', local_sord, ',') END, ''), 
	-- IFNULL(CASE WHEN sidx = 'WorkflowDescription'	THEN CONCAT('CONCAT(wf.code, '' - '', wf.description) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionRemarks'		THEN CONCAT('IFNULL(a.position_remarks,'''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Course'				THEN CONCAT('a.course ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentPosition'		THEN CONCAT('a.current_position ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ExpectedSalary'		THEN CONCAT('a.expected_salary ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateApplied'			THEN CONCAT('a.date_applied ', local_sord, ',') END, '')
);
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
    
    SET joined_tables = CONCAT("applicant a
	INNER JOIN reference_value b ON a.application_source = b.value 
		AND b.ref_code = ""APPLICATION_SOURCE"" ");
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_applicant
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, 	a.id
		, 	a.workflow_id
		, CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') `applicant_name`
		, b.description `application_source`
		-- , CASE WHEN IFNULL(a.current_step_code, '''') = '''' 
-- 			THEN ''''
--             ELSE CONCAT(a.current_step_code, '' - '', a.current_step_description) 
-- 		END `current_step`
		, IFNULL(a.position_remarks,'''') `position_remarks`
		, a.course
		, a.current_position
		, IFNULL(a.expected_salary, 0) `expected_salary`
		, IFNULL(DATE_FORMAT(a.date_applied, ''%m/%d/%Y''), '''') `date_applied`
        -- , CASE WHEN 
-- 			(
-- 				has_approval.id IS NOT NULL 
-- 				AND wft_result.result NOT IN (SELECT value FROM reference_value WHERE ref_code = ''NEGATIVE_RESULT_TYPE'')
-- 				AND wft.max_step_order != wft.latest_step_order 
-- 			) OR wft.record_id IS NULL
-- 				THEN 1
-- 				ELSE 0
-- 		END `has_approval`
		, CASE WHEN IFNULL(a.workflow_status, ''IN_PROGRESS'') = ''IN_PROGRESS'' 
			THEN true 
            ELSE false
		END `has_approval`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_applicant) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_applicant; 
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
		, INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
        , description
        , is_active
        , org_type
        , parent_org_id
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_by_org_type_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_by_org_type_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT,
    IN org_type VARCHAR(50)
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
		id
		, sync_id
		, sync_date
        , code
		, INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
        , description
        , parent_org_id
        , is_active
        , org_type
	FROM org_group
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') 
		AND is_active = 1
        AND org_type = ''', org_type ,'''
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_pending_task_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_pending_task_get_list`(
	IN param_recruiter_id INT,
	IN param_id INT,
    IN param_applicant varchar(255),
    IN param_description varchar(255),
    IN param_status_delimited varchar(500),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
    IN param_date_modified_from VARCHAR(10),
    IN param_date_modified_to VARCHAR(10),
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_recruiter_id VARCHAR(20);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_applicant VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_status_delimited VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_param_date_modified_from VARCHAR(50);
    DECLARE local_param_date_modified_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_recruiter_id = CONVERT(param_recruiter_id, CHAR(20));
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_applicant = CASE WHEN param_applicant = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_applicant,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'd.id' ELSE param_status_delimited END;
    
    SET local_param_date_created_from = 
		CASE WHEN STR_TO_DATE(param_date_created_from, '%m/%d/%Y') IS NULL OR param_date_created_from = ''
			THEN 'IFNULL(CONVERT(a.created_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_from, ''',''%m/%d/%Y''), '''')')  
        END;
        
    SET local_param_date_created_to = 
		CASE WHEN STR_TO_DATE(param_date_created_to, '%m/%d/%Y') IS NULL OR param_date_created_to = ''
			THEN 'IFNULL(CONVERT(a.created_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_to, ''',''%m/%d/%Y''), '''')')  
		END;
    
    SET local_param_date_modified_from = 
		CASE WHEN STR_TO_DATE(param_date_modified_from, '%m/%d/%Y') IS NULL OR param_date_modified_from = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_modified_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_modified_to = 
		CASE WHEN STR_TO_DATE(param_date_modified_to, '%m/%d/%Y') IS NULL OR param_date_modified_to = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_modified_to, ''',''%m/%d/%Y''), '''')')  
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
		, local_param_applicant , '= '''''
		, ' OR LOWER(CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(a.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description, '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' d.id IN (', local_param_status_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.modified_date, DATE),'''') BETWEEN ', local_param_date_modified_from, ' AND ', local_param_date_modified_to
    ,' )'
    ,' AND'
    ,' ('
		,' a.recruiter_user_id = ', local_param_recruiter_id
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('a.id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Applicant'	    THEN CONCAT('CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Description'	THEN CONCAT('a.description ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Status'		THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CreatedDate'	THEN CONCAT('DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ModifiedDate'	THEN CONCAT('DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_pending_task; 
    
    SET joined_tables = "recruiter_task a
	INNER JOIN applicant c ON a.applicant_id = c.id
    INNER JOIN reference_value d ON a.status = d.value AND d.ref_code = ""TASK_STATUS""  ";
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_pending_task
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, a.id
        , CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename) `applicant`
        , a.description
        , d.description `status`
        , a.created_by
        , DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') `created_date`
        , a.modified_by
        , DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r'') `modified_date`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_pending_task) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_pending_task; 
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
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
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
    SELECT DISTINCT `id`
		, sync_id
        , sync_date
        , INSTR(CONCAT(code, '' - '', title), ', local_term, ') `index`
        , code
        , position_level_id
        , parent_position_id
        , title
        , is_active
	FROM position
	WHERE LOWER(CONCAT(code, '' - '', title)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
    AND INSTR(CONCAT(code, '' - '', title), ', local_term, ') > 0
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_psgc_region_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_psgc_region_autocomplete`(
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
        , code
        , prefix
		, INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
        , description
        , created_by
        , created_date
	FROM psgc_region
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') 
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_recruiter_task_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_recruiter_task_get_list`(
	IN param_user_id INT,
	IN param_id INT,
    IN param_recruiter varchar(255),
    IN param_applicant varchar(255),
    IN param_description varchar(255),
    IN param_status_delimited varchar(500),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
    IN param_date_modified_from VARCHAR(10),
    IN param_date_modified_to VARCHAR(10),
    IN sidx varchar(255),
    IN sord varchar(255),
    IN row_start INT,
    IN param_rows INT
)
BEGIN
	-- Define variables for query portions 
    DECLARE where_clause VARCHAR(16383); 
    DECLARE order_by_clause VARCHAR(16383);
    DECLARE joined_tables VARCHAR(16383);
    
    -- Define parameters for conversion to string
    DECLARE local_param_user_id VARCHAR(20);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_recruiter VARCHAR(1000);
    DECLARE local_param_applicant VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_status_delimited VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_param_date_modified_from VARCHAR(50);
    DECLARE local_param_date_modified_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_user_id = CONVERT(param_user_id, CHAR(20));
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_recruiter = CASE WHEN param_recruiter = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_recruiter,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_applicant = CASE WHEN param_applicant = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_applicant,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'd.id' ELSE param_status_delimited END;
    
    SET local_param_date_created_from = 
		CASE WHEN STR_TO_DATE(param_date_created_from, '%m/%d/%Y') IS NULL OR param_date_created_from = ''
			THEN 'IFNULL(CONVERT(a.created_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_from, ''',''%m/%d/%Y''), '''')')  
        END;
        
    SET local_param_date_created_to = 
		CASE WHEN STR_TO_DATE(param_date_created_to, '%m/%d/%Y') IS NULL OR param_date_created_to = ''
			THEN 'IFNULL(CONVERT(a.created_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_to, ''',''%m/%d/%Y''), '''')')  
		END;
    
    SET local_param_date_modified_from = 
		CASE WHEN STR_TO_DATE(param_date_modified_from, '%m/%d/%Y') IS NULL OR param_date_modified_from = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_modified_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_modified_to = 
		CASE WHEN STR_TO_DATE(param_date_modified_to, '%m/%d/%Y') IS NULL OR param_date_modified_to = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_modified_to, ''',''%m/%d/%Y''), '''')')  
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
		, local_param_recruiter , '= '''''
		, ' OR LOWER(CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_recruiter , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_applicant , '= '''''
		, ' OR LOWER(CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_applicant , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(a.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description, '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' d.id IN (', local_param_status_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.modified_date, DATE),'''') BETWEEN ', local_param_date_modified_from, ' AND ', local_param_date_modified_to
    ,' )'
    ,' AND'
    ,' ('
		,' a.created_by = ', local_param_user_id
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('a.id ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Recruiter'		THEN CONCAT('CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Applicant'	    THEN CONCAT('CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename) ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Description'	THEN CONCAT('a.description ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Status'		THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CreatedDate'	THEN CONCAT('DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ModifiedDate'	THEN CONCAT('DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_recruiter_task; 
    
    SET joined_tables = "recruiter_task a
	INNER JOIN system_user b ON a.recruiter_user_id = b.sync_id
	INNER JOIN applicant c ON a.applicant_id = c.id
    INNER JOIN reference_value d ON a.status = d.value AND d.ref_code = ""TASK_STATUS""  ";
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_recruiter_task
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, a.id
        , CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename) `recruiter`
        , CONCAT(c.lastname, '', '', c.firstname, '' '', c.middlename) `applicant`
        , a.description
        , d.description `status`
        , a.created_by
        , DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') `created_date`
        , a.modified_by
        , DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r'') `modified_date`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_recruiter_task) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_recruiter_task; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_user_name_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_user_name_autocomplete`(
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
    SELECT DISTINCT sync_id `id`
		, CONCAT(lastname, '', '', firstname, '' '', IFNULL(middlename,''''), '' ('', username, '')'') `name`
        , INSTR(LOWER(CONCAT(lastname, '', '', firstname, '' '', IFNULL(middlename,''''))), LOWER(', local_term ,')) `index`
	FROM system_user
    WHERE is_active = 1 
    AND LOWER(CONCAT(lastname, '', '', firstname, '' '', IFNULL(middlename,''''), '' ('', username, '')'')) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
    AND INSTR(LOWER(CONCAT(lastname, '', '', firstname, '' '', IFNULL(middlename,''''), '' ('', username, '')'')), LOWER(', local_term ,')) > 0
	ORDER BY `index`, CONCAT(lastname, '', '', firstname, '' '', IFNULL(middlename,''''))',
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_autocomplete`(
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
		, module
		, code
		, description
        , INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
		, created_by
		, created_date
		, modified_by
		, modified_date
	FROM workflow
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_list`(
	IN param_id INT,
    IN param_code varchar(50),
    IN param_description varchar(50),
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
    DECLARE local_param_code VARCHAR(100);
    DECLARE local_param_description VARCHAR(300);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_code = CASE WHEN param_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_code,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_id , '= 0'
		, ' OR id = ', local_param_id
    ,' )'
    ,' AND'
    ,' ('
		, local_param_code , '= '''''
		, ' OR LOWER(code) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_code , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('id ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Code'			THEN CONCAT('code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'	THEN CONCAT('description ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_workflow; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_workflow
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM workflow a ', where_clause, ') x) `total_num`
		, id
        , code
        , description
        , created_by
        , created_date
        , modified_by
        , modified_date
	FROM workflow a '
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_workflow) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_workflow; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_step_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_step_autocomplete`(
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
		, workflow_id
        , INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
		, code
		, description
		, previous_step_code
		, is_required
		, tat_days
		, allow_backflow
		, result_type
		, `order`
	FROM workflow_step
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
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

--
-- Final view structure for view `tv_applicant`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `scope_org_group`,'' AS `lastname`,'' AS `firstname`,'' AS `middlename`,'' AS `suffix`,'' AS `application_source`,'' AS `mrf_transaction_id`,'' AS `workflow_status`,'' AS `current_step`,'' AS `date_scheduled`,'' AS `date_completed`,'' AS `approver_remarks`,'' AS `workflow_description`,'' AS `position_remarks`,'' AS `course`,'' AS `current_position`,0.00 AS `exptected_salary`,'' AS `date_applied`,0 AS `employee_id`,'' AS `birthdate`,'' AS `address_line_1`,'' AS `address_line_2`,'' AS `city`,'' AS `email`,'' AS `cellphone_number`,'' AS `referred_by` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_count_by_org_group`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_count_by_org_group`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_count_by_org_group` AS select '' AS `org_group`,'' AS `position_title`,0 AS `applicant_count` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_history`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_history` AS select 0 AS `id`,0 AS `order`,'' AS `step`,'' AS `status`,'' AS `timestamp`,'' AS `remarks`,'' AS `result_type` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_name`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_name`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_name` AS select 0 AS `id`,'' AS `applicant_name` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_tagging`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_tagging`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_tagging` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `applicant_name`,'' AS `application_source`,'' AS `position_remarks`,'' AS `desired_position`,'' AS `org_group_remarks`,'' AS `desired_org_group`,'' AS `referred_by_remarks`,'' AS `referred_by` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_application_approval`
--

/*!50001 DROP VIEW IF EXISTS `tv_application_approval`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_application_approval` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `applicant_name`,'' AS `application_source`,'' AS `current_step`,'' AS `workflow_description`,'' AS `org_group_remarks`,'' AS `desired_org_group`,'' AS `position_remarks`,'' AS `desired_position`,'' AS `course`,'' AS `current_position`,0.00 AS `exptected_salary`,0 AS `has_approval`,'' AS `date_applied` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_existing_applicant`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_existing_applicant`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_existing_applicant` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `applicant_name`,'' AS `current_step`,'' AS `status` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_pending_task`
--

/*!50001 DROP VIEW IF EXISTS `tv_pending_task`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_pending_task` AS select 0 AS `id`,0 AS `total_num`,0 AS `row_num`,'' AS `applicant`,'' AS `description`,'' AS `status`,'' AS `created_date`,'' AS `modified_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_recruiter_task`
--

/*!50001 DROP VIEW IF EXISTS `tv_recruiter_task`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_recruiter_task` AS select 0 AS `id`,0 AS `total_num`,0 AS `row_num`,'' AS `recruiter`,'' AS `applicant`,'' AS `description`,'' AS `status`,'' AS `created_date`,'' AS `modified_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_system_user_name`
--

/*!50001 DROP VIEW IF EXISTS `tv_system_user_name`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_system_user_name` AS select 0 AS `id`,'' AS `name` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_workflow`
--

/*!50001 DROP VIEW IF EXISTS `tv_workflow`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_workflow` AS select 0 AS `id`,'' AS `code`,'' AS `description`,0 AS `created_by`,'' AS `created_date`,0 AS `modified_by`,'' AS `modified_date` from DUAL  where (1 = 0) */;
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

-- Dump completed on 2021-04-06 17:59:31
