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
                Console.WriteLine("=== Search Appointments by ===");
                Console.WriteLine("1. Appointment ID");
                Console.WriteLine("2. Customer ID");
                Console.WriteLine("3. Pet ID");
                Console.WriteLine("4. Appointment Date");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Enter Appointment ID: ");
                        int appId = int.Parse(Console.ReadLine()!);

                        var result = abll.SearchByAppointmentId(appId);
                        if (result != null)
                        {
                            Print(result);
                        }
                        else
                        {
                            Console.WriteLine("Appointment not found.");
                        }
                        break;
                    case "2":
                        Console.Write("Enter Customer ID: ");
                        int custId = int.Parse(Console.ReadLine()!);
                        PrintList(abll.SearchByCustomerId(custId));
                        break;
                    case "3":
                        Console.Write("Enter Pet ID: ");
                        int petId = int.Parse(Console.ReadLine()!);
                        PrintList(abll.SearchByPetId(petId));
                        break;
                    case "4":
                        Console.Write("Enter Appointment Date (yyyy-MM-dd): ");
                        DateTime date = DateTime.Parse(Console.ReadLine()!);
                        PrintList(abll.SearchByDate(date));
                        break;
                    case "0": return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        private static void Print(Appointment a)
        {
            Console.Clear();
            Console.WriteLine("=== Appointment Found ===");
            Console.WriteLine($"ID: {a.AppointmentId}, Date: {a.AppointmentDate}, Owner: {a.OwnerName}, Pet: {a.PetName}, Service: {a.ServiceName}, Price: {a.Price}");

        }
        private static void PrintList(List<Appointment> aList)
        {
            Console.Clear();
            Console.WriteLine("=== Search Results ===");
            foreach (var a in aList)
            {
                Print(a);
            }
        }

    }
}
