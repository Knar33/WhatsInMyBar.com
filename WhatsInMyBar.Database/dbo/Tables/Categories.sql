CREATE TABLE [dbo].[Categories] (
    [CategoryID] INT            NOT NULL,
    [Name]       VARCHAR (1024) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

