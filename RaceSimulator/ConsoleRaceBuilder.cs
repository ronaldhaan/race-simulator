using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RaceSimulator.View.ConsoleApp
{
    public static class ConsoleRaceBuilder
    {
        private static int OrigTop;
        private static int OrigLeft;
        private static Direction _direction = Direction.North;
        private static readonly List<IParticipant> _participants = new List<IParticipant>();

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
                "-------", 
                "   #   ", 
                "   #   ",
                "-------" 
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
            int sectionDefWitdth = _finishHorizontal[0].Length;
            
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
            string[] drawString;
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
                        default:
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
                        default:
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
                        //case Direction.East:
                        //case Direction.West:
                        default:
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
                        //case Direction.East:
                        //case Direction.West:
                        default:
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
                Point pos = _direction switch
                {
                    Direction.North => new Point(1, 2),
                    Direction.South => new Point(2, 5),
                    Direction.West => new Point(2, 2),
                    _ => new Point(1, 5),
                };

                sectionDef = WriteParticipant(sectionDef, pos, GetPlaceholder(data.Left));
            }

            if (data.Right != null)
            {
                Point pos = _direction switch
                {
                    Direction.North => new Point(2, 4),
                    Direction.South => new Point(1, 2),
                    Direction.West => new Point(1, 2),
                    _ => new Point(2, 2),
                };

                sectionDef = WriteParticipant(sectionDef, pos, GetPlaceholder(data.Right));
            }

            return sectionDef;
        }

        private static char GetPlaceholder(IParticipant p)
        {
            if(!_participants.Contains(p))
            {
                _participants.Add(p);
            }

            int placholder = _participants.IndexOf(p) + 1;
            return placholder.ToString()[0];
        }

        /// <summary>
        /// Writes a <see cref="IParticipant"/> on a specific point in the <see cref="Section"/> definition.
        /// </summary>
        /// <param name="sectionDef">The <see cref="Section"/> definition</param>
        /// <param name="pos">The row(x) and the column(y) of the position</param>
        /// <param name="p">The character that will represent the <see cref="IParticipant"/></param>
        /// <returns>The <see cref="Section"/> definition with the <see cref="IParticipant"/>(s)</returns>
        private static string[] WriteParticipant(string[] sectionDef, Point pos, char p)
        {
            string[] newDefinition = new string[sectionDef.Length];
            sectionDef.CopyTo(newDefinition, 0);

            StringBuilder defBuilder = new StringBuilder(newDefinition[pos.X]);
            defBuilder[pos.Y] = p; //replace with placeholder of Participant.
            newDefinition[pos.X] = defBuilder.ToString();

            return newDefinition;
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
