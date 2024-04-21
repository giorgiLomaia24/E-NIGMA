using Best.Models;

namespace Best.DataAccess.IRepository.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company obj);
    void Save();

}
