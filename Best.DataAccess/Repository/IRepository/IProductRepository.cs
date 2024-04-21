using Best.DataAccess.IRepository.IRepository;
using Best.Models;

namespace Best.DataAccess.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        void Save();
    }
}
