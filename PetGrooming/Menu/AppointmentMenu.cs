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
        // Polymorphism Interface
        private readonly IAppointmentService _service;

        public AppointmentMenu()
        {
            // Interface implementation
            _service = new AppointmentService();
        }
        
        public void Manage()
        { 
            while (true)
            {
                Console.WriteLine("=== Appointment Management ===");
                Console.WriteLine("1. Create Appointment");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. Update Appointment");
                Console.WriteLine("4. Delete Appointment");
                Console.WriteLine("0. Cancel");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": Add(); break;
                    case "2": ViewAll(); break;
                    case "3": Update(); break;
                    case "4": Delete(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Invalid option. Please press any key to restart.");
                        Console.ReadKey(true);
                        break;
                }

            }
        }

        private void Add()
        {
            Console.Clear();
            var a = new Appointment();
            Console.Write("Enter Customer ID: ");
            a.CustomerId = int.Parse(Console.ReadLine()!);

            Console.Write("Enter Pet ID: ");
            a.PetId = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Appointment Date (yyyy-MM-dd HH:mm): ");
            Console.WriteLine("Available Every 30 Minutes");
            a.AppointmentDate = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Enter Groomer Name: ");
            Console.WriteLine("Available Groomers: Alice, Bob, Charlie");
            a.GroomerName = Console.ReadLine()!;

            Console.Write("Enter Service: ");
            Console.WriteLine("Available Services: Full Bath, Haircut, Nail Trim");
            a.Service = Console.ReadLine()!;

            Console.Write("Enter Price: ");
            a.Price = decimal.Parse(Console.ReadLine()!);

            _service.Create(a);
            Console.WriteLine("Appointment created successfully. Press Any Key to Exit");
            Console.ReadKey(true);
            Console.Clear();
        }

        private void ViewAll()
        {
            Console.Clear();
            var appList = _service.GetAll();

            Console.WriteLine("=== Current Appointments ===");

            if (appList.Count > 0)
            {
                foreach (var a in appList)
                {
                    Console.WriteLine($"Appointment ID: {a.AppointmentId}, Appointment Date: YYYY-MM-DD HH:mm: {a.AppointmentDate}, Customer ID: {a.CustomerId}, Pet ID: {a.PetId}, Date: {a.AppointmentDate}, Groomer: {a.GroomerName}, Service: {a.Service}, Price: {a.Price}");
                }

                Console.WriteLine("=== END ===");
            }
            else
            {
                Console.WriteLine("No appointments found.");
            }
        }

        private void Update()
        {
            Console.Clear();
            Console.Write("Enter Appointment ID to Update: ");

            var appId = int.Parse(Console.ReadLine()!);

            var a = _service.Find(appId);

            if (a == null)
            {
                Console.WriteLine("Appointment not found. Press Any Key to Exit");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Enter New Customer ID: ");
            a.CustomerId = int.Parse(Console.ReadLine()!);
            Console.Write("Enter New Pet ID: ");
            a.PetId = int.Parse(Console.ReadLine()!);
            Console.Write("Enter New Appointment Date (yyyy-MM-dd HH:mm): ");
            a.AppointmentDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Enter New Groomer Name: ");
            a.GroomerName = Console.ReadLine()!;
            Console.Write("Enter New Service: ");
            a.Service =  Console.ReadLine()!;
            Console.Write("Enter New Price: ");
            a.Price = decimal.Parse(Console.ReadLine()!);
            _service.Update(a);
            Console.WriteLine("Appointment updated successfully. Press Any Key to Exit");
            Console.ReadKey(true);
        }

        private void Delete()
        {
            Console.Clear();
            Console.Write("Enter Appointment ID to Delete: ");
            var appId = int.Parse(Console.ReadLine()!);
            var a = _service.Find(appId);
            if (a == null)
            {
                Console.WriteLine("Appointment not found. Press Any Key to Exit");
                Console.ReadKey(true);
                return;
            }
            _service.Delete(appId);
            Console.WriteLine("Appointment deleted successfully. Press Any Key to Exit");
            Console.ReadKey(true);
        }
    }
}
