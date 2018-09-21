-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateRecipeIngredients]
	@IngredientID INT,
	@RecipeID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO RecipeIngredients (IngredientID, RecipeID, DateCreated) VALUES (@IngredientID, @RecipeID, SYSDATETIMEOFFSET())
	
END