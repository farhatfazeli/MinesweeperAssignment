using System;
using System.Collections.Generic;
using System.Linq;



namespace Minesweeper.Model
{
    public enum GCD //GridCoordinate Direction
    {
        East, West,
        North, South,
        NEast, SWest,
        NWest, SEast
    }

    public struct GC // GridCoordinate
    {
        static public List<GCD> AllDirections => Enum.GetValues(typeof(GCD)).Cast<GCD>().ToList();

        public int X, Z;
        public GC(int x, int z)
        {
            X = x; Z = z;
        }
    }
}