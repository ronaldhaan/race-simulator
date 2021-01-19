using System;
using System.Collections.Generic;
using System.Linq;

using RaceSimulator.Library.Core.Interfaces;


namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimePerSectionData : TemplateData, IParticipantData
    {
        public string Name { get; set; }

        public TimeSpan TotalTime { get => TimeSpan.FromMilliseconds(TimePerSection.Values.Sum(x => Convert.ToInt64(x.TotalMilliseconds))); }

        public Dictionary<Section, TimeSpan> TimePerSection { get; set; }

        public ParticipantTimePerSectionData(string name, Section section, TimeSpan timespan)
        {
            Name = name;
            TimePerSection = new Dictionary<Section, TimeSpan>() { { section, timespan } };
        }

        public IParticipant Participant { get; set; }

        public override List<IParticipantData> AddTo(List<IParticipantData> value)
        {
            if (value == null) return null;

            if (value.Contains(this))
            {
                int index = value.IndexOf(this);
                var time = (ParticipantTimePerSectionData)value[index];
                var merged = Merge(new Dictionary<Section, TimeSpan>[] { time.TimePerSection, TimePerSection });
                time.TimePerSection.Clear();
                time.TimePerSection = merged;
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

            List<ParticipantTimePerSectionData> pTimeData = GetTemplateData<ParticipantTimePerSectionData>(pData).ToList();

            long minTicks = pTimeData.Min(x => x.TotalTime.Ticks);

            string name = Name;

            if (minTicks < TotalTime.Ticks)
            {
                name = pTimeData.Find(x => x.TotalTime.Ticks == minTicks).Name;
            }

            return name;
        }

        public override bool Equals(object obj)
        {
            return obj is ParticipantTimePerSectionData pData && pData.Name == Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
