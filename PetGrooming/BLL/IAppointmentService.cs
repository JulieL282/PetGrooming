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
        List<Appointment> GetAll();

        // Sorting
        List<Appointment> SortByDate();
        List<Appointment> SortByCustomerName();

        // Searching
        Appointment SearchByDate(DateTime AppointmentDate);
        Appointment SearchById(int appointmentId);
        Appointment SearchByCustomerId(int customerId);
    }
}
