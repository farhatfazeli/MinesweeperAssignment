using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Model
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; }
        public int AmountOfBombs { get; }

        public Action GameOver;
        public Action GameWon;

        #region Constructor
        private bool _isBombsSetup;
        private bool _isGameOver;
        private bool _isGameWon;

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
            BoardModel.UnrevealedTiles = BoardModel.TileDictionary.ListOfTiles.ToList();
        }
        #endregion

        private void TileModel_OnRevealClick(TileModel tile)
        {
            if (_isGameOver || _isGameWon) return;
            if (tile.HasFlag.Value) return;
            if (!_isBombsSetup)
            {
                SetupBombs(tile);
            }
            if (!tile.IsRevealed.Value)
            {
                RevealTiles(tile);
            }
            else if (tile.IsRevealed.Value && tile.CountOfAdjacentBombs.Value > 0)
            {
                FlaggedReveals(tile);
            }
        }

        private void TileModel_OnFlagClick(TileModel tile)
        {
            if (_isGameOver || _isGameWon) return;
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

            foreach (TileModel tileModel in tilesWithBombs)
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

        private void RevealTiles(TileModel tile)
        {
            if (tile.IsRevealed.Value) return;
            if (tile.HasBomb.Value) BombRevealed(tile);
            RevealTile(tile);
            if (tile.HasFlag.Value) FlagTile(tile); //removes flag if tile revealed
            if (tile.CountOfAdjacentBombs.Value > 0) return;

            foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
            {
                RevealTiles(surroundingTile);
            }
        }

        private void FlaggedReveals(TileModel tile)
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
                foreach (TileModel surroundingTile in BoardModel.SurroundingTiles(tile))
                {
                    if (!surroundingTile.HasFlag.Value)
                    {
                        RevealTile(surroundingTile);
                        if (surroundingTile.HasBomb.Value) BombRevealed(tile);
                    }
                }
            }
        }

        private void RevealTile(TileModel tile)
        {
            BoardModel.RevealTile(tile);
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (BoardModel.UnrevealedTiles.Count != BoardModel.TilesWithBombs.Count) return;

            _isGameWon = true;
            GameWon?.Invoke();
        }

        private void BombRevealed(TileModel tile)
        {
            BoardModel.RevealBombTiles();
            _isGameOver = true;
            GameOver?.Invoke();
        }

        private void FlagTile(TileModel tile)
        {
            tile.HasFlag.Toggle();
            if (tile.HasFlag.Value && BoardModel.AmountOfBombsRemaining.Value > 0)
            {
                BoardModel.AmountOfBombsRemaining.Value--;
            }
            else if (!tile.HasFlag.Value && BoardModel.AmountOfBombsRemaining.Value < AmountOfBombs)
            {
                BoardModel.AmountOfBombsRemaining.Value++;
            }
        }
    }
}
