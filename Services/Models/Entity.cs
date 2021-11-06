using System;

namespace Services.Models {
    public abstract class Entity {
        public Guid Id { get; } = Guid.NewGuid();
    }
}