using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

// http://zetcode.com/db/mysqlcsharptutorial/
// https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings

namespace DarkFactorCoreNet.Source.Database
{
    public class DFDataBase
    {
        private MySqlConnection conn;

        private static DFDataBase instance = null;

        public DFDataBase()
        {
            conn = null;
        }

        public static DFDataBase GetInstance()
        {
            if ( instance == null )
            {
                instance = new DFDataBase();
            }
            return instance;
        }

        public bool IsConnected()
        {
            return conn != null;
        }

        public void Connect(string server, int port, string schema, string username, string password)
        {
            string cs = string.Format("server={0};port={1};userid={2};password={3};database={4}",
                server,
                port,
                username,
                password,
                schema);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                Console.WriteLine("MySQL version : {0}", conn.ServerVersion);
                return;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
            }
        }

        public void ShowErrorCode(MySqlException ex)
        {
            /*
              const char* what = e.what();

              LogError   ( "CoreDataBase", "###### ERR: SQLException ###############################");
              LogErrorFMT( "CoreDataBase", "File: %s ", String(__FILE__).c_str() );
              LogErrorFMT( "CoreDataBase", "Func: %s ( line %d )", __FUNCTION__, __LINE__ );
              LogErrorFMT( "CoreDataBase", "SQL : %s", sqlString.c_str());

              if (what != NULL)
              {
                LogErrorFMT("CoreDataBase", "ERR : (code:%d) %s ( State : %s )", e.getErrorCode(), e.what(), e.getSQLState().c_str());
              }
              else
              {
                LogErrorFMT("CoreDataBase", "ERR : (code:%d) ( State : %s )", e.getErrorCode(), e.getSQLState().c_str());
              }

              LogError   ( "CoreDataBase", "########################################################");

            */
        }

        /************************************************************************************************
        * ExecuteSelect:
        * Select fields from a table. where clauses have to be done through bind variables
        *
        * @param  (String)  sqlString   - The db query to execute
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/
        public DFStatement ExecuteSelect( string sql )
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            if ( cmd != null )
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null )
                {
                    return new DFStatement(reader);
                }
            }
            return null;
        }

        /************************************************************************************************
        * ExecuteDelete:
        * Delete records in the database. This function will return the number of rows deleted. If the
        * query is a select, it will return -1 as error.
        *
        * @param  (String)  sqlString   - The db query to execute
        * @param  (va_list) pArgs       - Arguments for the sql string
        * @return (int)                 - Return the number of deleted rows
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/

    }
}
