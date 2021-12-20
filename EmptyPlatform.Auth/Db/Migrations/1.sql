BEGIN TRANSACTION;

DROP TABLE IF EXISTS [User];
CREATE TABLE [User] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [Id] text NOT NULL
, [Email] text NOT NULL
, [FirstName] text NOT NULL
, [SecondName] text NOT NULL
, [Birthday] text NOT NULL
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] text
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] text NOT NULL
);

DROP TABLE IF EXISTS [UserPassword];
CREATE TABLE [UserPassword] (
  [_Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [UserId] text NOT NULL
, [Password] text NOT NULL
, [_IsActive] INTEGER NOT NULL
, [_ActionNote] text
, [_CreatedDate] text NOT NULL
, [_CreatedByUserId] text NOT NULL
);

INSERT INTO [User] 
([Id],[Email],[FirstName],[SecondName],[Birthday],[_IsActive],[_CreatedDate],[_CreatedByUserId]) 
VALUES 
('e86adc24-321d-4e19-aeb4-6f294384bc2c','hitk@mail.ru','Данила','Федотов','1988-08-30 00:00:00.000',1,datetime(),'Manual insert');

INSERT INTO [UserPassword]
([UserId],[Password],[_IsActive],[_CreatedDate],[_CreatedByUserId])
VALUES
('e86adc24-321d-4e19-aeb4-6f294384bc2c','1',1,datetime(),'Manual insert');

COMMIT;