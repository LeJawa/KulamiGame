using UnityEngine;
using UnityEngine.UI;

namespace Kulami.Graphics
{
    public class UICounter : MonoBehaviour
    {
        [SerializeField]
        private Image _leftDigit;
        [SerializeField]
        private Image _rightDigit;

        [SerializeField]
        private Sprite[] _digits;

        Vector2 _rightDigitPosition;

        private void Start()
        {
            _rightDigitPosition = _rightDigit.rectTransform.anchoredPosition;
        }

        public void SetScore(int score)
        {
            if (score / 10 == 0)
            {
                _leftDigit.gameObject.SetActive(false);
                _rightDigit.rectTransform.anchoredPosition = Vector2.zero;
            }

            else
            {
                _leftDigit.gameObject.SetActive(true);
                _rightDigit.rectTransform.anchoredPosition = _rightDigitPosition;
            }

            _leftDigit.sprite = GetSprite(score / 10);
            _rightDigit.sprite = GetSprite(score % 10);
        }

        private Sprite GetSprite(int digit)
        {
            return _digits[digit];
        }
    }
}
