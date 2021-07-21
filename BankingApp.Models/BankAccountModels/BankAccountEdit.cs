using BankingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankAccountModels
{
    public class BankAccountEdit
    {
        [Display(Name = "Account Id")]
        public int BankAccountId { get; set; }
        [Display(Name = "Bank")]
        public string BankName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
