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
        private readonly IAppointmentBLL _app;
        private readonly ICustomerBLL _cust;
        private readonly IPetBLL _pet;
        private readonly IServiceBLL _service;

        public AppointmentMenu(
            IAppointmentBLL? app = null, 
            ICustomerBLL? cust = null,
            IPetBLL? pet = null,
            IServiceBLL? service = null)
        {
            // Interface implementation
            _app = app ?? new AppointmentBLL();
            _cust = cust ?? new CustomerBLL();
            _pet = pet ?? new PetBLL();
            _service = service ?? new ServiceBLL();
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
            if (!int.TryParse(Console.ReadLine(), out var cid))
            {
                Console.WriteLine("Invalid Customer ID.");
                return;
            }
            a.CustomerId = cid;

            Console.Write("Enter Pet ID: ");
            if (!int.TryParse(Console.ReadLine(), out var pid))
            {
                Console.WriteLine("Invalid Pet ID.");
                return;
            }
            a.PetId = int.Parse(Console.ReadLine()!);

            Console.Write("Enter Service: ");
            Console.WriteLine("Available Services: Full Bath, Haircut, Nail Trim");
            var serviceInput = Console.ReadLine() ?? "";
            a.Service = serviceInput;

            var services = _service.GetAll();
            var svc = services.Find(s => s.ServiceName.Equals(serviceInput, StringComparison.OrdinalIgnoreCase));

            if (svc != null)
            {
                Console.WriteLine($"The price for {svc.ServiceName} is {svc.Price:C}");
                Console.Write("Do you want to proceed with this service? (y/n): ");

            }
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(serviceInput) && decimal.TryParse(priceInput, out var price)) 
            {
                a.Price = price;
            }
            else
            {
                
                a.Price = svc?.Price ?? 0m;
            }

            Console.WriteLine("Enter Appointment Date (yyyy-MM-dd HH:mm): ");
            Console.WriteLine("Available Every 30 Minutes");
            if (!DateTime.TryParse(Console.ReadLine(), out var appDate))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }
            a.AppointmentDate = appDate;

            Console.Write("Enter Groomer Name: ");
            Console.WriteLine("Available Groomers: Alice, Bob, Charlie");
            a.GroomerName = Console.ReadLine() ?? string.Empty;

            _service.Create(a);
            Console.WriteLine("Appointment created successfully. Press Any Key to Exit");
            Console.ReadKey(true);

            /*Console.Write("Enter Price: ");
            a.Price = decimal.Parse(Console.ReadLine()!);*/

        }

        private void ViewAll()
        {
            Console.Clear();
            var appList = _service.GetAllWithNames();
           

            if (appList.Count > 0)
            {
                Console.WriteLine("=== Current Appointments ===");
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

            if (!int.TryParse(Console.ReadLine(), out var appId))
            {
                Console.WriteLine("Invalid Appointment ID.");
                return;
            }
            var a = _service.GetById(appId);
            
            if (a == null)
            {
                Console.WriteLine("Appointment not found. Press Any Key to Exit");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Please choose an option you wish to updat");
            Console.WriteLine("1. Appointment Date, 2. Groomer, 3. Service, 4. Price 0. Cancel");
            var choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Write("Enter new Appointment Date (yyyy-MM-dd HH:mm): ");
                    if (DateTime.TryParse(Console.ReadLine(), out var newDate))
                    {
                        a.AppointmentDate = newDate;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format.");
                        return;
                    }
                    break;
                case "2":
                    Console.Write("Enter new Groomer Name: ");
                    a.GroomerName = Console.ReadLine() ?? string.Empty;
                    break;
                case "3":
                    Console.Write("Enter new Service: ");
                    a.Service = Console.ReadLine() ?? string.Empty;
                    break;
                case "4":
                    Console.Write("Enter new Price: ");
                    if (decimal.TryParse(Console.ReadLine(), out var newPrice))
                    {
                        a.Price = newPrice;
                    }
                    else
                    {
                        Console.WriteLine("Invalid price format.");
                        return;
                    }
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Any Key to Exit");
                    Console.ReadKey(true);
                    return;
            }

        }

        private void Delete()
        {
            Console.Clear();
            Console.Write("Enter Appointment ID to Delete: ");

            if (!int.TryParse(Console.ReadLine(), out var appId))
            {
                Console.WriteLine("Invalid Appointment ID.");
                return;
            }

            _service.Delete(appId);
            Console.WriteLine("Appointment deleted successfully. Press Any Key to Exit");
            Console.ReadKey(true);
        }
    }
}
