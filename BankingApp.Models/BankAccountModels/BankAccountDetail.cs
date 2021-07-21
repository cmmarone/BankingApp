using BankingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankAccountModels
{
    public class BankAccountDetail
    {
        [Display(Name = "Bank")]
        public string BankName { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
