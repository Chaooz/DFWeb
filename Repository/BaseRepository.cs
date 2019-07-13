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
        private string database;

        protected DFDataBase dataBase;

        public BaseRepository()
        {
            server = "127.0.0.1";
            port = 5306;
            username = "dfweb";
            password = "testpass";
            database = "dfweb";
        }

        protected DFDataBase GetOrCreateDatabase()
        {
            if (dataBase == null )
            {
                dataBase = new DFDataBase();
            }

            if ( !dataBase.IsConnected() )
            {
                dataBase.Connect(server, port, database, username,password);
            }

            return dataBase;
        }
    }
}
