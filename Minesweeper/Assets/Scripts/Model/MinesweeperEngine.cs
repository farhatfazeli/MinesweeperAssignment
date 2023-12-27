using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Minesweeper.Model //TODO: unsubscribe from events
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; }
        public int AmountOfBombs { get; }

        #region Constructor
        private bool _isBombsSetup;

        public MinesweeperEngine(int width, int height, int amountOfBombs)
        {
            BoardModel = new BoardModel(width, height, amountOfBombs);
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
        #endregion

        private void TileModel_OnRevealClick(TileModel tile)
        {
            if (tile.HasFlag.Value) return;
            if (!_isBombsSetup)
            {
                SetupBombs(tile);
            }
            if (!tile.IsRevealed.Value)
            {
                RevealTile(tile);
            }
            else if (tile.IsRevealed.Value && tile.CountOfAdjacentBombs.Value > 0)
            {
                FlaggedReveal(tile);
            }
        }

        private void TileModel_OnFlagClick(TileModel tile)
        {
            if (tile.IsRevealed.Value) return;
            FlagTile(tile);
        }

        #region Bomb Setup
        private void SetupBombs(TileModel tileModel)
        {
            SpawnBombs(tileModel);
            CalculateAdjacentBombCounts();
            _isBombsSetup = true;
        }

        private void SpawnBombs(TileModel firstClickedTile)
        {
            List<TileModel> ineligibleTiles = BoardModel.SurroundingTiles(firstClickedTile); // no bombs are allowed on the first clicked tile and its surrounding tiles
            ineligibleTiles.Add(firstClickedTile);
            List<TileModel> eligibleTiles = BoardModel.TileDictionary.ListOfTiles.Except(ineligibleTiles).ToList();

            List<TileModel> tilesWithBombs = eligibleTiles.OrderBy(x => Guid.NewGuid()).Take(AmountOfBombs).ToList();

            foreach(TileModel tileModel in tilesWithBombs)
            {
                tileModel.HasBomb.Value = true;
                BoardModel.TilesWithBombs.Add(tileModel);
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
        #endregion

        private void RevealTile(TileModel tile)
        {
            if (tile.IsRevealed.Value) return;
            if (tile.HasBomb.Value) GameOver(tile);
            tile.Reveal();
            if (tile.HasFlag.Value) FlagTile(tile); //removes flag if tile revealed
            if (tile.CountOfAdjacentBombs.Value > 0) return;

            foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
            {
                RevealTile(surroundingTile);
            }
        }

        private void FlaggedReveal(TileModel tile)
        {
            int countOfAdjacentFlags = 0;
            foreach (TileModel surroundingTiles in BoardModel.SurroundingTiles(tile))
            {
                if (surroundingTiles.HasFlag.Value)
                {
                    countOfAdjacentFlags += 1;
                }

            }
            if (tile.CountOfAdjacentBombs.Value == countOfAdjacentFlags)
            {
                foreach(TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
                {
                    if(!surroundingTile.HasFlag.Value)
                    {
                        surroundingTile.Reveal();
                        if (surroundingTile.HasBomb.Value) GameOver(tile);
                    }
                }
            }
        }

        private void GameOver(TileModel tile)
        {
            foreach(TileModel bombTile in BoardModel.TilesWithBombs)
            {
                bombTile.Reveal();
            }
        }

        private void FlagTile(TileModel tile)
        {
            tile.HasFlag.Toggle();
            if(tile.HasFlag.Value && BoardModel.AmountOfBombsRemaining.Value > 0)
            {
                BoardModel.AmountOfBombsRemaining.Value--;
            }
            else if(!tile.HasFlag.Value && BoardModel.AmountOfBombsRemaining.Value < AmountOfBombs)
            {
                BoardModel.AmountOfBombsRemaining.Value++;
            }
        }
    }
}
