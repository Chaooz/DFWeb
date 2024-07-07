using DFCommonLib.DataAccess;
using DFCommonLib.Logger;

namespace DarkFactorCoreNet.Repository
{
    public class DFWebDatabasePatcher : StartupDatabasePatcher
    {
        private static string PATCHER = "AccountServer";

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

            return _dbPatcher.Successful();
        }
    }
}