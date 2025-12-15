using PetGrooming.DAL;
using PetGrooming.Models;

namespace PetGrooming1.Tests.Fakes
{
    // replace SQLite during unit testing
    public class FakeAppointmentDAL : IAppointmentDAL
    {
        // temp appointment table for testing
        private readonly List<Appointment> _items = new();
        private int _nextId = 1; 

        public void Insert(Appointment a)
        {
            // increment
            a.AppointmentId = _nextId++;
            _items.Add(Clone(a));
        }

        public void Update(Appointment a)
        {
            var idx = _items.FindIndex(x => x.AppointmentId == a.AppointmentId);
            if (idx >= 0) _items[idx] = Clone(a);
        }

        public void Delete(int appointmentId) => _items.RemoveAll(x => x.AppointmentId == appointmentId);

        public List<Appointment> GetAll() => _items.Select(Clone).ToList(); // copy list

        // getbyid if found clone, if not return null
        public Appointment? GetById(int appointmentId) => _items.FirstOrDefault(x => x.AppointmentId == appointmentId) is Appointment a ? Clone(a) : null;

        // clone to test
        private static Appointment Clone(Appointment a) => new()
        {
            AppointmentId = a.AppointmentId,
            CustomerId = a.CustomerId,
            PetId = a.PetId,
            ServiceId = a.ServiceId,
            AppointmentDate = a.AppointmentDate,
            GroomerName = a.GroomerName,
            Price = a.Price,
            OwnerName = a.OwnerName,
            PetName = a.PetName,
            ServiceName = a.ServiceName
        };
    }
}
