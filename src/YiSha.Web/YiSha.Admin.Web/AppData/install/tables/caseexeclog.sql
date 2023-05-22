-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:43
-- --------------------------------------
-- Server version 5.7.36 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of caseexeclog
-- 

DROP TABLE IF EXISTS `caseexeclog`;
CREATE TABLE IF NOT EXISTS `caseexeclog` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CaseExecId` bigint(20) NOT NULL,
  `LogId` varchar(32) NOT NULL,
  `LogType` tinyint(4) NOT NULL,
  `DateTime` datetime(3) NOT NULL,
  `Message` varchar(500) NOT NULL,
  `Level` tinyint(4) NOT NULL,
  `TransStep` tinyint(4) NOT NULL,
  `ExecutionPathName` varchar(1000) NOT NULL,
  `ExecutionPathId` varchar(500) NOT NULL,
  `ExecutionPathMd5` varchar(32) NOT NULL,
  `ExecutorId` varchar(32) NOT NULL,
  `ExecutorTypeName` varchar(50) NOT NULL,
  `ExecutorName` varchar(50) NOT NULL,
  `LineNumber` int(11) NOT NULL,
  `EndTime` datetime(3) NOT NULL,
  `ElapsedTime` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `Reason` varchar(500) NOT NULL,
  `AssertStatus` tinyint(4) NOT NULL,
  `Assert` varchar(50) NOT NULL,
  `InputParameters` varchar(1000) NOT NULL,
  `OutputParameters` varchar(1000) NOT NULL,
  `BeforeScreenshot` varchar(200) NOT NULL,
  `AfterScreenshot` varchar(200) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=573647517476982785 DEFAULT CHARSET=utf8;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:43
-- Total time: 0:0:0:0:286 (d:h:m:s:ms)
