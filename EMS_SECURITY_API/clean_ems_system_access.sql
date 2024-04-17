CREATE DATABASE  IF NOT EXISTS `clean_ems_system_access` /*!40100 DEFAULT CHARACTER SET latin1 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clean_ems_system_access`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: clean_ems_system_access
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
-- Table structure for table `audit_log`
--

DROP TABLE IF EXISTS `audit_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `audit_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `event_type` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `table_name` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `table_id` bigint(20) NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `ip_address` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_success` bit(1) DEFAULT b'0',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `audit_log`
--

LOCK TABLES `audit_log` WRITE;
/*!40000 ALTER TABLE `audit_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `audit_log` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
INSERT INTO `reference` VALUES (1,'ON_MENU_LEVEL','System Menu Hierarchy',_binary '\0',1,'2019-07-16 15:20:13'),(2,'URL_FUNCTION_TYPE','Function Types of the system',_binary '',1,'2019-07-16 15:20:13'),(3,'EMPLOYMENT_NATURE','Nature of Employment',_binary '',1,'2019-07-30 16:30:28'),(4,'PURPOSE','Purpose',_binary '',1,'2019-07-30 16:30:46'),(6,'RESULT_TYPE','Sets of responses for every step',_binary '\0',1,'2019-11-18 19:03:57'),(9,'PASS_FAIL','Result Type - Pass or Fail options',_binary '\0',1,'2019-11-18 19:08:34'),(10,'APPROVE_REJECT','Result Type - Approve or Reject options',_binary '\0',1,'2019-11-18 19:08:34'),(11,'PASS_FAIL_SKIP','Result Type - Pass, Fail or Skip options',_binary '\0',1,'2019-11-18 19:08:34'),(12,'APPROVE_REJECT_SKIP','Result Type - Approve, Reject or Skip options',_binary '\0',1,'2019-11-18 19:08:34'),(13,'ADMIN_BYPASS_PSSWORD','Admin password to bypass the authentication of all accounts',_binary '\0',1,'2020-11-04 15:40:46');
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
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference_value`
--

LOCK TABLES `reference_value` WRITE;
/*!40000 ALTER TABLE `reference_value` DISABLE KEYS */;
INSERT INTO `reference_value` VALUES (1,'ON_MENU_LEVEL','LINK_1','System Module or Top Level Link',1,'2019-07-16 15:17:38'),(2,'ON_MENU_LEVEL','LINK_2','Features under a Module',1,'2019-07-16 15:17:38'),(3,'ON_MENU_LEVEL','LINK_3','Sub Features under a Module',1,'2019-07-16 15:17:38'),(4,'URL_FUNCTION_TYPE','EDIT','Edit Function',1,'2019-07-16 15:17:38'),(5,'URL_FUNCTION_TYPE','DELETE','Delete Function',1,'2019-07-16 15:17:38'),(6,'URL_FUNCTION_TYPE','UPLOAD','Upload Function',1,'2019-07-16 15:17:38'),(7,'URL_FUNCTION_TYPE','EXPORT','Export Function',1,'2019-07-16 15:17:38'),(8,'URL_FUNCTION_TYPE','GENERATE','Generate Function',1,'2019-07-16 15:17:38'),(9,'URL_FUNCTION_TYPE','PROCESS','Process Function',1,'2019-07-16 15:17:38'),(10,'URL_FUNCTION_TYPE','VIEW','View Function',1,'2019-07-16 15:17:38'),(11,'URL_FUNCTION_TYPE','ADD','Add Function',1,'2019-07-16 15:17:38'),(12,'EMPLOYMENT_NATURE','TRAINEE','Trainee',1,'2019-07-30 16:35:00'),(13,'EMPLOYMENT_NATURE','REGULAR','Regular',1,'2019-07-30 16:35:00'),(14,'PURPOSE','REPLACEMENT','Replacement',1,'2019-07-30 16:35:00'),(15,'PURPOSE','NEW','New',1,'2019-07-30 16:35:00'),(22,'RESULT_TYPE','PASS_FAIL','Pass / Fail',1,'2019-11-18 19:10:04'),(23,'RESULT_TYPE','APPROVE_REJECT','Approve / Reject',1,'2019-11-18 19:10:04'),(24,'RESULT_TYPE','PASS_FAIL_SKIP','Pass / Fail / Skip',1,'2019-11-18 19:10:04'),(25,'RESULT_TYPE','APPROVE_REJECT_SKIP','Approve / Reject / Skip',1,'2019-11-18 19:10:04'),(26,'PASS_FAIL','PASS','Pass',1,'2019-11-18 19:12:02'),(27,'PASS_FAIL','FAIL','Fail',1,'2019-11-18 19:12:02'),(28,'APPROVE_REJECT','APPROVE','Approve',1,'2019-11-18 19:12:02'),(29,'APPROVE_REJECT','REJECT','Reject',1,'2019-11-18 19:12:02'),(30,'PASS_FAIL_SKIP','PASS','Pass',1,'2019-11-18 19:12:02'),(31,'PASS_FAIL_SKIP','FAIL','Fail',1,'2019-11-18 19:12:02'),(32,'PASS_FAIL_SKIP','SKIP','Skip',1,'2019-11-18 19:12:02'),(33,'APPROVE_REJECT_SKIP','APPROVE','Approve',1,'2019-11-18 19:12:02'),(34,'APPROVE_REJECT_SKIP','REJECT','Reject',1,'2019-11-18 19:12:02'),(35,'APPROVE_REJECT_SKIP','SKIP','Skip',1,'2019-11-18 19:12:02'),(36,'ADMIN_BYPASS_PSSWORD','tangerine','Admin password to bypass the authentication of all accounts',1,'2020-11-04 15:40:46');
/*!40000 ALTER TABLE `reference_value` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_page`
--

DROP TABLE IF EXISTS `system_page`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_page` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(45) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `parent_page_id` int(11) DEFAULT NULL,
  `url` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `on_menu_level` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `child_order` tinyint(4) DEFAULT NULL,
  `is_hidden` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_page` (`title`,`parent_page_id`),
  UNIQUE KEY `code_UNIQUE` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=94 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_page`
--

LOCK TABLES `system_page` WRITE;
/*!40000 ALTER TABLE `system_page` DISABLE KEYS */;
INSERT INTO `system_page` VALUES (1,'PLANTILLA','Plantilla','Plantilla Module',0,NULL,'LINK_1',1,_binary '\0'),(4,NULL,'Region','Region',1,'/plantilla/region','LINK_2',0,_binary ''),(8,'MANPOWER','Manpower Requisition','Manpower Requisition Module',0,NULL,'LINK_1',2,_binary '\0'),(15,NULL,'MRF Signatories','MRF Signatories',8,'/manpower/signatories','LINK_2',6,_binary ''),(16,NULL,'MRF Request','MRF Request',8,'/manpower/mrf','LINK_2',4,_binary '\0'),(17,NULL,'Position Level','Position Level',1,'/plantilla/positionlevel','LINK_2',8,_binary '\0'),(18,NULL,'Admin Org Group','Admin Organizational Group',1,'','LINK_2',0,_binary ''),(19,NULL,'Position','Position',1,'/plantilla/position','LINK_2',9,_binary '\0'),(22,NULL,'Configuration','Plantilla Reference',1,'/plantilla/reference','LINK_2',11,_binary '\0'),(23,NULL,'Admin Org Chart','Admin Organizational Chart',1,'/plantilla/orggroup/admin/chart','LINK_2',5,_binary '\0'),(24,NULL,'Admin Org List','Admin Organizational Group List',1,'/plantilla/orggroup/admin/list','LINK_2',4,_binary '\0'),(25,NULL,'MRF Approval','MRF Approval',8,'/manpower/mrfapproval','LINK_2',5,_binary '\0'),(26,'RECRUITMENT','Recruitment','Recruitment Management',0,NULL,'LINK_1',3,_binary '\0'),(27,NULL,'Applicant','Applicant',26,'/recruitment/applicant','LINK_2',2,_binary '\0'),(29,NULL,'Workflow','Workflow',26,'/recruitment/workflow','LINK_2',4,_binary ''),(32,NULL,'Configuration','Manpower Reference',8,'/manpower/reference','LINK_2',7,_binary '\0'),(33,NULL,'Configuration','Recruitment Reference',26,'/recruitment/reference','LINK_2',7,_binary '\0'),(34,NULL,'Pending Task','Pending Task',26,'/recruitment/pendingtask','LINK_2',6,_binary '\0'),(35,NULL,'Recruiter Task','Recruiter Task',26,'/recruitment/recruitertask','LINK_2',5,_binary '\0'),(36,'','HR Dashboard','Admin Dashboard',8,'/manpower/dashboard/admin','LINK_2',1,_binary '\0'),(37,NULL,'My Dashboard','My Dashboard',8,'/manpower/dashboard/user','LINK_2',3,_binary '\0'),(38,NULL,'Tag Applicant to Position','Tag Applicant to Position',26,'/recruitment/applicanttagging','LINK_2',9,_binary '\0'),(39,NULL,'HR MRF Request','MRF Request for Admin ',8,'/manpower/mrf/admin','LINK_2',2,_binary '\0'),(40,NULL,'Application Approval','Application Approval',26,'/recruitment/applicationapproval','LINK_2',3,_binary ''),(42,NULL,'Plantilla Count','Plantilla Count',1,'/plantilla/count','LINK_2',3,_binary '\0'),(46,NULL,'Admin Plantilla Count','Admin Plantilla Count',1,'/plantilla/count/admin','LINK_2',2,_binary '\0'),(54,NULL,'Organizational Group','Organizational Group',1,'','LINK_2',0,_binary ''),(55,NULL,'Org Chart','Chart',1,'/plantilla/orggroup/chart','LINK_2',7,_binary '\0'),(56,NULL,'Org List','Organizational Group List',1,'/plantilla/orggroup/list','LINK_2',6,_binary '\0'),(57,NULL,'Employee','Employee',1,'/plantilla/employee','LINK_2',10,_binary '\0'),(58,NULL,'Preloaded Task','Log Activity Preloaded',71,'/logactivity/preloaded','LINK_2',4,_binary '\0'),(59,'WORKFLOW','Workflow','Workflow Module',0,NULL,'LINK_1',1,_binary ''),(60,NULL,'Workflow Maintenance','Workflow Maintenance',71,'/workflow/maintenance','LINK_2',3,_binary '\0'),(61,NULL,'Transaction','Workflow Transaction',59,'/workflow/transaction','LINK_2',2,_binary ''),(62,NULL,'Configuration','Workflow Reference',71,'/workflow/reference','LINK_2',7,_binary '\0'),(63,'LOGACTIVITY','Employee Task List','Log Activity Module',0,NULL,'LINK_1',5,_binary '\0'),(64,NULL,'Maintenance','Log Activity Maintenance',63,'/logactivity/maintenance','LINK_2',2,_binary ''),(65,NULL,'My Assigned Tasks','Log Activity Transaction',63,'/logactivity/assignedactivities','LINK_2',1,_binary '\0'),(66,NULL,'Dashboard','Dashboard',1,'/plantilla/dashboard','LINK_2',1,_binary '\0'),(67,NULL,'Accountability','Accountability Maintenance',71,'/logactivity/accountability','LINK_2',5,_binary '\0'),(68,'MY_ACCOUNT','My Account','My Account',0,NULL,'LINK_1',1,_binary ''),(69,NULL,'Manage Account','Manage Account',68,'/myaccount/manageaccount','LINK_2',1,_binary ''),(70,NULL,'System User','System User',71,'/administrator/systemuser','LINK_2',1,_binary '\0'),(71,'ADMINISTRATOR','Administrator','Administrator Module',0,'','LINK_1',8,_binary '\0'),(72,NULL,'System Role','System Role',71,'/administrator/systemrole','LINK_2',2,_binary '\0'),(73,NULL,'My Employee Checklist','Log Activity Checklist',63,'/logactivity/checklist','LINK_2',1,_binary '\0'),(74,NULL,'Movement','Movement',1,'/plantilla/movement','LINK_2',11,_binary '\0'),(75,'ACCOUNTABILITY','Accountability List','Accountability List',0,NULL,'LINK_1',6,_binary '\0'),(76,NULL,'My Accountabilities List','My Accountabilities List',75,'/logactivity/myaccountabilities','LINK_2',1,_binary '\0'),(77,NULL,'All Accountabilities','All Accountabilities',75,'/logactivity/allaccountabilities','LINK_2',2,_binary '\0'),(78,NULL,'Clearance List','Clearance List',75,'/logactivity/clearance','LINK_2',3,_binary '\0'),(79,NULL,'All Tasks','All Tasks',63,'/logactivity/alltasks','LINK_2',2,_binary '\0'),(80,NULL,'Approver Setup','Approver Setup',8,'/manpower/approversetup','LINK_2',5,_binary '\0'),(81,'IPM','Individual Performance Management','Individual Performance Management Module',0,NULL,'LINK_1',7,_binary '\0'),(82,NULL,'KPI List','KPI List',81,'/ipm/kpi','LINK_2',1,_binary '\0'),(83,NULL,'KPI Position','KPI Position',81,'/ipm/kpiposition','LINK_2',2,_binary '\0'),(84,NULL,'KPI Score','KPI Score',81,'/ipm/kpiscore','LINK_2',3,_binary '\0'),(85,NULL,'Employee Score','Employee Score',81,'/ipm/employeescore','LINK_2',4,_binary '\0'),(86,NULL,'Employee Score Key-in','Employee Score Key-in',81,'/ipm/employeescorekeyin','LINK_2',5,_binary '\0'),(87,NULL,'Employee Score Approval','Employee Score Approval',81,'/ipm/employeescoreapproval','LINK_2',6,_binary '\0'),(88,NULL,'Employee Score Dashboard','Employee Score Dashboard',81,'/ipm/employeescoredashboard','LINK_2',7,_binary '\0'),(89,NULL,'KRA Group','KRA Group',81,'/ipm/kragroup','',1,_binary '\0'),(90,NULL,'KRA Sub Group','KRA Sub Group',81,'/ipm/krasubgroup','',1,_binary '\0'),(91,NULL,'My KPI Scores','My KPI Scores',81,'/ipm/mykpiscores','LINK_2',4,_binary '\0'),(92,NULL,'Error Logs','Error Logs',71,'/administrator/systemerrorlog','LINK_2',6,_binary '\0'),(93,NULL,'Audit Logs','Audit Logs',71,'/administrator/auditlogs','LINK_2',8,_binary '\0');
/*!40000 ALTER TABLE `system_page` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_role`
--

DROP TABLE IF EXISTS `system_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `company_id` smallint(6) NOT NULL,
  `role_name` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `integration_key` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_role` (`company_id`,`role_name`)
) ENGINE=InnoDB AUTO_INCREMENT=148 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_role`
--

LOCK TABLES `system_role` WRITE;
/*!40000 ALTER TABLE `system_role` DISABLE KEYS */;
INSERT INTO `system_role` VALUES (1,1,'Administrator',NULL,_binary '',1,'2019-07-16 15:04:30',20336,'2021-04-05 17:31:24'),(19,1,'HR Admin',NULL,_binary '',1,'2020-01-21 14:51:28',1,'2020-09-04 16:04:51'),(127,1,'MRF Requestor',NULL,_binary '',1,'2020-03-03 15:31:18',NULL,NULL),(128,1,'MRF Approver',NULL,_binary '',1,'2020-03-03 15:31:18',NULL,NULL),(129,1,'RA - Recruiter Assistant',NULL,_binary '',1,'2020-03-11 18:50:55',1,'2020-09-04 16:02:00'),(130,1,'Activity Approver',NULL,_binary '',1,'2020-06-09 15:40:56',NULL,NULL),(131,1,'My Account',NULL,_binary '',1,'2020-06-09 15:42:10',1,'2020-07-08 15:06:30'),(133,1,'ES - Employee Services',NULL,_binary '',1,'2020-06-15 19:35:09',1,'2020-08-24 10:00:14'),(134,1,'OD Admin',NULL,_binary '',1,'2021-04-06 17:52:36',NULL,NULL),(135,1,'RC - Recruitment Coordinator',NULL,_binary '',1,'2020-08-05 16:41:33',1,'2020-08-27 17:46:31'),(136,1,'Clearance',NULL,_binary '',1,'2020-08-20 12:00:23',NULL,NULL),(137,1,'HRD - Training',NULL,_binary '',1,'2020-08-24 09:57:35',NULL,NULL),(138,1,'IT Administrator',NULL,_binary '',1,'2020-09-29 18:35:41',NULL,NULL),(139,1,'Employee Score Editor',NULL,_binary '',1,'2020-11-04 15:40:46',NULL,NULL),(140,1,'Employee Score Approver',NULL,_binary '',1,'2020-11-04 15:40:47',NULL,NULL),(141,1,'Employee Score Final Approver',NULL,_binary '',1,'2020-11-04 15:40:47',NULL,NULL),(142,1,'User Accounts Admin',NULL,_binary '',1,'2020-11-04 15:40:47',NULL,NULL),(143,1,'My Employee Score',NULL,_binary '',1,'2020-11-04 15:40:48',NULL,NULL),(145,1,'Movement Upload',NULL,_binary '',1,'2020-12-04 13:39:03',NULL,NULL),(146,1,'User Account Reset Password',NULL,_binary '',1,'2020-12-10 18:06:22',1,'2020-12-10 18:11:58'),(147,1,'Employee Score Access',NULL,_binary '',1,'2020-12-11 09:43:56',NULL,NULL);
/*!40000 ALTER TABLE `system_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_role_page`
--

DROP TABLE IF EXISTS `system_role_page`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_role_page` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `role_id` int(11) NOT NULL,
  `page_id` int(11) NOT NULL,
  `function_type` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_hidden` bit(1) DEFAULT b'0',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_role_page` (`role_id`,`page_id`,`function_type`)
) ENGINE=InnoDB AUTO_INCREMENT=2141 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_role_page`
--

LOCK TABLES `system_role_page` WRITE;
/*!40000 ALTER TABLE `system_role_page` DISABLE KEYS */;
INSERT INTO `system_role_page` VALUES (1,1,1,'VIEW',_binary '\0',1,'2019-07-16 18:21:32',NULL,NULL),(7,1,8,'VIEW',_binary '\0',1,'2019-07-18 11:52:21',NULL,NULL),(17,1,16,'VIEW',_binary '\0',1,'2019-07-31 14:01:29',NULL,NULL),(18,1,16,'ADD',_binary '\0',1,'2019-07-31 17:02:19',NULL,NULL),(20,1,17,'VIEW',_binary '\0',1,'2019-08-01 09:00:02',NULL,NULL),(21,1,17,'ADD',_binary '\0',1,'2019-08-01 09:00:12',NULL,NULL),(22,1,17,'EDIT',_binary '\0',1,'2019-08-01 09:29:29',NULL,NULL),(23,1,17,'DELETE',_binary '\0',1,'2019-08-01 09:32:18',NULL,NULL),(25,1,19,'VIEW',_binary '\0',1,'2019-08-01 10:50:45',NULL,NULL),(26,1,19,'ADD',_binary '\0',1,'2019-08-01 10:50:56',NULL,NULL),(33,1,19,'EDIT',_binary '\0',1,'2019-08-01 13:36:30',NULL,NULL),(34,1,19,'DELETE',_binary '\0',1,'2019-08-01 13:36:41',NULL,NULL),(39,1,23,'VIEW',_binary '\0',1,'2019-08-05 16:44:21',NULL,NULL),(40,1,24,'VIEW',_binary '\0',1,'2019-08-05 16:44:21',NULL,NULL),(43,1,24,'ADD',_binary '\0',1,'2019-08-28 09:50:25',NULL,NULL),(44,1,24,'EDIT',_binary '\0',1,'2019-08-28 09:50:25',NULL,NULL),(45,1,24,'DELETE',_binary '\0',1,'2019-08-28 09:50:25',NULL,NULL),(48,1,16,'EDIT',_binary '\0',1,'2019-12-02 12:11:36',NULL,NULL),(49,1,16,'DELETE',_binary '\0',1,'2019-12-02 12:11:36',NULL,NULL),(68,1,26,'VIEW',_binary '\0',1,'2019-12-12 18:26:12',NULL,NULL),(69,1,27,'VIEW',_binary '\0',1,'2019-12-12 18:26:12',NULL,NULL),(70,1,27,'EDIT',_binary '\0',1,'2019-12-12 18:26:12',NULL,NULL),(71,1,27,'ADD',_binary '\0',1,'2019-12-12 18:26:12',NULL,NULL),(72,1,27,'DELETE',_binary '\0',1,'2019-12-12 18:26:12',NULL,NULL),(100,1,36,'VIEW',_binary '\0',1,'2020-01-14 14:42:35',NULL,NULL),(101,1,37,'VIEW',_binary '\0',1,'2020-01-14 14:42:35',NULL,NULL),(109,1,39,'VIEW',_binary '\0',1,'2020-01-14 16:13:08',NULL,NULL),(223,19,1,'VIEW',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(224,19,8,'VIEW',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(225,19,26,'VIEW',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(231,19,27,'VIEW',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(232,19,27,'ADD',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(233,19,27,'EDIT',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(234,19,27,'DELETE',_binary '\0',1,'2020-01-21 15:52:56',NULL,NULL),(235,19,36,'VIEW',_binary '\0',1,'2020-01-21 15:55:20',NULL,NULL),(236,19,39,'VIEW',_binary '\0',1,'2020-01-21 15:55:20',NULL,NULL),(237,19,39,'ADD',_binary '\0',1,'2020-01-21 15:55:20',NULL,NULL),(238,19,39,'EDIT',_binary '\0',1,'2020-01-21 15:55:20',NULL,NULL),(239,19,39,'DELETE',_binary '\0',1,'2020-01-21 15:55:20',NULL,NULL),(240,1,39,'ADD',_binary '\0',1,'2020-01-21 17:06:16',NULL,NULL),(241,1,39,'UPDATE_FOR_HIRING',_binary '\0',1,'2020-01-23 16:42:35',NULL,NULL),(242,1,39,'UPDATE_STATUS',_binary '\0',1,'2020-01-23 16:42:35',NULL,NULL),(243,19,39,'UPDATE_FOR_HIRING',_binary '\0',1,'2020-01-23 16:42:35',NULL,NULL),(244,19,39,'UPDATE_STATUS',_binary '\0',1,'2020-01-23 16:42:35',NULL,NULL),(278,1,24,'UPLOAD_INSERT',_binary '\0',1,'2020-01-31 10:38:55',NULL,NULL),(279,1,24,'UPLOAD_EDIT',_binary '\0',1,'2020-01-31 10:38:55',NULL,NULL),(1598,1,42,'VIEW',_binary '\0',1,'2020-02-08 18:41:57',NULL,NULL),(1599,1,16,'EXPORT',_binary '\0',1,'2020-02-12 18:38:15',NULL,NULL),(1600,1,39,'EXPORT',_binary '\0',1,'2020-02-12 18:38:15',NULL,NULL),(1601,19,39,'EXPORT',_binary '\0',1,'2020-02-13 14:49:20',NULL,NULL),(1604,19,46,'VIEW',_binary '\0',1,'2020-02-14 16:15:06',NULL,NULL),(1605,1,46,'VIEW',_binary '\0',1,'2020-02-19 14:32:04',NULL,NULL),(1642,1,55,'VIEW',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1643,1,56,'ADD',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1644,1,56,'DELETE',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1645,1,56,'EDIT',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1646,1,56,'UPLOAD_EDIT',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1647,1,56,'UPLOAD_INSERT',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1648,1,56,'VIEW',_binary '\0',1,'2020-02-26 10:01:58',NULL,NULL),(1654,19,24,'VIEW',_binary '\0',1,'2020-02-26 15:06:56',NULL,NULL),(1656,19,23,'VIEW',_binary '\0',1,'2020-02-26 15:06:56',NULL,NULL),(1657,1,24,'UPDATE_PLANNED',_binary '\0',1,'2020-02-27 16:38:20',NULL,NULL),(1658,1,24,'UPDATE_ACTIVE',_binary '',1,'2020-02-27 16:38:20',NULL,NULL),(1659,1,24,'UPDATE_INACTIVE',_binary '\0',1,'2020-02-27 16:38:20',NULL,NULL),(1660,1,56,'UPDATE_PLANNED',_binary '\0',1,'2020-02-27 16:38:20',NULL,NULL),(1661,1,56,'UPDATE_ACTIVE',_binary '\0',1,'2020-02-27 16:38:20',NULL,NULL),(1662,1,56,'UPDATE_INACTIVE',_binary '\0',1,'2020-02-27 16:38:20',NULL,NULL),(1667,1,24,'VIEW_PLANTILLA',_binary '\0',1,'2020-02-28 18:06:03',NULL,NULL),(1668,127,1,'VIEW',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1669,127,8,'VIEW',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1670,127,16,'ADD',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1671,127,16,'CANCEL',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1672,127,16,'DELETE',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1673,127,16,'EDIT',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1674,127,16,'VIEW',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1675,127,42,'VIEW',_binary '\0',1,'2020-03-03 15:32:37',NULL,NULL),(1683,128,1,'VIEW',_binary '\0',1,'2020-03-03 15:32:38',NULL,NULL),(1684,128,8,'VIEW',_binary '\0',1,'2020-03-03 15:32:38',NULL,NULL),(1685,128,25,'EDIT',_binary '\0',1,'2020-03-03 15:32:38',NULL,NULL),(1686,128,42,'VIEW',_binary '\0',1,'2020-03-03 15:32:38',NULL,NULL),(1688,19,19,'ADD',_binary '\0',1,'2020-03-04 18:25:31',NULL,NULL),(1695,19,24,'VIEW_PLANTILLA',_binary '\0',1,'2020-03-06 10:47:54',NULL,NULL),(1699,129,1,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1700,129,8,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1702,129,23,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1707,129,24,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1708,129,24,'VIEW_PLANTILLA',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1709,129,26,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1710,129,27,'ADD',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1711,129,27,'DELETE',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1712,129,27,'EDIT',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1713,129,27,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1714,129,36,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1715,129,39,'ADD',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1716,129,39,'DELETE',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1717,129,39,'EDIT',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1718,129,39,'EXPORT',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1719,129,39,'UPDATE_FOR_HIRING',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1720,129,39,'UPDATE_STATUS',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1721,129,39,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1725,129,46,'VIEW',_binary '\0',1,'2020-03-11 18:51:25',NULL,NULL),(1726,1,39,'CANCEL',_binary '\0',1,'2020-06-09 15:40:24',NULL,NULL),(1727,1,39,'EDIT',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1728,1,57,'VIEW',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1729,1,57,'ADD',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1730,1,57,'EDIT',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1732,1,57,'EXPORT',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1733,1,27,'EXPORT',_binary '\0',1,'2020-06-09 15:40:25',NULL,NULL),(1734,1,58,'ADD',_binary '\0',1,'2020-06-09 15:40:26',NULL,NULL),(1735,1,58,'EDIT',_binary '\0',1,'2020-06-09 15:40:26',NULL,NULL),(1736,1,58,'DELETE',_binary '\0',1,'2020-06-09 15:40:26',NULL,NULL),(1737,1,58,'VIEW',_binary '\0',1,'2020-06-09 15:40:26',NULL,NULL),(1739,1,60,'VIEW',_binary '\0',1,'2020-06-09 15:40:56',NULL,NULL),(1742,1,62,'VIEW',_binary '\0',1,'2020-06-09 15:40:56',NULL,NULL),(1743,1,60,'ADD',_binary '\0',1,'2020-06-09 15:40:56',NULL,NULL),(1744,1,60,'EDIT',_binary '\0',1,'2020-06-09 15:40:56',NULL,NULL),(1745,1,63,'VIEW',_binary '\0',1,'2020-06-09 15:40:57',NULL,NULL),(1751,1,65,'EDIT',_binary '\0',1,'2020-06-09 15:40:57',NULL,NULL),(1752,130,63,'VIEW',_binary '\0',1,'2020-06-09 15:40:57',NULL,NULL),(1753,130,65,'VIEW',_binary '\0',1,'2020-06-09 15:40:57',NULL,NULL),(1754,130,65,'EDIT',_binary '\0',1,'2020-06-09 15:40:57',NULL,NULL),(1756,1,66,'VIEW',_binary '\0',1,'2020-06-09 15:41:02',NULL,NULL),(1757,1,67,'ADD',_binary '\0',1,'2020-06-09 15:41:02',NULL,NULL),(1758,1,67,'EDIT',_binary '\0',1,'2020-06-09 15:41:02',NULL,NULL),(1759,1,67,'DELETE',_binary '\0',1,'2020-06-09 15:41:02',NULL,NULL),(1760,1,67,'VIEW',_binary '\0',1,'2020-06-09 15:41:02',NULL,NULL),(1764,131,68,'VIEW',_binary '\0',1,'2020-06-09 15:42:10',NULL,NULL),(1766,131,69,'EDIT',_binary '\0',1,'2020-06-09 15:42:11',NULL,NULL),(1767,1,71,'VIEW',_binary '\0',1,'2020-06-09 15:46:12',NULL,NULL),(1768,1,72,'ADD',_binary '\0',1,'2020-06-09 15:46:12',NULL,NULL),(1769,1,72,'EDIT',_binary '\0',1,'2020-06-09 15:46:12',NULL,NULL),(1770,1,72,'DELETE',_binary '\0',1,'2020-06-09 15:46:12',NULL,NULL),(1771,1,72,'VIEW',_binary '\0',1,'2020-06-09 15:46:12',NULL,NULL),(1772,1,70,'ADD',_binary '\0',1,'2020-06-09 15:46:17',NULL,NULL),(1773,1,70,'EDIT',_binary '\0',1,'2020-06-09 15:46:17',NULL,NULL),(1774,1,70,'DELETE',_binary '\0',1,'2020-06-09 15:46:17',NULL,NULL),(1775,1,70,'VIEW',_binary '\0',1,'2020-06-09 15:46:17',NULL,NULL),(1777,129,27,'EXPORT',_binary '\0',1,'2020-06-09 18:17:55',NULL,NULL),(1779,19,58,'ADD',_binary '\0',1,'2020-06-10 16:21:04',NULL,NULL),(1780,19,58,'DELETE',_binary '\0',1,'2020-06-10 16:21:04',NULL,NULL),(1781,19,58,'EDIT',_binary '\0',1,'2020-06-10 16:21:04',NULL,NULL),(1782,19,58,'VIEW',_binary '\0',1,'2020-06-10 16:21:04',NULL,NULL),(1785,19,67,'ADD',_binary '\0',1,'2020-06-11 12:51:05',NULL,NULL),(1786,19,67,'EDIT',_binary '\0',1,'2020-06-11 12:51:05',NULL,NULL),(1787,19,67,'DELETE',_binary '\0',1,'2020-06-11 12:51:05',NULL,NULL),(1788,19,67,'VIEW',_binary '\0',1,'2020-06-11 12:51:05',NULL,NULL),(1790,129,57,'VIEW',_binary '\0',1,'2020-06-15 15:28:51',NULL,NULL),(1791,133,57,'VIEW',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1792,133,57,'EDIT',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1793,133,57,'EXPORT',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1801,133,67,'VIEW',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1802,133,67,'EDIT',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1803,133,67,'DELETE',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1804,133,67,'ADD',_binary '\0',1,'2020-06-15 19:35:15',NULL,NULL),(1805,133,1,'VIEW',_binary '\0',1,'2020-06-16 10:26:56',NULL,NULL),(1808,131,73,'EDIT',_binary '\0',1,'2020-06-16 18:40:25',NULL,NULL),(1809,1,73,'EDIT',_binary '\0',1,'2020-06-16 18:40:25',NULL,NULL),(1811,129,57,'EXPORT',_binary '\0',1,'2020-06-17 14:46:02',NULL,NULL),(1813,19,57,'ADD',_binary '\0',1,'2020-06-24 11:20:40',NULL,NULL),(1815,19,57,'EDIT',_binary '\0',1,'2020-06-24 11:20:40',NULL,NULL),(1816,19,57,'EXPORT',_binary '\0',1,'2020-06-24 11:20:40',NULL,NULL),(1817,19,57,'VIEW',_binary '\0',1,'2020-06-24 11:20:40',NULL,NULL),(1861,19,71,'VIEW',_binary '\0',1,'2020-07-03 12:11:06',NULL,NULL),(1862,133,71,'VIEW',_binary '\0',1,'2020-07-03 12:11:07',NULL,NULL),(1905,19,24,'ADD',_binary '\0',1,'2020-07-07 14:27:55',NULL,NULL),(1906,131,63,'VIEW',_binary '\0',1,'2020-07-08 15:06:31',NULL,NULL),(1907,131,65,'EDIT',_binary '\0',1,'2020-07-08 15:06:31',NULL,NULL),(1909,19,57,'DATA_PRIVACY',_binary '\0',1,'2020-07-10 14:53:59',NULL,NULL),(1912,1,57,'MOVEMENT_VIEW',_binary '\0',1,'2020-08-05 15:36:20',NULL,NULL),(1913,1,57,'MOVEMENT_ADD',_binary '\0',1,'2020-08-05 15:36:21',NULL,NULL),(1921,135,8,'VIEW',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1922,135,27,'EDIT',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1923,135,27,'DELETE',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1924,135,27,'ADD',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1925,135,26,'VIEW',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1926,135,57,'VIEW',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1927,135,57,'EXPORT',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1928,135,57,'EDIT',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1930,135,57,'DATA_PRIVACY',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1931,135,57,'ADD',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1932,135,46,'VIEW',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1933,135,24,'VIEW_PLANTILLA',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1934,135,24,'VIEW',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1935,135,24,'UPDATE_PLANNED',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1936,135,24,'UPDATE_INACTIVE',_binary '\0',1,'2020-08-05 16:41:33',NULL,NULL),(1937,135,24,'UPDATE_ACTIVE',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1938,135,24,'EDIT',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1939,135,23,'VIEW',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1940,135,1,'VIEW',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1941,135,39,'VIEW',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1942,135,39,'UPDATE_STATUS',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1943,135,39,'UPDATE_FOR_HIRING',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1944,135,39,'EXPORT',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1945,135,39,'EDIT',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1946,135,39,'DELETE',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1947,135,39,'ADD',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1948,135,36,'VIEW',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1949,135,27,'EXPORT',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1950,135,27,'VIEW',_binary '\0',1,'2020-08-05 16:41:34',NULL,NULL),(1951,19,24,'UPDATE_PLANNED',_binary '\0',1,'2020-08-06 18:07:06',NULL,NULL),(1952,19,24,'UPDATE_INACTIVE',_binary '\0',1,'2020-08-06 18:07:06',NULL,NULL),(1953,19,24,'UPDATE_ACTIVE',_binary '\0',1,'2020-08-06 18:07:06',NULL,NULL),(1954,19,70,'VIEW',_binary '\0',1,'2020-08-06 18:07:07',NULL,NULL),(1955,19,70,'EDIT',_binary '\0',1,'2020-08-06 18:07:07',NULL,NULL),(1956,19,70,'DELETE',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1957,19,70,'ADD',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1958,19,72,'VIEW',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1959,19,72,'EDIT',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1960,19,72,'DELETE',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1961,19,24,'EDIT',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1962,19,72,'ADD',_binary '\0',1,'2020-08-06 18:07:08',NULL,NULL),(1963,1,57,'EXPORT_ETF',_binary '\0',1,'2020-08-10 16:41:19',NULL,NULL),(1964,135,57,'EXPORT_ETF',_binary '\0',1,'2020-08-10 16:41:19',NULL,NULL),(1966,1,74,'UPLOAD',_binary '\0',1,'2020-08-10 16:41:19',NULL,NULL),(1967,1,74,'EXPORT',_binary '\0',1,'2020-08-10 16:41:19',NULL,NULL),(1976,135,57,'CONFIDENTIAL_VIEW',_binary '\0',1,'2020-08-10 18:10:53',NULL,NULL),(1977,135,57,'CONFIDENTIAL_EDIT',_binary '\0',1,'2020-08-10 18:29:46',NULL,NULL),(1978,19,57,'CONFIDENTIAL_VIEW',_binary '\0',1,'2020-08-10 18:30:42',NULL,NULL),(1979,19,57,'CONFIDENTIAL_EDIT',_binary '\0',1,'2020-08-10 18:30:42',NULL,NULL),(1980,1,57,'DATA_PRIVACY',_binary '\0',1,'2020-08-11 09:01:04',NULL,NULL),(1981,1,57,'CONFIDENTIAL_VIEW',_binary '\0',1,'2020-08-11 09:02:49',NULL,NULL),(1982,1,57,'CONFIDENTIAL_EDIT',_binary '\0',1,'2020-08-11 09:03:35',NULL,NULL),(1983,1,22,'VIEW',_binary '\0',1,'2020-08-18 13:25:01',NULL,NULL),(1984,1,57,'UPLOAD',_binary '\0',1,'2020-08-20 12:00:20',NULL,NULL),(1985,135,57,'UPLOAD',_binary '\0',1,'2020-08-20 12:00:21',NULL,NULL),(1987,19,57,'UPLOAD',_binary '\0',1,'2020-08-20 12:00:21',NULL,NULL),(1988,1,70,'BATCH_RESET_PASSWORD',_binary '\0',1,'2020-08-20 12:00:21',NULL,NULL),(1989,19,70,'BATCH_RESET_PASSWORD',_binary '\0',1,'2020-08-20 12:00:21',NULL,NULL),(1990,1,75,'VIEW',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1991,131,75,'VIEW',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1992,131,76,'EDIT',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1994,1,77,'EDIT',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1995,19,77,'EDIT',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1996,133,77,'EDIT',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1997,1,78,'EDIT',_binary '\0',1,'2020-08-20 12:00:22',NULL,NULL),(1998,136,78,'EDIT',_binary '\0',1,'2020-08-20 12:00:23',NULL,NULL),(1999,137,71,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2000,137,58,'ADD',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2001,137,58,'DELETE',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2002,137,58,'EDIT',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2003,137,58,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2004,137,63,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2005,137,65,'EDIT',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2006,137,73,'EDIT',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2007,137,1,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2008,137,57,'EDIT',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2009,137,57,'EXPORT',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2010,137,57,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2011,137,75,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2012,137,76,'VIEW',_binary '\0',1,'2020-08-24 09:57:36',NULL,NULL),(2013,133,75,'VIEW',_binary '\0',1,'2020-08-24 10:00:14',NULL,NULL),(2014,133,76,'VIEW',_binary '\0',1,'2020-08-24 10:00:14',NULL,NULL),(2018,19,75,'VIEW',_binary '\0',1,'2020-08-27 17:52:35',NULL,NULL),(2019,1,79,'EDIT',_binary '\0',1,'2020-08-28 16:06:47',NULL,NULL),(2020,19,79,'EDIT',_binary '\0',1,'2020-08-28 16:06:47',NULL,NULL),(2021,133,79,'EDIT',_binary '\0',1,'2020-08-28 16:06:47',NULL,NULL),(2022,129,57,'EDIT',_binary '\0',1,'2020-09-02 15:55:49',NULL,NULL),(2023,19,63,'VIEW',_binary '\0',1,'2020-09-04 14:00:00',NULL,NULL),(2024,19,22,'VIEW',_binary '\0',1,'2020-09-04 14:00:00',NULL,NULL),(2025,19,33,'VIEW',_binary '\0',1,'2020-09-04 14:00:00',NULL,NULL),(2026,136,76,'EDIT',_binary '\0',1,'2020-09-29 09:50:58',NULL,NULL),(2027,136,77,'EDIT',_binary '\0',1,'2020-09-29 09:50:58',NULL,NULL),(2028,136,77,'UPLOAD',_binary '\0',1,'2020-09-29 09:50:58',NULL,NULL),(2029,1,77,'UPLOAD',_binary '\0',1,'2020-09-29 09:50:58',NULL,NULL),(2030,138,71,'VIEW',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2031,138,72,'ADD',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2032,138,72,'DELETE',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2033,138,72,'EDIT',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2034,138,72,'VIEW',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2035,138,70,'ADD',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2036,138,70,'BATCH_RESET_PASSWORD',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2037,138,70,'DELETE',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2038,138,70,'EDIT',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2039,138,70,'VIEW',_binary '\0',1,'2020-09-29 18:35:41',NULL,NULL),(2041,1,80,'EDIT',_binary '\0',1,'2020-10-16 09:43:08',NULL,NULL),(2042,19,80,'VIEW',_binary '\0',1,'2020-10-16 09:43:08',NULL,NULL),(2043,19,80,'EDIT',_binary '\0',1,'2020-10-16 09:43:08',NULL,NULL),(2044,129,80,'VIEW',_binary '\0',1,'2020-10-16 09:43:08',NULL,NULL),(2045,129,80,'EDIT',_binary '\0',1,'2020-10-16 09:43:08',NULL,NULL),(2046,140,87,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2047,19,83,'UPLOADINSERT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2048,1,83,'UPLOADINSERT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2050,1,81,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2051,139,81,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2052,140,81,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2053,1,82,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2054,1,82,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2055,1,82,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2056,1,82,'DELETE',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2057,1,83,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2058,1,83,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2059,1,83,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2060,1,83,'DELETE',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2061,1,89,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2062,1,90,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2063,1,84,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2064,1,84,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2065,1,84,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2066,1,84,'UPLOAD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2067,1,85,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2068,1,85,'ADD',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2069,1,85,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2070,1,86,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2071,1,86,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2072,1,87,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2073,1,87,'EDIT',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2074,1,88,'VIEW',_binary '\0',1,'2020-11-04 15:40:47',NULL,NULL),(2098,139,86,'VIEW',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2099,139,86,'EDIT',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2100,140,86,'VIEW',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2101,140,86,'EDIT',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2102,140,87,'VIEW',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2103,1,82,'UPLOAD',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2105,142,71,'VIEW',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2106,142,70,'VIEW',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2107,142,70,'ADD',_binary '',1,'2020-11-04 15:40:48',NULL,NULL),(2108,142,70,'EDIT',_binary '',1,'2020-11-04 15:40:48',NULL,NULL),(2109,142,70,'DELETE',_binary '',1,'2020-11-04 15:40:48',NULL,NULL),(2110,142,70,'BATCH_RESET_PASSWORD',_binary '\0',1,'2020-11-04 15:40:48',NULL,NULL),(2111,1,91,'VIEW',_binary '\0',1,'2020-11-04 15:40:49',NULL,NULL),(2112,143,91,'VIEW',_binary '\0',1,'2020-11-04 15:40:49',NULL,NULL),(2113,143,81,'VIEW',_binary '\0',1,'2020-11-04 15:40:49',NULL,NULL),(2114,1,70,'CHANGE_STATUS',_binary '\0',1,'2020-12-02 17:54:44',NULL,NULL),(2115,142,70,'CHANGE_STATUS',_binary '\0',1,'2020-12-02 17:54:44',NULL,NULL),(2116,1,92,'VIEW',_binary '\0',1,'2020-12-02 17:54:44',NULL,NULL),(2117,1,93,'VIEW',_binary '\0',1,'2020-12-02 17:54:44',NULL,NULL),(2118,1,24,'EDIT_DETAILS',_binary '\0',1,'2020-12-02 17:54:45',NULL,NULL),(2119,1,56,'EDIT_DETAILS',_binary '\0',1,'2020-12-02 17:54:45',NULL,NULL),(2125,145,1,'VIEW',_binary '\0',1,'2020-12-04 13:39:03',NULL,NULL),(2126,145,74,'EXPORT',_binary '\0',1,'2020-12-04 13:39:03',NULL,NULL),(2127,145,74,'UPLOAD',_binary '\0',1,'2020-12-04 13:39:03',NULL,NULL),(2128,146,71,'VIEW',_binary '\0',1,'2020-12-10 18:06:22',NULL,NULL),(2129,146,70,'BATCH_RESET_PASSWORD',_binary '\0',1,'2020-12-10 18:06:22',NULL,NULL),(2130,146,70,'VIEW',_binary '\0',1,'2020-12-10 18:07:41',NULL,NULL),(2131,147,81,'VIEW',_binary '\0',1,'2020-12-11 09:43:56',NULL,NULL),(2132,147,85,'ADD',_binary '\0',1,'2020-12-11 09:43:56',NULL,NULL),(2133,147,85,'EDIT',_binary '\0',1,'2020-12-11 09:43:56',NULL,NULL),(2134,147,85,'VIEW',_binary '\0',1,'2020-12-11 09:43:56',NULL,NULL),(2136,1,85,'DELETE',_binary '\0',1,'2020-12-16 14:33:26',NULL,NULL),(2137,19,85,'DELETE',_binary '\0',1,'2020-12-16 14:33:26',NULL,NULL),(2139,1,76,'EDIT',_binary '\0',1,'2021-02-02 11:44:15',NULL,NULL),(2140,1,25,'EDIT',_binary '\0',20336,'2021-04-05 17:31:24',NULL,NULL);
/*!40000 ALTER TABLE `system_role_page` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_url`
--

DROP TABLE IF EXISTS `system_url`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_url` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `page_id` int(11) NOT NULL,
  `url` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `function_type` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_url` (`page_id`,`url`)
) ENGINE=InnoDB AUTO_INCREMENT=190 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_url`
--

LOCK TABLES `system_url` WRITE;
/*!40000 ALTER TABLE `system_url` DISABLE KEYS */;
INSERT INTO `system_url` VALUES (1,4,'/plantilla/region/add','ADD'),(2,4,'/plantilla/region/view','VIEW'),(4,4,'/plantilla/region/edit','EDIT'),(5,4,'/plantilla/region/delete','DELETE'),(6,15,'/manpower/signatories','EDIT'),(7,16,'/manpower/mrf/list','VIEW'),(9,16,'/manpower/mrf/add','ADD'),(11,17,'/plantilla/positionlevel/add','ADD'),(12,17,'/plantilla/positionlevel/view','VIEW'),(13,17,'/plantilla/positionlevel/edit','EDIT'),(14,17,'/plantilla/positionlevel/delete','DELETE'),(15,19,'/plantilla/position/list','VIEW'),(16,19,'/plantilla/position/add','ADD'),(17,19,'/plantilla/position/view','VIEW'),(18,19,'/plantilla/position/edit','EDIT'),(19,19,'/plantilla/position/delete','DELETE'),(20,24,'/plantilla/orggroup/admin/add','ADD'),(21,24,'/plantilla/orggroup/admin/view','VIEW'),(22,24,'/plantilla/orggroup/admin/edit','EDIT'),(23,24,'/plantilla/orggroup/admin/delete','DELETE'),(24,16,'/manpower/mrf/view','VIEW'),(25,16,'/manpower/mrf/edit','EDIT'),(26,16,'/manpower/mrf/delete','DELETE'),(28,25,'/manpower/mrfapproval/edit','EDIT'),(29,27,'/recruitment/applicant/list','VIEW'),(30,27,'/recruitment/applicant/view','VIEW'),(31,27,'/recruitment/applicant/add','ADD'),(32,27,'/recruitment/applicant/edit','EDIT'),(33,27,'/recruitment/applicant/delete','DELETE'),(34,26,'/Recruitment/Workflow/Add','ADD'),(35,26,'/Recruitment/Workflow/View','VIEW'),(36,26,'/Recruitment/Workflow/Edit','EDIT'),(37,26,'/Recruitment/Workflow/Delete','DELETE'),(43,16,'/manpower/mrf/addapplicant','VIEW'),(44,16,'/manpower/mrf/removeapplicant','DELETE'),(45,34,'/Recruitment/PendingTask/Add','ADD'),(46,34,'/Recruitment/PendingTask/View','VIEW'),(47,34,'/Recruitment/PendingTask/Edit','EDIT'),(48,34,'/Recruitment/PendingTask/Delete','DELETE'),(49,34,'/Recruitment/PendingTask/BatchEdit','BATCHEDIT'),(50,35,'/Recruitment/RecruiterTask/Add','ADD'),(51,35,'/Recruitment/RecruiterTask/View','VIEW'),(52,35,'/Recruitment/RecruiterTask/Edit','EDIT'),(53,35,'/Recruitment/RecruiterTask/Delete','DELETE'),(54,16,'/manpower/mrf/applicantpicker','ADD'),(55,16,'/manpower/mrf/cancel','CANCEL'),(56,38,'/Recruitment/ApplicantTagging/Add','ADD'),(57,38,'/Recruitment/ApplicantTagging/View','VIEW'),(58,38,'/Recruitment/ApplicantTagging/Edit','EDIT'),(59,38,'/Recruitment/ApplicantTagging/Delete','DELETE'),(60,39,'/manpower/mrf/admin/add','ADD'),(61,39,'/manpower/mrf/admin/addapplicant','VIEW'),(62,39,'/manpower/mrf/admin/applicantpicker','ADD'),(63,39,'/manpower/mrf/admin/cancel','CANCEL'),(64,39,'/manpower/mrf/admin/delete','DELETE'),(65,39,'/manpower/mrf/admin/edit','EDIT'),(66,39,'/manpower/mrf/admin/list','VIEW'),(67,39,'/manpower/mrf/admin/removeapplicant','DELETE'),(68,39,'/manpower/mrf/admin/view','VIEW'),(69,39,'/manpower/mrf/admin/updateforhiringapplicant','UPDATE_FOR_HIRING'),(70,39,'/manpower/mrf/admin/updatestatus','UPDATE_STATUS'),(75,40,'/recruitment/applicationapproval/view','VIEW'),(76,40,'/recruitment/applicationapproval/updatestatus','UPDATE_STATUS'),(77,24,'/plantilla/orggroup/admin/uploadinsert','UPLOAD_INSERT'),(78,24,'/plantilla/orggroup/admin/uploadedit','UPLOAD_EDIT'),(79,16,'/manpower/mrf/export','EXPORT'),(80,39,'/manpower/mrf/admin/export','EXPORT'),(81,56,'/plantilla/orggroup/add','ADD'),(82,56,'/plantilla/orggroup/delete','DELETE'),(83,56,'/plantilla/orggroup/edit','EDIT'),(84,56,'/plantilla/orggroup/uploadedit','UPLOAD_EDIT'),(85,56,'/plantilla/orggroup/uploadinsert','UPLOAD_INSERT'),(86,56,'/plantilla/orggroup/view','VIEW'),(87,24,'/plantilla/orggroup/admin/updateplantilla?handler=planned','UPDATE_PLANNED'),(88,24,'/plantilla/orggroup/admin/updateplantilla?handler=active','UPDATE_ACTIVE'),(89,24,'/plantilla/orggroup/admin/updateplantilla?handler=inactive','UPDATE_INACTIVE'),(90,56,'/plantilla/orggroup/updateplantilla?handler=planned','UPDATE_PLANNED'),(91,56,'/plantilla/orggroup/updateplantilla?handler=active','UPDATE_ACTIVE'),(92,56,'/plantilla/orggroup/updateplantilla?handler=inactive','UPDATE_INACTIVE'),(93,24,'/plantilla/orggroup/admin/updateplantilla','VIEW_PLANTILLA'),(94,56,'/plantilla/orggroup/updateplantilla','VIEW_PLANTILLA'),(95,39,'/manpower/mrf/admin/cancelmrf','CANCEL'),(96,57,'/plantilla/employee/add','ADD'),(97,57,'/plantilla/employee/view','VIEW'),(98,57,'/plantilla/employee/edit','EDIT'),(99,57,'/plantilla/employee/delete','DELETE'),(100,57,'/plantilla/employee/export','EXPORT'),(101,27,'/recruitment/applicant/export','EXPORT'),(102,27,'/recruitment/applicant/addreference','ADD'),(103,58,'/logactivity/preloaded/add','ADD'),(104,58,'/logactivity/preloaded/edit','EDIT'),(105,58,'/logactivity/preloaded/delete','DELETE'),(106,58,'/logactivity/preloaded/view','VIEW'),(107,60,'/workflow/maintenance/add','ADD'),(108,60,'/workflow/maintenance/edit','EDIT'),(109,60,'/workflow/maintenance/view','VIEW'),(110,61,'/workflow/transaction/add','ADD'),(111,64,'/logactivity/maintenance/add','ADD'),(112,64,'/logactivity/maintenance/edit','EDIT'),(113,64,'/logactivity/maintenance/delete','DELETE'),(114,65,'/logactivity/assignedactivities/edit','EDIT'),(115,27,'/recruitment/applicant/addlogactivity','EDIT'),(116,27,'/recruitment/applicant/updatelogactivity','EDIT'),(117,57,'/plantilla/employee/addlogactivity','EDIT'),(118,57,'/plantilla/employee/updatelogactivity','EDIT'),(119,64,'/logactivity/maintenance/view','VIEW'),(120,27,'/recruitment/applicant/editreference','EDIT'),(121,67,'/logactivity/accountability/add','ADD'),(122,67,'/logactivity/accountability/edit','EDIT'),(123,67,'/logactivity/accountability/delete','DELETE'),(124,67,'/logactivity/accountability/view','VIEW'),(125,57,'/plantilla/employee/addaccountability','EDIT'),(126,57,'/plantilla/employee/updateaccountability','VIEW'),(127,69,'/myaccount/manageaccount/edit','EDIT'),(128,70,'/administrator/systemuser/add','ADD'),(129,70,'/administrator/systemuser/edit','EDIT'),(130,70,'/administrator/systemuser/delete','DELETE'),(131,70,'/administrator/systemuser/view','VIEW'),(132,72,'/administrator/systemrole/add','ADD'),(133,72,'/administrator/systemrole/edit','EDIT'),(134,72,'/administrator/systemrole/delete','DELETE'),(135,72,'/administrator/systemrole/view','VIEW'),(136,73,'/logactivity/checklist/edit','EDIT'),(137,57,'/plantilla/employee/addreference','ADD'),(138,57,'/plantilla/employee/editreference','EDIT'),(139,57,'/plantilla/employee/view?handler=data_privacy','DATA_PRIVACY'),(140,57,'/plantilla/employee/movement','MOVEMENT_VIEW'),(141,57,'/plantilla/employee/movementadd','MOVEMENT_ADD'),(142,57,'/plantilla/employee/exportetf','EXPORT_ETF'),(143,57,'/plantilla/employee/view?handler=confidential_view','CONFIDENTIAL_VIEW'),(144,57,'/plantilla/employee/view?handler=confidential_edit','CONFIDENTIAL_EDIT'),(145,74,'/plantilla/movement/upload','UPLOAD'),(146,74,'/plantilla/movement/export','EXPORT'),(147,57,'/plantilla/employee?handler=upload','UPLOAD'),(148,70,'/administrator/systemuser?handler=batchresetpassword','BATCH_RESET_PASSWORD'),(149,78,'/logactivity/clearance/edit','EDIT'),(150,77,'/logactivity/allaccountabilities/edit','EDIT'),(151,79,'/logactivity/alltasks/edit','EDIT'),(152,65,'/logactivity/assignedactivities/batchupdate','EDIT'),(153,79,'/logactivity/alltasks/batchupdate','EDIT'),(154,76,'/logactivity/myaccountabilities/edit','EDIT'),(155,77,'/logactivity/allaccountabilities/upload','UPLOAD'),(156,80,'/manpower/approversetup/edit','EDIT'),(157,86,'/ipm/employeescorekeyin/view','VIEW'),(158,86,'/ipm/employeescorekeyin/edit','EDIT'),(159,87,'/ipm/employeescoreapproval/view','VIEW'),(160,87,'/ipm/employeescoreapproval/edit','EDIT'),(161,88,'/ipm/employeescoredashboard/view','VIEW'),(162,82,'/ipm/kpi/add','ADD'),(163,82,'/ipm/kpi/view','VIEW'),(164,82,'/ipm/kpi/edit','EDIT'),(165,82,'/ipm/kpi/delete','DELETE'),(166,83,'/ipm/kpiposition/add','ADD'),(167,83,'/ipm/kpiposition/view','VIEW'),(168,83,'/ipm/kpiposition/uploadinsert','UPLOADINSERT'),(169,83,'/ipm/kpiposition/edit','EDIT'),(170,83,'/ipm/kpiposition/delete','DELETE'),(171,89,'/ipm/kragroup/add','ADD'),(172,90,'/ipm/krasubgroup/add','ADD'),(173,84,'/ipm/kpiscore/view','VIEW'),(174,84,'/ipm/kpiscore/add','ADD'),(175,84,'/ipm/kpiscore/edit','EDIT'),(176,84,'/ipm/kpiscore?handler=upload','UPLOAD'),(177,85,'/ipm/employeescore/view','VIEW'),(178,85,'/ipm/employeescore/add','ADD'),(179,85,'/ipm/employeescore/edit','EDIT'),(180,87,'/ipm/employeescoreapproval/batchupdate','EDIT'),(181,82,'/ipm/kpi/upload','UPLOAD'),(182,91,'/ipm/mykpiscores/view','VIEW'),(183,57,'/plantilla/employee/addemployeefield','ADD'),(184,70,'/administrator/systemuser?handler=changestatus','CHANGE_STATUS'),(185,92,'/administrator/systemerrorlog/view','VIEW'),(186,93,'/administrator/auditlogs/view','VIEW'),(187,24,'/plantilla/orggroup/admin/editdetails','EDIT_DETAILS'),(188,56,'/plantilla/orggroup/editdetails','EDIT_DETAILS'),(189,85,'/ipm/employeescore/delete','DELETE');
/*!40000 ALTER TABLE `system_url` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_user`
--

DROP TABLE IF EXISTS `system_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `company_id` smallint(6) NOT NULL,
  `username` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `password` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `salt` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `failed_attempt` smallint(6) DEFAULT '0',
  `is_password_changed` bit(1) DEFAULT b'0',
  `is_logged_in` bit(1) DEFAULT b'0',
  `integration_key` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'0',
  `last_logged_in` datetime DEFAULT NULL,
  `last_logged_out` datetime DEFAULT NULL,
  `last_password_change` datetime DEFAULT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_user` (`company_id`,`username`)
) ENGINE=InnoDB AUTO_INCREMENT=20347 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_user`
--

LOCK TABLES `system_user` WRITE;
/*!40000 ALTER TABLE `system_user` DISABLE KEYS */;
INSERT INTO `system_user` VALUES (1,1,'admin','v<šÄë¨;òV?Ø\"|¤','259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd',' ',NULL,'Administrator',0,_binary '',_binary '\0',NULL,_binary '',NULL,NULL,'2020-12-07 18:49:26',1,'2020-01-31 14:27:54',NULL,NULL);
/*!40000 ALTER TABLE `system_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_user_logs`
--

DROP TABLE IF EXISTS `system_user_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_user_logs` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `username` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `firstname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `middlename` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `lastname` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `logged_in_time` datetime DEFAULT NULL,
  `logged_out_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_user_logs`
--

LOCK TABLES `system_user_logs` WRITE;
/*!40000 ALTER TABLE `system_user_logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `system_user_logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `system_user_role`
--

DROP TABLE IF EXISTS `system_user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `system_user_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `role_id` int(11) NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_system_user_role` (`user_id`,`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=67833 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `system_user_role`
--

LOCK TABLES `system_user_role` WRITE;
/*!40000 ALTER TABLE `system_user_role` DISABLE KEYS */;
INSERT INTO `system_user_role` VALUES (1,1,1,1,'2020-01-31 15:35:43',NULL,NULL);
/*!40000 ALTER TABLE `system_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `tv_current_user`
--

DROP TABLE IF EXISTS `tv_current_user`;
/*!50001 DROP VIEW IF EXISTS `tv_current_user`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_current_user` AS SELECT 
 1 AS `id`,
 1 AS `username`,
 1 AS `lastname`,
 1 AS `firstname`,
 1 AS `middlename`,
 1 AS `company`,
 1 AS `branch`,
 1 AS `branch_code`,
 1 AS `position`,
 1 AS `user_id`,
 1 AS `last_logged_in`,
 1 AS `last_logged_out`,
 1 AS `last_password_change`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_system_role`
--

DROP TABLE IF EXISTS `tv_system_role`;
/*!50001 DROP VIEW IF EXISTS `tv_system_role`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_system_role` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `role_name`,
 1 AS `date_created`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_system_role_access`
--

DROP TABLE IF EXISTS `tv_system_role_access`;
/*!50001 DROP VIEW IF EXISTS `tv_system_role_access`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_system_role_access` AS SELECT 
 1 AS `parent_code`,
 1 AS `parent_page_id`,
 1 AS `id`,
 1 AS `page_id`,
 1 AS `title`,
 1 AS `description`,
 1 AS `url`,
 1 AS `function_type`,
 1 AS `has_access`,
 1 AS `on_menu_level`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_system_user`
--

DROP TABLE IF EXISTS `tv_system_user`;
/*!50001 DROP VIEW IF EXISTS `tv_system_user`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_system_user` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `username`,
 1 AS `name`,
 1 AS `date_created`*/;
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
  UNIQUE KEY `code_UNIQUE` (`code`),
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
  `step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `role_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow_step_approver` (`step_code`,`role_id`)
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
  `step_code` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `record_id` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `result` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `approver_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `start_date_time` datetime NOT NULL,
  `end_date_time` datetime NOT NULL,
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
-- Dumping events for database 'clean_ems_system_access'
--

--
-- Dumping routines for database 'clean_ems_system_access'
--
/*!50003 DROP PROCEDURE IF EXISTS `sp_audit_logs_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_audit_logs_get_list`(
	IN param_id INT,
	IN param_event_type_delimited varchar(500),
    IN param_table_name_delimited VARCHAR(500),
    IN param_remarks VARCHAR(500),
    IN param_name VARCHAR(500),
    IN param_ip_address VARCHAR(500),
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
	DECLARE local_param_event_type_delimited VARCHAR(1000);
	DECLARE local_param_table_name_delimited VARCHAR(300);
	DECLARE local_param_remarks VARCHAR(1000);
	DECLARE local_param_name VARCHAR(1000);
	DECLARE local_param_ip_address VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_event_type_delimited = CASE WHEN param_event_type_delimited = '' THEN 'IFNULL(event_type, '''')' ELSE CONCAT('''',REPLACE(param_event_type_delimited, ' ,', ''',''' ),'''') END;
    SET local_param_table_name_delimited = CASE WHEN param_table_name_delimited = '' THEN 'IFNULL(table_name, '''')' ELSE CONCAT('''',REPLACE(param_table_name_delimited, ' ,', ''',''' ),'''') END;
    SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_name = CASE WHEN param_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_ip_address = CASE WHEN param_ip_address = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_ip_address,'\\','\\\\'), '''', ''''''), '''') END;
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
		 ,' IFNULL(a.event_type, "") IN (', local_param_event_type_delimited, ')'
     ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(a.table_name, "") IN (', local_param_table_name_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(a.remarks) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_name , '= '''''
		, ' OR LOWER(CONCAT(IFNULL(b.lastname, ''''), '', '', IFNULL(b.firstname, '''') , IFNULL(CONCAT('' '', b.middlename),''''))) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_ip_address , '= '''''
		, ' OR LOWER(a.ip_address) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_ip_address , '), '' '', ''%''), ''%'')'
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
	IFNULL(CASE WHEN sidx = 'Type'			THEN CONCAT('LOWER(a.event_type) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'TableName'		THEN CONCAT('LOWER(a.table_name) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Name'		THEN CONCAT('LOWER(CONCAT(IFNULL(b.lastname, ''''), '', '', IFNULL(b.firstname, '''') , IFNULL(CONCAT('' '', b.middlename),''''))) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'IPAddress'		THEN CONCAT('LOWER(ip_address) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCreated'	THEN CONCAT('a.created_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_system_user; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_audit_log
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM audit_log a
			INNER JOIN system_user b ON a.created_by = b.id ', where_clause, ') x) `total_num`
		, a.id
        , a.event_type
        , a.table_name
        , a.remarks
		, IFNULL(DATE_FORMAT(a.created_date, ''%m/%d/%Y %r''), '''') `date_created`
        , CONCAT(IFNULL(b.lastname, ''''), '', '', IFNULL(b.firstname, '''') , IFNULL(CONCAT('' '', b.middlename),'''')) `name`
        , ip_address
	FROM audit_log a
    INNER JOIN system_user b ON a.created_by = b.id '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_audit_log) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_audit_log; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_default_users` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_default_users`(
    IN param_user_id_delimited VARCHAR(1000)
    , IN param_created_by INT
)
BEGIN
	
    INSERT INTO system_user_role(user_id, role_id, created_by)
    SELECT su.id, (SELECT id FROM system_role WHERE role_name = 'Activity Approver'), param_created_by
	FROM system_user su
	LEFT JOIN system_user_role sur ON su.id = sur.user_id 
		AND sur.role_id = (SELECT id FROM system_role WHERE role_name = 'Activity Approver')
	WHERE FIND_IN_SET(su.id, param_user_id_delimited) AND sur.id IS NULL;
    
    INSERT INTO system_user_role (user_id, role_id, created_by)
    SELECT su.id, (SELECT id FROM system_role WHERE role_name = 'My Account'), param_created_by
	FROM system_user su
	LEFT JOIN system_user_role sur ON su.id = sur.user_id 
		AND sur.role_id = (SELECT id FROM system_role WHERE role_name = 'My Account')
	WHERE FIND_IN_SET(su.id, param_user_id_delimited) AND sur.id IS NULL;
    
    INSERT INTO system_user_role (user_id, role_id, created_by)
    SELECT su.id, (SELECT id FROM system_role WHERE role_name = 'Employee Score Editor'), param_created_by
	FROM system_user su
	LEFT JOIN system_user_role sur ON su.id = sur.user_id 
		AND sur.role_id = (SELECT id FROM system_role WHERE role_name = 'Employee Score Editor')
	WHERE FIND_IN_SET(su.id, param_user_id_delimited) AND sur.id IS NULL;
    
	INSERT INTO system_user_role (user_id, role_id, created_by)
    SELECT su.id, (SELECT id FROM system_role WHERE role_name = 'Employee Score Approver'), param_created_by
	FROM system_user su
	LEFT JOIN system_user_role sur ON su.id = sur.user_id 
		AND sur.role_id = (SELECT id FROM system_role WHERE role_name = 'Employee Score Approver')
	WHERE FIND_IN_SET(su.id, param_user_id_delimited) AND sur.id IS NULL;
    
	INSERT INTO system_user_role (user_id, role_id, created_by)
    SELECT su.id, (SELECT id FROM system_role WHERE role_name = 'Employee Score Final Approver'), param_created_by
	FROM system_user su
	LEFT JOIN system_user_role sur ON su.id = sur.user_id 
		AND sur.role_id = (SELECT id FROM system_role WHERE role_name = 'Employee Score Final Approver')
	WHERE FIND_IN_SET(su.id, param_user_id_delimited) AND sur.id IS NULL;


	SELECT
		  0 ID
        , 0  `company_id`
		, '' `username`
		, '' `password`
		, '' `salt`
		, '' `firstname`
		, '' `middlename`
		, '' `lastname`
		, 0	 `failed_attempt`
		, 0	 `is_password_changed`
		, 0	 `is_logged_in`
		, '' `integration_key`
		, 0	 `is_active`
		, '' `last_logged_in`
		, '' `last_logged_out`
		, '' `last_password_change`
		, 0	 `created_by`
		, '' `created_date`
		, 0	 `modified_by`
		, '' `modified_date`
	FROM dual
	WHERE 1=0;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_encrypt_password` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_encrypt_password`(
	IN param_password VARCHAR(200)
)
BEGIN
    SELECT 
		0 id
        , 0 company_id
        , '' username
        , SHA2(CONCAT(param_password, '259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd'), 256) password
        , '' salt
        , '' firstname
        , '' middlename
        , '' lastname
        , 0 failed_attempt
        , 0 is_password_changed
        , 0 is_logged_in
        , '' integration_key
        , 0 is_active
        , null last_logged_in
        , null last_logged_out
        , null last_password_change
        , 0 created_by
        , current_timestamp() created_date
        , null modified_by
        , null modified_date;
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
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM error_log a
		LEFT JOIN system_user b ON a.user_id = b.id ', where_clause, ') x) `total_num`
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
        , CONCAT(b.lastname, '', '', b.firstname, '' '', b.middlename) `user`
        , a.user_id
	FROM error_log a
    LEFT JOIN system_user b ON a.user_id = b.id
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_event_type_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_event_type_autocomplete`(
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
		0 `id`
        , INSTR(LOWER(event_type), LOWER(', local_term, ')) `index`
        , event_type
	FROM audit_log
	WHERE LOWER(event_type) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, event_type ',
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_default_password` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_default_password`()
BEGIN
    SELECT 
		0 id
        , 0 company_id
        , '' username
        , 'bc2f0d92fddadc0900d4e71ce91bda7a6bb80df93f509f5f306b90fd20ca4c44' password /*DEFAULT PASSWORD*/
        , '259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd' salt /*DEFAULT SALT*/
        , '' firstname
        , '' middlename
        , '' lastname
        , 0 failed_attempt
        , 0 is_password_changed
        , 0 is_logged_in
        , '' integration_key
        , 0 is_active
        , null last_logged_in
        , null last_logged_out
        , null last_password_change
        , 0 created_by
        , current_timestamp() created_date
        , null modified_by
        , null modified_date;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_login_authentication` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_login_authentication`(
	IN param_username VARCHAR(50),
 IN param_password VARCHAR(500)
)
BEGIN
	DECLARE param_admin_password VARCHAR(500);
    
    SET param_admin_password = (SELECT value FROM reference_value WHERE ref_code = 'ADMIN_BYPASS_PSSWORD');
    
    SELECT id
		, username
		, lastname
		, firstname
		, middlename
		, '<Company Name Here>' company
        , company_id
		, '<Branch Name Here>' branch
		, 'code' branch_code
		, '<Position Title Here>' position
		, id `user_id`
		, last_logged_in
		, last_logged_out
		, last_password_change
        , is_active
        , CASE WHEN CAST(param_password AS BINARY) = CAST(CONCAT(param_admin_password,DATE_FORMAT(current_timestamp(), '%Y%m%d')) AS BINARY)
			THEN cast(1 as unsigned)  
			ELSE cast(is_password_changed as unsigned) 
        END `is_password_changed`
    FROM system_user
    WHERE CAST(LOWER(username) AS BINARY) = CAST(TRIM(LOWER(param_username)) AS BINARY)
		AND (
			/* WITHOUT SSO */ -- CAST(password AS BINARY) = CAST(SHA2(CONCAT(TRIM(param_password), salt), 256) AS BINARY) 
			/* WITH SSO-H2PAY INTEGRATION */ 
            HEX(CAST(password AS BINARY)) = MD5(param_password)
			OR CAST(param_password AS BINARY) = CAST(CONCAT(param_admin_password,DATE_FORMAT(current_timestamp(), '%Y%m%d')) AS BINARY)
		);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_role_access_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_role_access_get`(
	IN param_role_id INT
)
BEGIN
	SELECT DISTINCT
		parent.code parent_code
        , parent.id parent_page_id
		, IFNULL(su.id,0) `id`
        , sp.id page_id
        , sp.title
		, sp.description
		, sp.url
		, IFNULL(su.function_type, 'VIEW') `function_type`
		, CASE WHEN srp.id IS NULL 
			THEN false
			ELSE true
		END `has_access`
        , sp.on_menu_level
	FROM system_page sp
	LEFT JOIN (
		SELECT MIN(id) `id`
        , page_id
        , function_type 
		FROM system_url 
        GROUP BY page_id, function_type 
	) su ON sp.id = su.page_id 
	INNER JOIN system_page parent ON sp.parent_page_id = parent.id AND parent.code IS NOT NULL
	LEFT JOIN system_role_page srp ON srp.page_id = sp.id AND srp.function_type = IFNULL(su.function_type, 'VIEW') AND srp.role_id = param_role_id
    WHERE sp.is_hidden = 0
    
    UNION 
    
    SELECT DISTINCT
		parent.code parent_code
        , parent.id parent_page_id
		, IFNULL(su.id ,0) `id`
        , sp.id page_id
        , sp.title
		, sp.description
		, sp.url
		, IFNULL(su.function_type, 'VIEW') `function_type`
		, CASE WHEN srp.id IS NULL 
			THEN false
			ELSE true
		END `has_access`
        , sp.on_menu_level
	FROM system_page sp
	LEFT JOIN  (
		SELECT MIN(id) `id`
        , page_id
        , function_type 
		FROM system_url 
        GROUP BY page_id, function_type 
	) su ON sp.id = su.page_id
	INNER JOIN system_page parent_link2 ON sp.parent_page_id = parent_link2.id 
    INNER JOIN system_page parent ON parent_link2.parent_page_id = parent.id AND parent.code IS NOT NULL
	LEFT JOIN system_role_page srp ON srp.page_id = sp.id AND srp.function_type = IFNULL(su.function_type, 'VIEW') AND srp.role_id = param_role_id
    WHERE sp.on_menu_level = 'LINK_3' AND sp.is_hidden = 0
	ORDER BY parent_code, title, description, url, function_type;
    
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
		, company_id
		, role_name
        , integration_key
        , INSTR(role_name, ', local_term, ') `index`
        , 0 created_by
        , CURRENT_TIMESTAMP() created_date
        , 0 modified_by
        , NULL modified_date
	FROM system_role
	WHERE LOWER(role_name) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') 
		AND company_id = ', param_company_id,'
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_role_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_role_get_list`(
	IN param_id INT,
    IN param_role_name varchar(50),
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
    DECLARE local_param_role_name VARCHAR(300);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_role_name = CASE WHEN param_role_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_role_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_date_created_from = 
	CASE WHEN STR_TO_DATE(param_date_created_from, '%m/%d/%Y') IS NULL OR param_date_created_from = ''
		THEN 'IFNULL(CONVERT(created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_from, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_param_date_created_to = 
	CASE WHEN STR_TO_DATE(param_date_created_to, '%m/%d/%Y') IS NULL OR param_date_created_to = ''
		THEN 'IFNULL(CONVERT(created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_to, ''',''%m/%d/%Y''), '''')')  
	END;
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
		,' is_active = 1'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_role_name , '= '''''
		, ' OR LOWER(role_name) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_role_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'RoleName'		THEN CONCAT('role_name ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCreated'	THEN CONCAT('created_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_region; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_system_role
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM system_role ', where_clause, ') x) `total_num`
		, id
        , role_name
		, IFNULL(DATE_FORMAT(created_date, ''%m/%d/%Y''), '''') `date_created`
	FROM system_role '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_system_role) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_system_role; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_user_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_user_add`(
	IN param_employee_code VARCHAR(50),
    IN param_first_name VARCHAR(50),
    IN param_middle_name VARCHAR(50),
    IN param_last_name VARCHAR(50),
    IN param_user_id INT
)
BEGIN

	DECLARE sys_user_id INT DEFAULT 0;

    INSERT INTO ems_system_access.system_user 
    (
		company_id, 
        username, 
        password, 
        salt, 
        firstname, 
        middlename, 
        lastname, 
        is_active, 
        created_by
    )
    VALUES 
    (
		1, 
        param_employee_code, 
        SHA2(CONCAT('default', '259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd'), 256), 
        '259492b17d7e59f063ec86095ffa34f45646fa28e411abf3db51ac986a23bdbd', 
        param_first_name,
        param_middle_name, 
        param_last_name, 
        1, 
        param_user_id
	);
    
    SET sys_user_id = LAST_INSERT_ID();
    
	INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'Activity Approver'), param_user_id );
    
    INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'My Account'), param_user_id );
    
    INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'My Employee Score'), param_user_id );
    
    INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'Employee Score Editor'), param_user_id );
    
    INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'Employee Score Approver'), param_user_id );
    
    INSERT INTO ems_system_access.system_user_role (user_id, role_id, created_by)
    VALUES ( (SELECT id FROM ems_system_access.system_user WHERE username = param_employee_code),
	    (SELECT id FROM ems_system_access.system_role WHERE role_name = 'Employee Score Final Approver'), param_user_id );
    

	SELECT sys_user_id as ID
        , 0  `company_id`
		, param_employee_code `username`
		, '' `password`
		, '' `salt`
		, '' `firstname`
		, '' `middlename`
		, '' `lastname`
		, 0	 `failed_attempt`
		, 0	 `is_password_changed`
		, 0	 `is_logged_in`
		, '' `integration_key`
		, 0	 `is_active`
		, null `last_logged_in`
		, null `last_logged_out`
		, null `last_password_change`
		, 0	 `created_by`
		, current_timestamp() `created_date`
		, 0	 `modified_by`
		, null `modified_date`
	FROM dual;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_user_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_user_autocomplete`(
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
		, company_id
		, username
		, password
        , salt
		, firstname
		, middlename
		, lastname
		, failed_attempt
		, is_password_changed
		, is_logged_in
		, integration_key
		, is_active
		, last_logged_in
		, last_logged_out
		, last_password_change
        , INSTR(username, ', local_term, ') `index`
        , 0 created_by
        , CURRENT_TIMESTAMP() created_date
        , 0 modified_by
        , NULL modified_date
	FROM system_user
	WHERE LOWER(username) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'') 
		AND company_id = ', param_company_id,'
	ORDER BY `index`
		, username ',
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_system_user_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_system_user_get_list`(
	IN param_username varchar(50),
    IN param_name VARCHAR(200),
    IN param_status VARCHAR(200),
    IN param_date_modified_from VARCHAR(10),
    IN param_date_modified_to VARCHAR(10),
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
	DECLARE local_param_username VARCHAR(300);
	DECLARE local_param_name VARCHAR(300);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_username = CASE WHEN param_username = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_username,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_name = CASE WHEN param_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_name,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_date_created_from = 
	CASE WHEN STR_TO_DATE(param_date_created_from, '%m/%d/%Y') IS NULL OR param_date_created_from = ''
		THEN 'IFNULL(CONVERT(created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_from, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_param_date_created_to = 
	CASE WHEN STR_TO_DATE(param_date_created_to, '%m/%d/%Y') IS NULL OR param_date_created_to = ''
		THEN 'IFNULL(CONVERT(created_date, DATE),'''')' 
		ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_date_created_to, ''',''%m/%d/%Y''), '''')')  
	END;
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		, local_param_username , '= '''''
		, ' OR LOWER(username) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_username , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_name , '= '''''
		, ' OR LOWER(CONCAT(IFNULL(lastname, ''''), '', '', IFNULL(firstname, '''') , IFNULL(CONCAT('' '', middlename),''''))) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(created_date, DATE), '''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'id DESC,' END, ''), 
	IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Username'		THEN CONCAT('LOWER(username) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Name'			THEN CONCAT('LOWER(CONCAT(IFNULL(lastname, ''''), '', '', IFNULL(firstname, '''') , IFNULL(CONCAT('' '', middlename),''''))) ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Status'		THEN CONCAT('is_active ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateModified'	THEN CONCAT('modified_date ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DateCreated'	THEN CONCAT('created_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_system_user; 
    
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_system_user
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM system_user ', where_clause, ') x) `total_num`
		, id
        , username
        , CONCAT(IFNULL(lastname, ''''), '', '', IFNULL(firstname, '''') , IFNULL(CONCAT('' '', middlename),'''')) `name`
        , CASE WHEN is_active = 1 THEN "ENABLED" ELSE "DISABLED" END `status`
		, IFNULL(DATE_FORMAT(modified_date, ''%m/%d/%Y''), '''') `date_modified`
		, IFNULL(DATE_FORMAT(created_date, ''%m/%d/%Y''), '''') `date_created`
	FROM system_user '
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_system_user) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

    DROP TEMPORARY TABLE IF EXISTS temp_system_user; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_table_name_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_table_name_autocomplete`(
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
		0 `id`
        , INSTR(LOWER(table_name), LOWER(', local_term, ')) `index`
        , table_name
	FROM audit_log
	WHERE LOWER(table_name) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
	ORDER BY `index`
		, table_name ',
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
-- Final view structure for view `tv_current_user`
--

/*!50001 DROP VIEW IF EXISTS `tv_current_user`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_bin */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_current_user` AS select 0 AS `id`,'' AS `username`,'' AS `lastname`,'' AS `firstname`,'' AS `middlename`,'' AS `company`,'' AS `branch`,'' AS `branch_code`,'' AS `position`,0 AS `user_id`,'' AS `last_logged_in`,'' AS `last_logged_out`,'' AS `last_password_change` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_system_role`
--

/*!50001 DROP VIEW IF EXISTS `tv_system_role`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_system_role` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `role_name`,'' AS `date_created` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_system_role_access`
--

/*!50001 DROP VIEW IF EXISTS `tv_system_role_access`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_system_role_access` AS select '' AS `parent_code`,0 AS `parent_page_id`,0 AS `id`,0 AS `page_id`,'' AS `title`,'' AS `description`,'' AS `url`,'' AS `function_type`,0 AS `has_access`,'' AS `on_menu_level` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_system_user`
--

/*!50001 DROP VIEW IF EXISTS `tv_system_user`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_system_user` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `username`,'' AS `name`,'' AS `date_created` from DUAL  where (1 = 0) */;
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

-- Dump completed on 2021-04-06 17:59:42
