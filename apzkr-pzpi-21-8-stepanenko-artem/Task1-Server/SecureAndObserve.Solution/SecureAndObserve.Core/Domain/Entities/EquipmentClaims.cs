using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.Entities
{
    public class EquipmentClaims
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("GuardExstensions")]
        public Guid GuardExstensionsId {  get; set; }
        [ForeignKey("Equipment")]
        public Guid EquipmentId { get; set; }
    }
}
