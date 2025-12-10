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
            Console.WriteLine("0. Return to Main Menu");

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
                        Console.WriteLine("Invalid option. Please press any key to return to the menu.");
                        Console.ReadKey(true);
                        break;
                }

            }
            catch (ValidationException vex)
            {
                Console.WriteLine($"Validation Error: {vex.Message}");
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey(true);
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business Error: {bex.Message}");
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey(true);
            }
        }
        public void Add()
        {
            Console.Clear();
            var s = new Service();
            Console.WriteLine("=== Add New Service ===");
            Console.Write("Service Name: ");
            s.ServiceName = Console.ReadLine() ?? string.Empty;
            Console.Write("Base Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Console.WriteLine("Invalid price. Press any key to return to the menu.");
                Console.ReadKey(true);
                return;
            }
            s.BasePrice = price;
            
            _sbll.Create(s);

            Console.WriteLine("Service added successfully! Press any key to return to the menu.");
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
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey(true);
        }
        private void Update()
        {
            Console.Clear();
            Console.WriteLine("=== Update Service ===");
            Console.Write("Enter Service ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID. Press any key to return to the menu.");
                Console.ReadKey(true);
                return;
            }
            var s = _sbll.GetById(id);
            if (s == null)
            {
                Console.WriteLine("Service not found. Press any key to return to the menu.");
                Console.ReadKey(true);
                return;
            }

            Console.Write("New Base Price: ");
            var np = Console.ReadLine();
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Console.WriteLine("Invalid price. Press any key to return to the menu.");
                Console.ReadKey(true);
                return;
            }
            s.BasePrice = price;
            _sbll.Update(s);
            Console.WriteLine("Service updated successfully! Press any key to return to the menu.");
            Console.ReadKey(true);
        }
        private void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Service ===");
            Console.Write("Enter Service ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID. Press any key to return to the menu.");
                Console.ReadKey(true);
                return;
            }
            _sbll.Delete(id);
            Console.WriteLine("Service deleted successfully! Press any key to return to the menu.");
            Console.ReadKey(true);
        }
    }
}
