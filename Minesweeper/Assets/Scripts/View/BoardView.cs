using Minesweeper.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Minesweeper.View
{
    public class BoardView : MonoBehaviour
    {
        private BoardModel _boardModel;
        private GameObject _tilePrefab;
        private GameObject _bombPrefab;


        private void Awake()
        {
            _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            _bombPrefab = Resources.Load<GameObject>("Prefabs/Bomb");
        }

        public void Initialize(BoardModel boardModel)
        {
            _boardModel = boardModel;
            PopulateWithTiles();
        }

        private void PopulateWithTiles()
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
                tileView.GetComponent<TileView>().SpawnBomb += TileView_OnSpawnBomb;
            }
        }

        private void TileView_OnSpawnBomb(TileView tileView, TileModel tileModel)
        {
            Vector3 pos = GCHelper.ModelToView(tileModel.GC);
            GameObject bombView = Instantiate(_bombPrefab, pos, Quaternion.identity, tileView.transform);
            bombView.name = $"Bomb ({tileModel.GC.X}, {tileModel.GC.Z})";
        }
    }
}