using SecureAndObserve.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SecureAndObserve.Core.DTO
{
    public class OrderAddRequest
    {
        public Guid OwnerId { get; set; }
        [Required(ErrorMessage ="Type of service can't be blank")]
        public string? TypeOfService { get; set; }
        [Required(ErrorMessage = "Security level can't be blank")]
        public string? SecurityLevel { get; set; }
        public Order ToOrder()
        {
            return new Order()
            {
                OwnerId = OwnerId,
                TypeOfService = TypeOfService,
                SecurityLevel = SecurityLevel
            };
        }
    }
}
