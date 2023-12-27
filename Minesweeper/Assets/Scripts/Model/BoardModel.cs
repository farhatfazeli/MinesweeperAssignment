using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Model
{
    public class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        public TileDictionary TileDictionary { get; }
        public List<TileModel> TilesWithBombs { get; } = new();
        public List<TileModel> UnrevealedTiles { get; set; } = new();
        public ObservableValue<int> AmountOfBombsRemaining { get; set; }

        public BoardModel(int width, int height, int amountOfBombs)
        {
            Width = width;
            Height = height;
            TileDictionary = new(this);
            AmountOfBombsRemaining = new(amountOfBombs);
        }

        public void RevealTile(TileModel tile)
        {
            tile.Reveal();
            UnrevealedTiles.Remove(tile);
        }

        public void RevealBombTiles()
        {
            foreach (TileModel bombTile in TilesWithBombs)
            {
                bombTile.Reveal();
            }
        }

        public List<TileModel> SurroundingTiles(TileModel tileModel)
        {
            List<TileModel> surroundingTiles = new();
            foreach (GCD gcd in GC.AllDirections)
            {
                TileModel surroundingTile = TileDictionary.GetTile(tileModel.GC.NeighbourGC(gcd));
                surroundingTiles.Add(surroundingTile);
            }
            return surroundingTiles;
        }
    }
}


