using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public class AppointmentBLL : IAppointmentBLL
    {
        private readonly IAppointmentDAL _appointmentdal;
        private readonly ICustomerBLL _customerbll;
        private readonly IPetBLL _petbll;

        public AppointmentBLL(
            IAppointmentDAL? appointmentDAL = null, 
            ICustomerBLL? customerbll = null, 
            IPetBLL? petbll = null)
        {
            _appointmentdal = appointmentDAL ?? new AppointmentDAL();
            _customerbll = customerbll ?? new CustomerBLL();
            _petbll = petbll ?? new PetBLL();
        }

        public void Create(Appointment a)
        {
            if (a.CustomerId <= 0)
                throw new ValidationException("Invalid Customer ID");

            if (a.PetId <= 0)
                throw new ValidationException("Invalid Pet ID");

            if (a.AppointmentDate == DateTime.MinValue)
                throw new ValidationException("Appointment date is required.");

            if (a.AppointmentDate < DateTime.Now)
                throw new ValidationException("Appointment date must be in the future.");

            // Verify that the customer - Pet relationship is valid
            var customer = _customerbll.GetById(a.CustomerId) ?? throw new ValidationException("Customer not found.");
            var pet = _petbll.GetById(a.PetId) ?? throw new ValidationException("Pet not found.");

            if (pet.CustomerId != customer.CustomerId)
                throw new ValidationException("The pet does not belong to the customer.");

            try
            {
                _appointmentdal.Insert(a);
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
                _appointmentdal.Update(a);
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
                _appointmentdal.Delete(appointmentId);
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
                var appointments = _appointmentdal.GetAll();
                var customers = _customerbll.GetAll();
                var pets = _petbll.GetAll();

                foreach (var a in appointments)
                {
                    a.OwnerName = customers.FirstOrDefault(c => c.CustomerId == a.CustomerId)?.OwnerName ?? string.Empty;
                    a.PetName = pets.FirstOrDefault(p => p.PetId == a.PetId)?.PetName ?? string.Empty;
                }
                return appointments;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error loading appointments: ", ex);
            }
        }
        public Appointment? GetById(int appointmentId)
        {
            try
            {
                return _appointmentdal.GetById(appointmentId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error to get appointment by ID: ", ex);
            }
        }

        // Sorting
        public List<Appointment> SortByDate () => GetAllWithNames().OrderBy(a => a.AppointmentDate).ToList();
        public List<Appointment> SortByOwnerName () => GetAllWithNames().OrderBy(a => a.OwnerName).ToList();
        public List<Appointment> SortByPetName () => GetAllWithNames().OrderBy(a => a.PetName).ToList();


        // Searching
        public Appointment? SearchByAppointmentId(int appointmentId)
        {
            var appList = _appointmentdal.GetAll();
            return appList.FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> SearchByOwnerName(string ownerName)
        {
            
            return GetAllWithNames()
                .Where(a => a.OwnerName.Contains(ownerName ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Appointment> SearchByPetName(string petName)
        {
            return GetAllWithNames()
                .Where(a => a.PetName.Contains(petName ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
