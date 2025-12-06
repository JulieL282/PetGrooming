using PetGrooming.DAL;
using PetGrooming.Menu;

namespace PetGrooming
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Initialize database connection
            Database.Initialize();
            // Start the main menu
            MainMenu.Display();
        }
    }
}