using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;
using PetGrooming.BLL;

namespace PetGrooming.Menu
{
    public class AppointmentMenu
    {
        private readonly AppointmentService _service = new();

        public void Show() // Display?
        {
            while (true)
            {
                Console.WriteLine("=== Appointment Menu  ===");
                Console.WriteLine("1. Create Appointment");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. Sort Appointments by Date");
                Console.WriteLine("4. Search Appointment by ID");
                Console.WriteLine("5. Update Appointment");
                Console.WriteLine("6. Delete Appointment");
                Console.WriteLine("0. Cancel");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Add(); break;
                    case "2": ViewAll(); break;
                    case "3": SortByDate(); break;
                    case "4": SearchById(); break;
                    case "5": Update(); break;
                    case "6": Delete(); break;
                    case "0": return;
                }
            }
        }

        private void Add()
        {
            var a = new Appointment();
            Console.Write("Enter Customer ID: ");
            a.CustomerId = int.Parse(Console.ReadLine()!);

            Console.Write("Enter Pet ID: ");
            a.PetId = int.Parse(Console.ReadLine()!);

            Console.Write("Enter Appointment Date (yyyy-MM-dd HH:mm): ");
            a.AppointmentDate = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Enter Groomer Name: ");
            a.GroomerName = Console.ReadLine()!;

            Console.Write("Enter Service: ");
            a.Service = Console.ReadLine()!;

            Console.Write("Enter Price: ");
            a.Price = decimal.Parse(Console.ReadLine()!);

            _service.Create(a);
            Console.WriteLine("Appointment created successfully.");
        }

        private void ViewAll()
        {
            var appList = _service.GetAll();

            Console.WriteLine("=== Current Appointments ===");
            foreach (var a in appList)
            {
                Console.WriteLine($"ID: {a.AppointmentId}, Customer ID: {a.CustomerId}, Pet ID: {a.PetId}, Date: {a.AppointmentDate}, Groomer: {a.GroomerName}, Service: {a.Service}, Price: {a.Price}");
            }

            Console.WriteLine("=== END ===");
        }
    }
}
