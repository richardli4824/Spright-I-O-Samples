USE [Spright]

ALTER proc [dbo].[Blogs_UpdateV2]
			@Title nvarchar(200),
			@PublishDate datetime,
			@Category nvarchar(MAX)=null,
			@Slug nvarchar(MAX),
			@UserId nvarchar(128)=null,
			@PageContent nvarchar(MAX)=null,
			@IsFeatured bit,
			@Id int output

AS

BEGIN

	UPDATE [dbo].[Blogs]
	SET
			
			[Title] = @Title,
			[PublishDate] = @PublishDate,
			[Category] = @Category,
			[Slug] = @Slug,
			[UserId] = @UserId,
			[PageContent] = @PageContent,
			[IsFeatured] = @IsFeatured
			
	WHERE [Id]=@Id

END