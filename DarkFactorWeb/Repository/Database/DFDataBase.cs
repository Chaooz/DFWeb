using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using MySql.Data.MySqlClient;
using DarkFactorCoreNet.ConfigModel;

// http://zetcode.com/db/mysqlcsharptutorial/
// https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings

namespace DarkFactorCoreNet.Repository.Database
{
    public interface DepricatedIDFDatabase
    {
        bool IsConnected();
        DepricatedDFStatement ExecuteSelect( string sql );
        DepricatedDFStatement ExecuteSelect(string sqlString, Dictionary<string, object> variables);
        int ExecuteUpdate(string sqlString, Dictionary<string,object> variables);
        int ExecuteInsert(string sqlString, Dictionary<string, object> variables);
        int ExecuteDelete(string sqlString, Dictionary<string, object> variables);
    }

    public class DepricatedDFDataBase : DepricatedIDFDatabase
    { 
        private MySqlConnection conn;

        private DatabaseConfig dbOptions;

        public DepricatedDFDataBase ( IOptions<DatabaseConfig> options )
        {
            conn = null;
            dbOptions = options.Value;
        }

        public bool IsConnected()
        {
            return conn != null;
        }

        public bool ReConnect()
        {
            if ( conn == null )
            {
                return Connect(dbOptions.Server,
                                dbOptions.Port,
                                dbOptions.Schema,
                                dbOptions.Username,
                                dbOptions.Password);
            }
            return true;
        }

        public bool Connect(string server, int port, string schema, string username, string password)
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
                return true;
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
            return false;
        }

        public static Dictionary<string, object> CreateVariables()
        {
            return new Dictionary<string, object>();
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
        public DepricatedDFStatement ExecuteSelect( string sql )
        {
            return ExecuteSelect(sql, null);
        }

        /************************************************************************************************
        * ExecuteSelect:
        * Select fields from a table. where clauses have to be done through bind variables
        *
        * @param  (String)                      sqlString - The db query to execute
        * @param  (Dictionary<string, object>)  variables - Optional arguments to the select statement
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/
        public DepricatedDFStatement ExecuteSelect(string sqlString, Dictionary<string, object> variables)
        {
            ReConnect();

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            // Set bound variables
            cmd.Parameters.Clear();
            if (variables != null)
            {
                foreach (KeyValuePair<string, object> entry in variables)
                {
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                }
            }

            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader != null)
            {
                return new DepricatedDFStatement(reader);
            }
            return null;
        }

        /************************************************************************************************
        * ExecuteUpdate:
        * Updates records in the database. This function will return the number of rows updated. If the
        * query is a select, it will return -1 as error.
        *
        * @param  (String)  sqlString                       - The db query to execute
        * @param  (Dictionary<string, object>) variables    - Arguments for the sql string
        * @return (int)                                     - Return the number of deleted rows
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/
        public int ExecuteUpdate(string sqlString, Dictionary<string,object> variables)
        {
            ReConnect();

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Parameters.Clear();

            foreach (KeyValuePair<string, object> entry in variables)
            {
                cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }

            return cmd.ExecuteNonQuery();
        }

        /************************************************************************************************
        * ExecuteInsert:
        * Inserts records in the database. This function will return the number of rows updated. If the
        * query is a select, it will return -1 as error.
        *
        * @param  (String)  sqlString                       - The db query to execute
        * @param  (Dictionary<string, object>) variables    - Arguments for the sql string
        * @return (int)                                     - Return the number of inserted rows
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/
        public int ExecuteInsert(string sqlString, Dictionary<string, object> variables)
        {
            ReConnect();
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Parameters.Clear();

            foreach (KeyValuePair<string, object> entry in variables)
            {
                cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }

            return cmd.ExecuteNonQuery();
        }

        /************************************************************************************************
        * ExecuteDelete:
        * Delete records in the database. This function will return the number of rows deleted. If the
        * query is a select, it will return -1 as error.
        *
        * @param  (String)  sqlString                       - The db query to execute
        * @param  (Dictionary<string, object>) variables    - Arguments for the sql string
        * @return (int)                                     - Return the number of deleted rows
        *
        * @author Thor Richard Hansen
        *************************************************************************************************/
        public int ExecuteDelete(string sqlString, Dictionary<string, object> variables)
        {
            ReConnect();
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Parameters.Clear();

            foreach (KeyValuePair<string, object> entry in variables)
            {
                cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }

            return cmd.ExecuteNonQuery();
        }
    }
}
