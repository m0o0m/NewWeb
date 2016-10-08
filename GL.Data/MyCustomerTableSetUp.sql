

--
-- Table structure for table `CustomerRoles`
--

CREATE TABLE IF NOT EXISTS `CustomerRoles` (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CustomerUserClaims`
--

CREATE TABLE IF NOT EXISTS `CustomerUserClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `CustomerUserLogins`
--

CREATE TABLE IF NOT EXISTS `CustomerUserLogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `ApplicationUser_Logins` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CustomerUserRoles`
--

CREATE TABLE IF NOT EXISTS `CustomerUserRoles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IdentityRole_Users` (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CustomerUsers`
--

CREATE TABLE IF NOT EXISTS `CustomerUsers` (
  `Id` varchar(128) NOT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `NickName` varchar(256),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `CustomerUserClaims`
--
ALTER TABLE `CustomerUserClaims`
  ADD CONSTRAINT `CustomerUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `CustomerUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Constraints for table `CustomerUserLogins`
--
ALTER TABLE `CustomerUserLogins`
  ADD CONSTRAINT `CustomerUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `CustomerUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Constraints for table `CustomerUserRoles`
--
ALTER TABLE `CustomerUserRoles`
  ADD CONSTRAINT `CustomerUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `CustomerUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `CustomerRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `CustomerRoles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

