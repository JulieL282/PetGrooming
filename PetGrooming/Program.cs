using PetGrooming.DAL;
using PetGrooming.Menu;

namespace PetGrooming
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Initialize database connection
                Database.Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing database: " + ex.Message);
                return;
            }
            // Start the main menu
            var main = new MainMenu();
            main.Show();
        }
    }
}