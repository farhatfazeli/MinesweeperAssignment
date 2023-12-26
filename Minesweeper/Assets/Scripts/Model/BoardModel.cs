using System;

namespace Minesweeper.Model
{
    public class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        public TileDictionary TileDictionary { get; } = new();

        public BoardModel(int width, int height)
        {
            Width = width;
            Height = height;
            SpawnTiles();
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    GC gC = new GC(i, j);
                    TileModel tileModel = new TileModel(gC);
                    TileDictionary.Add(gC, tileModel);
                }
            }
        }

        public TileModel GetRandomTileWithoutBomb(Random random)
        {
            int x = random.Next(0, Width);
            int z = random.Next(0, Height);
            TileModel tileModel = TileDictionary.GetTile(new GC(x, z));
            if (!tileModel.HasBomb.Value)
                return tileModel;
            else return GetRandomTileWithoutBomb(random);
        }
    }
}


