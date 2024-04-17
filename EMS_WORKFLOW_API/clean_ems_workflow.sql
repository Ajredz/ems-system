CREATE DATABASE  IF NOT EXISTS `clean_ems_workflow` /*!40100 DEFAULT CHARACTER SET latin1 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clean_ems_workflow`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: clean_ems_workflow
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
-- Table structure for table `accountability`
--

DROP TABLE IF EXISTS `accountability`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `accountability` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `preload_name` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accountability`
--

LOCK TABLES `accountability` WRITE;
/*!40000 ALTER TABLE `accountability` DISABLE KEYS */;
/*!40000 ALTER TABLE `accountability` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accountability_details`
--

DROP TABLE IF EXISTS `accountability_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `accountability_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accountability_id` int(11) NOT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_accountability` (`accountability_id`,`type`,`title`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accountability_details`
--

LOCK TABLES `accountability_details` WRITE;
/*!40000 ALTER TABLE `accountability_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `accountability_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicant_log_activity`
--

DROP TABLE IF EXISTS `applicant_log_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant_log_activity` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `applicant_id` int(11) NOT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `sub_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_with_pass_fail` bit(1) NOT NULL DEFAULT b'0',
  `is_with_assignment` bit(1) NOT NULL DEFAULT b'0',
  `is_pass` bit(1) NOT NULL DEFAULT b'0',
  `current_status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `current_timestamp` datetime DEFAULT NULL,
  `assigned_user_id` int(11) DEFAULT NULL,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_email_sent` bit(1) DEFAULT b'0',
  `sent_datetime` datetime DEFAULT NULL,
  `applicant_name` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `assigned_org_group_id` int(11) DEFAULT NULL,
  `due_date` date DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicant_log_activity`
--

LOCK TABLES `applicant_log_activity` WRITE;
/*!40000 ALTER TABLE `applicant_log_activity` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant_log_activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicant_log_activity_attachment`
--

DROP TABLE IF EXISTS `applicant_log_activity_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant_log_activity_attachment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `applicant_log_activity_id` int(11) NOT NULL,
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
-- Dumping data for table `applicant_log_activity_attachment`
--

LOCK TABLES `applicant_log_activity_attachment` WRITE;
/*!40000 ALTER TABLE `applicant_log_activity_attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant_log_activity_attachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicant_log_activity_comments`
--

DROP TABLE IF EXISTS `applicant_log_activity_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant_log_activity_comments` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `applicant_log_activity_id` int(11) NOT NULL,
  `comments` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicant_log_activity_comments`
--

LOCK TABLES `applicant_log_activity_comments` WRITE;
/*!40000 ALTER TABLE `applicant_log_activity_comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant_log_activity_comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicant_log_activity_status_history`
--

DROP TABLE IF EXISTS `applicant_log_activity_status_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applicant_log_activity_status_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `applicant_log_activity_id` int(11) NOT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `timestamp` datetime DEFAULT NULL,
  `is_pass` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicant_log_activity_status_history`
--

LOCK TABLES `applicant_log_activity_status_history` WRITE;
/*!40000 ALTER TABLE `applicant_log_activity_status_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `applicant_log_activity_status_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `email_server_credential`
--

DROP TABLE IF EXISTS `email_server_credential`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `email_server_credential` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `template_code` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `subject` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `body` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `sender_display_name` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `sender_email` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `sender_username` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `sender_password` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `sender_salt` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `host` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `port` int(11) DEFAULT NULL,
  `enable_ssl` bit(1) DEFAULT b'0',
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `email_server_credential`
--

LOCK TABLES `email_server_credential` WRITE;
/*!40000 ALTER TABLE `email_server_credential` DISABLE KEYS */;
INSERT INTO `email_server_credential` VALUES (1,'LOG_ACTIVITY_NOTIFICATION','[LOG ACTIVITY] <<{title}>>','<b><<{applicant_employee}>> Name:</b> <<{name}>> <br> <b>Type:</b> <<{type}>> <br> <b>Description:</b> <<{description}>>','EMS Log Activity','noreply@server.com','noreply','password','','0.0.0.0',25,_binary '\0',_binary '',1,'2020-06-09 16:27:04');
/*!40000 ALTER TABLE `email_server_credential` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_accountability`
--

DROP TABLE IF EXISTS `employee_accountability`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_accountability` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `employee_id` int(11) NOT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `org_group_id` int(11) DEFAULT NULL,
  `current_status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `current_timestamp` datetime DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_accountability`
--

LOCK TABLES `employee_accountability` WRITE;
/*!40000 ALTER TABLE `employee_accountability` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_accountability` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_accountability_attachment`
--

DROP TABLE IF EXISTS `employee_accountability_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_accountability_attachment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_accountability_id` int(11) NOT NULL,
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
-- Dumping data for table `employee_accountability_attachment`
--

LOCK TABLES `employee_accountability_attachment` WRITE;
/*!40000 ALTER TABLE `employee_accountability_attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_accountability_attachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_accountability_comments`
--

DROP TABLE IF EXISTS `employee_accountability_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_accountability_comments` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `employee_accountability_id` int(11) NOT NULL,
  `comments` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_accountability_comments`
--

LOCK TABLES `employee_accountability_comments` WRITE;
/*!40000 ALTER TABLE `employee_accountability_comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_accountability_comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_accountability_status_history`
--

DROP TABLE IF EXISTS `employee_accountability_status_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_accountability_status_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `employee_accountability_id` int(11) NOT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `timestamp` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_accountability_status_history`
--

LOCK TABLES `employee_accountability_status_history` WRITE;
/*!40000 ALTER TABLE `employee_accountability_status_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_accountability_status_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_log_activity`
--

DROP TABLE IF EXISTS `employee_log_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_log_activity` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `employee_id` int(11) NOT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `sub_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_with_pass_fail` bit(1) NOT NULL DEFAULT b'0',
  `is_with_assignment` bit(1) NOT NULL DEFAULT b'0',
  `is_pass` bit(1) NOT NULL DEFAULT b'0',
  `current_status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `current_timestamp` datetime DEFAULT NULL,
  `assigned_user_id` int(11) DEFAULT NULL,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_email_sent` bit(1) DEFAULT b'0',
  `sent_datetime` datetime DEFAULT NULL,
  `employee_name` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `assigned_org_group_id` int(11) DEFAULT NULL,
  `is_visible` bit(1) NOT NULL DEFAULT b'0',
  `due_date` date DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_log_activity`
--

LOCK TABLES `employee_log_activity` WRITE;
/*!40000 ALTER TABLE `employee_log_activity` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_log_activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_log_activity_attachment`
--

DROP TABLE IF EXISTS `employee_log_activity_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_log_activity_attachment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `employee_log_activity_id` int(11) NOT NULL,
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
-- Dumping data for table `employee_log_activity_attachment`
--

LOCK TABLES `employee_log_activity_attachment` WRITE;
/*!40000 ALTER TABLE `employee_log_activity_attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_log_activity_attachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_log_activity_comments`
--

DROP TABLE IF EXISTS `employee_log_activity_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_log_activity_comments` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `employee_log_activity_id` int(11) NOT NULL,
  `comments` varchar(500) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_log_activity_comments`
--

LOCK TABLES `employee_log_activity_comments` WRITE;
/*!40000 ALTER TABLE `employee_log_activity_comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_log_activity_comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_log_activity_status_history`
--

DROP TABLE IF EXISTS `employee_log_activity_status_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_log_activity_status_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `employee_log_activity_id` int(11) NOT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `timestamp` datetime DEFAULT NULL,
  `is_pass` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_log_activity_status_history`
--

LOCK TABLES `employee_log_activity_status_history` WRITE;
/*!40000 ALTER TABLE `employee_log_activity_status_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_log_activity_status_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_score_approval_history`
--

DROP TABLE IF EXISTS `employee_score_approval_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `employee_score_approval_history` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `t_id` bigint(20) NOT NULL,
  `status` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `approver_id` int(11) DEFAULT NULL,
  `remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `timestamp` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_score_approval_history`
--

LOCK TABLES `employee_score_approval_history` WRITE;
/*!40000 ALTER TABLE `employee_score_approval_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_score_approval_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `log_activity`
--

DROP TABLE IF EXISTS `log_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `log_activity` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `module` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `sub_type` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `title` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `description` varchar(1000) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `is_with_pass_fail` bit(1) NOT NULL DEFAULT b'0',
  `is_with_assignment` bit(1) NOT NULL DEFAULT b'0',
  `log_activity_preloaded_id` int(11) NOT NULL,
  `is_visible` bit(1) NOT NULL DEFAULT b'0',
  `assigned_user_id` int(11) DEFAULT '0',
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log_activity`
--

LOCK TABLES `log_activity` WRITE;
/*!40000 ALTER TABLE `log_activity` DISABLE KEYS */;
/*!40000 ALTER TABLE `log_activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `log_activity_preloaded`
--

DROP TABLE IF EXISTS `log_activity_preloaded`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `log_activity_preloaded` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `preload_name` varchar(100) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int(11) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log_activity_preloaded`
--

LOCK TABLES `log_activity_preloaded` WRITE;
/*!40000 ALTER TABLE `log_activity_preloaded` DISABLE KEYS */;
/*!40000 ALTER TABLE `log_activity_preloaded` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `log_activity_preloaded_items`
--

DROP TABLE IF EXISTS `log_activity_preloaded_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `log_activity_preloaded_items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `preloaded_id` int(11) NOT NULL,
  `log_activity_id` int(11) NOT NULL,
  `created_by` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log_activity_preloaded_items`
--

LOCK TABLES `log_activity_preloaded_items` WRITE;
/*!40000 ALTER TABLE `log_activity_preloaded_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `log_activity_preloaded_items` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference`
--

LOCK TABLES `reference` WRITE;
/*!40000 ALTER TABLE `reference` DISABLE KEYS */;
INSERT INTO `reference` VALUES (1,'RESULT_TYPE','Sets of responses for every step',_binary '\0',1,'2020-06-09 16:27:37'),(2,'APPROVE_REJECT','Result Type - Approve or Reject options',_binary '\0',1,'2020-06-09 16:27:38'),(3,'APPROVE_REJECT_SKIP','Result Type - Approve, Reject or Skip options',_binary '\0',1,'2020-06-09 16:27:38'),(4,'PASS_FAIL','Result Type - Pass or Fail options',_binary '\0',1,'2020-06-09 16:27:38'),(5,'PASS_FAIL_SKIP','Result Type - Pass, Fail or Skip options',_binary '\0',1,'2020-06-09 16:27:38'),(6,'YES_NO','Result Type - Yes or No options',_binary '\0',1,'2020-06-09 16:27:38'),(7,'DONE_PENDING','Result Type - Done or Pending options',_binary '\0',1,'2020-06-09 16:27:38'),(8,'NEGATIVE_RESULT_TYPE','Negative Result Options',_binary '\0',1,'2020-06-09 16:27:39'),(9,'ACTIVITY_TYPE','Type of Activities',_binary '',1,'2020-06-09 16:27:39'),(10,'ACTIVITY_STATUS','Set of status for activities',_binary '\0',1,'2020-06-09 16:27:39'),(11,'APPR_ACTIVITY_STATUS','Set of activity status for the approver',_binary '\0',1,'2020-06-09 16:27:39'),(12,'ACTIVITY_STAT_FILTER','Set of activity status for filter',_binary '\0',1,'2020-06-09 16:27:39'),(13,'ATTACHMENT_TYPE','Types of Attachments',_binary '',1,'2020-06-09 16:27:39'),(14,'ACCOUNTABILITY_TYPE','Type of Accountability',_binary '\0',1,'2020-06-09 16:28:21'),(15,'ACCNTABILITY_STATUS','Set of status for accountability',_binary '\0',1,'2020-06-09 16:28:22'),(16,'ACTIVITY_MODULE','Set of activities for specific modules',_binary '\0',1,'2020-06-10 16:16:21');
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
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reference_value`
--

LOCK TABLES `reference_value` WRITE;
/*!40000 ALTER TABLE `reference_value` DISABLE KEYS */;
INSERT INTO `reference_value` VALUES (1,'RESULT_TYPE','PASS_FAIL','PASS / FAIL',1,'2020-06-09 16:27:38'),(2,'RESULT_TYPE','APPROVE_REJECT','APPROVE / REJECT',1,'2020-06-09 16:27:38'),(3,'RESULT_TYPE','PASS_FAIL_SKIP','PASS / FAIL / SKIP',1,'2020-06-09 16:27:38'),(4,'RESULT_TYPE','APPROVE_REJECT_SKIP','APPROVE / REJECT / SKIP',1,'2020-06-09 16:27:38'),(5,'RESULT_TYPE','ACCEPT_DECLINE','ACCEPT / DECLINE',1,'2020-06-09 16:27:38'),(6,'RESULT_TYPE','YES_NO','YES / NO',1,'2020-06-09 16:27:38'),(7,'RESULT_TYPE','DONE_PENDING','DONE / PENDING',1,'2020-06-09 16:27:38'),(8,'PASS_FAIL','PASS','PASS',1,'2020-06-09 16:27:38'),(9,'PASS_FAIL','FAIL','FAIL',1,'2020-06-09 16:27:38'),(10,'APPROVE_REJECT','APPROVE','APPROVE',1,'2020-06-09 16:27:38'),(11,'APPROVE_REJECT','REJECT','REJECT',1,'2020-06-09 16:27:38'),(12,'PASS_FAIL_SKIP','PASS','PASS',1,'2020-06-09 16:27:38'),(13,'PASS_FAIL_SKIP','FAIL','FAIL',1,'2020-06-09 16:27:38'),(14,'PASS_FAIL_SKIP','SKIP','SKIP',1,'2020-06-09 16:27:38'),(15,'APPROVE_REJECT_SKIP','APPROVE','APPROVE',1,'2020-06-09 16:27:39'),(16,'APPROVE_REJECT_SKIP','REJECT','REJECT',1,'2020-06-09 16:27:39'),(17,'APPROVE_REJECT_SKIP','SKIP','SKIP',1,'2020-06-09 16:27:39'),(18,'ACCEPT_DECLINE','ACCEPT','ACCEPT',1,'2020-06-09 16:27:39'),(19,'ACCEPT_DECLINE','DECLINE','DECLINE',1,'2020-06-09 16:27:39'),(20,'YES_NO','YES','YES',1,'2020-06-09 16:27:39'),(21,'YES_NO','NO','NO',1,'2020-06-09 16:27:39'),(22,'NEGATIVE_RESULT_TYPE','FAIL','FAIL',1,'2020-06-09 16:27:39'),(23,'NEGATIVE_RESULT_TYPE','DECLINE','DECLINE',1,'2020-06-09 16:27:39'),(24,'NEGATIVE_RESULT_TYPE','REJECT','REJECT',1,'2020-06-09 16:27:39'),(25,'NEGATIVE_RESULT_TYPE','NO','NO',1,'2020-06-09 16:27:39'),(26,'ACTIVITY_TYPE','RECRUITMENT','RECRUITMENT',1,'2020-06-09 16:27:39'),(27,'ACTIVITY_TYPE','PRE_EMPLOYMENT','PRE-EMPLOYMENT',1,'2020-06-09 16:27:39'),(28,'ACTIVITY_TYPE','ONBOARDING','ONBOARDING',1,'2020-06-09 16:27:39'),(29,'ACTIVITY_TYPE','TRAINING','TRAINING',1,'2020-06-09 16:27:39'),(30,'ACTIVITY_TYPE','1_1_BI_CHECK','BI CHECK',1,'2020-06-09 16:27:39'),(31,'ACTIVITY_TYPE','OTHERS','OTHERS',1,'2020-06-09 16:27:39'),(32,'RECRUITMENT','1_1_EXAM','EXAM',1,'2020-06-09 16:27:40'),(33,'RECRUITMENT','1_1_INTERVIEW','INTERVIEW',1,'2020-06-09 16:27:40'),(34,'PRE_EMPLOYMENT','0_0_MEDICAL','MEDICAL',1,'2020-06-09 16:27:40'),(35,'RECRUITMENT','0_1_BI_PAST_EMPLOYER','BI PAST EMPLOYER',1,'2020-06-09 16:27:40'),(36,'RECRUITMENT','1_0_NEGOTIATION','NEGOTIATION',1,'2020-06-09 16:27:40'),(37,'TRAINING','0_0_TRAINING','BI PAST TRAINING',1,'2020-06-09 16:27:40'),(38,'ACTIVITY_STATUS','1_NEW','NEW',1,'2020-06-09 16:27:40'),(39,'ACTIVITY_STATUS','2_WIP','WIP',1,'2020-06-09 16:27:40'),(40,'ACTIVITY_STATUS','2_CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(41,'ACTIVITY_STATUS','3_CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(42,'ACTIVITY_STATUS','3_DONE','DONE',1,'2020-06-09 16:27:40'),(43,'ACTIVITY_STATUS','4_CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(44,'APPR_ACTIVITY_STATUS','2_CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(45,'APPR_ACTIVITY_STATUS','3_CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(46,'APPR_ACTIVITY_STATUS','2_ASSIGN_ACCEPT','ASSIGNMENT ACCEPTED',1,'2020-06-09 16:27:40'),(47,'APPR_ACTIVITY_STATUS','2_ASSIGN_NOT_ACCEPT','ASSIGNMENT NOT ACCEPTED',1,'2020-06-09 16:27:40'),(48,'APPR_ACTIVITY_STATUS','3_DONE','DONE',1,'2020-06-09 16:27:40'),(49,'ACTIVITY_STAT_FILTER','NEW','NEW',1,'2020-06-09 16:27:40'),(50,'ACTIVITY_STAT_FILTER','CANCELLED','CANCELLED',1,'2020-06-09 16:27:40'),(51,'ACTIVITY_STAT_FILTER','WIP','WIP',1,'2020-06-09 16:27:40'),(52,'ACTIVITY_STAT_FILTER','DONE','DONE',1,'2020-06-09 16:27:41'),(53,'ACTIVITY_STAT_FILTER','ASSIGN_NOT_ACCEPT','ASSIGNMENT NOT ACCEPTED',1,'2020-06-09 16:27:41'),(54,'ACTIVITY_STAT_FILTER','ASSIGN_ACCEPT','ASSIGNMENT ACCEPTED',1,'2020-06-09 16:27:41'),(55,'ATTACHMENT_TYPE','RESUME','RESUME',1,'2020-06-09 16:27:41'),(56,'ATTACHMENT_TYPE','TRAINING_CERT','TRAINING CERTIFICATE',1,'2020-06-09 16:27:41'),(57,'ATTACHMENT_TYPE','OTHERS','OTHERS',1,'2020-06-09 16:27:41'),(58,'ACCOUNTABILITY_TYPE','FIXTURE','FIXTURE',1,'2020-06-09 16:28:21'),(59,'ACCOUNTABILITY_TYPE','COMPUTER','COMPUTER',1,'2020-06-09 16:28:21'),(60,'ACCOUNTABILITY_TYPE','VEHICLE','VEHICLE',1,'2020-06-09 16:28:22'),(61,'ACCNTABILITY_STATUS','NEW','NEW',1,'2020-06-09 16:28:22'),(62,'ACCNTABILITY_STATUS','ACCEPTED','ACCEPTED',1,'2020-06-09 16:28:22'),(63,'ACCNTABILITY_STATUS','FOR_CLEARANCE','FOR CLEARANCE',1,'2020-06-09 16:28:22'),(64,'ACCNTABILITY_STATUS','CLEARED','CLEARED',1,'2020-06-09 16:28:22'),(65,'ACCNTABILITY_STATUS','CANCELLED','CANCELLED',1,'2020-06-09 16:28:22'),(66,'ACTIVITY_MODULE','RECRUITMENT','RECRUITMENT',1,'2020-06-10 16:16:22'),(67,'ACTIVITY_MODULE','ONBOARDING','ONBOARDING',1,'2020-06-10 16:16:22'),(68,'ACTIVITY_MODULE','EXIT_MANAGEMENT','EXIT_MANAGEMENT',1,'2020-06-10 16:16:22'),(69,'ACTIVITY_MODULE','MOVEMENT','MOVEMENT',1,'2020-06-10 16:16:22'),(70,'ACTIVITY_MODULE','GENERAL','GENERAL',1,'2020-06-10 16:16:22'),(71,'ACCOUNTABILITY_TYPE','UNIFORM','UNIFORM',1,'2020-06-16 16:34:07'),(72,'ACCOUNTABILITY_TYPE','NEW_HIRE_KIT','NEW HIRE KIT',1,'2020-06-16 16:34:07'),(73,'ACCOUNTABILITY_TYPE','BENEFIT','BENEFIT',1,'2020-06-16 16:34:07');
/*!40000 ALTER TABLE `reference_value` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `tv_accountability`
--

DROP TABLE IF EXISTS `tv_accountability`;
/*!50001 DROP VIEW IF EXISTS `tv_accountability`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_accountability` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `preload_name`,
 1 AS `date_created`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_log_activity`
--

DROP TABLE IF EXISTS `tv_applicant_log_activity`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_log_activity` AS SELECT 
 1 AS `id`,
 1 AS `type`,
 1 AS `sub_type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `assigned_user_id`,
 1 AS `assigned_org_group_id`,
 1 AS `is_pass`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_log_activity_list`
--

DROP TABLE IF EXISTS `tv_applicant_log_activity_list`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity_list`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_log_activity_list` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `type`,
 1 AS `sub_type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `created_by`,
 1 AS `applicant_id`,
 1 AS `assigned_user_id`,
 1 AS `remarks`,
 1 AS `is_assignment`,
 1 AS `due_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_applicant_log_activity_status_history`
--

DROP TABLE IF EXISTS `tv_applicant_log_activity_status_history`;
/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity_status_history`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_applicant_log_activity_status_history` AS SELECT 
 1 AS `id`,
 1 AS `remarks`,
 1 AS `status`,
 1 AS `timestamp`,
 1 AS `user_id`,
 1 AS `is_pass`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_assigned_activities`
--

DROP TABLE IF EXISTS `tv_assigned_activities`;
/*!50001 DROP VIEW IF EXISTS `tv_assigned_activities`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_assigned_activities` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `type`,
 1 AS `sub_type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `created_by`,
 1 AS `applicant_id`,
 1 AS `employee_id`,
 1 AS `assigned_org_group_id`,
 1 AS `current_status_code`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_checklist`
--

DROP TABLE IF EXISTS `tv_checklist`;
/*!50001 DROP VIEW IF EXISTS `tv_checklist`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_checklist` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `type`,
 1 AS `sub_type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `created_by`,
 1 AS `employee_id`,
 1 AS `assigned_user_id`,
 1 AS `remarks`,
 1 AS `is_assignment`,
 1 AS `due_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_current_workflow_step`
--

DROP TABLE IF EXISTS `tv_current_workflow_step`;
/*!50001 DROP VIEW IF EXISTS `tv_current_workflow_step`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_current_workflow_step` AS SELECT 
 1 AS `step_code`,
 1 AS `step_description`,
 1 AS `approver_role_ids`,
 1 AS `workflow_status`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_employee_accountability`
--

DROP TABLE IF EXISTS `tv_employee_accountability`;
/*!50001 DROP VIEW IF EXISTS `tv_employee_accountability`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_employee_accountability` AS SELECT 
 1 AS `id`,
 1 AS `type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `created_by`,
 1 AS `org_group_id`,
 1 AS `modified_by`,
 1 AS `remarks`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_employee_accountability_status_history`
--

DROP TABLE IF EXISTS `tv_employee_accountability_status_history`;
/*!50001 DROP VIEW IF EXISTS `tv_employee_accountability_status_history`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_employee_accountability_status_history` AS SELECT 
 1 AS `id`,
 1 AS `remarks`,
 1 AS `status`,
 1 AS `timestamp`,
 1 AS `user_id`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_employee_log_activity`
--

DROP TABLE IF EXISTS `tv_employee_log_activity`;
/*!50001 DROP VIEW IF EXISTS `tv_employee_log_activity`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_employee_log_activity` AS SELECT 
 1 AS `id`,
 1 AS `type`,
 1 AS `sub_type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `assigned_user_id`,
 1 AS `assigned_org_group_id`,
 1 AS `is_pass`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_employee_log_activity_status_history`
--

DROP TABLE IF EXISTS `tv_employee_log_activity_status_history`;
/*!50001 DROP VIEW IF EXISTS `tv_employee_log_activity_status_history`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_employee_log_activity_status_history` AS SELECT 
 1 AS `id`,
 1 AS `remarks`,
 1 AS `status`,
 1 AS `timestamp`,
 1 AS `user_id`,
 1 AS `is_pass`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_log_activity`
--

DROP TABLE IF EXISTS `tv_log_activity`;
/*!50001 DROP VIEW IF EXISTS `tv_log_activity`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_log_activity` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `module`,
 1 AS `type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `is_pass_fail`,
 1 AS `is_assignment`,
 1 AS `is_preloaded`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_log_activity_preloaded`
--

DROP TABLE IF EXISTS `tv_log_activity_preloaded`;
/*!50001 DROP VIEW IF EXISTS `tv_log_activity_preloaded`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_log_activity_preloaded` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `preload_name`,
 1 AS `date_created`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_my_accountabilities`
--

DROP TABLE IF EXISTS `tv_my_accountabilities`;
/*!50001 DROP VIEW IF EXISTS `tv_my_accountabilities`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_my_accountabilities` AS SELECT 
 1 AS `total_num`,
 1 AS `row_num`,
 1 AS `id`,
 1 AS `type`,
 1 AS `title`,
 1 AS `description`,
 1 AS `current_status`,
 1 AS `current_timestamp`,
 1 AS `created_by`,
 1 AS `employee_id`,
 1 AS `org_group_id`,
 1 AS `modified_by`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_transaction`
--

DROP TABLE IF EXISTS `tv_transaction`;
/*!50001 DROP VIEW IF EXISTS `tv_transaction`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_transaction` AS SELECT 
 1 AS `order`,
 1 AS `workflow_code`,
 1 AS `step_code`,
 1 AS `step`,
 1 AS `status`,
 1 AS `date_scheduled`,
 1 AS `date_completed`,
 1 AS `timestamp`,
 1 AS `remarks`,
 1 AS `result_type`,
 1 AS `start_date_time`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `tv_workflow_get_next_workflow_step`
--

DROP TABLE IF EXISTS `tv_workflow_get_next_workflow_step`;
/*!50001 DROP VIEW IF EXISTS `tv_workflow_get_next_workflow_step`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `tv_workflow_get_next_workflow_step` AS SELECT 
 1 AS `id`,
 1 AS `code`,
 1 AS `description`,
 1 AS `order`,
 1 AS `workflow_code`*/;
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow`
--

LOCK TABLES `workflow` WRITE;
/*!40000 ALTER TABLE `workflow` DISABLE KEYS */;
INSERT INTO `workflow` VALUES (1,'RECRUITMENT','RECRUITMENT','Recruitment Template',1,'2020-06-09 15:55:07',1,'2020-07-09 09:52:28'),(2,'ONBOARDING','ONBOARDING','ONBOARDING',1,'2020-06-09 16:27:14',NULL,NULL),(3,'TASK','TASK','TASK',1,'2020-09-29 09:50:58',NULL,NULL),(4,'ACCOUNTABILITY','ACCOUNTABILITY','ACCOUNTABILITY',1,'2020-09-29 09:50:59',1,'2021-02-04 11:08:32'),(5,'IPM','IPM','IPM',1,'2020-11-04 15:40:49',1,'2021-02-01 12:29:06');
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
  `send_email_to_requester` bit(1) NOT NULL,
  `send_email_to_approver` bit(1) NOT NULL,
  `order` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow_step` (`workflow_id`,`code`,`previous_step_code`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow_step`
--

LOCK TABLES `workflow_step` WRITE;
/*!40000 ALTER TABLE `workflow_step` DISABLE KEYS */;
INSERT INTO `workflow_step` VALUES (2,1,'5-PRE-EMPLOYMENT REQ','Requirments Gathering',NULL,_binary '',1,_binary '','PASS_FAIL',_binary '\0',_binary '\0',7),(3,1,'4A-JOB OFFER','Job Offer',NULL,_binary '',1,_binary '\0','ACCEPT_DECLINE',_binary '\0',_binary '\0',5),(7,1,'1-INITIAL INTERVIEW','Initial Interview',NULL,_binary '',1,_binary '','PASS_FAIL',_binary '\0',_binary '\0',2),(8,1,'6-HIRED','HIRED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',8),(9,2,'1-NEW','1-NEW',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',1),(10,2,'2-WIP','2-WIP',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(11,2,'3-DONE','3-DONE',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',3),(12,1,'7-DEPLOYED','DEPLOYED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',9),(13,1,'2-EXAM','Online Exam',NULL,_binary '',1,_binary '\0','PASS_FAIL_SKIP',_binary '\0',_binary '\0',3),(14,1,'3-FINAL INTERVIEW','Final Interview',NULL,_binary '',1,_binary '\0','PASS_FAIL',_binary '\0',_binary '\0',4),(15,1,'FAILED','FAILED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',1),(16,1,'4B-JOB OFFER HR HEAD','JOB OFFER HR HEAD',NULL,_binary '',1,_binary '\0','ACCEPT_DECLINE',_binary '\0',_binary '\0',6),(17,1,'0-NEW','NEW',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(18,3,'CANCELLED','CANCELLED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(19,3,'NEW','NEW',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',1),(20,3,'WIP','WIP',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(21,3,'ASSIGN_ACCEPT','ASSIGNMENT ACCEPTED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(22,3,'ASSIGN_NOT_ACCEPT','ASSIGNMENT NOT ACCEPTED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(23,3,'DONE','DONE',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',3),(24,4,'CANCELLED','CANCELLED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(25,4,'NEW','NEW',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',1),(26,4,'ACCEPTED','ACCEPTED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(27,4,'FOR_CLEARANCE','FOR_CLEARANCE',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',3),(28,4,'CLEARED','CLEARED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',4),(29,5,'NEW','NEW',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(30,5,'FOR_APPROVAL','FOR_APPROVAL',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',1),(31,5,'FOR_REVISION','FOR_REVISION',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(32,5,'APPROVED','APPROVED',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',2),(34,5,'WIP','WIP',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0),(35,5,'VOID','VOID',NULL,_binary '',1,_binary '\0','YES_NO',_binary '\0',_binary '\0',0);
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
  `use_other_approval_setup` bit(1) NOT NULL,
  `approval_setup` varchar(20) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workflow_step_approver` (`step_code`,`role_id`,`workflow_id`)
) ENGINE=InnoDB AUTO_INCREMENT=100 DEFAULT CHARSET=latin1 COLLATE=latin1_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workflow_step_approver`
--

LOCK TABLES `workflow_step_approver` WRITE;
/*!40000 ALTER TABLE `workflow_step_approver` DISABLE KEYS */;
INSERT INTO `workflow_step_approver` VALUES (3,1,'6-HIRED',1,_binary '\0',NULL),(4,1,'6-HIRED',19,_binary '\0',NULL),(11,1,'1-INITIAL INTERVIEW',1,_binary '\0',NULL),(12,1,'1-INITIAL INTERVIEW',19,_binary '\0',NULL),(13,1,'4A-JOB OFFER',1,_binary '\0',NULL),(14,1,'4A-JOB OFFER',19,_binary '\0',NULL),(15,1,'5-PRE-EMPLOYMENT REQ',1,_binary '\0',NULL),(16,1,'5-PRE-EMPLOYMENT REQ',19,_binary '\0',NULL),(17,2,'1-NEW',1,_binary '\0',NULL),(18,2,'1-NEW',19,_binary '\0',NULL),(19,2,'2-WIP',1,_binary '\0',NULL),(20,2,'2-WIP',19,_binary '\0',NULL),(21,2,'3-DONE',1,_binary '\0',NULL),(22,2,'3-DONE',19,_binary '\0',NULL),(27,1,'7-DEPLOYED',1,_binary '\0',NULL),(28,1,'7-DEPLOYED',19,_binary '\0',NULL),(29,1,'2-EXAM',1,_binary '\0',NULL),(30,1,'2-EXAM',19,_binary '\0',NULL),(31,1,'3-FINAL INTERVIEW',1,_binary '\0',NULL),(32,1,'3-FINAL INTERVIEW',19,_binary '\0',NULL),(33,1,'FAILED',1,_binary '\0',NULL),(34,1,'FAILED',19,_binary '\0',NULL),(35,1,'4B-JOB OFFER HR HEAD',1,_binary '\0',NULL),(36,1,'4B-JOB OFFER HR HEAD',19,_binary '\0',NULL),(37,1,'0-NEW',1,_binary '\0',NULL),(38,3,'CANCELLED',137,_binary '\0',NULL),(39,3,'CANCELLED',19,_binary '\0',NULL),(40,3,'CANCELLED',1,_binary '\0',NULL),(41,3,'CANCELLED',130,_binary '\0',NULL),(42,3,'NEW',137,_binary '\0',NULL),(43,3,'NEW',19,_binary '\0',NULL),(44,3,'NEW',1,_binary '\0',NULL),(45,3,'WIP',137,_binary '\0',NULL),(46,3,'WIP',19,_binary '\0',NULL),(47,3,'WIP',1,_binary '\0',NULL),(48,3,'ASSIGN_ACCEPT',137,_binary '\0',NULL),(49,3,'ASSIGN_ACCEPT',19,_binary '\0',NULL),(50,3,'ASSIGN_ACCEPT',1,_binary '\0',NULL),(51,3,'ASSIGN_ACCEPT',130,_binary '\0',NULL),(52,3,'ASSIGN_NOT_ACCEPT',137,_binary '\0',NULL),(53,3,'ASSIGN_NOT_ACCEPT',19,_binary '\0',NULL),(54,3,'ASSIGN_NOT_ACCEPT',1,_binary '\0',NULL),(55,3,'ASSIGN_NOT_ACCEPT',130,_binary '\0',NULL),(56,3,'DONE',137,_binary '\0',NULL),(57,3,'DONE',19,_binary '\0',NULL),(58,3,'DONE',1,_binary '\0',NULL),(59,3,'DONE',130,_binary '\0',NULL),(60,4,'CANCELLED',133,_binary '\0',NULL),(61,4,'CANCELLED',19,_binary '\0',NULL),(62,4,'CANCELLED',1,_binary '\0',NULL),(63,4,'CANCELLED',136,_binary '\0',NULL),(64,4,'NEW',133,_binary '\0',NULL),(65,4,'NEW',19,_binary '\0',NULL),(66,4,'NEW',1,_binary '\0',NULL),(67,4,'ACCEPTED',133,_binary '\0',NULL),(68,4,'ACCEPTED',19,_binary '\0',NULL),(69,4,'ACCEPTED',1,_binary '\0',NULL),(70,4,'FOR_CLEARANCE',133,_binary '\0',NULL),(71,4,'FOR_CLEARANCE',19,_binary '\0',NULL),(72,4,'FOR_CLEARANCE',1,_binary '\0',NULL),(73,4,'CLEARED',133,_binary '\0',NULL),(74,4,'CLEARED',19,_binary '\0',NULL),(75,4,'CLEARED',1,_binary '\0',NULL),(76,4,'CLEARED',136,_binary '\0',NULL),(77,5,'NEW',1,_binary '\0',NULL),(78,5,'WIP',1,_binary '\0',NULL),(79,5,'FOR_APPROVAL',1,_binary '\0',NULL),(80,5,'APPROVED',1,_binary '\0',NULL),(81,5,'FOR_REVISION',1,_binary '\0',NULL),(83,5,'NEW',134,_binary '\0',NULL),(84,5,'WIP',134,_binary '\0',NULL),(85,5,'FOR_APPROVAL',134,_binary '\0',NULL),(86,5,'APPROVED',134,_binary '\0',NULL),(87,5,'FOR_REVISION',134,_binary '\0',NULL),(89,5,'WIP',139,_binary '\0',NULL),(90,5,'FOR_APPROVAL',139,_binary '\0',NULL),(91,5,'APPROVED',140,_binary '\0',NULL),(92,5,'FOR_REVISION',140,_binary '\0',NULL),(94,5,'FOR_REVISION',141,_binary '\0',NULL),(96,5,'NEW',139,_binary '\0',NULL),(97,5,'VOID',1,_binary '\0',NULL),(98,5,'VOID',134,_binary '\0',NULL),(99,4,'ACCEPTED',131,_binary '\0',NULL);
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
  `step_order` int(11) NOT NULL,
  `max_step_order` int(11) NOT NULL,
  `record_id` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `result` varchar(50) CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `approver_remarks` varchar(255) CHARACTER SET latin1 COLLATE latin1_bin DEFAULT NULL,
  `date_scheduled` date DEFAULT NULL,
  `date_completed` date DEFAULT NULL,
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
-- Dumping events for database 'clean_ems_workflow'
--

--
-- Dumping routines for database 'clean_ems_workflow'
--
/*!50003 DROP PROCEDURE IF EXISTS `sp_accountability_add_preloaded_to_employee` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_accountability_add_preloaded_to_employee`(
	IN param_accountability_preloaded_ids VARCHAR(500),
    IN param_employee_id INT,
    IN param_user_id INT
)
BEGIN
	DECLARE local_current_status VARCHAR(50);
	DECLARE local_current_timestamp DATETIME;
	
    -- SET local_current_status = 'NEW';
    SET local_current_status = 
    (SELECT ws.code 
    FROM workflow w 
    INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'ACCOUNTABILITY'
    WHERE ws.order = 1 LIMIT 1);
    SET local_current_timestamp = CURRENT_TIMESTAMP;
    
    INSERT INTO employee_accountability
    (
		  employee_id
        , type
        , title
        , description
        , org_group_id
        , current_status
        , `current_timestamp`
        , created_by
    )
    SELECT 
		  param_employee_id `employee_id`
        , b.type
        , b.title
        , b.description
        , b.org_group_id
        , local_current_status `current_status`
        , local_current_timestamp `current_timestamp`
        , param_user_id `created_by`
    FROM accountability a
    INNER JOIN accountability_details b ON a.id = b.accountability_id
    WHERE FIND_IN_SET(a.id, param_accountability_preloaded_ids)
		AND b.is_active = 1
        AND a.is_active = 1;
        
	INSERT INTO employee_accountability_status_history
    (
		  employee_accountability_id
        , status
        , remarks
        , user_id
        , timestamp
    )
    
    SELECT 
		APPLA_ID.employee_accountability_id `employee_accountability_id`
        , local_current_status `status`
        , NULL `remarks`
        , param_user_id `created_by`
        , local_current_timestamp `timestamp`
    FROM 
    (
		SELECT 
			APPLA.id `employee_accountability_id`
		FROM  employee_accountability APPLA 
		WHERE APPLA.current_timestamp = local_current_timestamp
		
	) APPLA_ID
    LEFT JOIN employee_accountability_status_history APPLASH
		ON APPLA_ID.employee_accountability_id = APPLASH.employee_accountability_id
	WHERE APPLASH.employee_accountability_id IS NULL;
	
    SELECT 
		ROW_COUNT() `id`
        , 0 `employee_accountability_id`
        , '' `status`
        , '' `remarks`
        , 0 `user_id`
        , null `timestamp`;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_accountability_details_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_accountability_details_get`(
	IN param_accountability_id INT
)
BEGIN
	SELECT
		  a.id
        , a.accountability_id
		, IFNULL(b.description, a.type) `type`
        , a.title
        , IFNULL(a.description, '') `description`
        , a.is_active
        , a.org_group_id
        , a.created_by
        , a.created_date
        , a.modified_by
        , a.modified_date
	FROM accountability_details a
    LEFT JOIN reference_value b ON a.type = b.value AND b.ref_code = 'ACCOUNTABILITY_TYPE'
    WHERE accountability_id = param_accountability_id
		AND a.is_active = 1;
	
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_accountability_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_accountability_get_list`(
	IN param_id INT,
	IN param_preload_name VARCHAR(255),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
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
    DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_preload_name VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_preload_name = CASE WHEN param_preload_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_preload_name,'\\','\\\\'), '''', ''''''), '''') END;
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
		, local_param_preload_name , '= '''''
		, ' OR LOWER(IFNULL(preload_name,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_preload_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(created_date, DATE),'''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'id,' END, ''), 
    IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'PreLoadName'			THEN CONCAT('IFNULL(preload_name, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateCreated'	THEN CONCAT('created_date', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_accountability; 
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_accountability
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM accountability a ', where_clause, ') x) `total_num`
			, id
			, preload_name
			, IFNULL(DATE_FORMAT(created_date, ''%m/%d/%Y''), '''') `date_created`
		FROM accountability a '
		, where_clause, order_by_clause
		, (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
		, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_accountability) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;
    
    DROP TEMPORARY TABLE IF EXISTS temp_accountability; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_log_activity_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_log_activity_get`(
	IN param_applicant_id INT
)
BEGIN
	SELECT
		APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, '%m/%d/%Y %h:%i:%s %p') `current_timestamp`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN 'Yes'
                    ELSE 'No'
				END
			ELSE 'N/A'
		END `is_pass`
        , APPLA.created_by
    FROM applicant_log_activity APPLA
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value AND ref_val_type.ref_code = 'ACTIVITY_TYPE'
    LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = 'ACTIVITY_STATUS'
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = 'APPR_ACTIVITY_STATUS'
    WHERE APPLA.is_active = 1 AND APPLA.is_active = 1
		AND APPLA.applicant_id = param_applicant_id;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_log_activity_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_log_activity_get_list`(
	IN param_applicant_id INT,
	IN param_type_delimited VARCHAR(500),
	IN param_sub_type_delimited VARCHAR(500),
	IN param_title VARCHAR(255),
    IN param_description VARCHAR(255),
    IN param_current_status_delimited VARCHAR(500),
    IN param_current_timestamp_from VARCHAR(10),
    IN param_current_timestamp_to VARCHAR(10),
    IN param_due_date_from VARCHAR(10),
    IN param_due_date_to VARCHAR(10),
    IN param_assigned_by_delimited VARCHAR(500),
    IN param_assigned_to_delimited VARCHAR(500),
    IN param_remarks VARCHAR(500),
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
    DECLARE local_param_applicant_id VARCHAR(20);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_sub_type_delimited VARCHAR(1000);
    DECLARE local_param_assigned_by_delimited VARCHAR(1000);
    DECLARE local_param_assigned_to_delimited VARCHAR(1000);
    DECLARE local_param_remarks VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_current_status_delimited VARCHAR(1000);
    DECLARE local_param_current_timestamp_from VARCHAR(50);
    DECLARE local_param_current_timestamp_to VARCHAR(50);
    DECLARE local_param_due_date_from VARCHAR(50);
    DECLARE local_param_due_date_to VARCHAR(50);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_applicant_id = CONVERT(param_applicant_id, CHAR(20));
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN '' ELSE param_type_delimited END;
    SET local_param_sub_type_delimited = CASE WHEN param_sub_type_delimited = '' THEN '' ELSE param_sub_type_delimited END;
    SET local_param_assigned_by_delimited = CASE WHEN param_assigned_by_delimited = '' THEN '' ELSE param_assigned_by_delimited END;
	SET local_param_assigned_to_delimited = CASE WHEN param_assigned_to_delimited = '' THEN '' ELSE param_assigned_to_delimited END;
    SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_status_delimited = CASE WHEN param_current_status_delimited = '' THEN '' ELSE param_current_status_delimited END;
    
    SET local_param_current_timestamp_from = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_from, '%m/%d/%Y') IS NULL OR param_current_timestamp_from = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_current_timestamp_to = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_to, '%m/%d/%Y') IS NULL OR param_current_timestamp_to = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_due_date_from = 
		CASE WHEN STR_TO_DATE(param_due_date_from, '%m/%d/%Y') IS NULL OR param_due_date_from = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_due_date_to = 
		CASE WHEN STR_TO_DATE(param_due_date_to, '%m/%d/%Y') IS NULL OR param_due_date_to = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_to, ''',''%m/%d/%Y''), '''')')  
		END;   
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ' ('
		,''''' = ''', local_param_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.type, ''''),''', local_param_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_sub_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.sub_type, ''''),''', local_param_sub_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(APPLA.title) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(IFNULL(APPLA.description,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_current_status_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.current_status, ''''),''', local_param_current_status_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_by_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.created_by, ''''),''', local_param_assigned_by_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_to_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.assigned_user_id, ''''),''', local_param_assigned_to_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(IFNULL(ALASH.remarks,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''') BETWEEN ', local_param_current_timestamp_from, ' AND ', local_param_current_timestamp_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.due_date, DATE),'''') BETWEEN ', local_param_due_date_from, ' AND ', local_param_due_date_to
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.applicant_id = ', local_param_applicant_id
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'APPLA.current_timestamp DESC ,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Type'				THEN CONCAT('ref_val_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SubType'			THEN CONCAT('ref_val_sub_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'				THEN CONCAT('APPLA.title ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('IFNULL(APPLA.description, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStatus'		THEN CONCAT('ref_val_status.description  ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentTimestamp'	THEN CONCAT('APPLA.current_timestamp ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Remarks'			THEN CONCAT('ALASH.remarks ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'DueDate'	        THEN CONCAT('APPLA.due_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
    
    SET joined_tables = ' applicant_log_activity APPLA
    INNER JOIN applicant_log_activity_status_history ALASH ON APPLA.id = ALASH.applicant_log_activity_id
		AND APPLA.current_status = ALASH.status
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value 
		AND ref_val_type.ref_code = ''ACTIVITY_TYPE''
	LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = ''ACTIVITY_STATUS'' 
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = ''APPR_ACTIVITY_STATUS'' 
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = ''TASK''
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    ';
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_assigned_activities
    SELECT '
	, ' DISTINCT (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '''') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'''')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, ''%m/%d/%Y %h:%i:%s %p'') `current_timestamp`
        , IFNULL(DATE_FORMAT(APPLA.due_date, ''%m/%d/%Y''), '''') `due_date`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN ''Yes''
                    ELSE ''No''
				END
			ELSE ''N/A''
		END `is_pass`
        , APPLA.created_by
        , APPLA.applicant_id
        , ALASH.remarks
        , APPLA.is_with_assignment `is_assignment`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , CONCAT(' LIMIT ', local_param_rows)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_assigned_activities) a WHERE row_num BETWEEN '
    , row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_log_activity_get_used_sub_type` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_log_activity_get_used_sub_type`(
)
BEGIN
	SELECT
		DISTINCT RV_SUBTYPE.id
        , RV_SUBTYPE.ref_code
        , RV_SUBTYPE.value
        , RV_SUBTYPE.description
        , RV_SUBTYPE.user_id
        , RV_SUBTYPE.created_date
    FROM applicant_log_activity APPLA
    INNER JOIN reference_value RV_SUBTYPE ON APPLA.sub_type = RV_SUBTYPE.value 
		AND RV_SUBTYPE.ref_code = APPLA.type
        AND APPLA.is_active = 1;	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_applicant_log_activity_status_history_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_applicant_log_activity_status_history_get`(
	IN param_applicant_activity_log_id INT
)
BEGIN
	SELECT
		ALASH.id
		, IFNULL(ref_val_status.description,'')  `status`
        , ALASH.remarks
        , ALASH.user_id
        , DATE_FORMAT(ALASH.timestamp, '%m/%d/%Y %h:%i:%s %p') `timestamp`
        , CASE WHEN ALA.is_with_pass_fail = 1 
			THEN
				CASE WHEN ALASH.is_pass = 1
					THEN 'Yes'
                    ELSE 'No'
				END
			ELSE 'N/A'
		END `is_pass`
    FROM applicant_log_activity_status_history ALASH
    -- LEFT JOIN reference_value ref_val_status ON ALASH.status = ref_val_status.value AND ref_val_status.ref_code = 'ACTIVITY_STATUS'
--     LEFT JOIN reference_value ref_val_status_appr ON ALASH.status = ref_val_status_appr.value 
-- 		AND ref_val_status_appr.ref_code = 'APPR_ACTIVITY_STATUS'
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    ) ref_val_status ON ref_val_status.code = ALASH.status
    INNER JOIN applicant_log_activity ALA ON ALASH.applicant_log_activity_id = ALA.id
    WHERE ALASH.applicant_log_activity_id = param_applicant_activity_log_id;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_assigned_activities_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_assigned_activities_get_list`(
	IN param_user_id INT,
    IN param_id INT,
    IN param_employee_delimited VARCHAR(500),
    IN param_applicant_delimited VARCHAR(500),
    IN param_type_delimited VARCHAR(500),
    IN param_sub_type_delimited VARCHAR(500),
    IN param_title VARCHAR(255),
    IN param_description VARCHAR(255),
    IN param_current_status_delimited VARCHAR(500),
    IN param_current_timestamp_from VARCHAR(10),
    IN param_current_timestamp_to VARCHAR(10),
    IN param_due_date_from VARCHAR(10),
    IN param_due_date_to VARCHAR(10),
    IN param_assigned_by_delimited VARCHAR(500),
    IN param_org_group_delimited VARCHAR(500),
    IN param_remarks VARCHAR(255),
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
    DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_employee_delimited VARCHAR(1000);
    DECLARE local_param_applicant_delimited VARCHAR(1000);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_sub_type_delimited VARCHAR(1000);
    DECLARE local_param_assigned_by_delimited VARCHAR(1000);
    DECLARE local_param_org_group_delimited VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_current_status_delimited VARCHAR(1000);
    DECLARE local_param_current_timestamp_from VARCHAR(50);
    DECLARE local_param_current_timestamp_to VARCHAR(50);
    DECLARE local_param_due_date_from VARCHAR(50);
    DECLARE local_param_due_date_to VARCHAR(50);
    DECLARE local_param_remarks VARCHAR(1000);
    DECLARE local_param_is_admin VARCHAR(20);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_user_id = CONVERT(param_user_id, CHAR(20));
	SET local_param_id = CONVERT(param_id, CHAR(20));
	SET local_param_employee_delimited = CASE WHEN param_employee_delimited = '' THEN '' ELSE param_employee_delimited END;
    SET local_param_applicant_delimited = CASE WHEN param_applicant_delimited = '' THEN '' ELSE param_applicant_delimited END;
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN '' ELSE param_type_delimited END;
    SET local_param_sub_type_delimited = CASE WHEN param_sub_type_delimited = '' THEN '' ELSE param_sub_type_delimited END;
    SET local_param_assigned_by_delimited = CASE WHEN param_assigned_by_delimited = '' THEN '' ELSE param_assigned_by_delimited END;
    SET local_param_org_group_delimited = CASE WHEN param_org_group_delimited = '' THEN '' ELSE param_org_group_delimited END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_status_delimited = CASE WHEN param_current_status_delimited = '' THEN '' ELSE param_current_status_delimited END;
	SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;

    SET local_param_current_timestamp_from = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_from, '%m/%d/%Y') IS NULL OR param_current_timestamp_from = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_current_timestamp_to = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_to, '%m/%d/%Y') IS NULL OR param_current_timestamp_to = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_due_date_from = 
		CASE WHEN STR_TO_DATE(param_due_date_from, '%m/%d/%Y') IS NULL OR param_due_date_from = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_due_date_to = 
		CASE WHEN STR_TO_DATE(param_due_date_to, '%m/%d/%Y') IS NULL OR param_due_date_to = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_to, ''',''%m/%d/%Y''), '''')')  
		END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    SET local_param_is_admin = CONVERT(param_is_admin, CHAR(20));
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
	,' ('
		, local_param_id , '= 0'
		, ' OR APPLA.id = ', local_param_id
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_employee_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.employee_id, ''''),''', local_param_employee_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_applicant_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.applicant_id, ''''),''', local_param_applicant_delimited, ''')'
    ,' )'
	,' AND'
    ,' ('
		,''''' = ''', local_param_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.type, ''''),''', local_param_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_sub_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.sub_type, ''''),''', local_param_sub_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(APPLA.title) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(IFNULL(APPLA.description,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_current_status_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.current_status, ''''),''', local_param_current_status_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_by_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.created_by, ''''),''', local_param_assigned_by_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_org_group_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.assigned_org_group_id, ''''),''', local_param_org_group_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(APPLA.remarks) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''') BETWEEN ', local_param_current_timestamp_from, ' AND ', local_param_current_timestamp_to
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.assigned_user_id = ', local_param_user_id, ' OR ( 1 = ', local_param_is_admin, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.due_date, DATE),'''') BETWEEN ', local_param_due_date_from, ' AND ', local_param_due_date_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'APPLA.current_timestamp DESC ,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Type'				THEN CONCAT('ref_val_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SubType'			THEN CONCAT('ref_val_sub_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'				THEN CONCAT('APPLA.title ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('IFNULL(APPLA.description, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStatus'		THEN CONCAT('ref_val_status.description  ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentTimestamp'	THEN CONCAT('APPLA.current_timestamp ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'DueDate'			THEN CONCAT('APPLA.due_date ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Remarks'			THEN CONCAT('APPLA.remarks ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
    
    SET joined_tables = ' (
		SELECT 
			  applicant_log_activity.id
			, applicant_id
			, 0 `employee_id`
			, type
			, sub_type
			, title
			, description
			, is_with_pass_fail
			, is_with_assignment
			, applicant_log_activity.is_pass
			, current_status
			, `current_timestamp`
			, assigned_user_id
			, assigned_org_group_id
			, is_active
			, created_by
			, created_date
			, modified_by
			, modified_date
            , due_date
            , ALASH.remarks 
		FROM applicant_log_activity
        LEFT JOIN applicant_log_activity_status_history ALASH ON applicant_log_activity.id = ALASH.applicant_log_activity_id
			AND applicant_log_activity.current_status = ALASH.status
		
        UNION
        
		SELECT 
			  employee_log_activity.id
			, 0 `applicant_id`
			, employee_id
			, type
			, sub_type
			, title
			, description
			, is_with_pass_fail
			, is_with_assignment
			, employee_log_activity.is_pass
			, current_status
			, `current_timestamp`
			, assigned_user_id
			, assigned_org_group_id
			, is_active
			, created_by
			, created_date
			, modified_by
			, modified_date
            , due_date
            , ELASH.remarks
		FROM employee_log_activity
		LEFT JOIN employee_log_activity_status_history ELASH ON employee_log_activity.id = ELASH.employee_log_activity_id
			AND employee_log_activity.current_status = ELASH.status
    ) APPLA
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value 
		AND ref_val_type.ref_code = ''ACTIVITY_TYPE''
	LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = ''ACTIVITY_STATUS'' 
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = ''APPR_ACTIVITY_STATUS'' 
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = ''TASK''
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    ';
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_assigned_activities
    SELECT '
	, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '''') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'''')  `current_status`
        , DATE_FORMAT(APPLA.`current_timestamp`, ''%m/%d/%Y %h:%i:%s %p'') `current_timestamp`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN ''Yes''
                    ELSE ''No''
				END
			ELSE ''N/A''
		END `is_pass`
        , APPLA.created_by
        , APPLA.applicant_id
        , APPLA.employee_id
        , IFNULL(DATE_FORMAT(APPLA.due_date, ''%m/%d/%Y''), '''') `due_date`
        , IFNULL(APPLA.remarks, '''') `remarks`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_assigned_activities) a',
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
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_checklist_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_checklist_get_list`(
	IN param_employee_id INT,
	IN param_type_delimited VARCHAR(500),
	IN param_sub_type_delimited VARCHAR(500),
	IN param_title VARCHAR(255),
    IN param_description VARCHAR(255),
    IN param_current_status_delimited VARCHAR(500),
    IN param_current_timestamp_from VARCHAR(10),
    IN param_current_timestamp_to VARCHAR(10),
    IN param_due_date_from VARCHAR(10),
    IN param_due_date_to VARCHAR(10),
    IN param_assigned_by_delimited VARCHAR(500),
    IN param_assigned_to_delimited VARCHAR(500),
    IN param_remarks VARCHAR(500),
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
    DECLARE local_param_employee_id VARCHAR(20);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_sub_type_delimited VARCHAR(1000);
    DECLARE local_param_assigned_by_delimited VARCHAR(1000);
    DECLARE local_param_assigned_to_delimited VARCHAR(1000);
    DECLARE local_param_remarks VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_current_status_delimited VARCHAR(1000);
    DECLARE local_param_current_timestamp_from VARCHAR(50);
    DECLARE local_param_current_timestamp_to VARCHAR(50);
    DECLARE local_param_due_date_from VARCHAR(50);
    DECLARE local_param_due_date_to VARCHAR(50);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_employee_id = CONVERT(param_employee_id, CHAR(20));
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN '' ELSE param_type_delimited END;
    SET local_param_sub_type_delimited = CASE WHEN param_sub_type_delimited = '' THEN '' ELSE param_sub_type_delimited END;
    SET local_param_assigned_by_delimited = CASE WHEN param_assigned_by_delimited = '' THEN '' ELSE param_assigned_by_delimited END;
	SET local_param_assigned_to_delimited = CASE WHEN param_assigned_to_delimited = '' THEN '' ELSE param_assigned_to_delimited END;
    SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_status_delimited = CASE WHEN param_current_status_delimited = '' THEN '' ELSE param_current_status_delimited END;
    
    SET local_param_current_timestamp_from = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_from, '%m/%d/%Y') IS NULL OR param_current_timestamp_from = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_current_timestamp_to = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_to, '%m/%d/%Y') IS NULL OR param_current_timestamp_to = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_due_date_from = 
		CASE WHEN STR_TO_DATE(param_due_date_from, '%m/%d/%Y') IS NULL OR param_due_date_from = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_due_date_to = 
		CASE WHEN STR_TO_DATE(param_due_date_to, '%m/%d/%Y') IS NULL OR param_due_date_to = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_to, ''',''%m/%d/%Y''), '''')')  
		END;   
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ' ('
		,''''' = ''', local_param_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.type, ''''),''', local_param_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_sub_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.sub_type, ''''),''', local_param_sub_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(APPLA.title) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(IFNULL(APPLA.description,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_current_status_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.current_status, ''''),''', local_param_current_status_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_by_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.created_by, ''''),''', local_param_assigned_by_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_to_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.assigned_user_id, ''''),''', local_param_assigned_to_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(IFNULL(ALASH.remarks,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''') BETWEEN ', local_param_current_timestamp_from, ' AND ', local_param_current_timestamp_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.due_date, DATE),'''') BETWEEN ', local_param_due_date_from, ' AND ', local_param_due_date_to
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.employee_id = ', local_param_employee_id
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.is_visible = 1'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'APPLA.current_timestamp DESC ,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Type'				THEN CONCAT('ref_val_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SubType'			THEN CONCAT('ref_val_sub_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'				THEN CONCAT('APPLA.title ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('IFNULL(APPLA.description, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStatus'		THEN CONCAT('ref_val_status.description  ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentTimestamp'	THEN CONCAT('APPLA.current_timestamp ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Remarks'			THEN CONCAT('ALASH.remarks ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'DueDate'	        THEN CONCAT('APPLA.due_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
    
    SET joined_tables = ' employee_log_activity APPLA
    INNER JOIN employee_log_activity_status_history ALASH ON APPLA.id = ALASH.employee_log_activity_id
		AND APPLA.current_status = ALASH.status
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value 
		AND ref_val_type.ref_code = ''ACTIVITY_TYPE''
	LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = ''ACTIVITY_STATUS'' 
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = ''APPR_ACTIVITY_STATUS'' 
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = ''TASK''
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    ';
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_assigned_activities
    SELECT '
	, ' DISTINCT (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '''') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'''')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, ''%m/%d/%Y %h:%i:%s %p'') `current_timestamp`
        , IFNULL(DATE_FORMAT(APPLA.due_date, ''%m/%d/%Y''), '''') `due_date`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN ''Yes''
                    ELSE ''No''
				END
			ELSE ''N/A''
		END `is_pass`
        , APPLA.created_by
        , APPLA.employee_id
        , ALASH.remarks
        , APPLA.is_with_assignment `is_assignment`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_assigned_activities) a ',
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
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_accountability_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_accountability_get`(
	IN param_employee_id INT
)
BEGIN
	SELECT
		  a.id
	 -- , a.type `type_code`
		, IFNULL(b.description, a.type) `type`
        , a.title `title`
        , IFNULL(a.description, '') `description`
     -- , a.current_status  `current_status_code`
        , IFNULL(c.description, a.current_status)  `current_status`
        , DATE_FORMAT(a.current_timestamp, '%m/%d/%Y %h:%i:%s %p') `current_timestamp`
        , a.created_by
        , a.org_group_id
        , ALASH.user_id `modified_by`
		, ALASH.remarks
    FROM employee_accountability a
    INNER JOIN (
		SELECT 
			b.employee_accountability_id
			, b.remarks
			, b.status
            , b.user_id
		FROM
		(
			SELECT employee_accountability_id, remarks, `status`, `user_id`, MAX(timestamp) `latest_timestamp`
			FROM employee_accountability_status_history
			GROUP BY employee_accountability_id, status
		) a INNER JOIN employee_accountability_status_history b 
		ON a.employee_accountability_id = b.employee_accountability_id
		AND a.status = b.status AND a.latest_timestamp = b.timestamp
    ) ALASH ON a.id = ALASH.employee_accountability_id
		AND a.current_status = ALASH.status
    LEFT JOIN reference_value b ON a.type = b.value AND b.ref_code = 'ACCOUNTABILITY_TYPE'
    -- LEFT JOIN reference_value c ON a.current_status = c.value AND c.ref_code = 'ACCNTABILITY_STATUS'
    LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'ACCOUNTABILITY'
    ) c ON c.code = a.current_status
    WHERE a.is_active = 1
		AND a.employee_id = param_employee_id
	ORDER BY a.current_timestamp DESC;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_accountability_status_history_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_accountability_status_history_get`(
	IN param_employee_accountability_id INT
)
BEGIN
	SELECT
		  a.id
		, IFNULL(b.description,'')  `status`
        , a.remarks
        , a.user_id
        , DATE_FORMAT(a.timestamp, '%m/%d/%Y %h:%i:%s %p') `timestamp`
    FROM employee_accountability_status_history a
    -- LEFT JOIN reference_value b ON a.status = b.value AND b.ref_code = 'ACCNTABILITY_STATUS'
    LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'ACCOUNTABILITY'
    ) b ON b.code = a.status
    INNER JOIN employee_accountability c ON a.employee_accountability_id = c.id
    WHERE a.employee_accountability_id = param_employee_accountability_id;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_log_activity_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_log_activity_get`(
	IN param_employee_id INT
)
BEGIN
	SELECT
		APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, '%m/%d/%Y %h:%i:%s %p') `current_timestamp`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN 'Yes'
                    ELSE 'No'
				END
			ELSE 'N/A'
		END `is_pass`
        , APPLA.created_by
    FROM employee_log_activity APPLA
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value AND ref_val_type.ref_code = 'ACTIVITY_TYPE'
    LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = 'ACTIVITY_STATUS'
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = 'APPR_ACTIVITY_STATUS'
    WHERE APPLA.is_active = 1 AND APPLA.is_active = 1
		AND APPLA.employee_id = param_employee_id;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_log_activity_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_log_activity_get_list`(
	IN param_employee_id INT,
	IN param_type_delimited VARCHAR(500),
	IN param_sub_type_delimited VARCHAR(500),
	IN param_title VARCHAR(255),
    IN param_description VARCHAR(255),
    IN param_current_status_delimited VARCHAR(500),
    IN param_current_timestamp_from VARCHAR(10),
    IN param_current_timestamp_to VARCHAR(10),
    IN param_due_date_from VARCHAR(10),
    IN param_due_date_to VARCHAR(10),
    IN param_assigned_by_delimited VARCHAR(500),
    IN param_assigned_to_delimited VARCHAR(500),
    IN param_remarks VARCHAR(500),
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
    DECLARE local_param_employee_id VARCHAR(20);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_sub_type_delimited VARCHAR(1000);
    DECLARE local_param_assigned_by_delimited VARCHAR(1000);
    DECLARE local_param_assigned_to_delimited VARCHAR(1000);
    DECLARE local_param_remarks VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_current_status_delimited VARCHAR(1000);
    DECLARE local_param_current_timestamp_from VARCHAR(50);
    DECLARE local_param_current_timestamp_to VARCHAR(50);
    DECLARE local_param_due_date_from VARCHAR(50);
    DECLARE local_param_due_date_to VARCHAR(50);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_employee_id = CONVERT(param_employee_id, CHAR(20));
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN '' ELSE param_type_delimited END;
    SET local_param_sub_type_delimited = CASE WHEN param_sub_type_delimited = '' THEN '' ELSE param_sub_type_delimited END;
    SET local_param_assigned_by_delimited = CASE WHEN param_assigned_by_delimited = '' THEN '' ELSE param_assigned_by_delimited END;
	SET local_param_assigned_to_delimited = CASE WHEN param_assigned_to_delimited = '' THEN '' ELSE param_assigned_to_delimited END;
    SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_status_delimited = CASE WHEN param_current_status_delimited = '' THEN '' ELSE param_current_status_delimited END;
    
    SET local_param_current_timestamp_from = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_from, '%m/%d/%Y') IS NULL OR param_current_timestamp_from = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_current_timestamp_to = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_to, '%m/%d/%Y') IS NULL OR param_current_timestamp_to = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_to, ''',''%m/%d/%Y''), '''')')  
		END;
        
	SET local_param_due_date_from = 
		CASE WHEN STR_TO_DATE(param_due_date_from, '%m/%d/%Y') IS NULL OR param_due_date_from = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_due_date_to = 
		CASE WHEN STR_TO_DATE(param_due_date_to, '%m/%d/%Y') IS NULL OR param_due_date_to = ''
			THEN 'IFNULL(CONVERT(APPLA.due_date, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_due_date_to, ''',''%m/%d/%Y''), '''')')  
		END;   
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ' ('
		,''''' = ''', local_param_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.type, ''''),''', local_param_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,''''' = ''', local_param_sub_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.sub_type, ''''),''', local_param_sub_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(APPLA.title) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(IFNULL(APPLA.description,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_current_status_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.current_status, ''''),''', local_param_current_status_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_by_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.created_by, ''''),''', local_param_assigned_by_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_assigned_to_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.assigned_user_id, ''''),''', local_param_assigned_to_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(IFNULL(ALASH.remarks,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''') BETWEEN ', local_param_current_timestamp_from, ' AND ', local_param_current_timestamp_to
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.due_date, DATE),'''') BETWEEN ', local_param_due_date_from, ' AND ', local_param_due_date_to
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.employee_id = ', local_param_employee_id
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'APPLA.current_timestamp DESC ,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Type'				THEN CONCAT('ref_val_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'SubType'			THEN CONCAT('ref_val_sub_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'				THEN CONCAT('APPLA.title ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('IFNULL(APPLA.description, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStatus'		THEN CONCAT('ref_val_status.description  ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentTimestamp'	THEN CONCAT('APPLA.current_timestamp ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'Remarks'			THEN CONCAT('ALASH.remarks ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'DueDate'	        THEN CONCAT('APPLA.due_date ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
    
    SET joined_tables = ' employee_log_activity APPLA
    INNER JOIN employee_log_activity_status_history ALASH ON APPLA.id = ALASH.employee_log_activity_id
		AND APPLA.current_status = ALASH.status
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value 
		AND ref_val_type.ref_code = ''ACTIVITY_TYPE''
	LEFT JOIN reference_value ref_val_sub_type ON APPLA.sub_type = ref_val_sub_type.value AND ref_val_sub_type.ref_code = APPLA.type
    -- LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = ''ACTIVITY_STATUS'' 
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = ''APPR_ACTIVITY_STATUS'' 
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = ''TASK''
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    ';
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_assigned_activities
    SELECT '
	, ' DISTINCT (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
		, IFNULL(ref_val_sub_type.description, APPLA.sub_type) `sub_type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '''') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'''')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, ''%m/%d/%Y %h:%i:%s %p'') `current_timestamp`
        , IFNULL(DATE_FORMAT(APPLA.due_date, ''%m/%d/%Y''), '''') `due_date`
        , APPLA.assigned_user_id `assigned_user_id`
        , APPLA.assigned_org_group_id `assigned_org_group_id`
        , CASE WHEN APPLA.is_with_pass_fail = 1 
			THEN
				CASE WHEN APPLA.is_pass = 1
					THEN ''Yes''
                    ELSE ''No''
				END
			ELSE ''N/A''
		END `is_pass`
        , APPLA.created_by
        , APPLA.employee_id
        , ALASH.remarks
        , APPLA.is_with_assignment `is_assignment`
	FROM ', joined_tables
    , where_clause, order_by_clause
    , CONCAT(' LIMIT ', local_param_rows)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_assigned_activities) a WHERE row_num BETWEEN '
    , row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;

-- select CONCAT(@create_temp_table, @select_query);
    DROP TEMPORARY TABLE IF EXISTS temp_assigned_activities; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_log_activity_status_history_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_log_activity_status_history_get`(
	IN param_employee_activity_log_id INT
)
BEGIN
	SELECT
		ALASH.id
		, IFNULL(ref_val_status.description,'')  `status`
        , ALASH.remarks
        , ALASH.user_id
        , DATE_FORMAT(ALASH.timestamp, '%m/%d/%Y %h:%i:%s %p') `timestamp`
        , CASE WHEN ALA.is_with_pass_fail = 1 
			THEN
				CASE WHEN ALASH.is_pass = 1
					THEN 'Yes'
                    ELSE 'No'
				END
			ELSE 'N/A'
		END `is_pass`
    FROM employee_log_activity_status_history ALASH
    -- LEFT JOIN reference_value ref_val_status ON ALASH.status = ref_val_status.value AND ref_val_status.ref_code = 'ACTIVITY_STATUS'
--     LEFT JOIN reference_value ref_val_status_appr ON ALASH.status = ref_val_status_appr.value 
-- 		AND ref_val_status_appr.ref_code = 'APPR_ACTIVITY_STATUS'
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    ) ref_val_status ON ref_val_status.code = ALASH.status
    INNER JOIN employee_log_activity ALA ON ALASH.employee_log_activity_id = ALA.id
    WHERE ALASH.employee_log_activity_id = param_employee_activity_log_id;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_employee_score_status_history_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employee_score_status_history_get`(
	IN param_employee_score_t_id INT
)
BEGIN
	SELECT
		ALASH.id
		, IFNULL(ref_val_status.description,'')  `status`
        , ALASH.remarks
        , ALASH.approver_id
        , DATE_FORMAT(ALASH.timestamp, '%m/%d/%Y %h:%i:%s %p') `timestamp`
    FROM employee_score_approval_history ALASH
    -- LEFT JOIN reference_value ref_val_status ON ALASH.status = ref_val_status.value AND ref_val_status.ref_code = 'ACTIVITY_STATUS'
--     LEFT JOIN reference_value ref_val_status_appr ON ALASH.status = ref_val_status_appr.value 
-- 		AND ref_val_status_appr.ref_code = 'APPR_ACTIVITY_STATUS'
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'IPM'
    ) ref_val_status ON ref_val_status.code = ALASH.status
    WHERE ALASH.t_id = param_employee_score_t_id
    ORDER BY ALASH.timestamp
    ;
	
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_log_activity_add_preloaded_to_applicant` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_log_activity_add_preloaded_to_applicant`(
	IN param_log_activity_preloaded_ids VARCHAR(500),
    IN param_applicant_id INT,
    IN param_user_id INT
)
BEGIN
	DECLARE local_current_status VARCHAR(50);
	DECLARE local_current_timestamp DATETIME;
	
    -- SET local_current_status = 
-- 		(SELECT value FROM reference_value WHERE ref_code = 'ACTIVITY_STATUS' order by value LIMIT 1);
	SET local_current_status = 
    (SELECT ws.code 
    FROM workflow w 
    INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    WHERE ws.order = 1 LIMIT 1);
	
    SET local_current_timestamp = CURRENT_TIMESTAMP;
    
    INSERT INTO applicant_log_activity
    (
		applicant_id
        , type
        , sub_type
        , title
        , description
        , is_with_pass_fail
		, is_with_assignment
        , is_pass
        , current_status
        , `current_timestamp`
        , assigned_user_id
        , assigned_org_group_id
        , created_by
        , due_date
    )
    SELECT 
		param_applicant_id `appplicant_id`
        , LA.type
        , LA.sub_type
        , LA.title
        , LA.description
        , LA.is_with_pass_fail
        , LA.is_with_assignment
        , b'0' `is_pass`
        , local_current_status `current_status`
        , local_current_timestamp `current_timestamp`
        , 0 `assigned_user_id`
        , 0 `assigned_org_group_id`
        , param_user_id `created_by`
        , now()
    FROM log_activity LA
    INNER JOIN log_activity_preloaded LAP ON LA.log_activity_preloaded_id = LAP.id
    WHERE FIND_IN_SET(LAP.id, param_log_activity_preloaded_ids)
		AND LA.is_active = 1
        AND LAP.is_active = 1;
        
	INSERT INTO applicant_log_activity_status_history
    (
		applicant_log_activity_id
        , status
        , remarks
        , user_id
        , timestamp
    )
    
    SELECT 
		APPLA_ID.applicant_log_activity_id `applicant_log_activity_id`
        , local_current_status `status`
        , NULL `remarks`
        , param_user_id `created_by`
        , local_current_timestamp `timestamp`
    FROM 
    (
    
		SELECT 
			APPLA.id `applicant_log_activity_id`
		FROM  applicant_log_activity APPLA 
		WHERE APPLA.current_timestamp = local_current_timestamp

	) APPLA_ID
    LEFT JOIN applicant_log_activity_status_history APPLASH
		ON APPLA_ID.applicant_log_activity_id = APPLASH.applicant_log_activity_id
	WHERE APPLASH.applicant_log_activity_id IS NULL;
	
    SELECT 
		ROW_COUNT() `id`
        , 0 `applicant_log_activity_id`
        , '' `status`
        , '' `remarks`
        , 0 `user_id`
        , null `timestamp`
        , 0 `is_pass`;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_log_activity_add_preloaded_to_employee` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_log_activity_add_preloaded_to_employee`(
	IN param_log_activity_preloaded_ids VARCHAR(500),
    IN param_employee_id INT,
    IN param_user_id INT
)
BEGIN
	DECLARE local_current_status VARCHAR(50);
	DECLARE local_current_timestamp DATETIME;
	
    -- SET local_current_status = 
-- 		(SELECT value FROM reference_value WHERE ref_code = 'ACTIVITY_STATUS' order by value LIMIT 1);
	SET local_current_status = 
    (SELECT ws.code 
    FROM workflow w 
    INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = 'TASK'
    WHERE ws.order = 1 LIMIT 1);
    
    SET local_current_timestamp = CURRENT_TIMESTAMP;
    
    INSERT INTO employee_log_activity
    (
		employee_id
        , type
        , sub_type
        , title
        , description
        , is_with_pass_fail
		, is_with_assignment
        , is_pass
        , current_status
        , `current_timestamp`
        , assigned_user_id
        , assigned_org_group_id
        , created_by
        , is_visible
        , due_date
    )
    SELECT 
		param_employee_id `employee_id`
        , LA.type
        , LA.sub_type
        , LA.title
        , LA.description
        , LA.is_with_pass_fail
        , LA.is_with_assignment
        , b'0' `is_pass`
        , local_current_status `current_status`
        , local_current_timestamp `current_timestamp`
        , assigned_user_id
        , 0 `assigned_org_group_id`
        , param_user_id `created_by`
        , LA.is_visible
        , now()
    FROM log_activity LA
    INNER JOIN log_activity_preloaded LAP ON LA.log_activity_preloaded_id = LAP.id
    WHERE FIND_IN_SET(LAP.id, param_log_activity_preloaded_ids)
		AND LA.is_active = 1
        AND LAP.is_active = 1;
        
	INSERT INTO employee_log_activity_status_history
    (
		employee_log_activity_id
        , status
        , remarks
        , user_id
        , timestamp
    )
    
    SELECT 
		APPLA_ID.employee_log_activity_id `employee_log_activity_id`
        , local_current_status `status`
        , NULL `remarks`
        , param_user_id `created_by`
        , local_current_timestamp `timestamp`
    FROM 
    (
		SELECT 
			APPLA.id `employee_log_activity_id`
		FROM  employee_log_activity APPLA 
		WHERE APPLA.current_timestamp = local_current_timestamp

	) APPLA_ID
    LEFT JOIN employee_log_activity_status_history APPLASH
		ON APPLA_ID.employee_log_activity_id = APPLASH.employee_log_activity_id
	WHERE APPLASH.employee_log_activity_id IS NULL;
	
    SELECT 
		ROW_COUNT() `id`
        , 0 `employee_log_activity_id`
        , '' `status`
        , '' `remarks`
        , 0 `user_id`
        , null `timestamp`
        , 0 `is_pass`;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_log_activity_get_by_log_activity_preloaded_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_log_activity_get_by_log_activity_preloaded_id`(
	IN param_log_activity_preloaded_id INT
)
BEGIN
	SELECT
		LA.id
        , LA.module
		, IFNULL(RV_TYPE.description, LA.type) `type`
		, IFNULL(RV_SUBTYPE.description, LA.sub_type) `sub_type`
        , LA.title
        , IFNULL(LA.description, '') `description`
        , LA.is_with_pass_fail
        , LA.is_with_assignment
        , LA.log_activity_preloaded_id
        , LA.is_visible
        , LA.assigned_user_id
        , LA.is_active
        , LA.created_by
        , LA.created_date
        , LA.modified_by
        , LA.modified_date
	FROM log_activity LA
    LEFT JOIN reference_value RV_TYPE ON LA.type = RV_TYPE.value AND RV_TYPE.ref_code = 'ACTIVITY_TYPE'
    LEFT JOIN reference_value RV_SUBTYPE ON LA.sub_type = RV_SUBTYPE.value AND RV_SUBTYPE.ref_code = LA.type
    WHERE log_activity_preloaded_id = param_log_activity_preloaded_id
		AND param_log_activity_preloaded_id <> 0
		AND LA.is_active = 1;
	
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_log_activity_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_log_activity_get_list`(
    IN param_module_delimited VARCHAR(500),
    IN param_type_delimited VARCHAR(500),
    IN param_title VARCHAR(500),
    IN param_description VARCHAR(500),
    IN param_is_pass_fail VARCHAR(20),
	IN param_is_assignment VARCHAR(20),
    IN param_is_preloaded VARCHAR(20),
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
    DECLARE local_param_module_delimited VARCHAR(1000);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_is_pass_fail VARCHAR(20);
    DECLARE local_param_is_assignment VARCHAR(20);
    DECLARE local_param_is_preloaded VARCHAR(20);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_module_delimited = CASE WHEN param_module_delimited = '' THEN 'b.id' ELSE param_module_delimited END;
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN 'c.id' ELSE param_type_delimited END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_is_pass_fail = CASE WHEN param_is_pass_fail = '' THEN 'd.id' ELSE param_is_pass_fail END;
    SET local_param_is_assignment = CASE WHEN param_is_assignment = '' THEN 'e.id' ELSE param_is_assignment END;
    SET local_param_is_preloaded = CASE WHEN param_is_preloaded = '' THEN 'f.id' ELSE param_is_preloaded END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
    ,' ('
		,' a.is_active = 1'
    ,' )'
	,' AND'
    ,' ('
		,' b.id IN (', local_param_module_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' c.id IN (', local_param_type_delimited, ')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(a.title) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(a.description) LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description, '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' d.id IN (', local_param_is_pass_fail, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' e.id IN (', local_param_is_assignment, ')'
    ,' )'
    ,' AND'
    ,' ('
		,' f.id IN (', local_param_is_preloaded, ')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'a.id DESC,' END, ''), 
    IFNULL(CASE WHEN sidx = 'Module'		THEN CONCAT('b.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Type'		    THEN CONCAT('c.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Title'		    THEN CONCAT('a.title ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Description'	THEN CONCAT('a.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'IsPassFail'	THEN CONCAT('d.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'IsAssignment'	THEN CONCAT('e.description ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'IsPreLoaded'	THEN CONCAT('f.description ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_log_activity; 
    
    SET joined_tables = "log_activity a
	INNER JOIN reference_value b ON a.module = b.value AND b.ref_code = ""ACTIVITY_MODULE""
	INNER JOIN reference_value c ON a.type = c.value AND c.ref_code = ""ACTIVITY_TYPE""
    INNER JOIN reference_value d ON CASE WHEN a.is_with_pass_fail = 1 THEN ""YES"" ELSE ""NO"" END = d.value AND d.ref_code = ""YES_NO""
	INNER JOIN reference_value e ON CASE WHEN a.is_with_assignment = 1 THEN ""YES"" ELSE ""NO"" END = e.value AND e.ref_code = ""YES_NO""
    INNER JOIN reference_value f ON CASE WHEN a.is_preloaded = 1 THEN ""YES"" ELSE ""NO"" END = f.value AND f.ref_code = ""YES_NO""
    ";
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_log_activity
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
            , a.id
			, b.description `module`
			, c.description `type`
			, a.title
            , a.description
            , d.description `is_pass_fail`
            , e.description `is_assignment`
            , f.description `is_preloaded`
		FROM ', joined_tables
		, where_clause, order_by_clause
        , ' LIMIT ', local_param_rows, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_log_activity) a WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows,';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;
    
    DROP TEMPORARY TABLE IF EXISTS temp_log_activity; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_log_activity_preloaded_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_log_activity_preloaded_get_list`(
	IN param_id INT,
	IN param_preload_name VARCHAR(255),
    IN param_date_created_from VARCHAR(10),
    IN param_date_created_to VARCHAR(10),
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
    DECLARE local_param_id VARCHAR(20);
    DECLARE local_param_preload_name VARCHAR(1000);
    DECLARE local_param_date_created_from VARCHAR(50);
    DECLARE local_param_date_created_to VARCHAR(50);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_id = CONVERT(param_id, CHAR(20));
    SET local_param_preload_name = CASE WHEN param_preload_name = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_preload_name,'\\','\\\\'), '''', ''''''), '''') END;
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
		, local_param_preload_name , '= '''''
		, ' OR LOWER(IFNULL(preload_name,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_preload_name , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(created_date, DATE),'''') BETWEEN ', local_param_date_created_from, ' AND ', local_param_date_created_to
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''				THEN 'id DESC,' END, ''), 
    IFNULL(CASE WHEN sidx = 'ID'			THEN CONCAT('id ', local_sord, ',') END, ''),
	IFNULL(CASE WHEN sidx = 'PreLoadName'			THEN CONCAT('IFNULL(preload_name, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'DateCreated'	THEN CONCAT('created_date', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_log_activity_preloaded; 
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
		CREATE TEMPORARY TABLE temp_log_activity_preloaded
		SELECT '
		, ' (SELECT COUNT(*) FROM (SELECT 1 FROM log_activity_preloaded a ', where_clause, ') x) `total_num`
			, id
			, preload_name
			, IFNULL(DATE_FORMAT(created_date, ''%m/%d/%Y''), '''') `date_created`
		FROM log_activity_preloaded a '
		, where_clause, order_by_clause
		, (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
		, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_log_activity_preloaded) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
    -- Execute first statement
    PREPARE stmt FROM @create_temp_table;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;
    
    -- Execute second statement
    PREPARE stmt1 FROM @select_query;
	EXECUTE stmt1;
	DEALLOCATE PREPARE stmt1;
    
    DROP TEMPORARY TABLE IF EXISTS temp_log_activity_preloaded; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_my_accountabilities_get_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_my_accountabilities_get_list`(
	IN param_employee_id INT,
	IN param_type_delimited VARCHAR(500),
	IN param_title VARCHAR(255),
    IN param_description VARCHAR(255),
    IN param_current_status_delimited VARCHAR(500),
    IN param_current_timestamp_from VARCHAR(10),
    IN param_current_timestamp_to VARCHAR(10),
    IN param_org_group_id_delimited VARCHAR(500),
    IN param_employee_id_delimited VARCHAR(500),
    IN param_remarks VARCHAR(255),
    IN param_is_admin BOOLEAN,
    IN param_is_clearance BOOLEAN,
    IN param_is_export BOOLEAN,
    IN param_modified_by_delimited VARCHAR(500),
    IN param_old_employee_id VARCHAR(255),
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
    DECLARE local_param_employee_id VARCHAR(20);
    DECLARE local_param_type_delimited VARCHAR(1000);
    DECLARE local_param_title VARCHAR(1000);
    DECLARE local_param_description VARCHAR(1000);
    DECLARE local_param_current_status_delimited VARCHAR(1000);
    DECLARE local_param_current_timestamp_from VARCHAR(50);
    DECLARE local_param_current_timestamp_to VARCHAR(50);
    DECLARE local_param_org_group_id_delimited VARCHAR(1000);
    DECLARE local_param_employee_id_delimited VARCHAR(1000);
	DECLARE local_param_remarks VARCHAR(1000);
    DECLARE local_param_is_admin VARCHAR(20);
    DECLARE local_param_is_clearance VARCHAR(20);
    DECLARE local_param_modified_by_delimited VARCHAR(1000);
	DECLARE local_param_old_employee_id VARCHAR(1000);
    
    DECLARE local_sord VARCHAR(4);
    DECLARE local_param_rows INT;
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_param_employee_id = CONVERT(param_employee_id, CHAR(20));
    SET local_param_type_delimited = CASE WHEN param_type_delimited = '' THEN '' ELSE param_type_delimited END;
    SET local_param_title = CASE WHEN param_title = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_title,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_description = CASE WHEN param_description = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_description,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_current_status_delimited = CASE WHEN param_current_status_delimited = '' THEN '' ELSE param_current_status_delimited END;
    SET local_param_org_group_id_delimited = CASE WHEN param_org_group_id_delimited = '' THEN '' ELSE param_org_group_id_delimited END;
    SET local_param_employee_id_delimited = CASE WHEN param_employee_id_delimited = '' THEN '' ELSE param_employee_id_delimited END;
    SET local_param_remarks = CASE WHEN param_remarks = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(param_remarks,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_param_is_admin = CONVERT(param_is_admin, CHAR(20));
    SET local_param_is_clearance = CONVERT(param_is_clearance, CHAR(20));
    
    SET local_param_current_timestamp_from = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_from, '%m/%d/%Y') IS NULL OR param_current_timestamp_from = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_from, ''',''%m/%d/%Y''), '''')')  
		END;
        
    SET local_param_current_timestamp_to = 
		CASE WHEN STR_TO_DATE(param_current_timestamp_to, '%m/%d/%Y') IS NULL OR param_current_timestamp_to = ''
			THEN 'IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''')' 
            ELSE CONCAT('IFNULL(STR_TO_DATE(''', param_current_timestamp_to, ''',''%m/%d/%Y''), '''')')  
		END;
    
	SET local_param_modified_by_delimited = CASE WHEN param_modified_by_delimited = '' THEN '' ELSE param_modified_by_delimited END;
    SET local_param_old_employee_id = CASE WHEN param_old_employee_id = '' THEN '' ELSE param_old_employee_id END;
    
    SET local_sord = CASE WHEN sord = '' THEN 'ASC' ELSE sord END;
    SET local_param_rows = param_rows + row_start - 1;
    
    -- Construct where clause then concat the converted parameters
    SET where_clause = CONCAT('WHERE'
     ' ('
		,''''' = ''', local_param_type_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.type, ''''),''', local_param_type_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_title , '= '''''
		, ' OR LOWER(APPLA.title) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_title , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_description , '= '''''
		, ' OR LOWER(IFNULL(APPLA.description,'''')) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_description , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
    ,' ('
		, local_param_remarks , '= '''''
		, ' OR LOWER(ALASH.remarks) 
				LIKE CONCAT(''%'', REPLACE(LOWER(' , local_param_remarks , '), '' '', ''%''), ''%'')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_current_status_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.current_status, ''''),''', local_param_current_status_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_org_group_id_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.org_group_id, ''''),''', local_param_org_group_id_delimited, ''')'
    ,' )'
     ,' AND'
     ' ('
		,''''' = ''', local_param_employee_id_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.employee_id, ''''),''', local_param_employee_id_delimited, ''')'
    ,' )'
    ,' AND'
    ,' ('
		,' IFNULL(CONVERT(APPLA.current_timestamp, DATE),'''') BETWEEN ', local_param_current_timestamp_from, ' AND ', local_param_current_timestamp_to
    ,' )'
    ,' AND'
    ,' ('
		,' APPLA.employee_id = ', local_param_employee_id, ' OR ( 1 = ', local_param_is_admin, ' AND 0 = ', local_param_is_clearance, ' )' 
        , ' OR '
        ,'( '
        ,'		1 = ', local_param_is_clearance 
        ,' 		AND'
        ,'		FIND_IN_SET(IFNULL(APPLA.org_group_id, ''''),''', local_param_org_group_id_delimited, ''')'
        ,' 		AND'
        ,'		APPLA.current_status IN (''FOR_CLEARANCE'', ''CLEARED'', ''CANCELLED'')'
		,')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_modified_by_delimited, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(ALASH.modified_by, ''''),''', local_param_modified_by_delimited, ''')'
    ,' )'
    ,' AND'
     ' ('
		,''''' = ''', local_param_old_employee_id, ''''
		,' OR '
		,' FIND_IN_SET(IFNULL(APPLA.employee_id, ''''),''', local_param_old_employee_id, ''')'
    ,' )'
    );
    
     -- Construct order by clause then concat the converted parameters
    SET order_by_clause = CONCAT(
    ' ORDER BY ',  
	IFNULL(CASE WHEN sidx = ''					THEN 'APPLA.current_timestamp DESC ,' END, ''), 
	IFNULL(CASE WHEN sidx = 'Type'				THEN CONCAT('ref_val_type.description ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Title'				THEN CONCAT('APPLA.title ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'Description'		THEN CONCAT('IFNULL(APPLA.description, '''') ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentStatus'		THEN CONCAT('ref_val_status.description  ', local_sord, ',') END, ''), 
	IFNULL(CASE WHEN sidx = 'CurrentTimestamp'	THEN CONCAT('APPLA.current_timestamp ', local_sord, ',') END, ''),
    IFNULL(CASE WHEN sidx = 'Remarks'	        THEN CONCAT('ALASH.remarks ', local_sord, ',') END, '')
    );
    
     -- Removal of last comma
    SET order_by_clause = SUBSTRING(order_by_clause,1,LENGTH(order_by_clause)-1);
    
    SET @row_num = 0;
    DROP TEMPORARY TABLE IF EXISTS temp_employee_accountability; 
    
    SET joined_tables = ' employee_accountability APPLA
    INNER JOIN (
		SELECT 
			b.employee_accountability_id
			, b.remarks
			, b.status
            , b.user_id `modified_by`
		FROM
		(
			SELECT employee_accountability_id, remarks, `status`, user_id, MAX(timestamp) `latest_timestamp`
			FROM employee_accountability_status_history
			GROUP BY employee_accountability_id, status
		) a INNER JOIN employee_accountability_status_history b 
		ON a.employee_accountability_id = b.employee_accountability_id
		AND a.status = b.status AND a.latest_timestamp = b.timestamp
    ) ALASH ON APPLA.id = ALASH.employee_accountability_id
		AND APPLA.current_status = ALASH.status
    LEFT JOIN reference_value ref_val_type ON APPLA.type = ref_val_type.value 
		AND ref_val_type.ref_code = ''ACCOUNTABILITY_TYPE''
--     LEFT JOIN reference_value ref_val_status ON APPLA.current_status = ref_val_status.value AND ref_val_status.ref_code = ''ACCNTABILITY_STATUS'' 
--     LEFT JOIN reference_value ref_val_status_appr ON APPLA.current_status = ref_val_status_appr.value AND ref_val_status_appr.ref_code = ''ACCNTABILITY_STATUS'' 
	LEFT JOIN (
		SELECT ws.code ,ws.description
        FROM workflow w
        INNER JOIN workflow_step ws ON w.id = ws.workflow_id AND w.code = ''ACCOUNTABILITY''
    ) ref_val_status ON ref_val_status.code = APPLA.current_status
    ';
		
     -- Merge query portions to create the first statement
    SET @create_temp_table = CONCAT('
    CREATE TEMPORARY TABLE temp_employee_accountability
    SELECT '
	, ' DISTINCT (SELECT COUNT(*) FROM (SELECT 1 FROM ', joined_tables, where_clause, ') x) `total_num`
		, APPLA.id
		, APPLA.type `type_code`
		, IFNULL(ref_val_type.description, APPLA.type) `type`
        , APPLA.title `title`
        , IFNULL(APPLA.description, '''') `description`
        , APPLA.current_status  `current_status_code`
        , IFNULL(ref_val_status.description,'''')  `current_status`
        , DATE_FORMAT(APPLA.current_timestamp, ''%m/%d/%Y %h:%i:%s %p'') `current_timestamp`
        , APPLA.org_group_id `org_group_id`
        , APPLA.created_by
        , APPLA.employee_id
        , ALASH.remarks
        , ALASH.modified_by
	FROM ', joined_tables
    , where_clause, order_by_clause
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_employee_accountability) a ',
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
    DROP TEMPORARY TABLE IF EXISTS temp_employee_accountability; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_reference_value_autocomplete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_reference_value_autocomplete`(
	IN ref_code VARCHAR(50),
    IN term VARCHAR(255),
    IN top_results INT
)
BEGIN
	-- Define parameters for conversion to string
	DECLARE local_ref_code VARCHAR(100);
    DECLARE local_term VARCHAR(500);
    DECLARE local_top_results VARCHAR(20);
    
    -- Convert passed parameters into string, to be concatenated into query portions
    SET local_ref_code = CASE WHEN ref_code = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(ref_code,'\\','\\\\'), '''', ''''''), '''') END;
    SET local_term = CASE WHEN term = '' THEN '''''' ELSE CONCAT('''', REPLACE(REPLACE(REPLACE(term,'\\','\\\\'), ' ', '%'), '''', ''''''), '''') END;
    SET local_top_results = CONVERT(top_results, CHAR(20));
    
	  -- Construct dynamic query then concat the converted parameters
     SET @dynamic_query = CONCAT('
    SELECT DISTINCT id
		, ref_code
        , value
		, INSTR(CONCAT(value, '' - '', description), ', local_term, ') `index`
        , description
        , 0 user_id
        , CURRENT_TIMESTAMP() created_date
	FROM reference_value
	WHERE LOWER(CONCAT(value, '' - '', description)) LIKE LOWER(CONCAT(''%'',', local_term, ',''%''))
		AND ref_code =', local_ref_code, '
	ORDER BY `index`
		, CONCAT(value, '' - '', description) ',
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_add_transaction` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_add_transaction`(
		IN param_workflow_code VARCHAR(50),
		-- IN param_request_type VARCHAR(20),
		IN param_step_code VARCHAR(50),
		IN param_approver_id INT,
		IN param_record_id INT,
		IN param_result VARCHAR(50),
		IN param_remarks VARCHAR(500),
		IN param_date_scheduled VARCHAR(20),
		IN param_date_completed VARCHAR(20),
		IN param_start_date DATETIME
	)
BEGIN
		DECLARE local_step_order INT;
		DECLARE local_max_step_order INT;
		DECLARE local_end_date_time DATETIME;
		
		SET local_end_date_time = CURRENT_TIMESTAMP;
		SELECT 
			wfs.order
			, max_step_order
		INTO 
			local_step_order
			, local_max_step_order 
		FROM workflow wf 
		INNER JOIN workflow_step wfs ON wfs.workflow_id = wf.id 
			AND wf.code = param_workflow_code
			AND wfs.code = param_step_code
		INNER JOIN 
		(
			SELECT workflow_id 
				, max(`order`) `max_step_order` 
			FROM workflow_step 
			GROUP BY workflow_id
		) wfs_max ON wf.id = wfs_max.workflow_id
		LIMIT 1;
		
		INSERT INTO workflow_transaction
		(`workflow_code`,
		-- `request_type`,
		`step_code`,
		`step_order`,
		`max_step_order`,
		`record_id`,
		`result`,
		`date_scheduled`,
		`date_completed`,
		`start_date_time`,
		`end_date_time`,
		`approved_by`,
		`approver_remarks`)
		VALUES(param_workflow_code
			-- , param_request_type
			, param_step_code
			, local_step_order
			, local_max_step_order
			, param_record_id
			, param_result
			, STR_TO_DATE(param_date_scheduled, '%m/%d/%Y')
			, STR_TO_DATE(param_date_completed, '%m/%d/%Y')
			, param_start_date
            , local_end_date_time
			, param_approver_id
			, param_remarks);

			SELECT
				wfs.code AS `step_code`,
				wfs.description AS `step_description`,
				GROUP_CONCAT(wfsa.role_id) AS `approver_role_ids`,
                CASE WHEN wfs_max.max_order = local_step_order AND param_step_code != 'FAILED'
					THEN 'COMPLETED'
					WHEN param_result IN (SELECT value FROM reference_value WHERE ref_code = 'NEGATIVE_RESULT_TYPE') 
						OR param_step_code = 'FAILED'
                    THEN 'FAILED'
                    ELSE 'IN_PROGRESS'
                END `workflow_status`
			FROM workflow_step wfs
			INNER JOIN workflow wf ON wfs.workflow_id = wf.id
			INNER JOIN workflow_step_approver wfsa ON wfsa.workflow_id = wf.id
				AND wfs.code = wfsa.step_code
			LEFT JOIN (
				SELECT 
					MAX(`order`) `max_order`
					, workflow_id 
				FROM workflow_step GROUP BY workflow_id
			) wfs_max ON wf.id = wfs_max.workflow_id
				-- AND wfs.order = wfs_max.max_order
			WHERE wf.code = param_workflow_code 
				AND (
						wfs.order = (local_step_order) 
                        -- OR wfs_max.max_order = local_step_order
					)
			-- GROUP BY wf.code;
			GROUP BY wf.code, wfsa.step_code, wfs_max.workflow_id;
            
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
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_all_workflow_step` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_all_workflow_step`(
IN param_workflow_code VARCHAR(100)
)
BEGIN

	SELECT
		ws.id
       , ws.code `code`
       , ws.description `description`
       , ws.order
       , w.code `workflow_code`
	FROM workflow w
   INNER JOIN workflow_step ws ON w.id = ws.workflow_id
   WHERE w.code = param_workflow_code;
   
   
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_list`(
	IN param_id INT,
    IN param_code varchar(50),
    IN param_description varchar(50),
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
    , (CASE WHEN param_is_export = 0 THEN CONCAT(' LIMIT ', local_param_rows) ELSE '' END)
	, ';');

	-- Create the second statement
	SET @select_query = CONCAT('SELECT * FROM (SELECT *, @row_num:= @row_num + 1 `row_num` FROM temp_workflow) a ',
		(CASE WHEN param_is_export = 0 THEN CONCAT(' WHERE row_num BETWEEN ', row_start, ' AND ', local_param_rows) ELSE '' END), ';');
    
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
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_next_workflow_step` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_next_workflow_step`(
	IN param_workflow_code VARCHAR(100)
    , IN param_current_step_code VARCHAR(100)
    , IN param_role_id_delimited VARCHAR(500)
)
BEGIN
	
    DECLARE next_order INT;
    
    SET next_order = 
		(SELECT `order` + 1 FROM workflow w INNER JOIN workflow_step ws ON w.id = ws.workflow_id 
        WHERE w.code = param_workflow_code AND ws.code = param_current_step_code);
    
	SELECT
		ws.id
        , ws.code `code`
        , ws.description `description`
        , ws.order
        , w.code `workflow_code`
	FROM workflow w
    INNER JOIN workflow_step ws ON w.id = ws.workflow_id
    INNER JOIN (
		SELECT DISTINCT workflow_id, step_code
        FROM workflow_step_approver
        WHERE FIND_IN_SET(role_id, param_role_id_delimited)
    ) wsa ON ws.code = wsa.step_code AND w.id = wsa.workflow_id
    WHERE w.code = param_workflow_code 
    AND (ws.order = next_order OR ws.order = 0 /* include zero `order` for cancelled */ ); 
    
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_roles_by_workflow_step_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_roles_by_workflow_step_code`(
	IN param_workflow_code VARCHAR(50),
    IN param_step_code VARCHAR(50)
)
BEGIN
	SELECT DISTINCT
		wfsa.id
        , wf.id `workflow_id`
        , wf.code `workflow_code`
        , wfs_cur.code `step_code`
        , wfsa.role_id
    FROM workflow wf
     INNER JOIN workflow_step wfs ON wf.id = wfs.workflow_id AND param_step_code = wfs.code
    LEFT JOIN workflow_step wfs_cur ON wf.id = wfs_cur.workflow_id
		AND (
				(wfs.order + 1 = wfs_cur.order AND wfs.code = param_step_code )
				OR
				(param_step_code = 'FINALIZED' AND wfs_cur.code = 'FINALIZED')
            )
		
    INNER JOIN workflow_step_approver wfsa ON wfs_cur.code = wfsa.step_code AND wf.id = wfsa.workflow_id
    WHERE wf.code = param_workflow_code;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_transaction_by_record_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_transaction_by_record_id`(
	IN param_workflow_id VARCHAR(50),
    IN param_record_id INT
)
BEGIN
    
    SELECT
		wfs.order
		, wf.code `workflow_code`
        , wfs.code `step_code`
		, wfs.code `step`
        , rv.description `status`
		, wfs.code`status_code`
		, IFNULL(DATE_FORMAT(e.end_date_time, '%m/%d/%Y %r'),'') `timestamp`
		, IFNULL(e.approver_remarks,'') `remarks`
        , wfs.result_type
        , IFNULL(DATE_FORMAT(e.date_scheduled, '%m/%d/%Y'),'') `date_scheduled`
        , IFNULL(DATE_FORMAT(e.date_completed, '%m/%d/%Y'),'') `date_completed`
        , e.end_date_time `start_date_time`
    FROM workflow wf 
	INNER JOIN workflow_step wfs ON wf.id = wfs.workflow_id
		AND wf.id = param_workflow_id
	INNER JOIN workflow_transaction e ON 
		e.record_id = param_record_id
		AND e.workflow_code = wf.code
		AND wfs.order = e.step_order
	LEFT JOIN (
		SELECT DISTINCT value, description FROM reference_value
        WHERE ref_code IN (select value from reference_value WHERE ref_code = 'RESULT_TYPE')
	) rv ON e.result = rv.value
	ORDER BY e.end_date_time DESC;
    
    -- SET @is_negative = 0;
--     SELECT 
-- 		wfs.order
-- 		, wf.code `workflow_code`
--         , wfs.code `step_code`
-- 		, wfs.description `step`
--         , @is_negative := 
-- 			CASE WHEN d.result IN (SELECT value FROM reference_value WHERE ref_code = 'NEGATIVE_RESULT_TYPE') /* negative scenarios */
-- 				THEN 1
--             WHEN @is_negative = 1
-- 				THEN @is_negative
-- 			ELSE 0
-- 		END
-- 		,	CASE WHEN d.step_order IS NULL
-- 				THEN 
-- 					CASE WHEN @is_negative = 1
-- 						THEN 'N/A'
-- 					WHEN e.step_order IS NULL AND wfs.order > 1
-- 						THEN 'Pending'
-- 					ELSE 'In-Progress'
-- 					END
-- 				ELSE d.result_description
-- 			END `status`
-- 		, d.result_code `status_code`
-- 		, IFNULL(DATE_FORMAT(d.end_date_time, '%m/%d/%Y %r'),'') `timestamp`
-- 		, IFNULL(d.approver_remarks,'') `remarks`
--         , wfs.result_type
--         , IFNULL(DATE_FORMAT(d.date_scheduled, '%m/%d/%Y'),'') `date_scheduled`
--         , IFNULL(DATE_FORMAT(d.date_completed, '%m/%d/%Y'),'') `date_completed`
--         , e.end_date_time `start_date_time`
-- 	from workflow wf 
-- 	INNER JOIN workflow_step wfs ON wf.id = wfs.workflow_id
-- 		AND wf.id = param_workflow_id
-- 	LEFT JOIN 
-- 	(
-- 		SELECT 
-- 			DISTINCT
--             rv.value `result_code`
-- 			, rv.description `result_description`
--             , wft.*
--         FROM workflow_transaction wft
--         INNER JOIN reference_value rv ON wft.result = rv.value
-- 			AND rv.ref_code IN (select value from reference_value WHERE ref_code = 'RESULT_TYPE')
--         INNER JOIN workflow wft_wf ON wft.workflow_code = wft_wf.code
-- 			AND wft_wf.id = param_workflow_id
--     ) d ON 
-- 		d.record_id = param_record_id
-- 		AND d.workflow_code = wf.code
-- 		AND wfs.order = d.step_order
-- 	LEFT JOIN workflow_transaction e ON 
-- 		e.record_id = param_record_id
-- 		AND e.workflow_code = wf.code
-- 		AND wfs.order = e.step_order + 1
-- 	ORDER BY wfs.order;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_workflow_get_workflow_step_by_role` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_get_workflow_step_by_role`(
	IN param_workflow_code VARCHAR(100)
    , IN param_role_id_delimited VARCHAR(500)
)
BEGIN

	SELECT
		ws.id
        , ws.code `code`
        , ws.description `description`
        , ws.order
        , w.code `workflow_code`
	FROM workflow w
    INNER JOIN workflow_step ws ON w.id = ws.workflow_id
    INNER JOIN (
		SELECT DISTINCT workflow_id, step_code
        FROM workflow_step_approver
        WHERE FIND_IN_SET(role_id, param_role_id_delimited)
    ) wsa ON ws.code = wsa.step_code AND w.id = wsa.workflow_id
    WHERE w.code = 'IPM';
    
    
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
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_workflow_step_autocomplete`(
	IN term VARCHAR(255),
    IN top_Results INT,
    IN workflow_code VARCHAR(255)
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
    SELECT DISTINCT WS.id
		, WS.workflow_id
        , INSTR(WS.code, ', local_term, ') `index`
		, WS.code
		, WS.description
		, WS.previous_step_code
		, WS.is_required
		, WS.tat_days
		, WS.allow_backflow
        , WS.send_email_to_requester
        , WS.send_email_to_approver
		, WS.result_type
		, WS.`order`
	FROM workflow_step WS
    INNER JOIN workflow W ON WS.workflow_id = W.id
	WHERE LOWER(WS.code) LIKE CONCAT(''%'',LOWER(', local_term, '),''%'')
    AND 
		(
			W.code = ''', CONVERT(workflow_code, CHAR(255)),'''
		)
	ORDER BY `index`
		, WS.code ',
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
-- Final view structure for view `tv_accountability`
--

/*!50001 DROP VIEW IF EXISTS `tv_accountability`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_accountability` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `preload_name`,'' AS `date_created` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_log_activity`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_log_activity` AS select 0 AS `id`,'' AS `type`,'' AS `sub_type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `assigned_user_id`,0 AS `assigned_org_group_id`,'' AS `is_pass` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_log_activity_list`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity_list`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_log_activity_list` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `type`,'' AS `sub_type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `created_by`,0 AS `applicant_id`,0 AS `assigned_user_id`,'' AS `remarks`,'' AS `is_assignment`,'' AS `due_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_applicant_log_activity_status_history`
--

/*!50001 DROP VIEW IF EXISTS `tv_applicant_log_activity_status_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_applicant_log_activity_status_history` AS select 0 AS `id`,'' AS `remarks`,'' AS `status`,'' AS `timestamp`,0 AS `user_id`,'' AS `is_pass` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_assigned_activities`
--

/*!50001 DROP VIEW IF EXISTS `tv_assigned_activities`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_assigned_activities` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `type`,'' AS `sub_type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `created_by`,0 AS `applicant_id`,0 AS `employee_id`,0 AS `assigned_org_group_id`,'' AS `current_status_code` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_checklist`
--

/*!50001 DROP VIEW IF EXISTS `tv_checklist`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_checklist` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `type`,'' AS `sub_type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `created_by`,0 AS `employee_id`,0 AS `assigned_user_id`,'' AS `remarks`,'' AS `is_assignment`,'' AS `due_date` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_current_workflow_step`
--

/*!50001 DROP VIEW IF EXISTS `tv_current_workflow_step`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_current_workflow_step` AS select '' AS `step_code`,'' AS `step_description`,'' AS `approver_role_ids`,'' AS `workflow_status` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_employee_accountability`
--

/*!50001 DROP VIEW IF EXISTS `tv_employee_accountability`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_employee_accountability` AS select 0 AS `id`,'' AS `type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `created_by`,0 AS `org_group_id`,0 AS `modified_by`,'' AS `remarks` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_employee_accountability_status_history`
--

/*!50001 DROP VIEW IF EXISTS `tv_employee_accountability_status_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_employee_accountability_status_history` AS select 0 AS `id`,'' AS `remarks`,'' AS `status`,'' AS `timestamp`,0 AS `user_id` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_employee_log_activity`
--

/*!50001 DROP VIEW IF EXISTS `tv_employee_log_activity`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_employee_log_activity` AS select 0 AS `id`,'' AS `type`,'' AS `sub_type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `assigned_user_id`,0 AS `assigned_org_group_id`,'' AS `is_pass` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_employee_log_activity_status_history`
--

/*!50001 DROP VIEW IF EXISTS `tv_employee_log_activity_status_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_employee_log_activity_status_history` AS select 0 AS `id`,'' AS `remarks`,'' AS `status`,'' AS `timestamp`,0 AS `user_id`,'' AS `is_pass` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_log_activity`
--

/*!50001 DROP VIEW IF EXISTS `tv_log_activity`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_log_activity` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `module`,'' AS `type`,'' AS `title`,'' AS `description`,'' AS `is_pass_fail`,'' AS `is_assignment`,'' AS `is_preloaded` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_log_activity_preloaded`
--

/*!50001 DROP VIEW IF EXISTS `tv_log_activity_preloaded`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_log_activity_preloaded` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `preload_name`,'' AS `date_created` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_my_accountabilities`
--

/*!50001 DROP VIEW IF EXISTS `tv_my_accountabilities`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_my_accountabilities` AS select 0 AS `total_num`,0 AS `row_num`,0 AS `id`,'' AS `type`,'' AS `title`,'' AS `description`,'' AS `current_status`,'' AS `current_timestamp`,0 AS `created_by`,0 AS `employee_id`,0 AS `org_group_id`,0 AS `modified_by` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_transaction`
--

/*!50001 DROP VIEW IF EXISTS `tv_transaction`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_transaction` AS select 0 AS `order`,'' AS `workflow_code`,'' AS `step_code`,'' AS `step`,'' AS `status`,'' AS `date_scheduled`,'' AS `date_completed`,'' AS `timestamp`,'' AS `remarks`,'' AS `result_type`,'' AS `start_date_time` from DUAL  where (1 = 0) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `tv_workflow_get_next_workflow_step`
--

/*!50001 DROP VIEW IF EXISTS `tv_workflow_get_next_workflow_step`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `tv_workflow_get_next_workflow_step` AS select 0 AS `id`,'' AS `code`,'' AS `description`,'' AS `order`,'' AS `workflow_code` from DUAL  where (1 = 0) */;
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

-- Dump completed on 2021-04-06 17:59:54
