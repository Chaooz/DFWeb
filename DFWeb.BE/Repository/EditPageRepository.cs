using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using DarkFactorCoreNet.Models;
using DFCommonLib.DataAccess;
using Org.BouncyCastle.Security;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace DarkFactorCoreNet.Repository
{
    public interface IEditPageRepository
    {
        bool SavePage(PageContentModel pageModel);
        bool DeletePage(int pageId);
        bool CreatePageWithParent(int parentPageId, string pageTotle, int sortId); 
        int GetArticleSectionMaxSortId(int pageId);
        bool CreateArticleSection(int pageId, string title, string content, int sortId);
        bool UpdateArticleSection(ArticleSectionModel articleSectionModel);
        bool DeleteArticleSection(ArticleSectionModel articleSectionModel);
        bool ChangeSectionLayout(int articleId, int layout);
        bool AddImage(int pageID, uint imageId);
        bool AddImageToSection(int sectionId, uint imageId);
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
                         + " promo_text = @promo_text, "
                         + " content_title =@content_title, "
                         + " content_text = @content_text, "
                         + " imageid = @imageid, "
                         + " sort =@sort, "
                         + " published = @published, "
                         + " tags = @tags, "
                         + " related_tags = @related_tags, "
                         + " last_updated = now() "
                         + "where id = @id ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@parentid", pageModel.ParentId);
                cmd.AddParameter("@promo_text", pageModel.PromoText);
                cmd.AddParameter("@content_title", pageModel.ContentTitle);
                cmd.AddParameter("@content_text", pageModel.ContentText);
                cmd.AddParameter("@imageid", pageModel.ImageId);
                cmd.AddParameter("@sort", pageModel.SortId);
                cmd.AddParameter("@published", pageModel.Acl);
                cmd.AddParameter("@tags", pageModel.Tags);
                cmd.AddParameter("@related_tags", pageModel.RelatedTags);

                cmd.AddParameter("@id", pageModel.PageId);
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
            string sql = @"insert into content(parentid,content_title,sort,content_text,published,externurl, last_updated) " + 
                        "values(@parentid,@content_title, @sortid, null, @published, null, now() ) ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@parentid", parentPageId);
                cmd.AddParameter("@content_title", pageTitle);
                cmd.AddParameter("@sortid", sortId + 1);
                cmd.AddParameter("@published", AccessLevel.Editor);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        private bool UpdatedArticle(int articleId)
        {
            string sql = @"update content set last_updated = now() where id = @articleId ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@articleId", articleId);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public int GetArticleSectionMaxSortId(int pageId)
        {
            string sql = @"select max(sort) from articlesection where pageid = @pageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader[0] is DBNull)
                        {
                            return 0;
                        }
                        return Convert.ToInt32(reader[0]);
                    }
                }
            }
            return 0;
        }

        public bool CreateArticleSection(int pageId, string title, string content, int sortId)
        {
            // Create page in database
            string sql = @"insert into articlesection(pageId,text,imageid,sort,layout) " + 
                        "values(@pageId, @content, @imageid, @sortId, @layout) ";

            int imageId = 0;
            int layout = 0;

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageId", pageId);
                cmd.AddParameter("@imageid", imageId);
                cmd.AddParameter("@content", content);
                cmd.AddParameter("@sortid", sortId);
                cmd.AddParameter("@layout", layout);
                int numRows = cmd.ExecuteNonQuery();

                var updatedArticle = UpdatedArticle(pageId);
                return numRows == 1 && updatedArticle;
            }
        }

        public bool UpdateArticleSection(ArticleSectionModel articleSectionModel)
        {
            string sql = @"update articlesection set "
                         + " text = @text, "
                         + " imageid = @imageid, "
                         + " sort = @sort, "
                         + " layout = @layout "
                         + "where id = @id ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@text", articleSectionModel.Text);
                cmd.AddParameter("@imageid", articleSectionModel.ImageId);
                cmd.AddParameter("@sort", articleSectionModel.SortId);
                cmd.AddParameter("@layout", articleSectionModel.Layout);
                cmd.AddParameter("@id", articleSectionModel.ID);
                int numRows = cmd.ExecuteNonQuery();
                var updatedArticle = UpdatedArticle(articleSectionModel.PageId);
                return numRows == 1 && updatedArticle;
            }
        }

        public bool DeleteArticleSection(ArticleSectionModel articleSectionModel)
        {
            string sql = @"delete from articlesection "
                         + "where id = @id "
                         + "and pageId = @pageId";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@id", articleSectionModel.ID);
                cmd.AddParameter("@PageId", articleSectionModel.PageId);
                int numRows = cmd.ExecuteNonQuery();
                var updatedArticle = UpdatedArticle(articleSectionModel.PageId);
                return numRows == 1 && updatedArticle;
            }
        }

        public bool ChangeSectionLayout(int articleId, int layout)
        {
            string sql = @"update articlesection set "
                         + " layout = @layout "
                         + "where id = @id ";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@layout", layout);
                cmd.AddParameter("@id", articleId);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }


        public bool AddImage(int pageId, uint imageId)
        {
            var sql = @"update content set imageid = @imageid, last_updated = now() where id = @pageid "; 
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                cmd.AddParameter("@imageid", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return numRows == 1;
            }
        }

        public bool AddImageToSection(int sectionId, uint imageId)
        {
            var sql = @"update articlesection set imageid = @imageid where id = @sectionid "; 
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@sectionid", sectionId);
                cmd.AddParameter("@imageid", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }


        public bool ChangeAccess(int pageId, int accessLevel)
        {
            var sql = @"update content set published = @accessLevel, last_updated = now() where id = @pageid "; 
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
