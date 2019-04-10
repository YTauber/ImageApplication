using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Views { get; set; }
        public string Password { get; set; }
    }

    public class Manager
    {
        private string _connectionString;

        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Image> GetAllImages()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Images";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Image> Images = new List<Image>();
            while (reader.Read())
            {
                Images.Add(new Image
                {
                    Id = (int)reader["id"],
                    FileName = (string)reader["FileName"],
                    Views = (int)reader["Views"],
                    Password = (string)reader["Password"]
                });
            }
            connection.Close();
            connection.Dispose();
            return Images;
        }

        public void InsertImage(Image image)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Images VALUES (@fileName, @views, @password) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@views", image.Views);
            cmd.Parameters.AddWithValue("@password", image.Password);
            connection.Open();
            image.Id = (int)(decimal)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
        }

        public void UpdateView(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Images SET Views = Views + 1 WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

        public Image GetImageById(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Images WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Image> Images = new List<Image>();
            if (!reader.Read())
            {
                return null;
            }
            Image image = new Image
            {
                Id = (int)reader["id"],
                FileName = (string)reader["FileName"],
                Views = (int)reader["Views"],
                Password = (string)reader["Password"]

            };
            connection.Close();
            connection.Dispose();
            return image;
        }
    }
}
