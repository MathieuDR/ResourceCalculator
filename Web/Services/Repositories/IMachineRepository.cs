using System.Threading.Tasks;
using Web.Models;

namespace Web.Services.Repositories {
    public interface IMachineRepository : IRepository<Machine> {
        // get by name async
        Task<Machine> GetByNameAsync(string name);
    }
}