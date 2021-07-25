using BankingApp.Data;
using BankingApp.Data.Entities;
using BankingApp.Models.BankAccountModels;
using BankingApp.Models.BankModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Services
{
    public class BankService
    {
        private readonly ApplicationDbContext _context;

        public BankService(ApplicationDbContext context) => _context = context;

        public IEnumerable<BankListItem> GetBanks()
        {
            var bankList = _context.Banks.Select(b => new BankListItem
            {
                BankId = b.Id,
                Name = b.Name
            }).ToList().OrderByDescending(b => b.Name);
            return bankList;
        }

        public BankDetailAdmin GetBankById(int id)
        {
            var entity = _context.Banks.FirstOrDefault(b => b.Id == id);

            var listItems = new List<BankAccountListItem>();
            foreach (var account in entity.Accounts)
            {
                var listItem = new BankAccountListItem
                {
                    AccountNumber = account.Id.ToString("D9"),
                    AccountType = account.AccountType.ToString()
                };
                listItems.Add(listItem);
            }

            return new BankDetailAdmin
            {
                BankId = entity.Id,
                Name = entity.Name,
                Accounts = listItems
            };
        }

        public bool CreateBank(BankCreate model)
        {
            var bank = new Bank { Name = model.Name };
            _context.Banks.Add(bank);
            return _context.SaveChanges() == 1;
        }

        public bool UpdateBank(BankEdit model)
        {
            var entity = _context.Banks.FirstOrDefault(b => b.Id == model.BankId);
            entity.Name = model.Name;
            return _context.SaveChanges() == 1;
        }

        public bool DeleteBank(int id)
        {
            var entity = _context.Banks.FirstOrDefault(b => b.Id == id);
            _context.Banks.Remove(entity);
            return _context.SaveChanges() == 1;
        }
    }
}