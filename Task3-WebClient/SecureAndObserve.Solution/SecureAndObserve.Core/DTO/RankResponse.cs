using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class RankResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PayPerHour { get; set; }
        public string? Description { get; set; }
    }
    public static class RankExtensions
    {
        public static RankResponse ToRankResponse(this Rank rank)
        {
            return new RankResponse()
            {
                Id = rank.Id,
                Name = rank.Name,
                PayPerHour = rank.PayPerHour,
                Description = rank.Description
            };
        }
    }
}
