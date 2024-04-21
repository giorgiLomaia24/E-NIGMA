using Best.DataAccess.IRepository.IRepository;
using Best.DataAccess.Repository.Repository;
using Best.Models;
using BestApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Best.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private  ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderHeader obj)
        {
            // Detach existing entity with the same key if it's being tracked
        var existingEntity = _db.ChangeTracker.Entries<OrderHeader>()
            .FirstOrDefault(e => e.Entity.Id == obj.Id && e.State != EntityState.Detached);

        if (existingEntity != null)
        {
            existingEntity.State = EntityState.Detached;
        }

        // Attach and update the modified entity
        _db.OrderHeaders.Update(obj);
        
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderheaderFromDb = _db.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if(orderheaderFromDb != null){
                orderheaderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus)){
                    orderheaderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
         var orderheaderFromDb = _db.OrderHeaders.FirstOrDefault(o => o.Id == id);
                if(!string.IsNullOrEmpty(sessionId)){
                orderheaderFromDb.SessionId = sessionId;       
                }
                if(!string.IsNullOrEmpty(paymentIntentId)){
                orderheaderFromDb.PaymentIntentId = paymentIntentId;
                orderheaderFromDb.PaymentDate = DateTime.Now;       
                }
        }
    }
}
