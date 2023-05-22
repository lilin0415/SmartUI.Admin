-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:49
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
-- Definition of testcasegroup
-- 

DROP TABLE IF EXISTS `testcasegroup`;
CREATE TABLE IF NOT EXISTS `testcasegroup` (
  `Id` bigint(20) NOT NULL,
  `EnvId` bigint(20) NOT NULL COMMENT '产品',
  `Code` varchar(120) NOT NULL COMMENT '功能模块',
  `Name` varchar(250) NOT NULL COMMENT '项目',
  `CaseIds` varchar(500) NOT NULL COMMENT '编码',
  `IsContinueWhenError` tinyint(2) NOT NULL COMMENT '名称',
  `IsParallelMode` tinyint(2) NOT NULL,
  `PrevStartTime` datetime(3) NOT NULL,
  `PrevEndTime` datetime(3) NOT NULL,
  `PrevFinishStatus` tinyint(4) NOT NULL,
  `PrevReason` varchar(500) NOT NULL,
  `ExecStatus` tinyint(4) NOT NULL,
  `StartTime` datetime(3) NOT NULL,
  `EndTime` datetime(3) NOT NULL,
  `FinishStatus` tinyint(4) NOT NULL,
  `Reason` varchar(500) NOT NULL,
  `IsEnable` tinyint(4) NOT NULL COMMENT '可用状态',
  `Remark` varchar(200) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table testcasegroup
-- 

/*!40000 ALTER TABLE `testcasegroup` DISABLE KEYS */;

/*!40000 ALTER TABLE `testcasegroup` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:49
-- Total time: 0:0:0:0:125 (d:h:m:s:ms)
