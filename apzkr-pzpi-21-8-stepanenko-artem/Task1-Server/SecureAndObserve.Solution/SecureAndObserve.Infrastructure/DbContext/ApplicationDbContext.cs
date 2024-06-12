using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { 

        }

        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentClaims> EquipmentClaims { get; set; }
        public virtual DbSet<GuardExstensions> GuardExstensions { get; set; }
        public virtual DbSet<GuardReport> GuardReports { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderGuards> OrderGuards { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Equipment>().ToTable("Equipment");
            builder.Entity<EquipmentClaims>().ToTable("EquipmentClaims");
            builder.Entity<GuardExstensions>().ToTable("GuardExstensions");
            builder.Entity<GuardReport>().ToTable("GuardReports");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<OrderGuards>().ToTable("OrderGuards");
            builder.Entity<Rank>().ToTable("Ranks");
            builder.Entity<Territory>().ToTable("Territories");

            base.OnModelCreating(builder);
        }

    }
}
