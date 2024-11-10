using DFCommonLib.DataAccess;
using DFCommonLib.Logger;

namespace DFWeb.BE.Repository
{
    public class DFWebDatabasePatcher : StartupDatabasePatcher
    {
        private static string PATCHER = "DFWebDatabasePatcher";

        public DFWebDatabasePatcher(IDBPatcher dbPatcher) : base(dbPatcher)
        {
        }

        public override bool RunPatcher()
        {
            base.RunPatcher();

            // Admin accounts
            _dbPatcher.Patch(PATCHER,2, "CREATE TABLE `users` ("
            + " `id` int(11) NOT NULL AUTO_INCREMENT, " 
            + " `username` varchar(100) NOT NULL DEFAULT '', "
            + " `acl` int(11) NOT NULL DEFAULT 0,"
            + " PRIMARY KEY (`id`)"
            + ")"
            );

            // Article sections
            _dbPatcher.Patch(PATCHER,3, "CREATE TABLE `articlesection` ("
            + " `id` int(11) NOT NULL AUTO_INCREMENT, " 
            + " `pageid` int(11) NOT NULL, "
            + " `text` text NOT NULL,"
            + " `imageid` int(11) NOT NULL,"
            + " `sort` int(11) NOT NULL,"
            + " `layout` int(11) NOT NULL,"
            + " PRIMARY KEY (`id`)"
            + ")"
            );

            // Last_Updated
            _dbPatcher.Patch(PATCHER,4, "ALTER TABLE `content` "
            + " ADD `last_updated` datetime " 
            );

            // Last_Updated
            _dbPatcher.Patch(PATCHER,5, "ALTER TABLE `content` "
            + " ADD `tags` varchar(100) NOT NULL DEFAULT '', " 
            + " ADD `related_tags` varchar(100) NOT NULL DEFAULT '' " 
            );

            // Main page
            _dbPatcher.Patch(PATCHER,6, "ALTER TABLE `content` "
            + " ADD `main_page` int(11) NOT NULL DEFAULT 0 " 
            );

            return _dbPatcher.Successful();
        }
    }
}