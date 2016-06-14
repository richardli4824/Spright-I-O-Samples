using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spright.Web.Models.Requests;
using System.Data.SqlClient;
using System.Data;
using Spright.Data;
using Spright.Web.Services.Interfaces;

namespace Spright.Web.Services
{
    public class BlogsMediaService : BaseService, IBlogsMediaService
    {
        public int Insert(BlogsMediaRequestModel model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogsMedia_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@BlogId", model.BlogId);
                   paramCollection.AddWithValue("@MediaId", model.MediaId);
                   paramCollection.AddWithValue("@IsCoverPhoto", model.IsCoverPhoto);

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );

            return uid;
        }

        public List<Domain.BlogMedia> BlogsMediaList(int BlogId)
        {
            List<Domain.BlogMedia> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.BlogsMedia_ListById"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@Id", BlogId);
              }
              , map: delegate (IDataReader reader, short set)
              {
                  Domain.BlogMedia p = new Domain.BlogMedia();
                  int startingIndex = 0;

                  p.BlogId = reader.GetSafeInt32(startingIndex++);
                  p.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                  p.MediaType = reader.GetSafeString(startingIndex++);
                  p.Path = reader.GetSafeString(startingIndex++);
                  p.FileName = reader.GetSafeString(startingIndex++);
                  p.FileType = reader.GetSafeString(startingIndex++);
                  p.Title = reader.GetSafeString(startingIndex++);
                  p.Description = reader.GetSafeString(startingIndex++);
                  p.UserID = reader.GetSafeString(startingIndex++);
                  p.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                  p.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                  p.ID = reader.GetSafeInt32(startingIndex++);

                  if (list == null)
                  {
                      list = new List<Domain.BlogMedia>();
                  }

                  list.Add(p);
              });

            return list;
        }

        public void UpdateProfilePhoto(BlogsMediaRequestModel model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Blogs_MediaUpdate"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@BlogId", model.BlogId);
                   paramCollection.AddWithValue("@MediaId", model.MediaId);

               }, returnParameters: delegate (SqlParameterCollection param) { }

               );

        }

        public void DeleteBlogMedia(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.BlogsMedia_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@MediaId", Id);

               }, returnParameters: delegate (SqlParameterCollection param) { }

               );

        }
    }
}
