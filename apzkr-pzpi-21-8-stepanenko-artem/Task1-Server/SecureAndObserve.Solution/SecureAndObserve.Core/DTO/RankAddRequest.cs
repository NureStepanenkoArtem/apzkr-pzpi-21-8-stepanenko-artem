using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class RankAddRequest
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Pay per hour value of Rank can't be blank")]
        public int PayPerHour { get; set; }
        public string? Description { get; set; }
        public Rank ToRank()
        {
            return new Rank
            {
                Name = Name,
                PayPerHour = PayPerHour,
                Description = Description
            };
        }
    }
}
