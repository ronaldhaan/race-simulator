using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{

    public class RaceData<T>
    {
        private readonly List<T> _list = new List<T>();

        public void Add(T value)
        {
            _list.Add(value);
        }
    }
}
