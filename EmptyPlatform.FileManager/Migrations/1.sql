DROP TABLE IF EXISTS [PhysicalFile];
CREATE TABLE [PhysicalFile] (
  [PhysicalFileId] TEXT PRIMARY KEY NOT NULL
, [ContentType] TEXT
, [Size] INTEGER
, [Hash] TEXT
, [CreatedDate] TEXT NOT NULL
, [CreatedByUserId] TEXT NOT NULL
, [IsActive] INTEGER NOT NULL
);

DROP TABLE IF EXISTS [File];
CREATE TABLE [File] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [FileId] TEXT NOT NULL
, [FileName] TEXT
, [PhysicalFileId] TEXT
, [CreatedDate] TEXT NOT NULL
, [CreatedByUserId] TEXT NOT NULL
, [IsActive] INTEGER NOT NULL
);