using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FluentResults;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace Services.Repositories {
    internal class MachineRepository : BaseLocalStorageRepository<Machine>, IMachineRepository {
        public MachineRepository(ILogger<MachineRepository> logger, ISyncLocalStorageService localStorageService) : base(
            logger, localStorageService) {
        }

        protected override string RepositoryPrefix => "Machine";

        public async Task<Machine> GetByNameAsync(string name) {
            var machines = await GetAll();
            return machines.FirstOrDefault(m => m.Name == name);
        }

        public async Task<Result> SeedDatabase() {
            var isSuccess = await SeedDatabase(new[] {
                new Machine {
                    Name = "Machine 1",
                },
                new Machine {
                    Name = "Machine 2",
                }
            });
            
            return isSuccess ? Result.Ok() : Result.Fail("Failed to seed database");
        }
    }
}