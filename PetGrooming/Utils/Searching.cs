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
        public static Appointment? BinarySearchById(List<Appointment> appList, int appointmentId)
        {
            int left = 0;
            int right = appList.Count - 1;
            
            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (appList[mid].AppointmentId == appointmentId)
                {
                    return appList[mid];
                }
                else if (appList[mid].AppointmentId < appointmentId)
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
    }
}
