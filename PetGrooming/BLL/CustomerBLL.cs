using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;
using PetGrooming.DAL;

namespace PetGrooming.BLL
{
    public class CustomerBLL : ICustomerBLL
    {

        private readonly ICustomerDAL _customerdal;
        public CustomerBLL(ICustomerDAL? dal = null)
        {
            _customerdal = dal ?? new CustomerDAL();
        }
        public void Create(Customer c)
        {
            if (string.IsNullOrWhiteSpace(c.OwnerName))
          
                throw new ValidationException("Owner name is required.");

                try
                {
                    _customerdal.Insert(c);
                }
                catch (DataAccessException ex)
                {
                    throw new BusinessException("Error creating customer info: ", ex);
                }
            
        }
        public void Update(Customer c)
        {
            if (c.CustomerId <= 0)
                throw new ValidationException("Invalid Customer ID.");
            try
            {
                _customerdal.Update(c);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating customer: ", ex);
            }
        }
        public void Delete(int customerId)
        {
            if (customerId <= 0)
                throw new ValidationException("Invalid Customer ID.");
            try
            {
                _customerdal.Delete(customerId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error deleting customer info: ", ex);
            }
        }
        public List<Customer> GetAll()
        {
            try
            {
                return _customerdal.GetAll();
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error loading customer list: ", ex);
            }
        }
        public Customer? GetById(int customerId)
        {
            if (customerId <= 0)
                throw new ValidationException("Invalid Customer ID.");
            try
            {
                return _customerdal.GetById(customerId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Customer ID {customerId} not found: ", ex);
            }
        }
        public List<Customer> SearchByOwnerName(string ownerName)
        {
            return GetAll()
                .Where(c => c.OwnerName
                .Contains(ownerName ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Customer> SortByOwnerName()
        {
            return GetAll().OrderBy(c => c.OwnerName).ToList();
        }

    }
}
