using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Controller
{
    public abstract class DataTable
    {
        protected const int DEFAULT_TABLE_WIDTH = 73;
        protected readonly int tableWidth;
        private readonly List<string> tracknames;

        public Dictionary<string, List<List<string>>> TableData { get; protected set; }

        protected List<string> columns;

        public DataTable(List<string> list, int tableWidth = DEFAULT_TABLE_WIDTH)
        {
            tracknames = list;
            this.tableWidth = tableWidth;
        }

        public void CreateDataCollections()
        {
            var tables = new Dictionary<string, List<List<string>>>();
            List<List<string>> rows = new List<List<string>>() { };
            List<IParticipant> participants = Utility.SortParticipants(Data.Competition.Participants);

            for (int i = 0; i < tracknames.Count + 1; i++)
            {
                if (i == 0)
                {
                    string title = "Participant Total ranglist";
                    tables.TryAdd(title, new List<List<string>> { columns });
                    //foreach (var pData in Data.Competition.RaceDataPerTrack)
                    //{
                    //    var name = pData.ParticipantPointsData.FindBest();
                    //    var p = Data.Competition.Participants.Find(p => p.Name == name);
                    //
                    //    var row = new List<string>
                    //    {
                    //        GetName(p),
                    //        Utility.GetPoints(p, pData.ParticipantPointsData).ToString(),
                    //    };
                    //
                    //    if (!tables.TryAdd(title, new List<List<string>> { columns, row }))
                    //    {
                    //        tables[title].Add(row);
                    //    }
                    //}
                }
                else
                {
                    string title = tracknames[i - 1];
                    var racedata = Data.Competition.RaceDataPerTrack.Find(x => x.TrackName == title);
                    Data.Competition.Participants.Sort(new ParticipantComparer(racedata));

                    foreach (IParticipant p in Data.Competition.Participants)
                    {
                        var row = new List<string>
                        {
                            GetName(p),
                            Utility.GetPoints(p, racedata.ParticipantPointsData).ToString(),
                            Utility.GetFinished(p, racedata.ParticipantTimePerSectionData).ToString(),
                            Utility.GetTimesCatchedUp(p, racedata.ParticipantTimesCatchedUp).ToString()

                        };

                        if (!tables.TryAdd(title, new List<List<string>> { columns, row }))
                        {
                            tables[title].Add(row);
                        }
                    }

                }
            }

            TableData = tables;
        }

        public abstract void CreateRow(params string[] cells);

        protected virtual void DrawTable(string title, List<List<string>> rows)
        {
            foreach (var row in rows)
            {
                CreateRow(row.ToArray());
            }
        }

        protected string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        protected string GetName(IParticipant p)
        {
            int index = Data.Competition.Participants.FindIndex(pa => pa.Name == p.Name) + 1;

            return $"{index}: {p.Name}";
        }

        public void Draw()
        {
            CreateDataCollections();
            foreach (var keyvalue in TableData)
            {
                DrawTable(keyvalue.Key, keyvalue.Value);
            }
        }
    }
}
