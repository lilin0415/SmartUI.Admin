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
-- Definition of sysautojob
-- 

DROP TABLE IF EXISTS `sysautojob`;
CREATE TABLE IF NOT EXISTS `sysautojob` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `JobGroupName` varchar(50) NOT NULL COMMENT '任务组名称',
  `JobName` varchar(50) NOT NULL COMMENT '任务名称',
  `JobStatus` int(11) NOT NULL COMMENT '任务状态(0禁用 1启用)',
  `CronExpression` varchar(50) NOT NULL COMMENT 'cron表达式',
  `StartTime` datetime(3) NOT NULL COMMENT '运行开始时间',
  `EndTime` datetime(3) NOT NULL COMMENT '运行结束时间',
  `NextStartTime` datetime(3) NOT NULL COMMENT '下次执行时间',
  `Remark` text NOT NULL COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='定时任务表';

-- 
-- Dumping data for table sysautojob
-- 

/*!40000 ALTER TABLE `sysautojob` DISABLE KEYS */;
INSERT INTO `sysautojob`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`JobGroupName`,`JobName`,`JobStatus`,`CronExpression`,`StartTime`,`EndTime`,`NextStartTime`,`Remark`) VALUES(16508640061124370,0,'2019-01-03 09:47:04.000','2023-03-18 16:36:48.000',0,16508640061130151,0,'YiShaAdmin','数据库备份',0,'0 0 3 1/1 * ?','2019-01-03 10:00:00.000','2019-12-31 00:00:00.000','2019-10-14 03:00:00.000','');
/*!40000 ALTER TABLE `sysautojob` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-23 06:56:21
-- Total time: 0:0:0:0:86 (d:h:m:s:ms)
