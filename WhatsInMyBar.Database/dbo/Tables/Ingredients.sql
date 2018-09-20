﻿CREATE TABLE [dbo].[Ingredients] (
    [IngredientID] INT                NOT NULL,
    [CategoryID]   INT                NULL,
    [Name]         VARCHAR (1024)     NULL,
    [DateCreated]  DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED ([IngredientID] ASC)
);

