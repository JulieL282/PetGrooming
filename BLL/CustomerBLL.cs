using PetGrooming.DAL;
using PetGrooming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetGrooming.BLL
{
    public class CustomerBLL : ICustomerBLL
    {

        private readonly ICustomerDAL _cdal;
        public CustomerBLL(ICustomerDAL? dal = null)
        {
            _cdal = dal ?? new CustomerDAL();
        }
        // Email Regex check
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public void Validate(Customer c)
        {
            if (string.IsNullOrWhiteSpace(c.OwnerName))
                throw new ValidationException("Owner name is required.");

            if (string.IsNullOrWhiteSpace(c.Email))
                throw new ValidationException("Email is required.");

            if (!IsValidEmail(c.Email))
                throw new ValidationException("Invalid email format.");
        }

        public void Create(Customer c)
        {
            Validate(c);

            try
            {
                _cdal.Insert(c);
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

            Validate(c);

            try
            {
                _cdal.Update(c);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating customer info: ", ex);
            }
        }
        public void Delete(int customerId)
        {
            if (customerId <= 0)
                throw new ValidationException("Invalid Customer ID.");

            try
            {
                _cdal.Delete(customerId);
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
                return _cdal.GetAll();
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
                return _cdal.GetById(customerId);
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
                .Contains(ownerName ?? string.Empty, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Customer> SortByOwnerName()
        {
            var copy = new List<Customer>(GetAll());
            copy.Sort((c1, c2) => string.Compare(c1.OwnerName, c2.OwnerName, StringComparison.OrdinalIgnoreCase));
            return copy;
        }

    }
}
