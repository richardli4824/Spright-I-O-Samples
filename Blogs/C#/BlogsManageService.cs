using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Spright.Data;
using Spright.Web.Models.Requests;
using Spright.Web.Models.ViewModels;
using Spright.Web.Services.Interfaces;
using Spright.Web.Models.Requests.EVA;
using Spright.Web.Domain;

namespace Spright.Web.Services
{
    public class BlogsManageService : BaseService, IBlogsManageService
    {
        private SystemEventService _SystemEventService { get; set; }

        public BlogsManageService(SystemEventService SystemEventService)
        {
            _SystemEventService = SystemEventService;
        }

        public int Insert(BlogsManageRequest model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Blogs_Insert2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@PublishDate", model.PublishDate);
                   paramCollection.AddWithValue("@Category", model.Category);
                   paramCollection.AddWithValue("@Slug", model.Slug);
                   //paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());
                   paramCollection.AddWithValue("@PageContent", model.PageContent);
                   paramCollection.AddWithValue("@IsFeatured", model.IsFeatured);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);


                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );

            SystemEventRequestModel se = new SystemEventRequestModel();
            se.TargetID = uid;
            se.ActorUserId = UserService.GetCurrentUserId();
            se.EventType = Enums.SystemEventType.NewBlog;
            _SystemEventService.Insert(se);

            return uid;
        }

        public void Update(BlogsManageRequest model, int BlogsId)

        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Blogs_UpdateV2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@PublishDate", model.PublishDate);
                   paramCollection.AddWithValue("@Category", model.Category);
                   paramCollection.AddWithValue("@Slug", model.Slug);
                   paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@PageContent", model.PageContent);
                   paramCollection.AddWithValue("@IsFeatured", model.IsFeatured);
                   paramCollection.AddWithValue("@Id", BlogsId);


               }, returnParameters: delegate (SqlParameterCollection param)
               {
               });
        }

        public List<Domain.Blog> GetBlogs()
        {
            List<Domain.Blog> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Blogs_SelectAll"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

               }
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Blog p = new Domain.Blog();
                   int startingIndex = 0;

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.PageContent = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.Blog>();
                   }

                   list.Add(p);
               }
               );
            return list;
        }

        public Domain.Blog GetBlogById(int Id)
        {
            Domain.Blog p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Blogs_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", Id);

               }, map: delegate (IDataReader reader, short set)
               {

                   if (set == 0)
                   {
                       p = new Domain.Blog();
                       int startingIndex = 0;

                       p.Id = reader.GetSafeInt32(startingIndex++);
                       p.Title = reader.GetSafeString(startingIndex++);
                       p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                       p.UserId = reader.GetSafeString(startingIndex++);
                       p.PageContent = reader.GetSafeString(startingIndex++);
                       p.Slug = reader.GetSafeString(startingIndex++);
                       p.IsFeatured = reader.GetSafeBool(startingIndex);
                       p.BMedia = new List<BlogMedia>();

                   }
                   else if (set == 1)
                   {
                       BlogMedia x = new BlogMedia();
                       int startingIndex = 0;

                       x.BlogId = reader.GetSafeInt32(startingIndex++);
                       x.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                       x.MediaType = reader.GetSafeString(startingIndex++);
                       x.Path = reader.GetSafeString(startingIndex++);
                       x.FileName = reader.GetSafeString(startingIndex++);
                       x.FileType = reader.GetSafeString(startingIndex++);
                       x.Title = reader.GetSafeString(startingIndex++);
                       x.Description = reader.GetSafeString(startingIndex++);
                       x.UserID = reader.GetSafeString(startingIndex++);
                       x.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                       x.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                       x.ID = reader.GetSafeInt32(startingIndex++);

                       p.BMedia.Add(x);
                       if (x.IsCoverPhoto == true)
                           p.Media = x;
                   }

               }
               );


            return p;
        }

        public void DeleteBlogById(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Blogs_Delete"
            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, returnParameters: delegate (SqlParameterCollection param)
            {
            }
             );
        }

        public List<Domain.Blog> GetBlogsLatestToOldest(PaginateListRequestModel model)
        {
            List<Domain.Blog> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Blog_Listingstwo"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);

               }
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Blog p = new Domain.Blog();
                   Domain.BlogMedia bm = new Domain.BlogMedia();
                   int startingIndex = 0;

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.PageContent = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   bm.BlogId = reader.GetSafeInt32(startingIndex++);
                   bm.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                   bm.ID = reader.GetSafeInt32(startingIndex++);
                   bm.MediaType = reader.GetSafeString(startingIndex++);
                   bm.Path = reader.GetSafeString(startingIndex++);
                   bm.FileName = reader.GetSafeString(startingIndex++);
                   bm.FileType = reader.GetSafeString(startingIndex++);
                   bm.Title = reader.GetSafeString(startingIndex++);
                   bm.Description = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.Blog>();
                   }
                   if (bm.ID == 0)
                   {
                       p.Media = null;
                   }

                   else
                   {
                       p.Media = bm;
                   }
                   list.Add(p);
               });
            return list;
        }

        public List<Domain.Blog> GetBlogsLatestToOldestByWebsiteId(PaginateListRequestModel model, int websiteId)
        {
            List<Domain.Blog> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Blog_Listings3"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                   paramCollection.AddWithValue("@websiteId", websiteId);

               }
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Blog p = new Domain.Blog();
                   Domain.BlogMedia bm = new Domain.BlogMedia();
                   int startingIndex = 0;

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.PageContent = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   bm.BlogId = reader.GetSafeInt32(startingIndex++);
                   bm.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                   bm.ID = reader.GetSafeInt32(startingIndex++);
                   bm.MediaType = reader.GetSafeString(startingIndex++);
                   bm.Path = reader.GetSafeString(startingIndex++);
                   bm.FileName = reader.GetSafeString(startingIndex++);
                   bm.FileType = reader.GetSafeString(startingIndex++);
                   bm.Title = reader.GetSafeString(startingIndex++);
                   bm.Description = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.Blog>();
                   }
                   if (bm.ID == 0)
                   {
                       p.Media = null;
                   }

                   else
                   {
                       p.Media = bm;
                   }
                   list.Add(p);
               });
            return list;
        }

        public List<Domain.Blog> GetBlogsByFeatured(PaginateListRequestModel model)
        {
            List<Domain.Blog> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Blogs_SelectByFeatured"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);

               }
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Blog p = new Domain.Blog();
                   Domain.BlogMedia bm = new Domain.BlogMedia();

                   int startingIndex = 0;

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.PageContent = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.IsFeatured = reader.GetSafeBool(startingIndex++);

                   bm.BlogId = reader.GetSafeInt32(startingIndex++);
                   bm.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                   bm.ID = reader.GetSafeInt32(startingIndex++);
                   bm.MediaType = reader.GetSafeString(startingIndex++);
                   bm.Path = reader.GetSafeString(startingIndex++);
                   bm.FileName = reader.GetSafeString(startingIndex++);
                   bm.FileType = reader.GetSafeString(startingIndex++);
                   bm.Title = reader.GetSafeString(startingIndex++);
                   bm.Description = reader.GetSafeString(startingIndex++);
                   p.Media = bm;

                   if (list == null)
                   {
                       list = new List<Domain.Blog>();
                   }

                   list.Add(p);
               });
            return list;
        }

        public int GetBlogsByFeatured_Count()
        {

            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Blogs_SelectByFeatured_Count"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {


                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0;
                       count = reader.GetSafeInt32(startingIndex++);
                   });

            return count;
        }

        public List<Domain.Blog> ListBlogsByWebsiteId(PaginateListRequestModel model, int websiteId)
        {
            List<Domain.Blog> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.WebsiteBlogs_ListBlogsByWebsiteId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                   paramCollection.AddWithValue("@websiteId", websiteId);

               }
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Website w = new Domain.Website();
                   Domain.Blog p = new Domain.Blog();
                   Domain.BlogMedia bm = new Domain.BlogMedia();

                   int startingIndex = 0;

                   w.ID = reader.GetSafeInt32(startingIndex++);
                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.PageContent = reader.GetSafeString(startingIndex++);
                   p.IsFeatured = reader.GetSafeBool(startingIndex++);

                   bm.BlogId = reader.GetSafeInt32(startingIndex++);
                   bm.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                   bm.ID = reader.GetSafeInt32(startingIndex++);
                   bm.MediaType = reader.GetSafeString(startingIndex++);
                   bm.Path = reader.GetSafeString(startingIndex++);
                   bm.FileName = reader.GetSafeString(startingIndex++);
                   bm.FileType = reader.GetSafeString(startingIndex++);
                   bm.Title = reader.GetSafeString(startingIndex++);
                   bm.Description = reader.GetSafeString(startingIndex++);
                   bm.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                   bm.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                   bm.ThumbnailPath = reader.GetSafeString(startingIndex++);
                   p.Media = bm;

                   if (list == null)
                   {
                       list = new List<Domain.Blog>();
                   }

                   list.Add(p);
               });
            return list;
        }

        public int WebsiteBlogs_Listings_Count(int websiteId)
        {

            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.WebsiteBlogs_Listings_Count"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@websiteId", websiteId);

                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0;
                       count = reader.GetSafeInt32(startingIndex++);

                   });

            return count;

        }

        public Domain.Blog GetBlogBySlug(string slug)
        {
            Domain.Blog p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.WebsiteBlogs_SelectByBlogSlug"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Slug", slug);

               }, map: delegate (IDataReader reader, short set)
               {

                   if (set == 0)
                   {
                       p = new Domain.Blog();
                       int startingIndex = 0;

                       p.Id = reader.GetSafeInt32(startingIndex++);
                       p.Title = reader.GetSafeString(startingIndex++);
                       p.PublishDate = reader.GetSafeDateTime(startingIndex++);
                       p.UserId = reader.GetSafeString(startingIndex++);
                       p.PageContent = reader.GetSafeString(startingIndex++);
                       p.Slug = reader.GetSafeString(startingIndex++);
                       p.IsFeatured = reader.GetSafeBool(startingIndex);
                       p.BMedia = new List<BlogMedia>();

                   }
                   else if (set == 1)
                   {
                       BlogMedia x = new BlogMedia();
                       int startingIndex = 0;

                       x.BlogId = reader.GetSafeInt32(startingIndex++);
                       x.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                       x.MediaType = reader.GetSafeString(startingIndex++);
                       x.Path = reader.GetSafeString(startingIndex++);
                       x.FileName = reader.GetSafeString(startingIndex++);
                       x.FileType = reader.GetSafeString(startingIndex++);
                       x.Title = reader.GetSafeString(startingIndex++);
                       x.Description = reader.GetSafeString(startingIndex++);
                       x.UserID = reader.GetSafeString(startingIndex++);
                       x.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                       x.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                       x.ID = reader.GetSafeInt32(startingIndex++);

                       p.BMedia.Add(x);
                       if (x.IsCoverPhoto == true)
                           p.Media = x;
                   }

               }
               );


            return p;
        }
    }
}
