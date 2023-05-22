-- MySqlBackup.NET 2.3.8.0
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
-- Definition of sysdatadictdetail
-- 

DROP TABLE IF EXISTS `sysdatadictdetail`;
CREATE TABLE IF NOT EXISTS `sysdatadictdetail` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `DictType` varchar(50) NOT NULL COMMENT '字典类型(外键)',
  `DictSort` int(11) NOT NULL COMMENT '字典排序',
  `DictKey` int(11) NOT NULL COMMENT '字典键(一般从1开始)',
  `DictValue` varchar(50) NOT NULL COMMENT '字典值',
  `ListClass` varchar(50) NOT NULL COMMENT '显示样式(default primary success info warning danger)',
  `DictStatus` int(11) NOT NULL COMMENT '字典状态(0禁用 1启用)',
  `IsDefault` int(11) NOT NULL COMMENT '默认选中(0不是 1是)',
  `Remark` varchar(50) NOT NULL COMMENT '备注',
  `IsSystem` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='字典数据表';

-- 
-- Dumping data for table sysdatadictdetail
-- 

/*!40000 ALTER TABLE `sysdatadictdetail` DISABLE KEYS */;
INSERT INTO `sysdatadictdetail`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`DictType`,`DictSort`,`DictKey`,`DictValue`,`ListClass`,`DictStatus`,`IsDefault`,`Remark`,`IsSystem`) VALUES(16508640061124400,0,'2019-01-05 07:16:12.000','2019-10-12 18:21:48.000',0,16508640061130151,0,'NewsType',1,1,'产品案例','primary',1,1,'',0),(16508640061124401,0,'2019-01-05 07:16:25.000','2019-01-05 07:16:25.000',0,0,0,'NewsType',2,2,'行业新闻','warning',1,0,'',0);
/*!40000 ALTER TABLE `sysdatadictdetail` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:47
-- Total time: 0:0:0:0:146 (d:h:m:s:ms)
