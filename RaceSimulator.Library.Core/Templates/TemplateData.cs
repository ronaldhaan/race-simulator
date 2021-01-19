using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaceSimulator.Library.Core.Templates
{
    public abstract class TemplateData
    {
        protected IEnumerable<T> GetTemplateData<T>(List<IParticipantData> templateDataList) where T : IParticipantData
        {
            foreach (IParticipantData pdata in templateDataList)
            {
                if (pdata is T data)
                {
                    yield return data;
                }
            }
        }

        public List<T> Add<T>(List<T> list) where T : IParticipantData
        {
            if (list == null) return null;

            return AddTo(list.Cast<IParticipantData>().ToList()).Cast<T>().ToList();
        }

        public Dictionary<TKey, TValue> Merge<TKey, TValue>(Dictionary<TKey, TValue>[] dictionaries)
        {
            return dictionaries
                         .SelectMany(dict => dict)
                         .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public abstract List<IParticipantData> AddTo(List<IParticipantData> value);
    }
}
