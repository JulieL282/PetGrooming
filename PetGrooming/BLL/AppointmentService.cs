using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;
using PetGrooming.DAL;
using PetGrooming.Utils;

namespace PetGrooming.BLL
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentDAL _appDal = new AppointmentDAL();

        //CRUD
        public void Create(Appointment a) => _appDal.Insert(a);
        public void Update(Appointment a) => _appDal.Update(a);
        public void Delete(int appointmentId) => _appDal.Delete(appointmentId);
        public List<Appointment> GetAll() => _appDal.GetAll();
        public Appointment? Find(int appointmentId) => _appDal.GetById(appointmentId);


        // Sorting - Bubble Sort
        public List<Appointment> SortByDate(List<Appointment> appList)
        {
            return Sorting.BubbleSortByDate(appList);
        }

        // Searching - Binary Search
        public Appointment? SearchById(List<Appointment> appList, int appointmentId)
        {
            return Searching.BinarySearchById(appList, appointmentId);
        }

    }
}
