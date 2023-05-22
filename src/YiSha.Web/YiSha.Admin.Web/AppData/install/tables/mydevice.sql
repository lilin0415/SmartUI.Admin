﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:45
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
-- Definition of mydevice
-- 

DROP TABLE IF EXISTS `mydevice`;
CREATE TABLE IF NOT EXISTS `mydevice` (
  `Id` bigint(20) NOT NULL,
  `Guid` char(60) NOT NULL COMMENT '产品',
  `Name` varchar(120) NOT NULL COMMENT '项目',
  `IP` varchar(60) NOT NULL,
  `MAC` varchar(60) NOT NULL COMMENT '编码',
  `LoginName` varchar(60) NOT NULL COMMENT '名称',
  `UserId` bigint(20) NOT NULL COMMENT '联系人',
  `UserName` varchar(60) NOT NULL,
  `UserLoginTime` datetime(3) NOT NULL,
  `LastActiveTime` datetime(3) NOT NULL,
  `Remark` text NOT NULL,
  `SortNum` int(11) NOT NULL,
  `IsEnable` tinyint(4) NOT NULL COMMENT '可用状态',
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `AppToken` varchar(120) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table mydevice
-- 

/*!40000 ALTER TABLE `mydevice` DISABLE KEYS */;
INSERT INTO `mydevice`(`Id`,`Guid`,`Name`,`IP`,`MAC`,`LoginName`,`UserId`,`UserName`,`UserLoginTime`,`LastActiveTime`,`Remark`,`SortNum`,`IsEnable`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`,`AppToken`) VALUES(500409662949036032,'a','my pc1','192.168.0.1','','aaa',0,'aaaa','1970-01-01 00:00:00.000','1970-01-01 00:00:00.000','',0,0,'2022-10-12 20:49:21.000',16508640061130151,'2022-10-13 21:00:57.000',16508640061130151,0,0,''),(500443391000055808,'b','b','b','b','b',0,'','1970-01-01 00:00:00.000','1970-01-01 00:00:00.000','',0,0,'2022-10-12 23:03:22.000',16508640061130151,'2022-10-12 23:03:22.000',16508640061130151,0,0,''),(501497959909695488,'602EFAC5CCDDC99BD1DB8E0C6A29949D','DESKTOP-IFJM3BN','192.168.56.1','00:71:CC:94:24:E5,68:F7:28:9C:A5:C1','STS',16508640061130151,'admin','2023-03-05 22:57:38.000','2023-03-05 22:57:38.000','',0,0,'2022-10-15 20:53:51.000',0,'2023-03-05 22:57:38.000',0,0,0,'BnMTbPpEJNVsLv2LKKBvFOho5TfrCYS0SU0r+Ka9bwaWF+zMpAli1MEtnExv0bue9raSBjQ8A69g6aBUbOn/R8w=='),(511501296675594240,'602EFAC5CCDDC99BD1DB8E0C6A29949D','DESKTOP-IFJM3BN','192.168.56.1','00:71:CC:94:24:E5,68:F7:28:9C:A5:C1','STS',508047220369526784,'admin1','2022-11-12 15:06:16.000','2022-11-12 15:06:16.000','',0,0,'2022-11-12 11:23:32.000',0,'2022-11-12 15:06:16.000',0,0,0,'');
/*!40000 ALTER TABLE `mydevice` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:45
-- Total time: 0:0:0:0:205 (d:h:m:s:ms)
