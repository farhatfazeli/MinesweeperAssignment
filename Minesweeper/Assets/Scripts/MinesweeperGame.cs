using Minesweeper.Model;
using Minesweeper.View;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    [SerializeField] private int _maxWidth = 100;
    [SerializeField] private int _maxHeight = 100;
    [SerializeField] private MinesweeperUI _minesweeperUI;
    private MinesweeperEngine _minesweeperEngine;
    private GameObject _board;
    private int _maxAmountOfBombs;


    private void Start()
    {
        _maxAmountOfBombs = _maxWidth * _maxHeight - 9; // no bombs allowed on first tile clicked nor on its surrounding tiles
        _minesweeperUI.SliderLimits = (4, 4, 1, _maxWidth, _maxHeight, _maxAmountOfBombs);
        Play(8, 8, 10);
    }

    private void Play(int width, int height, int amountOfBombs)
    {
        Destroy(_board);
        SetupEngine(width, height, amountOfBombs);
        SetupBoardView();
    }

    private void SetupEngine(int width, int height, int amountOfBombs)
    {
        _minesweeperEngine = new(width, height, amountOfBombs); // TODO: Insert checks on maximum width, height and amount of bombs
    }

    private void SetupBoardView()
    {
        _board = new GameObject("Board");
        _board.AddComponent<BoardView>().Initialize(_minesweeperEngine.BoardModel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int width = _minesweeperUI.SliderValues.Item1;
            int height = _minesweeperUI.SliderValues.Item2;
            int amountOfBombs = _minesweeperUI.SliderValues.Item3;
            Play(width, height, amountOfBombs);
        }
    }
}