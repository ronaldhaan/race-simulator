using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantPointsData : IParticipantData
    {
        public string Name { get; set; }
        
        public int Points { get; set; }
    }
}
