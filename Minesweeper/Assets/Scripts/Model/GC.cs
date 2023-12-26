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
        public int X, Z;

        public GC(int x, int z)
        {
            X = x; Z = z;
        }

        public readonly GC NeighbourGC(GCD gcd)
        {
            GC delta = new GC();
            switch (gcd)
            {
                case GCD.East:
                    delta = new GC(1, 0); break;
                case GCD.West:
                    delta = new GC(-1, 0); break;
                case GCD.North:
                    delta = new GC(0, 1); break;
                case GCD.South:
                    delta = new GC(0, -1); break;
                case GCD.NEast:
                    delta = new GC(1, 1); break;
                case GCD.SWest:
                    delta = new GC(-1, -1); break;
                case GCD.NWest:
                    delta = new GC(-1, 1); break;
                case GCD.SEast:
                    delta = new GC(1, -1); break;
            }
            return this + delta;
        }

        static public List<GCD> AllDirections => Enum.GetValues(typeof(GCD)).Cast<GCD>().ToList();

        public static GC operator +(GC lhs, GC rhs)
        {
            return new GC(lhs.X + rhs.X, lhs.Z + rhs.Z);
        }

        public static GC operator -(GC lhs, GC rhs)
        {
            return new GC(lhs.X - rhs.X, lhs.Z - rhs.Z);
        }
    }
}