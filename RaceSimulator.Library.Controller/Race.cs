using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Timers;

namespace RaceSimulator.Library.Controller
{
    public class Race
    {
        private Dictionary<Section, SectionData> _positions;
        private readonly Random _random;
        private readonly Timer _timer;
        private const int MAX_ROUNDS = 3;

        public event EventHandler<ParticipantsChangedEventArgs> ParticipantsChanged;
        public event EventHandler<RaceEndedEventArgs> RaceEnded;

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public Dictionary<IParticipant, int> Rounds { get; set; }

        public DateTime StartTime { get; set; }

        public Track Track { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            _random = new Random(DateTime.Now.Millisecond);
            _timer = new Timer(500)
            {
                Enabled = true
            };
            Rounds = new Dictionary<IParticipant, int>();

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
                if (IsRaceOverFor(Participants))
                {
                    EndRace();
                    return;
                }

                List<Section> sections = new List<Section>(Positions.Keys);
                Section lastSection = sections[^1];
                SectionData catchUpSD = new SectionData(null, 0, null, 0);
                if (Positions.TryGetValue(lastSection, out SectionData prevSD))
                {
                    foreach (Section currentSection in sections)
                    {
                        if (Positions.TryGetValue(currentSection, out SectionData currentSD))
                        {
                            #region left
                            bool catchUpLeftWasTrue = false;

                            if (catchUpSD.Left != null)
                            {
                                currentSD.Left = catchUpSD.Left;
                                catchUpSD.Left = null;
                                catchUpLeftWasTrue = true;
                            }
                            else if (prevSD.Left != null)
                            {
                                if (CanParticipantMoveForward(prevSD.Left))
                                {
                                    prevSD.Left.Equipment.Speed = 0;

                                    if (currentSD.Left == null)
                                    {
                                        currentSD.Left = prevSD.Left;
                                    }
                                    else
                                    {
                                        catchUpSD.Left = prevSD.Left;
                                    }

                                    prevSD.Left = null;
                                }

                                if (ReachedFinish(currentSection, prevSD.Left))// || (catchUpLeftWasTrue && ReachedFinish(currentSection, currentSD.Left)))
                                {
                                    catchUpLeftWasTrue = false;
                                    AddPoints(prevSD.Left);
                                    if (IsRaceOverFor(prevSD.Left))
                                    {
                                        if(prevSD.Left == currentSD.Left)
                                        {
                                            currentSD.Left = null;
                                        }

                                        prevSD.Left = null;
                                    }
                                }
                            }

                            #endregion left

                            #region right
                            bool catchUpRightWasTrue = false;
                            if (catchUpSD.Right != null)
                            {
                                currentSD.Right = catchUpSD.Right;
                                catchUpSD.Right = null;
                                catchUpRightWasTrue = true;
                            }
                            else if (prevSD.Right != null)
                            {
                                if (CanParticipantMoveForward(prevSD.Right))
                                {
                                    prevSD.Right.Equipment.Speed = 0;

                                    if (currentSD.Right == null)
                                    {
                                        currentSD.Right = prevSD.Right;
                                    }
                                    else
                                    {
                                        catchUpSD.Right = prevSD.Right;
                                    }

                                    prevSD.Right = null;
                                }

                                if (ReachedFinish(currentSection, prevSD.Right))// || (catchUpRightWasTrue && ReachedFinish(currentSection, currentSD.Right)))
                                {
                                    catchUpLeftWasTrue = false;
                                    AddPoints(prevSD.Right);
                                    if (IsRaceOverFor(prevSD.Right))
                                    {
                                        if (prevSD.Right == currentSD.Right)
                                        {
                                            currentSD.Right = null;
                                        }

                                        prevSD.Right = null;
                                    }
                                }
                            }

                            #endregion right

                            Positions[currentSection] = currentSD;
                            prevSD = currentSD;
                        }

                    }

                    ParticipantsChanged?.Invoke(this, new ParticipantsChangedEventArgs() { Track = Track });
                }
            }
        }

        private void AddPoints(IParticipant p)
        {
            p.Points += Rounds.Count == 0 ? 2 : 1;

            if (!Rounds.TryAdd(p, 1))
            {
                Rounds[p] += 1;
            }

        }

        private bool CanParticipantMoveForward(IParticipant p)
        {
            p = CalculateSpeed(p);
            if (p.Equipment.IsBroken || p.Equipment.Speed < 100)
            {
                return false;
            }

            return true;
        }

        private bool IsRaceOverFor(IParticipant p)
        {
            if(Rounds.TryGetValue(p, out int rounds))
            {
                if(rounds >= MAX_ROUNDS)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsRaceOverFor(List<IParticipant> participants)
        {
            bool isRaceOver = true;

            foreach(IParticipant p in participants)
            {
                if(!IsRaceOverFor(p))
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

        private IParticipant CalculateSpeed(IParticipant p)
        {
            IEquipment e = p.Equipment;

            if (!(e.IsBroken = IsBroken(e)))
            {
                e.Speed += e.Quality * e.Performance;
            }

            p.Equipment = e;

            return p;
        }

        private bool IsBroken(IEquipment e)
        {
            int random = _random.Next(2, 10);
            double chance = e.Quality * e.Performance * random;
            bool isBroken = !e.IsBroken && chance < 30;
            return isBroken;
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
