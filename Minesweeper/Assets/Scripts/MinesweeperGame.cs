using Minesweeper.Model;
using Minesweeper.View;
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MinesweeperGame : MonoBehaviour
{
    [SerializeField] private int _maxWidth = 100;
    [SerializeField] private int _maxHeight = 100;
    [SerializeField] private MinesweeperUI _minesweeperUI;
    private MinesweeperEngine _minesweeperEngine;
    private GameObject _board;

    private int _sliderWidth { get { return _minesweeperUI.SliderValues.Item1; } }
    private int _sliderHeight { get { return _minesweeperUI.SliderValues.Item2; } }

    private int _maxAmountOfBombs { get { return _sliderWidth * _sliderHeight - 9; } }// no bombs allowed on first tile clicked nor on its surrounding tiles


    private void Start()
    {
        _minesweeperUI.OnSliderValueChanged += UpdateSliderLimits;
        _minesweeperUI.SliderValues = (8, 8, 10);

        _minesweeperUI.OnNewGameButtonPressed += StartGame;
        Play(8, 8, 10);
    }

    private void UpdateSliderLimits()
    {
        _minesweeperUI.SetSliderLimits(new(4, 4, 1, _maxWidth, _maxHeight, _maxAmountOfBombs));
    }

    private void StartGame()
    {
        Play(_sliderWidth, _sliderHeight, _maxAmountOfBombs);
    }

    private void Play(int width, int height, int amountOfBombs)
    {
        Destroy(_board);
        SetupEngine(width, height, amountOfBombs);
        _minesweeperEngine.GameOver += OnGameOver;
        SetupBoardView();
    }

    private void OnGameOver()
    {
        Debug.Log("gameover");
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