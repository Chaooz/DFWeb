using DFCommonLib.DataAccess;
using DFCommonLib.Logger;

namespace DarkFactorCoreNet.Repository
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

            return _dbPatcher.Successful();
        }
    }
}