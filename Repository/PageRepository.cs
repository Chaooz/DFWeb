using System;
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

            string sql = string.Format("select id, parentid, menuname, content, sort, published from content where id = @id");

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

        public List<PageContentModel> GetPageList(int parentId)
        {
            var database = base.GetOrCreateDatabase();

            string sql = string.Format("select id, parentid, menuname, content, sort, published from content where parentid = @parentid");

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

        private PageContentModel ReadPage(DFStatement statement)
        {
            int id = statement.ReadUInt32("id");
            int parentId = statement.ReadUInt32("parentid");
            string title = statement.ReadString("menuname");
            string content = statement.ReadString("content");
            int sortId = statement.ReadUInt32("sort");
            bool published = statement.ReadUInt32("published") == 1;

            // Transition hack for now
            if (content != null)
            {
                content = content.Replace("\"/img/", "\"http://www.darkfactor.net/img/");
            }

            return new PageContentModel()
            {
                ID = id,
                ParentId = parentId,
                Title = title,
                Content = content,
                HtmlContent = new HtmlString(content),
                SortId = sortId,
                IsPublished = published
            };
        }

        public bool SavePage(PageContentModel pageModel)
        {
            int isPublised = (pageModel.IsPublished) ? 1 : 0;

            string sql = @"update content set parentid=@parentid, menuname=@menuname, content = @content, sort=@sort, published = @published where id = @id ";

            var variables = DFDataBase.CreateVariables();
            variables.Add("@parentid", pageModel.ParentId);
            variables.Add("@menuname", pageModel.Title);
            variables.Add("@content", pageModel.Content);
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
            var childList = GetPageList(pageId);
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
            List<PageContentModel> pageList = GetPageList(parentPageId);
            PageContentModel page = pageList.OrderBy(x => x.SortId).LastOrDefault();
            int sortId = 0;
            if ( page != null )
            {
                sortId = page.SortId;
            }

            // Create page in database
            var database = base.GetOrCreateDatabase();
            string insertSql = @"insert into content(parentid,menuname,sort,content,published,externurl) values(@parentid,@title, @sortid, null, false, null) ";
            var insertVariables = DFDataBase.CreateVariables();
            insertVariables.Add("@parentid", parentPageId);
            insertVariables.Add("@title", @"new page");
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

            var pageList = GetPageList(page.ParentId);
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

            var pageList = GetPageList(page.ParentId);
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
