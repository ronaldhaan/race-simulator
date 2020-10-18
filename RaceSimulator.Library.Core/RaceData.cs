using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public interface IParticipantData
    {
        public string Name { get; set; }
    }

    public class RaceData<T> where T : IParticipantData
    {
        private List<T> _list = new List<T>();

        public void Add(T value)
        {
            _list.Add(value);
        }

        public T Find(string name)
        {
            foreach(T t in _list)
            {
                if(t.Name == name)
                {
                    return t;
                }
            }

            return default;
        }
    }
}
