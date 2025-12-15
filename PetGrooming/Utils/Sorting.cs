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
        public static List<Appointment> SortByDate(List<Appointment> appList)
        {
            // Creating copy for sorting so original list is not modified
            var arr = new List<Appointment>(appList);

            int n = arr.Count;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j].AppointmentDate > arr[j + 1].AppointmentDate)
                    {
                        // Swap
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        swapped = true;
                    }
                }
                if (!swapped)
                    break; // List is sorted
            }
            return arr;
        }

        // Selection sort
        public static List<Appointment> SortByOwnerName(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (string.Compare(arr[j].OwnerName, arr[minIdx].OwnerName, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        minIdx = j;
                    }
                }
                if (minIdx != i)
                {
                    (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
                }

            }
            return arr;
        }
        // insertion sort
        public static List<Appointment> SortByPetName(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            int n = arr.Count;
            for (int i = 1; i < n; i++)
            {
                var key = arr[i];
                int j = i - 1;
                while (j >= 0 && string.Compare(arr[j].PetName, key.PetName, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
            return arr;
        }
        // Insertion sort
        public static List<Appointment> SortByAppointmentId(List<Appointment> appList)
        {
            var arr = new List<Appointment>(appList);
            int n = arr.Count;
            for (int i = 1; i < n; i++)
            {
                var key = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j].AppointmentId > key.AppointmentId)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
            return arr;
        }


    }
}