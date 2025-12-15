using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public interface ICustomerBLL
    {
        void Validate(Customer c);
        void Create(Customer c);
        void Update(Customer c);
        void Delete(int customerId);

        List<Customer> GetAll();
        Customer? GetById(int customerId);

        List<Customer> SearchByOwnerName(string ownerName);
        List<Customer> SortByOwnerName();
    }
}