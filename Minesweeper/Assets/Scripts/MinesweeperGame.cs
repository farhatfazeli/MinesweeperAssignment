using Minesweeper.View;
using Minesweeper.Model;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    private MinesweeperEngine _minesweeperEngine;

    private void Start()
    {
        _minesweeperEngine = new(10, 10, 10); // TODO: Insert checks on maximum width, height and amount of bombs
        SetupBoardView();
    }
    private void SetupBoardView()
    {
        GameObject board = new GameObject("Board");
        board.AddComponent<BoardView>().Initialize(_minesweeperEngine.BoardModel);
    }
}