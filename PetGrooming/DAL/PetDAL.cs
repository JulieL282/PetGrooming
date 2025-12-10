using Microsoft.Data.Sqlite;
using PetGrooming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.DAL
{
    public class PetDAL : IPetDAL
    {
        private readonly string _conn = Database.ConnectionString;
        public void Insert(Pet p)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Pets (CustomerId, PetName, Breed, Age)
                VALUES (@cid@petname, @breed, @age);
                ";
                cmd.Parameters.AddWithValue("@cid", p.CustomerId);
                cmd.Parameters.AddWithValue("@petname", p.PetName);
                cmd.Parameters.AddWithValue("@breed", p.Breed);
                cmd.Parameters.AddWithValue("@age", p.Age);
                
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error adding pet info: ", ex);
            }
        }

        public void Update(Pet p)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                UPDATE Pets
                SET CustomerId = @cid,
                    PetName = @petname,
                    Breed = @breed,
                    Age = @age,
                    CustomerId = @cid
                WHERE PetId = @pid;
                ";
                cmd.Parameters.AddWithValue("@cid", p.CustomerId);
                cmd.Parameters.AddWithValue("@petname", p.PetName);
                cmd.Parameters.AddWithValue("@breed", p.Breed);
                cmd.Parameters.AddWithValue("@age", p.Age);
                cmd.Parameters.AddWithValue("@pid", p.PetId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating pet info: ", ex);
            }
        }

        public void Delete(int petId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM Pets
                    WHERE PetId = @pid;
                    ";
                cmd.Parameters.AddWithValue("@pid", petId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting pet info: ", ex);
            }
        }
        public List<Pet> GetAll()
        {
            try
            {
                var petList = new List<Pet>();
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Pets;
                ";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    petList.Add(new Pet
                    {
                        PetId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32(1),
                        PetName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Breed = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4) 
                    });
                }
                return petList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving pet list: ", ex);
            }

        }
        public Pet? GetById(int petId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Pets
                WHERE PetId = @pid;
                ";
                cmd.Parameters.AddWithValue("@pid", petId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Pet
                    {
                        PetId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32(1),
                        PetName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Breed = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error retrieving pet info by ID {petId} : ", ex);
            }
        }
        public List<Pet> GetByCustomerId(int customerId)
        {
            try
            {
                var petList = new List<Pet>();
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Pets
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    petList.Add(new Pet
                    {
                        PetId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32(1),
                        PetName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Breed = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                    });
                }
                return petList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error retrieving pet info by customer ID {customerId} : ", ex);
            }
        }
    }
}