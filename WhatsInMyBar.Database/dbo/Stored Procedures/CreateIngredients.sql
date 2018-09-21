-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateIngredients]
	@IngredientID INT,
	@RecipeID INT,
	@Name Varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Ingredients (IngredientID, RecipeID, Name, DateCreated) VALUES (@IngredientID, @RecipeID, @Name, SYSDATETIMEOFFSET())
	
END