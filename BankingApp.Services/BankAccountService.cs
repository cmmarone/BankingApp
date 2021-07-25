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
        private readonly ApplicationDbContext _context;

        public BankAccountService(Guid ownerId, ApplicationDbContext context)
        {
            _ownerId = ownerId;
            _context = context;
        }

        public IEnumerable<BankAccountListItem> GetUserBankAccounts()
        {
            var accountList = _context.BankAccounts.Where(a => a.OwnerId == _ownerId).ToList();
            var listItemList = new List<BankAccountListItem>();
            foreach (var account in accountList)
            {
                var listItem = new BankAccountListItem
                {
                    AccountNumber = account.Id.ToString("D9"),
                    AccountType = account.AccountType.ToString()
                };
                listItemList.Add(listItem);
            }
            return listItemList.OrderBy(a => a.AccountNumber);
        }

        public BankAccountDetail GetBankAccountById(int id)
        {
            var entity = _context.BankAccounts.FirstOrDefault(a => a.Id == id && a.OwnerId == _ownerId);
            return new BankAccountDetail
            {
                BankName = entity.Bank.Name,
                AccountNumber = entity.Id.ToString("D9"),
                AccountType = entity.AccountType.ToString(),
                Balance = entity.Balance
            };

        }

        public BankAccountDetailAdmin GetBankAccountByIdAdmin(int id)
        {
            var entity = _context.BankAccounts.FirstOrDefault(a => a.Id == id);
            return new BankAccountDetailAdmin
            {
                BankName = entity.Bank.Name,
                FullName = $"{entity.LastName}, {entity.FirstName}",
                AccountNumber = entity.Id.ToString("D9"),
                AccountType = entity.AccountType.ToString(),
                Balance = entity.Balance
            };
        }

        public bool CreateBankAccount(BankAccountCreate model)
        {
            var account = new BankAccount
            {
                BankId = (_context.Banks.FirstOrDefault(b => b.Name.ToLower() == model.BankName.ToLower())).Id,
                AccountType = model.AccountType,
                Balance = 0,
                OwnerId = _ownerId,
                LastName = model.LastName,
                FirstName = model.FirstName
            };
            _context.BankAccounts.Add(account);
            return _context.SaveChanges() == 1;
        }

        public bool UpdateBankAccount(BankAccountEdit model)
        {
            var entity = _context.BankAccounts.FirstOrDefault(a => a.Id == model.BankAccountId);

            if (model.BankName != null)
                entity.BankId = (_context.Banks.FirstOrDefault(b => b.Name.ToLower() == model.BankName.ToLower())).Id;
            if (model.AccountType != null)
                entity.AccountType = model.AccountType ?? AccountType.Checking;
            if (model.Balance != null)
                entity.Balance = model.Balance ?? 0;
            if (model.LastName != null)
                entity.LastName = model.LastName;
            if (model.FirstName != null)
                entity.FirstName = model.FirstName;
            return _context.SaveChanges() == 1;
        }

        public bool DeleteBankAccount(int id)
        {
            var entity = _context.BankAccounts.FirstOrDefault(a => a.Id == id);
            _context.BankAccounts.Remove(entity);
            return _context.SaveChanges() == 1;

        }

        public bool MakeDeposit(Deposit model)
        {
            var account = _context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);
            account.Balance += model.Amount;
            return _context.SaveChanges() == 1;
        }

        public int MakeWithdrawal(Withdrawal model)
        {
            var account = _context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);
            if (account.AccountType == AccountType.IndividualInvestment)
                if (model.Amount > 1000) return 3;
            if (account.Balance < model.Amount) return 4;

            account.Balance -= model.Amount;
            if (_context.SaveChanges() == 1) return 1;
            else return 2;
        }

        public int MakeTransfer(Transfer model)
        {
            var sendingAccount = _context.BankAccounts.FirstOrDefault(a => a.Id == model.AccountNumber && a.OwnerId == _ownerId);
            var receivingAccount = _context.BankAccounts.FirstOrDefault(a => a.Id == model.TargetAccount);

            if (sendingAccount.Balance < model.Amount) return 3;

            sendingAccount.Balance -= model.Amount;
            receivingAccount.Balance += model.Amount;
            if (_context.SaveChanges() == 2) return 1;
            else return 2;
        }
    }
}