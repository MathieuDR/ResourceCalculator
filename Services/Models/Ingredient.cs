namespace Services.Models {
    public record Ingredient : Entity {
        public string Name { get; init; }
        public string Quantity { get; init; }
    }
}