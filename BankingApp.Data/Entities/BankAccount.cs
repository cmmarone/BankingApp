using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Data.Entities
{
    public enum AccountType
    {
        Checking,
        [Display(Name = "Corporate Investment")]
        CorporateInvestment,
        [Display(Name = "Individual Investment")]
        IndividualInvestment
    }

    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Bank))]
        public int BankId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public Guid OwnerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        // navigation properties
        public virtual Bank Bank { get; set; }
    }
}
