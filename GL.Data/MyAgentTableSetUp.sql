

--
-- Table structure for table AgentRoles
--

CREATE TABLE IF NOT EXISTS AgentRoles (
  Id int(11) NOT NULL,
  Name varchar(256) NOT NULL,
  PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table AgentUserClaims
--

CREATE TABLE IF NOT EXISTS AgentUserClaims (
  Id int(11) NOT NULL AUTO_INCREMENT,
  UserId int(11) NOT NULL,
  ClaimType longtext,
  ClaimValue longtext,
  PRIMARY KEY (Id),
  KEY UserId (UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table AgentUserLogins
--

CREATE TABLE IF NOT EXISTS AgentUserLogins (
  LoginProvider varchar(128) NOT NULL,
  ProviderKey varchar(128) NOT NULL,
  UserId int(11) NOT NULL,
  PRIMARY KEY (LoginProvider,ProviderKey,UserId),
  KEY ApplicationUser_Logins (UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table AgentUserRoles
--

CREATE TABLE IF NOT EXISTS AgentUserRoles (
  UserId int(11) NOT NULL,
  RoleId int(11) NOT NULL,
  PRIMARY KEY (UserId,RoleId),
  KEY IdentityRole_Users (RoleId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table AgentUsers
--

CREATE TABLE IF NOT EXISTS AgentUsers (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Email varchar(256) DEFAULT NULL,
  EmailConfirmed tinyint(1) NOT NULL,
  PasswordHash longtext,
  SecurityStamp longtext,
  PhoneNumber longtext,
  PhoneNumberConfirmed tinyint(1) NOT NULL,
  TwoFactorEnabled tinyint(1) NOT NULL,
  LockoutEndDateUtc datetime DEFAULT NULL,
  LockoutEnabled tinyint(1) NOT NULL,
  AccessFailedCount int(11) NOT NULL,
  UserName varchar(256) NOT NULL,
  AgentName varchar(32) DEFAULT NULL,
  AgentLv int(11) DEFAULT '1',
  AgentQQ varchar(11) DEFAULT NULL,
  Deposit decimal(19,4) DEFAULT '0.0000',
  InitialAmount decimal(19,4) DEFAULT '0.0000',
  AmountAvailable decimal(19,4) DEFAULT '0.0000',
  HavaAmount decimal(19,4) DEFAULT '0.0000',
  HigherLevel int(11) DEFAULT '0',
  LowerLevel int(11) DEFAULT '0',
  AgentState int(11) DEFAULT '0',
  OnlineState int(11) DEFAULT '0',
  RegisterTime datetime DEFAULT NULL,
  LoginIP varchar(50) DEFAULT NULL,
  LoginTime datetime DEFAULT NULL,
  Recharge decimal(19,4) DEFAULT '0.0000',
  Drawing decimal(19,4) DEFAULT '0.0000',
  DrawingPasswd varchar(33) DEFAULT NULL,
  RevenueModel int(11) DEFAULT '0',
  EarningsRatio int(11) DEFAULT '0',
  RebateRate int(11) DEFAULT '0',
  JurisdictionID longtext,
  Extend_isDefault bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Constraints for dumped tables
--

--
-- Constraints for table AgentUserClaims
--
ALTER TABLE AgentUserClaims
  ADD CONSTRAINT AgentUser_Claims FOREIGN KEY (UserId) REFERENCES AgentUsers (Id) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Constraints for table AgentUserLogins
--
ALTER TABLE AgentUserLogins
  ADD CONSTRAINT AgentUser_Logins FOREIGN KEY (UserId) REFERENCES AgentUsers (Id) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Constraints for table AgentUserRoles
--
ALTER TABLE AgentUserRoles
  ADD CONSTRAINT AgentUser_Roles FOREIGN KEY (UserId) REFERENCES AgentUsers (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT AgentUserRole_Users FOREIGN KEY (RoleId) REFERENCES AgentRoles (Id) ON DELETE CASCADE ON UPDATE NO ACTION;

