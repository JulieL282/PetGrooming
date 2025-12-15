using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public int CustomerId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public int Age { get; set; }

    }
}