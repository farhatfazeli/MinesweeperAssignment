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

        public void Initialize(TileModel tileModel)
        {
            _tileModel = tileModel;
            _tileModel.CountOfAdjacentBombs.Subscribe(TileModel_OnCountOfAdjacentBombsUpdate);
            Assert.IsNotNull(_bombCountText);
        }

        private void TileModel_OnCountOfAdjacentBombsUpdate(int value)
        {
            _bombCountText.text = value.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //_tileModel.Reveal();
            _tileModel.IncrementBombCount();
            //StartCoroutine(Flip());
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

