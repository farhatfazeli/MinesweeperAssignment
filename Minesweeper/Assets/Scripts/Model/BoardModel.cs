using System;
using System.Collections.Generic;

namespace Minesweeper.Model
{
    public class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        public TileDictionary TileDictionary { get; }
        public List<TileModel> TilesWithBombs { get; } = new();

        public BoardModel(int width, int height)
        {
            Width = width;
            Height = height;
            TileDictionary = new(this);
        }



        public TileModel GetRandomTileWithoutBomb(Random random) //TODO: Optimise random search
        {
            int x = random.Next(0, Width);
            int z = random.Next(0, Height);
            TileModel tileModel = TileDictionary.GetTile(new GC(x, z));
            if (!tileModel.HasBomb.Value)
                return tileModel;
            else return GetRandomTileWithoutBomb(random);
        }

        public List<TileModel> SurroundingTiles(TileModel tileModel)
        {
            List<TileModel> surroundingTiles = new();
            foreach(GCD gcd in GC.AllDirections)
            {
                TileModel surroundingTile = TileDictionary.GetTile(tileModel.GC.NeighbourGC(gcd));
                surroundingTiles.Add(surroundingTile);
            }
            return surroundingTiles;
        }
    }
}


