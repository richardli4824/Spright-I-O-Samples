USE [Spright]

ALTER proc [dbo].[WebsiteBlogs_SelectByBlogSlug]
(
@Slug nvarchar(MAX)
)
AS

BEGIN
DECLARE @BlogId int
SET @BlogId = (SELECT Id
			 FROM [dbo].[Blogs]
			WHERE Slug = @Slug)
SELECT
	[Id],
	[Title],
	[PublishDate],
	[UserId],
	[PageContent],
	[Slug],
	[IsFeatured]


FROM [dbo].[Blogs]
WHERE Slug = @Slug ORDER BY Id DESC

EXECUTE dbo.BlogsMedia_ListById @BlogId

END