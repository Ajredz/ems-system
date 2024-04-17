CREATE DATABASE  IF NOT EXISTS `clean_ems_manpower` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_bin */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clean_ems_manpower`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: clean_ems_manpower
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
-- Table structure for table `mrf`
--

DROP TABLE IF EXISTS `mrf`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mrf_transaction_id` varchar(70) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `old_mrf_id` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_id` int(11) NOT NULL,
  `position_id` int(11) NOT NULL,
  `is_confidential` bit(1) DEFAULT b'0',
  `nature_of_employment` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `purpose` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `vacancy` int(11) NOT NULL,
  `turnaround_time` int(11) NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `reason_for_cancellation` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `status` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `company_id` smallint(6) NOT NULL,
  `approved_by` int(11) DEFAULT NULL,
  `approved_date` datetime DEFAULT NULL,
  `approver_position_id` int(11) NOT NULL,
  `approver_org_group_id` int(11) NOT NULL,
  `alt_approver_position_id` int(11) NOT NULL DEFAULT '0',
  `alt_approver_org_group_id` int(11) NOT NULL DEFAULT '0',
  `temp_approver_id` int(11) NOT NULL DEFAULT '0',
  `approval_level` int(11) NOT NULL DEFAULT '0',
  `max_approval_level` int(11) NOT NULL DEFAULT '0',
  `cancelled_by` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_nprf` (`company_id`,`mrf_transaction_id`,`position_id`,`nature_of_employment`,`purpose`),
  UNIQUE KEY `mrf_transaction_id_UNIQUE` (`mrf_transaction_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf`
--

LOCK TABLES `mrf` WRITE;
/*!40000 ALTER TABLE `mrf` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_applicant`
--

DROP TABLE IF EXISTS `mrf_applicant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_applicant` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `mrf_id` int(11) NOT NULL,
  `applicant_id` int(11) NOT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `suffix` varchar(10) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `workflow_id` int(11) DEFAULT NULL,
  `current_step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `current_step_description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `workflow_status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `current_step_approver_role_ids` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `approver_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `date_completed` date DEFAULT NULL,
  `date_scheduled` date DEFAULT NULL,
  `current_result` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `result_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `for_hiring` bit(1) NOT NULL DEFAULT b'0',
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_nprf_approver` (`mrf_id`,`applicant_id`,`is_active`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_applicant`
--

LOCK TABLES `mrf_applicant` WRITE;
/*!40000 ALTER TABLE `mrf_applicant` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_applicant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_applicant_comments`
--

DROP TABLE IF EXISTS `mrf_applicant_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_applicant_comments` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `mrf_id` int(11) NOT NULL,
  `comments` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_applicant_comments`
--

LOCK TABLES `mrf_applicant_comments` WRITE;
/*!40000 ALTER TABLE `mrf_applicant_comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_applicant_comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_approval_history`
--

DROP TABLE IF EXISTS `mrf_approval_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_approval_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `mrf_id` bigint(20) NOT NULL,
  `result` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `level_of_approval` int(11) NOT NULL,
  `approver_id` int(11) NOT NULL,
  `alternate_approver_id` int(11) DEFAULT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `timestamp` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_approval_history`
--

LOCK TABLES `mrf_approval_history` WRITE;
/*!40000 ALTER TABLE `mrf_approval_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_approval_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_comments`
--

DROP TABLE IF EXISTS `mrf_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_comments` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `mrf_id` int(11) NOT NULL,
  `comments` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_comments`
--

LOCK TABLES `mrf_comments` WRITE;
/*!40000 ALTER TABLE `mrf_comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_defined_approver`
--

DROP TABLE IF EXISTS `mrf_defined_approver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_defined_approver` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hierarchy_level` int(11) NOT NULL,
  `requesting_position_id` int(11) NOT NULL,
  `requesting_org_group_id` int(11) NOT NULL,
  `approver_position_id` int(11) NOT NULL,
  `approver_org_group_id` int(11) NOT NULL,
  `alt_approver_position_id` int(11) DEFAULT NULL,
  `alt_approver_org_group_id` int(11) DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT '0',
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_defined_approver`
--

LOCK TABLES `mrf_defined_approver` WRITE;
/*!40000 ALTER TABLE `mrf_defined_approver` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_defined_approver` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mrf_signatories`
--

DROP TABLE IF EXISTS `mrf_signatories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mrf_signatories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `requester_id` int(11) NOT NULL,
  `position_id` int(11) NOT NULL,
  `workflow_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_mrf_signatories` (`requester_id`,`position_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mrf_signatories`
--

LOCK TABLES `mrf_signatories` WRITE;
/*!40000 ALTER TABLE `mrf_signatories` DISABLE KEYS */;
/*!40000 ALTER TABLE `mrf_signatories` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
INSERT INTO `reference` VALUES (1,'SYSTEM_MODULE','EMS list of modules',_binary '\0',1,'2019-11-22 19:36:44'),(7,'APPROVE_REJECT','Result Type - Approve or Reject options',_binary '\0',1,'2019-11-20 11:38:19'),(8,'APPROVE_REJECT_SKIP','Result Type - Approve, Reject or Skip options',_binary '\0',1,'2019-11-20 11:38:19'),(9,'PASS_FAIL','Result Type - Pass or Fail options',_binary '\0',1,'2019-11-20 11:38:19'),(10,'PASS_FAIL_SKIP','Result Type - Pass, Fail or Skip options',_binary '\0',1,'2019-11-20 11:38:19'),(11,'RESULT_TYPE','Sets of responses for every step',_binary '\0',1,'2019-11-20 11:38:19'),(12,'NATURE_OF_EMPLOYMENT','Nature of Employment',_binary '',1,'2019-11-22 15:58:49'),(13,'MRF_PURPOSE','MRF Purpose of request',_binary '',1,'2019-11-22 16:08:31'),(14,'REQUEST_TYPE','Manpower Module Transactions',_binary '\0',1,'2019-11-22 20:04:41'),(15,'MRF_STATUS','Status for MRF Approval',_binary '\0',1,'2019-11-25 11:12:31'),(16,'MRF_TRANSID_COUNTER','Lastest Counter for MRF Request ID',_binary '\0',1,'2019-11-30 20:33:20'),(17,'MRF_TRANSID_PREFIX','Year Prefix for MRF Request ID',_binary '\0',1,'2019-11-30 21:02:52'),(18,'ORGLIST_FILTER','Org Type displayed filter on Org Group List',_binary '\0',1,'2020-03-12 17:42:35'),(19,'ORGGROUPTYPE','Types of Organization Group',_binary '',1,'2020-10-16 09:43:10');
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
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference_value`
--

LOCK TABLES `reference_value` WRITE;
/*!40000 ALTER TABLE `reference_value` DISABLE KEYS */;
INSERT INTO `reference_value` VALUES (1,'SYSTEM_MODULE','SECURITY','SECURITY',1,'2019-11-22 19:40:25'),(2,'SYSTEM_MODULE','PLANTILLA','PLANTILLA',1,'2019-11-22 19:40:25'),(3,'SYSTEM_MODULE','MANPOWER','MANPOWER REQUISITION',1,'2019-11-22 19:40:25'),(4,'SYSTEM_MODULE','RECRUITMENT','RECRUITMENT MANAGEMENT',1,'2019-11-22 19:40:25'),(5,'REQUEST_TYPE','MRF_TRANSACTION','MANPOWER REQUISITION',1,'2019-11-22 20:07:11'),(9,'RESULT_TYPE','PASS_FAIL','PASS / FAIL',1,'2019-11-20 11:40:29'),(10,'RESULT_TYPE','APPROVE_REJECT','APPROVE / REJECT',1,'2019-11-20 11:40:29'),(11,'RESULT_TYPE','PASS_FAIL_SKIP','PASS / FAIL / SKIP',1,'2019-11-20 11:40:29'),(12,'RESULT_TYPE','APPROVE_REJECT_SKIP','APPROVE / REJECT / SKIP',1,'2019-11-20 11:40:29'),(13,'PASS_FAIL','PASS','PASS',1,'2019-11-20 11:40:29'),(14,'PASS_FAIL','FAIL','FAIL',1,'2019-11-20 11:40:29'),(15,'APPROVE_REJECT','APPROVED','APPROVED',1,'2019-11-20 11:40:29'),(16,'APPROVE_REJECT','REJECTED','REJECTED',1,'2019-11-20 11:40:29'),(17,'PASS_FAIL_SKIP','PASS','PASS',1,'2019-11-20 11:40:29'),(18,'PASS_FAIL_SKIP','FAIL','FAIL',1,'2019-11-20 11:40:29'),(19,'PASS_FAIL_SKIP','SKIP','SKIP',1,'2019-11-20 11:40:29'),(20,'APPROVE_REJECT_SKIP','APPROVED','APPROVED',1,'2019-11-20 11:40:29'),(21,'APPROVE_REJECT_SKIP','REJECTED','REJECTED',1,'2019-11-20 11:40:29'),(22,'APPROVE_REJECT_SKIP','SKIPPED','SKIPPED',1,'2019-11-20 11:40:29'),(24,'NATURE_OF_EMPLOYMENT','REGULAR','DIRECT PROBATIONARY',1,'2019-11-22 16:00:18'),(26,'MRF_STATUS','OPEN','OPEN',1,'2019-11-25 11:14:18'),(27,'MRF_STATUS','CLOSED','CLOSED',1,'2019-11-25 11:14:18'),(28,'MRF_TRANSID_COUNTER','1','COUNTER',1,'2019-11-30 20:33:36'),(29,'MRF_TRANSID_PREFIX','2021','YEAR PREFIX',1,'2019-11-30 21:03:13'),(30,'MRF_APPROVER_STATUS','FOR_APPROVAL','FOR APPROVAL',1,'2019-12-05 19:45:47'),(31,'MRF_APPROVER_STATUS','APPROVED','APPROVED',1,'2019-12-05 19:45:47'),(32,'MRF_APPROVER_STATUS','REJECTED','REJECTED',1,'2019-12-07 20:55:09'),(33,'MRF_STATUS','REJECTED','REJECTED',1,'2019-12-27 15:10:17'),(34,'MRF_STATUS','CANCELLED','CANCELLED',1,'2020-01-07 17:27:04'),(35,'MRF_APPROVER_STATUS','CANCELLED','CANCELLED',1,'2020-01-07 17:53:16'),(38,'MRF_PURPOSE','REP_TERMINATED','REPLACEMENT - TERMINATED',1,'2020-01-29 17:56:00'),(39,'MRF_PURPOSE','REP_RESIGNED','REPLACEMENT - RESIGNED',1,'2020-01-29 17:56:00'),(40,'MRF_PURPOSE','REP_LATERAL_TRANS','REPLACEMENT - LATERAL TRANSFER',1,'2020-01-29 17:56:00'),(41,'MRF_PURPOSE','REP_BRANCH_TRANS','REPLACEMENT - BRANCH TRANSFER',1,'2020-01-29 17:56:00'),(42,'MRF_PURPOSE','REP_AWOL','REPLACEMENT - AWOL',1,'2020-01-29 17:56:00'),(43,'MRF_PURPOSE','ADD_NEW_POSITION','ADDITIONAL HEAD COUNT - NEW POSITION',1,'2020-01-29 17:56:00'),(45,'MRF_PURPOSE','ADD_EXPANSION','ADDITIONAL HEAD COUNT - EXPANSION',1,'2020-01-29 17:56:00'),(46,'NATURE_OF_EMPLOYMENT','PART_TIME','PART-TIME [DISABLED]',1,'2020-02-03 16:01:32'),(47,'NATURE_OF_EMPLOYMENT','PROJ_BASED','PROJECT BASED [DISABLED]',1,'2020-02-03 16:01:32'),(48,'NATURE_OF_EMPLOYMENT','EXTERNAL','EXTERNAL/AGENCY [DISABLED]',1,'2020-02-03 16:01:32'),(49,'SETUP_MRF_APP_LEVEL','2','SYSTEM SETUP FOR THE MRF APPROVAL LEVEL',1,'2020-02-11 16:16:34'),(50,'ORGLIST_FILTER','REG','ORG TYPE DISPLAYED FILTER ON ORG GROUP LIST',1,'2020-03-12 17:42:35'),(51,'MRF_STATUS','HR_CANCELLED','CANCELLED BY HR',1,'2020-06-09 15:25:25'),(52,'MRF_STATUS','FOR_APPROVAL','FOR APPROVAL',1,'2020-06-09 15:25:25'),(53,'WORKFLOW_STATUS','COMPLETED','COMPLETED',1,'2020-06-09 15:25:25'),(54,'WORKFLOW_STATUS','IN_PROGRESS','IN-PROGRESS',1,'2020-06-09 15:25:25'),(55,'WORKFLOW_STATUS','FAILED','FAILED',1,'2020-06-09 15:25:25'),(56,'MRF_PURPOSE','ADD_SAMPLE','SAMPLE',13862,'2020-06-09 18:22:37'),(57,'MRF_PURPOSE','REP_PROMOTION','REPLACEMENT - PROMOTION',10623,'2020-06-15 14:50:08'),(58,'NATURE_OF_EMPLOYMENT','INTERNSHIP','INTERN/OJT',13862,'2020-06-23 16:22:27'),(59,'COMPANY_TAG','MNC','MOTORTRADE NATIONWIDE CORPORATION',1,'2020-07-20 14:14:34'),(60,'COMPANY_TAG','MTI','MOTORTRADE TOPLINE INC.',1,'2020-07-20 14:14:35'),(61,'COMPANY_TAG','HPTI','HONDA PRESTIGE TRADERS INC.',1,'2020-07-20 14:14:35'),(62,'COMPANY_TAG','MS','MOTORSAVE',1,'2020-07-20 14:14:35'),(63,'COMPANY_TAG','CMC','CONCERTED MANAGEMENT CORPORATION',1,'2020-07-20 14:14:36'),(64,'COMPANY_TAG','TPI','TRANSNATIONAL PROPERTIES INC.',1,'2020-07-20 14:14:36'),(65,'ORGGROUPTYPE','TOP','1.0 TOP LEVEL',1,'2020-10-16 09:43:10'),(66,'ORGGROUPTYPE','OPS','2.0 GROUP',1,'2020-10-16 09:43:10'),(67,'ORGGROUPTYPE','ZONE','3.1 ZONE',1,'2020-10-16 09:43:10'),(68,'ORGGROUPTYPE','REG','3.2 REGION',1,'2020-10-16 09:43:10'),(69,'ORGGROUPTYPE','AREA','3.3 AREA',1,'2020-10-16 09:43:10'),(70,'ORGGROUPTYPE','CLUS','3.4 CLUSTER',1,'2020-10-16 09:43:10'),(71,'ORGGROUPTYPE','BRN','3.5 BRANCH',1,'2020-10-16 09:43:10'),(72,'ORGGROUPTYPE','DEPT','4.1 DEPARTMENT',1,'2020-10-16 09:43:10'),(73,'ORGGROUPTYPE','SECTION','4.2 SECTION',1,'2020-10-16 09:43:10'),(74,'ORGGROUPTYPE','UNIT','4.3 UNIT',1,'2020-10-16 09:43:10');
/*!40000 ALTER TABLE `reference_value` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `region`
--

DROP TABLE IF EXISTS `region`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `region` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime DEFAULT NULL,
  `code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_region` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `region`
--

LOCK TABLES `region` WRITE;
/*!40000 ALTER TABLE `region` DISABLE KEYS */;
/*!40000 ALTER TABLE `region` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_role`
--

DROP TABLE IF EXISTS `system_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sync_id` int(11) NOT NULL,
  `sync_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `role_name` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `company_id` smallint(6) NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_role`
--

LOCK TABLES `system_role` WRITE;
/*!40000 ALTER TABLE `system_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `system_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `tv_approver_setup_get`
--

DROP TABLE IF EXISTS `tv_approver_setup_get`;
/*!50001 DROP VIEW IF EXISTS `tv_approver_setup_get`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_approver_setup_get` AS SELECT 
 1 AS `id`,
 1 AS `org_group`,
 1 AS `position_id`,
 1 AS `position`,
 1 AS `hierarchy_level`,
 1 AS `approver_position_id`,
 1 AS `approver_position`,
 1 AS `approver_org_group_id`,
 1 AS `approver_org_group`,
 1 AS `alt_approver_position_id`,
 1 AS `alt_approver_position`,
 1 AS `alt_approver_org_group_id`,
 1 AS `alt_approver_org_group`,
 1 AS `created_by`,
 1 AS `created_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf`
--

DROP TABLE IF EXISTS `tv_mrf`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `mrf_transaction_id`,
 1 AS `org_group_description`,
 1 AS `position_level_description`,
 1 AS `position_description`,
 1 AS `nature_of_employment`,
 1 AS `no_of_applicant`,
 1 AS `status`,
 1 AS `created_date`,
 1 AS `approved_date`,
 1 AS `hired_date`,
 1 AS `age`,
 1 AS `is_approved`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_approval_history`
--

DROP TABLE IF EXISTS `tv_mrf_approval_history`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_approval_history`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_approval_history` AS SELECT 
 1 AS `hierarchy_level`,
 1 AS `position_id`,
 1 AS `position_code`,
 1 AS `org_group_id`,
 1 AS `org_group_code`,
 1 AS `approval_status`,
 1 AS `approval_status_code`,
 1 AS `approved_date`,
 1 AS `approver_id`,
 1 AS `approval_remarks`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_dashboard_by_age`
--

DROP TABLE IF EXISTS `tv_mrf_dashboard_by_age`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_dashboard_by_age`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_dashboard_by_age` AS SELECT 
 1 AS `open_desc`,
 1 AS `open_val`,
 1 AS `open_count`,
 1 AS `closed_desc`,
 1 AS `closed_val`,
 1 AS `closed_count`,
 1 AS `age`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_defined_approver`
--

DROP TABLE IF EXISTS `tv_mrf_defined_approver`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_defined_approver`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_defined_approver` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `org_group`,
 1 AS `has_approver`,
 1 AS `last_modified_date`*/;
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
 1 AS `mrf_applicant_id`,
 1 AS `applicant_name`,
 1 AS `current_step`,
 1 AS `status`,
 1 AS `date_scheduled`,
 1 AS `date_completed`,
 1 AS `approver_remarks`,
 1 AS `workflow_id`,
 1 AS `current_step_code`,
 1 AS `current_result`,
 1 AS `result_type`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_form_signatories`
--

DROP TABLE IF EXISTS `tv_mrf_form_signatories`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_form_signatories`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_form_signatories` AS SELECT 
 1 AS `id`,
 1 AS `approver_description`,
 1 AS `approver_role_id`,
 1 AS `approval_actual_tat`,
 1 AS `approval_status`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_signatories`
--

DROP TABLE IF EXISTS `tv_mrf_signatories`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_signatories`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_signatories` AS SELECT 
 1 AS `id`,
 1 AS `workflow_id`,
 1 AS `workflow_step_id`,
 1 AS `workflow_step_approver_id`,
 1 AS `user_id`,
 1 AS `position_description`,
 1 AS `position_id`,
 1 AS `position_code`,
 1 AS `position_title`,
 1 AS `position_level_id`,
 1 AS `position_level_description`,
 1 AS `approver_name`,
 1 AS `approver_role_id`,
 1 AS `approver_description`,
 1 AS `workflow_step_code`,
 1 AS `tat_days`,
 1 AS `order`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_signatories_add`
--

DROP TABLE IF EXISTS `tv_mrf_signatories_add`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_signatories_add`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_signatories_add` AS SELECT 
 1 AS `requester_id`,
 1 AS `position_id`,
 1 AS `approver_description`,
 1 AS `approver_role_id`,
 1 AS `approver_name`,
 1 AS `approval_tat`,
 1 AS `approval_status`,
 1 AS `approval_status_code`,
 1 AS `order`,
 1 AS `approved_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_mrf_trans_id`
--

DROP TABLE IF EXISTS `tv_mrf_trans_id`;
/*!50001 DROP VIEW IF EXISTS `tv_mrf_trans_id`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_mrf_trans_id` AS SELECT 
 1 AS `id`,
 1 AS `trans_id`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_org_group_descendants`
--

DROP TABLE IF EXISTS `tv_org_group_descendants`;
/*!50001 DROP VIEW IF EXISTS `tv_org_group_descendants`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_org_group_descendants` AS SELECT 
 1 AS `descendants`*/;
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
-- Dumping events for database 'clean_ems_manpower'
--

--
-- Dumping routines for database 'clean_ems_manpower'
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_approver_setup_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_approver_setup_get`(
	IN param_org_group_id INT
)
BEGIN
	select 
		og.sync_id `id`
        , CONCAT(og.code, ' - ', og.description) `org_group`
        , p.sync_id `position_id`
        , CONCAT(p.code, ' - ', p.title) `position`
        , IFNULL(app.hierarchy_level, 0) `hierarchy_level`
        , IFNULL(app_p.sync_id,0) `approver_position_id`
        , IFNULL(CONCAT(app_p.code, ' - ', app_p.title), '') `approver_position`
        , IFNULL(app_og.sync_id,0) `approver_org_group_id`
        , IFNULL(CONCAT(app_og.code, ' - ', app_og.description), '') `approver_org_group`
        , IFNULL(alt_p.sync_id,0) `alt_approver_position_id`
        , IFNULL(CONCAT(alt_p.code, ' - ', alt_p.title), '') `alt_approver_position`
        , IFNULL(alt_og.sync_id,0) `alt_approver_org_group_id`
        , IFNULL(CONCAT(alt_og.code, ' - ', alt_og.description), '') `alt_approver_org_group`
        , IFNULL(app.created_by,0) `created_by`
        , IFNULL(app.created_date, CURRENT_TIMESTAMP) `created_date`
    from org_group og
    INNER JOIN org_group_position ogp ON og.sync_id = ogp.org_group_id 
		AND og.is_active = 1 AND ogp.is_active = 1
	INNER JOIN position p ON ogp.position_id = p.sync_id AND p.is_active = 1
    LEFT JOIN mrf_defined_approver app ON app.requesting_position_id = p.sync_id
		AND app.requesting_org_group_id = og.sync_id AND app.is_active = 1
    LEFT JOIN position app_p ON app.approver_position_id = app_p.sync_id
    LEFT JOIN org_group app_og ON app.approver_org_group_id = app_og.sync_id
    LEFT JOIN position alt_p ON app.alt_approver_position_id = alt_p.sync_id
    LEFT JOIN org_group alt_og ON app.alt_approver_org_group_id = alt_og.sync_id
	WHERE og.sync_id = param_org_group_id
    ORDER BY CONCAT(p.code, ' - ', p.title), IFNULL(app.hierarchy_level, 0);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_approver_setup_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_approver_setup_list`(
	IN param_org_group VARCHAR(255),
    IN param_has_approver VARCHAR(10),
    IN param_last_modified_date_from VARCHAR(10),
    IN param_last_modified_date_to VARCHAR(10),
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
    DECLARE local_param_org_group VARCHAR(300);
    DECLARE local_param_has_approver VARCHAR(100);
    DECLARE local_param_last_modified_date_from VARCHAR(50);
    DECLARE local_param_last_modified_date_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    
    SET local_param_org_group = CASE WHEN param_org_group = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_org_group,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_has_approver = CASE WHEN param_has_approver = '' THEN '' ELSE param_has_approver END;
    
     SET local_param_last_modified_date_from = 
		CASE WHEN STR_TO_DATE(param_last_modified_date_from, '%m/%d/%Y') IS NULL OR param_last_modified_date_from = ''
			THEN 'IFNULL(CONVERT(c.last_modified_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_last_modified_date_from, ''',''%m/%d/%Y''), '''')')  
        END;
        
    SET local_param_last_modified_date_to = 
		CASE WHEN STR_TO_DATE(param_last_modified_date_to, '%m/%d/%Y') IS NULL OR param_last_modified_date_to = ''
			THEN 'IFNULL(CONVERT(c.last_modified_date, DATE), '''')' 
			ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_last_modified_date_to, ''',''%m/%d/%Y''), '''')')  
		END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ' ('
		, local_param_org_group , '= '''''
		, ' OR LOWER(CONCAT(a.code, '' - '', a.description)) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_org_group , '), '' '', ''%''), ''%'')'
    ,' )'
     ,' AND'
    ,' ('
		
        -- , 'LOWER(CASE WHEN b.approver_count IS NOT NULL THEN ''YES'' ELSE ''NO'' END) IN (' , local_param_has_approver, ')'
        
        ,''''' = ''', local_param_has_approver, ''''
		,' OR '
		,' FIND_IN_SET((CASE WHEN b.approver_count IS NOT NULL THEN ''YES'' ELSE ''NO'' END),''', local_param_has_approver, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(c.last_modified_date, DATE),'''') BETWEEN ', local_param_last_modified_date_from, ' AND ', local_param_last_modified_date_to
    ,' )'
      ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'CASE WHEN b.approver_count IS NOT NULL THEN 1 ELSE 0 END DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'OrgGroup'			THEN CONCAT('CONCAT(a.code, '' - '', a.description) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'HasApprover'		THEN CONCAT('CASE WHEN b.approver_count IS NOT NULL THEN ''YES'' ELSE ''NO'' END ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'LastModifiedDate'	THEN CONCAT('IFNULL(DATE_FORMAT(c.last_modified_date, ''%m/%d/%Y %r''), '''') ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_org_group; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_org_group
    SELECT '
	, ' (SELECT COUNT(*) FROM 
			(
				SELECT 1 FROM org_group a
				LEFT JOIN
				(
					select count(id) `approver_count`, requesting_org_group_id
					from mrf_defined_approver 
					group by requesting_org_group_id
				) b ON a.sync_id = b.requesting_org_group_id
				LEFT JOIN
				(
					select MAX(IFNULL(modified_date, created_date)) `last_modified_date`, requesting_org_group_id
					from mrf_defined_approver 
					group by requesting_org_group_id
				) c ON a.sync_id = c.requesting_org_group_id ', where_clause, '
			) x
		) `total_num`
		, a.sync_id `id`
        , CONCAT(a.code, '' - '', a.description) `org_group`
		, CASE WHEN b.approver_count IS NOT NULL THEN ''YES'' ELSE ''NO'' END `has_approver`
		, IFNULL(DATE_FORMAT(c.last_modified_date, ''%m/%d/%Y %r''), '''') `last_modified_date`
    FROM org_group a
	LEFT JOIN
	(
		select count(id) `approver_count`, requesting_org_group_id
		from mrf_defined_approver 
		group by requesting_org_group_id
	) b ON a.sync_id = b.requesting_org_group_id
	LEFT JOIN
	(
		select MAX(IFNULL(modified_date, created_date)) `last_modified_date`, requesting_org_group_id
		from mrf_defined_approver 
		group by requesting_org_group_id
	) c ON a.sync_id = c.requesting_org_group_id
		
    '
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_org_group) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_org_group; 
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_approval_add_transaction` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_approval_add_transaction`(
	IN param_approver_id INT,
    IN param_record_id INT,
    IN param_result VARCHAR(50)
)
BEGIN
    DECLARE param_request_type VARCHAR(20);
    DECLARE previous_step_order INT;
    
    SET param_request_type = 'MRF_TRANSACTION';
    
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
		`approved_by`)
        SELECT 
			a.workflow_code
			, param_request_type
			, c.code
			, c.order
			, max_step_order
			, param_record_id
			, param_result
			, e.created_date
            , CURRENT_TIMESTAMP
            , param_approver_id
		FROM mrf_signatories a
		INNER JOIN workflow b ON a.workflow_code = b.code
		INNER JOIN workflow_step c ON c.workflow_id = b.id AND c.order = 1
		INNER JOIN 
        (
			SELECT workflow_id 
				, max(`order`) `max_step_order` 
            FROM workflow_step 
            GROUP BY workflow_id
		) d ON b.id = d.workflow_id
        INNER JOIN mrf e ON a.position_id = e.position_id AND a.requester_id = e.created_by
        WHERE e.id = param_record_id;
        
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
		`approved_by`)
		SELECT 
			a.workflow_code
			, param_request_type
			, ws_nxt.code
			, ws_nxt.order
			, f.max_step_order
			, param_record_id
			, param_result
            , CASE WHEN g.end_date_time IS NOT NULL 
				THEN g.end_date_time
				ELSE e.created_date
			END
			, CURRENT_TIMESTAMP
			, param_approver_id
		FROM mrf_signatories a
		INNER JOIN workflow b ON a.workflow_code = b.code
		INNER JOIN workflow_step c ON c.workflow_id = b.id
        LEFT JOIN workflow_step ws_nxt ON c.workflow_id = ws_nxt.workflow_id
			AND ws_nxt.order = c.order + 1
		INNER JOIN workflow_step_approver d ON c.code = d.step_code AND b.id = d.workflow_id
		INNER JOIN mrf e ON a.requester_id = e.created_by
			AND a.position_id = e.position_id 
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
			AND e.id = f.record_id
		LEFT JOIN workflow_transaction g ON b.code = g.workflow_code 
			AND g.request_type = param_request_type
			AND c.order = g.step_order
			AND e.id = g.record_id
		WHERE e.id = param_record_id;
	
		/*Update modified by and date of MRF when record is approved by final approver*/
		IF(
			SELECT 
				CASE WHEN COUNT(*) = max_step_order 
					THEN TRUE 
					ELSE FALSE 
				END 
			FROM workflow_transaction WHERE request_type = param_request_type AND record_id = param_record_id
		)
		THEN
				UPDATE mrf 
					SET approved_by = param_approver_id,
					approved_date = CURRENT_TIMESTAMP
				WHERE id = param_record_id;
		END IF;
        
    END IF;

	/*Update modified by and date of MRF when record is rejected*/
	IF(param_result = 'REJECTED')
    THEN
			UPDATE mrf 
				SET status = param_result,
                modified_by = param_approver_id,
                modified_date = CURRENT_TIMESTAMP
            WHERE id = param_record_id;
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_approval_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_approval_get_list`(
    IN param_approver_position_id INT,
    IN param_approver_org_group_id INT,
    IN param_roving_position_delimited VARCHAR(1000),
    IN param_roving_org_group_delimited VARCHAR(1000),
    IN param_approver_id INT,
    IN param_id INT,
    IN param_mrf_transaction_id VARCHAR(70),
    IN param_org_group_delimited VARCHAR(500),
    IN param_position_level_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_nature_of_employment_delimited VARCHAR(500),
    IN param_no_of_applicant_min INT,
    IN param_no_of_applicant_max INT,
    IN param_status_delimited VARCHAR(500),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
    IN param_date_approved_from VARCHAR(10),
    IN param_date_approved_to VARCHAR(10),
    IN param_age_min INT,
    IN param_age_max INT,
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
    DECLARE local_param_approver_position_id VARCHAR(20);
    DECLARE local_param_approver_org_group_id VARCHAR(20);
    DECLARE local_param_roving_org_group_delimited VARCHAR(1000);
    DECLARE local_param_roving_position_delimited VARCHAR(1000);
    DECLARE local_param_approver_id VARCHAR(20);
    DECLARE local_param_code VARCHAR(100);
    DECLARE local_param_description VARCHAR(300);
    
    DECLARE local_param_mrf_transaction_id VARCHAR(70);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_position_level_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_nature_of_employment_delimited VARCHAR(1000);
    DECLARE local_param_no_of_applicant_min VARCHAR(50);
    DECLARE local_param_no_of_applicant_max VARCHAR(50);
    DECLARE local_param_status_delimited VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_param_date_approved_from VARCHAR(50);
    DECLARE local_param_date_approved_to VARCHAR(50);
    DECLARE local_param_age_min VARCHAR(70);
    DECLARE local_param_age_max VARCHAR(70);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_approver_position_id = CONVERT(param_approver_position_id, CHAR(20));
    SET local_param_approver_org_group_id = CONVERT(param_approver_org_group_id, CHAR(20));
    SET local_param_roving_org_group_delimited = 
		CASE WHEN param_roving_org_group_delimited = '' OR param_roving_org_group_delimited = '0' 
			THEN 'NULL' 
            ELSE param_roving_org_group_delimited 
		END;
    SET local_param_roving_position_delimited = 
		CASE WHEN param_roving_position_delimited = '' OR param_roving_position_delimited = '0' 
			THEN 'NULL' 
            ELSE param_roving_position_delimited 
		END;
    SET local_param_approver_id = CONVERT(param_approver_id, CHAR(20));
    SET local_param_mrf_transaction_id = CASE WHEN param_mrf_transaction_id = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_mrf_transaction_id,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'a.org_group_id' ELSE param_org_group_delimited END;
    SET local_param_position_level_delimited = CASE WHEN param_position_level_delimited = '' THEN 'd.sync_id' ELSE param_position_level_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'a.position_id' ELSE param_position_delimited END;
    SET local_param_nature_of_employment_delimited = CASE WHEN param_nature_of_employment_delimited = '' THEN 'e.id' ELSE param_nature_of_employment_delimited END;
    SET local_param_no_of_applicant_min = CASE WHEN param_no_of_applicant_min = 0 THEN 'IFNULL(g.no_of_applicant, 0)' ELSE CONVERT(param_no_of_applicant_min,CHAR(20)) END;
    SET local_param_no_of_applicant_max = CASE WHEN param_no_of_applicant_max = 0 THEN 'IFNULL(g.no_of_applicant, 0)' ELSE CONVERT(param_no_of_applicant_max,CHAR(20)) END;
    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'g.id' ELSE param_status_delimited END;
    
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
    
    SET local_param_date_approved_from = 
		CASE WHEN STR_TO_DATE(param_date_approved_from, '%m/%d/%Y') IS NULL OR param_date_approved_from = ''
			THEN 'IFNULL(CONVERT(a.approved_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_approved_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_approved_to = 
		CASE WHEN STR_TO_DATE(param_date_approved_to, '%m/%d/%Y') IS NULL OR param_date_approved_to = ''
			THEN 'IFNULL(CONVERT(a.approved_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_approved_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_age_min = CASE WHEN param_age_min = 0 THEN 'DATEDIFF(IFNULL(a.approved_date, CURRENT_TIMESTAMP), a.created_date)' ELSE CONVERT(param_age_min,CHAR(20)) END;
    SET local_param_age_max = CASE WHEN param_age_max = 0 THEN 'DATEDIFF(IFNULL(a.approved_date, CURRENT_TIMESTAMP), a.created_date)' ELSE CONVERT(param_age_max,CHAR(20)) END;
    
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
		, local_param_mrf_transaction_id , '= '''''
		, ' OR LOWER(a.mrf_transaction_id) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_mrf_transaction_id , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' a.org_group_id IN (', local_param_org_group_delimited, ')'
    ,' )'
     ,' AND'
    ,' ('
		,' d.sync_id IN (', local_param_position_level_delimited, ')'
    ,' )'
	,' AND'
    ,' ('
		,' a.position_id IN (', local_param_position_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' e.id IN (', local_param_nature_of_employment_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(g.no_of_applicant, 0) BETWEEN ', local_param_no_of_applicant_min, ' AND ', local_param_no_of_applicant_max
    ,' )'
    ,' AND'
    ,' ('
		,' g.id IN (', local_param_status_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.approved_date, DATE),'''') BETWEEN ', local_param_date_approved_from, ' AND ', local_param_date_approved_to
    ,' )'
    ,' AND'
    ,' ('
		,' DATEDIFF(IFNULL(a.approved_date, CURRENT_TIMESTAMP), a.created_date) BETWEEN ', local_param_age_min, ' AND ', local_param_age_max
    ,' )'
    ,' AND'
    ,' ('
		,' app_his.approver_id = ', local_param_approver_id, ' OR '
		-- ,' ( '
-- 		,'  	(
-- 					a.selected_approver_id_1 = ', local_param_approver_id,'
--                     AND 
--                     IFNULL(app_his.level_of_approval,1) = 1
--                 )
-- 				OR
--                 (
-- 					a.selected_approver_id_2 = ', local_param_approver_id,'
--                     AND 
-- 					(
-- 						(app_his.level_of_approval = 1 AND IFNULL(a.approved_by,0) = 0)
--                         OR
--                         (a.approved_by = a.selected_approver_id_2 AND app_his.level_of_approval = 2)
--                     )
--                 )'
--         , ' )'
--         , ' OR'
        , ' ('
        , '		('
        , '			('
        , '				a.approver_position_id = ', local_param_approver_position_id, ''
        , '				AND '
        , ' 			a.approver_org_group_id = ', local_param_approver_org_group_id,''
		, '			) '
		, '			OR '
		, '			('
        , '				a.alt_approver_position_id = ', local_param_approver_position_id, ''
        , '				AND '
        , ' 			a.alt_approver_org_group_id = ', local_param_approver_org_group_id,''
		, '			) '
		, '			OR '
		, '			('
        , '				a.approver_position_id IN (', local_param_roving_position_delimited, ')'
        , '				AND '
        , ' 			a.approver_org_group_id IN (', local_param_roving_org_group_delimited,')'
		, '			) '
		, '		) '
		, '		AND '
        , ' 	a.status <> ''REJECTED'''
		, ' )'
    ,' )'
 --    ,' AND'
-- 	,'	('
-- 	,'		('
-- 	,'			IFNULL(wft.step_order + 1, 1) >= wfs.order'
-- 	,'			AND IFNULL(wft.result, ''FOR APPROVAL'') <> ''REJECTED'''
--     ,'  	)'
--     ,'  	OR'
-- 	,'		('
-- 	,'			IFNULL(wft.result, ''FOR APPROVAL'') = ''REJECTED'' AND wft.step_order = wfs.order'
-- 	,'		)'
--     ,'	)'
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''							THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'						THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'IsApproved'				THEN CONCAT('IFNULL(CASE WHEN a.approved_date IS NOT NULL THEN true ELSE false END, false) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'MRFID'						THEN CONCAT('a.mrf_transaction_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'OrgGroupDescription'		THEN CONCAT('CONCAT(b.code, '' - '', b.description) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionLevelDescription'	THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionDescription'		THEN CONCAT('CONCAT(c.code, '' - '', c.title) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NatureOfEmployment'		THEN CONCAT('e.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoOfApplicant'				THEN CONCAT('IFNULL(g.no_of_applicant, 0) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Status'					THEN CONCAT('f.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateCreated'				THEN CONCAT('DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateApproved'				THEN CONCAT('IFNULL(a.approved_date, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Age'						THEN CONCAT('DATEDIFF(IFNULL(approved_date,CURRENT_TIMESTAMP), a.created_date) ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_mrf; 
    
    SET joined_tables = CONCAT('mrf a
	INNER JOIN org_group b ON a.org_group_id = b.sync_id
	INNER JOIN position c ON a.position_id = c.sync_id
	INNER JOIN position_level d ON c.position_level_id = d.sync_id
	INNER JOIN reference_value e ON a.nature_of_employment = e.value AND e.ref_code = ''NATURE_OF_EMPLOYMENT''
    LEFT JOIN 
    (
		SELECT 
        b.id
			, b.mrf_id
			, b.approver_id
			, b.level_of_approval
			, b.result
			, b.timestamp
		FROM
		(
			SELECT mrf_id, approver_id, level_of_approval, MAX(`timestamp`) `timestamp` 
			FROM mrf_approval_history  
			GROUP BY mrf_id, approver_id, level_of_approval
		) a INNER JOIN
		mrf_approval_history b ON a.mrf_id = b.mrf_id 
			AND a.approver_id = b.approver_id
			AND a.level_of_approval = b.level_of_approval
			AND a.timestamp = b.timestamp
    )
    app_his ON a.id = app_his.mrf_id 
		AND a.approval_level >= app_his.level_of_approval
        AND app_his.approver_id = ', local_param_approver_id, '
	LEFT JOIN 
	(
		SELECT a.id `mrf_id`
			, COUNT(b.id) `no_of_applicant`
		FROM mrf a
		INNER JOIN mrf_applicant b ON a.id = b.mrf_id
        GROUP BY a.id
	) g ON a.id = g.mrf_id 
     INNER JOIN reference_value g ON 
	(
		CASE WHEN 
        (
			(
				a.approver_position_id = ', local_param_approver_position_id,' 
				AND 
				a.approver_org_group_id = ', local_param_approver_org_group_id ,'
			)
            OR
            (
				a.alt_approver_position_id = ', local_param_approver_position_id,' 
				AND 
				a.alt_approver_org_group_id = ', local_param_approver_org_group_id ,'
			)
            OR
            (
				a.approver_position_id IN (', local_param_roving_position_delimited,' )
				AND 
				a.approver_org_group_id IN (', local_param_roving_org_group_delimited ,')
			)
            -- OR
-- 			(	
--                 
--                 (
-- 					a.selected_approver_id_1 = ', local_param_approver_id,'
--                     AND 
--                     IFNULL(app_his.level_of_approval, 0)+1 = 1
--                 )
-- 				OR
--                 (
-- 					a.selected_approver_id_2 = ', local_param_approver_id,'
--                     AND 
--                     IFNULL(app_his.level_of_approval, 0)+1 = 2
--                 )
-- 			)
            
		)
		AND a.status NOT IN (''REJECTED'', ''CANCELLED'', ''OPEN'')
			THEN ''FOR_APPROVAL''
		WHEN app_his.result = ''APPROVED'' AND a.status NOT IN (''REJECTED'', ''CANCELLED'')
			THEN ''APPROVED''
		WHEN app_his.result = ''REJECTED'' -- OR a.status = ''REJECTED''
			THEN ''REJECTED''
		WHEN app_his.result = ''APPROVED''
			THEN ''APPROVED''
		WHEN a.status = ''CANCELLED''
			THEN ''CANCELLED''
			ELSE ''''
		END
    )
    = g.value AND g.ref_code = ''MRF_APPROVER_STATUS''  ');
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_mrf
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, a.id
		, a.mrf_transaction_id
        , '''' `scope_org_group`
		, CONCAT(b.code, '' - '', b.description) `org_group_description`
		, d.description `position_level_description`
		, CONCAT(c.code, '' - '', c.title) `position_description`
		, e.description `nature_of_employment`
		, IFNULL(g.no_of_applicant, 0) `no_of_applicant`
		, CASE WHEN 
        (
			(
				a.approver_position_id = ', local_param_approver_position_id,' 
				AND 
				a.approver_org_group_id = ', local_param_approver_org_group_id ,'
			)
            OR
			(
				a.alt_approver_position_id = ', local_param_approver_position_id,' 
				AND 
				a.alt_approver_org_group_id = ', local_param_approver_org_group_id ,'
			)
            OR
            (
				a.approver_position_id IN (', local_param_roving_position_delimited,' )
				AND 
				a.approver_org_group_id IN (', local_param_roving_org_group_delimited ,')
			)
           --  OR
-- 			(	
--                 (
-- 					a.selected_approver_id_1 = ', local_param_approver_id,'
--                     AND 
--                     IFNULL(app_his.level_of_approval, 0) <> 1
--                 )
-- 				OR
--                 (
-- 					a.selected_approver_id_2 = ', local_param_approver_id,'
--                     AND 
--                     IFNULL(app_his.level_of_approval, 0) <> 2
--                 )
-- 			)
		)
		AND a.status NOT IN (''REJECTED'', ''CANCELLED'', ''OPEN'')
			THEN ''For Approval''
		WHEN app_his.result = ''APPROVED'' AND a.status NOT IN (''REJECTED'', ''CANCELLED'')
			THEN ''Approved''
		WHEN app_his.result = ''REJECTED'' -- OR a.status = ''REJECTED''
			THEN ''Rejected''
		WHEN app_his.result = ''APPROVED''
			THEN ''Approved''
		WHEN a.status = ''CANCELLED''
			THEN ''Cancelled''
			ELSE ''''
		END `status`
		, DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') `created_date`
		, DATEDIFF(IFNULL(approved_date,CURRENT_TIMESTAMP), a.created_date) `age`
        	, IFNULL(DATE_FORMAT(
			CASE WHEN app_his.result = ''APPROVED'' AND app_his.approver_id = ', local_param_approver_id, ' AND a.status NOT IN (''REJECTED'', ''CANCELLED'')
				THEN app_his.timestamp
                ELSE NULL
			END
        , ''%m/%d/%Y %r''), '''') `approved_date`
        , CASE WHEN status = ''CLOSED'' 
				THEN IFNULL(DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r''), '''')
				ELSE ''''
			  END `hired_date`
        , IFNULL(CASE WHEN a.approved_date IS NOT NULL THEN true ELSE false END, false) `is_approved`
        , app_his.level_of_approval
        , IFNULL(app_his.level_of_approval, 0)+1 = 1
        ,IFNULL(app_his.level_of_approval, 0)+1 = 2
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_mrf) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_mrf; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_approval_history_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_approval_history_add`(
	IN param_approver_id INT,
	IN param_mrf_id INT,
    IN param_result VARCHAR(50),
    IN param_remarks VARCHAR(255)
    -- IN param_level_of_approval INT,
    -- IN param_next_approver_position_id INT,
	-- IN param_next_approver_org_group_id INT,
)
BEGIN
    DECLARE setup_approval_level int;
    DECLARE local_next_approver_position_id int;
    DECLARE local_next_approver_org_group_id int;
    DECLARE local_next_alt_approver_position_id int;
    DECLARE local_next_alt_approver_org_group_id int;
    DECLARE local_requesting_position_id int;
    DECLARE local_requesting_org_group_id int;
    DECLARE local_approval_level int;
    DECLARE local_max_approval_level int;
    
    SET setup_approval_level = (SELECT value FROM reference_value WHERE ref_code = 'SETUP_MRF_APP_LEVEL');
    
    SELECT position_id, org_group_id, approval_level, max_approval_level
    INTO local_requesting_position_id, local_requesting_org_group_id, local_approval_level, local_max_approval_level
    FROM mrf WHERE id = param_mrf_id;
    
     /*===================================== Generates org group hierarchy into temp table [sp_result_temp_mrf_approvers] ==================================*/
    CALL sp_mrf_get_approvers_as_temp_table(local_requesting_position_id, local_requesting_org_group_id);
    /*==================================================================== END =====================================================================*/

	IF local_max_approval_level <> local_approval_level
	THEN
		SELECT position_id, org_group_id, alt_position_id, alt_org_group_id
		INTO local_next_approver_position_id, local_next_approver_org_group_id,
        local_next_alt_approver_position_id, local_next_alt_approver_org_group_id
		FROM sp_result_temp_mrf_approvers WHERE hierarchy_level = (local_approval_level + 1);

		/*Set the position id and org group id for the next approver*/
		UPDATE mrf 
			SET  approver_position_id = IFNULL(local_next_approver_position_id,0),
			approver_org_group_id = IFNULL(local_next_approver_org_group_id,0),
            alt_approver_position_id = IFNULL(local_next_alt_approver_position_id,0),
			alt_approver_org_group_id = IFNULL(local_next_alt_approver_org_group_id,0),
			approval_level = (local_approval_level + 1)
		WHERE id = param_mrf_id;
	END IF;
    
    INSERT INTO mrf_approval_history
	(
		mrf_id
        , result
        , level_of_approval
        , approver_id
        , alternate_approver_id
        , remarks
	)
	VALUES 
    (
		param_mrf_id
        , param_result
        , local_approval_level
        , param_approver_id
        , null
        , param_remarks
	);
            
            
    -- IF (SELECT COUNT(*) FROM mrf_approval_history 
-- 		WHERE mrf_id = param_mrf_id 
-- 			AND param_result = result 
--             AND param_result = 'APPROVED') = setup_approval_level
--     THEN
		UPDATE mrf 
				SET status = 'OPEN',
                approved_by = param_approver_id,
                approved_date = CURRENT_TIMESTAMP
            WHERE id = param_mrf_id 
            AND local_approval_level = max_approval_level;
    -- END IF;

	/*Update modified by and date of MRF when record is rejected*/
	IF(param_result = 'REJECTED')
    THEN
    
		SELECT position_id, org_group_id, alt_position_id, alt_org_group_id
		INTO local_next_approver_position_id, local_next_approver_org_group_id
			, local_next_alt_approver_position_id, local_next_alt_approver_org_group_id
		FROM sp_result_temp_mrf_approvers WHERE hierarchy_level = 1;
		
        UPDATE mrf 
			SET status = param_result,
			modified_by = param_approver_id,
			modified_date = CURRENT_TIMESTAMP,
            approver_position_id = local_next_approver_position_id, /*set first level approver if rejected*/
			approver_org_group_id = local_next_approver_org_group_id, /*set first level approver if rejected*/
            alt_approver_position_id = IFNULL(local_next_alt_approver_position_id, 0), /*set first level approver if rejected*/
			alt_approver_org_group_id = IFNULL(local_next_alt_approver_org_group_id, 0), /*set first level approver if rejected*/
			approval_level = 1 /*set first level approver if rejected*/
		WHERE id = param_mrf_id;
        
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_dashboard_by_age` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_dashboard_by_age`(
	IN param_user_id INT,
	IN param_is_admin BOOLEAN    
)
BEGIN
	DECLARE local_param_user VARCHAR(25);

	If param_is_admin = 1 
	THEN 
		SET local_param_user = '%%'; 
	ELSE 
		SET local_param_user = param_user_id; 
	END IF;

	SELECT 
		1 AS ID, 
		(
			SELECT description FROM reference_value 
			WHERE ref_code = 'MRF_STATUS' 
				AND value = 'OPEN'
		) `OPEN_DESC`,
		(
			SELECT id FROM reference_value 
			WHERE ref_code = 'MRF_STATUS' 
				AND value = 'OPEN'
		) `OPEN_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
			WHERE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) <= 15 
				AND status = 'OPEN' 
				AND is_active = 1 AND created_by LIKE local_param_user
                AND approved_date IS NOT NULL
		) `OPEN_COUNT`,
		(
			SELECT description FROM reference_value 
			WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_DESC`,
		(
			SELECT id FROM reference_value 
			WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
			WHERE DATEDIFF(modified_date,approved_date) <= 15 
				AND status = 'CLOSED'
				AND is_active = 1 AND created_by LIKE local_param_user
                AND approved_date IS NOT NULL
		) `CLOSED_COUNT`,
		'Within 15 Days' AS AGE

	UNION ALL

	SELECT 
		2, 
		(
			SELECT description FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
		) `OPEN_DESC`,
		(
			SELECT id FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
		) `OPEN_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            WHERE (IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) BETWEEN 16 AND 30) AND status = 'OPEN' 
				AND is_active = 1 AND created_by LIKE local_param_user
				AND approved_date IS NOT NULL
		) `OPEN_COUNT`,
		(
			SELECT description FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_DESC`,
		(
			SELECT id FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            WHERE (DATEDIFF(modified_date,approved_date) BETWEEN 16 AND 30) AND status = 'CLOSED' 
				AND is_active = 1 AND created_by LIKE local_param_user
				AND approved_date IS NOT NULL
		) `CLOSED_COUNT`,
		'16 - 30 Days' 

	UNION ALL

	SELECT 
		3, 
		(
			SELECT description FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
		) `OPEN_DESC`,
		(
			SELECT id FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
		) `OPEN_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            WHERE (IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) BETWEEN 31 AND 45) AND status = 'OPEN' 
				AND is_active = 1 AND created_by LIKE local_param_user
				AND approved_date IS NOT NULL 
		) `OPEN_COUNT`,
		(
			SELECT description FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_DESC`,
		(
			SELECT id FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            WHERE (DATEDIFF(modified_date,approved_date) BETWEEN 31 AND 45) AND status = 'CLOSED' 
				AND is_active = 1 AND created_by LIKE local_param_user
				AND approved_date IS NOT NULL
		) `CLOSED_COUNT`,
		'31 - 45 Days'

	UNION ALL

	SELECT 
		4, 
		(	
			SELECT description FROM reference_value 
            WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
        ) `OPEN_DESC`,
		(
			SELECT id FROM reference_value WHERE ref_code = 'MRF_STATUS' and value = 'OPEN'
		) `OPEN_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            WHERE (IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) > 45) AND status = 'OPEN' 
				AND is_active = 1 AND created_by LIKE local_param_user
                AND approved_date IS NOT NULL
		) `OPEN_COUNT`,
		(
			SELECT description FROM reference_value WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_DESC`,
		(
			SELECT id FROM reference_value WHERE ref_code = 'MRF_STATUS' and value = 'CLOSED'
		) `CLOSED_VAL`,
		(
			SELECT COUNT(1) AS COUNT FROM mrf 
            -- WHERE (DATEDIFF(IFNULL(approved_date, CURRENT_TIMESTAMP), created_date) > 45) AND status = 'CLOSED' 
            WHERE (DATEDIFF(IFNULL(approved_date, CURRENT_TIMESTAMP), created_date) > 45) AND status = 'CLOSED' 
				AND is_active = 1 AND created_by LIKE local_param_user
                AND approved_date IS NOT NULL
		) `CLOSED_COUNT`,
		'More than 45 Days' 
        
	ORDER BY ID;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_existing_applicant_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_existing_applicant_list`(
	IN param_mrf_id INT,
	IN param_id_delimited VARCHAR(16383),
    IN param_id INT,
    IN param_for_hiring_id INT,
    IN param_applicant_name VARCHAR(255),
    IN param_current_step_delimited VARCHAR(500),
    IN param_status_delimited VARCHAR(500),
    IN param_date_scheduled_from VARCHAR(10),
    IN param_date_scheduled_to VARCHAR(10),
    IN param_date_completed_from VARCHAR(10),
    IN param_date_completed_to VARCHAR(10),
    IN param_approver_remarks VARCHAR(500),
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
    DECLARE local_param_mrf_id VARCHAR(20);
	DECLARE local_param_applicant_name VARCHAR(1000);
    DECLARE local_param_current_step_delimited VARCHAR(1000);
    DECLARE local_param_status_delimited VARCHAR(1000);
    DECLARE local_param_date_scheduled_from VARCHAR(50);
	DECLARE local_param_date_scheduled_to VARCHAR(50);
	DECLARE local_param_date_completed_from VARCHAR(50);
	DECLARE local_param_date_completed_to VARCHAR(50);
	DECLARE local_param_approver_remarks VARCHAR(1000);

    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_mrf_id = CONVERT(param_mrf_id, CHAR(20));
	SET local_param_applicant_name = CASE WHEN param_applicant_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_applicant_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_step_delimited = CASE WHEN param_current_step_delimited = '' THEN '' ELSE param_current_step_delimited END;
    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'IFNULL(a.workflow_status, ''IN_PROGRESS'')' ELSE CONCAT('''', REPLACE(REPLACE(param_status_delimited,'\\','\\\\'),',',''','''), '''') END;
    
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
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ,' ('
		, local_param_mrf_id , '= a.mrf_id'
    ,' )'
    ,' AND'
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
		,''''' = ''', local_param_current_step_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(a.current_step_code, ''''),''', local_param_current_step_delimited, ''')'
    ,' )'
	,' AND'
    ,' ('
		,' IFNULL(a.workflow_status, ''IN_PROGRESS'') IN (', local_param_status_delimited, ')'
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
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    ,' AND'
    ,' ('
		,' a.applicant_id IN (', param_id_delimited ,')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''						THEN 'for_hiring DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'					THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ForHiring'				THEN CONCAT('for_hiring ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApplicantName'			THEN CONCAT('CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStep'			THEN CONCAT('a.current_step_code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Status'				THEN CONCAT('IFNULL(a.current_step_result_value, '''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateScheduled'				THEN CONCAT('IFNULL(a.date_scheduled, '''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCompleted'				THEN CONCAT('IFNULL(a.date_completed, '''') ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'ApproverRemarks'				THEN CONCAT('IFNULL(a.approver_remarks, '''') ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_mrf_applicant; 
    
    SET joined_tables = "mrf_applicant a
	LEFT JOIN reference_value ref ON IFNULL(a.workflow_status, ""IN_PROGRESS"") = ref.value AND ref.ref_code = ""WORKFLOW_STATUS""
";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_mrf_applicant
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, a.applicant_id `id`
        , a.id `mrf_applicant_id`
		, CONCAT(a.lastname, IFNULL(CONCAT('' '', a.suffix), ''''), '', '', a.firstname, '' '', IFNULL(a.middlename, ''''), '' '') `applicant_name` 
		, CASE WHEN IFNULL(a.current_step_code, '''') = '''' 
			THEN ''''
            ELSE a.current_step_code
		END `current_step`
        , CASE WHEN a.ID = ', param_for_hiring_id,' THEN 1 ELSE 0 END `for_hiring`
		-- , IFNULL(CONCAT(wf.code, '' - '', wf.description), '''') `workflow_description`
		, IFNULL(ref.description, '''') `status`
        , a.workflow_id
        , a.current_step_code
        , a.current_result
        , a.result_type
        , IFNULL(DATE_FORMAT(a.date_scheduled, ''%m/%d/%Y''), '''') `date_scheduled`
        , IFNULL(DATE_FORMAT(a.date_completed, ''%m/%d/%Y''), '''') `date_completed`
        , a.approver_remarks
	FROM ', joined_tables
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_mrf_applicant) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_mrf_applicant; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_get_approval_history` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_get_approval_history`(
	IN param_requesting_position_id INT,
	IN param_requesting_org_group_id INT,
    IN param_position_id INT,
    IN param_mrf_id INT
)
BEGIN
	
    
    /*===================================== Generates org group hierarchy into temp table [sp_result_temp_mrf_approvers] ==================================*/
    CALL sp_mrf_get_approvers_as_temp_table(param_requesting_position_id, param_requesting_org_group_id);
    /*==================================================================== END =====================================================================*/
    
-- 	SET param_user_id = CASE WHEN param_record_id > 0 THEN (SELECT created_by FROM mrf WHERE id = param_record_id) ELSE param_user_id END;
    SELECT
		approvers.hierarchy_level
    	, approvers.position_id
    	, approvers.position_code
        , approvers.org_group_id
        , approvers.org_group_code
        , IFNULL(approvers.alt_position_id, 0) `alt_position_id`
        , IFNULL(approvers.alt_org_group_id, 0) `alt_org_group_id`
        , CASE WHEN his.timestamp IS NOT NULL AND his.result = 'APPROVED' 	
				THEN 'Approved'
            WHEN his.timestamp IS NOT NULL AND his.result = 'REJECTED' 		
				THEN 'Rejected'
			WHEN (prev_his.timestamp IS NOT NULL AND prev_his.result != 'REJECTED') OR approvers.hierarchy_level = 1
				THEN 'For Approval'
			WHEN (prev_his.result = 'REJECTED')
				THEN 'N/A'
			ELSE 'Pending'
		END `approval_status`
        , CASE WHEN his.timestamp IS NOT NULL AND his.result = 'APPROVED' 	
				THEN 'APPROVED'
            WHEN his.timestamp IS NOT NULL AND his.result = 'REJECTED' 		
				THEN 'REJECTED'
			WHEN (prev_his.timestamp IS NOT NULL AND prev_his.result != 'REJECTED') OR approvers.hierarchy_level = 1
				THEN 'FOR_APPROVAL'
			WHEN (prev_his.result = 'REJECTED')
				THEN 'N/A'
			ELSE 'PENDING'
		END `approval_status_code`
        , IFNULL(DATE_FORMAT(his.timestamp, '%m/%d/%Y %r'), '') `approved_date`
        , IFNULL(his.approver_id, 0) `approver_id`
        , his.remarks `approval_remarks`
	FROM sp_result_temp_mrf_approvers approvers 
    LEFT JOIN mrf m ON m.position_id = param_position_id AND param_mrf_id = m.id 
    LEFT JOIN 
    (
		SELECT 
			b.mrf_id
			, b.approver_id
			, b.level_of_approval
			, b.result
			, b.timestamp
            , b.remarks
		FROM
		(
			SELECT mrf_id, approver_id, level_of_approval, MAX(`timestamp`) `timestamp` 
			FROM mrf_approval_history  
			GROUP BY mrf_id, approver_id, level_of_approval
		) a INNER JOIN
		mrf_approval_history b ON a.mrf_id = b.mrf_id 
			AND a.approver_id = b.approver_id
			AND a.level_of_approval = b.level_of_approval
			AND a.timestamp = b.timestamp
    ) his ON m.id = his.mrf_id
		AND his.level_of_approval = approvers.hierarchy_level
	LEFT JOIN 
    (
		SELECT 
			b.mrf_id
			, b.approver_id
			, b.level_of_approval
			, b.result
			, b.timestamp
            , b.remarks
		FROM
		(
			SELECT mrf_id, approver_id, level_of_approval, MAX(`timestamp`) `timestamp` 
			FROM mrf_approval_history  
			GROUP BY mrf_id, approver_id, level_of_approval
		) a INNER JOIN
		mrf_approval_history b ON a.mrf_id = b.mrf_id 
			AND a.approver_id = b.approver_id
			AND a.level_of_approval = b.level_of_approval
			AND a.timestamp = b.timestamp
    ) prev_his ON m.id = prev_his.mrf_id
		AND prev_his.level_of_approval + 1 = approvers.hierarchy_level
    ORDER BY approvers.hierarchy_level
    ;
		
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_get_approvers_as_temp_table` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_get_approvers_as_temp_table`(
	IN param_requesting_position_id INT,
    IN param_requesting_org_group_id INT
)
BEGIN
/*
	NOTE: This script does not return a result set, it saves the result onto a temp table [sp_result_temp_mrf_approvers]
*/
	DECLARE setup_approval_level int;
    -- DECLARE tmp_childId int;
--     DECLARE tmp_childId_org int;
/*====================================================== Position Variables ==========================================================*/        
	
	SET @hierarchy_level = 00000;
	-- DROP TEMPORARY TABLE IF EXISTS tmp;
-- 	DROP TEMPORARY TABLE IF EXISTS results;
-- 	CREATE TEMPORARY TABLE tmp (childId INT NOT NULL);
-- 	CREATE INDEX tmp_childId_idx ON tmp (childId);
-- 	CREATE TEMPORARY TABLE results (childId INT NOT NULL, hierarchy_level INT NOT NULL);
    
 --    DROP TEMPORARY TABLE IF EXISTS temp_position_org_mapping;
--     CREATE TEMPORARY TABLE temp_position_org_mapping (
-- 		position_id INT NOT NULL
-- 		, org_hierarchy_level INT NOT NULL
-- 	);
    
    DROP TEMPORARY TABLE IF EXISTS temp_position_hierarchy;
    CREATE TEMPORARY TABLE temp_position_hierarchy (
		id INT NOT NULL
		, position_level_id INT NOT NULL
        , code VARCHAR(50) NOT NULL
        , title VARCHAR(255) NOT NULL
        , hierarchy_level INT NOT NULL
	);
	
    SET setup_approval_level = (SELECT value FROM reference_value WHERE ref_code = 'SETUP_MRF_APP_LEVEL');
/*============================================================= END ===================================================================*/

/*====================================================== Org Group Variables ==========================================================*/        
	SET @hierarchy_level_org = 00000;
	-- DROP TEMPORARY TABLE IF EXISTS tmp_org;
	-- DROP TEMPORARY TABLE IF EXISTS results_org;
	-- CREATE TEMPORARY TABLE tmp_org (childId INT NOT NULL);
	-- CREATE INDEX tmp_childId_idx_org ON tmp_org (childId);
-- 	CREATE TEMPORARY TABLE results_org (childId INT NOT NULL, hierarchy_level INT NOT NULL);
-- 	INSERT INTO tmp_org 
-- 		SELECT param_requesting_org_group_id;
	
--     INSERT INTO tmp_org 
-- 		SELECT parent_org_id FROM org_group WHERE sync_id = param_requesting_org_group_id;
	
--     DROP TEMPORARY TABLE IF EXISTS temp_org_group_upward;
-- 	CREATE TEMPORARY TABLE temp_org_group_upward (
-- 		org_group_id INT
--         , org_group_code VARCHAR(355)
--         , hierarchy_level INT
--         );
/*============================================================= END ===================================================================*/


/*====================================================== Get Org Group Hierarchy ==========================================================*/        
	-- WHILE (SELECT COUNT(*) FROM tmp_org) > 0 DO
-- 		SET tmp_childId_org = (SELECT MIN(childId) FROM tmp_org);
-- 		DELETE FROM tmp_org WHERE childId = tmp_childId_org;
--         
--         SET @hierarchy_level_org = @hierarchy_level_org + 1;
--         
--         INSERT INTO results_org 
--         SELECT sync_id, @hierarchy_level_org 
--         FROM org_group 
--         WHERE sync_id = tmp_childId_org AND is_active = 1 ;
-- 			-- AND (param_approval_level >= @hierarchy_level_org OR param_approval_level = 0);
-- 		
--         INSERT INTO tmp_org 
--         SELECT parent_org_id 
--         FROM org_group 
--         WHERE sync_id = tmp_childId_org AND is_active = 1 ;
-- 			-- AND (param_approval_level >= @hierarchy_level_org OR param_approval_level = 0);
-- 	END WHILE;

	-- INSERT INTO temp_org_group_upward
-- 	SELECT o.sync_id, o.code, r.hierarchy_level FROM org_group o
--     JOIN (SELECT childId, hierarchy_level FROM results_org) r 
-- 		ON o.sync_id = r.childId AND o.is_active = 1
-- 	ORDER BY r.hierarchy_level, o.code, o.description ;
    
/*==================================================================== END =====================================================================*/

/*======================================================== Get Position Hierarchy ==========================================================*/
    
    -- INSERT INTO tmp 
--     SELECT parent_position_id 
--     FROM position 
--     WHERE sync_id = (
-- 				SELECT parent_position_id 
-- 				FROM position 
-- 				WHERE sync_id = param_requesting_position_id 
-- 					AND parent_position_id IS NOT NULL
--         ) AND parent_position_id IS NOT NULL;
-- 	
-- 	WHILE (SELECT COUNT(*) FROM tmp) > 0 DO
-- 		SET tmp_childId = (SELECT MIN(childId) FROM tmp);
-- 		DELETE FROM tmp WHERE childId = tmp_childId;
--         SET @hierarchy_level = @hierarchy_level + 1;
--         INSERT INTO results SELECT sync_id, @hierarchy_level FROM position WHERE sync_id = tmp_childId AND is_active = 1 AND setup_approval_level >= @hierarchy_level;
-- 		INSERT INTO tmp SELECT parent_position_id FROM position WHERE sync_id = tmp_childId AND parent_position_id IS NOT NULL AND is_active = 1 AND setup_approval_level >= @hierarchy_level;
-- 	END WHILE;

	-- INSERT INTO temp_position_hierarchy
-- 	SELECT
-- 		o.sync_id `id`
--         , o.position_level_id
--         , o.code
--         , o.title
--         , r.hierarchy_level
--     FROM position o
--     JOIN (
-- 		SELECT childId, hierarchy_level FROM results 
--     ) r 
-- 		ON o.sync_id = r.childId AND o.is_active = 1
-- 	ORDER BY r.hierarchy_level, id ;
/*==================================================================== END =====================================================================*/

  -- INSERT INTO temp_position_org_mapping
--     SELECT 
-- 		ph.id `position_id`
-- 		, MIN(ogu.hierarchy_level) `min_hierarchy_level`
-- 	FROM org_group_position ogp 
-- 	INNER JOIN temp_position_hierarchy ph ON ogp.position_id = ph.id AND ogp.is_active = 1
-- 	INNER JOIN temp_org_group_upward ogu ON ogp.org_group_id = ogu.org_group_id
-- 	GROUP BY ph.code, ph.id;
--     
	DROP TEMPORARY TABLE IF EXISTS sp_result_temp_mrf_approvers;
	CREATE TEMPORARY TABLE sp_result_temp_mrf_approvers (
		hierarchy_level INT
        , position_id INT
        , position_code VARCHAR(355)
        , org_group_id INT
        , org_group_code VARCHAR(355)
        , alt_position_id INT
        , alt_org_group_id INT
        );
    
    INSERT INTO sp_result_temp_mrf_approvers
    VALUES (1, 0, '', 0, '', 0, 0);
    INSERT INTO sp_result_temp_mrf_approvers
    VALUES (2, 0, '', 0, '', 0, 0);
    
    SET SQL_SAFE_UPDATES = 0;
    
    -- UPDATE sp_result_temp_mrf_approvers temp
-- 	INNER JOIN org_group_position ogp ON temp.hierarchy_level IN (1,2)
-- 	INNER JOIN temp_position_hierarchy ph ON ogp.position_id = ph.id AND ogp.is_active = 1 
-- 		AND ph.hierarchy_level = temp.hierarchy_level
-- 	INNER JOIN temp_org_group_upward ogu ON ogp.org_group_id = ogu.org_group_id 
-- 	INNER JOIN temp_position_org_mapping pom ON ogu.hierarchy_level = pom.org_hierarchy_level
-- 	SET
-- 		temp.position_id = ph.id
-- 		, temp.position_code = ph.code
-- 		, temp.org_group_id = ogu.org_group_id
-- 		, temp.org_group_code = ogu.org_group_code
-- ;
    
     UPDATE sp_result_temp_mrf_approvers temp
	INNER JOIN mrf_defined_approver appr ON temp.hierarchy_level = appr.hierarchy_level
		AND appr.requesting_position_id = param_requesting_position_id
        AND appr.requesting_org_group_id = param_requesting_org_group_id
        AND appr.is_active = 1
	INNER JOIN position p ON appr.approver_position_id = p.sync_id
    INNER JOIN org_group o ON appr.approver_org_group_id = o.sync_id
	SET
		temp.position_id = p.sync_id
		, temp.position_code = p.code
		, temp.org_group_id = o.sync_id
		, temp.org_group_code = o.code
		, temp.alt_position_id = appr.alt_approver_position_id
		, temp.alt_org_group_id = appr.alt_approver_org_group_id
;
    
    
    -- INSERT INTO sp_result_temp_mrf_approvers
--     SELECT
-- 		ph.hierarchy_level
-- 		, ph.id `position_id`
-- 		, ph.code `position_code`
-- 		, ogu.org_group_id
-- 		, ogu.org_group_code
-- 	FROM org_group_position ogp 
-- 	INNER JOIN temp_position_hierarchy ph ON ogp.position_id = ph.id AND ogp.is_active = 1
-- 	INNER JOIN temp_org_group_upward ogu ON ogp.org_group_id = ogu.org_group_id
-- 	INNER JOIN temp_position_org_mapping pom ON ogu.hierarchy_level = pom.org_hierarchy_level
-- 	GROUP BY ph.code, ph.id;
       
	-- DROP TEMPORARY TABLE IF EXISTS tmp_org;
	-- DROP TEMPORARY TABLE IF EXISTS results_org;
    -- DROP TEMPORARY TABLE IF EXISTS tmp;
	-- DROP TEMPORARY TABLE IF EXISTS results;
    -- DROP TEMPORARY TABLE IF EXISTS temp_position_org_mapping;
    DROP TEMPORARY TABLE IF EXISTS temp_position_hierarchy;
    -- DROP TEMPORARY TABLE IF EXISTS temp_org_group_upward;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_get_list`(
	IN param_user_id INT,
    IN param_id INT,
    IN param_mrf_transaction_id VARCHAR(70),
    IN param_org_group_delimited VARCHAR(500),
    IN param_scope_org_type VARCHAR(50),
    IN param_scope_org_group_delimited VARCHAR(500),
    IN param_position_level_delimited VARCHAR(500),
    IN param_position_delimited VARCHAR(500),
    IN param_nature_of_employment_delimited VARCHAR(500),
    IN param_no_of_applicant_min INT,
    IN param_no_of_applicant_max INT,
    IN param_status_delimited VARCHAR(500),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
    IN param_date_approved_from VARCHAR(10),
    IN param_date_approved_to VARCHAR(10),
    IN param_date_hired_from VARCHAR(10),
    IN param_date_hired_to VARCHAR(10),
    IN param_age_min INT,
    IN param_age_max INT,
    IN param_is_admin BOOLEAN,
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
	DECLARE local_param_user_id VARCHAR(20);
	DECLARE local_param_is_admin VARCHAR(20);
	DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_code VARCHAR(100);
    DECLARE local_param_description VARCHAR(300);
    
    DECLARE local_param_mrf_transaction_id VARCHAR(70);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_scope_descendants_delimited VARCHAR(16383);
    DECLARE local_param_position_level_delimited VARCHAR(1000);
    DECLARE local_param_position_delimited VARCHAR(1000);
    DECLARE local_param_nature_of_employment_delimited VARCHAR(1000);
    DECLARE local_param_no_of_applicant_min VARCHAR(50);
    DECLARE local_param_no_of_applicant_max VARCHAR(50);
    DECLARE local_param_status_delimited VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_param_date_approved_from VARCHAR(50);
    DECLARE local_param_date_approved_to VARCHAR(50);
    DECLARE local_param_date_hired_from VARCHAR(50);
    DECLARE local_param_date_hired_to VARCHAR(50);
    DECLARE local_param_age_min VARCHAR(255);
    DECLARE local_param_age_max VARCHAR(255);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_user_id = CONVERT(param_user_id, CHAR(20));
    SET local_param_is_admin = CONVERT(param_is_admin, CHAR(20));
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_mrf_transaction_id = CASE WHEN param_mrf_transaction_id = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_mrf_transaction_id,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN 'a.org_group_id' ELSE param_org_group_delimited END;
    SET local_param_position_level_delimited = CASE WHEN param_position_level_delimited = '' THEN 'd.sync_id' ELSE param_position_level_delimited END;
    SET local_param_position_delimited = CASE WHEN param_position_delimited = '' THEN 'a.position_id' ELSE param_position_delimited END;
    SET local_param_nature_of_employment_delimited = CASE WHEN param_nature_of_employment_delimited = '' THEN 'e.id' ELSE param_nature_of_employment_delimited END;
    SET local_param_no_of_applicant_min = CASE WHEN param_no_of_applicant_min = 0 THEN 'IFNULL(g.no_of_applicant, 0)' ELSE CONVERT(param_no_of_applicant_min,CHAR(20)) END;
    SET local_param_no_of_applicant_max = CASE WHEN param_no_of_applicant_max = 0 THEN 'IFNULL(g.no_of_applicant, 0)' ELSE CONVERT(param_no_of_applicant_max,CHAR(20)) END;
    SET local_param_status_delimited = CASE WHEN param_status_delimited = '' THEN 'f.id' ELSE param_status_delimited END;
    
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
    
    SET local_param_date_approved_from = 
		CASE WHEN STR_TO_DATE(param_date_approved_from, '%m/%d/%Y') IS NULL OR param_date_approved_from = ''
			THEN 'IFNULL(CONVERT(a.approved_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_approved_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_approved_to = 
		CASE WHEN STR_TO_DATE(param_date_approved_to, '%m/%d/%Y') IS NULL OR param_date_approved_to = ''
			THEN 'IFNULL(CONVERT(a.approved_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_approved_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_date_hired_from = 
		CASE WHEN STR_TO_DATE(param_date_hired_from, '%m/%d/%Y') IS NULL OR param_date_hired_from = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_hired_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_date_hired_to = 
		CASE WHEN STR_TO_DATE(param_date_hired_to, '%m/%d/%Y') IS NULL OR param_date_hired_to = ''
			THEN 'IFNULL(CONVERT(a.modified_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_hired_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_age_min = 
    CASE WHEN param_age_min = 0 
		THEN 'CASE WHEN status = ''CLOSED'' 
				THEN DATEDIFF(modified_date,approved_date)
				ELSE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) 
		END' 
        ELSE CONVERT(param_age_min,CHAR(20)) 
	END;
    
    SET local_param_age_max = 
    CASE WHEN param_age_max = 0 
		THEN 'CASE WHEN status = ''CLOSED'' 
					THEN DATEDIFF(modified_date,approved_date)
					ELSE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) 
			END' 
        ELSE CONVERT(param_age_max,CHAR(20)) 
	END;
    
    SET local_param_scope_descendants_delimited = 
		CASE WHEN param_scope_org_group_delimited = '' OR param_scope_org_group_delimited = '0' OR local_param_is_admin = 0
			THEN 'org_group_id'
			ELSE (SELECT IFNULL(fn_org_group_get_descendants(param_scope_org_group_delimited),'''''') LIMIT 1)
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
		, local_param_mrf_transaction_id , '= '''''
		, ' OR LOWER(a.mrf_transaction_id) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_mrf_transaction_id , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' a.org_group_id IN (', local_param_org_group_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' a.org_group_id IN (', local_param_scope_descendants_delimited, ')'
    ,' )'
     ,' AND'
    ,' ('
		,' d.sync_id IN (', local_param_position_level_delimited, ')'
    ,' )'
	,' AND'
    ,' ('
		,' a.position_id IN (', local_param_position_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' e.id IN (', local_param_nature_of_employment_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(g.no_of_applicant, 0) BETWEEN ', local_param_no_of_applicant_min, ' AND ', local_param_no_of_applicant_max
    ,' )'
    ,' AND'
    ,' ('
		,' f.id IN (', local_param_status_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.approved_date, DATE),'''') BETWEEN ', local_param_date_approved_from, ' AND ', local_param_date_approved_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(a.modified_date, DATE),'''') BETWEEN ', local_param_date_hired_from, ' AND ', local_param_date_hired_to
    ,' )'
    ,' AND'
    ,' ('
		,' CASE WHEN status = ''CLOSED'' 
				THEN DATEDIFF(modified_date,approved_date)
				ELSE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) 
		END BETWEEN ', local_param_age_min, ' AND ', local_param_age_max
    ,' )'
    ,' AND'
    ,' ('
		,' a.created_by = ', local_param_user_id, ' OR ( 1 = ', local_param_is_admin, ') '
    ,' )'
    ,' AND'
    ,' ('
		,' a.is_active = 1'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''							THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'						THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'IsApproved'				THEN CONCAT('IFNULL(CASE WHEN a.approved_date IS NOT NULL THEN true ELSE false END, false) ', local_sord, ',') END, ''), 
    IFNULL(CASE WHEN sidx = 'MRFID'						THEN CONCAT('a.mrf_transaction_id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'OrgGroupDescription'		THEN CONCAT('CONCAT(b.code, '' - '', b.description) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionLevelDescription'	THEN CONCAT('d.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionDescription'		THEN CONCAT('CONCAT(c.code, '' - '', c.title) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NatureOfEmployment'		THEN CONCAT('e.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'NoOfApplicant'				THEN CONCAT('IFNULL(g.no_of_applicant, 0) ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Status'					THEN CONCAT('f.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CreatedDate'				THEN CONCAT('DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'ApprovedDate'				THEN CONCAT('IFNULL(a.approved_date, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'HiredDate'				    THEN CONCAT('IFNULL(a.modified_date, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Age'						THEN CONCAT('CASE WHEN status = ''CLOSED'' 
																		THEN DATEDIFF(modified_date,approved_date)
																		ELSE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) 
																	END ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_mrf; 
    
    SET joined_tables = "mrf a
	INNER JOIN org_group b ON a.org_group_id = b.sync_id
	INNER JOIN position c ON a.position_id = c.sync_id
	INNER JOIN position_level d ON c.position_level_id = d.sync_id
	INNER JOIN reference_value e ON a.nature_of_employment = e.value AND e.ref_code = ""NATURE_OF_EMPLOYMENT""
	INNER JOIN reference_value f ON a.status = f.value AND f.ref_code = ""MRF_STATUS""
	LEFT JOIN 
	(
		SELECT a.id `mrf_id`
			, COUNT(b.id) `no_of_applicant`
		FROM mrf a
		INNER JOIN mrf_applicant b ON a.id = b.mrf_id
        WHERE b.is_active = 1
        GROUP BY a.id
	) g ON a.id = g.mrf_id
	-- LEFT JOIN
-- 	(
-- 		SELECT 
-- 			h.start_date_time
--             , h.end_date_time
-- 			, h.record_id
-- 		FROM workflow_transaction h
-- 		INNER JOIN workflow i ON h.workflow_code = i.code AND module = ""MANPOWER"" AND h.request_type = ""MRF_TRANSACTION""
-- 		INNER JOIN workflow_step j ON h.step_code = j.code AND i.id = j.workflow_id
-- 		INNER JOIN 
-- 		(
-- 			SELECT workflow_id, MAX(code) `code`, MAX(`order`) `max_order` 
-- 			FROM workflow_step 
-- 			GROUP BY workflow_id
-- 		) k ON i.id = k.workflow_id AND j.code = k.code AND j.order = k.max_order
-- 	) h ON a.id = h.record_id 

";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_mrf
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
			, a.id
			, a.mrf_transaction_id
			, CONCAT(b.code, '' - '', b.description) `org_group_description`
            , IFNULL(b.sync_id, 0) `org_group_id`
			, d.description `position_level_description`
			, CONCAT(c.code, '' - '', c.title) `position_description`
			, e.description `nature_of_employment`
			, IFNULL(g.no_of_applicant, 0) `no_of_applicant`
			, f.description `status`
			, DATE_FORMAT(a.created_date, ''%m/%d/%Y %r'') `created_date`
			, CASE WHEN status = ''CLOSED'' 
				THEN DATEDIFF(modified_date,approved_date)
				ELSE IFNULL(DATEDIFF(CURRENT_TIMESTAMP, approved_date),0) 
			END `age`
			, IFNULL(DATE_FORMAT(a.approved_date, ''%m/%d/%Y %r''), '''') `approved_date`
            , CASE WHEN status = ''CLOSED'' 
				THEN IFNULL(DATE_FORMAT(a.modified_date, ''%m/%d/%Y %r''), '''')
				ELSE ''''
			  END `hired_date`
			, IFNULL(CASE WHEN a.approved_date IS NOT NULL THEN true ELSE false END, false) `is_approved`
		FROM ', joined_tables
		, where_clause, order_by_clause
        , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
		, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (',
    
    CASE WHEN local_param_is_admin = 1 
				THEN
					CONCAT('SELECT temp.*
					, @row_num:= @row_num + 1 `row_num`
					, IFNULL(scope.id, 0) `scope_org_group_id`
					, IFNULL(CONCAT(scope.code, '' - '', scope.description),'''') `scope_org_group`
					FROM 
						(
							SELECT * 
								, IFNULL(fn_org_group_get_chart_upward_by_org_type(org_group_id, ''',param_scope_org_type,'''), 0) `parent`
							FROM temp_mrf
						) temp
					 LEFT JOIN 
                     (
						SELECT id, sync_id, sync_date, parent_org_id, code, description, org_type, is_active FROM org_group
                        UNION
                        SELECT 0 id, 0 sync_id, NULL sync_date, 0 parent_org_id, '''' code, '''' description, '''' org_type, ''1'' is_active
                     ) scope ON temp.parent = scope.sync_id  AND scope.is_active = 1')
				ELSE
					'SELECT *
						, @row_num:= @row_num + 1 `row_num`
						, 0 `scope_org_group_id` 
                        , '''' `scope_org_group` 
					FROM temp_mrf'
			END
    ,') a',
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
    DROP TEMPORARY TABLE IF EXISTS temp_mrf; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_get_new_trans_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_get_new_trans_id`()
BEGIN
	DECLARE trans_id_counter INT;
    DECLARE trans_id_prefix VARCHAR(50);
    DECLARE trans_id_missing INT;
	SET trans_id_counter = (SELECT value FROM reference_value WHERE ref_code = 'MRF_TRANSID_COUNTER');
	SET trans_id_prefix = (SELECT value FROM reference_value WHERE ref_code = 'MRF_TRANSID_PREFIX');
    
    IF(YEAR(CURRENT_TIMESTAMP) != YEAR(STR_TO_DATE(trans_id_prefix, '%Y')))
    THEN
		SET trans_id_prefix = YEAR(CURRENT_TIMESTAMP);
        SET SQL_SAFE_UPDATES = 0;
        UPDATE reference_value 
        SET value = YEAR(CURRENT_TIMESTAMP)
        WHERE ref_code = 'MRF_TRANSID_PREFIX';
    END IF;
    
    SET trans_id_counter = trans_id_counter + 1;
    
    -- Get missing transaction id
    SET trans_id_missing = IFNULL((
		SELECT
			z.expected
			AS missing
		FROM 
		(
			SELECT@rownum:=@rownum+1 AS expected,
				IF(@rownum=CONVERT(SUBSTRING(mrf_transaction_id,10, LENGTH(mrf_transaction_id) - 9), UNSIGNED), 
				0, @rownum:=CONVERT(SUBSTRING(mrf_transaction_id,10, LENGTH(mrf_transaction_id) - 9), UNSIGNED)) AS got
			FROM
			(
				SELECT @rownum:=0
			) AS a
			JOIN mrf
			WHERE mrf.mrf_transaction_id LIKE CONCAT('%', '_', trans_id_prefix, '_', '%')
			ORDER BY mrf_transaction_id
		) AS z
		WHERE z.got!=0 LIMIT 1
    ), 0);
    
	IF(trans_id_missing = 0)
	THEN
		UPDATE reference_value 
		SET value =  trans_id_counter
		WHERE ref_code = 'MRF_TRANSID_COUNTER';
		SELECT 0 `id`, CONCAT('MRF-',trans_id_prefix,'-',LPAD(trans_id_counter, 5, '0')) `trans_id`;
	ELSE
		SELECT 0 `id`, CONCAT('MRF-',trans_id_prefix,'-',LPAD(trans_id_missing, 5, '0')) `trans_id`;
	END IF;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_get_signatories_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_get_signatories_add`(
	IN param_record_id INT,
    IN param_user_id INT,
	IN param_position_id INT
)
BEGIN
	SET param_user_id = CASE WHEN param_record_id > 0 THEN (SELECT created_by FROM mrf WHERE id = param_record_id) ELSE param_user_id END;
	SELECT
		aa.requester_id
        , aa.position_id
		, b.description `approver_description`
		, c.role_id `approver_role_id`
        , d.role_name `approver_name`
        , IFNULL(DATEDIFF(IFNULL(wft.end_date_time, CURRENT_TIMESTAMP), CASE WHEN b.order = 1 THEN e.created_date ELSE IFNULL(wft.start_date_time, CURRENT_TIMESTAMP) END), 0) `approval_tat`
        , CASE WHEN wft.start_date_time IS NOT NULL AND wft.result = 'APPROVED'
			THEN 'Approved'
            WHEN wft.start_date_time IS NOT NULL AND wft.result = 'REJECTED'
			THEN 'Rejected'
			WHEN (prev_wt.start_date_time IS NOT NULL AND prev_wt.result != 'REJECTED') OR b.order = 1
			THEN 'For Approval'
			ELSE 'Pending'
		END `approval_status`
        , CASE WHEN wft.start_date_time IS NOT NULL AND wft.result = 'APPROVED'
			THEN 'APPROVED'
            WHEN wft.start_date_time IS NOT NULL AND wft.result = 'REJECTED'
			THEN 'REJECTED'
			WHEN (prev_wt.start_date_time IS NOT NULL AND prev_wt.result != 'REJECTED') OR b.order = 1
			THEN 'FOR_APPROVAL'
			ELSE 'PENDING'
		END `approval_status_code`
        , b.order
        , IFNULL(DATE_FORMAT(wft.end_date_time, '%m/%d/%Y %r'), '') `approved_date`
	FROM mrf_signatories aa
    INNER JOIN workflow a ON aa.workflow_code = a.code
	INNER JOIN workflow_step b ON b.workflow_id = a.id
	INNER JOIN workflow_step_approver c ON b.code = c.step_code 
	INNER JOIN system_role d ON c.role_id = d.sync_id
    LEFT JOIN 
    (
		SELECT 
			record_id
            , workflow_code
            , step_code
            , result
            , step_order
            , start_date_time
            , end_date_time
        FROM workflow_transaction 
        WHERE request_type = "MRF_TRANSACTION"
			 AND record_id = param_record_id
        GROUP BY record_id, workflow_code, step_code, request_type
    ) wft ON aa.workflow_code = wft.workflow_code
		AND b.code = wft.step_code 
        AND b.order = wft.step_order
	LEFT JOIN mrf e ON e.id = wft.record_id
	-- Checking previous step used for displaying approval status
	LEFT JOIN workflow_step prev_ws ON b.workflow_id = prev_ws.workflow_id AND b.order - 1 = prev_ws.order
	LEFT JOIN workflow_transaction prev_wt ON prev_wt.request_type = 'MRF_TRANSACTION'
		AND prev_ws.code =prev_wt.step_code
        AND prev_wt.record_id = param_record_id
    
	WHERE (aa.requester_id = param_user_id OR param_user_id = 0)
		AND aa.position_id = param_position_id
	ORDER BY b.Order;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_id_get_dropdown` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_id_get_dropdown`(
	IN param_applicant_id INT
)
BEGIN
	SELECT 
		MA.id 
        , MA.mrf_id
		, MA.applicant_id
        , MA.firstname
        , MA.middlename
        , MA.lastname
        , MA.suffix
        , MA.workflow_id
        , CONCAT(MA.id, ',', MA.workflow_id) `current_step_code` /*to be used as value on dropdown*/
        , M.mrf_transaction_id `current_step_description` /*to be used as description on dropdown*/
        , MA.workflow_status
        , MA.current_step_approver_role_ids
        , MA.current_result
        , MA.result_type
        , MA.date_scheduled
        , MA.date_completed
        , MA.approver_remarks
        , MA.for_hiring
        , MA.is_active
        , MA.created_by
        , MA.created_date
        , MA.modified_by
        , MA.modified_date
	FROM mrf_applicant MA
	INNER JOIN mrf M ON MA.mrf_id = M.id
	WHERE MA.applicant_id = param_applicant_id;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_mrf_signatories_get_by_requester_position` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_mrf_signatories_get_by_requester_position`(
	IN param_requester_id INT,
    IN param_position_id INT
)
BEGIN
	SELECT a.id
		, d.id `workflow_id`
        , e.id `workflow_step_id`
        , f.id `workflow_step_approver_id`
		, a.requester_id `user_id`
		, CONCAT(c.code, ' - ', c.title) `position_description`
		, a.position_id `position_id`
        , c.code `position_code`
        , c.title `position_title`
        , c.position_level_id
        , h.description `position_level_description`
		, g.role_name `approver_name`
		, f.role_id `approver_role_id`
		, e.description `approver_description`
        , e.code `workflow_step_code`
		, e.tat_days
        , e.order
	FROM mrf_signatories a
	INNER JOIN position c ON a.position_id = c.sync_id
    INNER JOIN position_level h ON c.position_level_id = h.sync_id
	INNER JOIN workflow d ON a.workflow_code = d.code
	INNER JOIN workflow_step e ON d.id = e.workflow_id
	INNER JOIN workflow_step_approver f ON e.code = f.step_code
	INNER JOIN system_role g ON f.role_id = g.sync_id
    WHERE a.requester_id = param_requester_id
		AND a.position_id = param_position_id;
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
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
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
        , org_type
        , parent_org_id
        , is_active
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_autocomplete_by_org_type` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_autocomplete_by_org_type`(
	IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_term VARCHAR(16383);
    DECLARE local_top_results VARCHAR(20);
    DECLARE local_org_type_code VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
    SET local_org_type_code = (SELECT value FROM ems_plantilla.reference_value WHERE ref_code = 'ORGLIST_FILTER' LIMIT 1);
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT sync_id
        , code
		, INSTR(CONCAT(code, '' - '', description), ', local_term, ') `index`
        , description
        , parent_org_id
        , is_active
        , org_type
        , created_by
        , created_date
        , modified_by
        , modified_date
	FROM org_group
	WHERE LOWER(CONCAT(code, '' - '', description)) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
		AND org_type = ''', local_org_type_code ,'''
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_get_chart_upward` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_get_chart_upward`(IN start_from INT)
BEGIN
DECLARE tmp_childId int;
	SET @hierarchy_level = 00000;
	DROP TEMPORARY TABLE IF EXISTS tmp;
	DROP TEMPORARY TABLE IF EXISTS results;
	CREATE TEMPORARY TABLE tmp (childId INT NOT NULL);
	CREATE INDEX tmp_childId_idx ON tmp (childId);
	CREATE TEMPORARY TABLE results (childId INT NOT NULL, hierarchy_level INT NOT NULL);
	INSERT INTO tmp SELECT start_from;
	
    DROP TEMPORARY TABLE IF EXISTS temp_hierarchy_plantilla_count;
	CREATE TEMPORARY TABLE temp_hierarchy_plantilla_count (
		org_group_id INT
        , org_group_code VARCHAR(355)
        , hierarchy_level INT
        );
        
	WHILE (SELECT COUNT(*) FROM tmp) > 0 DO
		SET tmp_childId = (SELECT MIN(childId) FROM tmp);
		DELETE FROM tmp WHERE childId = tmp_childId;
        SET @hierarchy_level = @hierarchy_level + 1;
        INSERT INTO results SELECT id, @hierarchy_level FROM org_group WHERE id = tmp_childId AND is_active = 1;
		INSERT INTO tmp SELECT parent_org_id FROM org_group WHERE id = tmp_childId AND is_active = 1;
	END WHILE;

	insert into temp_hierarchy_plantilla_count
	SELECT o.id, o.code, r.hierarchy_level FROM org_group o
    JOIN (SELECT childId, hierarchy_level FROM results UNION SELECT start_from, 1) r 
		ON o.id = r.childId AND o.is_active = 1
	ORDER BY r.hierarchy_level, id ;
    -- WHERE id IN (SELECT childId FROM results UNION SELECT start_from) ORDER BY parent_org_id;
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_org_group_get_descendants` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_org_group_get_descendants`(
	IN start_from INT
)
BEGIN
DECLARE tmp_childId int;
	SET @hierarchy_level = 00001;
	DROP TEMPORARY TABLE IF EXISTS tmp;
	DROP TEMPORARY TABLE IF EXISTS results;
	CREATE TEMPORARY TABLE tmp (childId INT NOT NULL);
	CREATE INDEX tmp_childId_idx ON tmp (childId);
	CREATE TEMPORARY TABLE results (childId INT NOT NULL, hierarchy_level INT NOT NULL);
	INSERT INTO tmp SELECT start_from;

	WHILE (SELECT COUNT(*) FROM tmp) > 0 DO
		SET tmp_childId = (SELECT MIN(childId) FROM tmp);
		DELETE FROM tmp WHERE childId = tmp_childId;
        IF EXISTS (SELECT id FROM org_group WHERE parent_org_id = tmp_childId AND id <> parent_org_id AND is_active = 1)
        THEN
			SET @hierarchy_level = IFNULL((SELECT hierarchy_level FROM results WHERE childId = tmp_childId LIMIT 1),1);
			SET @hierarchy_level := @hierarchy_level + 1;
        END IF;
        
        INSERT INTO results SELECT id, @hierarchy_level FROM org_group WHERE parent_org_id = tmp_childId AND is_active = 1;
		INSERT INTO tmp SELECT id FROM org_group WHERE parent_org_id = tmp_childId AND id <> parent_org_id AND is_active = 1;
	END WHILE;

	SELECT IFNULL(GROUP_CONCAT(r.childId SEPARATOR ','),'') `descendants` FROM org_group o
    JOIN (SELECT childId, hierarchy_level FROM results UNION SELECT start_from, 1) r 
		ON o.id = r.childId AND o.id <> o.parent_org_id AND o.is_active = 1;
       
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_get_list`(
	IN param_id INT,
    IN param_position_level_ids VARCHAR(500),
    IN param_code VARCHAR(50),
    IN param_title VARCHAR(255),
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
    DECLARE local_param_position_level_ids VARCHAR(500);
    DECLARE local_param_code VARCHAR(100);
    DECLARE local_param_title VARCHAR(300);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_position_level_ids = CASE WHEN param_position_level_ids = '' THEN 'position_level_id' ELSE param_position_level_ids END;
    SET local_param_code = CASE WHEN param_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_code,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
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
		,' position_level_id IN (', local_param_position_level_ids, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_code , '= '''''
		, ' OR code LIKE CONCAT(''%'', REPLACE(' , local_param_code , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR title LIKE CONCAT(''%'', REPLACE(' , local_param_title , ', '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, ' a.is_active = 1' 
    ,' ) ');
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''							THEN 'a.id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'						THEN CONCAT('a.id ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'PositionLevelDescription'	THEN CONCAT('b.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Code'						THEN CONCAT('a.code ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'						THEN CONCAT('title ', local_sord, ',') END, ''));
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_position; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_position
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM position a ', where_clause, ') x) `total_num`
		, a.id
        , a.position_level_id
        , b.description `position_level_description`
        , a.code
        , a.title
        , , is_active
	FROM position a 
    INNER JOIN position_level b ON a.position_level_id = b.id
    '
    , where_clause, order_by_clause
    , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_position) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_position; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_get_parent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_get_parent`(
	IN param_start_from INT
    , IN param_levels INT /*If zero, display up to the highest level*/ 
)
BEGIN
DECLARE tmp_childId int;
	SET @hierarchy_level = 00000;
	DROP TEMPORARY TABLE IF EXISTS tmp;
	DROP TEMPORARY TABLE IF EXISTS results;
	CREATE TEMPORARY TABLE tmp (childId INT NOT NULL);
	CREATE INDEX tmp_childId_idx ON tmp (childId);
	CREATE TEMPORARY TABLE results (childId INT NOT NULL, hierarchy_level INT NOT NULL);
	
    INSERT INTO tmp 
    SELECT parent_position_id 
    FROM position 
    WHERE sync_id = param_start_from 
		AND parent_position_id IS NOT NULL;
	
	WHILE (SELECT COUNT(*) FROM tmp) > 0 DO
		SET tmp_childId = (SELECT MIN(childId) FROM tmp);
		DELETE FROM tmp WHERE childId = tmp_childId;
        SET @hierarchy_level = @hierarchy_level + 1;
        INSERT INTO results SELECT sync_id, @hierarchy_level FROM position WHERE sync_id = tmp_childId AND is_active = 1;
		INSERT INTO tmp SELECT parent_position_id FROM position WHERE sync_id = tmp_childId AND parent_position_id IS NOT NULL AND is_active = 1;
	END WHILE;

	SELECT
		o.sync_id `id`
        , o.position_level_id
        , o.code
        , o.title
        , r.hierarchy_level
    FROM position o
    JOIN (
		SELECT childId, hierarchy_level FROM results 
		-- UNION SELECT param_start_from, 1
    ) r 
		ON o.sync_id = r.childId AND o.is_active = 1
	WHERE param_levels >= hierarchy_level  OR param_levels = 0
	ORDER BY r.hierarchy_level, id ;
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_level_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_level_autocomplete`(
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
        , INSTR(description, ', local_term, ') `index`
        , description
        , 0 created_by
        , CURRENT_TIMESTAMP() created_date
        , 0 modified_by
        , NULL modified_date
        , company_id
        , is_active
	FROM position_level
	WHERE LOWER(description) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') AND is_active = 1
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_position_level_get_by_org_group` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_position_level_get_by_org_group`(
	IN param_id INT
)
BEGIN
	SELECT
		DISTINCT d.id
        , d.sync_id
        , d.sync_date
		, d.description
        , d.company_id
        , d.is_active
	FROM org_group a
	INNER JOIN org_group_position b ON a.sync_id = b.org_group_id AND a.is_active = 1
	INNER JOIN position c ON b.position_id = c.sync_id
	INNER JOIN position_level d ON c.position_level_id = d.sync_id
	WHERE a.sync_id = param_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_role_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_bin */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_role_autocomplete`(
	IN term VARCHAR(255),
    IN top_results INT,
    IN param_company_id SMALLINT
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
        , role_name
        , INSTR(role_name, ', local_term, ') `index`
        , is_active
	FROM system_role
	WHERE role_name LIKE CONCAT(''%'',', local_term, ',''%'') AND is_active = 1
	ORDER BY `index`
		, role_name ',
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
-- Final view structure for view `tv_approver_setup_get`
--

/*!50001 DROP VIEW IF EXISTS `tv_approver_setup_get`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_approver_setup_get` AS select 0 AS `id`,'' AS `org_group`,0 AS `position_id`,'' AS `position`,0 AS `hierarchy_level`,0 AS `approver_position_id`,'' AS `approver_position`,0 AS `approver_org_group_id`,'' AS `approver_org_group`,0 AS `alt_approver_position_id`,'' AS `alt_approver_position`,0 AS `alt_approver_org_group_id`,'' AS `alt_approver_org_group`,0 AS `created_by`,NULL AS `created_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `mrf_transaction_id`,'' AS `org_group_description`,'' AS `position_level_description`,'' AS `position_description`,'' AS `nature_of_employment`,0 AS `no_of_applicant`,'' AS `status`,'' AS `created_date`,'' AS `approved_date`,'' AS `hired_date`,0 AS `age`,0 AS `is_approved` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_approval_history`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_approval_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_approval_history` AS select 0 AS `hierarchy_level`,0 AS `position_id`,'' AS `position_code`,0 AS `org_group_id`,'' AS `org_group_code`,'' AS `approval_status`,'' AS `approval_status_code`,'' AS `approved_date`,0 AS `approver_id`,'' AS `approval_remarks` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_dashboard_by_age`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_dashboard_by_age`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_dashboard_by_age` AS select '' AS `open_desc`,0 AS `open_val`,0 AS `open_count`,'' AS `closed_desc`,0 AS `closed_val`,0 AS `closed_count`,'' AS `age` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_defined_approver`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_defined_approver`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_defined_approver` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `org_group`,'' AS `has_approver`,'' AS `last_modified_date` from DUAL  where (1 = 0) */;
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
/*!50001 VIEW `tv_mrf_existing_applicant` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,0 AS `mrf_applicant_id`,'' AS `applicant_name`,'' AS `current_step`,'' AS `status`,'' AS `date_scheduled`,'' AS `date_completed`,'' AS `approver_remarks`,0 AS `workflow_id`,'' AS `current_step_code`,'' AS `current_result`,'' AS `result_type` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_form_signatories`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_form_signatories`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_form_signatories` AS select 0 AS `id`,'' AS `approver_description`,0 AS `approver_role_id`,0 AS `approval_actual_tat`,'' AS `approval_status` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_signatories`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_signatories`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_signatories` AS select 0 AS `id`,0 AS `workflow_id`,0 AS `workflow_step_id`,0 AS `workflow_step_approver_id`,0 AS `user_id`,'' AS `position_description`,0 AS `position_id`,'' AS `position_code`,'' AS `position_title`,0 AS `position_level_id`,'' AS `position_level_description`,'' AS `approver_name`,0 AS `approver_role_id`,'' AS `approver_description`,'' AS `workflow_step_code`,0 AS `tat_days`,0 AS `order` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_signatories_add`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_signatories_add`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_signatories_add` AS select 0 AS `requester_id`,0 AS `position_id`,'' AS `approver_description`,0 AS `approver_role_id`,'' AS `approver_name`,0 AS `approval_tat`,'' AS `approval_status`,'' AS `approval_status_code`,0 AS `order`,'' AS `approved_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_mrf_trans_id`
--

/*!50001 DROP VIEW IF EXISTS `tv_mrf_trans_id`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_mrf_trans_id` AS select 0 AS `id`,'' AS `trans_id` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_org_group_descendants`
--

/*!50001 DROP VIEW IF EXISTS `tv_org_group_descendants`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_org_group_descendants` AS select '' AS `descendants` from DUAL  where (1 = 0) */;
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

-- Dump completed on 2021-04-06 17:58:39
