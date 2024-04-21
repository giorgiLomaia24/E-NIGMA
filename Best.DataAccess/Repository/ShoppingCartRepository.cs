using Best.DataAccess.IRepository.IRepository;
using Best.DataAccess.Repository.Repository;
using Best.Models;
using BestApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Best.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private  ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ShoppingCart obj)
        {
            // Detach existing entity with the same key if it's being tracked
        var existingEntity = _db.ChangeTracker.Entries<ShoppingCart>()
            .FirstOrDefault(e => e.Entity.Id == obj.Id && e.State != EntityState.Detached);

        if (existingEntity != null)
        {
            existingEntity.State = EntityState.Detached;
        }

        // Attach and update the modified entity
        _db.ShoppingCarts.Update(obj);
        
        }
    }
}
