using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Interfaces
{
    public interface IParticipantData
    {
        public string Name { get; set; }

        public IParticipant Participant { get; set; }

        public List<IParticipantData> AddTo(List<IParticipantData> value);  

        public string FindBest(List<IParticipantData> participants);

        List<T> Add<T>(List<T> list) where T : IParticipantData;
    }
}
