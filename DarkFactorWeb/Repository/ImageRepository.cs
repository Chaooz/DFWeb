using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using DarkFactorCoreNet.Models;
using DFCommonLib.DataAccess;
using Org.BouncyCastle.Security;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace DarkFactorCoreNet.Repository
{
    public interface IImageRepository
    {
        List<ImageModel> GetImageList(int pageId);

        bool AddImage(int pageId, String filename, byte[] data);
        bool DeleteImage(int imageId);
        string GetImage(int imageId);
    }

    public class ImageRepository : IImageRepository
    {
        private IDbConnectionFactory _connection;

        public ImageRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public bool AddImage(int pageId, String filename, byte[] data)
        {
            string sql = @"insert into images(pageId,filename,data, uploadeddate) values(@pageId,@filename, @data, now()) ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageId", pageId);
                cmd.AddParameter("@filename", filename);
                cmd.AddClobParameter("@data", data);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public bool DeleteImage(int imageId)
        {
            string sql = @"delete from images where id = @imageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imageid", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return (numRows == 1);
            }
        }

        public string GetImage(int imageId)
        {
            string sql = @"select data from images where id = @imageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imageid", imageId);
             
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int dataSize = 100000;
                        var fileData = new byte[dataSize];
                        int index = reader.GetOrdinal("data");
                        long numBytes = reader.GetBytes(index, 0, fileData, 0, dataSize);

                        return Convert.ToBase64String(fileData);
                    }
                }
            }
            return null;
        }

        private byte[] ReadArrayBlob(System.Data.IDataReader reader, string fieldName)
        {
            // Temp block to read data
            int blockSize = 10000;
            var blockData = new byte[blockSize];

            // Index of the field
            int fieldIndex = reader.GetOrdinal("data");

            byte[] outData = null;
            long dataRead = 1;
            int startIndex = 0;

            while (dataRead > 0)
            {
                dataRead = reader.GetBytes(fieldIndex, startIndex, blockData, 0, blockSize);
                
                // Increase the block size
                var newOutblock = new byte[startIndex + dataRead];
                if ( outData != null )
                {
                    Array.Copy(outData, newOutblock, outData.Length);
                }
                outData = newOutblock;

                // Copy the data
                Array.Copy(blockData, 0, outData, startIndex, dataRead);
            }
            return outData;
        }

        public List<ImageModel> GetImageList(int pageId)
        {
            List<ImageModel> imageList = new List<ImageModel>();

            string sql = string.Format("select id, filename, data, length(data) as dlen from images where pageid=@pageid" );
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageid", pageId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ImageModel imageModel = new ImageModel();
                        imageModel.Id = Convert.ToInt32(reader["id"]);
                        imageModel.Filename = reader["filename"].ToString();

//                        var blockData = ReadArrayBlob(reader, "data");

                        int index = reader.GetOrdinal("data");
                        int dataSize = reader.GetInt32(reader.GetOrdinal("dlen"));
                        var blockData = new byte[dataSize];
                        reader.GetBytes(index, 0, blockData, 0, dataSize);

                        imageModel.Filedata = Convert.ToBase64String(blockData);

                        imageList.Add(imageModel);
                    }
                }
            }
            return imageList;
        }

    }
}
