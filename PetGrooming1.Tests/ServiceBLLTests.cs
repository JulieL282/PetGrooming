using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetGrooming.BLL;
using PetGrooming.Models;
using PetGrooming1.Tests.Fakes;

namespace PetGrooming1.Tests
{
    [TestClass]
    public class ServiceBLLTests
    {
        private ServiceBLL _sbll = null!;
        private FakeServiceDAL _sdal = null!;

        [TestInitialize]
        public void Setup()
        {
            _sdal = new FakeServiceDAL();
            _sbll = new ServiceBLL(_sdal);
        }

        [TestMethod]
        public void Create_ValidService_InsertsService()
        {
            var s = new Service
            {
                ServiceName = "Bath",
                BasePrice = 30m
            };

            _sbll.Create(s);

            var all = _sdal.GetAll();
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual("Bath", all[0].ServiceName);
        }

        [TestMethod]
        public void Create_NegativePrice_ThrowsValidationException()
        {
            var s = new Service
            {
                ServiceName = "Bath",
                BasePrice = -10m
            };

            try
            {
                _sbll.Create(s);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }

        [TestMethod]
        public void GetByName_ReturnsCorrectService()
        {
            _sbll.Create(new Service { ServiceName = "Hair Cut", BasePrice = 20m });

            var result = _sbll.GetByName("Hair Cut");

            Assert.IsNotNull(result);
            Assert.AreEqual(20m, result.BasePrice);
        }

        [TestMethod]
        public void GetByName_EmptyName_ThrowsValidationException()
        {
            try
            {
                _sbll.GetByName("");
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }
    }
}
