using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.DAL;
using PetGrooming.Models;
using PetGrooming.Utils;

namespace PetGrooming.BLL
{
    public class AppointmentBLL : IAppointmentBLL
    {
        private readonly IAppointmentDAL _adal;
        private readonly ICustomerBLL _cbll;
        private readonly IPetBLL _pbll;
        private readonly IServiceBLL _sbll;

        public AppointmentBLL(
            IAppointmentDAL? adal = null,
            ICustomerBLL? cbll = null,
            IPetBLL? pbll = null,
            IServiceBLL? sbll = null)
        {
            _adal = adal ?? new AppointmentDAL();
            _cbll = cbll ?? new CustomerBLL();
            _pbll = pbll ?? new PetBLL();
            _sbll = sbll ?? new ServiceBLL();
        }

        public void Create(Appointment a)
        {
            if (a.CustomerId <= 0)
                throw new ValidationException("Customer ID is required.");

            if (a.PetId <= 0)
                throw new ValidationException("Pet ID is required");

            if (a.ServiceId <= 0)
                throw new ValidationException("Service ID is required.");

            if (a.AppointmentDate == DateTime.MinValue)
                throw new ValidationException("Appointment date is required.");

            if (a.AppointmentDate < DateTime.Now)
                throw new ValidationException("Appointment date must be in the future.");


            // Verify that the customer - Pet relationship is valid
            var customer = _cbll.GetById(a.CustomerId) ?? throw new ValidationException("Customer not found.");
            var pet = _pbll.GetById(a.PetId) ?? throw new ValidationException("Pet not found.");

            if (pet.CustomerId != customer.CustomerId)
                throw new ValidationException("The pet does not belong to the customer.");

            var service = _sbll.GetById(a.ServiceId) ?? throw new ValidationException("Service not found.");

            // Auto generate Price based on Service BasePrice
            a.Price = service.BasePrice;

            try
            {
                _adal.Insert(a);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error creating appointment: ", ex);
            }
        }
        
        public void Update(Appointment a)
        {
            if (a.AppointmentId <= 0)
                throw new ValidationException("Invalid Appointment ID.");
            try
            {
                _adal.Update(a);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating appointment: ", ex);
            }
        }
        public void Delete(int appointmentId)
        {
            if (appointmentId <= 0)
                throw new ValidationException("Invalid Appointment ID.");
            try
            {
                _adal.Delete(appointmentId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error deleting appointment: ", ex);
            }
        }

        public List<Appointment> GetAllWithNames()
        {
            try
            {
                var appointments = _adal.GetAll();
                var customers = _cbll.GetAll();
                var pets = _pbll.GetAll();
                var services = _sbll.GetAll();

                foreach (var a in appointments)
                {
                    a.OwnerName = customers.FirstOrDefault(c => c.CustomerId == a.CustomerId)?.OwnerName ?? string.Empty;
                    a.PetName = pets.FirstOrDefault(p => p.PetId == a.PetId)?.PetName ?? string.Empty;
                    a.ServiceName = services.FirstOrDefault(s => s.ServiceId == a.ServiceId)?.ServiceName ?? string.Empty;
                }
                return appointments;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error loading appointment list: ", ex);
            }
        }
        public Appointment? GetById(int appointmentId)
        {
            try
            {
                return _adal.GetById(appointmentId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error retrieving appointment by ID {appointmentId} : ", ex);
            }
        }

        // Sorting
        public List<Appointment> SortByDate() 
            => Sorting.BubbleSortByDate(GetAllWithNames());

        public List<Appointment> SortByOwnerName() 
            => Sorting.SortbyOwnerName(GetAllWithNames());

        public List<Appointment> SortByPetName() 
            => Sorting.SortbyPetName(GetAllWithNames());

        public List<Appointment> SortByAppointmentId() 
            => Sorting.SortbyAppointmentId(GetAllWithNames());



        // Searching
        public Appointment? SearchByAppointmentId(int appointmentId)
        {
            var appList = GetAllWithNames();
            var sorted = Sorting.SortbyAppointmentId(appList);
            return Searching.BinarySearchByAppointmentId(sorted, appointmentId);
        }

        public List<Appointment> SearchByCustomerId(int customerId)
        {
            return Searching.SearchByCustomerId(GetAllWithNames(), customerId);
        }
        public List<Appointment> SearchByPetId(int petId)
        {
            return Searching.SearchByPetId(GetAllWithNames(), petId);

        }
        public List<Appointment> SearchByDate(DateTime date)
        {
            return Searching.SearchByDate(GetAllWithNames(), date);
        }
    }
}
