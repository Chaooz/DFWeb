using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.Drawing;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Diagnostics;

using Org.BouncyCastle.Security;

using DFCommonLib.DataAccess;

using DFWeb.BE.Models;

namespace DFWeb.BE.Repository
{
    public interface IImageRepository
    {
        uint UploadImage(int pageId, String filename, byte[] data);
        bool DeleteImage(int imageId);
        ImageModel GetImage(int imageId);
        byte[] GetRawImage(int imageId);
        List<ImageModel> GetImages(int imagesPrPage, int pageNumber);
        bool UpdateImage(int imageId, string filename);
        bool UpdateImageData(int imageId, String filename, byte[] data);
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

        public byte[] GetRawImage(int imageId)
        {
            string sql = @"select data, length(data) as datalen from images where id = @imageid ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imageid", imageId);
             
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int dataSize = Convert.ToInt32(reader["datalen"]);

                        // Hardcap the filesize to 5 mb
                        if ( dataSize > 5000000 )
                        {
                            return null;
                        }

                        var fileData = new byte[dataSize];
                        int index = reader.GetOrdinal("data");
                        long numBytes = reader.GetBytes(index, 0, fileData, 0, dataSize);

                        return fileData;
                    }
                }
            }
            return null;
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

        public List<ImageModel> GetImages(int imagesPrPage, int pageNumber)
        {
            List<ImageModel> model = new List<ImageModel>();
            string sql = @"select id, filename, data, length(data) as datalen from images limit @imagesPrPage offset @pageNumber ";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@imagesPrPage", imagesPrPage);
                cmd.AddParameter("@pageNumber", pageNumber * imagesPrPage);

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

        public bool UpdateImageData(int imageId, String filename, byte[] data)
        {
            string sql = @"update images set data = @data where id = @imageId";
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddClobParameter("@data", data);
                cmd.AddParameter("@imageId", imageId);
                int numRows = cmd.ExecuteNonQuery();
                return numRows == 1;
            }
        }
    }
}
