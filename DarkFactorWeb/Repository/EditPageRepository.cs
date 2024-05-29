using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using DarkFactorCoreNet.Models;
using DFCommonLib.DataAccess;
using Org.BouncyCastle.Security;
using System.Data.SqlTypes;

namespace DarkFactorCoreNet.Repository
{
    public interface IEditPageRepository
    {
        bool SavePage(PageContentModel pageModel);
        bool DeletePage(int pageId);
        bool CreatePageWithParent(int parentPageId, string pageTotle, int sortId); 
        bool AddImage(int pageID, uint imageId);
        bool ChangeAccess(int pageId, int accessLevel);
    }

    public class EditPageRepository : IEditPageRepository
    {
        private IDbConnectionFactory _connection;

        public EditPageRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public bool SavePage(PageContentModel pageModel)
        {
            string sql = @"update content set "
                         + " parentid=@parentid, "
                         + " promo_title = @promo_title, "
                         + " promo_text = @promo_text, "
                         + " content_title =@content_title, "
                         + " content_text = @content_text, "
                         + " imageid = @imageid, "
                         + " sort =@sort, "
                         + " published = @published "
                         + "where id = @id ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@parentid", pageModel.ParentId);
                cmd.AddParameter("@promo_title", pageModel.PromoTitle);
                cmd.AddParameter("@promo_text", pageModel.PromoText);
                cmd.AddParameter("@content_title", pageModel.ContentTitle);
                cmd.AddParameter("@content_text", pageModel.ContentText);
                cmd.AddParameter("@imageid", pageModel.ImageId);
                cmd.AddParameter("@sort", pageModel.SortId);
                cmd.AddParameter("@published", pageModel.Acl);

                cmd.AddParameter("@id", pageModel.ID);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public bool DeletePage(int pageId)
        {
            string sql = @"delete from content where id = @id ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@id", pageId);
                int numRows = cmd.ExecuteNonQuery();
                return numRows == 1;
            }
        }

        public bool CreatePageWithParent( int parentPageId, string pageTitle, int sortId )
        {
            // Create page in database
            string sql = @"insert into content(parentid,content_title,sort,content_text,published,externurl) " + 
                        "values(@parentid,@content_title, @sortid, null, false, null) ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@parentid", parentPageId);
                cmd.AddParameter("@content_title", pageTitle);
                cmd.AddParameter("@sortid", sortId + 1);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public bool AddImage(int pageId, uint imageId)
        {
            var sql = @"update content set imageid = @imageid where id = @pageid "; 
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                cmd.AddParameter("@imageid", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public bool ChangeAccess(int pageId, int accessLevel)
        {
            var sql = @"update content set published = @accessLevel where id = @pageid "; 
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                cmd.AddParameter("@accessLevel", accessLevel);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }
    }
}
