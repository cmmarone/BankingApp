using BankingApp.Data;
using BankingApp.Models.BankModels;
using BankingApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BankingApp.UnitTests
{
    [TestClass]
    public class BankServiceTests
    {
        private BankService _bankService;
        private BankCreate _bankCreate1;
        private BankCreate _bankCreate2;
        private BankEdit _bankEdit;

        [TestInitialize]
        public void Arrange()
        {
            _bankService = new BankService();
            _bankCreate1 = new BankCreate { Name = "Test Bank 1" };
            _bankCreate2 = new BankCreate { Name = "Test Bank 2" };
        }

        [TestMethod]
        public void B_CreateBank_ShouldCreateEntity()
        {
            _bankService.CreateBank(_bankCreate1);
            _bankService.CreateBank(_bankCreate2);

            using (var context = new ApplicationDbContext())
            {
                bool query = context.Banks.Any(b => b.Name == _bankCreate1.Name);
                Assert.IsTrue(query);
            }
        }

        [TestMethod]
        public void C_GetBanks_ShouldReturnAllEntities()
        {
            var bankResults = _bankService.GetBanks();

            bool query1 = bankResults.Any(b => b.Name == _bankCreate1.Name);
            bool query2 = bankResults.Any(b => b.Name == _bankCreate2.Name);
            Assert.IsTrue(query1 && query2);
        }

        [TestMethod]
        public void D_GetBankById_ShouldReturnCorrectEntity()
        {
            using (var context = new ApplicationDbContext())
            {
                int id = (context.Banks.FirstOrDefault(b => b.Name == _bankCreate1.Name)).Id;
                var bank = _bankService.GetBankById(id);
                Assert.AreEqual(_bankCreate1.Name, bank.Name);
            }
        }

        [TestMethod]
        public void E_UpdateBank_ShouldUpdateValue()
        {
            using (var context = new ApplicationDbContext())
            {
                int id = (context.Banks.FirstOrDefault(b => b.Name == _bankCreate1.Name)).Id;
                _bankEdit = new BankEdit { BankId = id, Name = "Updated Bank 1" };
                _bankService.UpdateBank(_bankEdit);

                var bank = _bankService.GetBankById(id);
                Assert.AreEqual(_bankEdit.Name, bank.Name);
            }
        }

        [TestMethod]
        public void F_DeleteBank_ShouldRemoveEntity()
        {
            using (var context = new ApplicationDbContext())
            {
                int id1 = (context.Banks.FirstOrDefault(b => b.Name == _bankEdit.Name)).Id;
                int id2 = (context.Banks.FirstOrDefault(b => b.Name == _bankCreate2.Name)).Id;

                _bankService.DeleteBank(id1);
                _bankService.DeleteBank(id2);

                bool query = context.Banks.Any(b => b.Id == id1);
                Assert.IsFalse(query);
            }
        }
    }
}
