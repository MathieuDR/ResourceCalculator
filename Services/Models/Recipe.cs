using System.Collections.Generic;

namespace Services.Models {
    public class Recipe : Entity {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public MachineStep Processor { get; set; }
    }
}