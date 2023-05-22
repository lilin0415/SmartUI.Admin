﻿-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-23 06:56:21
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
-- Definition of product
-- 

DROP TABLE IF EXISTS `product`;
CREATE TABLE IF NOT EXISTS `product` (
  `Id` bigint(20) NOT NULL,
  `ParentId` bigint(20) NOT NULL COMMENT '用户名',
  `Name` varchar(50) NOT NULL COMMENT '密码',
  `Remark` varchar(100) NOT NULL COMMENT '密码重置token',
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
-- Dumping data for table product
-- 

/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product`(`Id`,`ParentId`,`Name`,`Remark`,`IsEnable`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`) VALUES(493792257187516416,0,'纺织同业分析','',0,'2022-09-24 14:34:08.000',16508640061130151,'2023-04-17 21:13:26.000',16508640061130151,0,0),(514792657864626176,0,'核算检测产品','',0,'2022-11-21 13:22:14.000',514589655711092736,'2022-11-21 22:17:16.000',514589655711092736,0,0),(514929492402114560,0,'销售订单处理','',0,'2022-11-21 22:25:58.000',514929092533948416,'2023-04-17 21:11:59.000',16508640061130151,0,0),(515294997797408768,0,'银企对账查询','',0,'2022-11-22 22:38:21.000',514589655711092736,'2023-04-17 21:11:35.000',16508640061130151,0,0),(541005186424901632,0,'电力总装一级资质申请','电力总装一级资质申请',0,'2023-02-01 21:21:28.000',540997529148329984,'2023-04-17 21:10:30.000',16508640061130151,0,0),(555034769604218880,0,'后台管理系统','',0,'2023-03-12 14:30:01.000',16508640061130151,'2023-03-12 14:30:01.000',16508640061130151,0,0);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:21
-- Total time: 0:0:0:0:100 (d:h:m:s:ms)
