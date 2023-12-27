using Minesweeper.Model;
using Minesweeper.View;
using UnityEngine;
using UnityEngine.UIElements;

public class MinesweeperUI : MonoBehaviour
{
    // minWidth, minHeight, minAmountOfBombs, maxWidth, maxHeight, maxAmountOfBombs
    public (int, int, int, int, int, int) SliderLimits { get; set; }

    // widthValue, heightValue, amountOfBombsValue
    public (int, int, int) SliderValues { get; set; }

    private Label _bombCountText;
    private Slider _widthSlider;
    private Slider _heightSlider;
    private Slider _bombAmountSlider;
    void Start()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        VisualElement bombCountVisual = rootElement.Q<VisualElement>("BombCountVisual");
        _bombCountText = bombCountVisual.Q<Label>("BombCountText");

        BoardView boardView = FindObjectOfType<BoardView>();
        boardView.BoardModel.AmountOfBombsRemaining.Subscribe(BoardModel_OnAmountOfBombsRemainingChanged);
        BoardModel_OnAmountOfBombsRemainingChanged(boardView.BoardModel.AmountOfBombsRemaining.Value);

        VisualElement newGameMenu = rootElement.Q<VisualElement>("NewGameMenu");
        _widthSlider = newGameMenu.Q<Slider>("Width");
        _heightSlider = newGameMenu.Q<Slider>("height");
        _bombAmountSlider = newGameMenu.Q<Slider>("AmountOfBombs");

        _widthSlider.lowValue = SliderLimits.Item1;
        _heightSlider.lowValue = SliderLimits.Item2;
        _bombAmountSlider.lowValue = SliderLimits.Item3;

        _widthSlider.highValue = SliderLimits.Item4;
        _heightSlider.highValue = SliderLimits.Item5;
        _bombAmountSlider.highValue = SliderLimits.Item6;
    }

    private void BoardModel_OnAmountOfBombsRemainingChanged(int value)
    {
        Debug.Log("CALLLELED");
        _bombCountText.text = "#BOMBS: " + value.ToString();
    }

    void Update()
    {
        SliderValues = new((int)_widthSlider.value, (int)_heightSlider.value, (int)_bombAmountSlider.value);
    }
}
