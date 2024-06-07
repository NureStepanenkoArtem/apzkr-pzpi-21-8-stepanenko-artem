using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.Entities
{
    public class Territory
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Users")]
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Square { get; set; }
        public string? Description {  get; set; }
        public string? Type { get; set; }
    }
}
