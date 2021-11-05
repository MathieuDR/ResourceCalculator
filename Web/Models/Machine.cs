using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models {
    public class Machine:Entity {
    [Required]
    public string Name { get; set; }
    }
}