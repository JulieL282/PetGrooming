using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite; // install SQLite
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
                File.Create(DbFile).Close();
            }

            //close automatically
            using var conn = new SqliteConnection(ConnStr);

            // Open the connection and save database
            conn.Open();

            using var cmd = conn.CreateCommand();

            // Customers Table
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Customers (
                    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
                    OwnerName TEXT NOT NULL,
                    PhoneNumber TEXT,
                    Email TEXT
                );";
            //No return
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

            // Services Table
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Services (
                    ServiceId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ServiceName TEXT NOT NULL UNIQUE,
                    BasePrice REAL NOT NULL
                );";
            cmd.ExecuteNonQuery();


            // Appointments Table
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Appointments (
                    AppointmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    PetId INTEGER NOT NULL,
                    ServiceId INTEGER NOT NULL,
                    AppointmentDate TEXT NOT NULL,
                    GroomerName TEXT,
                    Price REAL,
                    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
                    FOREIGN KEY (PetId) REFERENCES Pets(PetId),
                    FOREIGN KEY (ServiceId) REFERENCES Services(ServiceId)
                );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "Select Count(*) From Services;";
            var count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
            if (count == 0)
            {
                // Seed initial services
                cmd.CommandText = @"
                    INSERT INTO Services (ServiceName, BasePrice) VALUES
                    ('Full Grooming', 50.00),
                    ('Bath', 30.00),
                    ('Hair Cut', 20.00),
                    ('Nail Trimming', 15.00);
                ";
                cmd.ExecuteNonQuery();
            }
        }


    }
}