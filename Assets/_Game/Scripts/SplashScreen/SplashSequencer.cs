using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tofunaut.GridCCG.SplashScreen
{
    public class SplashSequencer : MonoBehaviour
    {
        public bool IsComplete { get; private set; }

        public List<SplashImage> splashImages;
        public UnityEvent OnComplete;

        private int _splashIndex;

        private void Awake()
        {
            foreach (var splashImage in splashImages)
                splashImage.gameObject.SetActive(false);

            _splashIndex = -1;
            Next();
        }

        public void Next()
        {
            if (IsComplete)
            {
                return;
            }
            
            if (_splashIndex >= 0)
            {
                splashImages[_splashIndex].gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(splashImages[_splashIndex].gameObject);
            }

            _splashIndex++;
            if (_splashIndex >= splashImages.Count && !IsComplete)
            {
                Complete();
                return;
            }

            splashImages[_splashIndex].gameObject.SetActive(true);
        }

        private void Complete()
        {
            IsComplete = true;
            OnComplete?.Invoke();
        }
    }
}