using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class GuardReportResponse
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Guard can't be null")]
        public Guid GuardExstensionsId { get; set; }
        [Required(ErrorMessage = "Order can't be null")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "Date can't be null")]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Message can't be null")]
        public string? Message { get; set; }
        public string? Descriptions { get; set; }
    }

    public static class GuardReportExtentions
    {
        public static GuardReportResponse ToGuardReportResponse(this GuardReport guardReport)
        {
            return new GuardReportResponse()
            {
                Id = guardReport.Id,
                GuardExstensionsId = guardReport.GuardExstensionsId,
                OrderId = guardReport.OrderId,
                Date = guardReport.Date,
                Message = guardReport.Message,
                Descriptions = guardReport.Descriptions
            };
        }
    }
}
