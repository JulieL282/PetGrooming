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
                // Initialize database connection
                Database.Initialize();
                Database.SeedIfEmpty();
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