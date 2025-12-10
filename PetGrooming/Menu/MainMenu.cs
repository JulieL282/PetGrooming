using PetGrooming.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.Menu
{
    public class MainMenu
    {
        private readonly ICustomerBLL _cbll;
        private readonly IPetBLL _pbll;
        private readonly IServiceBLL _sbll;
        private readonly IAppointmentBLL _abll;

        public MainMenu()
        {
            _cbll = new CustomerBLL();
            _pbll = new PetBLL();
            _sbll = new ServiceBLL();
            _abll = new AppointmentBLL();
        }
        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Pet Grooming Management System ===");
                Console.WriteLine("1. Manage Customers");
                Console.WriteLine("2. Manage Pets");
                Console.WriteLine("3. Manage Services");
                Console.WriteLine("4. Manage Appointments");
                Console.WriteLine("5. Sort Appointments");
                Console.WriteLine("6. Search Appointments");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        new CustomerMenu(_cbll, _pbll).Manage();
                        break;
                    case "2":
                        new PetMenu(_pbll, _cbll).Manage();
                        break;
                    case "3":
                        new ServiceMenu(_sbll).Manage();
                        break;
                    case "4":
                        new AppointmentMenu(_abll, _cbll, _pbll, _sbll).Manage();
                        break;
                    case "5":
                        SortingMenu.Show(_abll); // static
                        break;
                    case "6":
                        SearchingMenu.Show(_abll); // static
                        break;
                    case "0": return;
                        default:
                        Console.WriteLine("Invalid option. Please press any key to try again.");
                        Console.ReadKey(true);
                        break;

                }
            }
        }
    }
}
