using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using DarkFactorCoreNet.Models;
using DFCommonLib.DataAccess;
using Org.BouncyCastle.Security;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Drawing;

namespace DarkFactorCoreNet.Repository
{
    public interface IImageRepository
    {
        bool AddImage(int pageId, String filename, byte[] data);
        bool DeleteImage(int imageId);
        ImageModel GetImage(int imageId);
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

        public ImageModel GetImage(int imageId)
        {
            string sql = @"select filename, data from images where id = @imageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imageid", imageId);
             
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string fileName = reader["filename"].ToString();

                        int dataSize = 100000;
                        var fileData = new byte[dataSize];
                        int index = reader.GetOrdinal("data");
                        long numBytes = reader.GetBytes(index, 0, fileData, 0, dataSize);

                        ImageModel model = new ImageModel()
                        {
                            Id = imageId,
                            Filename = fileName,
                            Filedata = Convert.ToBase64String(fileData)
                        };
                        return model;
                    }
                }
            }
            return null;
        }
    }
}
