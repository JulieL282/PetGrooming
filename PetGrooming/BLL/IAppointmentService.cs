using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public interface IAppointmentService
    {
        //CRUD Operations
        void Create(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int appointmentId);
        List<Appointment> GetAll();
        Appointment? Find(int appointmentId);

        // Sorting
        List<Appointment> SortByDate();
        List<Appointment> SortByAppointmentId();

        // Searching
        Appointment SearchByAppointmentId(int appointmentId);
        Appointment SearchByCustomerId(int customerId);
        Appointment SearchByPetId(int petId);
        Appointment SearchByDate(DateTime AppointmentDate);
    }
}
