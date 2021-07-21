using BankingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankAccountModels
{
    public class BankAccountListItem
    {
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
    }
}
