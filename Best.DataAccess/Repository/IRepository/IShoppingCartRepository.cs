using Best.Models;

namespace Best.DataAccess.IRepository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart obj);
    void Save();

}
