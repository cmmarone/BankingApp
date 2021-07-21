﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.BankModels
{
    public class BankListItem
    {
        [Display(Name = "Bank Id")]
        public int BankId { get; set; }
        public string Name { get; set; }
    }
}
