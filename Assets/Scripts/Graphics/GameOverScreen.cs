using UnityEngine;
using TMPro;

namespace Kulami.Graphics
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerText;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(string winnerText, Color winnerTextColor)
        {
            gameObject.SetActive(true);
            _winnerText.text = winnerText;
            _winnerText.color = winnerTextColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}