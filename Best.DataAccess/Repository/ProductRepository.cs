using System.Linq;
using Best.DataAccess.IRepository;
using Best.DataAccess.Repository.Repository;
using Best.Models;
using BestApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Best.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

  public void Update(Product obj)
{
    var existingEntity = _db.Products.Find(obj.Id);

    if (existingEntity != null)
    {
        // Update the properties of the existing entity
        existingEntity.Title = obj.Title;
        existingEntity.ISBN = obj.ISBN;
        existingEntity.ListPrice = obj.ListPrice;
        existingEntity.Price = obj.Price;
        existingEntity.Price50 = obj.Price50;
        existingEntity.Price100 = obj.Price100;
        existingEntity.Description = obj.Description;
        existingEntity.Author = obj.Author;
        existingEntity.Status = obj.Status;
        existingEntity.CategoryId = obj.CategoryId;

        // Update the ImageUrl only if a new image is provided
        if (!string.IsNullOrEmpty(obj.ImageUrl))
        {
            existingEntity.ImageUrl = obj.ImageUrl;
        }

        _db.SaveChanges();
    }
}


    }
}
