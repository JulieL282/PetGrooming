using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public interface IPetBLL
    {
        void Create(Pet p);
        void Update(Pet p);
        void Delete(int petId);

        List<Pet> GetAll();
        Pet? GetById(int petId);
        List<Pet> GetByCustomerId(int customerId);
        List<Pet> SearchByPetName(string petName);
        List<Pet> SortByPetName();
    }
}
