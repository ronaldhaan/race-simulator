using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;

namespace RaceSimulator.Library.Controller
{
    public class Race
    {
        private Dictionary<Section, SectionData> _positions;
        private readonly Random _random;
        private readonly Timer _timer;
        private const int MAX_ROUNDS = 1;// 3;

        public event EventHandler<ParticipantsChangedEventArgs> ParticipantsChanged;
        public event EventHandler<RaceEndedEventArgs> RaceEnded;

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public DateTime StartTime { get; set; }

        public Track Track { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            _random = new Random(DateTime.Now.Millisecond);
            _timer = new Timer(500)
            {
                Enabled = true
            };

            Participants = participants;
            Positions = new Dictionary<Section, SectionData>();
            StartTime = DateTime.UtcNow;
            Track = track;

            _timer.Elapsed += OnTimedEvent;

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
            Stack<SectionData> sectionDatas = new Stack<SectionData>();
            foreach(Section section in Track.Sections)
            {
                SectionData sd = new SectionData(null, 0, null, 0);

                sectionDatas.Push(sd);
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    for(int i = Participants.Count-1; i >= 0; i-=2)
                    {
                        SectionData s = sectionDatas.Pop();

                        s.Left = Participants[i];
                        try
                        {
                            s.Right = Participants[i - 1];
                        }
                        catch(Exception)
                        {

                        }
                    }
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
                    p.Equipment.Quality = _random.Next(1, 10);
                    p.Equipment.Performance = _random.Next(1, 10);
                    p.Points = 0;
                }
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void OnTimedEvent(object obj, EventArgs e)
        {
            if (Positions != null && Positions.Count > 0)
            {
                if (IsRaceOver(Participants.ToArray()))
                {
                    EndRace();
                    return;
                }

                var newPositions = new Dictionary<Section, SectionData>();

                SectionData prevSD = Positions.Last().Value;

                foreach (KeyValuePair<Section, SectionData> keyValue in Positions)
                {
                    Section currentSection = keyValue.Key;
                    SectionData currentSD = keyValue.Value;

                    if (currentSD.Left != null)
                    {
                        if (ReachedFinish(currentSection, currentSD.Left))
                        {
                            currentSD.Left.Points++;
                            if (IsRaceOver(currentSD.Left))
                            {
                                currentSD.Left = null;
                                prevSD.Left = null;
                            }
                        }
                        else
                        {
                            currentSD.Left.Equipment = CalculateSpeed(currentSD.Left.Equipment);
                            if (currentSD.Left.Equipment.Speed >= 100 && prevSD.Left == null)
                            {
                                currentSD.Left.Equipment.Speed = 0;
                                prevSD.Left = currentSD.Left;
                                currentSD.Left = null;
                            }
                        }
                    }

                    if (currentSD.Right != null)
                    {
                        if (ReachedFinish(currentSection, currentSD.Right))
                        {
                            currentSD.Right.Points++;
                            if (IsRaceOver(currentSD.Right))
                            {
                                currentSD.Right = null;
                                prevSD.Right = null;
                            }
                        }
                        else
                        {
                            currentSD.Right.Equipment = CalculateSpeed(currentSD.Right.Equipment);
                            if (currentSD.Right.Equipment.Speed >= 100 && prevSD.Right == null)
                            {
                                currentSD.Right.Equipment.Speed = 0;
                                prevSD.Right = currentSD.Right;
                                currentSD.Right = null;
                            }
                        }
                    }

                    newPositions.Add(currentSection, prevSD);
                    prevSD = currentSD;
                }

                Positions = newPositions;

                ParticipantsChanged?.Invoke(this, new ParticipantsChangedEventArgs() { Track = Track });
            }
        }

        private bool IsRaceOver(params IParticipant[] participants)
        {
            bool isRaceOver = false;

            foreach(var p in participants)
            {
                if(p.Points >= MAX_ROUNDS)
                {
                    isRaceOver = true;
                }
                else
                {
                    isRaceOver = false;
                    break;
                }
            }

            return isRaceOver;
        }

        private bool ReachedFinish(Section section, IParticipant p)
        {
            return section.SectionType == SectionTypes.Finish && p != null;
        }


        private IEquipment CalculateSpeed(IEquipment e)
        {
            e.Speed += e.Quality * e.Performance;

            return e;
        }

        private bool ended = false;

        private void EndRace()
        {
            if(!ended)
            {
                ended = true;
                _timer.Stop();
                ParticipantsChanged = null;

                RaceEnded?.Invoke(this, new RaceEndedEventArgs() { Track = Track });
            }
        }
    }
}
