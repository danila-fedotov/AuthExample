BEGIN TRANSACTION;

DROP TABLE IF EXISTS [Session];
CREATE TABLE [Session] (
  [SessionId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Device] text NOT NULL
, [Address] text NOT NULL
, [CreatedDate] text NOT NULL
, [ClosedDate] text
);

DROP TABLE IF EXISTS [User];
CREATE TABLE [User] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Email] text
, [FirstName] text
, [SecondName] text
, [Birthday] text
, [IsActive] INTEGER NOT NULL
, [ActionNote] text
, [CreatedDate] text NOT NULL
, [CreatedByUserId] text NOT NULL
);

DROP TABLE IF EXISTS [UserPassword];
CREATE TABLE [UserPassword] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Password] text
, [IsActive] INTEGER NOT NULL
, [ActionNote] text
, [CreatedDate] text NOT NULL
, [CreatedByUserId] text NOT NULL
);

DROP TABLE IF EXISTS [Role];
CREATE TABLE [Role] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [RoleId] TEXT NOT NULL
, [Name] TEXT NOT NULL
, [IsActive] INTEGER NOT NULL
, [ActionNote] TEXT
, [CreatedDate] TEXT NOT NULL
, [CreatedByUserId] TEXT NOT NULL
);

DROP TABLE IF EXISTS [UserRole];
CREATE TABLE [UserRole] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [RoleId] text NOT NULL
, [CreatedDate] text NOT NULL
, [CreatedByUserId] TEXT NOT NULL
, [CreatedWithNote] TEXT
, [ClosedDate] TEXT
, [ClosedByUserId] TEXT
, [ClosedWithNote] TEXT
);

DROP TABLE IF EXISTS [RolePermission];
CREATE TABLE [RolePermission] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [RoleId] text NOT NULL
, [PermissionsAsJson] text
, [IsActive] INTEGER NOT NULL
, [ActionNote] text
, [CreatedDate] text NOT NULL
, [CreatedByUserId] text NOT NULL
);

INSERT INTO [User] 
([UserId],[Email],[FirstName],[SecondName],[Birthday],[IsActive],[CreatedDate],[CreatedByUserId]) 
VALUES 
('e86adc24-321d-4e19-aeb4-6f294384bc2c','hitk@mail.ru','Данила','Федотов','1988-08-30 00:00:00.000',1,datetime(),'root'),
('e86adc24-321d-4e19-aeb4-6f294384bc1c','danila.fedotov.v@gmail.com','Danila','Fedotov','1988-08-30 00:00:00.000',1,datetime(),'root');

INSERT INTO [UserPassword]
([UserId],[Password],[IsActive],[CreatedDate],[CreatedByUserId])
VALUES
('e86adc24-321d-4e19-aeb4-6f294384bc2c','1',1,datetime(),'root'),
('e86adc24-321d-4e19-aeb4-6f294384bc1c','1',1,datetime(),'root');

INSERT INTO [Role] 
([RoleId],[Name],[IsActive],[CreatedDate],[CreatedByUserId]) 
VALUES 
('0595bd21-8f81-49dc-bd3a-6f1de096e555','Admin',1,datetime(),'root'),
('0595bd21-8f81-49dc-bd3a-6f1de096e551','Guest',1,datetime(),'root');

INSERT INTO [RolePermission]
([RoleId],[PermissionsAsJson],[IsActive],[CreatedDate],[CreatedByUserId])
VALUES
('0595bd21-8f81-49dc-bd3a-6f1de096e555','{
   "user",
   "role"
}',1,datetime(),'root');

INSERT INTO [UserRole]
([UserId],[RoleId],[CreatedDate],[CreatedByUserId])
VALUES
('e86adc24-321d-4e19-aeb4-6f294384bc1c','0595bd21-8f81-49dc-bd3a-6f1de096e555',datetime(),'root'),
('e86adc24-321d-4e19-aeb4-6f294384bc2c','0595bd21-8f81-49dc-bd3a-6f1de096e551',datetime(),'root');

COMMIT;