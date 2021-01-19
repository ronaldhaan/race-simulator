using RaceSimulator.Library.Controller;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.View.ConsoleApp
{
    public class ConsoleTable : DataTable
    {

        public ConsoleTable(List<string> list, int tableWidth = DEFAULT_TABLE_WIDTH) : base(list, tableWidth) 
        {
            columns = new List<string> { "Name", "Points", "Finished", "Times catched up" };
        }

        public void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        public override void CreateRow(params string[] cells)
        {
            string row = "|";
            int width = (tableWidth - cells.Length) / cells.Length;
            foreach (string column in cells)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        protected override void DrawTable(string title, List<List<string>> rows)
        {
            Console.WriteLine(title);
            PrintLine();
            base.DrawTable(title, rows);
            PrintLine();

            Console.WriteLine();
        }
    }
}
