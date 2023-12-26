namespace Minesweeper.Model
{
    public class TileModel
    {
        public GC GC;
        public bool IsRevealed { get; private set; }

        public TileModel(GC gC)
        {
            GC = gC;
        }

        public void Reveal()
        {
            IsRevealed = true;
        }
    }
}
