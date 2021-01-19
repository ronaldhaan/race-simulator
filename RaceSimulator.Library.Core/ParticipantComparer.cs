using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class ParticipantComparer : IComparer<IParticipant>
    {
        private readonly TrackRaceData raceData;

        public ParticipantComparer() : this(null) { }

        public ParticipantComparer(TrackRaceData data)
        {
            raceData = data;
        }

        public int Compare(IParticipant p1, IParticipant p2)
        {
            if (p1 == null || p2 == null)
            {
                return 0;
            }

            return raceData == null ? CompareDefault(p1, p2) : CompareWithData(p1, p2);
        }

        private int CompareWithData(IParticipant p1, IParticipant p2)
        {
            int pointP1 = 0;
            int pointP2 = 0;

            TimeSpan p1Ts = Utility.GetFinished(p1, raceData.ParticipantTimePerSectionData);
            int p1Times = Utility.GetTimesCatchedUp(p1, raceData.ParticipantTimesCatchedUp);

            TimeSpan p2Ts = Utility.GetFinished(p2, raceData.ParticipantTimePerSectionData);
            int p2Times = Utility.GetTimesCatchedUp(p2, raceData.ParticipantTimesCatchedUp);

            if(p1Ts < p2Ts)
            {
                pointP2++;
            } 
            else if (p1Ts > p2Ts)
            {
                pointP1++;
            }
            else
            {
                if(p1Times < p2Times)
                {
                    pointP1++;
                }
                else if(p1Times > p2Times)
                {
                    pointP2++;
                }
            }


            return CompareDefaultInt(pointP1, pointP2);
        }

        private int CompareDefault(IParticipant p1, IParticipant p2) => CompareDefaultInt(p1.Points, p2.Points);

        private int CompareDefaultInt(int x, int y)
        {
            if (x < y)
            {
                return -1;
            }
            else if (x > y)
            {
                return 1;
            }

            return 0;
        }
    }
}
