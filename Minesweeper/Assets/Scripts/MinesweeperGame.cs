using Minesweeper.Model;
using Minesweeper.View;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    #region UI data
    [SerializeField] private int _minWidth = 4;
    [SerializeField] private int _minHeight = 4;
    [SerializeField] private int _minAmountOfBombs = 1;
    [SerializeField] private int _maxWidth = 100;
    [SerializeField] private int _maxHeight = 100;
    [SerializeField] private int _startingWidth = 5;
    [SerializeField] private int _startingHeight = 5;
    [SerializeField] private int _startingAmountOfBombs = 4;
    [SerializeField] private MinesweeperUI _minesweeperUI;
    [SerializeField] private Camera _camera;
    [SerializeField] private int _padding = 5;

    private int SliderWidth => _minesweeperUI.SliderValues.width;
    private int SliderHeight => _minesweeperUI.SliderValues.height;
    private int SliderBombs => _minesweeperUI.SliderValues.bombs;
    private int MaxAmountOfBombs => (SliderWidth * SliderHeight) - 9; // no bombs allowed on first tile clicked nor on its surrounding tiles
    #endregion

    private MinesweeperEngine _minesweeperEngine;
    private BoardView _boardView;

    private void Start()
    {
        InitializeUI();
        StartGame(_startingWidth, _startingHeight, _startingAmountOfBombs);
    }

    private void InitializeUI()
    {
        _minesweeperUI.UpdateSliderValues(_startingWidth, _startingHeight, _startingAmountOfBombs);
        UpdateSliderLimits();
        _minesweeperUI.OnSliderValueChanged += UpdateSliderLimits;
        _minesweeperUI.NewGameButtonClicked += StartNewGame;
    }

    private void UpdateSliderLimits()
    {
        _minesweeperUI.SetSliderLimits(new(_minWidth, _minHeight, _minAmountOfBombs, _maxWidth, _maxHeight, MaxAmountOfBombs));
    }

    private void StartNewGame()
    {
        Destroy(_boardView.gameObject);
        _minesweeperEngine.GameOver -= OnGameOver;
        _minesweeperEngine.GameWon -= OnGameWon;
        StartGame(SliderWidth, SliderHeight, SliderBombs);
    }

    private void StartGame(int width, int height, int amountOfBombs)
    {
        SetupEngine(width, height, amountOfBombs);
        _minesweeperEngine.GameOver += OnGameOver;
        _minesweeperEngine.GameWon += OnGameWon;
        SetupBoardView();
        _minesweeperUI.SetupBoardLink();
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float targetOrthographicSize = Mathf.Max(SliderWidth, SliderHeight) * 0.5f + _padding;

        _camera.orthographicSize = targetOrthographicSize;

        float aspectRatio = Screen.width / (float)Screen.height;

        if (SliderWidth > SliderHeight)
        {
            _camera.transform.position = new Vector3(SliderWidth * 0.5f, 10f, SliderHeight * 0.5f);
        }
        else
        {
            _camera.transform.position = new Vector3(SliderWidth * 0.5f * aspectRatio, 10f, SliderHeight * 0.5f);
        }
    }

    private void OnGameOver()
    {
        Debug.Log("gameover");
        _boardView.LoseAnimation();
    }

    private void OnGameWon()
    {
        Debug.Log("gamewon");
        _boardView.WinAnimation();
    }

    private void SetupEngine(int width, int height, int amountOfBombs)
    {
        _minesweeperEngine = new(width, height, amountOfBombs);
    }

    private void SetupBoardView()
    {
        GameObject board = new GameObject("Board");
        _boardView = board.AddComponent<BoardView>();
        _boardView.Initialize(_minesweeperEngine.BoardModel);
    }
}