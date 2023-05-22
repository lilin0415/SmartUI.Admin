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
-- Definition of tenant
-- 

DROP TABLE IF EXISTS `tenant`;
CREATE TABLE IF NOT EXISTS `tenant` (
  `Id` bigint(20) NOT NULL,
  `Code` varchar(30) NOT NULL,
  `Name` varchar(100) NOT NULL COMMENT '名称',
  `ShortName` varchar(50) NOT NULL COMMENT '间称',
  `mnemonic` varchar(20) NOT NULL COMMENT '助记码',
  `Contact` varchar(20) NOT NULL COMMENT '联系人',
  `Phone` varchar(20) NOT NULL COMMENT '电话',
  `QQ` varchar(20) NOT NULL,
  `Email` varchar(20) NOT NULL,
  `PostCode` varchar(10) NOT NULL,
  `Addr` varchar(100) NOT NULL,
  `taxpayer_no` varchar(20) NOT NULL COMMENT '纳税人识别号',
  `tax_rate` decimal(10,4) NOT NULL COMMENT '增值税税率',
  `Type` tinyint(4) NOT NULL,
  `opening_bank` varchar(100) NOT NULL COMMENT '开户行',
  `bank_acnt` varchar(20) NOT NULL COMMENT '银行账号',
  `biz_user_Id` bigint(20) NOT NULL COMMENT '业务人员',
  `Remark` varchar(80) NOT NULL,
  `SortNum` int(11) NOT NULL,
  `IsEnable` tinyint(4) NOT NULL COMMENT '可用状态',
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `BaseIsDelete` tinyint(4) NOT NULL,
  `VisibleScope` tinyint(4) NOT NULL,
  `AllowJoinType` tinyint(4) NOT NULL,
  `DefaultDepartmentId` bigint(20) NOT NULL,
  `DefaultRoleIds` varchar(100) NOT NULL,
  `DefaultPositionIds` varchar(100) NOT NULL,
  `TenantCreatorRoleId` bigint(20) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 
-- Dumping data for table tenant
-- 

/*!40000 ALTER TABLE `tenant` DISABLE KEYS */;
INSERT INTO `tenant`(`Id`,`Code`,`Name`,`ShortName`,`mnemonic`,`Contact`,`Phone`,`QQ`,`Email`,`PostCode`,`Addr`,`taxpayer_no`,`tax_rate`,`Type`,`opening_bank`,`bank_acnt`,`biz_user_Id`,`Remark`,`SortNum`,`IsEnable`,`BaseCreateTime`,`BaseCreatorId`,`BaseModifyTime`,`BaseModifierId`,`BaseVersion`,`BaseIsDelete`,`VisibleScope`,`AllowJoinType`,`DefaultDepartmentId`,`DefaultRoleIds`,`DefaultPositionIds`,`TenantCreatorRoleId`) VALUES(505502463793565696,'SmartUI','系统组织','','','','','','','','','',0.0000,1,'','',0,'',0,0,'2022-10-26 22:06:19.000',16508640061130151,'2022-11-23 10:11:17.000',16508640061130151,0,0,0,0,0,'16508640061130147','',0),(506793007484243968,'test2','xxx','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-10-30 11:34:29.000',16508640061130151,'2022-11-14 23:26:24.000',16508640061130151,0,0,0,0,0,'','',0),(508038260291801088,'DefaultTenant','默认组织','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-02 22:02:40.000',16508640061130151,'2022-11-14 23:40:39.000',16508640061130151,0,0,1,1,508038260300189696,'508038262263123968','',0),(508427058527866880,'aaa','aaa','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-03 23:47:37.000',16508640061130151,'2022-11-03 23:47:37.000',16508640061130151,0,0,0,0,0,'','',0),(508429025765494784,'ddd','ddd','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-03 23:55:26.000',16508640061130151,'2022-11-03 23:55:26.000',16508640061130151,0,0,0,0,0,'','',0),(515100261337796609,'aaabbb','test组织','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-22 09:44:32.000',514589655711092736,'2022-11-26 09:33:29.000',514589655711092736,0,0,1,1,515100261337796608,'515100262055022592','',0),(516362434676527104,'aaabbb','aaa','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-25 21:19:58.000',514589655711092736,'2022-11-25 21:19:58.000',514589655711092736,0,0,1,1,516362434676527105,'516362434676527106','',0),(516377532681949184,'aaabbb2','aaa','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-25 22:19:58.000',514589655711092736,'2022-11-26 09:41:41.000',514589655711092736,0,0,1,1,516377532681949185,'516377532681949186','',0),(516377836349558784,'8888','2222','','','','','','','','','',0.0000,0,'','',0,'',0,0,'2022-11-25 22:21:10.000',514589655711092736,'2022-11-26 10:16:41.000',16508640061130151,0,0,1,1,516377836349558785,'516377836349558786','',0);
/*!40000 ALTER TABLE `tenant` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:49
-- Total time: 0:0:0:0:160 (d:h:m:s:ms)
