using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public class PetBLL : IPetBLL
    {
        private readonly IPetDAL _pdal;
        public PetBLL(IPetDAL? dal = null)
        {
            _pdal = dal ?? new PetDAL();
        }
        public void Create(Pet p)
        {
            if (p.CustomerId <= 0)
                throw new ValidationException("Customer ID is required.");

            if (string.IsNullOrWhiteSpace(p.PetName))
                throw new ValidationException("Pet name is required.");

            try
            {
                _pdal.Insert(p);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error creating pet info: ", ex);
            }
        }
        public void Update(Pet p)
        {
            if (p.PetId <= 0)
                throw new ValidationException("Invalid Pet ID.");

            try
            {
                _pdal.Update(p);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating pet info: ", ex);
            }
        }
        public void Delete(int petId)
        {
            if (petId <= 0)
                throw new ValidationException("Invalid Pet ID.");
            try
            {
                _pdal.Delete(petId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error deleting pet info: ", ex);
            }
        }
        public List<Pet> GetAll()
        {
            try
            {
                return _pdal.GetAll();
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error loading Pet List: ", ex);
            }
        }
        public Pet? GetById(int petId)
        {
            if (petId <= 0)
                throw new ValidationException("Invalid Pet ID.");
            try
            {
                return _pdal.GetById(petId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Pet ID {petId} not found: ", ex);
            }
        }
        public List<Pet> GetByCustomerId(int customerId)
        {
            if (customerId <= 0)
                throw new ValidationException("Invalid Customer ID.");
            try
            {
                return _pdal.GetByCustomerId(customerId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Pets not found with the provided Customer ID {customerId}", ex);
            }
        }
        public List<Pet> SearchByPetName(string petName)
        {
           return GetAll()
                .Where(p => p.PetName
                .Contains(petName ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Pet> SortByPetName()
        {
            var copy = new List<Pet>(GetAll());
            copy.Sort((p1, p2) => string.Compare(p1.PetName, p2.PetName, StringComparison.OrdinalIgnoreCase));
            return copy;
        }
    }
}
