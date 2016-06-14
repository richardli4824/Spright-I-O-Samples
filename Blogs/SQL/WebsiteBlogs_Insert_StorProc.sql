USE [Spright

ALTER proc [dbo].[WebsiteBlogs_Insert]
			@WebsiteId int,
			@BlogId int,
			@Id int output


AS

BEGIN

	INSERT INTO [dbo].[Website_Blogs]
			(
			[WebsiteId],
			[BlogId]
			)
	VALUES
			( @WebsiteId, @BlogId);


	SET		@Id = SCOPE_IDENTITY()

END