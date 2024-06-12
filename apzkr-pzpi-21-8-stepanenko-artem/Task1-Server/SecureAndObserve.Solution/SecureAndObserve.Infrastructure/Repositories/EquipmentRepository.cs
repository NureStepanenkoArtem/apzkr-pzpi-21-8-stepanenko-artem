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
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ApplicationDbContext _db;
        public EquipmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Equipment> AddEquipment(Equipment equipment)
        {
            await _db.Equipment.AddAsync(equipment);
            await _db.SaveChangesAsync();
            return equipment;
        }
        public async Task<List<Equipment>> GetAllEquipment()
        {
            return await _db.Equipment.ToListAsync();
        }
        public async Task<Equipment?> GetEquipmentByEquipmentId(Guid equipmentId)
        {
            return await _db.Equipment.FirstOrDefaultAsync(temp => temp.Id == equipmentId);
        }
        public async Task<bool> DeleteEquipmentByEquipmentId(Guid equipmentId)
        {
            _db.Equipment.RemoveRange(_db.Equipment.Where(temp => temp.Id == equipmentId));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
