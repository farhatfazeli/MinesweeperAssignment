namespace Minesweeper.Model
{
    public class TileModel
    {
        public GC GC;
        public bool IsRevealed { get; private set; }
        public Observable<bool> HasBomb { get; set; } = new(true);
        public Observable<int> CountOfAdjacentBombs { get; set; } = new(0);

        public TileModel(GC gC)
        {
            GC = gC;
        }

        public void Reveal()
        {
            if (IsRevealed) return;
            IsRevealed = true;
        }

        public void IncrementBombCount()
        {
            CountOfAdjacentBombs.Value += 1;
        }
    }
}
