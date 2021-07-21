using BankingApp.Data;
using BankingApp.Data.Entities;
using BankingApp.Models.ActionModels;
using BankingApp.Models.BankAccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Services
{
    public class BankAccountService
    {
        private readonly Guid _ownerId;
        public BankAccountService(Guid ownerId) => _ownerId = ownerId;

        public IEnumerable<BankAccountListItem> GetUserBankAccounts()
        {
            using (var context = new ApplicationDbContext())
            {
                var accountList = context.BankAccounts.Where(a => a.OwnerId == _ownerId)
                    .Select(a => new BankAccountListItem
                    {
                        AccountNumber = a.Id.ToString("D9"),
                        AccountType = a.AccountType
                    }).ToList().OrderByDescending(a => a.AccountNumber);
                return accountList;
            }
        }

        public BankAccountDetail GetBankAccountById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.BankAccounts.FirstOrDefault(a => a.Id == id && a.OwnerId == _ownerId);
                return new BankAccountDetail
                {
                    BankName = entity.Bank.Name,
                    AccountNumber = entity.Id.ToString("D9"),
                    AccountType = entity.AccountType,
                    Balance = entity.Balance
                };
            }
        }

        public BankAccountDetailAdmin GetBankAccountByIdAdmin(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.BankAccounts.FirstOrDefault(a => a.Id == id);
                return new BankAccountDetailAdmin
                {
                    BankName = entity.Bank.Name,
                    FullName = $"{entity.LastName}, {entity.FirstName}",
                    AccountNumber = entity.Id.ToString("D9"),
                    AccountType = entity.AccountType,
                    Balance = entity.Balance
                };
            }
        }

        public bool CreateBankAccount(BankAccountCreate model)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = new BankAccount
                {
                    BankId = (context.Banks.FirstOrDefault(b => b.Name.ToLower() == model.BankName.ToLower())).Id,
                    AccountType = model.AccountType,
                    Balance = 0,
                    OwnerId = _ownerId,
                    LastName = model.LastName,
                    FirstName = model.FirstName
                };
                context.BankAccounts.Add(account);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdateBankAccount(BankAccountEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.BankAccounts.FirstOrDefault(a => a.Id == model.BankAccountId);

                entity.BankId = (context.Banks.FirstOrDefault(b => b.Name.ToLower() == model.BankName.ToLower())).Id;
                entity.AccountType = model.AccountType;
                entity.Balance = model.Balance;
                entity.LastName = model.LastName;
                entity.FirstName = model.FirstName;
                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteBankAccount(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.BankAccounts.FirstOrDefault(a => a.Id == id);
                context.BankAccounts.Remove(entity);
                return context.SaveChanges() == 1;
            }
        }

        public bool MakeDeposit(Deposit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);

                account.Balance += model.Amount;
                return context.SaveChanges() == 1;
            }
        }

        public bool MakeWithdrawal(Withdrawal model)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);

                account.Balance -= model.Amount;
                return context.SaveChanges() == 1;
            }
        }

        public bool MakeTransfer(Transfer model)
        {
            using (var context = new ApplicationDbContext())
            {
                var sendingAccount = context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);
                var receivingAccount = context.BankAccounts.FirstOrDefault(a => a.Id == model.TargetAccount);
                sendingAccount.Balance -= model.Amount;
                receivingAccount.Balance += model.Amount;
                return context.SaveChanges() == 2;
            }
        }
    }
}