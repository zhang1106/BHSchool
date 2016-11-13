use bhcs
set global optimizer_switch='derived_merge=OFF';

ALTER TABLE `bhcs`.`bhclass` 
ADD COLUMN `deleted` bit NOT NULL DEFAULT 0;

ALTER TABLE `bhcs`.`class_student` 
ADD COLUMN `status` VARCHAR(45) NOT NULL DEFAULT 'Active' AFTER `id`;

ALTER TABLE `bhcs`.`message` 
ADD COLUMN `name` VARCHAR(45) NULL AFTER `id`;

CREATE TABLE `transaction` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberId` int(11) NOT NULL,
  `amount` decimal(22,4) DEFAULT NULL,
  `description` varchar(200) DEFAULT NULL,
  `updatedAt` datetime NOT NULL,
  `updatedBy` varchar(100) NOT NULL,
  `status` varchar(45) NOT NULL DEFAULT 'confirmed' COMMENT '1-confirmed\n0-cancelled',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

CREATE TABLE `transactiondetail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `transactionId` int(11) NOT NULL,
  `ItemId` int(11) DEFAULT NULL,
  `transactionType` varchar(45) DEFAULT NULL COMMENT 'Register Class/Cancel Class/Coupon/Fee',
  `description` varchar(200) DEFAULT NULL,
  `updatedAt` datetime NOT NULL,
  `updatedBy` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;




CREATE DATABASE `bhcs` /*!40100 DEFAULT CHARACTER SET utf8 */;

CREATE TABLE `address` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberId` int(11) DEFAULT NULL,
  `address1` varchar(200) DEFAULT NULL,
  `address2` varchar(200) DEFAULT NULL,
  `city` varchar(100) DEFAULT NULL,
  `state` varchar(45) DEFAULT NULL,
  `zip` varchar(20) DEFAULT NULL,
  `country` varchar(45) DEFAULT NULL,
  `updatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;


CREATE TABLE `bhclass` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `timeslotId` int(11) NOT NULL DEFAULT '0',
  `teacherId` int(11) DEFAULT NULL,
  `classroomId` int(11) DEFAULT NULL,
  `fee` decimal(10,0) DEFAULT NULL,
  `semester` varchar(100) NOT NULL,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedBy` varchar(45) DEFAULT NULL,
  `courseId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
CREATE TABLE `class_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `classId` int(11) NOT NULL,
  `studentId` int(11) NOT NULL,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `confirmed` bit(1) DEFAULT b'0',
  `modifiedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;

CREATE TABLE `classroom` (
  `id` bigint(15) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `capacity` int(11) DEFAULT NULL,
  `function` varchar(200) DEFAULT NULL,
  `description` varchar(200) DEFAULT NULL,
  `createAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `course` (
  `id` bigint(15) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `textbook` varchar(100) DEFAULT NULL,
  `textbookFee` decimal(10,0) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  `minAge` int(11) DEFAULT NULL,
  `modifiedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
REATE TABLE `member` (
  `id` int(15) NOT NULL AUTO_INCREMENT,
  `firstname` varchar(45) DEFAULT NULL,
  `lastname` varchar(45) DEFAULT NULL,
  `chineseName` varchar(45) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `born` date DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `type` varchar(45) DEFAULT NULL COMMENT 'student/teacher/board/admin',
  `title` varchar(45) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `householdId` int(11) DEFAULT NULL,
  `active` bit(1) DEFAULT b'1',
  `modifiedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `createdBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

CREATE TABLE `message` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(200) DEFAULT NULL,
  `body` longtext,
  `category` varchar(45) DEFAULT NULL,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `createdBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;


CREATE TABLE `timeslot` (
  `id` bigint(15) NOT NULL,
  `start` varchar(45) DEFAULT NULL,
  `end` varchar(45) DEFAULT NULL,
  `createAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

set global optimizer_switch='derived_merge=OFF';CREATE TABLE `config` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `type` varchar(45) DEFAULT NULL,
  `key` varchar(45) DEFAULT NULL,
  `value` varchar(45) DEFAULT NULL,
  `description` varchar(200) DEFAULT NULL,
  `modifiedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
