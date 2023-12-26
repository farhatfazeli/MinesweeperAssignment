using System;

namespace Minesweeper.Model
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; private set; }

        public MinesweeperEngine(int width, int height, int amountOfBombs)
        {
            BoardModel = new BoardModel(width, height);
            SpawnTiles();
            SpawnBombs(amountOfBombs);
            CalculateAdjacentBombCounts();
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < BoardModel.Width; i++)
            {
                for (int j = 0; j < BoardModel.Height; j++)
                {
                    GC gC = new GC(i, j);
                    TileModel tileModel = new TileModel(gC);
                    BoardModel.TileDictionary.Add(gC, tileModel);
                    tileModel.RevealClick.Subscribe(TileModel_OnRevealClick);
                    tileModel.FlagClick.Subscribe(TileModel_OnFlagClick);
                }
            }
        }

        private void CalculateAdjacentBombCounts()
        {
            foreach (TileModel bombTile in BoardModel.TilesWithBombs)
            {
               foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(bombTile))
                {
                    surroundingTile.IncrementBombCount();
                }
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

        private void TileModel_OnRevealClick(TileModel tile)
        {
            RevealTile(tile);
        }

        private void TileModel_OnFlagClick(TileModel tile)
        {
            FlagTile(tile);
        }

        private void FlagTile(TileModel tile)
        {

        }

        private void RevealTile(TileModel tile)
        {
            if (tile.CountOfAdjacentBombs.Value > 0 || tile.IsRevealed.Value) return;
            tile.Reveal();
            foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
            {
                RevealTile(surroundingTile);
            }
        }
    }
}
