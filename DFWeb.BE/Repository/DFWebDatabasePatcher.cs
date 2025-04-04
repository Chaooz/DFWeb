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

            _dbPatcher.Patch(PATCHER,4, "CREATE TABLE `content` ("
            + "`id` int(11) NOT NULL AUTO_INCREMENT,"
            + "`parentid` int(11) NOT NULL DEFAULT 0,"
            + "`content_title` varchar(50) NOT NULL DEFAULT '',"
            + "`sort` int(11) NOT NULL DEFAULT 0,"
            + "`promo_title` text DEFAULT NULL,"
            + "`promo_text` text DEFAULT NULL,"
            + "`content_text` text DEFAULT NULL,"
            + "`imageId` int(11) NOT NULL,"
            + "`published` int(11) NOT NULL DEFAULT 0,"
            + "`externurl` varchar(255) DEFAULT NULL,"
            + "`last_updated` datetime DEFAULT NULL,"
            + "`tags` varchar(100) NOT NULL DEFAULT '',"
            + "`related_tags` varchar(100) NOT NULL DEFAULT '',"
            + "`main_page` int(11) NOT NULL DEFAULT 0,"
            + "PRIMARY KEY (`id`)"
            + ")"
            );

            _dbPatcher.Patch(PATCHER,5, "CREATE TABLE `contenttags` ("
                + "`contentid` int(11) NOT NULL,"
                + "`tagid` int(11) NOT NULL"
                + ")"
            );

            _dbPatcher.Patch(PATCHER,6, "CREATE TABLE `tags` ("
                + "`id` int(11) NOT NULL AUTO_INCREMENT"
                + "`tag` varchar(50) NOT NULL DEFAULT ''"
                + ")"
            );

            _dbPatcher.Patch(PATCHER,7, "CREATE TABLE `relatedtags` ("
                + "`contentid` int(11) NOT NULL,"
                + "`tagid` int(11) NOT NULL"
                + ")"
            );

            _dbPatcher.Patch(PATCHER,8, "CREATE TABLE `images` ("
                + "`id` int(11) NOT NULL AUTO_INCREMENT,"
                + "`pageid` varchar(50) NOT NULL DEFAULT '',"
                + "`filename` varchar(255) NOT NULL DEFAULT '',"
                + "`data` mediumblob DEFAULT NULL,"
                + "`uploadeddate` datetime DEFAULT NULL,"
                + "PRIMARY KEY (`id`)"
                + ")"
            );

            return _dbPatcher.Successful();
        }
    }
}