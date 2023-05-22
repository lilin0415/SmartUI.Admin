-- MySqlBackup.NET 2.3.8.0
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
-- Definition of sysdatadict
-- 

DROP TABLE IF EXISTS `sysdatadict`;
CREATE TABLE IF NOT EXISTS `sysdatadict` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `DictType` varchar(50) NOT NULL COMMENT '字典类型',
  `DictSort` int(11) NOT NULL COMMENT '字典排序',
  `Remark` varchar(50) NOT NULL COMMENT '备注',
  `IsSystem` tinyint(1) NOT NULL,
  `CanAddItem` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='字典类型表';

-- 
-- Dumping data for table sysdatadict
-- 

/*!40000 ALTER TABLE `sysdatadict` DISABLE KEYS */;
INSERT INTO `sysdatadict`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`DictType`,`DictSort`,`Remark`,`IsSystem`,`CanAddItem`) VALUES(16508640061124399,0,'2019-01-05 07:15:41.000','2019-01-05 09:30:19.000',0,0,0,'NewsType',1,'新闻类别',0,0);
/*!40000 ALTER TABLE `sysdatadict` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:22
-- Total time: 0:0:0:0:107 (d:h:m:s:ms)
