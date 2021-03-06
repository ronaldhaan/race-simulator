﻿using RaceSimulator.Library.Core.Enumerations;

namespace RaceSimulator.Library.Core.Interfaces
{
    public interface IParticipant
    {
        public string Name { get; set; }

        public int Points { get; set; }

        public IEquipment Equipment { get; set; }

        public TeamColor TeamColor { get; set; }
    }
}
