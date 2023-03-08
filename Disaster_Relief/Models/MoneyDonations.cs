using System;
using System.ComponentModel.DataAnnotations;

namespace Disaster_Relief.Models
{
    public class MoneyDonations
    {
        [Key]
        
        public int DonationID { get; set; }

        public string DonationName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]

        //code attribution
        //this method was taken from Stack Overflow
        //https://stackoverflow.com/questions/45361586/asp-net-mvc-core-date-only-format-is-not-working-using-dataannotation
        //nam
        //https://stackoverflow.com/users/1232087/nam
        [DataType(DataType.Date)]
        public DateTime? DonationDate { get; set; }
    }
}
