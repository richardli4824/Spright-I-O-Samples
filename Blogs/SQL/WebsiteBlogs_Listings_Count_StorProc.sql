USE [Spright]

ALTER proc [dbo].[WebsiteBlogs_Listings_Count]

@websiteId int

/*
Execute [dbo].[WebsiteBlogs_Listings_Count] 1120
*/
	
As 
 

Begin
		

SELECT count(BlogId)  

--FROM [dbo].[Blogs]

FROM [dbo].[Website_Blogs]

WHERE  websiteId = @websiteId

End

