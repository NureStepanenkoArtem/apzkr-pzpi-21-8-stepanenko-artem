using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class GuardReportAddRequest
    {
        [Required(ErrorMessage ="Guard can't be null")]
        public Guid GuardExstensionsId { get; set; }
        [Required(ErrorMessage ="Order can't be null")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage ="Date can't be null")]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage ="Message can't be null")]
        public string? Message { get; set; }
        public string? Descriptions { get; set; }
        public GuardReport ToGuardReport()
        {
            return new GuardReport()
            {
                GuardExstensionsId = GuardExstensionsId,
                OrderId = OrderId,
                Date = Date,
                Message = Message,
                Descriptions = Descriptions
            };
        }
    }
}
