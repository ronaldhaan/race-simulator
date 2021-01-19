using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class TrackRaceData
    {
        public string TrackName { get; set; }
        public RaceData<ParticipantPointsData> ParticipantPointsData { get; set; }
        public RaceData<ParticipantTimeData> ParticipantTimeData { get; set; }
        public RaceData<ParticipantTimePerSectionData> ParticipantTimePerSectionData { get; set; }
        public RaceData<ParticipantTimesCatchedUp> ParticipantTimesCatchedUp { get; set; }

        public TrackRaceData()
        {
            ParticipantPointsData = new RaceData<ParticipantPointsData>();
            ParticipantTimeData = new RaceData<ParticipantTimeData>();
            ParticipantTimePerSectionData = new RaceData<ParticipantTimePerSectionData>();
            ParticipantTimesCatchedUp = new RaceData<ParticipantTimesCatchedUp>();
        }

        public void Add(List<TrackRaceData> list)
        {
            if(!list.Contains(this))
            {
                list.Add(this);
            }
        }
    }
}
