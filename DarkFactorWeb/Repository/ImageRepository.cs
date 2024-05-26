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
        uint UploadImage(int pageId, String filename, byte[] data);
        bool DeleteImage(int imageId);
        ImageModel GetImage(int imageId);
        List<ImageModel> GetImages(int maxImages);
        bool UpdateImage(int imageId, string filename);
    }

    public class ImageRepository : IImageRepository
    {
        private IDbConnectionFactory _connection;

        public ImageRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public uint UploadImage(int pageId, String filename, byte[] data)
        {
            string sql = @"insert into images(pageId,filename,data, uploadeddate) values(@pageId,@filename, @data, now()) ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@pageId", pageId);
                cmd.AddParameter("@filename", filename);
                cmd.AddClobParameter("@data", data);
                int numRows = cmd.ExecuteNonQuery();
            }
            return GetId();
        }

        private uint GetId()
        {
            var sql = @"SELECT LAST_INSERT_ID() as id";
            using (var cmd = _connection.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        uint id = Convert.ToUInt32(reader["id"]);
                        return id;
                    }
                }
            }
            return 0;
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
            string sql = @"select filename, data, length(data) as datalen from images where id = @imageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imageid", imageId);
             
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string fileName = reader["filename"].ToString();
                        int dataSize = Convert.ToInt32(reader["datalen"]);

                        // Hardcap the filesize to 5 mb
                        if ( dataSize > 5000000 )
                        {
                            return null;
                        }

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

        public List<ImageModel> GetImages(int maxImages)
        {
            List<ImageModel> model = new List<ImageModel>();
            string sql = @"select id, filename, data, length(data) as datalen from images limit @maxImages ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@maxImages", maxImages);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int imageId = Convert.ToInt32(reader["id"]);
                        string fileName = reader["filename"].ToString();
                        int dataSize = Convert.ToInt32(reader["datalen"]);

                        // Hardcap the filesize to 5 mb
                        if (dataSize > 5000000)
                        {
                            continue;
                        }

                        var fileData = new byte[dataSize];
                        int index = reader.GetOrdinal("data");
                        long numBytes = reader.GetBytes(index, 0, fileData, 0, dataSize);

                        ImageModel image = new ImageModel()
                        {
                            Id = imageId,
                            Filename = fileName,
                            Filedata = Convert.ToBase64String(fileData)
                        };
                        model.Add(image);
                    }
                }
            }
            return model;
        }

        public bool UpdateImage(int imageId, string filename)
        {
            string sql = @"update images set filename = @filename where id = @imageId ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@filename", filename);
                cmd.AddParameter("@imageId", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return numRows == 1;
            }
        }
    }
}
