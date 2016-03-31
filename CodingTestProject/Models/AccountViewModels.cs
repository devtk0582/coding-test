using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodingTestProject.Models
{
    public class AccountViewModel
    {
        public Account Account { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }

}
