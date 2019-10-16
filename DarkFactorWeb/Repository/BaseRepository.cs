using System;
using DarkFactorCoreNet.Source.Database;

namespace DarkFactorCoreNet.Repository
{
    public class BaseRepository
    {
        private string username;
        private string password;
        private string server;
        private int port;
        private string schema;

        public BaseRepository()
        {
            server = "127.0.0.1";
            port = 5306;
            username = "webuser";
            password = "secretpwd";
            schema = "dfweb";
        }

        protected DFDataBase GetOrCreateDatabase()
        {
            DFDataBase db = DFDataBase.GetInstance();

            if ( !db.IsConnected() )
            {
                db.Connect(server, port, schema, username,password);
            }

            return db;
        }
    }
}
