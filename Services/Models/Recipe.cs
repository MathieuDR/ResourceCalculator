using System.Collections.Generic;

namespace Services.Models {
    public record Recipe : Entity {
        public string Name { get; init; }
        public string Description { get; init; }
        public List<Ingredient> Ingredients { get; init; }
        public MachineStep Processor { get; init; }
    }
}