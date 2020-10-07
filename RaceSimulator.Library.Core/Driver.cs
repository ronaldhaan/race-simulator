using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Core
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColor TeamColor { get; set; }

        public Driver(string name, int points, IEquipment equipment, TeamColor teamColor)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
        }
    }
}
