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
    public class GuardExstensionsAddRequest
    {
        public Guid UserId { get; set; }
        public Guid RankId { get; set; }
        public GuardExstensions ToGuardExstensions()
        {
            return new GuardExstensions()
            {
                UserId = UserId,
                RankId = RankId
            };
        }
    }
}
