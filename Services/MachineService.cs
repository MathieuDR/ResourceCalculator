using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Services.Models;
using Services.Repositories;

namespace Services {
    internal class MachineService : IMachineService {
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<MachineService> _logger;

        public MachineService(IMachineRepository machineRepository, ILogger<MachineService> logger) {
            _machineRepository = machineRepository;
            _logger = logger;

            _ = SeedDatabase(); // Fire and forget
        }
        
       public async Task<Result<Machine>> GetMachine(Guid id) {
            _logger.LogInformation("Getting machine with id {id}", id);
            var machine = await _machineRepository.Get(id);

            return machine is null ? Result.Fail<Machine>("Machine not found") : Result.Ok(machine);
        }

        public async Task<Result<Machine>> GetMachineByName(string machineName) {
            _logger.LogInformation($"Getting machine with name {machineName}");

            var machine = await _machineRepository.GetByNameAsync(machineName);
            return machine is null ? Result.Fail<Machine>("Machine not found") : Result.Ok(machine);
        }

        public async Task<Result<IEnumerable<Machine>>> GetMachines() {
            _logger.LogInformation("Getting all machines");

            var machines = await _machineRepository.GetAll();
            return machines is null ? Result.Fail<IEnumerable<Machine>>("Machines not found") : Result.Ok(machines);
        }

        public async Task<Result<Machine>> AddMachine(Machine machine) {
            _logger.LogInformation($"Adding new machine {machine.Name}");

            var result = await _machineRepository.Add(machine);
            return result is null ? Result.Fail<Machine>("Machine not added") : Result.Ok(result);
        }

        public async Task<Result<Machine>> UpdateMachine(Machine machine) {
            _logger.LogInformation($"Updating machine with id {machine.Id}");

            var result = await _machineRepository.Update(machine);
            return result is null ? Result.Fail<Machine>("Machine not updated") : Result.Ok(result);
        }

        public async Task<Result> DeleteMachine(Machine machine) {
            _logger.LogInformation($"Deleting machine with id {machine.Id}");

            var result = await _machineRepository.Delete(machine);
            return result ? Result.Ok() : Result.Fail("Machine not deleted");
        }

        public async Task<Result> DeleteMachineById(Guid id) {
            _logger.LogInformation($"Deleting machine with id {id}");

            var result = await _machineRepository.Delete(id);
            return result ? Result.Ok() : Result.Fail("Machine not deleted");
        }
        
        // seed database
        private async Task<Result> SeedDatabase() {
            _logger.LogInformation("Seeding");
            
            var result = await _machineRepository.SeedDatabase();

            if (result.IsFailed) {
                _logger.LogError("Seeding failed: "+ result.CombineMessage());
            }
            return result;
        }
    }
}