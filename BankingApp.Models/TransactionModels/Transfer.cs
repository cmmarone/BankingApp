using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.ActionModels
{
    public class Transfer
    {
        [Display(Name = "From Account Number")]
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "To Account Number")]
        public int TargetAccount { get; set; }
    }
}
