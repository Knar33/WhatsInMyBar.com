-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllRecipes]
AS
BEGIN
	SET NOCOUNT ON;

	Select RecipeID, Name, Link, Thumbnail, Description, DateCreated from Recipes
	
END