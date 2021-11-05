using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign;
using Common.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Web.Models;
using Web.Services;

namespace Web.Pages.Dashboard.Analysis {
    public partial class Index : ComponentBase {
        private Machine _machine = new();

        // collection of machines
        private List<Machine> _machines;


        protected override async Task OnInitializedAsync() {
            _machines = (await MachineService.GetMachines()).Value.ToList();
            await base.OnInitializedAsync();
        }

        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public IMachineService MachineService { get; set; }

        private async Task OnFinish(EditContext editContext) {
            if (!editContext.Validate()) {
                await OnFinishFailed(editContext);
            }

            var result = await MachineService.AddMachine(_machine);
            if (result.IsSuccess) {
                _machines.Add(result.Value);
                _machine = new();
                _ = MessageService.Success("Successfully added a machine");
                StateHasChanged();
            }
            else {
                _ = MessageService.Error($"Could not add machine: {result.CombineMessage(", ")}");
            }
        }

        private async Task OnFinishFailed(EditContext editContext) {
            _ =  MessageService.Error(
                $"Could not add machine: {string.Join(", ", editContext.GetValidationMessages())}");
        }
    }
}