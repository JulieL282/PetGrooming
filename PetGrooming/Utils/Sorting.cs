using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.Utils
{
    public static class Sorting
    {

        // Bubble Sort by Date (ascending)
        public static List<Appointment> BubbleSortByDate(List<Appointment> appList)
        {
            // Creating copy for sorting so original list is not modified
            var arr = new List<Appointment>(appList);

            int n = arr.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j].AppointmentDate > arr[j + 1].AppointmentDate)
                    {
                        // Swap
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    }
                }
            }
            return arr;
        }

        public static List<Appointment> SortbyOwnerName(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            arr.Sort((a, b) => string.Compare(a.OwnerName, b.OwnerName, StringComparison.OrdinalIgnoreCase));
            return arr;
        }
        public static List<Appointment> SortbyPetName(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            arr.Sort((a, b) => string.Compare(a.PetName, b.PetName, StringComparison.OrdinalIgnoreCase));
            return arr;
        }
        public static List<Appointment> SortbyAppointmentId(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            arr.Sort((a, b) => a.AppointmentId.CompareTo(b.AppointmentId));
            return arr;
        }

    }
}


