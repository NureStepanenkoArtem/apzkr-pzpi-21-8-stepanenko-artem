using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id {  get; set; }
        [ForeignKey("Users")]
        public Guid OwnerId { get; set; }
        public string? TypeOfService { get; set; }
        public string? SecurityLevel { get; set; }
    }
}
