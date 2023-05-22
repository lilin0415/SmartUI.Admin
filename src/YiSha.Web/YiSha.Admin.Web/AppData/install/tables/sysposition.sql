﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-23 06:56:22
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
-- Definition of sysposition
-- 

DROP TABLE IF EXISTS `sysposition`;
CREATE TABLE IF NOT EXISTS `sysposition` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `PositionName` varchar(50) NOT NULL COMMENT '职位名称',
  `PositionSort` int(11) NOT NULL COMMENT '职位排序',
  `PositionStatus` int(11) NOT NULL COMMENT '职位状态(0禁用 1启用)',
  `Remark` varchar(50) NOT NULL COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='职位表';

-- 
-- Dumping data for table sysposition
-- 

/*!40000 ALTER TABLE `sysposition` DISABLE KEYS */;
INSERT INTO `sysposition`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`PositionName`,`PositionSort`,`PositionStatus`,`Remark`) VALUES(16508640061130139,0,'2018-12-06 09:43:34.000','2019-04-02 17:03:42.000',0,16508640061130151,0,'董事长',1,1,'CEO'),(16508640061130140,0,'2018-12-06 09:47:46.000','2019-10-12 17:29:40.000',0,16508640061130151,0,'总经理',2,1,''),(16508640061130141,0,'2018-12-06 09:47:56.000','2018-12-06 09:47:56.000',0,0,0,'项目经理',3,1,''),(16508640061130142,0,'2018-12-06 09:48:10.000','2018-12-06 09:48:10.000',0,0,0,'测试经理',4,1,''),(16508640061130143,0,'2018-12-06 09:48:18.000','2018-12-31 18:29:57.000',0,0,0,'程序员',5,2,''),(16508640061130144,0,'2018-12-22 11:41:39.000','2018-12-26 16:23:44.000',0,0,0,'前端',6,1,''),(16508640061130145,0,'2018-12-22 18:44:32.000','2018-12-22 18:44:32.000',0,0,0,'财务专员',7,1,'');
/*!40000 ALTER TABLE `sysposition` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:22
-- Total time: 0:0:0:0:90 (d:h:m:s:ms)
