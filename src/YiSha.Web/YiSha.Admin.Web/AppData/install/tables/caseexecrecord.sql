-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:43
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
-- Definition of caseexecrecord
-- 

DROP TABLE IF EXISTS `caseexecrecord`;
CREATE TABLE IF NOT EXISTS `caseexecrecord` (
  `Id` bigint(20) NOT NULL,
  `TaskId` bigint(20) NOT NULL COMMENT '应用类型',
  `TaskItemId` bigint(20) NOT NULL COMMENT '产品',
  `TaskExecId` bigint(20) NOT NULL COMMENT '功能模块',
  `GroupId` bigint(20) NOT NULL COMMENT '编码',
  `CaseId` bigint(20) NOT NULL COMMENT '名称',
  `EnvId` bigint(20) NOT NULL,
  `ProjectGuid` varchar(37) NOT NULL COMMENT '联系人',
  `ProjectVersion` char(30) NOT NULL COMMENT '电话',
  `Code` varchar(120) NOT NULL,
  `Name` varchar(250) NOT NULL,
  `StartTime` datetime(3) NOT NULL,
  `EndTime` datetime(3) NOT NULL,
  `ExecStatus` tinyint(4) NOT NULL COMMENT '纳税人识别号',
  `FinishStatus` tinyint(4) NOT NULL COMMENT '增值税税率',
  `Reason` varchar(500) NOT NULL,
  `VarJson` text NOT NULL,
  `Remark` varchar(80) NOT NULL,
  `SortNum` int(11) NOT NULL,
  `IsEnable` tinyint(4) NOT NULL COMMENT '可用状态',
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `ConsumeStatus` tinyint(2) NOT NULL,
  `ConsumedTime` datetime(3) NOT NULL,
  `UserId` bigint(20) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `AppToken` varchar(120) NOT NULL,
  `AppVersion` varchar(30) NOT NULL,
  `DeviceGuid` char(50) NOT NULL,
  `DeviceName` varchar(50) NOT NULL,
  `DeviceIP` varchar(80) NOT NULL,
  `DeviceMAC` varchar(80) NOT NULL,
  `DeviceLoginName` varchar(80) NOT NULL,
  `TotalAssertionCount` int(255) NOT NULL,
  `SucceedAssertionCount` int(255) NOT NULL,
  `FailedAssertionCount` int(255) NOT NULL,
  `SupportParallel` tinyint(2) NOT NULL,
  `Priority` tinyint(4) NOT NULL,
  `TypeId` bigint(20) NOT NULL,
  `LogFilePath` varchar(100) NOT NULL,
  `Guid` varchar(33) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:43
-- Total time: 0:0:0:0:149 (d:h:m:s:ms)
