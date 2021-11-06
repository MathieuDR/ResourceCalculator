using System.Threading.Tasks;
using Services.Models;

namespace Services.Repositories {
    internal interface IMachineRepository : IRepository<Machine> {
        // get by name async
        Task<Machine> GetByNameAsync(string name);
    }
}