using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public interface IAppointmentBLL
    {
        void Create(Appointment a);
        void Update(Appointment a);
        void Delete(int appointmentId);

        List<Appointment> GetAllWithNames();
        Appointment? GetById(int appointmentId);

        // Sorting
        List<Appointment> SortByDate();
        List<Appointment> SortByOwnerName();
        List<Appointment> SortByPetName();

        //Searching
        Appointment? SearchByAppointmentId(int appointmentId);
        List<Appointment> SearchByOwnerName(string ownerName);
        List<Appointment> SearchByPetName(string petName);
    }
}
