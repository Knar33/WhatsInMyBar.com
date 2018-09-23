-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllIngredientCategories]
AS
BEGIN
	SET NOCOUNT ON;

	Select IngredientID, CategoryID, datecreated from IngredientCategories
	
END