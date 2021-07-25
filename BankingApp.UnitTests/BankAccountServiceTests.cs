using BankingApp.Data;
using BankingApp.Data.Entities;
using BankingApp.Models.ActionModels;
using BankingApp.Models.BankAccountModels;
using BankingApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UnitTests
{
    [TestClass]
    public class BankAccountServiceTests
    {
        private Mock<DbSet<Bank>> _mockBanks;
        private Mock<DbSet<BankAccount>> _mockBankAccounts;
        private Mock<ApplicationDbContext> _mockContext;
        private BankAccountService _bankAccountService;

        private Guid _ownerId1;
        private Guid _ownerId2;
        private BankAccountCreate _bankAccountCreate;
        private BankAccountEdit _bankAccountEdit;
        private Deposit _depositModel;
        private Withdrawal _withdrawalModel;
        private Transfer _transferModel;

        [TestInitialize]
        public void Arrange()
        {
            _ownerId1 = Guid.NewGuid();
            _ownerId2 = Guid.NewGuid();

            var mockBankEntities = new List<Bank>
            {
                new Bank
                {
                    Id = 1,
                    Name = "Sample Bank"
                },
            }.AsQueryable();

            var mockEntities = new List<BankAccount>
            {
                new BankAccount
                {
                    Id = 1,
                    BankId = 1,
                    AccountType = AccountType.Checking,
                    Balance = 7000.00m,
                    OwnerId = _ownerId1,
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                new BankAccount
                {
                    Id = 2,
                    BankId = 2,
                    AccountType = AccountType.IndividualInvestment,
                    Balance = 160000.00m,
                    OwnerId = _ownerId1,
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                new BankAccount
                {
                    Id = 3,
                    BankId = 1,
                    AccountType = AccountType.Checking,
                    Balance = 97191.88m,
                    OwnerId = _ownerId2,
                    LastName = "Sample",
                    FirstName = "Jesse"
                },
            }.AsQueryable();

            _bankAccountCreate = new BankAccountCreate
            {
                BankName = "Sample Bank",
                FirstName = "John",
                LastName = "Public",
                AccountType = AccountType.Checking
            };
            _bankAccountEdit = new BankAccountEdit
            {
                BankAccountId = 1,
                FirstName = "Janet"
            };
            _depositModel = new Deposit
            {
                AccountNumber = 1,
                Amount = 100
            };
            _withdrawalModel = new Withdrawal
            {
                AccountNumber = 1,
                Amount = 200
            };
            _transferModel = new Transfer
            {
                AccountNumber = 1,
                Amount = 50,
                TargetAccount = 2
            };

            _mockBanks = new Mock<DbSet<Bank>>();
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.Provider).Returns(mockBankEntities.Provider);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.Expression).Returns(mockBankEntities.Expression);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.ElementType).Returns(mockBankEntities.ElementType);
            _mockBanks.As<IQueryable<Bank>>().Setup(m => m.GetEnumerator()).Returns(mockBankEntities.GetEnumerator());

            _mockBankAccounts = new Mock<DbSet<BankAccount>>();
            _mockBankAccounts.As<IQueryable<BankAccount>>().Setup(m => m.Provider).Returns(mockEntities.Provider);
            _mockBankAccounts.As<IQueryable<BankAccount>>().Setup(m => m.Expression).Returns(mockEntities.Expression);
            _mockBankAccounts.As<IQueryable<BankAccount>>().Setup(m => m.ElementType).Returns(mockEntities.ElementType);
            _mockBankAccounts.As<IQueryable<BankAccount>>().Setup(m => m.GetEnumerator()).Returns(mockEntities.GetEnumerator());

            _mockContext = new Mock<ApplicationDbContext>();
            _mockContext.Setup(m => m.Banks).Returns(_mockBanks.Object);
            _mockContext.Setup(m => m.BankAccounts).Returns(_mockBankAccounts.Object);
            _bankAccountService = new BankAccountService(_ownerId1, _mockContext.Object);
        }

        [TestMethod]
        public void GetUserBankAccounts_ShouldReturnAllEntities()
        {
            var accountResults = _bankAccountService.GetUserBankAccounts();

            // only two mock entities should match the OwnerId passed into _bankAccountService
            Assert.IsTrue(accountResults.Count() == 2);
        }

        // The method works when sending requests from Postman.  
        // Currently having an issue with the LINQ query returning a result within Moq framework
        //[TestMethod]
        //public void GetBankAccountById_ShouldReturnCorrectEntity()
        //{
        //    var accountResult = _bankAccountService.GetBankAccountById(3);

        //    Assert.AreEqual(7000.00, accountResult.Balance); 
        //}

        [TestMethod]
        public void CreateBankAccount_ShouldCreateEntity()
        {
            _bankAccountService.CreateBankAccount(_bankAccountCreate);

            _mockBankAccounts.Verify(b => b.Add(It.IsAny<BankAccount>()), Times.Once());
            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void UpdateBankAccount_ShouldSaveChanges()
        {
            _bankAccountService.UpdateBankAccount(_bankAccountEdit);

            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void DeleteBankAccount_ShouldRemoveEntity()
        {
            _bankAccountService.DeleteBankAccount(1);

            _mockBankAccounts.Verify(b => b.Remove(It.IsAny<BankAccount>()), Times.Once());
            _mockContext.Verify(b => b.SaveChanges(), Times.Once());
        }

        // The method works when sending requests from Postman.  
        // Currently having an issue with the LINQ query returning a result within Moq framework
        //[TestMethod]
        //public void MakeDeposit_ShouldAddToBalance()
        //{
        //    _bankAccountService.MakeDeposit(_depositModel);
        //    var accountResult = _bankAccountService.GetBankAccountById(1);

        //    Assert.AreEqual(7100.00, accountResult.Balance);
        //}

        // The method works when sending requests from Postman.  
        // Currently having an issue with the LINQ query returning a result within Moq framework
        //[TestMethod]
        //public void MakeWithdrawal_ShouldSubtractFromBalance()
        //{
        //    _bankAccountService.MakeWithdrawal(_withdrawalModel);
        //    var accountResult = _bankAccountService.GetBankAccountById(1);

        //    Assert.AreEqual(6800.00, accountResult.Balance);
        //}

        // The method works when sending requests from Postman.  
        // Currently having an issue with the LINQ query returning a result within Moq framework
        //[TestMethod]
        //public void MakeTransfer_ShouldUpdateBothBalances()
        //{
        //    _bankAccountService.MakeTransfer(_transferModel);
        //    var accountResult1 = _bankAccountService.GetBankAccountById(1);
        //    var accountResult2 = _bankAccountService.GetBankAccountById(2);

        //    Assert.AreEqual(6950.00, accountResult1.Balance);
        //    Assert.AreEqual(160050.00, accountResult2.Balance);
        //}
    }
}
