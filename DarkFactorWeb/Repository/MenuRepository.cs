using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DFCommonLib.DataAccess;

namespace DarkFactorCoreNet.Repository
{
    public interface IMenuRepository
    {
        List<MenuItem> GetAllItems();
    }

    public class MenuRepository : IMenuRepository
    {
        private IDbConnectionFactory _connection;

        public MenuRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<MenuItem> GetAllItems()
        {
            List<MenuItem> itemList = new List<MenuItem>();

            var sql = @"select id, parentid, content_title, published from content where main_page = 0 order by sort";
            using (var cmd = _connection.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MenuItem menuItem = new MenuItem();

                        menuItem.ID         = Convert.ToInt32(reader["id"]);
                        menuItem.ParentID   = Convert.ToInt32(reader["parentid"]);
                        menuItem.Name       = reader["content_title"].ToString();
                        menuItem.IsPublished= Convert.ToUInt32(reader["published"]) == 1;

                        itemList.Add(menuItem);
                    }
                }
            }

            return itemList;
        }
    }
}
