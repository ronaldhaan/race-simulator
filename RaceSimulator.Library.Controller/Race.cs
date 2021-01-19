using RaceSimulator.Library.Core;
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
        private const int INTERVAL = 500;

        private readonly SectionData _catchUpSD;
        private bool _ended = false;
        private Dictionary<Section, SectionData> _positions;
        private readonly Random _random;
        private readonly Timer _timer;
        private const int MAX_ROUNDS = 3;

        public event EventHandler<ParticipantsChangedEventArgs> ParticipantsMoved;
        public event EventHandler<RaceFinishedEventArgs> RaceFinished;

        public List<IParticipant> Participants { get; set; }
        
        public Dictionary<string, int> Rounds { get; set; }
        public Dictionary<Section, SectionData> Positions { get => _positions; private set => _positions = value; }

        public RaceData<ParticipantPointsData> ParticipantPointsData { get; set; }

        public RaceData<ParticipantTimesCatchedUp> TimesCatchedUp { get; set; }

        public RaceData<ParticipantTimePerSectionData> TimePerSectionData { get; set; }    
        
        public RaceData<ParticipantTimeData> FinishedTimes { get; set; }

        public DateTime StartTime { get; set; }

        public Track Track { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            _catchUpSD = new SectionData();
            _random = new Random(DateTime.Now.Millisecond);
            
            _timer = new Timer(INTERVAL);
            _timer.Enabled = true;
            _timer.Elapsed += OnTimedEvent;

            Participants = participants;
            StartTime = DateTime.UtcNow;
            Track = track;

            Rounds = new Dictionary<string, int>();
            Positions = new Dictionary<Section, SectionData>();
            FinishedTimes = new RaceData<ParticipantTimeData>();
            TimesCatchedUp = new RaceData<ParticipantTimesCatchedUp>();
            TimePerSectionData = new RaceData<ParticipantTimePerSectionData>();
            ParticipantPointsData = new RaceData<ParticipantPointsData>();

            PlaceParticipants();
            RandomizeEquipment();
        }

        private void StopRace()
        {
            if (!_ended)
            {
                _ended = true;
                _timer.Stop();
                ParticipantsMoved = null;

                RaceFinished?.Invoke(this, new RaceFinishedEventArgs(FinishedTimes, ParticipantPointsData, TimePerSectionData, TimesCatchedUp, Track));
            }
        }

        public SectionData GetSectionData(Section section)
        {
            if(section == null)
            {
                return null;
            }

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

        /// <summary>
        /// Places the participants on the track.
        /// </summary>
        public void PlaceParticipants()
        {
            Stack<SectionData> sectionDatas = new Stack<SectionData>();
            foreach(Section section in Track.Sections)
            {
                SectionData sd = new SectionData();

                sectionDatas.Push(sd);
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    // set the participants on the section behind the startgrid.
                    for(int i = Participants.Count-1; i > 0; i-=2)
                    {
                        SectionData s = sectionDatas.Pop();

                        s.Left = Participants[i];
                        
                        if (i > 0)
                        {
                            s.Right = Participants[i - 1];
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
                    p.Equipment.Quality = _random.Next(4, 10);
                    p.Equipment.Performance = _random.Next(4, 10);
                    p.Points = 0;
                }
            }
        }

        public void Start()
        {
            _timer.Start();
        }


        public void OnTimedEvent(object obj, ElapsedEventArgs e)
        {
            if (Positions.Count > 0)
            {
                if (IsRaceOverFor(Participants))
                {
                    StopRace();
                    return;
                }

                ContinueRace(e.SignalTime);
            }
        }

        private void ContinueRace(DateTime signalTime)
        {
            List<Section> sections = new List<Section>(Positions.Keys);
            if (Positions.TryGetValue(sections[^1], out SectionData prevSD))
            {
                for (int i = 0; i < sections.Count; i++)
                {
                    Section currentSection = sections[i];
                    int nextIndex = ((i + 1) < sections.Count) ? (i + 1) : 0;

                    if (Positions.TryGetValue(currentSection, out SectionData currentSD))
                    {
                        //left

                        ParticipantMoveData moveData = new ParticipantMoveData(currentSection, sections[nextIndex], currentSD.Left, prevSD.Left, _catchUpSD.Left);
                        MoveParticipantWhenPossible(ref moveData, signalTime);
                        currentSD.Left = moveData.Current;
                        prevSD.Left = moveData.Previous;
                        _catchUpSD.Left = moveData.CatchingUp;

                        //right 
                        moveData.ChangeParticipants(currentSD.Right, prevSD.Right, _catchUpSD.Right);
                        MoveParticipantWhenPossible(ref moveData, signalTime);
                        currentSD.Right = moveData.Current;
                        prevSD.Right = moveData.Previous;
                        _catchUpSD.Right = moveData.CatchingUp;

                        moveData = null;

                        Positions[currentSection] = currentSD;
                        prevSD = currentSD;
                    }
                }

                ParticipantsMoved?.Invoke(this, new ParticipantsChangedEventArgs(Track));
            }
        }

        private void MoveParticipantWhenPossible(ref ParticipantMoveData moveData, DateTime signalTime)
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
                        moveData.Current = moveData.Previous;
                        moveData.Previous = null;
                    }
                    else if(moveData.NextSection.SectionType != SectionTypes.Finish)
                    {
                        moveData.CatchingUp = moveData.Previous;
                        moveData.Previous = null;
                    }
                }

                if (moveData.Current != null && ReachedFinish(moveData.CurrentSection.SectionType, moveData.Current))
                {
                    AddRound(moveData.Current);
                    if (IsRaceOverFor(moveData.Current))
                    {
                        SetTime(moveData.Current, moveData.CurrentSection, signalTime);

                        moveData.Current = null;
                    }
                }
            }
        }

        private void SetTime(IParticipant p, Section section, DateTime signalTime)
        {
            TimeSpan time = GetTime(signalTime);
            if (!TimePerSectionData.TryAdd(new ParticipantTimePerSectionData(p.Name, section, time)))
            {
                TimePerSectionData.FindByName(p.Name).TimePerSection.Add(section, time);
            }
        }

        private TimeSpan GetTime(DateTime dateTime)
        {
            TimeSpan timespan = DateTimeToTimeSpan(dateTime) - DateTimeToTimeSpan(StartTime);
            return TimeSpan.FromSeconds(Math.Round(timespan.TotalSeconds, 1));
        }

        #region Data Storage

        private void AddTimesCatchedUp(IParticipant p)
        {
            TimesCatchedUp.Add(new ParticipantTimesCatchedUp(p, 1));
        }

        private void AddRound(IParticipant p) 
        {
            if(!Rounds.TryAdd(p.Name, 1))
            {
                Rounds[p.Name] += 1;
            }

            ParticipantPointsData.Add(new ParticipantPointsData(p));
        }
        #endregion Data Storage

        private void CalculateSpeed(ref IParticipant p)
        {
            IEquipment e = p.Equipment;

            if (!(e.IsBroken = IsBroken(e)))
            {
                e.Speed += e.Quality * e.Performance;
            }

            p.Equipment = e;
        }

        private bool CanParticipantMoveForward(IParticipant p)
        {
            CalculateSpeed(ref p);

            return !(p.Equipment.IsBroken || p.Equipment.Speed < 100);
        }

        /// <summary>
        /// Checks if the race is over for the participant.
        /// </summary>
        /// <param name="p">The participant</param>
        /// <returns>True, when the participant raced all the rounds, False otherwise.</returns>
        private bool IsRaceOverFor(IParticipant p)
        {
            if (p != null && Rounds.TryGetValue(p.Name, out int rounds) && rounds >= MAX_ROUNDS)
            {
                FinishedTimes.Add(new ParticipantTimeData()
                {
                    Name = p.Name,
                    Participant = p,
                    TimeSpan = DateTimeToTimeSpan(DateTime.UtcNow)
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the race is over for multiple participants.
        /// </summary>
        /// <param name="p">The collection of participant</param>
        /// <returns>True, when all the participant raced all the rounds, False otherwise.</returns>
        private bool IsRaceOverFor(List<IParticipant> participants)
        {
            return participants.Count > 0 && participants.TrueForAll(p => IsRaceOverFor(p));
        }

        private bool ReachedFinish(SectionTypes sectionType, IParticipant p)
        {
            return sectionType == SectionTypes.Finish && p != null;
        }

        private bool IsBroken(IEquipment e)
        {
            double chance = e.Quality * e.Performance * _random.Next(2, 10);

            return !e.IsBroken && chance < 35;
        }

        private TimeSpan DateTimeToTimeSpan(DateTime? dt)
        {
             return dt != null && dt.HasValue ? new TimeSpan(0, dt.Value.Hour, dt.Value.Minute, dt.Value.Second, dt.Value.Millisecond) : TimeSpan.Zero;
        }
    }
}
