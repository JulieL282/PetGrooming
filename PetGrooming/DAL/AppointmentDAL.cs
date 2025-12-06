using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public class AppointmentDAL
    {
        private readonly string _conn = Database.ConnectionString;

        public void Insert(Appointment a)
        {
            using var conn = new SqliteConnection(_conn);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Appointments (CustomerId, PetId, AppointmentDate, Service)
                VALUES (@cid, @pid, @appdate, @service);
            ";
            cmd.Parameters.AddWithValue("@cid", a.CustomerId);
            cmd.Parameters.AddWithValue("@pid", a.PetId);
            cmd.Parameters.AddWithValue("@appdate", a.AppointmentDate.ToString("s")); // Make it Sortable
            cmd.Parameters.AddWithValue("@service", a.Service);
            cmd.ExecuteNonQuery();
        }

        public void Update(Appointment a)
        {
            using var conn = new SqliteConnection(_conn);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Appointments
                SET CustomerId = @cid,
                    PetId = @pid,
                    AppointmentDate = @appdate,
                    GroomerName = @groomer,
                    Price = @price,
                    Service = @service
                WHERE AppointmentId = @aid;
            ";
            cmd.Parameters.AddWithValue("@cid", a.CustomerId);
            cmd.Parameters.AddWithValue("@pid", a.PetId);
            cmd.Parameters.AddWithValue("@appdate", a.AppointmentDate);
            cmd.Parameters.AddWithValue("@groomer", a.GroomerName);
            cmd.Parameters.AddWithValue("@price", a.Price);
            cmd.Parameters.AddWithValue("@service", a.Service);
            cmd.Parameters.AddWithValue("@aid", a.AppointmentId);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int appointmentId)
        {
            using var conn = new SqliteConnection(_conn);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM Appointments
                WHERE AppointmentId = @aid;
            ";
            cmd.Parameters.AddWithValue("@aid", appointmentId);
            cmd.ExecuteNonQuery();
        }

        public List<Appointment> GetAll()
        {
            var appList = new List<Appointment>();
            using var conn = new SqliteConnection(_conn);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT *
                FROM Appointments;
            ";
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var appointment = new Appointment
                {
                    AppointmentId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    PetId = reader.GetInt32(2),
                    AppointmentDate = reader.GetDateTime(3),
                    GroomerName = reader.GetString(4),
                    Price = reader.GetDecimal(5),
                    Service = reader.GetString(6)
                };
                appList.Add(appointment);
            }
            return appList;
        }

        public Appointment? GetById(int appointmentId)
        {
            using var conn = new SqliteConnection(_conn);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT *
                FROM Appointments
                WHERE AppointmentId = @aid;
            ";
            cmd.Parameters.AddWithValue("@aid", appointmentId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Appointment
                {
                    AppointmentId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    PetId = reader.GetInt32(2),
                    AppointmentDate = reader.GetDateTime(3),
                    GroomerName = reader.GetString(4),
                    Price = reader.GetDecimal(5),
                    Service = reader.GetString(6)
                };
            }
            return null;
        }
    }
}
