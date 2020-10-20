using RaceSimulator.Library.Core.Interfaces;
using System.Collections.Generic;

namespace RaceSimulator.Library.Core
{
    public class RaceData<T> where T : ITemplateData
    {
        private List<T> _list = new List<T>();

        public List<T> Data { get => _list; } 

        public void Add(T value)
        {
            _list.Add(value);
        }

        public T FindByName(string name)
        {
            foreach(T t in _list)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }

            return default;
        }
    }
}
