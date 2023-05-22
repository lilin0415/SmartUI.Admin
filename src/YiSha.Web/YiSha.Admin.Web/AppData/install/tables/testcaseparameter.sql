-- MySqlBackup.NET 2.3.8.0
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
-- Definition of testcaseparameter
-- 

DROP TABLE IF EXISTS `testcaseparameter`;
CREATE TABLE IF NOT EXISTS `testcaseparameter` (
  `Id` bigint(20) NOT NULL,
  `CaseId` bigint(20) NOT NULL COMMENT '产品',
  `ProjectGuid` char(37) NOT NULL,
  `ProjectVersion` varchar(30) NOT NULL,
  `DocumentGuid` char(37) NOT NULL COMMENT '项目',
  `DocumentType` tinyint(4) NOT NULL,
  `VarName` varchar(40) NOT NULL COMMENT '编码',
  `Value` varchar(200) NOT NULL COMMENT '名称',
  `UseInheritedValue` tinyint(2) NOT NULL,
  `SortNum` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table testcaseparameter
-- 

/*!40000 ALTER TABLE `testcaseparameter` DISABLE KEYS */;

/*!40000 ALTER TABLE `testcaseparameter` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:23
-- Total time: 0:0:0:0:89 (d:h:m:s:ms)
