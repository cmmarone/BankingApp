using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.ActionModels
{
    public class Withdrawal
    {
        [Display(Name = "Account number")]
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
