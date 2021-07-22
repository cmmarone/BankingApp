using BankingApp.Data;
using BankingApp.Data.Entities;
using BankingApp.Models.BankAccountModels;
using BankingApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UnitTests
{
    [TestClass]
    public class BankAccountServiceTests
    {
        private Guid _ownerId;
        private BankAccountService _bankAccountService;
        private BankAccountCreate _bankAccountCreate1;
        private BankAccountCreate _bankAccountCreate2;


        [TestInitialize]
        public void Arrange()
        {
            _ownerId = Guid.NewGuid();
            _bankAccountService = new BankAccountService(_ownerId);

            var bank = new Bank { Name = "Sample Bank" };
            using (var context = new ApplicationDbContext()) { context.Banks.Add(bank); }

            _bankAccountCreate1 = new BankAccountCreate
            {
                BankName = "Sample Bank",
                FirstName = "Drew",
                LastName = "Sample",
                AccountType = AccountType.Checking
            };
            _bankAccountCreate2 = new BankAccountCreate
            {
                BankName = "Sample Bank",
                FirstName = "Linda",
                LastName = "Pretender",
                AccountType = AccountType.Checking
            };
        }

        [TestMethod]
        public void B_CreateBankAccount_ShouldCreateEntity()
        {
            _bankAccountService.CreateBankAccount(_bankAccountCreate1);
            _bankAccountService.CreateBankAccount(_bankAccountCreate2);

            using (var context = new ApplicationDbContext())
            {
                bool query = context.BankAccounts.Any(b => b.LastName == _bankAccountCreate1.LastName);
                Assert.IsTrue(query);
            }
        }

        [TestMethod]
        public void C_GetUserBankAccounts_ShouldReturnAllEntities()
        {
            var accountResults = _bankAccountService.GetUserBankAccounts();

            bool query = accountResults.Count() >= 2;
            Assert.IsTrue(query);
        }


    }
}
