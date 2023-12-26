using Minesweeper.Model;
using Minesweeper.View;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    private MinesweeperEngine _minesweeperEngine;

    private void Start()
    {
        SetupEngine();
        SetupBoardView();
    }

    private void SetupEngine()
    {
        _minesweeperEngine = new(8, 8, 10); // TODO: Insert checks on maximum width, height and amount of bombs
    }

    private void SetupBoardView()
    {
        GameObject board = new GameObject("Board");
        board.AddComponent<BoardView>().Initialize(_minesweeperEngine.BoardModel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spaaace");
            _minesweeperEngine.BoardModel.TileDictionary.GetTile(new GC(5, 5)).IncrementBombCount();
        }
    }
}