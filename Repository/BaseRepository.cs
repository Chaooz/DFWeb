using System;
using DarkFactorCoreNet.Source.Database;

namespace DarkFactorCoreNet.Repository
{
    public class BaseRepository
    {
        private string username;
        private string password;
        private string server;
        private string database;

        protected DFDataBase dataBase;

        public BaseRepository()
        {
            server = "";
            username = "";
            password = "";
            database = "";
        }

        protected DFDataBase GetOrCreateDatabase()
        {
            if (dataBase == null )
            {
                dataBase = new DFDataBase();
            }

            if ( !dataBase.IsConnected() )
            {
                dataBase.Connect(server, database, username,password);
            }

            return dataBase;
        }
    }
}
