using MySql.Data.MySqlClient;

namespace DarkFactorCoreNet.Source.Database
{
    public class DFStatement
    {
        private MySqlDataReader reader;
        private int index;

        public DFStatement(MySqlDataReader reader)
        {
            this.reader = reader;
            this.index = 0;
        }

        public bool ReadNext()
        {
            if ( this.reader != null )
            {
                return this.reader.Read();
            }
            return false;
        }

        public int ReadUInt32()
        {
            if ( this.reader != null)
            {
                return this.reader.GetInt32(index++);
            }
            return 0;
        }

        public int ReadUInt32( string columnName )
        {
            if (this.reader != null)
            {
                index++;
                return this.reader.GetInt32(columnName);
            }
            return 0;
        }

        public string ReadString(string columnName)
        {
            if (this.reader != null)
            {
                index++;
                return this.reader.GetString(columnName);
            }
            return null;
        }
    }
}
