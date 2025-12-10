using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.BLL;
using PetGrooming.Models;

namespace PetGrooming.Menu
{
    public class CustomerMenu
    {
        private readonly ICustomerBLL _cbll;
        private readonly IPetBLL _pbll;

        public CustomerMenu(
            ICustomerBLL? cbll = null,
            IPetBLL? pbll = null)
        {
            _cbll = cbll ?? new CustomerBLL();
            _pbll = pbll ?? new PetBLL();
        }

        public void Manage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Customer Management ===");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. View All Customers");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Manage Pets");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Please select an option: ");
                var input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": Add(); break;
                        case "2": ViewAll(); break;
                        case "3": Update(); break;
                        case "4": Delete(); break;
                        case "5": ManagePets(); break;
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
                    Console.ReadKey(true);
                }
                catch (BusinessException bex)
                {
                    Console.WriteLine($"Business Error: {bex.Message}");
                    Console.ReadKey(true);
                }
            }

        }
        private void Add()
        {
            Console.Clear();
            Console.WriteLine("=== Add New Customer ===");
            var customer = new Customer();
            Console.Write("Owner Name: ");
            customer.OwnerName = Console.ReadLine() ?? string.Empty;
            Console.Write("Phone Number: ");
            customer.PhoneNumber = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            customer.Email = Console.ReadLine() ?? string.Empty;
            _cbll.Create(customer);
            Console.WriteLine("Customer added successfully! Press any key to exit.");
            Console.ReadKey(true);


        }
        private void ViewAll()
        {
            Console.Clear();
            Console.WriteLine("=== All Customers ===");
            var customers = _cbll.GetAll();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (var c in customers)
                {
                    Console.WriteLine($"ID: {c.CustomerId}, Name: {c.OwnerName}, Phone: {c.PhoneNumber}, Email: {c.Email}");
                }
            }

            Console.WriteLine("Press any key to return to exit.");
            Console.ReadKey(true);
        }
        private void Update()
        {
            Console.Clear();
            Console.WriteLine("=== Update Customer ===");
            Console.Write("Enter Customer ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = new Customer { CustomerId = customerId };
                Console.Write("New Owner Name: ");
                customer.OwnerName = Console.ReadLine() ?? string.Empty;
                Console.Write("New Phone Number: ");
                customer.PhoneNumber = Console.ReadLine() ?? string.Empty;
                Console.Write("New Email: ");
                customer.Email = Console.ReadLine() ?? string.Empty;
                _cbll.Update(customer);
                Console.WriteLine("Customer updated successfully! Press any key to exit.");
            }
            else
            {
                Console.WriteLine("Invalid Customer ID. Press any key to exit.");
            }
            Console.ReadKey(true);
        }
        private void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Customer ===");
            Console.Write("Enter Customer ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                _cbll.Delete(customerId);
                Console.WriteLine("Customer deleted successfully! Press any key to exit.");
            }
            else
            {
                Console.WriteLine("Invalid Customer ID. Press any key to exit.");
            }
            Console.ReadKey(true);
        }
        private void ManagePets()
        {
            Console.Clear();
            Console.WriteLine("=== Pet Management ===");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID. Press any key to exit.");
                Console.ReadKey(true);
                return;
            }
            var cust = _cbll.GetById(customerId);
            if (cust == null)
            {
                Console.WriteLine("Customer not found. Press any key to exit.");
                Console.ReadKey(true);
                return;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Manage pets for this customer {cust.OwnerName} ===");
                Console.WriteLine("1. Add Pet");
                Console.WriteLine("2. View Pets");
                Console.WriteLine("3. Update Pet");
                Console.WriteLine("4. Delete Pet");
                Console.WriteLine("0. Back to Customer Menu");
                Console.Write("Please select an option: ");
                var input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1":
                            {
                                Console.WriteLine("=== Add New Pet ===");
                                var pet = new Pet { CustomerId = customerId };
                                Console.Write("Pet Name: "); pet.PetName = Console.ReadLine() ?? string.Empty;
                                Console.Write("Breed: "); pet.Breed = Console.ReadLine() ?? string.Empty;
                                Console.Write("Age: ");
                                if (!int.TryParse(Console.ReadLine(), out int age))
                                {
                                    age = 0;
                                    pet.Age = age;
                                }
                                _pbll.Create(pet);
                                Console.WriteLine("Pet added successfully! Press any key to exit.");
                                Console.ReadKey(true);
                                break;
                            }
                        case "2":
                            {
                                Console.WriteLine("=== Pets List ===");
                                var pets = _pbll.GetByCustomerId(customerId);
                                if (pets.Count == 0)
                                {
                                    Console.WriteLine("No pets found for this customer.");
                                }
                                else
                                {
                                    foreach (var p in pets)
                                    {
                                        Console.WriteLine($"ID: {p.PetId}, Name: {p.PetName}, Breed: {p.Breed}, Age: {p.Age}");
                                    }
                                }
                                Console.WriteLine("Press any key to return to exit.");
                                Console.ReadKey(true);
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("=== Update Pet ===");
                                Console.Write("Enter Pet ID to update: ");
                                if (!int.TryParse(Console.ReadLine(), out int petId))
                                {
                                    Console.WriteLine("Invalid Pet ID. Press any key to exit.");
                                    Console.ReadKey(true);
                                    break;
                                }
                                var pet = _pbll.GetById(petId);
                                if (pet == null)
                                {
                                    Console.WriteLine("Pet not found. Press any key to exit.");
                                    Console.ReadKey(true);
                                    break;
                                }
                                Console.WriteLine("New Age: ");
                                var ageInput = Console.ReadLine();
                                if (int.TryParse(ageInput, out int newAge))
                                {
                                    pet.Age = newAge;
                                }
                                _pbll.Update(pet);
                                Console.WriteLine("Pet updated successfully! Press any key to exit.");
                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("=== Delete Pet ===");
                                Console.Write("Enter Pet ID to delete: ");
                                if (!int.TryParse(Console.ReadLine(), out int petId))
                                {
                                    Console.WriteLine("Invalid Pet ID. Press any key to exit.");
                                    Console.ReadKey(true);
                                    break;
                                }
                                _pbll.Delete(petId);
                                Console.WriteLine("Pet deleted successfully! Press any key to exit.");
                                Console.ReadKey(true);
                                break;

                            }
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
                    Console.ReadKey(true);
                }
                catch (BusinessException bex)
                {
                    Console.WriteLine($"Business Error: {bex.Message}");
                    Console.ReadKey(true);

                }
            }
        }
    }
}
