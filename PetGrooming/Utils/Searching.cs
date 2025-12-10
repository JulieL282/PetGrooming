using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.Utils
{
    public static class Searching
    {
        public static Appointment? BinarySearchByAppointmentId(List<Appointment> sorted, int targetId)
        {
            int left = 0;
            int right = sorted.Count - 1;
            
            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (sorted[mid].AppointmentId == targetId)
                {
                    return sorted[mid];
                }
                else if (sorted[mid].AppointmentId < targetId)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return null; // Not found
        }

        //Linear Searches
        public static List<Appointment> SearchByCustomerId(List<Appointment> appList, int customerId)
        {
            return appList.Where(a => a.CustomerId == customerId).ToList();
        }

        public static List<Appointment> SearchByPetId(List<Appointment> appList, int petId)
        {
           
            return appList.Where(a => a.PetId == petId).ToList();
        }

        public static List<Appointment> SearchByDate(List<Appointment> appList, DateTime date)
        {
            
            return appList.Where(a => a.AppointmentDate.Date == date.Date).ToList();
        }
    }
}