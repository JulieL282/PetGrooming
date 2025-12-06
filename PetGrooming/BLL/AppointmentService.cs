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


        // Sorting with copy
        public List<Appointment> SortByDate()
        {
            var copy = new List<Appointment>(_appDal.GetAll());
            return Sorting.BubbleSortByDate(copy);
        }

        public List<Appointment> SortByAppointmentId()
        {
            var copy = new List<Appointment>(_appDal.GetAll());
            return Sorting.BubbleSortByAppointmentId(copy);
        }

        // Searching
        public Appointment? SearchByAppointmentId(int appointmentId)
        {
            // Make a copy and sort it first
            var copy = new List<Appointment>(_appDal.GetAll());
            Sorting.BubbleSortByDate(copy);
            return Searching.BinarySearchByAppointmentId(copy, appointmentId);
        }

        public List<Appointment> SearchByCustomerId(int customerId)
        {
            var list = _appDal.GetAll();
            return Searching.SearchByCustomerId(list, customerId);
        }

        public List<Appointment> SearchByPetId(int petId)
        {
            var list = _appDal.GetAll();
            return Searching.SearchByPetId(list, petId);
        }

        public List<Appointment> SearchByDate(DateTime appointmentDate)
        {
            var list = _appDal.GetAll();
            return Searching.SearchByDate(list, appointmentDate);
        }
    }
}
