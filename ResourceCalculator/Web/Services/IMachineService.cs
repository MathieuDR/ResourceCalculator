using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Web.Models;

namespace Web.Services {
    public interface IMachineService {
        Task<Result<Machine>> GetMachine(Guid id);
        Task<Result<Machine>> GetMachineByName(string machineName);
        Task<Result<IEnumerable<Machine>>> GetMachines();
        Task<Result<Machine>> AddMachine(Machine machine);
        Task<Result<Machine>> UpdateMachine(Machine machine);
        Task<Result> DeleteMachine(Machine machine);
        Task<Result> DeleteMachineById(Guid id);
    }
}