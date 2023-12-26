using System;

namespace Minesweeper.Model
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; }
        public int AmountOfBombs { get; }

        private bool _isBombsSetup;

        public MinesweeperEngine(int width, int height, int amountOfBombs)
        {
            BoardModel = new BoardModel(width, height);
            AmountOfBombs = amountOfBombs;
            SpawnTiles();
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

        private void SetupBombs(TileModel tileModel)
        {
            SpawnBombs(tileModel);
            CalculateAdjacentBombCounts();
            _isBombsSetup = true;
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

        private void SpawnBombs(TileModel firstClickedTile)
        {
            Random random = new();
            for(int i = 0;  i < AmountOfBombs; i++)
            {
                TileModel tile = BoardModel.GetRandomTileWithoutBomb(random);
                tile.HasBomb.Value = true;
                BoardModel.TilesWithBombs.Add(tile);
            }
        }

        private void TileModel_OnRevealClick(TileModel tile)
        {
            if(!_isBombsSetup)
            {
                SetupBombs(tile);
            }
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
            if (tile.IsRevealed.Value) return;
            tile.Reveal();

            if (tile.CountOfAdjacentBombs.Value > 0)
            {
                tile.ShowCountOfAdjacentBombs.Value = true;
                return;
            }

            foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
            {
                RevealTile(surroundingTile);
            }
        }
    }
}
