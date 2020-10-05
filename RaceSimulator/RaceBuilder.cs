using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RaceSimulator.View
{
    public static class RaceBuilder
    {
        private static int OrigTop;
        private static int OrigLeft;
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
                "|     |", 
                "|-#-#-|",
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
                "|     |",
                "|     |",
                "|-^-^-|",
                "|     |"
            };

        private const int TRACK_WIDTH = 5;
        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static Direction _direction = Direction.North;

        #endregion sectionDefinitions
        /// <summary>
        /// Draws a stack
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static void DrawTrack(Track track)
        {
            int x = 0;
            int y = 1;
            foreach(Section section in track.Sections)
            {
                string[] sectionDef = getSectionDefenition(section);

                SectionData data = Data.CurrentRace.GetSectionData(section);

                if (data.Left != null || data.Right != null)
                {
                    sectionDef = SetParticipantsOnSection(data, sectionDef);
                }

                DrawSection(section, sectionDef, x, y);

                (x, y) = GetNewPositions(x, y);
            }
        }

        private static (int x, int y) GetNewPositions(int x, int y)
        {
            int sectionDefHeight = _finishHorizontal.Length;
            int sectionDefWitdth = _finishHorizontal[0].Length + 2;
            
            switch (_direction)
            {
                case Direction.North:
                    if (y >= sectionDefHeight)
                    {
                        y -= sectionDefHeight;
                    }
                    break;
                case Direction.East:
                    x += sectionDefWitdth;
                    break;
                case Direction.South:
                    y += sectionDefHeight;
                    break;
                case Direction.West:
                    if (x >= sectionDefWitdth)
                    {
                        x -= sectionDefWitdth;
                    }
                    break;
            }

            return (x, y);
        }

        private static string[] getSectionDefenition(Section section)
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

        private static void DrawSection(Section section, string[] sectionDef, int x, int y)
        {
            foreach (string def in sectionDef)
            {
                WriteAt(def, x, y);
                y++;
            }
        }

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
                        row = 2;
                        col = 4;
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
                    
                if (i == col && char.IsWhiteSpace(c) && !done)
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
