using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class ParticipantMoveData
    {
        public Section CurrentSection { get; set; }

        public IParticipant Current { get; set; }

        public IParticipant Previous { get; set; }

        public IParticipant CatchingUp { get; set; }

        public ParticipantMoveData(Section currentSection, IParticipant current, IParticipant prev, IParticipant catchUp)
        {
            CurrentSection = currentSection;
            Current = current;
            Previous = prev;
            CatchingUp = catchUp;
        }
    }
}
