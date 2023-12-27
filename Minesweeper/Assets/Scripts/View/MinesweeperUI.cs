using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Minesweeper.View
{

    public class MinesweeperUI : MonoBehaviour
    {
        public (int width, int height, int bombs) SliderValues { get; set; }

        private Label _bombCountLabel;
        private SliderInt _widthSlider;
        private Slider _heightSlider;
        private Slider _bombAmountSlider;
        private Button _newGameButton;

        public Action NewGameButtonClicked;
        public Action OnSliderValueChanged;

        private const string _bombCountVisualName = "BombCountVisual";
        private const string _bombCountLabelName = "BombCountLabel";
        private const string _newGameMenuName = "NewGameMenu";
        private const string _widthSliderName = "Width";
        private const string _heightSliderName = "Height";
        private const string _bombAmountSliderName = "AmountOfBombs";
        private const string _newGameButtonName = "NewGameButton";

        void Awake()
        {
            SetupUIElements();
            SetupEventHandlers();
            //SetSliderLimits(new(4, 4, 1, 100, 100, 8 * 8 - 9));
            //UpdateSliderLabels();
        }

        private void SetupUIElements()
        {
            VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

            _bombCountLabel = rootElement.Q<VisualElement>(_bombCountVisualName).Q<Label>(_bombCountLabelName);

            VisualElement newGameMenu = rootElement.Q<VisualElement>(_newGameMenuName);
            _widthSlider = newGameMenu.Q<SliderInt>(_widthSliderName);
            _heightSlider = newGameMenu.Q<Slider>(_heightSliderName);
            _bombAmountSlider = newGameMenu.Q<Slider>(_bombAmountSliderName);
            _newGameButton = newGameMenu.Q<Button>(_newGameButtonName);
        }

        private void SetupEventHandlers()
        {
            _widthSlider.RegisterCallback<ChangeEvent<int>>(OnWidthChanged);
            _heightSlider.RegisterCallback<ChangeEvent<float>>(OnHeightChanged);
            _bombAmountSlider.RegisterCallback<ChangeEvent<float>>(OnBombAmountChanged);
            _newGameButton.clicked += OnNewGameButtonClicked;
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

        private void UpdateSliderLabels()
        {
            _widthSlider.labelElement.text = "Width: " + SliderValues.width;
            _heightSlider.labelElement.text = "Height: " + SliderValues.height;
            _bombAmountSlider.labelElement.text = "#Bombs: " + SliderValues.bombs;
        }

        public void UpdateSliderValues(int width, int height, int bombs)
        {
            SliderValues = (width, height, bombs);
            _widthSlider.value = SliderValues.width;
            _heightSlider.value = SliderValues.height;
            _bombAmountSlider.value = SliderValues.bombs;
            UpdateSliderLabels();
        }

        public void SetupBoardLink()
        {
            BoardView boardView = FindObjectOfType<BoardView>();
            boardView.BoardModel.AmountOfBombsRemaining.Subscribe(BoardModel_OnAmountOfBombsRemainingChanged);
            BoardModel_OnAmountOfBombsRemainingChanged(boardView.BoardModel.AmountOfBombsRemaining.Value);
        }

        private void BoardModel_OnAmountOfBombsRemainingChanged(int value)
        {
            _bombCountLabel.text = "#BOMBS: " + value.ToString();
        }

        private void OnWidthChanged(ChangeEvent<int> evt)
        {
            UpdateSliderValues(evt.newValue, SliderValues.height, SliderValues.bombs);
            UpdateSliderLabels();
            OnSliderValueChanged?.Invoke();
        }

        private void OnHeightChanged(ChangeEvent<float> evt)
        {
            UpdateSliderValues(SliderValues.width, (int)evt.newValue, SliderValues.bombs);
            UpdateSliderLabels();
            OnSliderValueChanged?.Invoke();
        }

        private void OnBombAmountChanged(ChangeEvent<float> evt)
        {
            UpdateSliderValues(SliderValues.width, SliderValues.height, (int)evt.newValue);
            UpdateSliderLabels();
        }

        private void OnNewGameButtonClicked()
        {
            NewGameButtonClicked?.Invoke();
        }
    }
}
