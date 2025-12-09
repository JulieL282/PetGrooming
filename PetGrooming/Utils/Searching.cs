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
        public static Appointment? BinarySearchByAppointmentId(List<Appointment> appList, int appointmentId)
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

        public static List<Appointment> SearchByCustomerId(List<Appointment> appList, int customerId)
        {
            List<Appointment> results = new List<Appointment>();

            foreach (var appointment in appList)
            {
                if (appointment.CustomerId == customerId)
                {
                    results.Add(appointment);
                }
            }
            return results;
        }

        public static List<Appointment> SearchByPetId(List<Appointment> appList, int petId)
        {
            List<Appointment> results = new List<Appointment>();

            foreach (var appointment in appList)
            {
                if (appointment.PetId == petId)
                {
                    results.Add(appointment);
                }
            }
            return results;
        }

        public static List<Appointment> SearchByDate(List<Appointment> appList, DateTime date)
        {
            List<Appointment> results = new List<Appointment>();

            foreach (var appointment in appList)
            {
                if (appointment.AppointmentDate.Date == date.Date)
                {
                    results.Add(appointment);
                }
            }
            return results;
        }
    }
}

//namespace PetGrooming.Utils
//{
//    public static class Searching
//    {
//        public static Appointment? BinarySearchByAppointmentId(List<Appointment> src, int appointmentId)
//        {
//            var list = src.OrderBy(a => a.AppointmentId).ToList();
//            int left = 0, right = list.Count - 1;
//            while (left <= right)
//            {
//                int mid = (left + right) / 2;
//                if (list[mid].AppointmentId == appointmentId) return list[mid];
//                if (list[mid].AppointmentId < appointmentId) left = mid + 1;
//                else right = mid - 1;
//            }
//            return null;
//        }
//    }
//}
