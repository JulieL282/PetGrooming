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
                Console.Clear();
                Console.WriteLine("=== Sort Appointments by ===");
                Console.WriteLine("1. Appointment Date");
                Console.WriteLine("2. Owner Name");
                Console.WriteLine("3. Pet Name");
                Console.WriteLine("4. Appointment ID");
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

                List<Appointment>? aList = input switch

                {
                    "1" => abll.SortByDate(),
                    "2" => abll.SortByOwnerName(),
                    "3" => abll.SortByPetName(),
                    "4" => abll.SortByAppointmentId(),
                    _ => null,
                };

                if (aList == null)
                {
                    Console.WriteLine("\nInvalid option. Press any key to try again.");
                    Console.ReadKey(true);
                    continue;
                }

                Console.Clear();
                Console.WriteLine("=== Sorted Appointments ===");
                if (aList.Count == 0)
                {
                    Console.WriteLine("No appointments found.");
                }
                else
                {
                    foreach (var a in aList)
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
                Console.WriteLine("Press Any Key to return.");
                Console.ReadKey(true);
            }

        }
        
    }
}
