using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantPointsData : TemplateData, IParticipantData
    { 
        public string Name { get; set; }
        
        public int Points { get; set; }

        public IParticipant Participant { get; set; }

        public ParticipantPointsData(IParticipant p): this(p.Name, p.Points) 
        {
            Participant = p;
        }

        public ParticipantPointsData(string name, int points)
        {
            Name = name;
            Points = points == 0 ? 1 : points;
        }

        public override List<IParticipantData> AddTo(List<IParticipantData> value)
        {
            if (value == null) return null;

            if (value.Contains(this))
            {
                Points++;
            }
            else
            {
                value.Add(this);
            }

            return value;
        }

        public string FindBest(List<IParticipantData> participantsData)
        {
            if (participantsData == null || participantsData.Count == 0)
            {
                return string.Empty;
            }

            var data = GetTemplateData<ParticipantPointsData>(participantsData).ToList();

            int maxpoints = data.Max(x => x.Points);

            string name = Name;

            if (maxpoints > Points)
            {
                name = data.Find(x => x.Points == maxpoints).Name;
            }

            return name;
        }

        public override bool Equals(object obj)
        {
            return obj is ParticipantPointsData pData && pData.Name == Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
