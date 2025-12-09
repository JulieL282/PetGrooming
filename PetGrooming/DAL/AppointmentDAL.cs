using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private readonly string _conn = Database.ConnectionString;
        public void Insert(Appointment a)
        {
            try
            {
                using var conn = new SqliteConnection(_conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Appointments (CustomerId, PetId, AppointmentDate, GroomerName, Price, Service)
                VALUES (@cid, @pid, @appdate, @groomer, @price, @service);
                ";
                cmd.Parameters.AddWithValue("@cid", a.CustomerId);
                cmd.Parameters.AddWithValue("@pid", a.PetId);
                cmd.Parameters.AddWithValue("@appdate", a.AppointmentDate.ToString("s")); // Make it Sortable
                cmd.Parameters.AddWithValue("@groomer", a.GroomerName);
                cmd.Parameters.AddWithValue("@price", a.Price);
                cmd.Parameters.AddWithValue("@service", a.Service);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error adding appointment: ", ex);
            }

        }

        public void Update(Appointment a)
        {
            try
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
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating appointment: ", ex);
            }

        }

        public void Delete(int appointmentId)
        {
            try
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
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting appointment: " + ex);
            }

        }

        public List<Appointment> GetAll()
        {
            try
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
                    var date = reader.IsDBNull(3) ? DateTime.MinValue : DateTime.Parse(reader.GetString(3));
                    appList.Add(new Appointment
                    {
                        AppointmentId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32(1),
                        PetId = reader.GetInt32(2),
                        AppointmentDate = date,
                        GroomerName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Price = reader.IsDBNull(5) ? 0m : reader.GetDecimal(5),
                        Service = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                    });
                    
                }
                return appList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error retrieving appointment list: ", ex);
            }

        }

        public Appointment? GetById(int appointmentId)
        {
            try
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
                    var date = reader.IsDBNull(3) ? DateTime.MinValue : DateTime.Parse(reader.GetString(3));
                    return new Appointment
                    {
                        AppointmentId = reader.GetInt32(0),
                        CustomerId = reader.GetInt32(1),
                        PetId = reader.GetInt32(2),
                        AppointmentDate = date,
                        GroomerName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Price = reader.IsDBNull(5) ? 0m : reader.GetDecimal(5),
                        Service = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error retrieving appointment info by ID {appointmentId} : ", ex);

            }
        }
    }
}
