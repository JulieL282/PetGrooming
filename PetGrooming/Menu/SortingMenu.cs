using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.BLL;
using PetGrooming.Models;

namespace PetGrooming.Menu
{
    public static class SortingMenu
    {
        public static void Show(IAppointmentBLL abll)
        {
            while (true)
            {
                Console.WriteLine("=== Sort Appointments by ===");
                Console.WriteLine("1. Appointment Date");
                Console.WriteLine("2. Owner Name");
                Console.WriteLine("3. Pet Name");
                Console.WriteLine("4. Appointment ID");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string? input = Console.ReadLine();

                List<Appointment>? aList = null;

                switch (input)
                {
                    case "1":
                        aList = abll.SortByDate();
                        break;
                    case "2":
                        aList = abll.SortByOwnerName();
                        break;
                    case "3":
                        aList = abll.SortByPetName();
                        break;
                    case "4":
                        aList = abll.SortByAppointmentId();
                        break;
                    case "0":
                        return; // Exit the menu
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        continue;
                }

                Print(aList);
            }
        }
        private static void Print(List<Appointment> aList)
        {
            Console.Clear();
            Console.WriteLine("=== Sorted Appointments ===");
            foreach (var a in aList)
            {
                Console.WriteLine($"ID: {a.AppointmentId}, Date: {a.AppointmentDate}, Owner: {a.OwnerName}, Pet: {a.PetName}");
            }
            Console.WriteLine("Press Any Key to Return to Sorting Menu");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
