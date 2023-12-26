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
            foreach (GC gc in _boardModel.TileDictionary.Keys)
            {
                SpawnTile(gc);
            }

            void SpawnTile(GC coordinate)
            {
                Vector3 pos = GCHelper.ModelToView(coordinate);
                GameObject tile = Instantiate(_tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile ({coordinate.X}, {coordinate.Z})";
                //tile.GetComponent<TileView>().Initialize(_boardModel.TileDictionary.GetTile(coordinate));
            }
        }

    }
}