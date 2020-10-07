using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Core
{
    public class Car : IEquipment
    {
        public int Performance { get; set; }
        public int Quality { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public Car(int performance, int quality, int speed, bool isBroken)
        {
            Performance = performance;
            Quality = quality;
            Speed = speed;
            IsBroken = isBroken;
        }
    }
}
