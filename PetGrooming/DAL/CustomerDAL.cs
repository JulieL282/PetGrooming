using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly string _conn = Database.ConnectionString;

        public void Insert(Customer c)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Customers (OwnerName, PhoneNumber, Email)
                VALUES (@owner, @phone, @email);
                ";
                cmd.Parameters.AddWithValue("@owner", c.OwnerName);
                cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(c.PhoneNumber) ? DBNull.Value : (object)c.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(c.Email) ? DBNull.Value : (object)c.Email);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error adding customer info: ", ex);
            }
        }

        public void Update(Customer c)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                UPDATE Customers
                SET OwnerName = @owner,
                    PhoneNumber = @phone,
                    Email = @email
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@owner", c.OwnerName);
                cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(c.PhoneNumber) ? DBNull.Value : (object)c.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(c.Email) ? DBNull.Value : (object)c.Email);
                cmd.Parameters.AddWithValue("@cid", c.CustomerId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating customer info: ", ex);
            }

        }

        public void Delete(int customerId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var trans = conn.BeginTransaction();
                using var cmd = conn.CreateCommand();


                // Delete Appointments for customerid - Foreign Key
                //cmd.CommandText = @"
                //DELETE FROM Appointments
                //WHERE CustomerId = @cid;
                //";
                //cmd.Parameters.AddWithValue("@cid", customerId);
                //cmd.ExecuteNonQuery();


                // Pets table
                cmd.CommandText = @"
                DELETE FROM Pets
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.ExecuteNonQuery();


                // Delete from Customer table - Primary Key
                cmd.CommandText = @"
                DELETE FROM Customers
                WHERE CustomerId = @cid2;
                ";
                cmd.Parameters.AddWithValue("@cid2", customerId);
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting customer info: ", ex);
            }
        }

        public List<Customer> GetAll()
        {
            try
            {
                var custList = new List<Customer>();
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Customers;
                ";

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    custList.Add(new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        OwnerName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        PhoneNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    });
                }
                return custList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving customer list: ", ex);
            }
        }

        public Customer? GetById(int customerId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT *
                FROM Customers
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        OwnerName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        PhoneNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error retrieving customer info by ID {customerId} : ", ex);
            }

        }

    }
}