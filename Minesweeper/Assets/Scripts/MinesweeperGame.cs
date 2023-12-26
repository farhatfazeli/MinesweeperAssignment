using Minesweeper.View;
using Minesweeper.Model;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    private MinesweeperEngine _minesweeperEngine;

    private void Start()
    {
        _minesweeperEngine = new();
        SetupBoardView();
    }
    private void SetupBoardView()
    {
        GameObject board = new GameObject("Board");
        board.AddComponent<BoardView>().Initialize(_minesweeperEngine.BoardModel);
    }
}