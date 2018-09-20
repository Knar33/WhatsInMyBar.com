-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateIngredients]
	@IngredientID INT,
	@Name Varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Ingredients (IngredientID, Name, DateCreated) VALUES (@IngredientID, @Name, SYSDATETIMEOFFSET())
	
END