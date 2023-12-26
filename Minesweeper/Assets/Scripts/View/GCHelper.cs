using Minesweeper.Model;
using UnityEngine;

namespace Minesweeper.View
{
    public static class GCHelper
    {
        private static readonly int _size = 1;

        public static Vector3 ModelToView(GC coordinate)
        {
            return _size * new Vector3(coordinate.X, 0, coordinate.Z);
        }
    }
}