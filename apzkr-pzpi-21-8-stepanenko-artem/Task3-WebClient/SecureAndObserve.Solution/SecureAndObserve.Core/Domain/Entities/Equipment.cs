using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.Entities
{
    public class Equipment
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Amount { get; set; } = 0;
    }
}
