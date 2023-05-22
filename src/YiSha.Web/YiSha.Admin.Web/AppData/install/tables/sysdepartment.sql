﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:47
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
-- Definition of sysdepartment
-- 

DROP TABLE IF EXISTS `sysdepartment`;
CREATE TABLE IF NOT EXISTS `sysdepartment` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `ParentId` bigint(20) NOT NULL COMMENT '父部门Id(0表示是根部门)',
  `DepartmentName` varchar(50) NOT NULL COMMENT '部门名称',
  `Telephone` varchar(50) NOT NULL COMMENT '部门电话',
  `Fax` varchar(50) NOT NULL COMMENT '部门传真',
  `Email` varchar(50) NOT NULL COMMENT '部门Email',
  `PrincipalId` bigint(20) NOT NULL COMMENT '部门负责人Id',
  `DepartmentSort` int(11) NOT NULL COMMENT '部门排序',
  `Remark` text NOT NULL COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='部门表';

-- 
-- Dumping data for table sysdepartment
-- 

/*!40000 ALTER TABLE `sysdepartment` DISABLE KEYS */;
INSERT INTO `sysdepartment`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`ParentId`,`DepartmentName`,`Telephone`,`Fax`,`Email`,`PrincipalId`,`DepartmentSort`,`Remark`) VALUES(16508640061124402,0,'2019-01-04 17:38:23.000','2023-03-11 21:24:21.000',0,16508640061130151,0,0,'北京总公司','010-6666666','010-8888888','',16508640061130152,1,''),(16508640061124403,1,'2018-10-10 01:00:00.000','2023-03-11 21:24:41.000',0,16508640061130151,0,16508640061124402,'天津分公司','1','','',16508640061130150,1,''),(16508640061124404,1,'2018-10-10 01:00:00.000','2023-03-11 21:24:52.000',0,16508640061130151,0,16508640061124402,'保定分公司','1','','',0,2,''),(16508640061124405,1,'2018-10-10 01:00:00.000','2019-01-01 21:33:24.000',0,0,0,16508640061124403,'研发部','1','','',0,1,'专注前端与后端结合的开发模式'),(16508640061124406,1,'2018-10-10 01:00:00.000','2019-01-04 17:35:59.000',0,0,0,16508640061124403,'测试部','1','','',0,3,''),(16508640061124407,1,'2018-10-10 01:00:00.000','2018-12-26 17:47:37.000',0,0,0,16508640061124403,'前端设计部','1','','',0,2,''),(16508640061124408,0,'2018-12-22 18:36:58.000','2018-12-26 17:48:32.000',0,0,0,16508640061124403,'财务部','0551-87654321','0551-12345678','wangxue@yishasoft.com',0,15,'2'),(16508640061124409,0,'2018-12-22 18:47:42.000','2018-12-26 17:48:15.000',0,0,0,16508640061124403,'市场部','','','',0,7,''),(16508640061124410,0,'2018-12-22 18:48:02.000','2018-12-26 17:48:25.000',0,0,0,16508640061124403,'行政部','','','',0,10,'');
/*!40000 ALTER TABLE `sysdepartment` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:47
-- Total time: 0:0:0:0:115 (d:h:m:s:ms)
