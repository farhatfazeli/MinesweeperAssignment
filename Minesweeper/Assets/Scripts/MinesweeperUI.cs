using Minesweeper.Model;
using Minesweeper.View;
using UnityEngine;
using UnityEngine.UIElements;

public class MinesweeperUI : MonoBehaviour
{
    private Label _bombCountText;
    void Start()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        VisualElement bombCountVisual = rootElement.Q<VisualElement>("BombCountVisual");
        _bombCountText = bombCountVisual.Q<Label>("BombCountText");

        BoardView boardView = FindObjectOfType<BoardView>();
        boardView.BoardModel.AmountOfBombsRemaining.Subscribe(BoardModel_OnAmountOfBombsRemainingChanged);
        BoardModel_OnAmountOfBombsRemainingChanged(boardView.BoardModel.AmountOfBombsRemaining.Value);
    }

    private void BoardModel_OnAmountOfBombsRemainingChanged(int value)
    {
        Debug.Log("CALLLELED");
        _bombCountText.text = "#BOMBS: " + value.ToString();
    }

    void Update()
    {

    }
}
