using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RaceSimulator.Library.Core
{
    public class RaceData<T> where T : IParticipantData
    {
        private List<T> _list;

        public List<T> Data { get => new List<T>(_list); }

        public RaceData()
        {
            _list = new List<T>();
        }


        public void Add (T value)
        {
            //if (FindByName(value.Name) != null)
            //{
            //    throw new ArgumentException("Duplicate values not allowed", nameof(value));
            //}

            _list = value.Add(_list); //WARNING 
        }

        public bool TryAdd(T value)
        {
            if(FindByName(value.Name) == null)
            {
                Add(value);
                return true;
            }

            return false;
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

        public bool IsTypeOf<G>()
        {
            return typeof(T) is G;
        }

        public void Sort(Comparison<T> p)
        {
            _list.Sort(p);
        }
        public string FindBest()
        {
            return _list.FirstOrDefault().FindBest(_list as List<IParticipantData>);
        }
    }
}
