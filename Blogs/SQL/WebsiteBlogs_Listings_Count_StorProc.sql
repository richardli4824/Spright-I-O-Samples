USE [Spright]

ALTER proc [dbo].[WebsiteBlogs_Listings_Count]

@websiteId int

As 
 
Begin
		
SELECT count(BlogId)  

FROM [dbo].[Website_Blogs]

WHERE  websiteId = @websiteId

End

