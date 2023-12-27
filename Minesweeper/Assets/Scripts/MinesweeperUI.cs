using Minesweeper.View;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MinesweeperUI : MonoBehaviour
{
    // minWidth, minHeight, minAmountOfBombs, maxWidth, maxHeight, maxAmountOfBombs
    //public (int, int, int, int, int, int) SliderLimits { get; set; }

    // widthValue, heightValue, amountOfBombsValue
    public (int, int, int) SliderValues { get; set; }

    private Label _bombCountText;
    private SliderInt _widthSlider;
    private Slider _heightSlider;
    private Slider _bombAmountSlider;
    private Button _newGameButton;
    public Action OnNewGameButtonPressed;
    public Action OnSliderValueChanged;

    void Start()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        VisualElement bombCountVisual = rootElement.Q<VisualElement>("BombCountVisual");
        _bombCountText = bombCountVisual.Q<Label>("BombCountText");

        BoardView boardView = FindObjectOfType<BoardView>();
        boardView.BoardModel.AmountOfBombsRemaining.Subscribe(BoardModel_OnAmountOfBombsRemainingChanged);
        BoardModel_OnAmountOfBombsRemainingChanged(boardView.BoardModel.AmountOfBombsRemaining.Value);

        VisualElement newGameMenu = rootElement.Q<VisualElement>("NewGameMenu");
        _widthSlider = newGameMenu.Q<SliderInt>("Width");
        _heightSlider = newGameMenu.Q<Slider>("Height");
        _bombAmountSlider = newGameMenu.Q<Slider>("AmountOfBombs");
        _newGameButton = newGameMenu.Q<Button>("NewGameButton");

        _widthSlider.RegisterCallback<ChangeEvent<int>>(OnWidthChanged);
        _heightSlider.RegisterCallback<ChangeEvent<float>>(OnHeightChanged);
        _bombAmountSlider.RegisterCallback<ChangeEvent<float>>(OnBombAmountChanged);
        _newGameButton.clicked += OnNewGameButtonPressed;

        SetSliderLimits(new(4, 4, 1, 100, 100, 8 * 8 - 9));

        _widthSlider.labelElement.text = "Width: " + SliderValues.Item1;
        _heightSlider.labelElement.text = "Height: " + SliderValues.Item2;
        _bombAmountSlider.labelElement.text = "#Bombs: " + SliderValues.Item3;
    }

    public void SetSliderLimits((int, int, int, int, int, int) sliderLimits)
    {
        _widthSlider.lowValue = sliderLimits.Item1;
        _heightSlider.lowValue = sliderLimits.Item2;
        _bombAmountSlider.lowValue = sliderLimits.Item3;

        _widthSlider.highValue = sliderLimits.Item4;
        _heightSlider.highValue = sliderLimits.Item5;
        _bombAmountSlider.highValue = sliderLimits.Item6;
    }

    private void BoardModel_OnAmountOfBombsRemainingChanged(int value)
    {
        _bombCountText.text = "#BOMBS: " + value.ToString();
    }

    private void OnWidthChanged(ChangeEvent<int> evt)
    {
        SliderValues = new(evt.newValue, SliderValues.Item2, SliderValues.Item3);
        _widthSlider.labelElement.text = "Width: " + SliderValues.Item1;
        OnSliderValueChanged?.Invoke();
    }

    private void OnHeightChanged(ChangeEvent<float> evt)
    {
        SliderValues = new(SliderValues.Item1, (int)evt.newValue, SliderValues.Item3);
        _heightSlider.labelElement.text = "Height: " + SliderValues.Item2;
        OnSliderValueChanged?.Invoke();

    }

    private void OnBombAmountChanged(ChangeEvent<float> evt)
    {
        SliderValues = new(SliderValues.Item1, SliderValues.Item2, (int)evt.newValue);
        _bombAmountSlider.labelElement.text = "#Bombs: " + SliderValues.Item3;
    }
}
