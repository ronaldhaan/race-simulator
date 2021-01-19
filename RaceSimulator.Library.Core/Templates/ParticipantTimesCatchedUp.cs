using RaceSimulator.Library.Core.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimesCatchedUp : TemplateData, IParticipantData
    {
        public string Name { get; set; }

        public int TimesCatchedUp { get; set; }
        public IParticipant Participant { get; set ; }

        public ParticipantTimesCatchedUp() { }

        public ParticipantTimesCatchedUp(IParticipant p, int times)
        {
            Participant = p;
            Name = p.Name;
            TimesCatchedUp = times;
        }

        public override List<IParticipantData> AddTo(List<IParticipantData> value)
        {
            if (value == null) return null;

            if (value.Contains(this))
            {
                TimesCatchedUp++;
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

            List<ParticipantTimesCatchedUp> data = GetTemplateData<ParticipantTimesCatchedUp>(participantsData).ToList();

            int maxTimesCatchedUp = data.Max(x => x.TimesCatchedUp);

            string name = Name;

            if (maxTimesCatchedUp > TimesCatchedUp)
            {
                name = data.Find(x => x.TimesCatchedUp == maxTimesCatchedUp).Name;
            }

            return name;
        }

        public override bool Equals(object obj)
        {
            return obj is ParticipantTimesCatchedUp pData && pData.Name == Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
