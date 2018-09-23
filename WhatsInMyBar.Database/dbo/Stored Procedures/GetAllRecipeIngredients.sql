-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllRecipeIngredients]
AS
BEGIN
	SET NOCOUNT ON;

	Select RecipeID, IngredientID, datecreated from RecipeIngredients
	
END