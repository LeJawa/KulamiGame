using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private float _screenWidth = 1920f;
        private float _screenHeight = 1080f;

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
            _rectTransform.anchoredPosition = -Vector2.left * _screenWidth;
        }

        public void GoToOptions()
        {
            _rectTransform.anchoredPosition = -Vector2.right * _screenWidth;
        }

        public void Reset()
        {
            _rectTransform.anchoredPosition = Vector2.zero;
        }



    }
}
