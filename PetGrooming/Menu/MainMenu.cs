using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.Menu
{
    internal class MainMenu
    {
        public void Show()
        {
            var appointmentMenu = new AppointmentMenu();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Welcome to the Pet Grooming Service ===");
                Console.WriteLine("1. Appointment Management (Add / View/ Update / Delete");
                Console.WriteLine("2. View Sorted Appointments");
                Console.WriteLine("3. Search Appointments");
                Console.WriteLine("0. Exit");
                Console.Write("Please select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": appointmentMenu.Manage(); break;
                    case "2": appointmentMenu.SortedView();break;
                    case "3": appointmentMenu.Search(); break;
                    case "0": Console.WriteLine("Exiting the application. Goodbye"); break;

                    default:
                        Console.WriteLine("Invalid option. Please press any key to resart.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}
