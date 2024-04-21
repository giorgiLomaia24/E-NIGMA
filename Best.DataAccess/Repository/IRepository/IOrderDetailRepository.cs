using Best.Models;

namespace Best.DataAccess.IRepository.IRepository;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail obj);
    void Save();

}
