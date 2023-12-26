using Minesweeper.Model;
using UnityEngine;

namespace Minesweeper.View
{
    public class BoardView : MonoBehaviour
    {
        private BoardModel _boardModel;
        private GameObject _tilePrefab;

        private void Awake()
        {
            _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        }

        public void Initialize(BoardModel boardModel)
        {
            _boardModel = boardModel;
            PopulateBoardView();
        }

        private void PopulateBoardView()
        {
            foreach (TileModel tileModel in _boardModel.TileDictionary.ListOfTiles)
            {
                SpawnTileView(tileModel);
            }

            void SpawnTileView(TileModel tileModel)
            {
                Vector3 pos = GCHelper.ModelToView(tileModel.GC);
                GameObject tileView = Instantiate(_tilePrefab, pos, Quaternion.identity, transform);
                tileView.name = $"Tile ({tileModel.GC.X}, {tileModel.GC.Z})";
                tileView.GetComponent<TileView>().Initialize(tileModel);
            }
        }

    }
}