-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateRecipes]
	@RecipeID INT,
	@Name Varchar(1024),
	@Link Varchar(1024),
	@Thumbnail Varchar(4096),
	@Description Varchar(4096)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Recipes (RecipeID, Name, Link, Thumbnail, Description, DateCreated) VALUES (@RecipeID, @Name, @Link, @Thumbnail, @Description, SYSDATETIMEOFFSET())
	
END