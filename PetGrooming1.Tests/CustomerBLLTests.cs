using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetGrooming.BLL;
using PetGrooming.Models;
using PetGrooming1.Tests.Fakes;

namespace PetGrooming1.Tests
{
    [TestClass]
    public class CustomerBLLTests
    {
        private CustomerBLL _cbll = null!;
        private FakeCustomerDAL _cdal = null!;

        [TestInitialize]
        public void Setup()
        {
            _cdal = new FakeCustomerDAL();
            _cbll = new CustomerBLL(_cdal);
        }

        [TestMethod]
        public void Create_ValidCustomer_InsertsCustomer()
        {
            var c = new Customer
            {
                OwnerName = "Julie",
                Email = "julie@test.com"
            };

            _cbll.Create(c);

            var all = _cdal.GetAll();
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual("Julie", all[0].OwnerName);
        }

        [TestMethod]
        public void Create_EmptyOwnerName_ThrowsValidationException()
        {
            var c = new Customer
            {
                OwnerName = "",
                Email = "julie@test.com"
            };

            try
            {
                _cbll.Create(c);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }

        [TestMethod]
        public void Create_InvalidEmail_ThrowsValidationException()
        {
            var c = new Customer
            {
                OwnerName = "Julie",
                Email = "invalid-email"
            };

            try
            {
                _cbll.Create(c);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }

        [TestMethod]
        public void Update_InvalidCustomerId_ThrowsValidationException()
        {
            var c = new Customer
            {
                CustomerId = 0,
                OwnerName = "Julie",
                Email = "julie@test.com"
            };

            try
            {
                _cbll.Update(c);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }
    }
}
