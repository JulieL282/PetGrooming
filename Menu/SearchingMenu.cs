using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.BLL;
using PetGrooming.Models;

namespace PetGrooming.Menu
{
    public static class SearchingMenu
    {
        public static void Show(IAppointmentBLL abll)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Search Appointments by ===");
                Console.WriteLine("1. Appointment ID");
                Console.WriteLine("2. Customer ID");
                Console.WriteLine("3. Pet ID");
                Console.WriteLine("4. Appointment Date");
                Console.WriteLine("9. Back to Main Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease select an option: ");

                string? input = Console.ReadLine();

                if (input == "9") return;
                if (input == "0")
                {
                    Console.WriteLine("Exiting the system. Goodbye\n");
                    Environment.Exit(0);
                }

                switch (input)
                {
                    case "1":
                        SearchByAppointmentId(abll);
                        break;

                    case "2":
                        SearchByCustomerId(abll);
                        break;

                    case "3":
                        SearchByPetId(abll);
                        break;

                    case "4":
                        SearchByDate(abll);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Press any key to return.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private static void SearchByAppointmentId(IAppointmentBLL abll)
        {
            Console.Write("Enter Appointment ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var result = abll.SearchByAppointmentId(id);
                Console.Clear();
                if (result != null)
                    Print(result);
                else
                    Console.WriteLine("\nAppointment not found.");
            }
            else
                Console.WriteLine("\nInvalid Appointment ID.");

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }

        private static void SearchByCustomerId(IAppointmentBLL abll)
        {
            Console.Write("Enter Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                PrintList(abll.SearchByCustomerId(id));
            else
                Console.WriteLine("\nInvalid Customer ID.");

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }

        private static void SearchByPetId(IAppointmentBLL abll)
        {
            Console.Write("Enter Pet ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                PrintList(abll.SearchByPetId(id));
            else
                Console.WriteLine("\nInvalid Pet ID.");

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }

        private static void SearchByDate(IAppointmentBLL abll)
        {
            Console.Write("Enter Date (yyyy-MM-dd): ");
            string raw = Console.ReadLine() ?? "";

            if (DateTime.TryParseExact(raw, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                PrintList(abll.SearchByDate(date));
            else
                Console.WriteLine("\nInvalid date format.");

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }

        private static void Print(Appointment a)
        {
            Console.WriteLine("=== Appointment Found ===");
            Console.WriteLine($"ID:       {a.AppointmentId}");
            Console.WriteLine($"Date:     {a.AppointmentDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Owner:    {a.OwnerName}");
            Console.WriteLine($"Pet:      {a.PetName}");
            Console.WriteLine($"Service:  {a.ServiceName}");
            Console.WriteLine($"Price:    {a.Price:C}\n");
        }

        private static void PrintList(List<Appointment> list)
        {
            Console.Clear();
            Console.WriteLine("\n=== Search Results ===\n");

            if (list == null || list.Count == 0)
            {
                Console.WriteLine("\nNo results found.");
                return;
            }

            foreach (var a in list)
            {
                Console.WriteLine(
                    $"ID:{a.AppointmentId} | " +
                    $"Date:{a.AppointmentDate:yyyy-MM-dd HH:mm} | " +
                    $"Owner:{a.OwnerName} | " +
                    $"Pet:{a.PetName} | " +
                    $"Service:{a.ServiceName} | " +
                    $"Price:{a.Price:C}"
        );
            }
            Console.WriteLine("\n=== End of List ===");
        }
    }

}