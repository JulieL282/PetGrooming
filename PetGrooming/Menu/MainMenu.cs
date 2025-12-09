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
            var sortingMenu = new SortingMenu();
            var searchingMenu = new SearchingMenu();
            var customermenu = new CustomerMenu();
            var servicemenue = new ServiceMenu();
            
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Welcome to the Pet Grooming Service ===");
                Console.WriteLine("1. Appointment Management (Add / View/ Update / Delete");
                Console.WriteLine("2. View Sorted Appointments");
                Console.WriteLine("3. Search Appointments");
                Console.WriteLine("4. Customer Management (Add / View / Update / Delete");
                Console.WriteLine("5. Service Management (Add / View / Update / Delete");
                Console.WriteLine("0. Exit");
                Console.Write("Please select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": appointmentMenu.Manage(); break;
                    case "2": sortingMenu.Show(); break;
                    case "3": searchingMenu.Show(); break;
                    case "4": customermenu.Manage(); break;
                    case "5": servicemenue.Manage(); break;
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
