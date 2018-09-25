CREATE TABLE [dbo].[Categories] (
    [CategoryID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (1024) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);



