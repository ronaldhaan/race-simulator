using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace RaceSimulator.Library.Controller
{
    public class Race
    {
        private Dictionary<Section, SectionData> _positions;
        private readonly Random _random;
        private Timer _timer;

        public event EventHandler<ParticipantsChangedEventArgs> ParticipantsChanged;

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public DateTime StartTime { get; set; }

        public Track Track { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            _random = new Random(DateTime.Now.Millisecond);
            _timer = new Timer(500);
            _timer.Enabled = true;
            _timer.Elapsed += OnTimedEvent;

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

        public void RandomizeEquipment()
        {
            foreach(IParticipant p in Participants)
            {
                if (p.Equipment != null)
                {
                    p.Equipment.Quality = _random.Next(10);
                    p.Equipment.Performance = _random.Next(10);
                }
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void OnTimedEvent(object obj, EventArgs e)
        {
            var newPositions = new Dictionary<Section, SectionData>();

            SectionData prevSD = Positions.Last().Value;
            foreach (KeyValuePair<Section, SectionData> keyValue in Positions)
            {  
                Section currentSection = keyValue.Key;
                SectionData currentSD = keyValue.Value;

                if (currentSD.Left != null)
                {
                    currentSD.Left.Equipment = CalculateSpeed(currentSD.Left.Equipment);
                    if (currentSD.Left.Equipment.Speed >= 100)
                    {
                        currentSD.Left.Equipment.Speed = 0;
                        prevSD.Left = currentSD.Left;
                        currentSD.Left = null;
                    }
                }

                if (currentSD.Right != null)
                {
                    currentSD.Right.Equipment = CalculateSpeed(currentSD.Right.Equipment);
                    if (currentSD.Right.Equipment.Speed >= 100)
                    {
                        currentSD.Right.Equipment.Speed = 0;
                        prevSD.Right = currentSD.Right;
                        currentSD.Right = null;
                    }
                }

                newPositions.Add(currentSection, prevSD);
                prevSD = currentSD;
            }

            Positions = newPositions;

            ParticipantsChanged(this, new ParticipantsChangedEventArgs() { Track = Track });

        }


        private IEquipment CalculateSpeed(IEquipment e)
        {
            e.Speed += e.Quality * e.Performance;

            return e;
        }



    }
}
