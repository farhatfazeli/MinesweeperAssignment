using Minesweeper.Model;
using UnityEngine;

namespace Minesweeper.View
{
    public class BoardView : MonoBehaviour
    {
        private BoardModel _boardModel;

        public void Initialize(BoardModel boardModel)
        {
            _boardModel = boardModel;
        }
    }
}