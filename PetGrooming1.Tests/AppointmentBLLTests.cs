using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetGrooming.BLL;
using PetGrooming.Models;
using PetGrooming1.Tests.Fakes;

namespace PetGrooming1.Tests
{
    //MSTest
    [TestClass]
    public class AppointmentBLLTests
    {
        // declare dependancies for fake DaL
        private FakeAppointmentDAL _adal = null!;
        private FakeCustomerDAL _cdal = null!;
        private FakePetDAL _pdal = null!;
        private FakeServiceDAL _sdal = null!;

        // test BLLs
        private CustomerBLL _cbll = null!;
        private PetBLL _pbll = null!;
        private ServiceBLL _sbll = null!;
        private AppointmentBLL _abll = null!;

        [TestInitialize]
        public void Setup()

        {
            // temp memory to test BLL
            _adal = new FakeAppointmentDAL();
            _cdal = new FakeCustomerDAL();
            _pdal = new FakePetDAL();
            _sdal = new FakeServiceDAL();

            // inject fake DAL into BLLs
            _cbll = new CustomerBLL(_cdal);
            _pbll = new PetBLL(_pdal);
            _sbll = new ServiceBLL(_sdal);

            // inject all to App bll - business logic test
            _abll = new AppointmentBLL(_adal, _cbll, _pbll, _sbll);

            // Seeding test data
            var cust = new Customer { OwnerName = "Julie", Email = "julie@test.com" };
            _cbll.Create(cust); // ID becomes 1

            // pet and owner relationship test
            var pet = new Pet { CustomerId = 1, PetName = "Miso", Breed = "Labradoodle", Age = 1 };
            _pbll.Create(pet);

            // check service name and price
            var service = new Service { ServiceName = "Full Grooming", BasePrice = 50m };
            _sbll.Create(service);
        }

        [TestMethod]
        public void Create_ValidAppointment_SetsPriceFromService_AndInserts()
        {
            var a = new Appointment
            {
                CustomerId = 1,
                PetId = 1,
                ServiceId = 1,
                AppointmentDate = DateTime.Now.AddDays(2),
                GroomerName = "Alice",
                Price = 999m // try overwrite - business logic test
            };

            _abll.Create(a);

            var all = _adal.GetAll();
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual(50m, all[0].Price, "Price should be set from Service BasePrice.");
        }

        [TestMethod]
        public void Create_PastDate_ThrowsValidationException()
        {
            var a = new Appointment
            {
                CustomerId = 1,
                PetId = 1,
                ServiceId = 1,
                AppointmentDate = DateTime.Now.AddMinutes(-1)
            };

            try
            {
                _abll.Create(a);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }

        }

        [TestMethod]
        public void Create_PetNotBelongToCustomer_ThrowsValidationException()
        {
            // New customer
            _cbll.Create(new Customer { OwnerName = "Other", Email = "other@test.com" }); // ID=2

            var a = new Appointment
            {
                CustomerId = 2,  
                PetId = 1,       // pet 1 belongs to customer 1
                ServiceId = 1,
                AppointmentDate = DateTime.Now.AddDays(1)
            };

           
            try
            {
                _abll.Create(a);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }

        }

        [TestMethod]
        public void GetAllWithNames_PopulatesOwnerPetServiceNames()
        {
            _abll.Create(new Appointment
            {
                CustomerId = 1,
                PetId = 1,
                ServiceId = 1,
                AppointmentDate = DateTime.Now.AddDays(1),
                GroomerName = "Bob"
            });

            var list = _abll.GetAllWithNames();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Julie", list[0].OwnerName);
            Assert.AreEqual("Miso", list[0].PetName);
            Assert.AreEqual("Full Grooming", list[0].ServiceName);
        }
    }
}
