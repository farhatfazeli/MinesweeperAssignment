using Minesweeper.Model;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Minesweeper.View
{
    public class TileView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int _tileFlipSpeed = 60;
        [SerializeField] private TextMeshPro _bombCountText;
        private TileModel _tileModel;

        public Action<TileView, TileModel> SpawnBomb { get; set; }

        public void Initialize(TileModel tileModel)
        {
            _tileModel = tileModel;
            _tileModel.CountOfAdjacentBombs.Subscribe(TileModel_OnCountOfAdjacentBombsUpdate);
            _tileModel.IsRevealed.Subscribe(TileModel_OnRevealTile);
            _tileModel.ShowCountOfAdjacentBombs.Subscribe(TileModel_OnShowCountOfAdjacentBombs);
            _tileModel.HasBomb.Subscribe(TileModel_OnHasBomb);
            Assert.IsNotNull(_bombCountText);
        }

        private void TileModel_OnHasBomb(bool boolean)
        {
            if (boolean)
            {
                SpawnBomb.Invoke(this, _tileModel);
            }
        }

        private void TileModel_OnCountOfAdjacentBombsUpdate(int value)
        {
            _bombCountText.text = value.ToString();
        }

        private void TileModel_OnShowCountOfAdjacentBombs(bool boolean)
        {
            _bombCountText.gameObject.SetActive(boolean);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("I'm Tile " + gameObject.name + " and I have this bombs around me " + _tileModel.CountOfAdjacentBombs.Value);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _tileModel.OnRevealClick();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _tileModel.OnFlagClick();
            }
        }

        private void TileModel_OnRevealTile(bool value)
        {
            StartCoroutine(Flip());
            Debug.Log("Test tile reveal");
        }

        private IEnumerator Flip()
        {
            Vector3 targetRotation = new Vector3(180, 0, 0);
            while (transform.rotation.eulerAngles.x < targetRotation.x)
            {
                transform.rotation *= Quaternion.Euler(_tileFlipSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}

