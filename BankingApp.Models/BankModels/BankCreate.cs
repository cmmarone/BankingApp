using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankModels
{
    public class BankCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
