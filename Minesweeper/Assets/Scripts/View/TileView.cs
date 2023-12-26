using Minesweeper.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minesweeper.View
{
    public class TileView : MonoBehaviour, IPointerClickHandler
    {
        private TileModel _tileModel;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            _tileModel = new TileModel(new GC(0, 0));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tileModel.Reveal();
            StartCoroutine(Flip());
        }

        private IEnumerator Flip()
        {
            int rotationSpeed = 30;
            Vector3 targetRotation = new Vector3(180, 0, 0);
            while (transform.rotation.eulerAngles.x < targetRotation.x)
            {
                Debug.Log("eulerX " + transform.rotation.eulerAngles.x + "and targetrotation.x: " + targetRotation.x);
                transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}

