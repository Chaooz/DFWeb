using System;
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
            string sql = string.Format("select id, menuname, content, published from content where id = {0} ", pageId);

            DFStatement statement = base.GetOrCreateDatabase()
                .ExecuteSelect(sql);

            while (statement.ReadNext())
            {
                int id = statement.ReadUInt32("id");
                string title = statement.ReadString("menuname");
                string content = statement.ReadString("content");
                bool published = statement.ReadUInt32("published") == 1;

                return new PageContentModel()
                {
                    ID = id,
                    Title = title,
                    Content = new HtmlString(content),
                    IsPublished = published
                };
            }
            return null;
        }
    }
}
