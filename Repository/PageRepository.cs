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
            var database = base.GetOrCreateDatabase();

            string sql = string.Format("select id, menuname, content, published from content where id = @id");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@id", pageId);

            using (DFStatement statement = database.ExecuteSelect(sql, variables))
            {
                while (statement.ReadNext())
                {
                    int id = statement.ReadUInt32("id");
                    string title = statement.ReadString("menuname");
                    string content = statement.ReadString("content");
                    bool published = statement.ReadUInt32("published") == 1;

                    // Transition hack for now
                    if ( content != null )
                    {
                        content = content.Replace("\"/img/", "\"http://www.darkfactor.net/img/");
                    }

                    return new PageContentModel()
                    {
                        ID = id,
                        Title = title,
                        Content = content,
                        HtmlContent = new HtmlString(content),
                        IsPublished = published
                    };
                }
                return null;
            }
        }

        public bool SavePage(PageContentModel pageModel)
        {
            int isPublised = (pageModel.IsPublished) ? 1 : 0;

            string sql = @"update content set menuname=@menuname, content = @content, published = @published where id = @id ";

            var variables = DFDataBase.CreateVariables();
            variables.Add("@menuname", pageModel.Title);
            variables.Add("@content", pageModel.Content);
            variables.Add("@published", isPublised);
            variables.Add("@id", pageModel.ID);

            int updatedRows = base.GetOrCreateDatabase().ExecuteUpdate(sql, variables);
            return ( updatedRows == 1);
        }
    }
}
