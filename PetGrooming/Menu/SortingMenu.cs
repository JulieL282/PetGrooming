using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.BLL;

namespace PetGrooming.Menu
{
    public class SortingMenu
    {
        private readonly IAppointmentBLL _app;
        public SortingMenu(IAppointmentBLL? app = null)
        {
            _app = app ?? new AppointmentBLL();
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Sorting Appointments by ===");
            Console.WriteLine("1. Appointment Date");
            Console.WriteLine("2. Owner Name");
            Console.WriteLine("3. Pet Name");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Select an option: ");

            var input = Console.ReadLine();
            var list = input switch
            {
                "1" => _app.SortByDate(),
                "2" => _app.SortByOwnerName(),
                "3" => _app.SortByPetName(),
                "0" => null,
                _ => null
            };

            foreach (var appointment in list)
            {
                Console.WriteLine($"ID: {appointment.AppointmentId}, Date: {appointment.AppointmentDate}, Owner: {appointment.OwnerName}, Pet: {appointment.PetName}");
                Console.WriteLine("Press Any Key to Return to Sorting Menu");
                Console.ReadKey(true);
            }
        }
    }
}
