using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Disaster_Relief.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }

        public Decimal AmountSpent { get; set; }
    }
}
