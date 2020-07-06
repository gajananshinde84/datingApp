using System.Threading.Tasks;
using datingApp.API.Models;
using System.Collections.Generic;
namespace datingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
    }
}