using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class ParticipantMoveData
    {
        public Section CurrentSection { get; set; }

        public Section NextSection { get; set; }

        public IParticipant Current { get; set; }

        public IParticipant Previous { get; set; }

        public IParticipant CatchingUp { get; set; }

        public ParticipantMoveData(Section currentSection, Section nextSection, IParticipant current, IParticipant prev, IParticipant catchUp)
        {
            CurrentSection = currentSection;
            NextSection = nextSection;
            Current = current;
            Previous = prev;
            CatchingUp = catchUp;
        }

        public void ChangeParticipants(IParticipant current, IParticipant prev, IParticipant catchup)
        {
            Current = current;
            Previous = prev;
            CatchingUp = catchup;
        }
    }
}
