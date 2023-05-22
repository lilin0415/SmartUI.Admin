﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-23 06:56:20
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
-- Definition of devicegroupdetail
-- 

DROP TABLE IF EXISTS `devicegroupdetail`;
CREATE TABLE IF NOT EXISTS `devicegroupdetail` (
  `Id` bigint(20) NOT NULL,
  `GroupId` bigint(20) NOT NULL,
  `DeviceId` bigint(20) NOT NULL,
  `Remark` text NOT NULL,
  `SortNum` int(11) NOT NULL,
  `IsEnable` tinyint(4) NOT NULL COMMENT '可用状态',
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table devicegroupdetail
-- 

/*!40000 ALTER TABLE `devicegroupdetail` DISABLE KEYS */;
INSERT INTO `devicegroupdetail`(`Id`,`GroupId`,`DeviceId`,`Remark`,`SortNum`,`IsEnable`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`) VALUES(500409662949036032,0,0,'',0,0,'2022-10-12 20:49:21.000',16508640061130151,'2022-10-13 21:00:57.000',16508640061130151,0,0),(500443391000055808,0,0,'',0,0,'2022-10-12 23:03:22.000',16508640061130151,'2022-10-12 23:03:22.000',16508640061130151,0,0),(511501296675594240,0,0,'',0,0,'2022-11-12 11:23:32.000',0,'2022-11-12 15:06:16.000',0,0,0),(569852963778990080,511501296675594240,501497959909695488,'',0,0,'2023-04-22 11:52:14.000',16508640061130151,'2023-04-22 11:52:14.000',16508640061130151,0,0),(569856581445881856,511501296675594240,511501296675594240,'',0,0,'2023-04-22 12:06:36.000',16508640061130151,'2023-04-22 12:06:36.000',16508640061130151,0,0),(571820893920038912,500409662949036032,571594389369917440,'',0,0,'2023-04-27 22:12:05.029',16508640061130151,'2023-04-27 22:12:05.029',16508640061130151,0,0),(571820894469492736,500409662949036032,511501296675594240,'',0,0,'2023-04-27 22:12:05.161',16508640061130151,'2023-04-27 22:12:05.161',16508640061130151,0,0),(571820894469492737,500409662949036032,500409662949036032,'',0,0,'2023-04-27 22:12:05.161',16508640061130151,'2023-04-27 22:12:05.161',16508640061130151,0,0);
/*!40000 ALTER TABLE `devicegroupdetail` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:20
-- Total time: 0:0:0:0:125 (d:h:m:s:ms)
