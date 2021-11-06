using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace Services.Repositories {
    internal class MachineRepository : BaseLocalStorageRepository<Machine>, IMachineRepository {
        public MachineRepository(ILogger<MachineRepository> logger, ILocalStorageService localStorageService) : base(
            logger, localStorageService) {
        }

        protected override string RepositoryPrefix => "Machine";

        public async Task<Machine> GetByNameAsync(string name) {
            var machines = await GetAll();
            return machines.FirstOrDefault(m => m.Name == name);
        }
    }
}