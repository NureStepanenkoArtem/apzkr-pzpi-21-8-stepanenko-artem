using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.Entities
{
    public class Rank
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name of Rank can't be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pay per hour value of Rank can't be blank")]
        public int PayPerHour { get; set; }
        public string? Description { get; set; }
    }
}
