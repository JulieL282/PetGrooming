using PetGrooming.BLL;
using PetGrooming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetGrooming.Menu
{
    public class AppointmentMenu
    {
        // Polymorphism Interface
        private readonly IAppointmentBLL _abll;
        private readonly IServiceBLL _sbll;
        private readonly ICustomerBLL _cbll;
        private readonly IPetBLL _pbll;

        public AppointmentMenu(
            IAppointmentBLL? abll = null,
            IServiceBLL? sbll = null,
            ICustomerBLL? cbll = null,
            IPetBLL? pbll = null
            )
        {
            // Interface implementation
            _abll = abll ?? new AppointmentBLL();
            _sbll = sbll ?? new ServiceBLL();
            _cbll = cbll ?? new CustomerBLL();
            _pbll = pbll ?? new PetBLL();
        }

        public void Manage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Appointment Management ===");
                Console.WriteLine("1. Create Appointment");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. Update Appointment");
                Console.WriteLine("4. Delete Appointment");
                Console.WriteLine("9. Back to Main Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect an option: ");

                var input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": Add(); break;
                        case "2": ViewAll(); break;
                        case "3": Update(); break;
                        case "4": Delete(); break;
                        case "9": return;
                        case "0":
                            Console.WriteLine("Exiting the system. Goodbye\n");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please press any key to return.\n");
                            Console.ReadKey(true);
                            break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ReadKey(true);
                }


            }
        }

        private void Add()
        {
            Console.Clear();
            var a = new Appointment();

            Console.WriteLine("\n=== Create New Appointment [Press 'x' to cancel] ===");

            // Customer ID
            while (true)
            {
                Console.Write("Enter Customer ID: ");
                var raw = Console.ReadLine() ?? "";
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!int.TryParse(raw, out var cid))
                {
                    Console.WriteLine("Invalid Customer ID. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }
                a.CustomerId = cid;
                break;
            }

            var cust = _cbll.GetById(a.CustomerId);
            if (cust == null)
            {
                Console.WriteLine("Customer not found. Press any key to return.\n");
                Console.ReadKey(true);
                return;
            }

            // Pet ID
            while (true)
            {
                Console.Write("Enter Pet ID: ");
                var raw = Console.ReadLine() ?? "";
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!int.TryParse(raw, out var pid))
                {
                    Console.WriteLine("Invalid Pet ID. Try again.\n");
                    continue;
                }

                var pet = _pbll.GetById(pid);

                if (pet == null || pet.CustomerId != a.CustomerId)
                {
                    Console.WriteLine("Pet not found for this customer. Press any key to return.\n");
                    Console.ReadKey(true);
                    return;
                }

                a.PetId = pid;
                break;
            }

            var services = _sbll.GetAll();

            Console.WriteLine("\n=== Available Services: ===");
            foreach (var svc in services)
            {
                Console.WriteLine($"- {svc.ServiceId}. {svc.ServiceName}: {svc.BasePrice:C}");
            }

            // Service ID
            while (true)
            {
                Console.Write("\nEnter Service ID: ");
                var raw = Console.ReadLine() ?? "";
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!int.TryParse(raw, out var sid))
                {
                    Console.WriteLine("Invalid Service ID. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }

                var svc = services.FirstOrDefault(s => s.ServiceId == sid);

                if (svc == null)
                {
                    Console.WriteLine("Invalid Service ID. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }

                a.ServiceId = sid;
                a.Price = svc.BasePrice;
                Console.WriteLine($"Price: {a.Price:C}");
                break;
            }

            // Appointment date
            while (true)
            {
                Console.Write("\nEnter Appointment Date (yyyy-MM-dd HH:mm): ");
                var raw = Console.ReadLine() ?? "";
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (DateTime.TryParse(raw, out var date))
                {
                    if (date > DateTime.Now)
                    {
                        a.AppointmentDate = date;
                        break;
                    }
                    Console.WriteLine("Appointment must be in the future.\n");
                    Console.ReadKey(true);
                    continue;
                }
                else
                {
                    Console.WriteLine("\nInvalid date format. Press Any Key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }
            }

            // Groomer
            string[] groomers = { "Alice", "Bob", "Charlie" };
            Console.WriteLine("\nAvailable Groomers: Alice, Bob, Charlie");
            while (true)
            {
                Console.Write("Enter Groomer Name: ");
                var g = Console.ReadLine() ?? "";
                if (string.Equals(g, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (groomers.Contains(g, StringComparer.OrdinalIgnoreCase))
                {
                    a.GroomerName = g;
                    break;
                }

                Console.WriteLine("Invalid Groomer name. Press any key to return.\n");
                Console.ReadKey(true);
            }

            try
            {
                _abll.Create(a);
                Console.WriteLine("\nAppointment created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue.\n");
            Console.ReadKey(true);
        }

        private void ViewAll()
        {
            Console.Clear();
            var appList = _abll.GetAllWithNames();


            if (appList.Count == 0)
            {
                Console.WriteLine("\nNo appointments found. Press Any Key to return.\n");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine("=== Appointments ===");
            foreach (var app in appList)
            {
                Console.WriteLine($"ID: {app.AppointmentId}, Date: {app.AppointmentDate}, Owner: {app.OwnerName}, Pet: {app.PetName}, Groomer: {app.GroomerName}, Service: {app.ServiceName}, Price: {app.Price:C}\n");
            }
            Console.WriteLine("Press Any Key to return.\n");
            Console.ReadKey(true);

        }

        private void Update()
        {
            Console.Clear();
            Console.WriteLine("=== Update Appointment [Press 'x' to cancel] ===");

            int id;
            while (true)
            {
                Console.Write("Enter Appointment ID to update: ");
                var raw = Console.ReadLine() ?? "";
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!int.TryParse(raw, out id))
                {
                    Console.WriteLine("\nInvalid Appointment ID. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }
                break;
            }

            var a = _abll.GetById(id);
            if (a == null)
            {
                Console.WriteLine("\nInvalid Appointment ID. Press any key to return.\n");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("\nPlease choose an option you wish to update");
            Console.WriteLine("1. Appointment Date");
            Console.WriteLine("2. Groomer Name");
            Console.WriteLine("3. Service");
            Console.WriteLine("0. Cancel");
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    while (true)
                    {
                        Console.Write("New Date (yyyy-MM-dd HH:mm): ");
                        var raw = Console.ReadLine() ?? "";
                        if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                        if (DateTime.TryParse(raw, out var date))
                        {
                            if (date > DateTime.Now)
                            {
                                a.AppointmentDate = date;
                                break;
                            }
                            Console.WriteLine("Date must be in the future.");
                            Console.ReadKey(true);

                        }
                        else
                        {
                            Console.WriteLine("Invalid date. Try again.\n");
                            Console.ReadKey(true);
                        }
                    }
                    break;

                case "2":
                    {
                        string[] groomers = { "Alice", "Bob", "Charlie" };
                        while (true)
                        {
                            Console.Write("New Groomer: ");
                            var g = Console.ReadLine() ?? "";
                            if (string.Equals(g, "x", StringComparison.OrdinalIgnoreCase)) return;

                            if (groomers.Contains(g, StringComparer.OrdinalIgnoreCase))
                            {
                                a.GroomerName = g;
                                break;
                            }
                            Console.WriteLine("Invalid Groomer name. Try again.\n");
                            Console.ReadKey(true);
                        }
                    }
                    break;

                case "3":
                    {
                        var services = _sbll.GetAll();
                        Console.WriteLine("=== Available Services: ===");
                        foreach (var svc in services)
                        {
                            Console.WriteLine($"- {svc.ServiceId}. {svc.ServiceName}: {svc.BasePrice:C}");
                        }

                        while (true)
                        {
                            Console.Write("New Service ID: ");
                            var raw = Console.ReadLine() ?? "";
                            if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                            if (!int.TryParse(raw, out var sid))
                            {
                                Console.WriteLine("Invalid Service ID. Try again.\n");
                                Console.ReadKey(true);
                                continue;
                            }

                            var svc = services.FirstOrDefault(s => s.ServiceId == sid);
                            if (svc == null)
                            {
                                Console.WriteLine("Service not found. Try again.\n");
                                Console.ReadKey(true);
                                continue;
                            }

                            a.ServiceId = sid;
                            a.Price = svc.BasePrice;
                            Console.WriteLine($"New Price: {a.Price:C}");
                            break;
                        }
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option. Press Any Key to Exit\n");
                    Console.ReadKey(true);
                    return;
            }
            _abll.Update(a);
            Console.WriteLine("\nAppointment updated successfully! Press Any Key to Exit");
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
            Console.WriteLine("\nAppointment deleted successfully! Press Any Key to Exit");
            Console.ReadKey(true);
        }
    }
}