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
                INSERT INTO Pets (PetName, Breed, Age, CustomerId)
                VALUES (@petname, @breed, @age, @cid);
                ";
                cmd.Parameters.AddWithValue("@petname", p.PetName);
                cmd.Parameters.AddWithValue("@breed", p.Breed);
                cmd.Parameters.AddWithValue("@age", p.Age);
                cmd.Parameters.AddWithValue("@cid", p.CustomerId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error inserting pet: " + ex);
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
                SET PetName = @petname,
                    Breed = @breed,
                    Age = @age,
                    CustomerId = @cid
                WHERE PetId = @pid;
                ";
                cmd.Parameters.AddWithValue("@petname", p.PetName);
                cmd.Parameters.AddWithValue("@breed", p.Breed);
                cmd.Parameters.AddWithValue("@age", p.Age);
                cmd.Parameters.AddWithValue("@cid", p.CustomerId);
                cmd.Parameters.AddWithValue("@pid", p.PetId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating pet: " + ex);
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
                throw new DataAccessException("Error deleting pet: " + ex);
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
                SELECT PetId, PetName, Breed, Age, CustomerId
                FROM Pets;
                ";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var pet = new Pet
                    {
                        PetId = reader.GetInt32(0),
                        PetName = reader.GetString(1),
                        Breed = reader.GetString(2),
                        Age = reader.GetInt32(3),
                        CustomerId = reader.GetInt32(4)
                    };
                    petList.Add(pet);
                }
                return pet;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving pets: " + ex);
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
                SELECT PetId, PetName, Breed, Age, CustomerId
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
                        PetName = reader.GetString(1),
                        Breed = reader.GetString(2),
                        Age = reader.GetInt32(3),
                        CustomerId = reader.GetInt32(4)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving pet by ID: " + ex);
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
                SELECT PetId, PetName, Breed, Age, CustomerId
                FROM Pets
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var pet = new Pet
                    {
                        PetId = reader.GetInt32(0),
                        PetName = reader.GetString(1),
                        Breed = reader.GetString(2),
                        Age = reader.GetInt32(3),
                        CustomerId = reader.GetInt32(4)
                    };
                    petList.Add(pet);
                }
                return petList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving pets by customer ID: " + ex);
            }
        }
    }
}
