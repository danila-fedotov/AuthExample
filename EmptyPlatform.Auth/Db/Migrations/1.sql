BEGIN TRANSACTION;

DROP TABLE IF EXISTS [Session];
CREATE TABLE [Session] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Device] text NOT NULL
, [Address] text NOT NULL
, [CreatedDate] text NOT NULL
, [ClosedDate] text
);

DROP TABLE IF EXISTS [User];
CREATE TABLE [User] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [Id] text NOT NULL
, [Email] text
, [FirstName] text
, [SecondName] text
, [Birthday] text
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] text
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] text NOT NULL
);

DROP TABLE IF EXISTS [UserPassword];
CREATE TABLE [UserPassword] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Password] text
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] text
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] text NOT NULL
);

DROP TABLE IF EXISTS [Role];
CREATE TABLE [Role] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [Id] TEXT NOT NULL
, [Name] TEXT NOT NULL
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] TEXT
, [_CreatedDate] TEXT NOT NULL
, [_CreatedByUserId] TEXT NOT NULL
);

DROP TABLE IF EXISTS [UserRole];
CREATE TABLE [UserRole] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [RoleId] text NOT NULL
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] TEXT NOT NULL
, [_CreatedWithNote] TEXT
, [_ClosedDate] TEXT
, [_ClosedByUserId] TEXT
, [_ClosedWithNote] TEXT
);

DROP TABLE IF EXISTS [RolePermission];
CREATE TABLE [RolePermission] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [RoleId] text NOT NULL
, [PermissionsAsJson] text
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] text
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] text NOT NULL
);

INSERT INTO [User] 
([Id],[Email],[FirstName],[SecondName],[Birthday],[_IsActive],[_CreatedDate],[_CreatedByUserId]) 
VALUES 
('e86adc24-321d-4e19-aeb4-6f294384bc2c','hitk@mail.ru','Данила','Федотов','1988-08-30 00:00:00.000',1,datetime(),'root');

INSERT INTO [UserPassword]
([UserId],[Password],[_IsActive],[_CreatedDate],[_CreatedByUserId])
VALUES
('e86adc24-321d-4e19-aeb4-6f294384bc2c','1',1,datetime(),'root');

INSERT INTO [Role] 
([Id],[Name],[_IsActive],[_CreatedDate],[_CreatedByUserId]) 
VALUES 
('0595bd21-8f81-49dc-bd3a-6f1de096e555','Admin',1,datetime(),'root');

INSERT INTO [RolePermission]
([RoleId],[PermissionsAsJson],[_IsActive],[_CreatedDate],[_CreatedByUserId])
VALUES
('0595bd21-8f81-49dc-bd3a-6f1de096e555','{
   "user":[
      "create",
      "get",
      "getusers",
      "remove",
      "update"
   ]
}',1,datetime(),'root');

INSERT INTO [UserRole]
([UserId],[RoleId],[_CreatedDate],[_CreatedByUserId])
VALUES
('e86adc24-321d-4e19-aeb4-6f294384bc2c','0595bd21-8f81-49dc-bd3a-6f1de096e555',datetime(),'root');

COMMIT;