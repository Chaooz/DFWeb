﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Source.Database;

namespace DarkFactorCoreNet.Repository
{
    public class PageRepository : BaseRepository
    {
        public PageRepository()
        {
        }

        public PageContentModel GetPage(int pageId)
        {
            var database = base.GetOrCreateDatabase();

            string sql = string.Format("select id, parentid, promo_title, promo_text, content_title, content_text, image, sort, published from content where id = @id");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@id", pageId);

            using (DFStatement statement = database.ExecuteSelect(sql, variables))
            {
                if (statement.ReadNext())
                {
                    return ReadPage(statement);
                }
                return null;
            }
        }

        public List<PageContentModel> GetPagesWithParentId(int parentId)
        {
            var database = base.GetOrCreateDatabase();

            string sql = string.Format("select id, parentid, promo_title, promo_text, content_title, content_text, image, sort, published from content where parentid = @parentid order by sort");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@parentid", parentId);

            List<PageContentModel> pageList = new List<PageContentModel>();

            using (DFStatement statement = database.ExecuteSelect(sql, variables))
            {
                while (statement.ReadNext())
                {
                    var page = ReadPage(statement);
                    pageList.Add(page);
                }
            }
            return pageList;
        }

        public List<PageContentModel> GetPagesWithTag(string tag)
        {
            var database = base.GetOrCreateDatabase();

            // Todo, change this to check against tagged fields in the document
            string sql = string.Format("select id, parentid, promo_title, promo_text, content_title, " +
                                       "content_text, image, sort, published " +
                                       "from content " +
                                       "where content_title like @tag");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@tag", tag);

            List<PageContentModel> pageList = new List<PageContentModel>();

            using (DFStatement statement = database.ExecuteSelect(sql, variables))
            {
                while (statement.ReadNext())
                {
                    var page = ReadPage(statement);
                    pageList.Add(page);
                }
            }
            return pageList;
        }

        private PageContentModel ReadPage(DFStatement statement)
        {
            int id = statement.ReadUInt32("id");
            int parentId = statement.ReadUInt32("parentid");
            string promoTitle = statement.ReadString("promo_title");
            string promoText = statement.ReadString("promo_text");
            string contentTitle = statement.ReadString("content_title");
            string contentText = statement.ReadString("content_text");
            string image = statement.ReadString("image");
            int sortId = statement.ReadUInt32("sort");
            bool published = statement.ReadUInt32("published") == 1;

            // Transition hack for now
            if (contentText != null)
            {
                contentText = contentText.Replace("\"/img/", "\"http://www.darkfactor.net/img/");
            }

            if (promoText != null)
            {
                promoText = promoText.Replace("\"/img/", "\"http://www.darkfactor.net/img/");
            }

            return new PageContentModel()
            {
                ID = id,
                ParentId = parentId,
                PromoTitle = promoTitle,
                PromoText = promoText,
                ContentTitle = contentTitle,
                ContentText = contentText,
                Image = image,
                HtmlContent = new HtmlString(contentText),
                HtmlTeaser = new HtmlString(promoText),
                SortId = sortId,
                IsPublished = published
            };
        }

        public bool SavePage(PageContentModel pageModel)
        {
            int isPublised = (pageModel.IsPublished) ? 1 : 0;

            string sql = @"update content set "
                         + " parentid=@parentid, "
                         + " promo_title = @promo_title, "
                         + " promo_text = @promo_text, "
                         + " content_title =@content_title, "
                         + " content_text = @content_text, "
                         + " image = @image, "
                         + " sort =@sort, "
                         + " published = @published "
                         + "where id = @id ";

            var variables = DFDataBase.CreateVariables();
            variables.Add("@parentid", pageModel.ParentId);
            variables.Add("@promo_title", pageModel.PromoTitle);
            variables.Add("@promo_text", pageModel.PromoText);
            variables.Add("@content_title", pageModel.ContentTitle);
            variables.Add("@content_text", pageModel.ContentText);
            variables.Add("@image", pageModel.Image);
            variables.Add("@sort", pageModel.SortId);
            variables.Add("@published", isPublised);
            variables.Add("@id", pageModel.ID);

            int updatedRows = base.GetOrCreateDatabase().ExecuteUpdate(sql, variables);
            return ( updatedRows == 1);
        }

        public int DeletePage(int pageId)
        {
            var page = GetPage(pageId);
            if (page == null)
            {
                return pageId;
            }

            // Do not delete page if it has children
            var childList = GetPagesWithParentId(pageId);
            if ( childList.Count > 0 )
            {
                return pageId;
            }

            string sql = @"delete from content where id = @id ";

            var variables = DFDataBase.CreateVariables();
            variables.Add("@id", pageId);

            base.GetOrCreateDatabase().ExecuteDelete(sql, variables);

            return page.ParentId;
        }

        public bool CreatePage( int pageId )
        {
            var page = GetPage(pageId);
            if ( page != null )
            {
                return CreateChildPage(page.ParentId);
            }
            return false;
        }

        public bool CreateChildPage( int parentPageId )
        {
            List<PageContentModel> pageList = GetPagesWithParentId(parentPageId);
            PageContentModel page = pageList.OrderBy(x => x.SortId).LastOrDefault();
            int sortId = 0;
            if ( page != null )
            {
                sortId = page.SortId;
            }

            // Create page in database
            var database = base.GetOrCreateDatabase();
            string insertSql = @"insert into content(parentid,content_title,sort,content_text,published,externurl) values(@parentid,@content_title, @sortid, null, false, null) ";
            var insertVariables = DFDataBase.CreateVariables();
            insertVariables.Add("@parentid", parentPageId);
            insertVariables.Add("@content_title", @"new page");
            insertVariables.Add("@sortid", sortId + 1);

            int insertedRows = base.GetOrCreateDatabase().ExecuteInsert(insertSql, insertVariables);
            return (insertedRows == 1);
        }

        public bool MovePageDown(int pageId)
        {
            PageContentModel page = GetPage(pageId);
            if (page == null )
            {
                return false;
            }

            var pageList = GetPagesWithParentId(page.ParentId);
            PageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId > page.SortId).FirstOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            int swapSortId = page.SortId;
            page.SortId = swapPage.SortId;
            swapPage.SortId = swapSortId;

            bool didSave1 = SavePage(page);
            bool didSave2 = SavePage(swapPage);
            return didSave1 && didSave2;
        }

        public bool MovePageUp(int pageId)
        {
            PageContentModel page = GetPage(pageId);
            if (page == null)
            {
                return false;
            }

            var pageList = GetPagesWithParentId(page.ParentId);
            PageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId < page.SortId).LastOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            int swapSortId = page.SortId;
            page.SortId = swapPage.SortId;
            swapPage.SortId = swapSortId;

            bool didSave1 = SavePage(page);
            bool didSave2 = SavePage(swapPage);
            return didSave1 && didSave2;
        }
    }
}