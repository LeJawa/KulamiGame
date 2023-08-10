using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kulami.UI
{
    public class Options : MonoBehaviour
    {
        [SerializeField] private Toggle _showPossibleMovesToggle;
        [SerializeField] private Toggle _showScoresDuringGameToggle;
        [SerializeField] private Toggle _showTileOwnershipToggle;

        private void Start()
        {
            _showPossibleMovesToggle.isOn = GameOptions.Instance.ShowPossibleMoves;
            _showScoresDuringGameToggle.isOn = GameOptions.Instance.ShowScoresDuringGame;
            _showTileOwnershipToggle.isOn = GameOptions.Instance.ShowTileOwnership;
        }
    }
}
