USE [Spright]

ALTER PROC [dbo].[WebsiteBlogs_ListBlogsByWebsiteId]

@CurrentPage int = 1,
@ItemsPerPage int = 5,  
@websiteID int 
   
AS

BEGIN
SELECT  wb.WebsiteID
	   ,wb.BlogId
	   ,b.Title
	   ,b.PublishDate
	   ,b.Slug
	   ,b.UserId
	   ,b.PageContent
	   ,b.IsFeatured
	   ,bm.BlogId
	   ,bm.IsCoverPhoto
	   ,m.ID
	   ,m.MediaType
	   ,m.Path
	   ,m.FileName
	   ,m.FileType
	   ,m.Title
	   ,m.Description
	   ,m.CreatedDate
	   ,m.ModifiedDate
	   ,m.ThumbnailPath
		

FROM [dbo].[Website_Blogs] as wb

LEFT JOIN [dbo].[Blogs] as b
on b.Id = wb.BlogId
LEFT JOIN  [dbo].[Blogs_Media] as bm
on bm.BlogId = b.Id
LEFT JOIN  [dbo].[Medias] as m
on m.ID = bm.MediaId
WHERE WebsiteID = @WebsiteID and
      bm.IsCoverPhoto=1

ORDER BY PublishDate DESC

OFFSET ((@CurrentPage - 1) * @ItemsPerPage) ROWS
  FETCH NEXT  @ItemsPerPage ROWS ONLY
      
END