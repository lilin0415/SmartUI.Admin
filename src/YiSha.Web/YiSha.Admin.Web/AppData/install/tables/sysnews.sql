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
-- Definition of sysnews
-- 

DROP TABLE IF EXISTS `sysnews`;
CREATE TABLE IF NOT EXISTS `sysnews` (
  `Id` bigint(20) NOT NULL,
  `BaseIsDelete` int(11) NOT NULL,
  `BaseCreateTime` datetime(3) NOT NULL,
  `BaseModifyTime` datetime(3) NOT NULL,
  `BaseCreatorId` bigint(20) NOT NULL,
  `BaseModifierId` bigint(20) NOT NULL,
  `BaseVersion` int(11) NOT NULL,
  `NewsTitle` varchar(300) NOT NULL COMMENT '新闻标题',
  `NewsContent` longtext NOT NULL COMMENT '新闻内容',
  `NewsTag` varchar(200) NOT NULL COMMENT '新闻标签',
  `ProvinceId` bigint(20) NOT NULL COMMENT '省份Id',
  `CityId` bigint(20) NOT NULL COMMENT '城市Id',
  `CountyId` bigint(20) NOT NULL COMMENT '区县Id',
  `ThumbImage` varchar(200) NOT NULL COMMENT '缩略图',
  `NewsSort` int(11) NOT NULL COMMENT '新闻排序',
  `NewsAuthor` varchar(50) NOT NULL COMMENT '发布者',
  `NewsDate` datetime(3) NOT NULL COMMENT '发布时间',
  `NewsType` int(11) NOT NULL COMMENT '新闻类型(1产品案例 2行业新闻)',
  `ViewTimes` int(11) NOT NULL COMMENT '查看次数',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='新闻表';

-- 
-- Dumping data for table sysnews
-- 

/*!40000 ALTER TABLE `sysnews` DISABLE KEYS */;
INSERT INTO `sysnews`(`Id`,`BaseIsDelete`,`BaseCreateTime`,`BaseModifyTime`,`BaseCreatorId`,`BaseModifierId`,`BaseVersion`,`NewsTitle`,`NewsContent`,`NewsTag`,`ProvinceId`,`CityId`,`CountyId`,`ThumbImage`,`NewsSort`,`NewsAuthor`,`NewsDate`,`NewsType`,`ViewTimes`) VALUES(34571912667467776,0,'2019-04-06 09:36:26.000','2019-10-10 12:22:22.000',16508640061130151,16508640061130151,0,'UHC健康会','<p>UHC 健康会，您的健康管家，为您的健康保驾护航。</p><p>\n        <img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/8722abb613cd46b4af5b4ded7ddf5fad.jpg\" data-filename=\"/\" style=\"width: 550px;\">\n    </p><p>\n        <img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/1ffc4edd922e4cb195744c13f9eec636.jpg\" data-filename=\"/\" style=\"width: 550px;\">\n    </p><p>\n        <img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/e0728828482542f099ab79ba7d3ef701.jpg\" data-filename=\"/\" style=\"width: 550px;\">\n\n    </p><p>\n        <img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/57b1153fbf514d9384ba9837a46737cf.jpg\" data-filename=\"/\" style=\"width: 550px;\">\n        <br>\n    </p>\n    <p>小程序码</p><p>\n        <img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/46ece527595a408e9e62b2334374b560.jpg\" data-filename=\"/\" style=\"width: 430px;\">\n        <br>\n    </p><p><br></p>','微信小程序，健康会',0,0,0,'https://www.yishasoft.com/api/Resource/News/2019/07/31/eee642de4d3443779c0670e0da8eeed7.png',1,'管理员','2019-04-06 09:29:00.000',1,138),(76797547762421760,0,'2019-07-31 22:06:02.000','2019-10-12 17:29:52.000',16508640061130151,16508640061130151,0,'58名师','<p>汇聚同城的各科教师，找老师，就上58名师！</p><p><img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/b0316da26b5546d1bf07fcd05e8889e0.png\" data-filename=\"/\" style=\"width: 270px;\"></p><p><br></p><p><img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/ad812ef0248a4fcb94edfa69d1ee5a66.png\" data-filename=\"/\" style=\"width: 269px;\"></p><p><img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/b2921e1e08e04b0d92b29485356eacb1.png\" data-filename=\"/\" style=\"width: 273px;\"></p><p><img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/a1ee188e3d3b462c8f401a9205ddd10e.png\" data-filename=\"/\" style=\"width: 274px;\"></p><p>小程序码</p><p><img src=\"https://www.yishasoft.com/api/Resource/News/2019/07/31/1c30e2bb681448bab52e566993b82472.jpg\" data-filename=\"/\" style=\"width: 430px;\"><br></p><p><br></p>','微信小程序，找老师',340000,340100,340172,'https://www.yishasoft.com/api/Resource/News/2019/07/31/a627c3eed0ca428391fa62a841652ea4.png',2,'管理员','2019-07-31 21:44:00.000',1,35);
/*!40000 ALTER TABLE `sysnews` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-05-21 16:06:48
-- Total time: 0:0:0:0:117 (d:h:m:s:ms)
