using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming1.Tests.Fakes
{
    public class FakeServiceDAL : IServiceDAL
    {
        private readonly List<Service> _items = new();
        private int _nextId = 1;

        public void Insert(Service s)
        {
            s.ServiceId = _nextId++;
            _items.Add(Clone(s));
        }

        public void Update(Service s)
        {
            var idx = _items.FindIndex(x => x.ServiceId == s.ServiceId);
            if (idx >= 0) _items[idx] = Clone(s);
        }

        public void Delete(int serviceId) => _items.RemoveAll(x => x.ServiceId == serviceId);

        public List<Service> GetAll() => _items.Select(Clone).ToList();

        public Service? GetById(int serviceId) => _items.FirstOrDefault(x => x.ServiceId == serviceId) is Service s ? Clone(s) : null;

        public Service? GetByName(string serviceName)
            => _items.FirstOrDefault(x => string.Equals(x.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase)) is Service s ? Clone(s) : null;

        private static Service Clone(Service s) => new()
        {
            ServiceId = s.ServiceId,
            ServiceName = s.ServiceName,
            BasePrice = s.BasePrice
        };
    }
}

