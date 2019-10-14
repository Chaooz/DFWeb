using MySql.Data.MySqlClient;
using System;

namespace DarkFactorCoreNet.Source.Database
{
    public class DFStatement : IDisposable
    {
        // Track whether Dispose has been called.
        private bool disposed = false;

        private MySqlDataReader reader;
        private int index;

        public DFStatement(MySqlDataReader reader)
        {
            this.reader = reader;
            this.index = 0;
        }

        ~DFStatement()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (this.reader != null)
                    {
                        this.reader.Close();
                        this.reader = null;
                    }
                }

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

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
                index = this.reader.GetOrdinal(columnName);
                if (!this.reader.IsDBNull(index))
                {
                    return this.reader.GetInt32(index++);
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
            try
            {
                if (this.reader != null)
                {
                    index = this.reader.GetOrdinal(columnName);
                    if (!this.reader.IsDBNull(index))
                    {
                        return this.reader.GetString(index);
                    }
                }
            }
            catch(Exception)
            {
                return null;
            }
            return null;
        }
    }
}
