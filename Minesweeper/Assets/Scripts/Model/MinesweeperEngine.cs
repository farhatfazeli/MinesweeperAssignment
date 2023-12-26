﻿using System;
using System.Collections.Generic;

namespace Minesweeper.Model
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; private set; }

        public MinesweeperEngine(int width, int height, int amountOfBombs)
        {
            BoardModel = new BoardModel(width, height);
            SpawnBombs(amountOfBombs);
            CalculateAdjacentBombCounts();
        }

        private void CalculateAdjacentBombCounts()
        {
            foreach (TileModel tile in BoardModel.TilesWithBombs)
            {
               // List<TileModel> surroundingTiles =
               //
               GC gc = new GC(1, 2);

            }
        }

        private void SpawnBombs(int amountOfBombs)
        {
            Random random = new();
            for(int i = 0;  i < amountOfBombs; i++)
            {
                TileModel tile = BoardModel.GetRandomTileWithoutBomb(random);
                tile.HasBomb.Value = true;
                BoardModel.TilesWithBombs.Add(tile);
            }
        }
    }
}
