using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class EquipmentClaimsRepository : IEquipmentClaimsRepository
    {
        private readonly ApplicationDbContext _db;
        public EquipmentClaimsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<EquipmentClaims> AddEquipmentClaims(EquipmentClaims equipmentClaims)
        {
            await _db.EquipmentClaims.AddAsync(equipmentClaims);
            await _db.SaveChangesAsync();
            return equipmentClaims;
        }
        public async Task<List<EquipmentClaims>> GetAllEquipmentClaims()
        {
            return await _db.EquipmentClaims.ToListAsync();
        }
        public async Task<EquipmentClaims?> GetEquipmentClaimsByEquipmentClaimsId(Guid equipmentClaimsId)
        {
            return await _db.EquipmentClaims.FirstOrDefaultAsync(temp => temp.Id == equipmentClaimsId);
        }
        public async Task<bool> DeleteEquipmentClaimsByEquipmentClaimsId(Guid equipmentClaimsId)
        {
            _db.EquipmentClaims.RemoveRange(_db.EquipmentClaims.Where(temp => temp.Id == equipmentClaimsId));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
