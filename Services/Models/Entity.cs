using System;

namespace Services.Models {
    public abstract record Entity {
        public Guid Id { get; init; }
    }
}