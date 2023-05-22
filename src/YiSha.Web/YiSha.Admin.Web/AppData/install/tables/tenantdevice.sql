﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-23 06:56:23
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
-- Definition of tenantdevice
-- 

DROP TABLE IF EXISTS `tenantdevice`;
CREATE TABLE IF NOT EXISTS `tenantdevice` (
  `TenantId` bigint(20) NOT NULL,
  `Id` bigint(20) NOT NULL,
  `AppToken` char(120) NOT NULL COMMENT '应用类型',
  `DeviceGuid` char(60) NOT NULL COMMENT '产品',
  `UserId` bigint(20) NOT NULL COMMENT '联系人',
  `LastActiveTime` datetime(3) NOT NULL,
  `AppVersion` varchar(50) NOT NULL,
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
-- Dumping data for table tenantdevice
-- 

/*!40000 ALTER TABLE `tenantdevice` DISABLE KEYS */;
INSERT INTO `tenantdevice`(`TenantId`,`Id`,`AppToken`,`DeviceGuid`,`UserId`,`LastActiveTime`,`AppVersion`,`Remark`,`SortNum`,`IsEnable`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`) VALUES(505502463793565696,500409662949036032,'a','a',0,'1970-01-01 00:00:00.000','','',0,0,'2022-10-12 20:49:21.000',16508640061130151,'2022-10-13 21:00:57.000',16508640061130151,0,0),(505502463793565696,500443391000055808,'b','b',0,'1970-01-01 00:00:00.000','','',0,0,'2022-10-12 23:03:22.000',16508640061130151,'2022-10-12 23:03:22.000',16508640061130151,0,0),(505502463793565696,501497959909695488,'16508640061130151__602EFAC5CCDDC99BD1DB8E0C6A29949D','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-10-26 23:41:48.000','1.0.1.20303','',0,0,'2022-10-15 20:53:51.000',0,'2022-10-26 23:41:48.000',0,0,0),(505502463793565696,511565074289139712,'BuyFlezLUGD7+HBGueRKQmBCAwTMylBRIwfAOrjF7TCRVCf1ciGaJFBJRA+v9BTtpdrle/poUTo5WtGz8ZKs85zvdE7/7bQn2','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-12 15:37:56.000','1.0.1.20303','',0,1,'2022-11-12 15:36:58.000',16508640061130151,'2022-11-12 15:37:56.000',16508640061130151,0,0),(505502463793565696,511584936361205760,'BX2muEHenfHyj7XAMc9sOv97KOd9aT2Ejx5hbqe41ReOoDnEqCcafBPKy/gOcRHHMFzRpsnYEBehUKNafO0NL7yQJ3PGF+rNdXHCNGXBVeGc=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-13 17:37:21.000','1.0.1.20303','',0,1,'2022-11-12 16:55:54.000',16508640061130151,'2022-11-13 17:37:24.000',16508640061130151,0,0),(505502463793565696,511959054789124096,'BX2muEHenfHyj7XAMc9sOv97KOd9aT2Ejx5hbqe41ReOoDnEqCcafBPKy/gOcRHHMFzRpsnYEBehUKNafO0NL7yQJ3PGF+rNdkqvgQkp9DMI=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-20 23:01:04.000','1.0.1.20303','',0,1,'2022-11-13 17:42:30.000',16508640061130151,'2022-11-20 23:01:04.000',16508640061130151,0,0),(508038260291801088,512400034344476672,'BR6PbjC3CsGSH8hW1VeNNh+c24lkWCHkYi1HeqQmQwuRiihaWPMSundq4fNHm+ay0VFYnzuHuRVfEG/EYRpKcgGrqgSvbf1icgj0HWDSsvw4=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-20 10:21:01.000','1.0.1.20303','',0,1,'2022-11-14 22:54:48.000',16508640061130151,'2022-11-20 10:21:01.000',16508640061130151,0,0),(508427058527866880,514214171739426816,'BT83reDM+jpZ8gAkbg6Eyq18apSJvsPfkSEJmUC+dePky3tYwQ3HdkKHNgZMevWJxNkY9LqyXkE00a39fGk0yzN2hIO3NT6GngENDB+HCxKk=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-20 10:15:37.000','1.0.1.20303','',0,1,'2022-11-19 23:03:32.000',16508640061130151,'2022-11-20 10:15:37.000',16508640061130151,0,0),(508429025765494784,514217802882617344,'BT83reDM+jpaF+ThrJKzG2957EOgqEpUwzAmzhHe22AZeNZSnrmZA69EccEmHkh/3Irvw33Q7a8Xb4qh6JTXusD5Ynp3lbVRr7H0q71AM+3Y=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-19 23:17:58.000','1.0.1.20303','',0,1,'2022-11-19 23:17:58.000',16508640061130151,'2022-11-19 23:17:58.000',16508640061130151,0,0),(506793007484243968,514225061075357696,'B6qggnGeWRAGPL1HMm6gNPcarKytNq03xuTjadlMixh1J7BHRdAGxJgFIK6+SAxZ2S8qBmdt3Du4+1IOepu+pQyZG88/pXnaah9aNBDF3Vk0=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-20 10:16:12.000','1.0.1.20303','',0,1,'2022-11-19 23:46:48.000',16508640061130151,'2022-11-20 10:16:12.000',16508640061130151,0,0),(505502463793565696,516630465705152512,'BX2muEHenfHyj7XAMc9sOv97KOd9aT2Ejx5hbqe41ReOoDnEqCcafBPKy/gOcRHHMFzRpsnYEBehUKNafO0NL7yQJ3PGF+rNdaOUPyiOPQVw=','602EFAC5CCDDC99BD1DB8E0C6A29949D',16508640061130151,'2022-11-26 15:05:02.000','1.0.1.20302','',0,1,'2022-11-26 15:05:02.000',16508640061130151,'2022-11-26 15:05:02.000',16508640061130151,0,0);
/*!40000 ALTER TABLE `tenantdevice` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:23
-- Total time: 0:0:0:0:90 (d:h:m:s:ms)
