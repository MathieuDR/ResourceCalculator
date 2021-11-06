namespace Services.Models {
    public record MachineStep : Entity {
        public Machine Machine { get; init; }
        public PowerStage Stage { get; init; }
        public int Voltage { get; init; }
        public int Amperage { get; init; }
        public int Time { get; init; }
        public int Ticks => Time * 20;
        public int TotalPower => Voltage * Ticks * Amperage;
    }
}