using System;

namespace Web.Models {
    public abstract class Entity {
        public Guid Id { get; } = Guid.NewGuid();
    }
}