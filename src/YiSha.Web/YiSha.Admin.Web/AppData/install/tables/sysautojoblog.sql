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
-- Definition of sysautojoblog
-- 

DROP TABLE IF EXISTS `sysautojoblog`;
CREATE TABLE IF NOT EXISTS `sysautojoblog` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `JobGroupName` varchar(50) NOT NULL COMMENT '任务组名称',
  `JobName` varchar(50) NOT NULL COMMENT '任务名称',
  `LogStatus` int(11) NOT NULL COMMENT '执行状态(0失败 1成功)',
  `Remark` text NOT NULL COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户所属表';

-- 
-- Dumping data for table sysautojoblog
-- 

/*!40000 ALTER TABLE `sysautojoblog` DISABLE KEYS */;

/*!40000 ALTER TABLE `sysautojoblog` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:21
-- Total time: 0:0:0:0:83 (d:h:m:s:ms)
