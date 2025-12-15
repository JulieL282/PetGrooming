using PetGrooming.BLL;
using PetGrooming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Console.WriteLine("9. Back to Main Menu");
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
                        case "5": ManagePets(); break;
                        case "9": return;
                        case "0":
                            Console.WriteLine("Exiting the system. Goodbye\n");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please press any key to return.");
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

            // name loop
            while (true)
            {
                Console.Write("Owner Name: ");
                var name = Console.ReadLine()?.Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(name))
                {
                    customer.OwnerName = name;
                    break;
                }
                Console.WriteLine("Owner name is required.\n");
            }

            Console.Write("Phone Number: ");
            customer.PhoneNumber = Console.ReadLine() ?? string.Empty;

            // email loop
            while (true)
            {
                Console.Write("Email: ");
                string email = Console.ReadLine() ?? string.Empty;
                customer.Email = email;

                try
                {
                    // Test email validation in BLL
                    _cbll.Validate(customer);
                    break; // exit if validated
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine($"Validation Error: {ex.Message}");
                    Console.WriteLine("Please try again.\n");
                }
            }

            try
            {
                _cbll.Create(customer);
                Console.WriteLine($"Customer {customer.OwnerName} added successfully! Press any key to return.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
            }
            catch (BusinessException ex)
            {
                Console.WriteLine($"Business Error: {ex.Message}");
            }

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
                    Console.WriteLine($"- ID: {c.CustomerId}, Name: {c.OwnerName}, Phone: {c.PhoneNumber}, Email: {c.Email}\n");
                }
            }

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }
        private void Update()
        {
            Console.Clear();
            Console.WriteLine($"=== Update Customer ===");
            Console.Write("Enter Customer ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            Customer? existing;
            try
            {
                existing = _cbll.GetById(customerId);
            }
            catch (ValidationException)
            {
                Console.WriteLine("Invalid customer ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            if (existing == null)
            {
                Console.WriteLine("Customer not found. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            var updated = new Customer { CustomerId = customerId };

            Console.Write($"New Owner Name (Leave blank to keep '{existing.OwnerName}'): ");
            string? name = Console.ReadLine();
            updated.OwnerName = string.IsNullOrWhiteSpace(name) ? existing.OwnerName : name;

            Console.Write($"New Phone Number (Leave blank to keep '{existing.PhoneNumber}'): ");
            string? phone = Console.ReadLine();
            updated.PhoneNumber = string.IsNullOrWhiteSpace(phone) ? existing.PhoneNumber : phone;

            Console.Write($"New Email (Leave blank to keep '{existing.Email}'): ");
            string? email = Console.ReadLine();
            updated.Email = string.IsNullOrWhiteSpace(email) ? existing.Email : email;

            try
            {
                _cbll.Update(updated);
                Console.WriteLine("Customer updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }
        private void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Customer ===");
            Console.Write("Enter Customer ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            try
            {
                _cbll.Delete(customerId);
                Console.WriteLine("Customer deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }
        private void ManagePets()
        {
            Console.Clear();
            Console.WriteLine("=== Pet Management ===");
            Console.Write("Enter Customer ID to manage pets: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }
            var cust = _cbll.GetById(customerId);
            if (cust == null)
            {
                Console.WriteLine("Customer not found. Press any key to return.");
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
                Console.WriteLine("9. Back to Customer Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease select an option: ");
                var input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1":
                            AddPet(customerId);
                            break;

                        case "2":
                            ViewPets(customerId);
                            break;

                        case "3":
                            UpdatePet(customerId);
                            break;

                        case "4":
                            DeletePet(customerId);
                            break;

                        case "9": return;
                        case "0":
                            Console.WriteLine("Exiting the system. Goodbye\n");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please press any key to return.");
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

        private void AddPet(int customerId)
        {
            Console.Clear();
            Console.WriteLine("=== Add New Pet ===");

            var pet = new Pet { CustomerId = customerId };

            Console.Write("Pet Name: ");
            pet.PetName = Console.ReadLine() ?? string.Empty;

            Console.Write("Breed: ");
            pet.Breed = Console.ReadLine() ?? string.Empty;

            Console.Write("Age (Years): ");
            int.TryParse(Console.ReadLine(), out int age);
            pet.Age = age;

            _pbll.Create(pet);

            Console.WriteLine($"Pet {pet.PetName} added Successfully! Press any key to return.");
            Console.ReadKey(true);
        }

        private void ViewPets(int customerId)
        {
            Console.Clear();
            Console.WriteLine("=== Pet List===");

            var pets = _pbll.GetByCustomerId(customerId);

            if (pets.Count == 0)
                Console.WriteLine("No pets registered.");
            else
                pets.ForEach(p =>
                    Console.WriteLine($"ID: {p.PetId}, Name: {p.PetName}, Breed: {p.Breed}, Age: {p.Age}\n")
                );

            Console.WriteLine("Press any key to return.");
            Console.ReadKey(true);
        }

        private void UpdatePet(int customerId)
        {
            Console.Clear();
            Console.WriteLine($"=== Update Pet for {_cbll.GetById(customerId)?.OwnerName} ===");

            Console.Write("Enter Pet ID: ");
            if (!int.TryParse(Console.ReadLine(), out int petId))
            {
                Console.WriteLine("Invalid ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            var pet = _pbll.GetById(petId);

            if (pet == null || pet.CustomerId != customerId)
            {
                Console.WriteLine("Pet not found for this customer. Press any key to return.");
                Console.ReadKey(true);
                return;
            }


            Console.WriteLine($"\nEditing Pet's age:");
            Console.WriteLine($"Name : {pet.PetName}");
            Console.WriteLine($"Breed: {pet.Breed}");
            Console.WriteLine($"Current Age (Years): {pet.Age}\n");

            Console.Write("New Age in years: ");
            if (int.TryParse(Console.ReadLine(), out int newAge) && newAge >= 0)
                pet.Age = newAge;
            else
            {
                Console.WriteLine("Invalid age. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            try
            {
                _pbll.Update(pet);
                Console.WriteLine($"{pet.PetName}'s age updated successfully! Press any key to return.");
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

        private void DeletePet(int customerId)
        {
            Console.Clear();
            Console.WriteLine($"=== Delete Pet for {_cbll.GetById(customerId)?.OwnerName} ===");

            Console.Write("Enter Pet ID: ");
            if (!int.TryParse(Console.ReadLine(), out int petId))
            {
                Console.WriteLine("Invalid ID. Press any key to return.");
                Console.ReadKey(true);
                return;
            }

            var pet = _pbll.GetById(petId);

            if (pet == null || pet.CustomerId != customerId)
            {
                Console.WriteLine("Pet not found for this customer. Press any key to return.");
                Console.ReadKey(true);
                return;
            }


            try
            {
                _pbll.Delete(petId);
                Console.WriteLine($"Pet {pet.PetName} deleted successfully! Press any key to return.");
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
    }
}
