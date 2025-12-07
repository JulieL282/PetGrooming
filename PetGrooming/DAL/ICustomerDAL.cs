using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.DAL
{
    public interface ICustomerDAL
    {
        void Insert(Customer c);
        void Update(Customer c);
        void Delete(int customerId);
        List<Customer> GetAll();
        Customer? GetById(int customerId);
    }
}
