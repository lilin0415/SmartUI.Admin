-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2023-05-21 16:06:48
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
-- Definition of taskexecrecord
-- 

DROP TABLE IF EXISTS `taskexecrecord`;
CREATE TABLE IF NOT EXISTS `taskexecrecord` (
  `Id` bigint(20) NOT NULL,
  `EnvId` bigint(20) NOT NULL,
  `BaseIsDelete` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `TaskId` bigint(20) NOT NULL,
  `Name` varchar(50) NOT NULL COMMENT '任务名称',
  `IsTemp` tinyint(2) NOT NULL COMMENT '临时任务',
  `SourceType` tinyint(4) NOT NULL COMMENT '1：计划自动生成,2:手动点击计划生成的任务',
  `ExecStatus` tinyint(4) NOT NULL COMMENT '执行状态',
  `FinishStatus` tinyint(4) NOT NULL COMMENT '结束状态',
  `Reason` varchar(500) NOT NULL COMMENT '原因',
  `CronExpression` varchar(50) NOT NULL COMMENT 'cron表达式',
  `NextStartTime` datetime(3) NOT NULL,
  `StartTime` datetime(3) NOT NULL COMMENT '运行开始时间',
  `EndTime` datetime(3) NOT NULL COMMENT '运行结束时间',
  `Remark` varchar(500) NOT NULL COMMENT '备注',
  `StopWhenError` tinyint(2) NOT NULL COMMENT '出错停止',
  `IsParallelMode` tinyint(2) NOT NULL COMMENT '并行执行用例',
  `IsEnable` tinyint(24) NOT NULL COMMENT '任务状态(0禁用 1启用)',
  `DeviceDeployMode` tinyint(4) NOT NULL,
  `MultipleInstances` tinyint(4) NOT NULL,
  `TotalCaseCount` int(11) NOT NULL COMMENT '总数量',
  `SucceedCaseCount` int(11) NOT NULL COMMENT '成功的数据',
  `FailedCaseCount` int(11) NOT NULL COMMENT '错误的数据',
  `CancelledCaseCount` int(11) NOT NULL COMMENT '取消的用例数量',
  `ConsumeMode` tinyint(4) NOT NULL,
  `ConsumerId` bigint(20) NOT NULL,
  `Guid` varchar(33) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='定时任务表';


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:48
-- Total time: 0:0:0:0:156 (d:h:m:s:ms)
