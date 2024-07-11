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
    public interface IPageRepository
    {
        PageContentModel GetPage(int pageId);
        List<TeaserPageContentModel> GetPagesWithParentId(int parentId);
        List<TeaserPageContentModel> GetPagesWithTag(string tag);
        List<TeaserPageContentModel> GetNewArticles(int maxArticles);
        List<TagModel> GetTagsForPage(int pageId );
        List<String> GetRelatedTags(int pageId);
        IList<ArticleSectionModel> GetArticleSections(int pageId);
    }

    public class PageRepository : IPageRepository
    {
        private IDbConnectionFactory _connection;

        public PageRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public PageContentModel GetPage(int pageId)
        {
            var sql = @"select id, parentid, promo_title, promo_text, content_title, content_text, imageid, sort, published " +
                    "from content where id = @pageId";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageId", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PageContentModel pageContent = new PageContentModel();

                        pageContent.ID              = Convert.ToInt32(reader["id"]);
                        pageContent.ParentId        = Convert.ToInt32(reader["parentid"]);
                        pageContent.PromoTitle      = reader["promo_title"].ToString();
                        pageContent.PromoText       = reader["promo_text"].ToString();
                        pageContent.ContentTitle    = reader["content_title"].ToString();
                        pageContent.ContentText     = reader["content_text"].ToString();
                        pageContent.ImageId         = Convert.ToInt32(reader["imageid"]);
                        pageContent.SortId          = Convert.ToInt32(reader["sort"]);
                        pageContent.Acl             = Convert.ToInt32(reader["published"]);

                        return pageContent;
                    }
                }
            }
            return null;
        }

        public List<TeaserPageContentModel> GetPagesWithParentId(int parentId)
        {
            string sql = @"select id, parentid, promo_title, promo_text, content_title, content_text, imageid, sort, published " +
                        "from content where parentid = @bindVariable order by sort";
            return GetPageList(sql, parentId);
        }

        public List<TeaserPageContentModel> GetPagesWithTag(string tag)
        {
            // TODO : Remove this hackj
            var lowerTag = tag.ToLower();

            string sql = @"select c.id, c.parentid, c.promo_title, c.promo_text, c.content_title, " +
                                       "c.content_text, c.imageid, c.sort, c.published " +
                                       "from content c, contenttags, tags " +
                                       "where c.id = contenttags.contentid " +
                                       "and contenttags.tagid = tags.id " +
                                       "and tags.tag = @bindVariable";

            return GetPageList(sql, tag);
        }

        public List<TeaserPageContentModel> GetNewArticles(int maxArticles)
        {
            List<TeaserPageContentModel> pageList = new List<TeaserPageContentModel>();

            string sql = string.Format(@"select c.id, c.parentid, c.promo_title, c.promo_text, c.content_title, " +
                                       "c.content_text, c.imageid, c.sort, c.published " +
                                       "from content c " +
                                       "where last_updated is not null " +
                                       "order by last_updated desc " +
                                       "limit {0}", maxArticles);

            return GetPageList(sql, null);
        }


        private List<TeaserPageContentModel> GetPageList(string sql, int bindVariable)
        {
            return GetPageList(sql, bindVariable.ToString());
        }

        private List<TeaserPageContentModel> GetPageList(string sql, string bindVariable)
        {
            List<TeaserPageContentModel> pageList = new List<TeaserPageContentModel>();
            
            using (var cmd = _connection.CreateCommand(sql))
            {
                if ( !string.IsNullOrEmpty( bindVariable ))
                {
                    cmd.AddParameter("@bindVariable", bindVariable);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TeaserPageContentModel pageContent = new TeaserPageContentModel();

                        pageContent.ID              = Convert.ToInt32(reader["id"]);
                        pageContent.ParentId        = Convert.ToInt32(reader["parentid"]);
                        pageContent.PromoTitle      = reader["promo_title"].ToString();
                        pageContent.PromoText       = reader["promo_text"].ToString();
                        pageContent.ContentTitle    = reader["content_title"].ToString();
                        pageContent.ImageId         = Convert.ToInt32(reader["imageid"]);
                        pageContent.SortId          = Convert.ToInt32(reader["sort"]);
                        pageContent.Acl             = Convert.ToInt32(reader["published"]);
                        pageList.Add(pageContent);
                    }
                }
            }
            return pageList;
        }

        public List<String> GetRelatedTags(int pageId)
        {
            List<String> tagList = new List<String>();

            string sql = string.Format("select t.tag " +
                                       "from tags t, relatedtags rt " +
                                       "where rt.contentid = @pageid " +
                                       "and rt.tagid = t.id " );

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string tag = reader["tag"].ToString();
                        tagList.Add(tag);
                    }
                }
            }
            return tagList;
        }

        public List<TagModel> GetTagsForPage( int pageId )
        {
            List<TagModel> tagList = new List<TagModel>();
            string sql = string.Format("select t.id, t.tag " +
                            "from tags t, contenttags ct " +
                            "where ct.tagid = t.id " +
                            "and ct.contentid = @pageId " );

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TagModel tag = new TagModel()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["name"].ToString()
                        };
                        tagList.Add(tag);
                    }
                }
            }
            return tagList;
        }

        public IList<ArticleSectionModel> GetArticleSections(int pageId)
        {
            IList<ArticleSectionModel> list = new List<ArticleSectionModel>();

            var sql = @"select id, pageid,text, imageid, sort, layout " +
                    "from articlesection where pageid = @pageId order by sort";

            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArticleSectionModel articleSection = new ArticleSectionModel()
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            PageId = Convert.ToInt32(reader["pageid"]),
                            Text = reader["text"].ToString(),
                            ImageId = Convert.ToInt32(reader["imageid"]),
                            SortId = Convert.ToInt32(reader["sort"]),
                            Layout = Convert.ToInt32(reader["layout"])
                        };
                        list.Add(articleSection);
                    }
                }
            }
            return list;
        }
    }
}
