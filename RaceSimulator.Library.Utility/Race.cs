using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Controller
{
    public class Race
    {
        private Random _random;

        private Dictionary<Section, SectionData> _positions;

        public Track Track { get; set; }

        public List<IParticipant> Participants { get; set; }
        
        public DateTime StartTime { get; set; }

        public Dictionary<Section, SectionData> Positions { get; }

        public Race() 
        {
            StartTime = DateTime.UtcNow;
            _random = new Random(DateTime.Now.Millisecond);
        }

        public Race(Track track, List<IParticipant> participants) : this()
        {
            Track = track;
            Participants = participants;
        }

        public SectionData GetSectionData(Section section)
        {
            SectionData data = new SectionData();
            if(Positions.TryGetValue(section, out SectionData sectionData))
            {
                data = sectionData;
            } 
            else
            {
                Positions.Add(section, data);
            }

            return data;
        }

        public void RandomizeEquipment()
        {
            foreach(var participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }



    }
}
