using Best.Models;

namespace Best.DataAccess.IRepository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category obj);
    void Save();

}
