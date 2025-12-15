using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming1.Tests.Fakes
{
    public class FakePetDAL : IPetDAL
    {
        private readonly List<Pet> _items = new();
        private int _nextId = 1;

        public void Insert(Pet p)
        {
            p.PetId = _nextId++;
            _items.Add(Clone(p));
        }

        public void Update(Pet p)
        {
            var idx = _items.FindIndex(x => x.PetId == p.PetId);
            if (idx >= 0) _items[idx] = Clone(p);
        }

        public void Delete(int petId) => _items.RemoveAll(x => x.PetId == petId);

        public List<Pet> GetAll() => _items.Select(Clone).ToList();

        public Pet? GetById(int petId) => _items.FirstOrDefault(x => x.PetId == petId) is Pet p ? Clone(p) : null;

        public List<Pet> GetByCustomerId(int customerId)
            => _items.Where(x => x.CustomerId == customerId).Select(Clone).ToList();

        private static Pet Clone(Pet p) => new()
        {
            PetId = p.PetId,
            CustomerId = p.CustomerId,
            PetName = p.PetName,
            Breed = p.Breed,
            Age = p.Age
        };
    }
}
