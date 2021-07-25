using BankingApp.Data;
using BankingApp.Data.Entities;
using BankingApp.Models.BankModels;
using BankingApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BankingApp.UnitTests
{
    [TestClass]
    public class BankServiceTests
    {
        private Mock<DbSet<Bank>> _mockBanks;
        private Mock<ApplicationDbContext> _mockContext;
        private BankService _bankService;

        private BankCreate _bankCreate1;
        private BankEdit _bankEdit;

        [TestInitialize]
        public void Arrange()
        {
            var testEntities = new List<Bank>
            {
                new Bank { Id = 1, Name = "Bank One" },
                new Bank { Id = 2, Name = "Bank Two" },
                new Bank { Id = 3, Name = "Bank Three" }
            }.AsQueryable();
            _bankCreate1 = new BankCreate { Name = "New Bank" };
            _bankEdit = new BankEdit { BankId = 1, Name = "Changed Name" };

            _mockBanks = new Mock<DbSet<Bank>>();
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.Provider).Returns(testEntities.Provider);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.Expression).Returns(testEntities.Expression);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.ElementType).Returns(testEntities.ElementType);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.GetEnumerator()).Returns(testEntities.GetEnumerator());
            _mockContext = new Mock<ApplicationDbContext>();
            _mockContext.Setup(m => m.Banks).Returns(_mockBanks.Object);
            _bankService = new BankService(_mockContext.Object);
        }

        [TestMethod]
        public void GetBanks_ShouldReturnAllEntities()
        {
            var bankResults = _bankService.GetBanks();

            Assert.IsTrue(bankResults.Count() == 3);
        }

        [TestMethod]
        public void GetBankById_ShouldReturnCorrectEntity()
        {
            var bankResult = _bankService.GetBankById(1);

            Assert.AreEqual("Bank One", bankResult.Name);
        }

        [TestMethod]
        public void CreateBank_ShouldAddEntity()
        {
            _bankService.CreateBank(_bankCreate1);

            _mockBanks.Verify(b => b.Add(It.IsAny<Bank>()), Times.Once());
            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void UpdateBank_ShouldSaveChanges()
        {
            _bankService.UpdateBank(_bankEdit);

            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void DeleteBank_ShouldRemoveEntity()
        {
            _bankService.DeleteBank(1);

            _mockBanks.Verify(b => b.Remove(It.IsAny<Bank>()), Times.Once());
            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }
    }
}
