using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimesCatchedUp : ITemplateData
    {
        public string Name { get; set; }

        public int TimesCatchedUp { get; set; }

        public ParticipantTimesCatchedUp() { }

        public ParticipantTimesCatchedUp(string name, int times)
        {
            Name = name;
            TimesCatchedUp = times;
        }
    }
}
