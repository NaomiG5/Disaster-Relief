using System;
using System.ComponentModel.DataAnnotations;

namespace Disaster_Relief.Models
{
    public class Disaster
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }

        //code attribution
        //this method was taken from Stack Overflow
        //https://stackoverflow.com/questions/45361586/asp-net-mvc-core-date-only-format-is-not-working-using-dataannotation
        //nam
        //https://stackoverflow.com/users/1232087/nam
        [DataType(DataType.Date)]
        public DateTime SDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EDate { get; set; }
        public string AidNeeded { get; set; }
    

    }
}
