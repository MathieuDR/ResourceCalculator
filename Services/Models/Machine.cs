using System.ComponentModel.DataAnnotations;

namespace Services.Models {
    public class Machine : Entity {
        [Required]
        public string Name { get; set; }
    }
}