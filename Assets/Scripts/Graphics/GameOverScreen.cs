using UnityEngine;
using TMPro;

namespace Kulami.Graphics
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerText;
        [SerializeField] private TextMeshProUGUI _playerOnePointsText;
        [SerializeField] private TextMeshProUGUI _playerTwoPointsText;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(string winnerText, int playerOnePoints, int playerTwoPoints)
        {
            gameObject.SetActive(true);
            _winnerText.text = winnerText;
            _playerOnePointsText.text = $"{playerOnePoints}";
            _playerTwoPointsText.text = $"{playerTwoPoints}";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}