using System.ComponentModel.DataAnnotations;

namespace Services.Models {
    public record Machine : Entity {
        [Required]
        public string Name { get; init; }
    }
}