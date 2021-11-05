namespace Web.Models {
    public class MachineStep : Entity{
        public Machine Machine { get; set; }
        public PowerStage Stage { get; set; }
        public int Voltage { get; set; }
        public int Amperage { get; set; }
        public int Time { get; set; }
        public int Ticks => Time * 20;
        public int TotalPower => Voltage * Ticks * Amperage;
    }
}