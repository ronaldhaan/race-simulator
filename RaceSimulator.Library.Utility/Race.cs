using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace RaceSimulator.Library.Controller
{
    public delegate void PositionChangedObserver(object obj, EventArgs e);

    public class Race
    {
        private readonly Random _random;
        private Dictionary<Section, SectionData> _positions;

        public event PositionChangedObserver PositionChanged; 

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public DateTime StartTime { get; set; }

        public Track Track { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            _random = new Random(DateTime.Now.Millisecond);

            Participants = participants;
            Positions = new Dictionary<Section, SectionData>();
            StartTime = DateTime.UtcNow;
            Track = track;

            PlaceParticipants();
            RandomizeEquipment();
        }

        public SectionData GetSectionData(Section section)
        {
            if(section == null)
            {
                return null;
            }

            SectionData data = new SectionData(null, 0, null, 0);
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

        /// <summary>
        /// Places the participants on the track.
        /// </summary>
        public void PlaceParticipants()
        {
            foreach(Section section in Track.Sections)
            {
                SectionData sd = new SectionData(null, 0, null, 0);

                if (section.SectionType == SectionTypes.StartGrid)
                {
                    sd.Left = Participants[0];
                    sd.DistanceLeft = 2;
                    sd.Right = Participants[1];
                    sd.DistanceRight = 1;
                }

                Positions.Add(section, sd);
            }
        }
        public void Move()
        {
            SectionData sectionData = GetSectionData(Track.Sections.Last.Value);
            var newPositions = new Dictionary<Section, SectionData>();

            foreach (KeyValuePair<Section, SectionData> keyValue in Positions)
            {
                newPositions.Add(keyValue.Key, sectionData);
                sectionData = keyValue.Value;
            }

            Positions = newPositions;

            PositionChanged(Track, new EventArgs());
        }

        public void RandomizeEquipment()
        {
            foreach(IParticipant p in Participants)
            {
                if (p.Equipment != null)
                {
                    p.Equipment.Quality = _random.Next();
                    p.Equipment.Performance = _random.Next();
                }
            }
        }



    }
}
