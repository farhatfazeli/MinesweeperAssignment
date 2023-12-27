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
                StartCoroutine(ScaleBomb(bomb, _bigBombScale));
            }
        }

        public void WinAnimation()
        {
            Debug.Log("in win animation");
            foreach (TileView tileView in _allTileViews)
            {
                Debug.Log("before flip");

                tileView.TileModel_OnRevealTile(true);
            }
            foreach (Transform bomb in _allBombs)
            {
                Debug.Log("before start coroutine");

                StartCoroutine(ScaleBomb(bomb, _smallBombScale));
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

        public IEnumerator ScaleBomb(Transform bomb, Vector3 targetScale)
        {
            yield return new WaitForSeconds(1);
            Vector3 initialScale = bomb.localScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                bomb.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            bomb.localScale = targetScale;
        }

        public Vector3 _smallBombScale = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 _bigBombScale = new Vector3(10f, 10f, 10f);
        public float duration = 1f;
    }
}