using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetGrooming.BLL;
using PetGrooming.Models;
using PetGrooming1.Tests.Fakes;

namespace PetGrooming1.Tests
{
    [TestClass]
    public class PetBLLTests
    {
        private PetBLL _pbll = null!;
        private FakePetDAL _pdal = null!;

        [TestInitialize]
        public void Setup()
        {
            _pdal = new FakePetDAL();
            _pbll = new PetBLL(_pdal);
        }

        [TestMethod]
        public void Create_ValidPet_InsertsPet()
        {
            var pet = new Pet
            {
                CustomerId = 1,
                PetName = "Miso",
                Breed = "Labradoodle",
                Age = 1
            };

            _pbll.Create(pet);

            var all = _pdal.GetAll();
            Assert.AreEqual(1, all.Count); // only  1 pet
            Assert.AreEqual("Miso", all[0].PetName);
        }

        [TestMethod]
        public void Create_MissingPetName_ThrowsValidationException()
        {
            var pet = new Pet
            {
                CustomerId = 1,
                PetName = ""
            };

            try
            {
                _pbll.Create(pet);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }

        [TestMethod]
        public void Update_OnlyAgeIsUpdated()
        {
            var pet = new Pet
            {
                CustomerId = 1,
                PetName = "Miso",
                Breed = "Labradoodle",
                Age = 1
            };

            _pbll.Create(pet);

            var existing = _pbll.GetAll()[0];
            existing.Age = 3;

            _pbll.Update(existing);

            var updated = _pbll.GetById(existing.PetId)!;
            Assert.AreEqual(3, updated.Age); // new age
            Assert.AreEqual("Miso", updated.PetName); // name stays the same
        }

        [TestMethod]
        public void Delete_InvalidPetId_ThrowsValidationException()
        {
            try
            {
                _pbll.Delete(0);
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (ValidationException)
            {
                // Test passed
            }
        }
    }
}
