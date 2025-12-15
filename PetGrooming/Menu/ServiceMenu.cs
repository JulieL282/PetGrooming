using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.BLL;
using PetGrooming.Models;

namespace PetGrooming.Menu
{
    public class ServiceMenu
    {
        private readonly IServiceBLL _sbll;
        public ServiceMenu(IServiceBLL? sbll = null)
        {
            _sbll = sbll ?? new ServiceBLL();
        }
        public void Manage()
        {
            Console.Clear();
            Console.WriteLine("=== Service Management ===");
            Console.WriteLine("1. Add Service");
            Console.WriteLine("2. View All Services");
            Console.WriteLine("3. Update Service");
            Console.WriteLine("4. Delete Service");
            Console.WriteLine("9. Return to Main Menu");
            Console.WriteLine("0. Exit");
            Console.Write("\nPlease select an option: ");


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
            catch (ValidationException vex)
            {
                Console.WriteLine($"Validation Error: {vex.Message}");
                Console.WriteLine("Press any key to return.\n");
                Console.ReadKey(true);
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business Error: {bex.Message}");
                Console.WriteLine("Press any key to return.\n");
                Console.ReadKey(true);
            }
        }
        public void Add()
        {
            Console.Clear();
            var s = new Service();
            Console.WriteLine("=== Add New Service [Press 'x' to cancel] ===");
            Console.Write("Service Name: ");

            while (true)
            {
                Console.Write("Service Name: ");
                var name = Console.ReadLine() ?? string.Empty;

                if (string.Equals(name, "x", StringComparison.OrdinalIgnoreCase))
                    return;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    s.ServiceName = name;
                    break;
                }

                Console.WriteLine("Service name is required.\n");
            }

            while (true)
            {
                Console.Write("Base Price: ");
                var raw = Console.ReadLine() ?? string.Empty;
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!decimal.TryParse(raw, out var price))
                {
                    Console.WriteLine("Invalid price. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }
                s.BasePrice = price;
                break;
            }

            _sbll.Create(s);

            Console.WriteLine("Service added successfully! Press any key to return.\n");
            Console.ReadKey(true);
        }
        public void ViewAll()
        {
            Console.Clear();
            Console.WriteLine("=== All Services ===");
            var sList = _sbll.GetAll();
            if (sList.Count == 0)
            {
                Console.WriteLine("No services found.");
            }
            foreach (var s in sList)
            {
                Console.WriteLine($"ID: {s.ServiceId}, Name: {s.ServiceName}, Price: {s.BasePrice:C}");
            }
            Console.WriteLine("Press any key to return.\n");
            Console.ReadKey(true);
        }
        private void Update()
        {
            Console.Clear();
            Console.WriteLine("=== Update Service Price [Press 'x' to cancel] ===");
            int id;

            while (true)
            {
                Console.Write("Enter Service ID to update: ");
                var raw = Console.ReadLine() ?? string.Empty;
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!int.TryParse(raw, out id))
                {
                    Console.WriteLine("Invalid ID. Press any key to return.\n");
                    Console.ReadKey(true);
                    continue;
                }
                break;
            }

            var s = _sbll.GetById(id);
            if (s == null)
            {
                Console.WriteLine("Service not found. Press any key to return.\n");
                Console.ReadKey(true);
                return;
            }

            decimal newPrice;
            while (true)
            {
                Console.Write($"New Price for {s.ServiceName}: ");
                var raw = Console.ReadLine() ?? string.Empty;
                if (string.Equals(raw, "x", StringComparison.OrdinalIgnoreCase)) return;

                if (!decimal.TryParse(raw, out newPrice) || newPrice < 0)
                {
                    Console.WriteLine("Invalid price. Please try again.\n");
                    continue;
                }
                break;
            }

            s.BasePrice = newPrice;

            try
            {
                _sbll.Update(s);
                Console.WriteLine("Service price updated successfully! Press any key to return.\n");
            }
            catch (ValidationException vex)
            {
                Console.WriteLine($"Validation Error: {vex.Message}");
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business Error: {bex.Message}");
            }

            Console.ReadKey(true);
        }
        private void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Service ===");
            Console.Write("Enter Service ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID. Press any key to return.\n");
                Console.ReadKey(true);
                return;
            }
            try
            {
                _sbll.Delete(id);
                Console.WriteLine("Service deleted successfully! Press any key to return.\n");
            }
            catch (ValidationException vex)
            {
                Console.WriteLine($"Validation Error: {vex.Message}");
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business Error: {bex.Message}");
                Console.WriteLine("This service is booked in existing appointments.");
                Console.WriteLine("You must delete those appointments first.");
            }

            Console.ReadKey(true);
        }
    }
}