﻿using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Data;
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

        public event EventHandler<ParticipantsChangedEventArgs> ParticipantsMoved;
        public event EventHandler<RaceFinishedEventArgs> RaceFinished;

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public Dictionary<IParticipant, int> Rounds { get; set; }

        public Dictionary<IParticipant, int> TimesCatchedUp { get; set; }

        public List<ParticipantTimeData> FinishedTimes { get; set; }

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
            FinishedTimes = new List<ParticipantTimeData>();

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
                    FinishRace();
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
                            //left
                             ParticipantMoveData moveData = new ParticipantMoveData(currentSection, currentSD.Left, prevSD.Left, catchUpSD.Left);
                            moveData = MoveParticipantWhenPossible(moveData);
                            currentSD.Left = moveData.Current;
                            prevSD.Left = moveData.Previous;
                            catchUpSD.Left = moveData.CatchingUp;

                            //right 
                            moveData = new ParticipantMoveData(currentSection, currentSD.Right, prevSD.Right, catchUpSD.Right);
                            moveData = MoveParticipantWhenPossible(moveData);
                            currentSD.Right = moveData.Current;
                            prevSD.Right = moveData.Previous;
                            catchUpSD.Right = moveData.CatchingUp;

                            Positions[currentSection] = currentSD;
                            prevSD = currentSD;
                        }

                    }

                    ParticipantsMoved?.Invoke(this, new ParticipantsChangedEventArgs(Track));
                }
            }
        }

        private ParticipantMoveData MoveParticipantWhenPossible(ParticipantMoveData moveData)
        {
            if (moveData.CatchingUp != null)
            {
                moveData.Current = moveData.CatchingUp;
                AddTimesCatchedUp(moveData.CatchingUp);
                moveData.CatchingUp = null;
            }
            else if (moveData.Previous != null)
            {
                if (CanParticipantMoveForward(moveData.Previous))
                {
                    moveData.Previous.Equipment.Speed = 0;

                    if (moveData.Current == null)
                    {
                        moveData.Current =  moveData.Previous;
                    }
                    else
                    {
                        moveData.CatchingUp = moveData.Previous;
                    }

                    moveData.Previous = null;
                }

                if (ReachedFinish(moveData.CurrentSection, moveData.Previous))
                {
                    AddRound(moveData.Previous);
                    if (IsRaceOverFor(moveData.Previous))
                    {
                        if (moveData.Previous == moveData.Current)
                        {
                            moveData.Current = null;
                        }

                        moveData.Previous = null;
                    }
                }
            }

            return moveData;
        }

        private void AddUniqueValue(Dictionary<IParticipant, int> dic, IParticipant p)
        {
            if (!dic.TryAdd(p, 1))
            {
                dic[p] += 1;
            }
        }

        private void AddTimesCatchedUp(IParticipant p) => AddUniqueValue(TimesCatchedUp, p);

        private void AddRound(IParticipant p) => AddUniqueValue(Rounds, p);

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
                    ParticipantTimeData time = new ParticipantTimeData(p.Name, DateTimeToTimeSpan(DateTime.Now));
                    if (!FinishedTimes.Contains(time)) 
                    {
                        FinishedTimes.Add(time);
                    }

                    return true;
                }
            }

            return false;
        }

        private TimeSpan DateTimeToTimeSpan(DateTime? ts)
        {
            if (ts.HasValue)
            {
                return new TimeSpan(0, ts.Value.Hour, ts.Value.Minute, ts.Value.Second, ts.Value.Millisecond);
            }

            return TimeSpan.Zero;
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

        private void FinishRace()
        {
            if(!ended)
            {
                ended = true;
                _timer.Stop();
                ParticipantsMoved = null;

                List<IParticipant> ranglist = GetRanglist();

                RaceFinished?.Invoke(this, new RaceFinishedEventArgs(ranglist, FinishedTimes, TimesCatchedUp, Track));
            }
        }

        private List<IParticipant> GetRanglist()
        {
            return new List<IParticipant>(Rounds.Keys);
        }
    }
}
