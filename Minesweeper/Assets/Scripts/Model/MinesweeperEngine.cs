namespace Minesweeper.Model
{
    public class MinesweeperEngine
    {
        public BoardModel BoardModel { get; private set; }

        public MinesweeperEngine()
        {
            BoardModel = new BoardModel();
        }
    }
}
