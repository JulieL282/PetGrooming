using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;

// Create Database and Tables
namespace PetGrooming.DAL
{
    public static class Database
    {
        private const string DbFile = "PetGrooming.db";
        private const string ConnStr = "Data Source = PetGrooming.db";

        public static string ConnectionString => ConnStr;

        public static void Initialize()
        {
            if (!File.Exists(DbFile))
            {
                SqliteConnection.CreateFile(DbFile);
            }

            //close automatically
            using var conn = new SqliteConnection(ConnStr);

            // Open the connection and save database
            conn.Open();

            using var cmd = conn.CreateCommand();

            // Customers Table (CRUD)
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Customers (
                    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
                    OwnerName TEXT NOT NULL,
                    PhoneNumber TEXT,
                    Email TEXT
                );";

                //No retrun
                cmd.ExecuteNonQuery();

            // Pets Table
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Pets (
                    PetId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    PetName TEXT NOT NULL,
                    Breed TEXT,
                    Age INTEGER,
                    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
                );";

               cmd.ExecuteNonQuery();

            // Appointments Table
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Appointments (
                    AppointmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    PetId INTEGER NOT NULL,
                    AppointmentDate TEXT NOT NULL,
                    GroomerName TEXT,
                    Price REAL,
                    Service TEXT,
                    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
                    FOREIGN KEY (PetId) REFERENCES Pets(PetId)
                );";
                cmd.ExecuteNonQuery();
        }
    }
}
