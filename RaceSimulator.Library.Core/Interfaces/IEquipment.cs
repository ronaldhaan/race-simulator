namespace RaceSimulator.Library.Core.Interfaces
{
    public interface IEquipment
    {
        public int Performance { get; set; }
        
        public int Quality { get; set; }

        public int Speed { get; set; }

        public bool IsBroken { get; set; }
    }
}
