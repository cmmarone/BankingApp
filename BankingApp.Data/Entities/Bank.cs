using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Data.Entities
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // navigation properties
        public virtual ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();
    }
}
