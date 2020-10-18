using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimeData : IParticipantData
    {
        public string Name { get; set; }

        public TimeSpan TimeSpan { get; set; }
    }
}
