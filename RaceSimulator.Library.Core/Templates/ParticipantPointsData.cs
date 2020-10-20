using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantPointsData : ITemplateData
    {
        public string Name { get; set; }
        
        public int Points { get; set; }

        public ParticipantPointsData(IParticipant p): this(p.Name, p.Points) { }

        public ParticipantPointsData(string name, int points)
        {
            Name = name;
            Points = points;
        }

        public void AddPoints(int points)
        {
            Points += points;
        }
    }
}
