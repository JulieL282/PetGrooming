using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public interface IAppointmentDAL
    {
        void Insert(Appointment a);
        void Update(Appointment a);
        void Delete(int appointmentId);
        List<Appointment> GetAll();
        Appointment? GetById(int appointmentId);
    }
}