-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllIngredients]
AS
BEGIN
	SET NOCOUNT ON;

	Select IngredientID, Name, DateCreated from Ingredients
	
END