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
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<Pet> Pets { get; set; }

        public Customer()
        {
            Pets = new List<Pet>();

        }
    }
