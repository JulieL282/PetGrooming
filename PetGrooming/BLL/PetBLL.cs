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
        private readonly IPetDAL _petdal;
        public PetBLL(IPetDAL? dal = null)
        {
            _petdal = dal ?? new PetDAL();
        }
        public void Create(Pet p)
        {
            if (p.CustomerId <= 0)
                throw new ValidationException("Please Enter Valid Customer ID for this Pet.");

            if (string.IsNullOrWhiteSpace(p.PetName))
                throw new ValidationException("Pet name is required.");

            try
            {
                _petdal.Insert(p);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error creating pet: ", ex);
            }
        }
        public void Update(Pet p)
        {
            if (p.PetId <= 0)
                throw new ValidationException("Invalid Pet ID.");

            try
            {
                _petdal.Update(p);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error updating pet: ", ex);
            }
        }
        public void Delete(int petId)
        {
            if (petId <= 0)
                throw new ValidationException("Invalid Pet ID.");
            try
            {
                _petdal.Delete(petId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Error deleting pet: ", ex);
            }
        }
        public List<Pet> GetAll()
        {
            try
            {
                return _petdal.GetAll();
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
                return _petdal.GetById(petId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Pet ID not found: ", ex);
            }
        }
        public List<Pet> GetByCustomerId(int customerId)
        {
            if (customerId <= 0)
                throw new ValidationException("Invalid Customer ID.");
            try
            {
                return _petdal.GetByCustomerId(customerId);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("Pets not found with the provided Customer ID ", ex);
            }
        }
        public List<Pet> SearchByPetName(string petName)
        {
           var petList = GetAll();
            return petList.Where(p => p.PetName.Contains(petName ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Pet> SortByPetName()
        {
            var petList = GetAll();
            return petList.OrderBy(p => p.PetName).ToList();
        }
    }
}
