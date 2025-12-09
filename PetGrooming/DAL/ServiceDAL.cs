using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public class ServiceDAL : IServiceDAL
    {
        private readonly string _conn = Database.ConnectionString;

        public void Insert(Service s)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Services (ServiceName, Price)
                VALUES (@sname, @price);
                ";
                cmd.Parameters.AddWithValue("@sname", s.ServiceName);
                cmd.Parameters.AddWithValue("@price", s.Price);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error adding service info: ", ex);
            }
        }

        public void Update(Service s)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                UPDATE Services
                SET ServiceName = @sname,
                    Price = @price
                WHERE ServiceId = @sid;
                ";
                cmd.Parameters.AddWithValue("@sname", s.ServiceName);
                cmd.Parameters.AddWithValue("@price", s.Price);
                cmd.Parameters.AddWithValue("@sid", s.ServiceId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating service info: ", ex);
            }
        }

        public void Delete(int serviceId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                DELETE FROM Services
                WHERE ServiceId = @sid;
                ";
                cmd.Parameters.AddWithValue("@sid", serviceId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting service info: ", ex);
            }
        }

        public List<Service> GetAll()
        {
            
            try
            {
                var sList = new List<Service>();
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Services;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sList.Add(new Service
                    {
                        ServiceId = reader.GetInt32(0),
                        ServiceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2)
                    });
                }
                return sList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving service list: ", ex);
            }
        }

        public Service? GetById(int serviceId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Services
                WHERE ServiceId = @sid;
                ";
                cmd.Parameters.AddWithValue("@sid", serviceId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Service
                    {
                        ServiceId = reader.GetInt32(0),
                        ServiceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error retrieving service by ID {serviceId} : ", ex);
            }
        }
    }
}
