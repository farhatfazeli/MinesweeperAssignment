using Minesweeper.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.View
{
    public class BoardView : MonoBehaviour
    {
        public BoardModel BoardModel { get; private set; }
        private GameObject _tilePrefab;
        private GameObject _bombPrefab;

        private readonly List<TileView> _allTileViews = new();
        private readonly List<Transform> _allBombs = new();


        private Vector3 _smallBombScale = new Vector3(0.1f, 0.1f, 0.1f);
        private Vector3 _bigBombScale = new Vector3(50f, 50f, 50f);
        private float _animationDuration = 2f;

        private void Awake()
        {
            _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            _bombPrefab = Resources.Load<GameObject>("Prefabs/Bomb");
        }

        public void Initialize(BoardModel boardModel)
        {
            BoardModel = boardModel;
            PopulateWithTiles();
        }

        public void LoseAnimation()
        {
            foreach (Transform bomb in _allBombs)
            {
                StartCoroutine(ScaleBomb(bomb, _bigBombScale, true));
            }
        }

        public void WinAnimation()
        {
            foreach (TileView tileView in _allTileViews)
            {
                tileView.TileModel_OnRevealTile(true);
            }
            foreach (Transform bomb in _allBombs)
            {
                StartCoroutine(ScaleBomb(bomb, _smallBombScale, false));
            }
        }

        private void PopulateWithTiles()
        {
            foreach (TileModel tileModel in BoardModel.TileDictionary.ListOfTiles)
            {
                SpawnTileView(tileModel);
            }

            void SpawnTileView(TileModel tileModel)
            {
                Vector3 pos = GCHelper.ModelToView(tileModel.GC);
                GameObject tile = Instantiate(_tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile ({tileModel.GC.X}, {tileModel.GC.Z})";
                tile.GetComponent<TileView>().Initialize(tileModel);
                tile.GetComponent<TileView>().SpawnBomb += TileView_OnSpawnBomb;
                _allTileViews.Add(tile.GetComponent<TileView>());
            }
        }

        private void TileView_OnSpawnBomb(TileView tileView, TileModel tileModel)
        {
            Vector3 pos = GCHelper.ModelToView(tileModel.GC);
            GameObject bomb = Instantiate(_bombPrefab, pos, Quaternion.identity, tileView.transform);
            bomb.name = $"Bomb ({tileModel.GC.X}, {tileModel.GC.Z})";
            _allBombs.Add(bomb.transform);
        }

        public IEnumerator ScaleBomb(Transform bomb, Vector3 targetScale, bool reappear)
        {
            yield return new WaitForSeconds(1);
            Vector3 initialScale = bomb.localScale;
            float elapsed = 0f;
            while (elapsed < _animationDuration)
            {
                bomb.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / _animationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (reappear)
            {
                bomb.localScale = initialScale;

            }
            else
            {
                bomb.localScale = targetScale;
            }
        }
    }
}