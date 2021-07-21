using BankingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankAccountModels
{
    public class BankAccountCreate
    {
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }
    }
}
