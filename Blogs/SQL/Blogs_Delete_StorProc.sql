USE [Spright]

ALTER proc [dbo].[Blogs_Delete]

			@Id int
AS

BEGIN

DELETE FROM [dbo].[Blogs]
		WHERE Id = @Id

END