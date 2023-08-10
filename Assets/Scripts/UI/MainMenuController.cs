using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private float _screenWidth = 1920f;
        private float _screenHeight = 1080f;

        [SerializeField] private float _transitionTime = 1f;
        [SerializeField] private Ease _transitionEase = Ease.Flash;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void GoToStartMenu()
        {
            Reset();
        }

        public void GoToCredits()
        {
            _rectTransform.DOAnchorPos3DX(_screenWidth, _transitionTime).SetEase(_transitionEase);
        }

        public void GoToOptions()
        {
            _rectTransform.DOAnchorPos3DX(-_screenWidth, _transitionTime).SetEase(_transitionEase);
        }

        public void Reset()
        {
            _rectTransform.DOAnchorPos3DX(0, _transitionTime).SetEase(_transitionEase);
        }



    }
}
