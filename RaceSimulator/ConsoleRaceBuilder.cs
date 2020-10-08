using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Drawing;
using System.Text;

namespace RaceSimulator.View.ConsoleApp
{
    public static class ConsoleRaceBuilder
    {
        private static int OrigTop;
        private static int OrigLeft;

        /// <summary>
        /// Initializes the start values of the <see cref="ConsoleRaceBuilder"/> class.
        /// </summary>
        public static void Initialize()
        {
            OrigTop = Console.CursorTop;
            OrigLeft = Console.CursorLeft;
        }

        #region graphics
        #region sectionDefinitions
        private static readonly string[] _finishHorizontal = 
            { 
                "-----", 
                "  #  ", 
                "  #  ",
                "-----" 
            };
        private static readonly string[] _finishVertical = 
            { 
                "|     |", 
                "|-#-#-|",
                "|     |", 
                "|     |" 
            };

        private static readonly string[] _straightHorizontal = 
            {  
                "-------", 
                "       ", 
                "       ",
                "-------" 
            };
        private static readonly string[] _straightVertical = 
            { 
                "|     |",  
                "|     |", 
                "|     |",
                "|     |" 
            };

        private static readonly string[] _leftUpCorner = 
            {   
                " /-----", 
                "/      ",
                "|      ",
                "|     /" 
            };
        private static readonly string[] _rightUpCorner = 
            { 
                "-----\\ ", 
                "      \\", 
                "      |",
                "\\     |",
            };
        private static readonly string[] _leftDownCorner = 
            { 
                "|     \\",
                "|      ",
                "\\      ", 
                " \\-----" 
            };
        private static readonly string[] _rightDownCorner = 
            { 
                "/     |", 
                "      |",
                "      /", 
                "-----/ " 
        };

        private static readonly string[] _startGridHorizontal = 
            { 
                "-------", 
                "      ]",
                "   ]   ",
                "-------" 
            };

        private static readonly string[] _startGridVertical =
            {
                "| _   |",
                "|   _ |",
                "|     |",
                "|     |"
            };

        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static Direction _direction = Direction.North;

        #endregion sectionDefinitions

        public static void RedrawTrack(object obj, ParticipantsChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }

        /// <summary>
        /// Draws a <see cref="Track"/> on the <see cref="Console"/>
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static void DrawTrack(Track track)
        {
            Point cursorPoint = new Point(0, 1);
            foreach(Section section in track.Sections)
            {
                string[] sectionDef = GetSectionDefenition(section);

                SectionData sectionData = Data.CurrentRace.GetSectionData(section);

                if (sectionData.Left != null || sectionData.Right != null)
                {
                    sectionDef = SetParticipantsOnSection(sectionData, sectionDef);
                }

                DrawSection(sectionDef, cursorPoint);

                cursorPoint = GetNextSectionPoint(cursorPoint);
            }

            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.CursorLeft = 0;
        }

        /// <summary>
        /// Gets the new <see cref="Console"/> positions as x and y coordinates.
        /// </summary>
        /// <param name="x">The old X coordinate</param>
        /// <param name="y">The old y coordinate</param>
        /// <returns></returns>
        private static Point GetNextSectionPoint(Point cursorPoint)
        {
            int sectionDefHeight = _finishHorizontal.Length;
            int sectionDefWitdth = _finishHorizontal[0].Length + 2;
            
            switch (_direction)
            {
                case Direction.North:
                    if (cursorPoint.Y >= sectionDefHeight)
                    {
                        cursorPoint.Y -= sectionDefHeight;
                    }
                    break;
                case Direction.East:
                    cursorPoint.X += sectionDefWitdth;
                    break;
                case Direction.South:
                    cursorPoint.Y += sectionDefHeight;
                    break;
                case Direction.West:
                    if (cursorPoint.X >= sectionDefWitdth)
                    {
                        cursorPoint.X -= sectionDefWitdth;
                    }
                    break;
            }

            return cursorPoint;
        }

        /// <summary>
        /// Gets the <see cref="Section"/> definition of the section using <see cref="SectionTypes"/>
        /// </summary>
        /// <param name="section">The <see cref="Section"/> Object</param>
        /// <returns>The definition of the given <see cref="Section"/> Object </returns>
        private static string[] GetSectionDefenition(Section section)
        {
            string[] drawString = null;
            switch (section.SectionType)
            {
                case SectionTypes.Finish:
                    switch (_direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            drawString = _finishVertical;
                            break;
                        case Direction.East:
                        case Direction.West:
                            drawString = _finishHorizontal;
                            break;
                    }
                    break;
                case SectionTypes.LeftCorner:
                    switch (_direction)
                    {
                        case Direction.North:
                            drawString = _rightUpCorner;
                            _direction = Direction.West;
                            break;
                        case Direction.West:
                            drawString = _leftUpCorner;
                            _direction = Direction.South;
                            break;
                        case Direction.South:
                            drawString = _leftDownCorner;
                            _direction = Direction.East;
                            break;
                        case Direction.East:
                            drawString = _rightDownCorner;
                            _direction = Direction.North;
                            break;
                        
                    }
                    break;
                case SectionTypes.RightCorner:
                    switch (_direction)
                    {
                        case Direction.North:
                            drawString = _leftUpCorner;
                            _direction = Direction.East;
                            break;
                        case Direction.East:
                            drawString = _rightUpCorner;
                            _direction = Direction.South;
                            break;
                        case Direction.South:
                            drawString = _rightDownCorner;
                            _direction = Direction.West;
                            break;
                        case Direction.West:
                        default:
                            drawString = _leftDownCorner;
                            _direction = Direction.North;
                            break;
                    }
                    break;
                case SectionTypes.StartGrid:
                    switch (_direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            drawString = _startGridVertical;
                            break;
                        case Direction.East:
                        case Direction.West:
                            drawString = _startGridHorizontal;
                            break;
                    }
                    break;
                case SectionTypes.Straight:
                default:
                    switch (_direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            drawString = _straightVertical;
                            break;
                        case Direction.East:
                        case Direction.West:
                            drawString = _straightHorizontal;
                            break;
                    }
                    break;
            }
            return drawString;
        }

        /// <summary>
        /// Draws the <see cref="Section"/> in the <see cref="Console"/>
        /// </summary>
        /// <param name="section">The <see cref="Section"/> Object that will be written.</param>
        /// <param name="sectionDef">The definition of the <see cref="Section"/></param>
        /// <param name="x">The X coordinate in the <see cref="Console"/> where the Section will be written</param>
        /// <param name="y">The Y coordinate in the <see cref="Console"/> where the Section will be written</param>
        private static void DrawSection(string[] sectionDef, Point cursorPoint)
        {
            int y = cursorPoint.Y;
            foreach (string def in sectionDef)
            {
                WriteAt(def, cursorPoint.X, y);
                y++;
            }
        }

        /// <summary>
        /// Sets the <see cref="IParticipant"/> in the <see cref="Section"/> definition using it's <see cref="SectionData"/> Object.
        /// </summary>
        /// <param name="data">The <see cref="SectionData"/> of the <see cref="Section"/> Object</param>
        /// <param name="sectionDef">The <see cref="Section"/> definition.</param>
        /// <returns>The <see cref="Section"/> definition with the <see cref="IParticipant"/>(s)</returns>
        private static string[] SetParticipantsOnSection(SectionData data, string[] sectionDef)
        {            
            if (data.Left != null)
            {
                int row = 0;
                int col = 0;
                switch (_direction)
                {
                    case Direction.North:
                        row = 1;
                        col = 2;
                        break;
                    case Direction.South:
                        row = 2;
                        col = 5;
                        break;
                    case Direction.West:
                        row = 2;
                        col = 2;
                        break;
                    case Direction.East:
                        row = 1;
                        col = 5;
                        break;
                }

                sectionDef = WriteParticipant(sectionDef, row, col, '1');
            }

            if (data.Right != null)
            {
                int row = 0;
                int col = 0;
                switch (_direction)
                {
                    case Direction.North:
                        row = 2;
                        col = 4;
                        break;
                    case Direction.South:
                        row = 1;
                        col = 2;
                        break;
                    case Direction.West:
                        row = 1;
                        col = 2;
                        break;
                    case Direction.East:
                        row = 2;
                        col = 2;
                        break;
                }
                sectionDef = WriteParticipant(sectionDef, row, col, '2');
            }

            return sectionDef;
        }

        /// <summary>
        /// Writes a <see cref="IParticipant"/> on a specific point in the <see cref="Section"/> definition.
        /// </summary>
        /// <param name="sectionDef">The <see cref="Section"/> definition</param>
        /// <param name="row">The row(x) of the point</param>
        /// <param name="col">The column(y) of the point</param>
        /// <param name="p">The character that will represent the <see cref="IParticipant"/></param>
        /// <returns>The <see cref="Section"/> definition with the <see cref="IParticipant"/>(s)</returns>
        private static string[] WriteParticipant(string[] sectionDef, int row, int col, char p)
        {
            string[] newDef = new string[sectionDef.Length];
            for (int i = 0; i < newDef.Length; i++)
            {
                newDef[i] = sectionDef[i];
            }
            
            string defRow = newDef[row];

            StringBuilder stringBuilder = new StringBuilder();

            bool done = false;
            for (int i = 0; i < defRow.Length; i++)
            {
                char c = defRow[i];
                    
                if (i == col && !done)
                {
                    done = true;
                    stringBuilder.Append(p);
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            string s = stringBuilder.ToString();

            newDef[row] = s;

            return newDef;

        }

        /// <summary>
        /// Writes a string on the given x and y coordinates.
        /// </summary>
        /// <param name="s">The string that is gonna be written</param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        private static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(OrigLeft + x, OrigTop + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        #endregion graphics

    }
}
