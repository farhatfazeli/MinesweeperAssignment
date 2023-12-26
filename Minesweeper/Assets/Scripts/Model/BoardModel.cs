using System.Collections.Generic;

namespace Minesweeper.Model
{
    public class BoardModel
    {
        public Dictionary<GC, TileModel> TileDictionary = new();

        public BoardModel()
        {
            SpawnTiles();
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GC gC = new GC(i, j);
                    TileModel tileModel = new TileModel(gC);
                    TileDictionary.Add(gC, tileModel);
                }
            }
        }
    }
}


