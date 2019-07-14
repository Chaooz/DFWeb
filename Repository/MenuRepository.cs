using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Source.Database;

namespace DarkFactorCoreNet.Repository
{
    public class MenuRepository : BaseRepository
    {
        public MenuRepository()
        {
        }

        public List<MenuItem> GetAllItems()
        {            List<MenuItem> itemList = new List<MenuItem>();

            DFStatement statement = base.GetOrCreateDatabase()
                .ExecuteSelect("select id, parentid, menuname, published from content order by sort");

            while (statement.ReadNext())
            {
                int id          = statement.ReadUInt32("id");
                int parentId    = statement.ReadUInt32("parentid");
                string name     = statement.ReadString("menuname");
                bool published  = statement.ReadUInt32("published") == 1;

                itemList.Add(new MenuItem()
                {
                    ID = id,
                    ParentID = parentId,
                    Name = name,
                    IsPublished = published
                });
            }

            return itemList;
        }
    }
}
