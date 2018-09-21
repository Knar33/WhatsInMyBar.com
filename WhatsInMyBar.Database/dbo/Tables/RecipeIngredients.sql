CREATE TABLE [dbo].[RecipeIngredients] (
    [IngredientID] INT                NOT NULL,
    [RecipeID]     INT                NULL,
    [DateCreated]  DATETIMEOFFSET (7) NULL
);

