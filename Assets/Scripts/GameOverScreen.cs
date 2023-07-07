using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _winnerText;
    [SerializeField] private TMPro.TextMeshProUGUI _playerOnePointsText;
    [SerializeField] private TMPro.TextMeshProUGUI _playerTwoPointsText;

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
