using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimesCatchedUp
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
