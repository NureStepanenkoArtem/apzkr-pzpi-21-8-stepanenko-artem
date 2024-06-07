using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureAndObserve.Core.Domain.Entities;

namespace SecureAndObserve.Core.DTO
{
    public class GuardExstensionsResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RankId { get; set; }
    }
    public static class GuardExstensionsExtensions
    {
        public static GuardExstensionsResponse ToGuardExstensionsResponse(this GuardExstensions guardExstensions)
        {
            return new GuardExstensionsResponse()
            {
                Id = guardExstensions.Id,
                UserId = guardExstensions.UserId,
                RankId = guardExstensions.RankId,
            };
        }
    }
}
