using System;
using PetGrooming.DAL;
using PetGrooming.Menu;

namespace PetGrooming
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Create DB, tables, and seed service data
                Database.Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                return;
            }
            
            // Start the main menu
            var main = new MainMenu();
            main.Show();
        }
    }
}