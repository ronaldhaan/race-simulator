﻿using System;

using RaceSimulator.Library.Core.Interfaces;


namespace RaceSimulator.Library.Core.Templates
{
    public class ParticipantTimeData : ITemplateData
    {
        public string Name { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public ParticipantTimeData(string name, TimeSpan time)
        {
            Name = name;
            TimeSpan = time;
        }

        public override bool Equals(object obj)
        {
            ParticipantTimeData b = obj as ParticipantTimeData;

            return Name == b.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
