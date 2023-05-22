﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:44
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
-- Definition of deploytask
-- 

DROP TABLE IF EXISTS `deploytask`;
CREATE TABLE IF NOT EXISTS `deploytask` (
  `Id` bigint(20) NOT NULL,
  `TaskId` bigint(20) NOT NULL,
  `AppToken` char(120) NOT NULL,
  `DeviceGuid` char(50) NOT NULL,
  `UserId` bigint(20) NOT NULL COMMENT '项目',
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table deploytask
-- 

/*!40000 ALTER TABLE `deploytask` DISABLE KEYS */;
INSERT INTO `deploytask`(`Id`,`TaskId`,`AppToken`,`DeviceGuid`,`UserId`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`) VALUES(500449306654806016,497453087103914000,'a','a',0,'2022-10-12 23:26:53.000',16508640061130151,'2022-10-12 23:26:53.000',16508640061130151,0,0),(500449307049070592,497453087103914000,'b','b',0,'2022-10-12 23:26:53.000',16508640061130151,'2022-10-12 23:26:53.000',16508640061130151,0,0),(500452733036269568,497453087103913984,'b','b',0,'2022-10-12 23:40:30.000',16508640061130151,'2022-10-12 23:40:30.000',16508640061130151,0,0),(510361777037381632,503864221755248640,'16508640061130151__602EFAC5CCDDC99BD1DB8E0C6A29949D','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-09 07:55:30.000',16508640061130151,'2022-11-09 07:55:30.000',16508640061130151,0,0);
/*!40000 ALTER TABLE `deploytask` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:44
-- Total time: 0:0:0:0:461 (d:h:m:s:ms)