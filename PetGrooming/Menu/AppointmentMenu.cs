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
        private readonly IAppointmentBLL _abll;
        private readonly IServiceBLL _sbll;

        public AppointmentMenu(
            IAppointmentBLL? abll = null, 
            IServiceBLL? sbll = null)
        {
            // Interface implementation
            _abll = abll ?? new AppointmentBLL();
             _sbll = sbll ?? new ServiceBLL();
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
                try
                {
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
                catch (ValidationException vex)
                {
                    Console.WriteLine($"Validation Error: {vex.Message}");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey(true);
                }
                catch (BusinessException bex)
                {
                    Console.WriteLine($"Business Error: {bex.Message}");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey(true);
                }


            }
        }

        private void Add()
        {
            Console.Clear();
            var a = new Appointment();
            Console.WriteLine("=== Create New Appointment ===");
            Console.Write("Enter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out var cid))
            {
                Console.WriteLine("Invalid Customer ID.");
                Console.ReadKey(true);
                return;
            }
            a.CustomerId = cid;

            Console.Write("Enter Pet ID: ");
            if (!int.TryParse(Console.ReadLine(), out var pid))
            {
                Console.WriteLine("Invalid Pet ID.");
                Console.ReadKey(true);
                return;
            }
            a.PetId = pid;

            var services = _sbll.GetAll();
            Console.WriteLine("=== Available Services: ===");

            foreach (var svc in services)
            {
                Console.WriteLine($"- {svc.ServiceId}. {svc.ServiceName}: {svc.BasePrice:C}");
            }
            Console.WriteLine("Please enter the Service ID from the list above: ");
            if (!int.TryParse(Console.ReadLine(), out var sid))
            {
                Console.WriteLine("Invalid Service ID.");
                Console.ReadKey(true);
                return;
            }
            a.ServiceId = sid;

            if (sid > 0)
            {
                var service = services.FirstOrDefault(s => s.ServiceId == sid);
                if (service != null)
                {
                    a.Price = service.BasePrice;
                }
                else
                {
                    Console.WriteLine("Service not found for the given Service ID.");
                    Console.ReadKey(true);
                    return;
                }
            }
            Console.WriteLine("Press Enter to continue with the Base Price");
            var priceinput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(priceinput))
            {
                a.Price = 0m; // auto fill base price
            }
            else
            {
                if (decimal.TryParse(priceinput, out var customPrice))
                {
                    a.Price = customPrice;
                }
                else
                {
                    Console.WriteLine("Invalid price format.");
                    Console.ReadKey(true);
                    return;
                }
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

            _abll.Create(a);
            Console.WriteLine("Appointment created successfully. Press Any Key to Exit");
            Console.ReadKey(true);

        }

        private void ViewAll()
        {
            Console.Clear();
            var appList = _abll.GetAllWithNames();
           

            if (appList.Count == 0)
            {
                Console.WriteLine("No appointments found. Press Any Key to Exit");
                Console.ReadKey(true);
                return;
            }
            foreach ( var app in appList)
            {
                Console.WriteLine($"ID: {app.AppointmentId}, Date: {app.AppointmentDate}, Owner: {app.OwnerName}, Pet: {app.PetName}, Groomer: {app.GroomerName}, Service: {app.ServiceName}, Price: {app.Price:C}");
            }
            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey(true);

        }

        private void Update()
        {
            Console.Clear();
            Console.Write("Enter Appointment ID to Update: ");

            if (!int.TryParse(Console.ReadLine(), out var appId))
            {
                Console.WriteLine("Invalid Appointment ID.");
                Console.ReadKey(true);
                return;
            }
            var a = _abll.GetById(appId);
            
            if (a == null)
            {
                Console.WriteLine("Appointment not found. Press Any Key to Exit");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Please choose an option you wish to updat");
            Console.WriteLine("1. Appointment Date");
            Console.WriteLine("2. Groomer Name");
            Console.WriteLine("3. Service");
            Console.WriteLine("4. Price");
            Console.WriteLine("0. Cancel");
            Console.Write("Select an option: ");
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
                    Console.WriteLine("Available Groomers: Alice, Bob, Charlie");
                    a.GroomerName = Console.ReadLine() ?? string.Empty;
                    break;
                case "3":
                    Console.Write("Enter new Service: ");
                    var services = _sbll.GetAll();
                    foreach (var svc in services)
                    {
                        Console.WriteLine($"- {svc.ServiceId}. {svc.ServiceName}: {svc.BasePrice:C}");
                    }
                    Console.WriteLine("Please enter the Service ID from the list above: ");
                    if (!int.TryParse(Console.ReadLine(), out var newSid))
                    {
                        Console.WriteLine("Invalid Service ID.");
                        return;
                    }
                    a.ServiceId = newSid;
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
                        Console.ReadKey(true);
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
            _abll.Update(a);
            Console.WriteLine("Appointment updated successfully. Press Any Key to Exit");
            Console.ReadKey(true);

        }

        private void Delete()
        {
            Console.Clear();
            Console.Write("Enter Appointment ID to Delete: ");

            if (!int.TryParse(Console.ReadLine(), out var appId))
            {
                Console.WriteLine("Invalid Appointment ID.");
                Console.ReadKey(true);
                return;
            }

            _abll.Delete(appId);
            Console.WriteLine("Appointment deleted successfully. Press Any Key to Exit");
            Console.ReadKey(true);
        }
    }
}
