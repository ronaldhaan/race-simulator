using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimeData : TemplateData, IParticipantData
    {
        public string Name { get; set; }

        public TimeSpan TimeSpan { get; set; }
        public IParticipant Participant { get; set; }

        public override List<IParticipantData> AddTo(List<IParticipantData> value)
        {
            if (value == null) return null;

            if (value.Contains(this))
            {
                TimeSpan = new TimeSpan(DateTime.Now.Ticks);
            }
            else
            {
                value.Add(this);
            }

            return value;
        }

        public string FindBest(List<IParticipantData> pData)
        {
            if (pData == null || pData.Count == 0)
            {
                return string.Empty;
            }

            List<ParticipantTimeData> pTimeData = GetTemplateData<ParticipantTimeData>(pData).ToList();

            long minTicks = pTimeData.Min(x => x.TimeSpan.Ticks);

            string name = Name;

            if (minTicks < TimeSpan.Ticks)
            {
                name = pTimeData.Find(x => x.TimeSpan.Ticks == minTicks).Name;
            }

            return name;
        }

        public override bool Equals(object obj)
        {
            return obj is ParticipantTimeData pData && pData.Name == Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}