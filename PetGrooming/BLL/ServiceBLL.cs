using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public class ServiceBLL : IServiceBLL
    {
        public readonly IServiceDAL _servicedal;
        public ServiceBLL(IServiceDAL? dal = null)
        {
            _servicedal = dal ?? new ServiceDAL();
        }
        public void Create(Service s)
        {
            if (string.IsNullOrWhiteSpace(s.ServiceName))
                throw new ValidationException("Service name is required.");

            if (s.Price < 0)
                throw new ValidationException("Service price cannot be negative.");

            try
            {
                _servicedal.Insert(s);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error creating service info: ", ex);
            }
        }
        public void Update(Service s)
        {
            if (s.ServiceId <= 0)
                throw new ValidationException("Invalid Service ID.");
            try
            {
                _servicedal.Update(s);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating service info: ", ex);
            }
        }
        public void Delete(int serviceId)
        {
            if (serviceId <= 0)
                throw new ValidationException("Invalid Service ID.");
            try
            {
                _servicedal.Delete(serviceId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error deleting service info: ", ex);
            }
        }
        public List<Service> GetAll()
        {
            try
            {
                return _servicedal.GetAll();
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error retrieving service list: ", ex);
            }
        }
        public Service? GetById(int serviceId)
        {
            if (serviceId <= 0)
                throw new ValidationException("Invalid Service ID.");
            try
            {
                return _servicedal.GetById(serviceId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error retrieving service info with the provided Id {serviceId} : ", ex);
            }
        }
    }
}
