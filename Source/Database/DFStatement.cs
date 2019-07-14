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
            this.index = 0;
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
                if (!this.reader.IsDBNull(index))
                {
                    return this.reader.GetInt32(index++);
                }
            }
            index++;
            return 0;
        }

        public int ReadUInt32( string columnName )
        {
            if (this.reader != null)
            {
                if (!this.reader.IsDBNull(index))
                {
                    index++;
                    return this.reader.GetInt32(columnName);
                }
            }
            index++;
            return 0;
        }

        public string ReadString()
        {
            if (this.reader != null)
            {
                if (!this.reader.IsDBNull(index))
                {
                    return this.reader.GetString(index++);
                }
            }
            index++;
            return null;
        }

        public string ReadString(string columnName)
        {
            if (this.reader != null)
            {
                if ( !this.reader.IsDBNull(index))
                {
                    index++;
                    return this.reader.GetString(columnName);
                }
            }
            index++;
            return null;
        }
    }
}
