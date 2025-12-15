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
        public readonly IServiceDAL _sdal;
        public ServiceBLL(IServiceDAL? dal = null)
        {
            _sdal = dal ?? new ServiceDAL();
        }
        public void Create(Service s)
        {
            if (string.IsNullOrWhiteSpace(s.ServiceName))
                throw new ValidationException("Service name is required.");

            if (s.BasePrice < 0)
                throw new ValidationException("Service price cannot be negative.");

            try
            {
                _sdal.Insert(s);
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
                _sdal.Update(s);
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
                _sdal.Delete(serviceId);
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
                return _sdal.GetAll();
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
                return _sdal.GetById(serviceId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error retrieving service info with the provided Id {serviceId} : ", ex);
            }
        }
        public Service? GetByName(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ValidationException("Service name is required.");
            try
            {
                return _sdal.GetByName(serviceName);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error retrieving service info : ", ex);
            }
        }
    }
}