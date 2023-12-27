namespace Minesweeper.Model
{
    public class TileModel
    {
        public GC GC;
        public ObservableValue<bool> IsRevealed { get; private set; } = new(false);
        public ObservableValue<bool> HasBomb { get; set; } = new(false);
        public ObservableValue<bool> HasFlag { get; set; } = new(false);

        public ObservableValue<int> CountOfAdjacentBombs { get; set; } = new(0);
        public ObservableValue<bool> ShowCountOfAdjacentBombs { get; } = new(false);
        public Observable<TileModel> RevealClick { get; } = new();
        public Observable<TileModel> FlagClick { get; } = new();


        public TileModel(GC gC)
        {
            GC = gC;
        }

        public void Reveal()
        {
            IsRevealed.Value = true;
            if (CountOfAdjacentBombs.Value > 0 && !HasBomb.Value)
            {
                ShowCountOfAdjacentBombs.Value = true;
            }
        }

        public void IncrementBombCount()
        {
            CountOfAdjacentBombs.Value += 1;
        }

        public void OnRevealClick()
        {
            RevealClick?.Invoke(this);
        }

        public void OnFlagClick()
        {
            FlagClick?.Invoke(this);
        }
    }
}
