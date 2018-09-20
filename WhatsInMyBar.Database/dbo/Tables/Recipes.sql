CREATE TABLE [dbo].[Recipes] (
    [RecipeID]    INT                NOT NULL,
    [Name]        VARCHAR (1024)     NULL,
    [Link]        VARCHAR (1024)     NULL,
    [Thumbnail]   VARCHAR (4096)     NULL,
    [Description] VARCHAR (4096)     NULL,
    [DateCreated] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Recipes] PRIMARY KEY CLUSTERED ([RecipeID] ASC)
);

