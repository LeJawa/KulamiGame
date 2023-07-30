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

        public void SetScore(int score)
        {
            if (score / 10 == 0)
            {
                _leftDigit.gameObject.SetActive(false);
            }

            else
            {
                _leftDigit.gameObject.SetActive(true);
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
