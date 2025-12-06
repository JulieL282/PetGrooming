using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<Pet> Pets { get; set; }

        public Customer()
        {
            Pets = new List<Pet>();

        }
    }
}
