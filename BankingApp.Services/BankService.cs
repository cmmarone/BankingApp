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
        public IEnumerable<BankListItem> GetBanks()
        {
            using (var context = new ApplicationDbContext())
            {
                var bankList = context.Banks.Select(b => new BankListItem
                {
                    BankId = b.Id,
                    Name = b.Name
                }).ToList().OrderByDescending(b => b.Name);
                return bankList;
            }
        }

        public BankDetailAdmin GetBankById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Banks.FirstOrDefault(b => b.Id == id);

                var listItems = new List<BankAccountListItem>();
                foreach (var account in entity.Accounts)
                {
                    var listItem = new BankAccountListItem
                    {
                        AccountNumber = account.Id.ToString("D9"),
                        AccountType = account.AccountType
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
        }

        public bool CreateBank(BankCreate model)
        {
            using (var context = new ApplicationDbContext())
            {
                var bank = new Bank { Name = model.Name };
                context.Banks.Add(bank);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdateBank(BankEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Banks.FirstOrDefault(b => b.Id == model.BankId);
                entity.Name = model.Name;
                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteBank(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Banks.FirstOrDefault(b => b.Id == id);
                context.Banks.Remove(entity);
                return context.SaveChanges() == 1;
            }
        }
    }
}