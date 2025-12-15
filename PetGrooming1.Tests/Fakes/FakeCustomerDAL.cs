using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming1.Tests.Fakes
{
    public class FakeCustomerDAL : ICustomerDAL
    {
        private readonly List<Customer> _items = new();
        private int _nextId = 1;

        public void Insert(Customer c)
        {
            c.CustomerId = _nextId++;
            _items.Add(Clone(c));
        }

        public void Update(Customer c)
        {
            var idx = _items.FindIndex(x => x.CustomerId == c.CustomerId);
            if (idx >= 0) _items[idx] = Clone(c);
        }

        public void Delete(int customerId) => _items.RemoveAll(x => x.CustomerId == customerId);

        public List<Customer> GetAll() => _items.Select(Clone).ToList();

        public Customer? GetById(int customerId) => _items.FirstOrDefault(x => x.CustomerId == customerId) is Customer c ? Clone(c) : null;

        // clone
        private static Customer Clone(Customer c) => new()
        {
            CustomerId = c.CustomerId,
            OwnerName = c.OwnerName,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            Pets = c.Pets ?? new List<Pet>()
        };
    }
}
