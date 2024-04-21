using Best.Models;

namespace Best.DataAccess.IRepository.IRepository;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser obj);
    void Save();

}
