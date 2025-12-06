using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int PetId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string GroomerName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Service { get; set; } = string.Empty;

    }
}
