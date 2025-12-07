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
                VALUES (@ownername, @phoneno, @email);
                ";
                cmd.Parameters.AddWithValue("@ownername", c.OwnerName);
                cmd.Parameters.AddWithValue("@phoneno", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error inserting customer: " + ex);
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
                SET OwnerName = @ownername,
                    PhoneNumber = @phoneno,
                    Email = @email
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@ownername", c.OwnerName);
                cmd.Parameters.AddWithValue("@phoneno", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.Parameters.AddWithValue("@cid", c.CustomerId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating customer: " + ex);
            }
            
        }

        public void Delete(int customerId)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();


                // Delete Appointment for customerid - Foreign Key
                cmd.CommandText = @"
                DELETE FROM Appointments
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.ExecuteNonQuery();


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
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting customer: " + ex);
            }
        }

        //updated list to return all customers
        public List<Customer> GetAll()
        {
            try
            {
                var custlist = new List<Customer>();
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
                    var customer = new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        OwnerName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        PhoneNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    };
                    custlist.Add(customer);
                }
                return custlist;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving customers: " + ex);
            }    
        }

        public Customer? GetById(int customerId)
        {
            try
            {
                Customer? customer = null;
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                SELECT CustomerId, OwnerName, PhoneNumber, Email
                FROM Customers
                WHERE CustomerId = @cid;
                ";
                cmd.Parameters.AddWithValue("@cid", customerId);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        OwnerName = reader.GetString(1),
                        PhoneNumber = reader.GetString(2),
                        Email = reader.GetString(3)
                    };
                }
                return customer;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving customer: " + ex);
            }
           
        }

    }
}
